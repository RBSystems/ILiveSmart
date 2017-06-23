using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using System.Net.Sockets;

namespace ILiveSmart
{
    public class UDPClient
    {
        public UDPServer server = null;

        public string host = string.Empty;
        public int port = 0;
        public UDPClient()
            : this("", 0)
        {
        }
        public UDPClient(string host, int port)
        {
           // Socket clientSocket = new Socket(Crestron.SimplSharp.AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
            this.host = host;
            this.port = port;
            server = new UDPServer();
        }
        public void Connect()
        {
            server.EnableUDPServer(this.host, 0, this.port);
        }
        public void DisConnect()
        {
            server.DisableUDPServer();
        }
        public void SendData(byte[] sendbytes)
        {


            server.SendData(sendbytes, sendbytes.Length);

        }
        public byte[] RecevedData()
        {
            byte[] rbytes = { };
            if (server.ReceiveData() > 0)
            {
                rbytes = server.IncomingDataBuffer;
            }

            return rbytes;
        }
    }
}