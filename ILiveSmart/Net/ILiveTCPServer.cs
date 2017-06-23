﻿using System;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net;
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharpPro.CrestronThread;

namespace ILiveSmart
{
    public class ILiveTCPServer
    {
        public delegate void TcpDataHandler(string data);


        public event TcpDataHandler TcpDataEvent;

        TCPServer tcp = null;
        //public int IPPort { get; set; }
        public ILiveTCPServer(int port)
        {
            tcp = new TCPServer("0.0.0.0",port,4096);
            tcp.SocketStatusChange += new TCPServerSocketStatusChangeEventHandler(tcp_SocketStatusChange);
           // this.Open();
            // 
        }

        void tcp_SocketStatusChange(TCPServer myTCPServer, uint clientIndex, SocketStatus serverSocketStatus)
        {
            if (serverSocketStatus==SocketStatus.SOCKET_STATUS_LINK_LOST||serverSocketStatus==SocketStatus.SOCKET_STATUS_NO_CONNECT)
            {
                tcp.DisconnectAll();
                this.Listen();
            }
           // throw new NotImplementedException();
        }


        public void Listen()
        {
            
            while (true)
            {

               SocketErrorCodes codes= tcp.WaitForConnection();
               // TcpClient client = tcpServer.AcceptTcpClient();
                while (true)
                {
                    try
                    {
                      //  ILiveDebug.WriteLine("ReceiveData");
                        int i=tcp.ReceiveData();
                        if (i>0)
                        {
                            string readdata = System.Text.Encoding.GetEncoding(28591).GetString(tcp.IncomingDataBuffer, 0, i);

                            if (this.TcpDataEvent!=null)
                            {
                                this.TcpDataEvent(readdata);
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    catch(Exception e)
                    {
                        ILiveDebug.Instance.WriteLine("TCPError:" + e.Message);
                       // break;
                    }
                }
            }  
        }
    }
}