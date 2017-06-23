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
        /// <summary>
        /// 因为接的是串口服务器 所以用网络命令
        /// </summary>
        public UDPServer server = new UDPServer();
        public BackAudio(string host, int port)
        {
            server.EnableUDPServer(host, 6001, port);
        }

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
        #region 区域0 客厅
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
        public void Zoom0Mute(bool mute)
        {
            this.AudioMute(0x00, mute);
        }
        #endregion
        #region 区域一 卧室
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
        public void Zoom1Mute(bool mute)
        {
            this.AudioMute(0x01, mute);
        }
        #endregion
        #region 区域二 书房
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
        public void Zoom2Mute(bool mute)
        {
            this.AudioMute(0x02, mute);
        }
        #endregion
        #region 区域三 卫生间
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
        public void Zoom3Mute(bool mute)
        {
            this.AudioMute(0x03, mute);
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
        public void Zoom4Mute(bool mute)
        {
            this.AudioMute(0x04, mute);
        }
        #endregion

        #region 函数
        /// <summary>
        /// 开关机
        /// </summary>
        /// <param name="zone">终端号放高四位，通道编号放低4位</param>
        /// <param name="cmd">命令标志:0x01音量+ 0x03关机 0x07开机 0x08音量-</param>
        /// <param name="source">音源</param>
        private void SetPower(byte zone,byte cmd,byte p)
        {
            byte[] code = { 0xa3, zone, cmd, p };
            this.SendDataCheck(code);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone">终端号放高四位，通道编号放低4位</param>
        /// <param name="cmd">命令标志:0x01音量+ 0x08音量-</param>
        /// <param name="source">音量 音量数值（0-31之间）</param>
        private void SetAudioVol(byte zone, byte cmd, byte p)
        {
            byte[] code = { 0xa3, zone, cmd, p };
            this.SendData(code);
        }
        private void SetAudioVol(byte zone,byte vol)
        {
            byte[] code = { 0xc0, zone, 0x00, vol };
            this.SendData(code);
        }
        private void AudioMute(byte zone,bool mute)
        {
            if (mute)
            {
                this.SendDataCheck(new byte[] { 0xab, zone, 0x01, 0x00 });
            }
            else
            {
                this.SendDataCheck(new byte[] { 0xab, zone, 0x00, 0x00 });
            }

        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendbytes"></param>
        private void SendDataCheck(byte[] sendbytes)
        {
            /*
             1.协议的第一个字节小于等于0xe0时，协议的整体长度（包含最后的异或字节）为5个字节长度，第一个字节大于0xe0时，协议的整体长度为协议接收的第三个字节的值。
            2.当协议长度为5个字节时，因为无协议长度信息，所以直接从第二个字节校验到第四个字节。
            3.当协议长度大于5个字节时，先将第三个字节的长度信息的值-1，修改为实际的数
             */
            if (sendbytes!=null&&sendbytes.Length>0)
            {
                byte b0 = sendbytes[0];

                if (b0>0xe0)
                {
                    //当协议长度大于5个字节时，先将第三个字节的长度信息的值-1，修改为实际的数
                   // byte[] data = { sendbytes[0], sendbytes[1], sendbytes[2], sendbytes[3], (byte)(sendbytes[1] + sendbytes[2] + sendbytes[3]) };
                  //  this.SendData(data);
                }
                else
                {
                    byte[] data = { sendbytes[0], sendbytes[1], sendbytes[2], sendbytes[3], (byte)(sendbytes[1] + sendbytes[2] + sendbytes[3]) };
                    this.SendData(data);
                }
            }
        }
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