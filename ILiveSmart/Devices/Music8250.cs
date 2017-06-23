using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;

namespace SenSmart
{
    public class Music8250
    {
        public delegate void MusicScenceEventHandler(bool val);


        public event MusicScenceEventHandler MusicScenceEvent = null;

        private bool _MusicIsOn = false;
        public bool MusicIsOn
        {
            get 
            {
                return this._MusicIsOn;
            }
            set 
            {
                if (this.MusicScenceEvent!=null)
                {
                    this.MusicScenceEvent(value);
                }
                this._MusicIsOn = value;
            }
        }

        /// <summary>
        /// 背景音乐
        /// </summary>
        public bool MusicIsBusy = false;

                private IROutputPort port = null;
                public Music8250(IROutputPort port)
        {
            this.port = port;
        }
        #region 开关
        public void ZoneOn()
                {
                    this.MusicIsOn = true;
            this.MusicIsBusy = true;
            this.Zone1On();
            this.Zone2On();
            this.Zone3On();
            this.Zone4On();
            this.Zone5On();
            this.Zone6On();
            this.Zone7On();
            this.Zone8On();
            this.Zone9On();
            this.Zone10On();
            this.Zone11On();
            this.Zone12On();
            this.MusicIsBusy = false;
            
        }
        public void ZoneOff()
        {
            this.MusicIsOn = false;
            this.MusicIsBusy = true;
            this.Zone1Off();
            this.Zone2Off();
            this.Zone3Off();
            this.Zone4Off();
            this.Zone5Off();
            this.Zone6Off();
            this.Zone7Off();
            this.Zone8Off();
            this.Zone9Off();
            this.Zone10Off();
            this.Zone11Off();
            this.Zone12Off();
            this.MusicIsBusy = false;
            
        }
        public void Zone1On()
        {
            //\xFF\x01\x00\xA1\x00\xA2\xFE
            /*
             FF:开头
             * 01：区域 01-06
             * 00：音源
             * A1：开机(A0关机)
             * 00：
             * A2：校验
             * FE结束
             */
           // ILiveDebug.Instance.WriteLine("Zone1On");
            this.SendData(new byte[]{0xFF, 0x01, 0x06, 0xA1, 0x00, 0xA8, 0xFE});
            Thread.Sleep(1000);
        }
        public void Zone2On()
        {
            //\xFF\x02\x00\xA1\x00\xA3\xFE
            this.SendData(new byte[]{0xFF, 0x02, 0x06, 0xA1, 0x00, 0xA9, 0xFE});
            Thread.Sleep(1000);
        }
        public void Zone3On()
        {
            //\xFF\x03\x00\xA1\x00\xA4\xFE
            this.SendData(new byte[]{0xFF, 0x03, 0x06, 0xA1, 0x00, 0xAA, 0xFE});
            Thread.Sleep(1000);
        }
        public void Zone4On()
        {
            this.SendData(new byte[] { 0xFF, 0x04, 0x06, 0xA1, 0x00, 0xAB, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone5On()
        {
            this.SendData(new byte[] { 0xFF, 0x05, 0x06, 0xA1, 0x00, 0xAC, 0xFE });

            Thread.Sleep(1000);
        }
        public void Zone6On()
        {
            this.SendData(new byte[] { 0xFF, 0x06, 0x06, 0xA1, 0x00, 0xAD, 0xFE });

            Thread.Sleep(1000);
        }
        public void Zone7On()
        {
            this.SendData(new byte[] { 0xFF, 0x07, 0x06, 0xA1, 0x00, 0xAE, 0xFE });

            Thread.Sleep(1000);
        }
        public void Zone8On()
        {
            this.SendData(new byte[] { 0xFF, 0x08, 0x06, 0xA1, 0x00, 0xAF, 0xFE });

            Thread.Sleep(1000);
        }
        public void Zone9On()
        {
            this.SendData(new byte[] { 0xFF, 0x09, 0x06, 0xA1, 0x00, 0xB0, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone10On()
        {
            this.SendData(new byte[] { 0xFF, 0x0A, 0x06, 0xA1, 0x00, 0xB1, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone11On()
        {
            this.SendData(new byte[] { 0xFF, 0x0B, 0x06, 0xA1, 0x00, 0xB2, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone12Onmp3()
        {
            this.SendData(new byte[] { 0xFF, 0x0C, 0x01, 0xA1, 0x00, 0xAE, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone12On()
        {
            this.SendData(new byte[] { 0xFF, 0x0C, 0x06, 0xA1, 0x00, 0xB3, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone1Off()
        {
            this.SendData(new byte[] { 0xFF, 0x01, 0x06, 0xA0, 0x00, 0xA7, 0xFE });
            Thread.Sleep(1000);
            //\xFF\x01\x00\xA0\x00\xA1\xFE
        }
        public void Zone2Off()
        {
            this.SendData(new byte[] { 0xFF, 0x02, 0x06, 0xA0, 0x00, 0xA8, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone3Off()
        {
            this.SendData(new byte[] { 0xFF, 0x03, 0x06, 0xA0, 0x00, 0xA9, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone4Off()
        {
            this.SendData(new byte[] { 0xFF, 0x04, 0x06, 0xA0, 0x00, 0xAA, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone5Off()
        {
            this.SendData(new byte[] { 0xFF, 0x05, 0x06, 0xA0, 0x00, 0xAB, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone6Off()
        {
            this.SendData(new byte[] { 0xFF, 0x06, 0x06, 0xA0, 0x00, 0xAC, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone7Off()
        {
            this.SendData(new byte[] { 0xFF, 0x07, 0x06, 0xA0, 0x00, 0xAD, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone8Off()
        {
            this.SendData(new byte[] { 0xFF, 0x08, 0x06, 0xA0, 0x00, 0xAE, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone9Off()
        {
            this.SendData(new byte[] { 0xFF, 0x09, 0x06, 0xA0, 0x00, 0xAF, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone10Off()
        {
            this.SendData(new byte[] { 0xFF, 0x0A, 0x06, 0xA0, 0x00, 0xB0, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone11Off()
        {
            this.SendData(new byte[] { 0xFF, 0x0B, 0x06, 0xA0, 0x00, 0xB1, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone12Off()
        {
            this.SendData(new byte[] { 0xFF, 0x0C, 0x06, 0xA0, 0x00, 0xB2, 0xFE });
            Thread.Sleep(1000);
        }
        public void Zone12Offmp3()
        {
            this.SendData(new byte[] { 0xFF, 0x0C, 0x01, 0xA0, 0x00, 0xAD, 0xFE });
            Thread.Sleep(1000);
        }
        /// <summary>
        /// 播放MP3
        /// </summary>
        public void MusicPlay12()
        {
            this.Zone12Onmp3();
            this.SendData(new byte[] { 0xFF, 0x0C, 0x01, 0xA4, 0x00, 0xB1, 0xFE });//停止

            Thread.Sleep(1000);
            this.SendData(new byte[] { 0xFF, 0x0C, 0x01, 0xA2, 0x00, 0xAF, 0xFE });//播放


        }
        public void MusicPause()
        {
            this.SendData(new byte[] { 0xFF, 0x0C, 0x01, 0xA4, 0x00, 0xB1, 0xFE });//停止

        }
        // this.myIROutputPort6.SendSerialData(this.GetCMDString());功放暂停


        //FF 01 01 A2 00 A4 FE
        //FF 01 01 A4 00 A6 FE
        //FF 01 01 A5 00 A7 FE
        //FF 01 01 A6 00 A8 FE

        #endregion


        private void SendData(byte[] sendbytes)
        {
            this.port.SendSerialData(this.GetCMDString(sendbytes));


        }
        private string GetCMDString(params byte[] sendBytes)
        {
            // sendBytes = new byte[] { 0x01, 0x06, 0x07, 0xDC, 0x50, 0x61, 0xB4, 0xAC };
            return Encoding.GetEncoding(28591).GetString(sendBytes, 0, sendBytes.Length);
        }

        /// <summary>
        /// auxdio功放
        /// </summary>
        /// <param name="room">房间号：00-09 </param>
        /// <param name="source">节目源 mp3 01 tuner 02 dvd 03 pc 04 tv 05 aux 06</param>
        /// <param name="fun">功能
        /// 开启：A1 关闭：A0 查询：D2 停止：A4 下一曲：A6 上一曲：A5 播放：A2 音量：A7
        /// </param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        private string GetMusicAVCMDString(byte room, byte source, byte fun, byte p)
        {
            /*
         *   起始位：FF
             *   房间号：00-09 
         *     未知：00 （节目源） /
         *    功能：
         *     未知：00 音量+：01 音量-：00
         *   校验位：除去起始和终止 
         *     终止：FE
         *     
         * 房间打开 FF 00 00 A1 00 A1 FE./
         * 房间关闭 FF 00 00 A0 00 A0 FE
         */
            byte check = (byte)(room + source + fun + p);
            byte[] data = new byte[] { 0xFF, room, source, fun, p, check, 0xFE };

            return Encoding.GetEncoding(28591).GetString(data, 0, data.Length);

        }
    }
}