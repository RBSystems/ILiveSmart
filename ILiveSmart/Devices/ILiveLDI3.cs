using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace ILiveSmart
{
    /// <summary>
    /// 朗大背景音乐 I3
    /// </summary>
    public class ILiveLDI3
    {
        public ComPort comMusicI3;
        public ILiveLDI3(ComPort com)
        {
            #region 注册串口
            comMusicI3 = com;
            comMusicI3.SerialDataReceived += new ComPortDataReceivedEvent(comMusicI3_SerialDataReceived);
            if (!comMusicI3.Registered)
            {
                if (comMusicI3.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                    ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", comMusicI3.DeviceRegistrationFailureReason);
                if (comMusicI3.Registered)
                    comMusicI3.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate9600,
                                                                     ComPort.eComDataBits.ComspecDataBits8,
                                                                     ComPort.eComParityType.ComspecParityNone,
                                                                     ComPort.eComStopBits.ComspecStopBits1,
                                         ComPort.eComProtocolType.ComspecProtocolRS232,
                                         ComPort.eComHardwareHandshakeType.ComspecHardwareHandshakeNone,
                                         ComPort.eComSoftwareHandshakeType.ComspecSoftwareHandshakeNone,
                                         false);
            }
            #endregion
        }
        public void SendCMD(string cmd)
        {
            //数据头（0xFA） 房间号 指令码 组ID（0x00） 效验码 结束符（0xFE）
            switch (cmd)
            {
                case "PowerOn":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x51, 0x00, 0x4C, 0xFE});
                    break;
                case "PowerOff":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x52, 0x00, 0x4D, 0xFE });
                    break;
                case "SourceTF":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x70, 0x00, 0x6B, 0xFE });
                    break;
                case "SourceU":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x71, 0x00, 0x6C, 0xFE });

                    break;
                case "SourceRadio":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x76, 0x00, 0x71, 0xFE });

                    break;
                case "SourceAux":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x77, 0x00, 0x72, 0xFE });

                    break;
                case "SourceBlue":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x78, 0x00, 0x73, 0xFE });

                    break;
                case "Mute":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x43, 0x00, 0x3E, 0xFE });

                    break;
                case "MuteOff":
                   // this.SendData(new byte[] { 0xFA, 0x01, 0x52, 0x00, 0x71, 0x4D, 0xFE });
                    break;
                case "VolAdd":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x41, 0x00, 0x3C, 0xFE });
                    break;
                case "VolSub":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x42, 0x00, 0x3D, 0xFE });
                    break;
                case "Pause":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x38, 0x00, 0x33, 0xFE });
                    break;
                case "Play":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x37, 0x00, 0x32, 0xFE });
                    break;
                case "Prev":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x31, 0x00, 0x2C, 0xFE });
                    break;
                case "Next":
                    this.SendData(new byte[] { 0xFA, 0x01, 0x32, 0x00, 0x2D, 0xFE });
                    break;
                default:
                    break;
            }
        }
        private void SendData(byte[] sendbytes)
        {
            ILiveDebug.Instance.WriteLine("I3ComSendData"+ILiveUtil.ToHexString(sendbytes));
            string cmd = Encoding.GetEncoding(28591).GetString(sendbytes, 0, sendbytes.Length);

            this.comMusicI3.Send(cmd);
        }
        void comMusicI3_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            //int exeid = 0;

             //byte[] sendBytes = Encoding.ASCII.GetBytes(args.SerialData);
             // ILiveDebug.Instance.WriteLine("485Data:"+ILiveUtil.ToHexString(sendBytes));
        }
    }
}