using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;

namespace ILiveSmart
{

    /// <summary>
    /// IRACC空调网关
    /// </summary>
    public class ILiveIRACC
    {
                public delegate void Push16IHandler(int id, bool iChanStatus);
        public event Push16IHandler Push16IEvent;

        public ComPort comDaHua;
        public ILiveIRACC(ComPort com)
        {
            #region 注册串口
            comDaHua = com;
            comDaHua.SerialDataReceived += new ComPortDataReceivedEvent(comDaHua_SerialDataReceived);
            if (!comDaHua.Registered)
            {
                if (comDaHua.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                    ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", comDaHua.DeviceRegistrationFailureReason);
                if (comDaHua.Registered)
                    comDaHua.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate9600,
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
        public void SendIRACCPower(bool on, IRACCFL fl)
        {
            byte dd = 0x60;

            if (on)
            {
                dd = 0x61;
            }
            this.SendIRACC(0x01, 0x06, 0x07, 0xD0, (byte)fl, dd);//00
        }
        public void SendIRACCPower(int group, bool on, IRACCFL fl)
        {

            byte dd = 0x60;
            byte gp = (byte)(208 + (group * 3));
            if (on)
            {
                dd = 0x61;
            }
            this.SendIRACC(0x01, 0x06, 0x07, gp, (byte)fl, dd);//00
        }
        public void SendIRACCSetMode(int group, IRACCMode mode)
        {
            byte gp = (byte)(208 + (group * 3) + 1);

            this.SendIRACC(0x01, 0x06, 0x07, gp,0x00,(byte)mode);//00
        }
        public void SendIRACCTemp(int group, int wendu)
        {
            byte gp = (byte)(208 + (group * 3) + 2);

            byte[] shi = System.BitConverter.GetBytes(wendu * 10);

            this.SendIRACC(0x01, 0x06, 0x07, gp, shi[1], shi[0]);//16

        }

        //public void CanTing(bool onoroff, int fengliang)
        //{
        //    if (onoroff)
        //    {
        //        switch (fengliang)
        //        {
        //            case 1://LL开机
        //              //  this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x10, 0x61, 0x45, 0x6F);//00
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x10, 0x61);//00

        //                break;
        //            case 2://L开机
        //               // this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x20, 0x61, 0x51, 0x6F);//00
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x20, 0x61);//00

        //                break;
        //            case 3://M开机
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x30, 0x61, 0x5C, 0xAF);//00

        //                break;
        //            case 4://H开机
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x40, 0x61, 0x79, 0x6F);//00

