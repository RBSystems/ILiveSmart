using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;

namespace ILiveSmart.Music
{
    /*
     0x0a Fm
0x0b Mp3
0x0c Aux
0x06 Dvd
0x10 FM2
0x11 IPOD
0x12 NET_RADIO
0x13 CLOUD_MUSIC
     */
    /// <summary>
    /// 泊声背景音乐控制
    /// </summary>
    public class BackAudio
    {
        public UDPServer server = new UDPServer();

        #region 全区
        /// <summary>
        /// 区域 true：开 false：关闭
        /// </summary>
        public bool ZoomOn
        {
            set
            {
                if (value == true)
                {
                    this._Zoom1On = true;
                    this._Zoom2On = true;
                    this._Zoom3On = true;
                    this._Zoom4On = true;

                    byte[] code = { 0x90, 0x0f, 0x01, 0x00, 0x0e };
                    this.SendData(code);
                }
                else
                {
                    this._Zoom1On = false;
                    this._Zoom2On = false;
                    this._Zoom3On = false;
                    this._Zoom4On = false;

                    byte[] code = { 0x90, 0x0f, 0xc0, 0x00, 0xcf };
                    this.SendData(code);
                }
                // this._ZoomOn = value;
            }
        }
        #endregion
        #region 区域0
        private bool _Zoom0On = false;
        /// <summary>
        /// 区域1 true：开 false：关闭
        /// </summary>
        public bool Zoom0On
        {
            get
            {
                return this._Zoom0On;
            }
            set
            {
                if (value == true)
                {
                    byte[] code = { 0xa3, 0x00, 0x07, 0x00, 0x07 };
                    this.SendData(code);
                }
                else
                {
                    byte[] code = { 0xa3, 0x00, 0x03, 0x00, 0x03 };
                    this.SendData(code);
                }
            }
        }
        private AudioSource _Zoom0Source = AudioSource.AUX;
        public AudioSource Zoom0Source
        {
            get
            {
                return this._Zoom0Source;
            }
            set
            {
                byte[] code = null;

                switch (value)
                {
                    case AudioSource.MP3:
                        break;
                    case AudioSource.Cloud:
                        break;
                    case AudioSource.Net:
                        break;
                    case AudioSource.FM1:
                        break;
                    case AudioSource.FM2:
                        break;
                    case AudioSource.iPod:
                        break;
                    case AudioSource.CD:
                        break;
                    case AudioSource.AUX:
                        code = new byte[] { 0xa3, 0x00, 0x0c, 0x00, 0x0c };
                        break;
                    default:
                        break;
                }
                this.SendData(code);
                this._Zoom1Source = value;
            }
        }
        public void Zoom0VolDown()
        {
            byte[] code = { 0xa3, 0x00, 0x08, 0x00, 0x08 };
            this.SendData(code);
        }
        public void Zoom0VolUp()
        {
            byte[] code = { 0xa3, 0x00, 0x01, 0x00, 0x01 };
            this.SendData(code);
        }
        public void Zoom0SetVol(byte vol)
        {
            byte[] code = { 0xc0, 0x00, 0x00, vol, vol };
            this.SendData(code);
        }
        #endregion
        #region 区域一
        private bool _Zoom1On = false;
        /// <summary>
        /// 区域1 true：开 false：关闭
        /// </summary>
        public bool Zoom1On
        {
            get
            {
                return this._Zoom1On;
            }
            set
            {
                if (value == true)
                {
                    byte[] code = { 0xa3, 0x01, 0x07, 0x00, 0x06 };
                    this.SendData(code);
                }
                else
                {
                    byte[] code = { 0xa3, 0x01, 0x03, 0x00, 0x02 };
                    this.SendData(code);
                }
            }
        }
        private AudioSource _Zoom1Source = AudioSource.AUX;
        public AudioSource Zoom1Status
        {
            get
            {
                return this._Zoom1Source;
            }
            set
            {
                byte[] code = null;

                switch (value)
                {
                    case AudioSource.MP3:
                        break;
                    case AudioSource.Cloud:
                        break;
                    case AudioSource.Net:
                        break;
                    case AudioSource.FM1:
                        break;
                    case AudioSource.FM2:
                        break;
                    case AudioSource.iPod:
                        break;
                    case AudioSource.CD:
                        break;
                    case AudioSource.AUX:
                        code = new byte[] { 0xa3, 0x01, 0x0c, 0x00, 0x0d };
                        break;
                    default:
                        break;
                }
                this.SendData(code);
                this._Zoom1Source = value;
            }
        }
        public void Zoom1VolDown()
        {
            byte[] code = { 0xa3, 0x01, 0x08, 0x00, 0x09 };
            this.SendData(code);
        }
        public void Zoom1VolUp()
        {
            byte[] code = { 0xa3, 0x01, 0x01, 0x00, 0x00 };
            this.SendData(code);
        }
        public void Zoom1SetVol(byte vol)
        {
            int check = vol ^ 0x01;
            byte[] code = { 0xc0, 0x01, 0x00, vol, (byte)check };
            this.SendData(code);
        }
        #endregion
        #region 区域二
        private bool _Zoom2On = false;
        public bool Zoom2On
        {
            get
            {
                return this._Zoom2On;
            }
            set
            {
                if (value == true)
                {
                    byte[] code = { 0xa3, 0x02, 0x07, 0x00, 0x05 };
                    this.SendData(code);
                }
                else
                {
                    byte[] code = { 0xa3, 0x02, 0x03, 0x00, 0x01 };
                    this.SendData(code);
                }
            }
        }

