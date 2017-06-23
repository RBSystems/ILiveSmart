using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.Lighting;
using Crestron.SimplSharpPro.Lighting.Din;
using Crestron.SimplSharpPro.DeviceSupport;

namespace ILiveSmart
{
    public class CP3Smart
    {
        public delegate void YelaPressHandler(int button);

        private CrestronControlSystem controlSystem = null;


        public DYelaLock YelaLock;
      //  public ComPort comYelaLock;
        public ComPort comSMS;

        public DigitalInput myDigitalInputPort1;
        public DigitalInput myDigitalInputPort2;
        public DigitalInput myDigitalInputPort3;
        public DigitalInput myDigitalInputPort4;
        public DigitalInput myDigitalInputPort5;
        public DigitalInput myDigitalInputPort6;
        public DigitalInput myDigitalInputPort7;
        public DigitalInput myDigitalInputPort8;

        public IROutputPort myIROutputPort1;
        public IROutputPort myIROutputPort2;
        public IROutputPort myIROutputPort3;
        public IROutputPort myIROutputPort4;
        public IROutputPort myIROutputPort5;
        public IROutputPort myIROutputPort6;
        public IROutputPort myIROutputPort7;
        public IROutputPort myIROutputPort8;

        public Relay relayBedRoomScreenUp;
        public Relay relayBedRoomScreenDown;
        public Relay relayStudyRoomDoor;
        public Relay myRelayPort4;
        public Relay myRelayPort5;
        public Relay myRelayPort6;
        public Relay myRelayPort7;
        public Relay myRelayPort8;

        public CP3Smart(CrestronControlSystem system)
        {
            this.controlSystem = system;
            
        }

        public void RegisterDevices()
        {

            #region 注册串口
            if (this.controlSystem.SupportsComPort)
            {
                this.YelaLock = new DYelaLock(this.controlSystem.ComPorts[2]);



                comSMS = this.controlSystem.ComPorts[3];
                //comSMS.SerialDataReceived += new ComPortDataReceivedEvent(comSMS_SerialDataReceived);

                if (comSMS.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                    ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", comSMS.DeviceRegistrationFailureReason);

                if (comSMS.Registered)
                    comSMS.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate9600,
                                                                     ComPort.eComDataBits.ComspecDataBits8,
                                                                     ComPort.eComParityType.ComspecParityNone,
                                                                     ComPort.eComStopBits.ComspecStopBits1,
                                         ComPort.eComProtocolType.ComspecProtocolRS232,
                                         ComPort.eComHardwareHandshakeType.ComspecHardwareHandshakeNone,
                                         ComPort.eComSoftwareHandshakeType.ComspecSoftwareHandshakeNone,
                                         false);
            }
            #endregion

            #region 注册红外
            if (this.controlSystem.SupportsIROut)
            {
                this.myIROutputPort1 = this.controlSystem.IROutputPorts[1];
                
                this.myIROutputPort2 = this.controlSystem.IROutputPorts[2];
                this.myIROutputPort3 = this.controlSystem.IROutputPorts[3];
                this.myIROutputPort4 = this.controlSystem.IROutputPorts[4];
                this.myIROutputPort5 = this.controlSystem.IROutputPorts[5];
                this.myIROutputPort6 = this.controlSystem.IROutputPorts[6];
                this.myIROutputPort7 = this.controlSystem.IROutputPorts[7];
                this.myIROutputPort8 = this.controlSystem.IROutputPorts[8];
            }
            #endregion

            #region 注册继电器
            relayBedRoomScreenUp = this.controlSystem.RelayPorts[1];
            relayBedRoomScreenUp.StateChange += new RelayEventHandler(relayBedRoomScreenUp_StateChange);

            if (relayBedRoomScreenUp.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("Relay Port couldn't be registered. Cause: {0}", relayBedRoomScreenUp.DeviceRegistrationFailureReason);

            relayBedRoomScreenDown = this.controlSystem.RelayPorts[2];
            relayBedRoomScreenDown.StateChange += new RelayEventHandler(relayBedRoomScreenDown_StateChange);

            if (relayBedRoomScreenDown.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("Relay Port couldn't be registered. Cause: {0}", relayBedRoomScreenDown.DeviceRegistrationFailureReason);

            relayStudyRoomDoor = this.controlSystem.RelayPorts[3];
            relayStudyRoomDoor.StateChange += new RelayEventHandler(relayStudyRoomDoor_StateChange);

            if (relayStudyRoomDoor.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("Relay Port couldn't be registered. Cause: {0}", relayStudyRoomDoor.DeviceRegistrationFailureReason);
            #endregion

        }

        void relayStudyRoomDoor_StateChange(Relay relay, RelayEventArgs args)
        {
           // throw new NotImplementedException();
        }

        void relayBedRoomScreenDown_StateChange(Relay relay, RelayEventArgs args)
        {
           // throw new NotImplementedException();
        }

        void relayBedRoomScreenUp_StateChange(Relay relay, RelayEventArgs args)
        {
           // throw new NotImplementedException();
        }

    }
}