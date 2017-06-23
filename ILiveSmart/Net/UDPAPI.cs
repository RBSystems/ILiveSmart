using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmart
{
    public class UDPAPI
    {
        public static string h = "192.168.1.41";
        public static int p = 6890;

        public static void SendData(string data)
        {
            byte[] sendBytes = Encoding.ASCII.GetBytes(data);
            UDPClient client = new UDPClient(h, p);
            client.Connect();
            client.SendData(sendBytes);
            client.DisConnect();
        }

        public static void SendData(string host, int port, string data)
        {
            h = host;
            p = port;

            byte[] sendBytes = Encoding.ASCII.GetBytes(data);
            UDPClient client = new UDPClient(h, p);
            client.Connect();
            client.SendData(sendBytes);
            client.DisConnect();
        }
        public static void SendData(string host, int port, byte[] sendBytes)
        {
            UDPClient client = new UDPClient(host, port);
            client.Connect();
            client.SendData(sendBytes);
            client.DisConnect();
        }
    }
}
