using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Umk.Ntpp
{
    public class NtppConnectionData
    {
        private IPAddress ipAddress;
        private string hostname;
        public int Port = 11236; //domyślny port NTPP

        public IPAddress IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                //hostname = (hostEntry != null) ? hostEntry.HostName : ipAddress.ToString();
                hostname = hostEntry.HostName;                
            }
        }

        public string Hostname
        {
            get
            {
                return hostname;
            }
            set
            {
                hostname = value;
                ipAddress = IPAddress.Parse(hostname);
            }
        }
    }

    public class NtppClient
    {
        public enum ServerState { NotConnected, WaitingForConnection, Connecting, Connected }

        private ServerState state = ServerState.NotConnected;

        public ServerState State
        {
            get
            {
                return state;
            }
            private set
            {
                ServerState oldState = this.state;
                this.state = value;
                onStateChanged(oldState, state);
            }
        }

        public string LastErrorMessage { get; private set; }

        private NtppConnectionData connectionData;
        private TcpClient tcpClient;

        private StreamWriter sw = null;
        private StreamReader sr = null;

        private void useStreamFromTcpClient(TcpClient tcpClient) //static?
        {
            stopReceivingLoopThread = false;

            NetworkStream tcpClientStream = tcpClient.GetStream();
            //tcpClient.ReceiveTimeout
            sw = new StreamWriter(tcpClientStream); sw.AutoFlush = true;
            sr = new StreamReader(tcpClientStream);

            startReceivingText();
        }

        #region Client
        public bool Connect(NtppConnectionData connectionData, int timeoutMs = 10000)
        {
            try
            {
                State = ServerState.Connecting;
                this.connectionData = connectionData;
                tcpClient = new TcpClient();
                if (!tcpClient.ConnectAsync(connectionData.Hostname, connectionData.Port).Wait(timeoutMs))
                {
                    State = ServerState.NotConnected;
                    throw new Exception("Connection to server time out");
                }
                State = ServerState.Connected;
                useStreamFromTcpClient(tcpClient);
                return true;
            }
            catch (Exception exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        //static ConnectAsClient
        #endregion

        #region Server
        private TcpListener tcpListener = null;
        private Thread listeningLoopThread = null;

        public bool WaitForConnection(NtppConnectionData connectionData)
        {
            try
            {
                this.connectionData = connectionData;
                tcpListener = new TcpListener(connectionData.IpAddress, connectionData.Port);
                tcpListener.Start();
                listeningLoopThread = new Thread(waitingForConnectionThreadProcedure);
                listeningLoopThread.Start();
                State = ServerState.WaitingForConnection;
                return true;
            }
            catch (Exception exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        public void waitingForConnectionThreadProcedure()
        {
            try
            {
                tcpClient = tcpListener.AcceptTcpClient();
                useStreamFromTcpClient(tcpClient);
                State = ServerState.Connected;
            }
            catch (System.Threading.ThreadAbortException)
            {
                LastErrorMessage = "Nasłuchiwanie zostało przerwane";
            }
            catch (Exception exc)
            {
                LastErrorMessage = exc.Message;
            }
        }

        //static ConnectAsServer
        #endregion

        public bool Disconnect()
        {
            try
            {
                stopReceivingLoopThread = true;

                if (tcpClient != null)
                {
                    if (State == ServerState.Connected)
                        SendText(NtppHelper.ByeByeCommand);
                    if (sw != null) { sw.Close(); sw = null; }
                    if (sr != null) { sr.Close(); sr = null; }
                    tcpClient.Close(); tcpClient = null;
                }

                if (tcpListener != null)
                {
                    tcpListener.Stop(); tcpListener = null;
                }

                State = ServerState.NotConnected;
                return true;
            }
            catch(Exception exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }


        #region Sender
        public bool SendText(string text)
        {
            string _text = NtppHelper.RemoveNewLines(text);
            try
            {
                if (State != ServerState.Connected) throw new Exception("Connection is required to send the text");
                sw.Write(_text + Environment.NewLine);
                //sw.Flush();
                return true;
            }
            catch (Exception exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }
        #endregion

        #region Receiver
        private Thread textReceivingThread = null;
        public string LastReceivedText { get; private set; }
        private bool stopReceivingLoopThread = false;

        private void startReceivingText()
        {
            textReceivingThread = new Thread(textReceivingLoop);
            textReceivingThread.Start();
        }

        private void textReceivingLoop()
        {
            while (true)
            {
                if (stopReceivingLoopThread) break; //dziwne!!!!
                try
                {
                    string _text = sr.ReadLine();
                    string text = NtppHelper.RecoverNewLines(_text);
                    if (string.IsNullOrEmpty(text)) return;
                    switch (text)
                    {
                        case NtppHelper.ByeByeCommand:
                            Disconnect();
                            break;
                        default:
                            LastReceivedText = text;
                            onTextReceived(text);
                            break;
                    }
                }
                catch (Exception exc)
                {
                    LastErrorMessage = exc.Message;
                }
                finally
                {
                    Thread.Sleep(10);
                }
            }
        }
        #endregion

        #region Events
        public class TextReceivedEventArgs : EventArgs
        {
            public NtppConnectionData ReceivedFrom;
            public string ReceivedText;
        }

        public class StateChangedEventArgs : EventArgs
        {
            public ServerState OldState, NewState;
        }

        public event EventHandler<TextReceivedEventArgs> TextReceived;
        public event EventHandler<StateChangedEventArgs> StateChanged;

        protected void onTextReceived(string text)
        {
            if (TextReceived != null) TextReceived(this, new TextReceivedEventArgs() { ReceivedFrom = connectionData, ReceivedText = text });
        }

        protected void onStateChanged(ServerState oldState, ServerState newState)
        {
            if (StateChanged != null) StateChanged(this, new StateChangedEventArgs() { OldState = oldState, NewState = newState });
        }
        #endregion
    }

    static class NtppHelper
    {
        private const string newLineReplacement = "&@nl$;";

        public static string RemoveNewLines(string text)
        {
            return text.Replace(Environment.NewLine, newLineReplacement);
        }

        public static string RecoverNewLines(string text)
        {
            return text.Replace(newLineReplacement, Environment.NewLine);
        }

        public const string ByeByeCommand = "&@bye$;";
    }
}
