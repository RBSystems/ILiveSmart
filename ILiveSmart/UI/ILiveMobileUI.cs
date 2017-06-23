using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;
using Crestron.SimplSharp.CrestronSockets;

namespace ILiveSmart.UI
{
    public class SocketConnection
    {

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Boolean isDataMasked;
        public Boolean IsDataMasked
        {
            get { return isDataMasked; }
            set { isDataMasked = value; }
        }

        public TCPServer ConnectionSocket;
        public uint clientindex;
        // public Socket ConnectionSocket;

        private int MaxBufferSize;
        private string Handshake;
        private string New_Handshake;

        // public byte[] receivedDataBuffer;
        private byte[] FirstByte;
        private byte[] LastByte;
        private byte[] ServerKey1;
        private byte[] ServerKey2;


        public event ILiveSmart.UI.ILiveMobileUI.NewConnectionEventHandler NewConnection;
        public event ILiveSmart.UI.ILiveMobileUI.DataReceivedEventHandler DataReceived;
        public event ILiveSmart.UI.ILiveMobileUI.DisconnectedEventHandler Disconnected;

        public SocketConnection()
        {
            MaxBufferSize = 1024 * 100;
         
        }
        #region 接收正式数据
        public void Read(TCPServer myTCPServer, uint clientIndex, int numberOfBytesReceived)
        {
            if (!myTCPServer.ClientConnected(clientindex)) return;
            string messageReceived = string.Empty;
            try
            {
                messageReceived = Encoding.GetEncoding(28591).GetString(myTCPServer.GetIncomingDataBufferForSpecificClient(clientindex), 0, numberOfBytesReceived);
                DataReceived(this, messageReceived, EventArgs.Empty);

                myTCPServer.ReceiveDataAsync(clientindex, this.Read, 0);

            }
            catch (Exception ex)
            {
                if (Disconnected != null)
                    Disconnected(this, EventArgs.Empty);
            }
        }

        #endregion



        //void ReceivedCallBack(TCPServer myTCPServer, uint clientIndex, int numberOfBytesReceived)
        #region SocketClose
        public void SocketClose()
        {
            this.ConnectionSocket.Disconnect(clientindex);
        }
        #endregion

    }

    public class ILiveMobileUI
    {
        /// <summary>
        /// 连接建立委托
        /// </summary>
        /// <param name="e"></param>
        public delegate void NewConnectionEventHandler(EventArgs e);
        /// <summary>
        /// 数据接收委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public delegate void DataReceivedEventHandler(Object sender, string message, EventArgs e);
        /// <summary>
        /// 连接断开委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DisconnectedEventHandler(Object sender, EventArgs e);



        private TCPServer Listener;
        /// <summary>
        /// 最大缓冲区大小
        /// </summary>
        private int MaxBufferSize;
        /// <summary>
        /// 最大连接数
        /// </summary>
        private int ConnectionsQueueLength;

        public int ServerPort = 8800;

        /// <summary>
        /// 客户端列表
        /// </summary>
        List<SocketConnection> connectionSocketList = new List<SocketConnection>();

        public event NewConnectionEventHandler NewConnection;
        public event DataReceivedEventHandler DataReceived;
        public event DisconnectedEventHandler Disconnected;

        public delegate void TcpDataHandler(string data);

        public event TcpDataHandler TcpDataEvent;
        public void Register()
        {
            this.ServerPort = 8800;
            this.MaxBufferSize = 1024 * 100;
            this.ConnectionsQueueLength = 20;

            new Thread(StartListen, null, Thread.eThreadStartOptions.Running);
        }

        public object StartListen(object o)
        {
            Listener = new TCPServer("0.0.0.0", ServerPort, MaxBufferSize, EthernetAdapterType.EthernetLANAdapter, ConnectionsQueueLength);

            Listener.SocketStatusChange += new TCPServerSocketStatusChangeEventHandler(Listener_SocketStatusChange);

            //WebSocketAddress

