using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using WebSocketServer;
using Crestron.SimplSharpPro.CrestronThread;

namespace ILiveSmart
{
    public class WebSocketUI
    {  // public delegate void DataReceivedEventHandler(object sender, string message, EventArgs e);
        public event DataReceivedEventHandler DataReceived;

        private WebSocketServer.WebSocketServer WSServer = new WebSocketServer.WebSocketServer();
        /// <summary>
        /// 接收事件
        /// </summary>
        private Thread webSocketThread;

        public WebSocketUI()
        {
        }
        public void Register()
        {
            webSocketThread = new Thread(WebSocketServerStart, null, Thread.eThreadStartOptions.Running);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        internal object WebSocketServerStart(object o)
        {

            WSServer.NewConnection += new NewConnectionEventHandler(WSServer_NewConnection);
            WSServer.Disconnected += new DisconnectedEventHandler(WSServer_Disconnected);
            WSServer.DataReceived += this.DataReceived;
            WSServer.Log += new LogEventHandler(WSServer_Log);
            WSServer.StartServer();
            return o;
        }

        void WSServer_Log(string Msg)
        {
            ILiveDebug.Instance.WriteLine("Log" + DateTime.Now.ToShortTimeString() + ":" + Msg);
        }
        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="e"></param>
        void WSServer_NewConnection(EventArgs e)
        {
            //ILiveDebug.Instance.WriteLine("IpadConnection" + DateTime.Now.ToShortTimeString() + ":" + e.ToString());
        }
        /// <summary>
        /// 客户端断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WSServer_Disconnected(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        public void WSServer_Send(string msg)
        {
            WSServer.Send(msg);
        }
        //void WSServer_DataReceived(object sender, string message, EventArgs e)
        //{
        //    if (!message.StartsWith("CP3"))
        //    {
        //        return;
        //    }
        //    message = message.Replace("CP3", "");
        //    if (!message.StartsWith("Scence"))
        //    {
        //        string cmd = message.Replace("Scence", "");
        //        this.ExeScence(cmd);
        //    }

        //    //this.WSServer_Send(DateTime.Now.ToLongTimeString() + "CP3:" + message);
        //    //WebSocket接收消息
        //}



    }
}