using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;

namespace ILiveSmart
{
    /// <summary>
    /// 奥斯迪DM838背景音乐
    /// </summary>
    public class ILiveDM838
    {
        public UDPServer server = new UDPServer();

       // string ip = "192.168.0.1";
        public ILiveDM838(string ip)
        {
            //this.ip = ip;
            try
            {
                server.EnableUDPServer(ip, 0, 40188);
            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine(ex.Message);
            }
        }
        public void DisConnect()
        {
            server.DisableUDPServer();
        }
        public void SendCMD(string cmd)
        {
            switch (cmd)
            {
                case "PowerOn":
                    this.SendData(new byte[] { 0x00, 0x06, 0x00, 0x00, 0x71, 0x00, 0x20, 0x01, 0x02, 0x08, 0x01, 0x09 });
                    break;
                case "PowerOff":
                    this.SendData(new byte[] { 0x00, 0x06, 0x00, 0x00, 0x71, 0x00, 0x20, 0x01, 0x02, 0x08, 0x00, 0x08 });
                    break;
                case "SourceLocal":
                    this.SendData(new byte[] { 0x00,0x02,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0x81,0x89 });
                    break;
                case "SourceNet":
                    this.SendData(new byte[] {0x00,0x02,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0xD1,0xD9 });

                    break;
                case "SourceRadio":
                    this.SendData(new byte[] { 0x00,0x02,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0xC1,0xC9 });

                    break;
                case "SourceAux":
                    this.SendData(new byte[] { 00,0x02,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0x51,0x59 });

                    break;
                case "SourceBlue":
                    this.SendData(new byte[] { 00,0x02,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0xA1,0xA9 });

                    break;
                case "Mute":
                    this.SendData(new byte[] { 0x00,0x07,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0x10,0x18 });

                    break;
                case "MuteOff":
                    this.SendData(new byte[] {0x00,0x07,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0x01,0x09 });

                    break;
                case "VolAdd":
                    this.SendData(new byte[] {0x00,0x31,0x00,0x00,0x71,0x00,0x20,0x01,0x01,0x01,0x01 });

                    break;
                case "VolSub":
                    this.SendData(new byte[] { 0x00,0x31,0x00,0x00,0x71,0x00,0x20,0x01,0x01,0x00,0x00 });

                    break;
                case "Pause":
                    this.SendData(new byte[] { 0x00,0x05,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0x02,0x0A });

                    break;
                case "Play":
                    this.SendData(new byte[] { 0x00,0x05,0x00,0x00,0x71,0x00,0x20,0x01,0x02,0x08,0x01,0x09 });

                    break;
                case "Prev":
                    this.SendData(new byte[] { 0x00,0x0A ,0x00,0x00,0x71,0x00,0x20,0x01,0x01,0x01,0x01 });

                    break;
                case "Next":
                    this.SendData(new byte[] { 00,0x0A,0x00,0x00,0x71,0x00,0x20,0x01,0x01,0x10,0x10 });

                    break;
                default:
                    break;
            }
        }
        private void SendData(byte[] sendbytes)
        {
            //ILiveDebug.Instance.WriteLine("DM838"+server.AddressToAcceptConnectionFrom+":" + ILiveUtil.ToHexString(sendbytes));
            server.SendData(sendbytes, sendbytes.Length);
        }
        public byte[] RecevedData()
        {
            byte[] rbytes = { };
            if (server.ReceiveData() > 0)
            {
                rbytes = server.IncomingDataBuffer;
            }
            return rbytes;
        }
    }
}