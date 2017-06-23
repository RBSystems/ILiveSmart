using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;

namespace ILiveSmart
{
    /// <summary>
    /// 窗帘控制接口
    /// </summary>
    public class CurtainsAPI
    {
        public string serverip = "192.168.1.25";
        ILiveTCPClient tcpClient = new ILiveTCPClient();
        GuangYinExecute gy = new GuangYinExecute();
        public byte[] GetCode(byte vlaue)
        {
                byte[] bs = new byte [8];
                bs[0] = 0x55;
                bs[1] = 0x01;
                bs[2] = 0x25;
                bs[3] = 0x00;
                bs[4] = 0x00;
                bs[5] = 0x01;
                bs[6] = vlaue;
                bs[7] = (byte)(bs[0] + bs[1] + bs[2] + bs[3] + bs[4] + bs[5] + bs[6]);
                return bs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">控制器地址 默认为0x01</param>
        /// <param name="cmd">功能码： 0x10:查询  0x11 0x12 0x13</param>
        /// <param name="vlaue"></param>
        /// <returns></returns>
        public byte[] GetCode(byte address,byte cmd,UInt32 vlaue)
        {
                byte[] bs = new byte [8];
                bs[0] = 0x55;
                bs[1] = address;
                bs[2] = cmd;
                bs[3] = (byte)(vlaue>>24 & 0xFF);
                bs[4] = (byte)(vlaue>>16 & 0xFF);
                bs[5] = (byte)(vlaue>>8 & 0xFF);
                bs[6] = (byte)(vlaue & 0xFF);
                bs[7] = (byte)(bs[0] + bs[1] + bs[2] + bs[3] + bs[4] + bs[5] + bs[6]);
            return bs;
        }

        #region 客厅窗帘
        public void LivingWindowOpen()
        {
            byte[] data={0x55,0xAA,0x03,0x01,0x00,0x00,0x01};
            gy.SendPort4(data);
        }
        public void LivingWindowClose()
        {
            byte[] data = { 0x55, 0xAA, 0x03, 0x03, 0x00, 0x00, 0x03 };
            gy.SendPort4(data);
        }
        public void LivingWindowStop()
        {
            byte[] data = { 0x55, 0xAA, 0x03, 0x02, 0x00, 0x00, 0x02 };
            gy.SendPort4(data);
        }

        public void LivingWindow10()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x20, 0x24 };
            gy.SendPort4(data);
        }
        public void LivingWindow20()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x40, 0x44 };
            gy.SendPort4(data);
        }
        public void LivingWindow30()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x60, 0x64 };
            gy.SendPort4(data);
        }
        public void LivingWindow40()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x80, 0x84 };
            gy.SendPort4(data);
        }
        public void LivingWindow50()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0xA0, 0xA4 };
            gy.SendPort4(data);
        }
        public void LivingWindow60()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0xC0, 0xC4 };
            gy.SendPort4(data);
        }
        public void LivingWindow70()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0xE0, 0xE4 };
            gy.SendPort4(data);
        }
        public void LivingWindow100()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0xFF, 0x03 };
            gy.SendPort4(data);
        }
        #endregion
        #region 客厅纱窗
        public void LivingWindow1Open()
        {
            byte[] data = { 0x55, 0xAA, 0x03, 0x01, 0x00, 0x00, 0x01 };
            gy.SendPort5(data);
        }
        public void LivingWindow1Close()
        {
            byte[] data = { 0x55, 0xAA, 0x03, 0x03, 0x00, 0x00, 0x03 };
            gy.SendPort5(data);
        }
        public void LivingWindow1Stop()
        {
            byte[] data = { 0x55, 0xAA, 0x03, 0x02, 0x00, 0x00, 0x02 };
            gy.SendPort5(data);
        }

        public void LivingWindow110()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x20, 0x24 };
            gy.SendPort5(data);
        }
        public void LivingWindow120()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x40, 0x44 };
            gy.SendPort5(data);
        }
        public void LivingWindow130()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x60, 0x64 };
            gy.SendPort5(data);
        }
        public void LivingWindow140()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x80, 0x84 };
            gy.SendPort5(data);
        }
        public void LivingWindow150()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0xA0, 0xA4 };
            gy.SendPort5(data);
        }
        public void LivingWindow160()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0xC0, 0xC4 };
            gy.SendPort5(data);
        }
        public void LivingWindow170()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0xE0, 0xE4 };
            gy.SendPort5(data);
        }
        public void LivingWindow1100()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0xFF, 0x03 };
            gy.SendPort5(data);
        }
        #endregion
        #region 卧室窗帘
        public void BedRoomWindowOpen()
        {
            byte[] data = { 0x55, 0xAA, 0x03, 0x01, 0x00, 0x00, 0x01 };
            gy.SendPort6(data);
        }
        public void BedRoomWindowClose()
        {
            byte[] data = { 0x55, 0xAA, 0x03, 0x03, 0x00, 0x00, 0x03 };
            gy.SendPort6(data);
        }
        public void BedRoomWindowStop()
        {
            byte[] data = { 0x55, 0xAA, 0x03, 0x02, 0x00, 0x00, 0x02 };
            gy.SendPort6(data);
        }

        public void BedRoomWindow10()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x00, 0x20, 0x24 };
            gy.SendPort6(data);
        }
        public void BedRoomWindow20()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x00, 0x40, 0x44 };
            gy.SendPort6(data);
        }
        public void BedRoomWindow30()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x00, 0x60, 0x64 };
            gy.SendPort6(data);
        }
        public void BedRoomWindow40()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x00, 0x80, 0x84 };
            gy.SendPort6(data);
        }
        public void BedRoomWindow50()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x00, 0xA0, 0xA4 };
            gy.SendPort6(data);
        }
        public void BedRoomWindow60()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x00, 0xC0, 0xC4 };
            gy.SendPort6(data);
        }
        public void BedRoomWindow70()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x00, 0xE0, 0xE4 };
            gy.SendPort6(data);
        }
        public void BedRoomWindow100()
        {
            byte[] data = { 0x55, 0xAA, 0x04, 0x04, 0x00, 0x00, 0xFF, 0x03 };
            gy.SendPort6(data);
        }
        #endregion
        #region 书房窗帘
        private int studyroomwindowtime = 0;

        public void StudyRoomWindowUp()
        {
            StudyRoomWindowIsUp = true;
            new Thread(new ThreadCallbackFunction(this.StudyRoomWindowUpExe), this, Thread.eThreadStartOptions.Running);

        }
        public object StudyRoomWindowUpExe(object o)
        {
            if (studyroomwindowtime==0)
            {
                byte[] data = this.GetCode(0x13);
                tcpClient.Send(serverip, 3000, Encoding.GetEncoding(28591).GetString(data, 0, data.Length));

                studyroomwindowtime = 20;
                while (studyroomwindowtime > 0)
                {
                    studyroomwindowtime--;
                    Thread.Sleep(1000);
                }

            }
            return o;
        }
        public void StudyRoomWindowDown()
        {
            StudyRoomWindowIsUp = false;
            new Thread(new ThreadCallbackFunction(this.StudyRoomWindowDownExe), this, Thread.eThreadStartOptions.Running);

        }
        public object StudyRoomWindowDownExe(object o)
        {
            if (studyroomwindowtime ==0)
            {
                byte[] data = this.GetCode(0x14);
                tcpClient.Send(serverip, 3000, Encoding.GetEncoding(28591).GetString(data, 0, data.Length));

                studyroomwindowtime = 20;
                while (studyroomwindowtime > 0)
                {
                    studyroomwindowtime--;
                    Thread.Sleep(1000);
                }
            }
            return o;
        }

        public void StudyRoomWindowStop()
        {
            if (studyroomwindowtime > 0)
            {
                byte[] data = this.GetCode(0x14);
                tcpClient.Send(serverip, 3000, Encoding.GetEncoding(28591).GetString(data, 0, data.Length));

            }
        }

        private bool StudyRoomWindowIsUp = false;//1上升状态 2下降状态 0 运行状态
        public void StudyRoomWindowToggle()
        {
            if (StudyRoomWindowIsUp)
            {
                if (studyroomwindowtime > 0)
                {
                    //ILiveDebug.WriteLine("stop");
                    byte[] data = this.GetCode(0x14);
                    tcpClient.Send(serverip, 3000, Encoding.GetEncoding(28591).GetString(data, 0, data.Length));
                    studyroomwindowtime =0;

                }
                else
                {
                   // ILiveDebug.WriteLine("down");
                    this.StudyRoomWindowDown();
                }
                
            }
            else
            {
                if (studyroomwindowtime>0)
                {
                    ILiveDebug.Instance.WriteLine("stop");
                    byte[] data = this.GetCode(0x13);
                    tcpClient.Send(serverip, 3000, Encoding.GetEncoding(28591).GetString(data, 0, data.Length));
                    studyroomwindowtime = 0;
                }
                else
                {
                    ILiveDebug.Instance.WriteLine("up");
                    this.StudyRoomWindowUp();
                }
                
            }
        }
        #endregion
        
    }
}