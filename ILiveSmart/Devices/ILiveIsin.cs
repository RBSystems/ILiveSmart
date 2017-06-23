using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;

namespace SenSmart.Exec
{
    /// <summary>
    /// 爱联继电器
    /// </summary>
    public class ILiveIsin
    {
        public ILiveIsinStatus Status = new ILiveIsinStatus();

        public ComPort comIsin;
        int addr = 0;
        public ILiveIsin(int addr, ComPort com)
            : this(com)
        {
            this.addr = addr;
        }

        public ILiveIsin(ComPort com)
        {
            #region 注册串口
            comIsin = com;
            comIsin.SerialDataReceived += new ComPortDataReceivedEvent(comIsin_SerialDataReceived);
            if (!comIsin.Registered)
            {
                if (comIsin.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                    ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", comIsin.DeviceRegistrationFailureReason);
                if (comIsin.Registered)
                    comIsin.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate9600,
                                                                     ComPort.eComDataBits.ComspecDataBits8,
                                                                     ComPort.eComParityType.ComspecParityNone,
                                                                     ComPort.eComStopBits.ComspecStopBits1,
                                         ComPort.eComProtocolType.ComspecProtocolRS485,
                                         ComPort.eComHardwareHandshakeType.ComspecHardwareHandshakeNone,
                                         ComPort.eComSoftwareHandshakeType.ComspecSoftwareHandshakeNone,
                                         false);
            }
            #endregion
        }
        void comIsin_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            byte[] sendBytes = Encoding.GetEncoding(28591).GetBytes(args.SerialData);
         //   ILiveDebug.Instance.WriteLine(this.addr+"IsinReceived" + BitConverter.ToString(sendBytes, 0, sendBytes.Length));
        }


        public void RelayOpen()
        {
            this.Status.Relay0 = true;
            this.Status.Relay1 = true;
            this.Status.Relay2 = true;
            this.Status.Relay3 = true;
            this.Status.Relay4 = true;
            this.Status.Relay5 = true;
            this.Status.Relay6 = true;
            this.Status.Relay7 = true;
            this.Status.Relay8 = true;
            this.Status.Relay9 = true;
            this.Status.Relay10 = true;
            this.Status.Relay11 = true;
            this.Relay8SW8(addr, 255, 15, true);
            Thread.Sleep(1000);
            // this.Relay8SW8(4, 127, 15, false);
            // Thread.Sleep(1000);
            // this.Relay8SW8(5, 255, 15, false);
        }
        public void RelayClose()
        {
            this.Status.Relay0 = false;
            this.Status.Relay1 = false;
            this.Status.Relay2 = false;
            this.Status.Relay3 = false;
            this.Status.Relay4 = false;
            this.Status.Relay5 = false;
            this.Status.Relay6 = false;
            this.Status.Relay7 = false;
            this.Status.Relay8 = false;
            this.Status.Relay9 = false;
            this.Status.Relay10 = false;
            this.Status.Relay11 = false;

            this.Relay8SW8(addr, 255, 15, false);
            Thread.Sleep(1000);
            // this.Relay8SW8(4, 127, 15, true);
            // Thread.Sleep(1000);
            //  this.Relay8SW8(5, 255, 15, true);
        }
        public void Relay8SW8(int port1, int port2, bool states)
        {
            this.Relay8SW8(this.addr, port1, port2, states);
        }
        private void Relay8SW8(int address, int port1, int port2, bool states)
        {
            //for (int i = 0; i < 12; i++)
            //{
            //    int b = (port1 >> i) & 1;
            //    bool state = b == 1 ? true : false;

            //    switch (i)
            //    {
            //        case 0:
            //            this.Status.Relay0 =state;
            //            break;
            //        case 1:
            //            this.Status.Relay1 = state;
            //            break;
            //        case 2:
            //            this.Status.Relay2 = state;
            //            break;
            //        case 3:
            //            this.Status.Relay3 = state;
            //            break;
            //        case 4:
            //            this.Status.Relay4 = state;
            //            break;
            //        case 5:
            //            this.Status.Relay5 = state;
            //            break;
            //        case 6:
            //            this.Status.Relay6 = state;
            //            break;
            //        case 7:
            //            this.Status.Relay7 = state;
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //for (int i = 0; i < 4; i++)
            //{
            //    int b = (port2 >> i) & 1;
            //    bool state = b == 1 ? true : false;

            //    switch (i)
            //    {
            //        case 0:
            //            this.Status.Relay8 = state;
            //            break;
            //        case 1:
            //            this.Status.Relay9 = state;
            //            break;
            //        case 2:
            //            this.Status.Relay10 = state;
            //            break;
            //        case 3:
            //            this.Status.Relay11 = state;
            //            break;
            //        default:
            //            break;
            //    }
            //}
            /*
             * 1 B2 协议头 
             * 2 00 设备类型 
             * 3 1-99 设备编号
             * 4 A1 功能
             * 5 00-FF 
             *  00 选中 0 路 
             *  FF 选中 8 路 
             *  通道 5－12
             * 6
             *  00-FF 
             *  00 选中 0 路 
             *  FF 选中 8 路
             *  通道 1－4
             * 7 00 选中通道关 01 选中通道开 02 选中通道反 通道状态
             * 8 Check 校验和高位（2-7 位校验） 
             * 9 Check 校验和低位 10 2B 协议尾 
             */
            // byte[] checkarr1 = new byte[2];
            // this.ConvertIntToByteArray(address + port1 + port1 + 162, ref checkarr1);
            byte[] checkarr1 = BitConverter.GetBytes(address + port1 + port2 + 162);
            byte[] checkarr2 = BitConverter.GetBytes(address + port1 + port2 + 161);
            // this.ConvertIntToByteArray(address + port1 + port1 + 161, ref checkarr2);

            byte[] sendBytes = new byte[] { 0xB2, 0x00, (byte)address, 0xA1, (byte)port1, (byte)port2, 0x00, checkarr2[1], checkarr2[0], 0x2B };
            if (states)
            {
                sendBytes = new byte[] { 0xB2, 0x00, (byte)address, 0xA1, (byte)port1, (byte)port2, 0x01, checkarr1[1], checkarr1[0], 0x2B };
            }

            string cmd = Encoding.GetEncoding(28591).GetString(sendBytes, 0, sendBytes.Length);

            this.comIsin.Send(cmd);
            Thread.Sleep(500);

        }
    }
    public class ILiveIsinStatus
    {
        public bool Relay0 = false;
        public bool Relay1 = false;
        public bool Relay2 = false;
        public bool Relay3 = false;
        public bool Relay4 = false;
        public bool Relay5 = false;
        public bool Relay6 = false;
        public bool Relay7 = false;
        public bool Relay8 = false;
        public bool Relay9 = false;
        public bool Relay10 = false;
        public bool Relay11 = false;
    }
}