        private AudioSource _Zoom2Source = AudioSource.AUX;
        public AudioSource Zoom2Status
        {
            get
            {
                return this._Zoom2Source;
            }
            set
            {
                byte[] code = null;

                switch (value)
                {
                    case AudioSource.MP3:
                        break;
                    case AudioSource.Cloud:
                        break;
                    case AudioSource.Net:
                        break;
                    case AudioSource.FM1:
                        break;
                    case AudioSource.FM2:
                        break;
                    case AudioSource.iPod:
                        break;
                    case AudioSource.CD:
                        break;
                    case AudioSource.AUX:
                        code = new byte[] { 0xa3, 0x02, 0x0c, 0x00, 0x0e };
                        break;
                    default:
                        break;
                }
                this.SendData(code);
                this._Zoom2Source = value;
            }
        }
        public void Zoom2SetVol(byte vol)
        {
            int check = vol ^ 0x02;
            byte[] code = { 0xc0, 0x02, 0x00, vol, (byte)check };
            this.SendData(code);
        }
        public void Zoom2VolDown()
        {
            byte[] code = { 0xa3, 0x02, 0x08, 0x00, 0x0a };
            this.SendData(code);
        }
        public void Zoom2VolUp()
        {
            byte[] code = { 0xa3, 0x02, 0x01, 0x00, 0x03 };
            this.SendData(code);
        }
        #endregion
        #region 区域三
        private bool _Zoom3On = false;
        public bool Zoom3On
        {
            get
            {
                return this._Zoom3On;
            }
            set
            {
                if (value == true)
                {
                    byte[] code = { 0xa3, 0x03, 0x07, 0x00, 0x04 };
                    this.SendData(code);
                }
                else
                {
                    byte[] code = { 0xa3, 0x03, 0x03, 0x00, 0x00 };
                    this.SendData(code);
                }
            }
        }
        private AudioSource _Zoom3Source = AudioSource.AUX;
        public AudioSource Zoom3Status
        {
            get
            {
                return this._Zoom3Source;
            }
            set
            {
                byte[] code = null;

                switch (value)
                {
                    case AudioSource.MP3:
                        break;
                    case AudioSource.Cloud:
                        break;
                    case AudioSource.Net:
                        break;
                    case AudioSource.FM1:
                        break;
                    case AudioSource.FM2:
                        break;
                    case AudioSource.iPod:
                        break;
                    case AudioSource.CD:
                        break;
                    case AudioSource.AUX:
                        code = new byte[] { 0xa3, 0x03, 0x0c, 0x00, 0x0f };
                        break;
                    default:
                        break;
                }
                this.SendData(code);
                this._Zoom3Source = value;
            }
        }
        public void Zoom3VolDown()
        {
            byte[] code = { 0xa3, 0x03, 0x08, 0x00, 0x0b };
            this.SendData(code);
        }
        public void Zoom3VolUp()
        {
            byte[] code = { 0xa3, 0x03, 0x01, 0x00, 0x02 };
            this.SendData(code);
        }
        public void Zoom3SetVol(byte vol)
        {
            int check = vol ^ 0x03;
            byte[] code = { 0xc0, 0x03, 0x00, vol, (byte)check };
            this.SendData(code);
        }
        #endregion
        #region 区域四
        private bool _Zoom4On = false;
        public bool Zoom4On
        {
            get
            {
                return this._Zoom4On;
            }
            set
            {
                if (value == true)
                {
                    byte[] code = { 0xa3, 0x04, 0x07, 0x00, 0x03 };
                    this.SendData(code);
                }
                else
                {
                    byte[] code = { 0xa3, 0x04, 0x03, 0x00, 0x07 };
                    this.SendData(code);
                }
            }
        }
        private AudioSource _Zoom4Source = AudioSource.AUX;
        public AudioSource Zoom4Status
        {
            get
            {
                return this._Zoom4Source;
            }
            set
            {
                byte[] code = null;

                switch (value)
                {
                    case AudioSource.MP3:
                        break;
                    case AudioSource.Cloud:
                        break;
                    case AudioSource.Net:
                        break;
                    case AudioSource.FM1:
                        break;
                    case AudioSource.FM2:
                        break;
                    case AudioSource.iPod:
                        break;
                    case AudioSource.CD:
                        break;
                    case AudioSource.AUX:
                        code = new byte[] { 0xa3, 0x04, 0x0c, 0x00, 0x08 };
                        break;
                    default:
                        break;
                }
                this.SendData(code);
                this._Zoom4Source = value;
            }
        }
        public void Zoom4VolDown()
        {
            byte[] code = { 0xa3, 0x04, 0x08, 0x00, 0x0c };
            this.SendData(code);
        }
        public void Zoom4VolUp()
        {
            byte[] code = { 0xa3, 0x00, 0x01, 0x00, 0x05 };
            this.SendData(code);
        }
        public void Zoom4SetVol(byte vol)
        {
            int check = vol ^ 0x04;
            byte[] code = { 0xc0, 0x04, 0x00, vol, (byte)check };
            this.SendData(code);
        }
        #endregion

        public BackAudio(string host,int port)
        {

            server.EnableUDPServer(host, 6001, port);
        }

        #region 函数
        private void SendData(byte[] sendbytes)
        {
            server.SendData(sendbytes, sendbytes.Length);

        }
        private byte[] RecevedData()
        {
            byte[] rbytes = { };
            if (server.ReceiveData() > 0)
            {
                rbytes = server.IncomingDataBuffer;
            }
            return rbytes;
        }

        public void DisConnect()
        {
            server.DisableUDPServer();
        }
        #endregion

    }
}