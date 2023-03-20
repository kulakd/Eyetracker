using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace JacekMatulewski.Communication.Ntpp
{
    //Network Text Passing Protocol
    public class NtppConnectionData //identyczne jak dane połączenia w Tcp
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
                hostname = (hostEntry != null) ? hostEntry.HostName : ipAddress.ToString();
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

    //Network Text Passing Protocol
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
                onStateChanged(oldState, this.state);
            }
        }

        public string LastErrorMessage { get; private set; }

        private NtppConnectionData connectionData;
        private TcpClient tcpClient;

        #region Client
        public bool Connect(NtppConnectionData connectionData, int timeoutMs = 10000) //odpowiednik StartClient
        {
            try
            {
                State = ServerState.Connecting;
                this.connectionData = connectionData;
                //tcpClient = new TcpClient(connectionData.Hostname, connectionData.Port); //tu nie można ustawić timeoutu
                tcpClient = new TcpClient();
                if (!tcpClient.ConnectAsync(connectionData.Hostname, connectionData.Port).Wait(timeoutMs))
                {
                    State = ServerState.NotConnected;
                    throw new Exception("Connection to server time out");
                }
                useStreamsFromTcpClient(tcpClient);
                State = ServerState.Connected;
                return true;
            }
            catch (Exception exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        public static NtppClient ConnectAsClient(NtppConnectionData connectionData)
        {
            NtppClient ntppClient = new NtppClient();
            if (ntppClient.Connect(connectionData)) return ntppClient;
            else throw new Exception(ntppClient.LastErrorMessage);
        }
        #endregion

        #region Server
        private TcpListener tcpListener = null;
        private Thread listeningLoopThread = null;

        //nie ma funkcji StopServer - za to należy wywołać Disconnect
        public bool WaitForConnection(NtppConnectionData connectionData) //odpowiednik StartServer
        {
            try
            {
                this.connectionData = connectionData;
                tcpListener = new TcpListener(connectionData.IpAddress, connectionData.Port); //przyjmuje ze wszystkich (obsłuży wiele klientów)
                tcpListener.Start();
                listeningLoopThread = new Thread(waitingForConnection); listeningLoopThread.Start();
                State = ServerState.WaitingForConnection;
                return true;
            }
            catch (Exception exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        private void waitingForConnection() //czeka na połączenia
        {
            try
            {
                tcpClient = tcpListener.AcceptTcpClient();
                useStreamsFromTcpClient(tcpClient);
                State = ServerState.Connected;
            }
            catch (System.Threading.ThreadAbortException)
            {
                LastErrorMessage = "Pętla nasłuchiwacza została zatrzymana";
            }
            catch (Exception exc)
            {
                LastErrorMessage = exc.Message;
            }
        }

        public static NtppClient ConnectAsServer(NtppConnectionData connectionData)
        {
            NtppClient ntppClient = new NtppClient();
            if (ntppClient.WaitForConnection(connectionData)) return ntppClient;
            else throw new Exception(ntppClient.LastErrorMessage);
        }
        #endregion

        public bool Disconnect()
        {
            try
            {
                stopReceivingLoopThread = true;

                if (tcpClient != null)
                {
                    if (State == ServerState.Connected)
                        SendText(NtppHelper.ByeCommand);
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
            catch (Exception exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        private void useStreamsFromTcpClient(TcpClient tcpClient)
        {
            stopReceivingLoopThread = false;
            NetworkStream tcpClientStream = tcpClient.GetStream();
            //tcpClientStream.ReadTimeout = 10000;
            //tcpClientStream.WriteTimeout = 10000;
            sw = new StreamWriter(tcpClientStream); sw.AutoFlush = true;
            sr = new StreamReader(tcpClientStream);
            startReceivingText();
        }

        #region Sender
        private StreamWriter sw;

        public bool SendText(string text)
        {
            string _text = NtppHelper.RemoveNewLines(text);
            try
            {
                if (State != ServerState.Connected) throw new Exception("Connection is requires to send the text");
                sw.Write(_text + Environment.NewLine); //dodaję znak końca linii, bo w serwerze używam ReadLine (możnaby wysyłać kilka tekstów i dopiero kończyć znakiem końca linii)
                //sw.Flush(); //niepotrzebne, bo AutoFlush                    
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
        private StreamReader sr;
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
                if (stopReceivingLoopThread) break;
                try
                {
                    string _text = sr.ReadLine(); //ze względu na ReadLine wiadomości muszą kończyć się znakiem końca linii; przez \n nie może być w wiadomości - trzeba wymieniać na inny znak
                    string text = NtppHelper.RecoverNewLines(_text);
                    if (text == null || text == "") return;
                    switch (text) //komendy
                    {
                        case NtppHelper.ByeCommand:
                            Disconnect();
                            break;
                        default:
                            LastReceivedText = text;
                            onTextReceived(text); //uwaga, to będzie w osobnym wątku                    
                            break;
                    }
                }
                catch (Exception exc)
                {
                    //może być błąd przy rozłączaniu
                    LastErrorMessage = exc.Message;
                }
                Thread.Sleep(10);
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
        private const string newlineReplacement = "&@nl$;";

        public static string RemoveNewLines(string text)
        {
            return text.Replace(Environment.NewLine, newlineReplacement);
        }

        public static string RecoverNewLines(string text)
        {
            return text.Replace(newlineReplacement, Environment.NewLine);
        }

        public const string ByeCommand = "&@bye$;";
    }
}
