using System.Net;
using System.Net.Sockets;

namespace Connections
{
    #region Additional small classes
    public enum ConnectionState { NotConnected, Waiting, Connecting, Connected, Disconnecting }
    public class ConnectionEventEventArgs : EventArgs
    {
        public ConnectionState SenderState { get; private set; }
        public ConnectionState ReceiverState { get; private set; }
        public ConnectionEventEventArgs(ConnectionState senderState, ConnectionState receiverState)
        {
            SenderState = senderState;
            ReceiverState = receiverState;
        }
        public override string ToString()
        {
            return SenderState == ReceiverState
                ? "Connection Status: " + SenderState
                : $"Connection Status: Sender {SenderState} - Receiver {ReceiverState}";
        }
    }
    #endregion

    public class P2PTCPVideoConnection
    {
        private TcpClient sendClient;
        private TcpClient receiveClient;
        private TcpListener sendListener;
        private TcpListener receiveListener;
        private Task listeningForSendTask;
        private Task listeningForReceiveTask;
        private Task receivingTask;
        private Task sendingTask;
        private IDispatcher dispatcher;

        public bool ReceiveVideo { get; set; }

        private NetworkStream sendStream;
        private NetworkStream receiveStream;

        private CancellationTokenSource abortSource = new CancellationTokenSource();
        private CancellationTokenSource receivingSource = new CancellationTokenSource();
        private CancellationTokenSource sendingSource = new CancellationTokenSource();

        public Queue<Message> sendQ = new Queue<Message>();

        private MemoryStream vid = new MemoryStream();
        public MemoryStream Video
        {
            get
            {
                vid.Position = 0;
                return vid;
            }
            set
            {
                MemoryStream old = vid;
                vid = value;
                old.Dispose();
            }
        }

        private ConnectionState senderState = ConnectionState.NotConnected;
        public ConnectionState SenderConnectionState
        {
            get => senderState;
            private set
            {
                senderState = value;
                OnConnectionStateChanged();
            }
        }
        private ConnectionState receiverState = ConnectionState.NotConnected;
        public ConnectionState ReceiverConnectionState
        {
            get => receiverState;
            private set
            {
                receiverState = value;
                OnConnectionStateChanged();
            }
        }



        #region Events
        public event EventHandler<MemoryStream> ImageReceived;
        public event EventHandler<string> StringReceived;
        public event EventHandler<ConnectionEventEventArgs> ConnectionStateChanged;
        public event EventHandler<Exception> ExceptionThrown;

        public void DispatchEvent(Action a)
        {
            if (dispatcher != null && dispatcher.IsDispatchRequired)
                dispatcher.Dispatch(a);
            else
                a();
        }

        private void ReceiveImage(MemoryStream image)
        {
            if (ImageReceived != null)
                DispatchEvent(() => ImageReceived(this, image));
        }

        private void ReceiveString(string message)
        {
            if (StringReceived != null)
                DispatchEvent(() => StringReceived(this, message));
        }

        private void OnConnectionStateChanged()
        {
            if (ConnectionStateChanged != null)
                DispatchEvent(() => ConnectionStateChanged(this, new ConnectionEventEventArgs(senderState, receiverState)));
        }

        private void ThrowException(Exception ex)
        {
            if (ExceptionThrown != null)
                DispatchEvent(() => ExceptionThrown(this, ex));
        }
        #endregion

        public P2PTCPVideoConnection(bool receiveVideo = false)
        {
            ReceiveVideo = receiveVideo;
            dispatcher = Dispatcher.GetForCurrentThread();
        }

        public bool Connect(ConnectionSettings server, int timeoutMs = 10000)
        {
            try
            {
                //Sender
                SenderConnectionState = ConnectionState.Connecting;
                sendClient = new TcpClient();
                if (!sendClient.ConnectAsync(server.Address, server.ReceivePort).Wait(timeoutMs))
                    throw new TimeoutException("Sender client connection timeout");

                SenderConnectionState = ConnectionState.Connected;
                sendStream = sendClient.GetStream();

                //Receiver
                ReceiverConnectionState = ConnectionState.Connecting;
                receiveClient = new TcpClient();
                if (!receiveClient.ConnectAsync(server.Address, server.SendPort).Wait(timeoutMs))
                    throw new TimeoutException("Receiver client connection timeout");

                ReceiverConnectionState = ConnectionState.Connected;
                receiveStream = receiveClient.GetStream();
            }
            catch (Exception ex)
            {
                Disconnect();
                ThrowException(ex);
                return false;
            }

            StartReceiving();
            if (ReceiveVideo)
                RequestVideo();
            StartSending();
            return true;
        }

