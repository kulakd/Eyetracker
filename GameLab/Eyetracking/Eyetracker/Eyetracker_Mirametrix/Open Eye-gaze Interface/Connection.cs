using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameLab.Eyetracking.OpenEyeGazeInterface
{
    // Object attendant connectrion from this host to server ET
    public static class Connection
    {
        public static TcpClient Client = null;
        private static NetworkStream _networkStream = null;
        public static bool IsConnected()
        {
            if (Client == null) return false;
            return Client.Connected;
        }

        public static void Connect(string ip, int port)

        {
            try
            {
                Client = new TcpClient(ip, port);
                _networkStream = Client.GetStream();
                _networkStream.ReadTimeout = 10000;
              //  _networkStream.WriteTimeout = 1000;
            }
            catch (GameLabException e)
            {
                throw new GameLabException("Error during connect to serwer: +" + e.Message);
            }
        }

        public static void Disconnect()
        {
            try
            {
                _networkStream.Close();
                Client.Close();
            }
            catch (Exception e)
            {
                throw new GameLabException("Error during disconnect to serwer" + e.Message);
            }
        }

        public static bool Send(string data)
        {
            try
            {
                byte[] message = Encoding.ASCII.GetBytes(data);
                _networkStream.Write(message, 0, message.Length);
                //_networkStream.FlushAsync();
                _networkStream.Flush();
            }
            catch (Exception e)
            {
                throw new GameLabException("Error during send data to serwer" + e.Message);
            }
            return true;
        }

        public static string ReceiveAnswer(string idQuestion)
        {
            string answer = "";
            int received = 0;
            byte[] data = new byte[256];

            string tmp = "";
            while (true)
            {
                do
                {
                    received = _networkStream.Read(data, 0, data.Length);
                    tmp += Encoding.ASCII.GetString(data, 0, received);
                } while ((tmp.Contains(">") == false));

                if (tmp.Contains("\"" + idQuestion + "\""))
                {
                    var messages = tmp.Split('<');
                    foreach (var item in messages)
                    {
                        if (item.Contains("\"" + idQuestion + "\"") && item.Contains(">\r\n"))
                        {
                            answer = "<" + item;
                            break;
                        }
                    }

                }
                if (answer != "") break;
            }
            return answer;

        }

        public static string Receive()
        {
            string answer = "";
            int received = 0;
            byte[] data = new byte[256];
            var count = 0;
            while ((!_networkStream.DataAvailable) && (count < 10))
            {
                Thread.Sleep(50);
                count++;
            }
            try
            {
                string tmp;
                while (_networkStream.DataAvailable)
                {
                    received = _networkStream.Read(data, 0, data.Length);
                    tmp = Encoding.ASCII.GetString(data, 0, received);
                    if (tmp.Contains("REC"))
                        break;
                    answer += tmp;
                };
                var tab = answer.Split(new string[] { "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                if(tab.Length > 0)
                    answer = tab[tab.Length - 1] + "\r\n";
            }
            catch (Exception e)
            {
                throw new GameLabException("Error during received data from serwer" + e.Message);
            }
            return answer;
        }

        public static string ReceiveREC()
        {
            string answer = "";
            int received = 0;
            byte[] data = new byte[256];
            var count = 0;
            while (_networkStream.CanRead && (!_networkStream.DataAvailable) && (count < 10))
            {
                Thread.Sleep(50);
                count++;
            }
            try
            {
                while (_networkStream.CanRead && _networkStream.DataAvailable)
                {
                    received = _networkStream.Read(data, 0, data.Length);
                    answer += Encoding.ASCII.GetString(data, 0, received);
                };
            }
            catch (Exception e)
            {
                throw new GameLabException("Error during received data from serwer" + e.Message);
            }
            return answer;
        }
    }
}
