using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmart
{
    /// <summary>
    /// 光因扩展器
    /// </summary>
    public class GuangYinExecute
    {
        /// <summary>
        /// 机柜光因扩展器1 1、背景音乐功放 2、语音播放器 3、HDMI矩阵 4、投影吊架 5、空 6、空
        /// </summary>
        string host1 = "192.168.1.21";
        /// <summary>
        /// 机柜光因扩展器2 1、客厅投影 2、客厅功放 3、卧室投影 4、客厅窗帘 5、客厅纱窗 6、卧室窗帘
        /// </summary>
        string host2 = "192.168.1.22";
        /// <summary>
        /// 会议室宇泰扩展器2 1、 2、 3、 4、
        /// </summary>
        string host3 = "192.168.1.23";
        /// <summary>
        /// 卧室光因扩展器1 1、卧室投影 2、空 IR1、卧室功放 IR2、卧室蓝光机 IR3、卧室电视机 IR4、卧室镜面电视
        /// </summary>
        string host4 = "192.168.1.24";
        /// <summary>
        /// 客厅光因扩展器2 1、客厅投影 2、 IR1、客厅功放 IR2、客厅蓝光机 IR3、 IR4、
        /// </summary>
        string host5 = "192.168.1.26";
        #region 机柜光因扩展器2
        /// <summary>
        /// 客厅投影
        /// </summary>
        /// <param name="data"></param>
        public void SendPort1(byte[] data)
        {
            UDPAPI.SendData(host2, 8001, data);
        }
        /// <summary>
        /// 客厅功放
        /// </summary>
        /// <param name="data"></param>
        public void SendPort2(byte[] data)
        {
            UDPAPI.SendData(host2, 8002, data);
        }
        /// <summary>
        /// 卧室投影
        /// </summary>
        /// <param name="data"></param>
        public void SendPort3(byte[] data)
        {
            UDPAPI.SendData(host2, 8003, data);
        }
        /// <summary>
        /// 客厅窗帘
        /// </summary>
        /// <param name="data"></param>
        public void SendPort4(byte[] data)
        {
            UDPAPI.SendData(host2, 8004, data);
        }
        /// <summary>
        /// 客厅纱窗
        /// </summary>
        /// <param name="data"></param>
        public void SendPort5(byte[] data)
        {
            UDPAPI.SendData(host2, 8005, data);
        }
        /// <summary>
        /// 卧室窗帘
        /// </summary>
        /// <param name="data"></param>
        public void SendPort6(byte[] data)
        {
            UDPAPI.SendData(host2, 8006, data);
        }

        #endregion
        #region 卧室光因扩展器
        /// <summary>
        /// 卧室投影仪
        /// </summary>
        /// <param name="data"></param>
        public void SendBedRoomPort1(byte[] data)
        {
            UDPAPI.SendData(host4, 8001, data);
        }
        /// <summary>
        /// 空
        /// </summary>
        /// <param name="data"></param>
        public void SendBedRoomPort2(byte[] data)
        {
            UDPAPI.SendData(host2, 8001, data);
        }
        /// <summary>
        /// 卧室IR
        /// </summary>
        /// <param name="data"></param>
        public void SendBedRoomIR(int port,int index)
        {
            //irsend,1,39000,1,0,66,22,67,11,88,33\x0D 
            //irdb,1,10\x0D 
            byte[] sendBytes = Encoding.GetEncoding(28591).GetBytes("irdb,"+port+","+index);
            byte[] send = new byte[sendBytes.Length + 1];
            Buffer.BlockCopy(sendBytes, 0, send, 0, sendBytes.Length);
            send[send.Length - 1] = 0x0D;
           // ILiveDebug.Instance.WriteLine("GY:" + ILiveUtil.ToHexString(send));
            UDPAPI.SendData(host4, 9001, send);
        }
        #endregion
        ///// <summary>
        ///// 背景音乐功放
        ///// </summary>
        ///// <param name="data"></param>
        //public void SendPort7(byte[] data)
        //{
        //    UDPAPI.SendData(host1, 8001, data);
        //}
        ///// <summary>
        ///// 串口播放器zxx x 
        ///// </summary>
        ///// <param name="data"></param>
        //public void SendPort8(byte[] data)
        //{
        //    UDPAPI.SendData(host1, 8002, data);
        //}
        //public void SendPort9(byte[] data)
        //{
        //    UDPAPI.SendData(host1, 8003, data);
        //}
        //public void SendPort10(byte[] data)
        //{
        //    UDPAPI.SendData(host1, 8004, data);
        //}
        //public void SendPort11(byte[] data)
        //{
        //    UDPAPI.SendData(host1, 8005, data);
        //}
        //public void SendPort12(byte[] data)
        //{
        //    UDPAPI.SendData(host1, 8006, data);
        //}
    }
}