            while (true)
            {
                //While Wait Connect
                try
                {
                    SocketErrorCodes codes = Listener.WaitForConnection("0.0.0.0", this.OnClientConnect);
                }
                catch (Exception ex)
                {
                    // logger.Log(ex.Message);
                }
            }

          //  ConnectionSocket = new TCPServer("0.0.0.0", port, 4096);
           // ConnectionSocket.SocketStatusChange += new TCPServerSocketStatusChangeEventHandler(tcp_SocketStatusChange);
            // this.Open();

            //ILiveTCPServer tcp = new ILiveTCPServer(8800);
            //tcp.TcpDataEvent += this.OnTcpReceived;
            //tcp.Listen();
            return null;
        }

        public void OnClientConnect(TCPServer myTCPServer, uint clientIndex)
        {
            if (myTCPServer.ClientConnected(clientIndex))
            {
                SocketConnection socketConn = new SocketConnection();
                socketConn.clientindex = clientIndex;
                socketConn.ConnectionSocket = myTCPServer;
                socketConn.NewConnection += new NewConnectionEventHandler(socketConn_NewConnection);
                socketConn.DataReceived += new DataReceivedEventHandler(socketConn_BroadcastMessage);
                socketConn.Disconnected += new DisconnectedEventHandler(socketConn_Disconnected);

                myTCPServer.ReceiveDataAsync(clientIndex, socketConn.Read, 0);


                connectionSocketList.Add(socketConn);
                //ClientConnected clientIndex
            }

        }
        /// <summary>
        /// 网络状态变化
        /// </summary>
        /// <param name="myTCPServer"></param>
        /// <param name="clientIndex"></param>
        /// <param name="serverSocketStatus"></param>
        void Listener_SocketStatusChange(TCPServer myTCPServer, uint clientIndex, SocketStatus serverSocketStatus)
        {

            if (serverSocketStatus == SocketStatus.SOCKET_STATUS_NO_CONNECT || serverSocketStatus==SocketStatus.SOCKET_STATUS_LINK_LOST)
            {
                for (int i = 0; i < connectionSocketList.Count; i++)
                {
                    if (connectionSocketList[i].clientindex == clientIndex)
                    {
                        connectionSocketList[i].SocketClose();
                        connectionSocketList.Remove(connectionSocketList[i]);
                    }
                }
            }

            //logger.Log("SocketStatusChange:" + serverSocketStatus);
            //throw new NotImplementedException();
        }

        void socketConn_Disconnected(Object sender, EventArgs e)
        {
   
            if (Disconnected != null)
                Disconnected(sender, e);

            SocketConnection sConn = sender as SocketConnection;
            if (sConn != null)
            {
                // Send(string.Format("{0} DisConnected", sConn.Name));
                sConn.SocketClose();
                connectionSocketList.Remove(sConn);
            }
        }

        void socketConn_BroadcastMessage(Object sender, string message, EventArgs e)
        {
            //   logger.Log("ClientMsg:" + message);
            if (DataReceived != null)
            {
                this.DataReceived(sender, message, e);
            }
            // Send("ServerMsg:"+DateTime.Now.ToLongTimeString());
        }

        void socketConn_NewConnection(EventArgs e)
        {
            if (NewConnection != null)
                NewConnection(EventArgs.Empty);
        }

        public void Send(string message)
        {
            byte[] messageSend = Encoding.GetEncoding(28591).GetBytes(message);

            foreach (SocketConnection item in connectionSocketList)
            {
                if (!item.ConnectionSocket.ClientConnected(item.clientindex)) return;
                try
                {

                    item.ConnectionSocket.SendData(item.clientindex, messageSend, messageSend.Length);

                }
                catch (Exception ex)
                {
                 //   this.Log(string.Format("Exception:{0}", ex.Message));

                    //  logger.Log(ex.Message);
                }
            }
        }
 
        public void OnTcpReceived(string data)
        {
            ILiveDebug.Instance.WriteLine("OnTcpReceived:" + data);
            switch (data)
            {
                
                case "r62":
                   // this.RelayPorts[6].Close();
                    break;

                default:
                    break;
            }


        }
    }
}