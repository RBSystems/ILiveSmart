using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace ILiveSmart
{
    /// <summary>
    /// 耶鲁指纹锁 所有设备都是大写D开头
    /// </summary>
    public class DYelaLock
    {
        public ComPort comYelaLock;

        public delegate void YelaPressHandler(int button);

        public event YelaPressHandler YelaPressEvent;

        byte[] yelaLock;

        public DYelaLock(ComPort c)
        {
            #region 注册串口
            comYelaLock = c;
            comYelaLock.SerialDataReceived += new ComPortDataReceivedEvent(comYelaLock_SerialDataReceived);
            if (!comYelaLock.Registered)
            {
                if (comYelaLock.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                    ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", comYelaLock.DeviceRegistrationFailureReason);
                if (comYelaLock.Registered)
                    comYelaLock.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate19200,
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
        void comYelaLock_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {

            byte[] receivedBytes = Encoding.GetEncoding(28591).GetBytes(args.SerialData);
            if (receivedBytes != null && receivedBytes.Length > 0)
            {
                foreach (var item in receivedBytes)
                {
                    if (item == 0x0F)
                    {
                        try
                        {
                            this.YelaProcess(yelaLock);
                        }
                        catch (Exception)
                        {
                        }


                        yelaLock = null;
                    }
                    else
                    {
                        yelaLock = ILiveUtil.AddByteToBytes(yelaLock, item);
                    }
                }
            }
        }

        public void YelaProcess(byte[] yeladata)
        {
            ILiveDebug.Instance.WriteLine(ILiveUtil.ToHexString(yeladata));
            if (yeladata != null && yeladata.Length > 4)
            {
                if (yeladata[1] == 0x19)
                {
                    if (yeladata[3] == 0x23 || yeladata[2] == 0x81)
                    {
                        //门已经打开
                    }
                    else
                    {
                        //门已经关闭
                    }
                    if (yeladata[2] == 0x81)
                    {
                        this.YelaButtonPress(yeladata[4]);
                        //            honur=receivedBytes[4];
                        //            Admin[honur]=1;
                        //            delay(100);
                        //            Admin[honur]=0;
                    }
                }
            }
            // yelaLock = null;
        }
        public void YelaButtonPress(int i)
        {
            this.YelaPressEvent(i);

        }

        /// <summary>
        /// 开门
        /// </summary>
        public void OpenDoor()
        {
            byte[] b = { 0x05, 0x91, 0x02, 0x11, 0x82, 0x0F };
            this.SendYela(b);
        }
        /// <summary>
        /// 关门
        /// </summary>
        public void CloseDoor()
        {
            byte[] b = { 0x05, 0x91, 0x02, 0x12, 0x81, 0x0F };

            this.SendYela(b);
        }

        public void SendYela(byte[] data)
        {
            string senddata = Encoding.GetEncoding(28591).GetString(data, 0, data.Length);
            this.comYelaLock.Send(senddata);
        }
    }
}