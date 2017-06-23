using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;

namespace ILiveSmartTest
{
    public class UDPClient
    {
        private bool IsConnected = false;
        public UDPServer server = null;

        public string host = string.Empty;
        public int port = 0;
        public UDPClient()
            : this("", 0)
        {
        }
        public UDPClient(string host, int port)
        {
            this.host = host;
            this.port = port;
            server = new UDPServer();
        }
        public void Connect()
        {
            if (!IsConnected)
            {
                server.EnableUDPServer(this.host, 0, this.port);
                this.IsConnected = true;
            }
           
        }
        public void DisConnect()
        {
            if (this.IsConnected)
            {
                server.DisableUDPServer();
                this.IsConnected = false;
            }
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