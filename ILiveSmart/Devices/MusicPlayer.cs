using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;

namespace SenSmart
{
    /// <summary>
    /// 串口播放器
    /// </summary>
    public class MusicPlayer
    {
       // public UDPServer server = new UDPServer();
        private IROutputPort port = null;
        public MusicPlayer(IROutputPort port)
        {
            this.port = port;
        }
        #region 背景音乐
        /// <summary>
        /// 指定播放第一曲
        /// </summary>
        public void MusicPlay1()
        {
            this.SendData(new byte[]{0x7E, 0xFF, 0x06, 0x03, 0x00, 0x00, 0x01, 0xFE, 0xF7, 0xEF});
            Thread.Sleep(100);
        }
        public void MusicPlay2()
        {
            this.SendData(new byte[] { 0x7E, 0xFF, 0x06, 0x03, 0x00, 0x00, 0x02, 0xFE, 0xF6, 0xEF });
            Thread.Sleep(100);
        }
        public void MusicPlay3()
        {
            this.SendData(new byte[] { 0x7E, 0xFF, 0x06, 0x03, 0x00, 0x00, 0x03, 0xFE, 0xF5, 0xEF });
            Thread.Sleep(100);
        }
        public void MusicPlay4()
        {
            this.SendData(new byte[] { 0x7E, 0xFF, 0x06, 0x03, 0x00, 0x00, 0x04, 0xFE, 0xF4, 0xEF });
            Thread.Sleep(100);
        }
        public void MusicPlay5()
        {
            this.SendData(new byte[] { 0x7E, 0xFF, 0x06, 0x03, 0x00, 0x00, 0x05, 0xFE, 0xF3, 0xEF });
            Thread.Sleep(100);
        }
        public void MusicPlay6()
        {
            this.SendData(new byte[] { 0x7E, 0xFF, 0x06, 0x03, 0x00, 0x00, 0x06, 0xFE, 0xF2, 0xEF });
            Thread.Sleep(100);
        }
        public void MusicPlay7()
        {
            //this.Zone12On();切换信号源
            this.SendData(new byte[] { 0x7E, 0xFF, 0x06, 0x03, 0x00, 0x00, 0x07, 0xFE, 0xF1, 0xEF });
            Thread.Sleep(100);
        }
        public void MusicPlay()
        {
            this.SendData(new byte[] { 0x7E, 0xFF, 0x06, 0x0D, 0x00, 0x00, 0x00, 0xFE, 0xEE, 0xEF });//继续播放
            Thread.Sleep(100);
        }
        public void MusicPause()
        {
            this.SendData(new byte[] { 0x7E, 0xFF, 0x06, 0x0E, 0x00, 0x00, 0x00, 0xFE, 0xED, 0xEF });//语音播放器暂停
            Thread.Sleep(300);
           // this.myIROutputPort6.SendSerialData(this.GetCMDString(0xFF, 0x0C, 0x01, 0xA4, 0x00, 0xB1, 0xFE));功放暂停

        }
        #endregion

        public void MusicVolUp()
        {
            this.SendData(new byte[] {0x7E, 0xFF, 0x06, 0x04, 0x00, 0x00, 0x00, 0xFE, 0xF7, 0xEF});

            //\xFF\x01\x01\xA7\x01\xAA\xFE
        }
        public void MusicVolDown()
        {
            this.SendData(new byte[] {0x7E, 0xFF, 0x06, 0x05, 0x00, 0x00, 0x00, 0xFE, 0xF6, 0xEF});
        }

        private void SendData(byte[] sendbytes)
        {
            this.port.SendSerialData(this.GetCMDString(sendbytes));


        }
        private string GetCMDString(params byte[] sendBytes)
        {
            // sendBytes = new byte[] { 0x01, 0x06, 0x07, 0xDC, 0x50, 0x61, 0xB4, 0xAC };
            return Encoding.GetEncoding(28591).GetString(sendBytes, 0, sendBytes.Length);
        }
        public void SetVol(int i)
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x1E, 0xFE, 0xD7, 0xEF };
            switch (i)
            {
                case 1:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x03, 0xFE, 0xF2, 0xEF };
                    break;
                case 2:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x06, 0xFE, 0xEF, 0xEF };
                    break;
                case 3:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x09, 0xFE, 0xEC, 0xEF };
                    break;
                case 4:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x0C, 0xFE, 0xE9, 0xEF };
                    break;
                case 5:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x0F, 0xFE, 0xE6, 0xEF };
                    break;
                case 6:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x12, 0xFE, 0xE3, 0xEF };
                    break;
                case 7:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x15, 0xFE, 0xE0, 0xEF };
                    break;
                case 8:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x18, 0xFE, 0xDD, 0xEF };
                    break;
                case 9:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x1B, 0xFE, 0xDA, 0xEF };
                    break;
                case 10:
                    code = new byte[] { 0x7E, 0xFF, 0x06, 0x06, 0x00, 0x00, 0x1E, 0xFE, 0xD7, 0xEF };
                    break;
                default:
                    break;
            }
            //   byte b=Convert.ToByte(i);

            this.SendData(code);
        }





        #region 函数
        private string GetMusicPlayerCMDString(params byte[] sendBytes)
        {
            int sum = 0;
            for (int i = 0; i < sendBytes.Length; i++)
            {
                sum += sendBytes[i];
            }
            sum = 0 - sum;
            int hValue = (sum >> 8) & 0xFF;
            int lValue = sum & 0xFF;
            byte[] arr = new byte[sendBytes.Length + 4];
            Buffer.BlockCopy(sendBytes, 0, arr, 1, sendBytes.Length);
            Buffer.SetByte(arr, 0, 0x7E);
            Buffer.SetByte(arr, sendBytes.Length + 1, (byte)hValue);
            Buffer.SetByte(arr, sendBytes.Length + 2, (byte)lValue);
            Buffer.SetByte(arr, sendBytes.Length + 3, 0xEF);
            return Encoding.GetEncoding(28591).GetString(arr, 0, arr.Length);

        }

        #endregion

    }
}