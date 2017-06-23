using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmartTest
{
    public class UDPAPI
    {
        public static readonly UDPAPI Instance = new UDPAPI();
       private UDPClient client = null;

        private UDPAPI()
        {
            client = new UDPClient("192.168.1.41", 8800);
            
        }
        public void SendData(string data)
        {
            lock (this)
            {
                byte[] sendBytes = Encoding.ASCII.GetBytes(data);

                client.Connect();
                client.SendData(sendBytes);
                client.DisConnect();
            }
    
        }

        public void SendData(string host, int port, string data)
        {
            lock (this)
            {
                UDPClient client2 = new UDPClient(host, port);

                byte[] sendBytes = Encoding.ASCII.GetBytes(data);
                client2.Connect();
                client2.SendData(sendBytes);
                client2.DisConnect();
            }

        }
        public void SendData(string host, int port, byte[] sendBytes)
        {
            lock (this)
            {
                UDPClient client3 = new UDPClient(host, port);
                client3.Connect();
                client3.SendData(sendBytes);
                client3.DisConnect();
            }

        }
    }
}