        #region Server
        public bool WaitForConnection(ConnectionSettings settings)
        {
            try
            {
                receiveListener = new TcpListener(settings.Address, settings.ReceivePort);
                receiveListener.Start();

                listeningForReceiveTask = new Task(ListenerForReceiveThreadLoop);
                listeningForReceiveTask.Start();

                ReceiverConnectionState = ConnectionState.Waiting;

                sendListener = new TcpListener(settings.Address, settings.SendPort);
                sendListener.Start();

                listeningForSendTask = new Task(ListenerForSendThreadLoop);
                listeningForSendTask.Start();

                SenderConnectionState = ConnectionState.Waiting;

                return true;
            }
            catch (Exception ex)
            {
                Abort();
                ThrowException(ex);
                return false;
            }
        }

        private async void ListenerForReceiveThreadLoop()
        {
            if (receiveListener == null)
                return;
            try
            {
                receiveClient = await receiveListener.AcceptTcpClientAsync(abortSource.Token);
                if (abortSource.IsCancellationRequested) return;

                receiveStream = receiveClient.GetStream();
                ReceiverConnectionState = ConnectionState.Connected;

                StartReceiving();
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                ReceiverConnectionState = ConnectionState.NotConnected;
            }
        }
        private async void ListenerForSendThreadLoop()
        {
            if (sendListener == null)
                return;
            try
            {
                sendClient = await sendListener.AcceptTcpClientAsync(abortSource.Token);
                if (abortSource.IsCancellationRequested) return;

                sendStream = sendClient.GetStream();
                SenderConnectionState = ConnectionState.Connected;

                SendingLoop();
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                SenderConnectionState = ConnectionState.NotConnected;
            }
        }

        private void Abort()
        {
            abortSource.Cancel();
        }
        #endregion

        #region Sending
        private void StartSending()
        {
            sendingTask = new Task(SendingLoop);
            sendingTask.Start();
        }

        public void Send(string message) =>
            sendQ.Enqueue(new Message(message));

        public void SendVideo() =>
            sendQ.Enqueue(new Message(Video));

        public void RequestVideo() =>
            sendQ.Enqueue(Message.NextFrameMessage);

        private async void SendingLoop()
        {
            while (!sendingSource.IsCancellationRequested)
            {
                while (sendQ.Count == 0 && !sendingSource.IsCancellationRequested)
                    await Task.Delay(10);

                if (sendingSource.IsCancellationRequested)
                    return;

                Message m = sendQ.Dequeue();
                try
                {
                    if (sendStream == null)
                        throw new Exception("Not Connected");
                    Message.Send(m, sendStream, sendingSource.Token);
                }
                catch (Exception e) { ThrowException(e); }
            }
        }
        #endregion

        #region Receiving 
        private void StartReceiving()
        {
            receivingTask = new Task(Receive);
            receivingTask.Start();
        }

        private async void Receive()
        {
            while (!receivingSource.IsCancellationRequested)
            {
                try
                {
                    if (receiveStream == null) return;
                    Message ms = await Message.Receive(receiveStream, receivingSource.Token);
                    object cont = ms.GetContents();
                    switch (ms.Type)
                    {
                        case Message.MessageType.String:
                            if (cont == null) throw new Exception("Invalid Message");
                            ReceiveString((string)cont);
                            break;
                        case Message.MessageType.MemoryStream:
                            if (cont == null) throw new Exception("Invalid Message");
                            ReceiveImage((MemoryStream)cont);
                            if (ReceiveVideo) sendQ.Enqueue(Message.NextFrameMessage);
                            break;
                        case Message.MessageType.EndConnection:
                            Disconnect();
                            break;
                        case Message.MessageType.SendNextFrame:
                            SendVideo();
                            break;
                        default:
                            throw new Exception("Unexpected Message Type");
                    }
                }
                catch (Exception e)
                {
                    ThrowException(e);
                };
            }
        }
        #endregion

        public void Disconnect()
        {
            SenderConnectionState = ConnectionState.Disconnecting;
            ReceiverConnectionState = ConnectionState.Disconnecting;
            sendQ.Clear();

            abortSource.Cancel();
            receivingSource.Cancel();
            sendingSource.Cancel();

            DisconnectClient(ref sendClient);
            DisconnectListener(ref sendListener);
            SenderConnectionState = ConnectionState.NotConnected;

            DisconnectClient(ref receiveClient);
            DisconnectListener(ref receiveListener);
            ReceiverConnectionState = ConnectionState.NotConnected;

            receiveStream = null;
            sendStream = null;
        }
        private void DisconnectClient(ref TcpClient client)
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }
        }
        private void DisconnectListener(ref TcpListener listener)
        {
            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }
        }
    }
}