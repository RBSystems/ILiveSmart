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
    /// 聪普8路继电器
    /// </summary>
    public class ILiveCongPu
    {
        public CongPu8SW8Status Status = new CongPu8SW8Status();

        public delegate void Push16IHandler(int id, bool iChanStatus);

        public event Push16IHandler Push16IEvent;

        public ComPort comCongPu;
        int addr = 0;
        public ILiveCongPu(int addr,ComPort com):this(com)
        {
            this.addr = addr;
        }
        public ILiveCongPu(ComPort com)
        {
            #region 注册串口
                comCongPu = com;
                comCongPu.SerialDataReceived += new ComPortDataReceivedEvent(comCongPu_SerialDataReceived);
                if (!comCongPu.Registered)
                {
                    if (comCongPu.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                        ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", comCongPu.DeviceRegistrationFailureReason);
                    if (comCongPu.Registered)
                        comCongPu.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate115200,
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
        void comCongPu_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            //int exeid = 0;

            byte[] sendBytes = Encoding.ASCII.GetBytes(args.SerialData);
            if (sendBytes != null && sendBytes.Length == 3)
            {
                if (sendBytes[0] == 0x1B)
                {
                    byte iChanIdx = sendBytes[1];
                    bool iChanStatus = Convert.ToBoolean(sendBytes[2]);
                    if (iChanIdx > 8)
                    {
                        if (9 == iChanIdx)/*RD[16]*/
                        {
                            if (this.Push16IEvent!=null)
                            {
                                this.Push16IEvent(16, iChanStatus);

                            }
                        }
                        else if ((iChanIdx <= 22) && (iChanIdx > 15))	/*RD[9] ~ RD[15]*/
                        {
                            /*iChanIdx 属于[16,22]*/
                           // Push_16I(31 - iChanIdx, iChanStatus);
                            if (this.Push16IEvent != null)
                            {
                                this.Push16IEvent(31 - iChanIdx, iChanStatus);
                            }
                        }
                    }
                    else
                    {
                        if (iChanIdx > 0)/*RD[1] ~ RD[8]*/
                        {
                            if (this.Push16IEvent != null)
                            {
                                //Push_16I(9 - iChanIdx, iChanStatus);
                                this.Push16IEvent(9 - iChanIdx, iChanStatus);
                            }
                        }

                    }
                }
            }
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
            for (int i = 0; i < 8; i++)
            {
                
                this.Relay8SW8(addr, i, true);
            }
            //this.Relay8SW8(6, 1, true);
            //this.Relay8SW8(6, 2, true);
            //this.Relay8SW8(6, 3, true);
            //this.Relay8SW8(6, 4, true);
            //this.Relay8SW8(6, 5, true);
            //this.Relay8SW8(6, 6, true);
            //this.Relay8SW8(6, 7, true);
            //this.Relay8SW8(7, 0, true);
            //this.Relay8SW8(7, 1, true);
            //this.Relay8SW8(7, 2, true);
            //this.Relay8SW8(7, 3, true);
            //this.Relay8SW8(7, 4, true);
            //this.Relay8SW8(7, 5, true);
            //this.Relay8SW8(7, 6, true);
            //this.Relay8SW8(7, 7, true);
            //this.Relay8SW8(8, 0, true);
            //this.Relay8SW8(8, 1, true);
            //this.Relay8SW8(8, 2, true);
            //this.Relay8SW8(8, 3, true);
            //this.Relay8SW8(8, 4, true);
            //this.Relay8SW8(8, 5, true);
            //this.Relay8SW8(8, 6, true);
            //this.Relay8SW8(8, 7, true);
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
            for (int i = 0; i < 8; i++)
            {
                this.Relay8SW8(addr, i, false);
            }
            //this.Relay8SW8(6, 1, false);
            //this.Relay8SW8(6, 2, false);
            //this.Relay8SW8(6, 3, false);
            //this.Relay8SW8(6, 4, false);
            //this.Relay8SW8(6, 5, false);
            //this.Relay8SW8(6, 6, false);
            //this.Relay8SW8(6, 7, false);
            //this.Relay8SW8(7, 0, false);
            //this.Relay8SW8(7, 1, false);
            //this.Relay8SW8(7, 2, false);
            //this.Relay8SW8(7, 3, false);
            //this.Relay8SW8(7, 4, false);
            //this.Relay8SW8(7, 5, false);
            //this.Relay8SW8(7, 6, false);
            //this.Relay8SW8(7, 7, false);
            //this.Relay8SW8(8, 0, false);
            //this.Relay8SW8(8, 1, false);
            //this.Relay8SW8(8, 2, false);
            //this.Relay8SW8(8, 3, false);
            //this.Relay8SW8(8, 4, false);
            //this.Relay8SW8(8, 5, false);
            //this.Relay8SW8(8, 6, false);
            //this.Relay8SW8(8, 7, false);
        }
        public void Relay8SW8(int port, bool states)
        {
            switch (port)
            {
                case 0:
                    this.Status.Relay0 = states;
                    break;
                case 1:
                    this.Status.Relay1 = states;
                    break;
                case 2:
                    this.Status.Relay2 = states;
                    break;
                case 3:
                    this.Status.Relay3 = states;
                    break;
                case 4:
                    this.Status.Relay4 = states;
                    break;
                case 5:
                    this.Status.Relay5 = states;
                    break;
                case 6:
                    this.Status.Relay6 = states;
                    break;
                case 7:
                    this.Status.Relay7 = states;
                    break;
                default:
                    break;
            }
            

            this.Relay8SW8(addr, port, states);
        }
        private void Relay8SW8(int address, int port, bool states)
        {
            byte[] sendBytes = new byte[] { 0x52, (byte)address, (byte)port, 0x00, (byte)(address + port), 0xAA };
            if (states)
            {
                sendBytes = new byte[] { 0x52, (byte)address, (byte)port, 0x01, (byte)(address + port + 1), 0xAA };
            }
            string cmd = Encoding.GetEncoding(28591).GetString(sendBytes, 0, sendBytes.Length);

            this.comCongPu.Send(cmd);
            Thread.Sleep(200);
        }
    }
    public class CongPu8SW8Status
    {
        public bool Relay0 = false;
        public bool Relay1 = false;
        public bool Relay2 = false;
        public bool Relay3 = false;
        public bool Relay4 = false;
        public bool Relay5 = false;
        public bool Relay6 = false;
        public bool Relay7 = false;
    }
}