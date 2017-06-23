using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;

namespace ILiveSmart.Music
{
    /// <summary>
    /// 串口播放器
    /// </summary>
    public class MusicPlayer
    {
        public UDPServer server = new UDPServer();
        public MusicPlayer(string host, int port)
        {

            server.EnableUDPServer(host, 6002, port);
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
          /// <summary>
        /// 播放爵士乐
        /// </summary>
        public void PlayJueShi()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x17, 0x00, 0x00, 0x02, 0xFE, 0xE2, 0xEF };
           this.SendData(code);
        }
        /// <summary>
        /// 播放钢琴曲
        /// </summary>
        public void PlayGangQing()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x17, 0x00, 0x00, 0x01, 0xFE, 0xE3, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 播放乡村音乐
        /// </summary>
        public void PlayXiangCun()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x17, 0x00, 0x00, 0x03, 0xFE, 0xE1, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 开始播放
        /// </summary>
        public void PlayStart()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x07, 0x00, 0x00, 0x00, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 暂停播放
        /// </summary>
        public void PlayPause()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x0E, 0x00, 0x00, 0x00, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        public void PlayStop()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x16, 0x00, 0x00, 0x00, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 通知保姆 4秒
        /// </summary>
        public void PlayTongZhiBaoMu()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x13, 0x00, 0x00, 0x01, 0xFE, 0xE7, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 主卧室有人需要帮助3秒
        /// </summary>
        public void PlayBaoJing()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x13, 0x00, 0x00, 0x02, 0xFE, 0xE6, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 非法入侵 4秒
        /// </summary>
        public void PlayRuQing()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x13, 0x00, 0x00, 0x03, 0xFE, 0xE5, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 欢迎回家3秒
        /// </summary>
        public void PlayWelcome()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x13, 0x00, 0x00, 0x04, 0xFE, 0xE4, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 安防启动 2秒
        /// </summary>
        public void PlayAnFangStart()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x13, 0x00, 0x00, 0x05, 0xFE, 0xE3, 0xEF };
            this.SendData(code);
        }
        /// <summary>
        /// 安防撤销3秒
        /// </summary>
        public void PlayAnFangEnd()
        {
            byte[] code = { 0x7E, 0xFF, 0x06, 0x13, 0x00, 0x00, 0x06, 0xFE, 0xE2, 0xEF };
            this.SendData(code);
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
    }
}