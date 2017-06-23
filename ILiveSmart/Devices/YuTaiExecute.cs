using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;

namespace ILiveSmart
{
    /// <summary>
    /// 暂时不用
    /// </summary>
    public class YuTaiExecute
    {
        public string serverip = "192.168.1.21";
        ILiveTCPClient tcpClient = new ILiveTCPClient();
        /// <summary>
        /// 背景音乐
        /// </summary>
        /// <param name="data"></param>
        public void SendPort1(string data)
        {
            tcpClient.Send(serverip, 10001, (string)data);
            
        }
        /// <summary>
        /// 串口播放器
        /// </summary>
        /// <param name="data"></param>
        public void SendPort2(string data)
        {
            tcpClient.Send(serverip, 10002, data);
        }
        public void SendPort3(string data)
        {
            tcpClient.Send(serverip, 10003, data);
        }
        public void SendPort4(string data)
        {
            tcpClient.Send(serverip, 10004, data);
        }
    }
}