        //                break;
        //            case 5://HH开机
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x50, 0x61, 0x74, 0xAF);//00
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (fengliang)
        //        {
        //            case 1://LL关机
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x10, 0x60, 0x84, 0xAF);//00

        //                break;
        //            case 2://L关机
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x20, 0x60, 0x90, 0xAF);//00

        //                break;
        //            case 3://M关机
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x30, 0x60, 0x9D, 0x6F);//00

        //                break;
        //            case 4://H关机
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x40, 0x60, 0xB8, 0xAF);//00

        //                break;
        //            case 5://HH关机
        //                this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x50, 0x60, 0xB5, 0x6F);//00

        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //}


        //public void CanTingSetMode(int mode)
        //{
        //    switch (mode)
        //    {
        //        case 1://通风
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD1, 0x00, 0x00, 0xD8, 0x87);//00

        //            break;
        //        case 2://制热
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD1, 0x00, 0x01, 0x19, 0x47);//00

        //            break;
        //        case 3://制冷
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD1, 0x00, 0x02, 0x59, 0x46);//00

        //            break;
        //        case 4://自动
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD1, 0x00, 0x03, 0x98, 0x86);//00

        //            break;
        //        case 5://温度点设定
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD1, 0x00, 0x04, 0x58, 0x85);//00

        //            break;
        //        case 6://除湿
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD0, 0x00, 0x05, 0xB5, 0x6F);//00

        //            break;
        //        default:
        //            break;
        //    }
        //}
        ///// <summary>
        ///// 设置温度16-32
        ///// </summary>
        ///// <param name="wendu"></param>
        //public void CanTingSetTemp(int wendu)
        //{
        //    switch (wendu)
        //    {
        //        case 16:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xA0, 0x28, 0xFF);//16

        //            break;
        //        case 17:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xAA, 0xA8, 0xF8);//17
        //            break;
        //        case 18:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xB4, 0x28, 0xF0);//18
        //            break;
        //        case 19:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xBE, 0xA8, 0xF7);//19
        //            break;
        //        case 20:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xC8, 0x29, 0x11);//20
        //            break;
        //        case 21:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xD2, 0xA8, 0xDA);//21
        //            break;
        //        case 22:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xDC, 0x29, 0x1E);//22
        //            break;
        //        case 23:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xE6, 0xA9, 0x0D);//23
        //            break;
        //        case 24:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xF0, 0x28, 0xC3);//24
        //            break;
        //        case 25:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0xFA, 0xA8, 0xC4);//25
        //            break;
        //        case 26:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x01, 0x04, 0x28, 0xD4);//26
        //            break;
        //        case 27:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x01, 0x0E, 0xA8, 0xD3);//27
        //            break;
        //        case 28:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x01, 0x18, 0x29, 0x1D);//28
        //            break;
        //        case 29:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x01, 0x22, 0xA9, 0x01);//29
        //            break;
        //        case 30:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0x2C, 0x28, 0xCA);//30
        //            break;
        //        case 31:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0x36, 0xA9, 0x01);//31
        //            break;
        //        case 32:
        //            this.SendIRACC(0x01, 0x06, 0x07, 0xD2, 0x00, 0x40, 0x28, 0xE7);//32
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //#endregion
        //#region 次卧

        //#endregion
        //#region 客厅

        //#endregion
        //public void TempOn()
        //{
        //    //Thread.Sleep(1000);
        //    //this.comDaHua.Send(this.GetCMDString(0x01, 0x06, 0x07, 0xD3, 0x50, 0x61, 0x84, 0xAF));//00

        //}
        //public void TempOff()
        //{
        // //   this.comDaHua.Send(this.GetCMDString(0x01, 0x06, 0x07, 0xD0, 0x50, 0x60, 0xB5, 0x6F));//00

        //}

        private void SendIRACC(params byte[] data)
        {

            string senddata = this.GetCMDString(data);
            this.comDaHua.Send(senddata);
        }
        private string GetCMDString(params byte[] senddata)
        {


            byte[] crc = this.Crc_16(senddata);

            byte[] sendBytes = this.copybyte(senddata, crc);
            ILiveDebug.Instance.WriteLine("IRACCSendData:" + ILiveUtil.ToHexString(sendBytes));

            //label1.Text += this.ToHexString(sendBytes) + "\r\n";
            //  ILiveDebug.Instance.WriteLine("iracc:" + ILiveUtil.ToHexString(sendBytes));
            return Encoding.GetEncoding(28591).GetString(sendBytes, 0, sendBytes.Length);
        }
        private byte[] Crc_16(byte[] source)
        {
            byte[] ret = new byte[2];

            int CRC = 0xFFFF;//set all 1

            if (source.Length <= 0)
                CRC = 0;
            else
            {
                foreach (var item in source)
                {
                    CRC = CRC ^ (int)item;
                    for (int i = 0; i <= 7; i++)
                    {
                        if ((CRC & 1) != 0)
                            CRC = (CRC >> 1) ^ 0xA001;
                        else
                            CRC = CRC >> 1;    //
                    }
                }

                ret[1] = (byte)((CRC & 0xff00) >> 8);//高位置
                ret[0] = (byte)(CRC & 0x00ff);  //低位置
            }
            return ret;
        }
        private byte[] copybyte(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }
        void comDaHua_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            //int exeid = 0;

            byte[] sendBytes = Encoding.ASCII.GetBytes(args.SerialData);
             ILiveDebug.Instance.WriteLine("IRACCReturnData:"+ILiveUtil.ToHexString(sendBytes));
        }
    }

    public enum IRACCFL
    {
        /// <summary>
        /// 
        /// </summary>
        LL = 0x10,
        L = 0x20,
        M = 0x30,
        H = 0x40,
        HH = 0x50
    }
    public enum IRACCMode
    {
        /// <summary>
        /// 通风
        /// </summary>
        TF = 0x00,
        /// <summary>
        /// 制热
        /// </summary>
        ZR = 0x01,
        /// <summary>
        /// 制冷
        /// </summary>
        ZL = 0x02,
        /// <summary>
        /// 自动
        /// </summary>
        ZD = 0x03,
        /// <summary>
        /// 温度点
        /// </summary>
        WD = 0x04,
        /// <summary>
        /// 除湿
        /// </summary>
        CS = 05
    }

}