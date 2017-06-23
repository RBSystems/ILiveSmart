using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.Keypads;
using Crestron.SimplSharpPro.GeneralIO;
using Crestron.SimplSharpPro.CrestronThread;
using ILiveSmart.Light;

namespace ILiveSmart.UI
{
    public class SmartInput
    {
        //public delegate void DigitalPressHandler(bool button);
        //public event DigitalPressHandler DigitalPressEvent;
        public WebSocketUI webui;

        private ControlSystem controlSystem = null;
        private ILiveSmartAPI _logic = null;
        public Tsw1052 tsw1052;
        public CrestronMobile mobile;

        public C2niCb c2nicb_BedRoom;
        public C2niCb c2nicb_StudyRoom;
        public C2niCb c2nicb_MettingRoom;
        public C2niCb c2nicb_GeneralRoom;

        public GlsOirCCn glsOirCCn_WashRoom;
        public GlsOirCCn glsOirCCn_Door;

        public ComPort cp3_com_16I;

        public DigitalInput myDigitalInputPort1;

        public SmartInput(ControlSystem system,ILiveSmartAPI logic)
        {
            this.controlSystem = system;
            this._logic = logic;
        }
        internal void Start()
        {
            this.RegisterDevices();
        }
        #region 注册所有设备
        public void RegisterDevices()
        {
            #region 注册快思聪总线设备
            c2nicb_BedRoom = new C2niCb(0x20, this.controlSystem);                     
            c2nicb_BedRoom.ButtonStateChange += new ButtonEventHandler(c2nicb_BedRoom_ButtonStateChange);
            c2nicb_BedRoom.DigitalInputPorts[1].StateChange += new DigitalInputEventHandler(SmartInput_StateChange1);
            if (c2nicb_BedRoom.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("c2nicb_BedRoom failed registration. Cause: {0}", c2nicb_BedRoom.RegistrationFailureReason);

            c2nicb_StudyRoom = new C2niCb(0x21, this.controlSystem);                    
            c2nicb_StudyRoom.ButtonStateChange += new ButtonEventHandler(c2nicb_StudyRoom_ButtonStateChange);
            if (c2nicb_StudyRoom.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("c2nicb_StudyRoom failed registration. Cause: {0}", c2nicb_StudyRoom.RegistrationFailureReason);
            

            c2nicb_MettingRoom = new C2niCb(0x22, this.controlSystem);                     
            c2nicb_MettingRoom.ButtonStateChange += new ButtonEventHandler(c2nicb_MettingRoom_ButtonStateChange);
            if (c2nicb_MettingRoom.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("c2nicb_MettingRoom failed registration. Cause: {0}", c2nicb_MettingRoom.RegistrationFailureReason);

            c2nicb_GeneralRoom = new C2niCb(0x23, this.controlSystem);                     
            c2nicb_GeneralRoom.ButtonStateChange += new ButtonEventHandler(c2nicb_GeneralRoom_ButtonStateChange);
            if (c2nicb_GeneralRoom.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("c2nicb_GeneralRoom failed registration. Cause: {0}", c2nicb_GeneralRoom.RegistrationFailureReason);

            glsOirCCn_WashRoom = new GlsOirCCn(0x97, this.controlSystem);
            glsOirCCn_WashRoom.GlsOccupancySensorChange += new GlsOccupancySensorChangeEventHandler(glsOirCCn_WashRoom_GlsOccupancySensorChange);
            if (glsOirCCn_WashRoom.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("glsOirCCn_WashRoom failed registration. Cause: {0}", glsOirCCn_WashRoom.RegistrationFailureReason);

            glsOirCCn_Door = new GlsOirCCn(0x98, this.controlSystem);
            glsOirCCn_Door.GlsOccupancySensorChange += new GlsOccupancySensorChangeEventHandler(glsOirCCn_Door_GlsOccupancySensorChange);
            if (glsOirCCn_Door.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("glsOirCCn_Door failed registration. Cause: {0}", glsOirCCn_Door.RegistrationFailureReason);

            #endregion

            #region 注册IO
            try
            {
               myDigitalInputPort1= this.c2nicb_BedRoom.DigitalInputPorts[1];
               // myDigitalInputPort1 = this.controlSystem.SupportsDigitalInput;
            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine(ex.Message + this.controlSystem.SupportsDigitalInput);
            }
            
           // myDigitalInputPort1.StateChange += new DigitalInputEventHandler(myDigitalInputPort1_StateChange);
           // if (myDigitalInputPort1.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
             //   ErrorLog.Error("myDigitalInputPort1 failed registration. Cause: {0}", myDigitalInputPort1.DeviceRegistrationFailureString);

            #endregion
            #region 注册串口
            if (this.controlSystem.SupportsComPort)
            {
                cp3_com_16I = this.controlSystem.ComPorts[1];
                cp3_com_16I.SerialDataReceived += new ComPortDataReceivedEvent(cp3_com_16I_SerialDataReceived);

                if (cp3_com_16I.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                    ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", cp3_com_16I.DeviceRegistrationFailureReason);

                if (cp3_com_16I.Registered)
                    cp3_com_16I.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate115200,
                                                                     ComPort.eComDataBits.ComspecDataBits8,
                                                                     ComPort.eComParityType.ComspecParityNone,
                                                                     ComPort.eComStopBits.ComspecStopBits1,
                                         ComPort.eComProtocolType.ComspecProtocolRS485,
                                         ComPort.eComHardwareHandshakeType.ComspecHardwareHandshakeNone,
                                         ComPort.eComSoftwareHandshakeType.ComspecSoftwareHandshakeNone,
                                         false);

            }
            #endregion

            #region 注册网络设备
            if (this.controlSystem.SupportsEthernet)
            {
                tsw1052 = new Tsw1052(0x03, this.controlSystem);
                //mobile = new CrestronMobile(0x04, this.controlSystem);

                tsw1052.SigChange += new SigEventHandler(tsw1052_SigChange);
                tsw1052.ExtenderVoipReservedSigs.DeviceExtenderSigChange += new DeviceExtenderJoinChangeEventHandler(ExtenderVoipReservedSigs_DeviceExtenderSigChange);
               // tsw1052.ExtenderVoiceControlReservedSigs.DeviceExtenderSigChange += new DeviceExtenderJoinChangeEventHandler(ExtenderVoiceControlReservedSigs_DeviceExtenderSigChange);
               
                
                this._logic.LivingScenceFinished += this.BusyPageClose;
                this._logic.LivingScenceStart += this.BusyPageShow;
                //tsw1052.ButtonStateChange += new ButtonEventHandler(tsw1052_ButtonStateChange);
                //mobile.SigChange += new SigEventHandler(mobile_SigChange);

                // Register the devices for usage. This should happen after the 
                // eventhandler registration, to ensure no data is missed.
                if (tsw1052.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                {

                    ILiveDebug.Instance.WriteLine("tsw1052 failed registration. Cause: {0}", tsw1052.RegistrationFailureReason);
                    ErrorLog.Error("tsw1052 failed registration. Cause: {0}", tsw1052.RegistrationFailureReason);
                }
                tsw1052.ExtenderVoipReservedSigs.Use();
           //     ILiveDebug.Instance.WriteLine("sip:" + tsw1052.ExtenderVoipReservedSigs.MyURIFeedback.);
                #region 注册远程控制设备（WebSocket）
                this.webui = new WebSocketUI();
                this.webui.DataReceived += new WebSocketServer.DataReceivedEventHandler(WebSocketUI_DataReceived);
                this.webui.Register();
                #endregion
            }
            #endregion

            #region 客厅灯光事件
            this._logic.SetFoyerLightWatchEvent(new LightWatchEventHandler(this.OnFoyerLightChange));
            this._logic.SetDropLightWatchEvent(new LightWatchEventHandler(this.OnDropLightChange));
            this._logic.SetRightLightWatchEvent(new LightWatchEventHandler(this.OnRightLightChange));
            this._logic.SetFrontLightWatchEvent(new LightWatchEventHandler(this.OnFrontLightChange));
            this._logic.SetBackLightWatchEvent(new LightWatchEventHandler(this.OnBackLightChange));
            this._logic.SetBeltLightWatchEvent(new LightWatchEventHandler(this.OnBeltLightChange));

            #endregion
        }

        void ExtenderVoiceControlReservedSigs_DeviceExtenderSigChange(DeviceExtender currentDeviceExtender, SigEventArgs args)
        {
          // ILiveDebug.Instance.WriteLine("ExtenderVoiceControlReservedSigs_DeviceExtenderSigChange:"+args.Sig.Number+":"+args.Sig.BoolValue);
          //  throw new NotImplementedException();
        }

        void ExtenderVoipReservedSigs_DeviceExtenderSigChange(DeviceExtender currentDeviceExtender, SigEventArgs args)
        {
             switch (args.Sig.Type)
            {
                case eSigType.Bool:
                    {
                       // ILiveDebug.Instance.WriteLine("ExtenderVoipReservedSigs_DeviceExtenderSigChange:" + args.Sig.Number + ":" + args.Sig.BoolValue);


                        this.ProcessTsw1052ExtenderVoipReservedSigs(args.Sig.Number, args.Sig.BoolValue);
                    }
                    break;
                case eSigType.UShort:
                    {
                        {
                          //  this.ProcessTsw1052UShort(args.Sig.Number, args.Sig.UShortValue);
                        }
                    }
                    break;
            }

  

            //throw new NotImplementedException();
        }


        void _logic_StudyRoomScenceStart(string s)
        {
           // throw new NotImplementedException();
        }

        private void ProcessTsw1052ExtenderVoipReservedSigs(uint sigNumber, bool boolValue)
        {
            if (boolValue)
            {
                switch ((TSW1052Bool)sigNumber)
                {
                    case TSW1052Bool.Incoming:
                        ILiveDebug.Instance.WriteLine("ProcessTsw1052ExtenderVoipReservedSigs:Incoming");

                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_SIPIncoming].BoolValue = true;
                        Thread.Sleep(500);
                        tsw1052.BooleanInput[(uint)TSW1052Bool.SIPEnableVideo].BoolValue = true;
                        tsw1052.StringInput[12].StringValue = "rtsp://admin:admin@192.168.1.51:554/cam/realmonitor?channel=1&subtype=0";
                        Thread.Sleep(500);
                        tsw1052.BooleanInput[(uint)TSW1052Bool.SIPShowVideo].BoolValue = true;

                        //  ILiveDebug.Instance.WriteLine("ProcessTsw1052ExtenderVoipReservedSigs:Incoming" + shortValue.ToString());
                        break;
                    default:
                        break;
                }
            }
        }



        #region 客厅灯光反馈
        public void OnFoyerLightChange(ushort lightlevel)
        {
            double pre = (lightlevel / (double)65535) * 100;
            ushort Light1_Pic_Mode = this.GetPicMode(lightlevel);
            this.tsw1052.UShortInput[20].UShortValue = lightlevel;
            this.tsw1052.UShortInput[10].UShortValue = Light1_Pic_Mode;
            this.tsw1052.StringInput[20].StringValue = Math.Ceiling(pre).ToString() + "%";
        }
        public void OnDropLightChange(ushort lightlevel)
        {
            double pre = (lightlevel / (double)65535) * 100;
            ushort Light1_Pic_Mode = this.GetPicMode(lightlevel);
            this.tsw1052.UShortInput[21].UShortValue = lightlevel;
            this.tsw1052.UShortInput[11].UShortValue = Light1_Pic_Mode;
            this.tsw1052.StringInput[21].StringValue = Math.Ceiling(pre).ToString() + "%";
        }
        public void OnBeltLightChange(ushort lightlevel)
        {
            double pre = (lightlevel / (double)65535) * 100;
            ushort Light1_Pic_Mode = this.GetPicMode(lightlevel);
            this.tsw1052.UShortInput[22].UShortValue = lightlevel;
            this.tsw1052.UShortInput[12].UShortValue = Light1_Pic_Mode;
            this.tsw1052.StringInput[22].StringValue = Math.Ceiling(pre).ToString() + "%";
        }
        
        public void OnFrontLightChange(ushort lightlevel)
        {
            double pre = (lightlevel / (double)65535) * 100;
            ushort Light1_Pic_Mode = this.GetPicMode(lightlevel);
            this.tsw1052.UShortInput[23].UShortValue = lightlevel;
            this.tsw1052.UShortInput[13].UShortValue = Light1_Pic_Mode;
            this.tsw1052.StringInput[23].StringValue = Math.Ceiling(pre).ToString() + "%";
        }
        public void OnRightLightChange(ushort lightlevel)
        {
            double pre = (lightlevel / (double)65535) * 100;
            ushort Light1_Pic_Mode = this.GetPicMode(lightlevel);
            this.tsw1052.UShortInput[24].UShortValue = lightlevel;
            this.tsw1052.UShortInput[14].UShortValue = Light1_Pic_Mode;
            this.tsw1052.StringInput[24].StringValue = Math.Ceiling(pre).ToString() + "%";
        }
        public void OnBackLightChange(ushort lightlevel)
        {
            double pre = (lightlevel / (double)65535) * 100;
            ushort Light1_Pic_Mode = this.GetPicMode(lightlevel);
            this.tsw1052.UShortInput[25].UShortValue = lightlevel;
            this.tsw1052.UShortInput[15].UShortValue = Light1_Pic_Mode;
            this.tsw1052.StringInput[25].StringValue = Math.Ceiling(pre).ToString() + "%";
        }


        public ushort GetPicMode(ushort lightlevel)
        {
            ushort Light1_Pic_Mode = 0;
            if (lightlevel <= 6553)
            {
                Light1_Pic_Mode = 0;
            }
            else if (lightlevel > 6553 && lightlevel <= 13107)
            {
                Light1_Pic_Mode = 1;
            }
            else if (lightlevel > 13107 && lightlevel <= 19960.5)
            {
                Light1_Pic_Mode = 2;
            }
            else if (lightlevel > 19960.5 && lightlevel <= 19960.5)
            {
                Light1_Pic_Mode = 3;
            }
            else if (lightlevel > 19960.5 && lightlevel <= 26214)
            {
                Light1_Pic_Mode = 4;
            }
            else if (lightlevel > 26214 && lightlevel <= 32767.5)
            {
                Light1_Pic_Mode = 5;
            }
            else if (lightlevel > 32767.5 && lightlevel <= 39321)
            {
                Light1_Pic_Mode = 6;
            }
            else if (lightlevel > 39321 && lightlevel <= 45874.5)
            {
                Light1_Pic_Mode = 7;
            }
            else if (lightlevel > 45874.5 && lightlevel <= 52428)
            {
                Light1_Pic_Mode = 8;
            }
            else if (lightlevel > 52428 && lightlevel <= 58981.5)
            {
                Light1_Pic_Mode = 9;
            }
            else if (lightlevel > 58981.5 && lightlevel <= 65535)
            {
                Light1_Pic_Mode = 10;
            }
            return Light1_Pic_Mode;
        }
        #endregion


        #endregion

        #region 面板事件
        /// <summary>
        /// 经理室面板事件
        /// </summary>
        /// <param name="device"></param>
        /// <param name="args"></param>
        void c2nicb_GeneralRoom_ButtonStateChange(GenericBase device, ButtonEventArgs args)
        {
            if (args.NewButtonState == eButtonState.Pressed)
            {
                Button button = args.Button;
                switch (button.Number)
                {

                    case 2://左上
                        {
                            this._logic.GeneralOfficeLightToggle();
                            break;
                        }

                    case 4://左中
                        {
                            break;
                        }
                    case 6://左下
                        {
                            break;
                        }
                    case 7://右上
                        {
                            break;
                        }
                    case 9://右中
                        {
                            break;
                        }
                    case 11://右下
                        {
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 会议室面板事件
        /// </summary>
        /// <param name="device"></param>
        /// <param name="args"></param>
        void c2nicb_MettingRoom_ButtonStateChange(GenericBase device, ButtonEventArgs args)
        {
            if (args.NewButtonState == eButtonState.Pressed)
            {
                Button button = args.Button;
                switch (button.Number)
                {

                    case 2://左上
                        {
                            this._logic.MettingLightToggle();
                            break;
                        }

                    case 4://左中
                        {
                            break;
                        }
                    case 6://左下
                        {
                            break;
                        }
                    case 7://右上
                        {
                            break;
                        }
                    case 9://右中
                        {
                            break;
                        }
                    case 11://右下
                        {
                            break;
                        }
                    default:
                        break;
                }
            }
        }

     //   private bool studyroombuttonlight = false;
        private bool studyroombuttontemp = false;
       // private bool studyroombuttonaudio = false;
        /// <summary>
        /// 书房按钮事件
        /// </summary>
        /// <param name="device"></param>
        /// <param name="args"></param>
        void c2nicb_StudyRoom_ButtonStateChange(GenericBase device, ButtonEventArgs args)
        {
            
            if (args.NewButtonState == eButtonState.Pressed)
            {
                if (this._logic.StudyRoomScenceIsBusy)
                {
                    return;
                }

                Button button = args.Button;
                switch (button.Number)
                {

                    case 2://左上
                        {
                            this._logic.StudyRoomScenceStart += new ILiveSmartAPI.ScenceEventHandler(this.StudyRoomScenceWatchBookStart);
                            this._logic.StudyRoomScenceFinished += new ILiveSmartAPI.ScenceEventHandler(this.StudyRoomScenceWatchBookFinished);
                            //this.StudyRoomScenceWatchBookStart();
                            this._logic.ScenceStudyRoomWatchBook();
                            break;
                        }

                    case 4://左中
                        {
                            this._logic.StudyRoomScenceStart += new ILiveSmartAPI.ScenceEventHandler(this.StudyRoomScenceOfficeStart);
                            this._logic.StudyRoomScenceFinished += new ILiveSmartAPI.ScenceEventHandler(this.StudyRoomScenceOfficeFinished);
                            //this.StudyRoomScenceWatchBookStart();
                            this._logic.ScenceStudyRoomOffice();

                           // new Thread(new ThreadCallbackFunction(this.StudyRoomScenceOffice), this, Thread.eThreadStartOptions.Running);
                            break;
                        }
                    case 6://左下
                        {
                            new Thread(new ThreadCallbackFunction(this.StudyRoomScenceLeave), this, Thread.eThreadStartOptions.Running);

   
                            break;
                        }
                    case 7://右上
                        {
                            this._logic.StudyRoomLightToggle();
                            if (this._logic.StudyRoomLightIsOpen)
                            {
                              //  studyroombuttonlight = false;
                                this.c2nicb_StudyRoom.Feedbacks[7].State = true;
                                this.c2nicb_StudyRoom.Feedbacks[8].State = true;
                            }
                            else
                            {
                              //  studyroombuttonlight = true;
                                this.c2nicb_StudyRoom.Feedbacks[7].State = false;
                                this.c2nicb_StudyRoom.Feedbacks[8].State = false;
                            }
                            break;
                        }
                    case 9://右中
                        {
                            this._logic.StudyRoomTempToggle();
                            if (studyroombuttontemp)
                            {
                                studyroombuttontemp = false;
                                this.c2nicb_StudyRoom.Feedbacks[9].State = false;
                                this.c2nicb_StudyRoom.Feedbacks[10].State = false;
                            }
                            else
                            {
                                studyroombuttontemp = true;
                                this.c2nicb_StudyRoom.Feedbacks[9].State = true;
                                this.c2nicb_StudyRoom.Feedbacks[10].State = true;
                            }
                            break;
                        }
                    case 11://右下
                        {
                            new Thread(new ThreadCallbackFunction(this.StudyRoomWindowToggle), this, Thread.eThreadStartOptions.Running);

                            break;
                        }
                    default:
                        break;
                }
                if (args.NewButtonState == eButtonState.Held)
                {
                }
            }
        }
        /// <summary>
        /// 卧室面板按钮事件
        /// </summary>
        /// <param name="device"></param>
        /// <param name="args"></param>
        void c2nicb_BedRoom_ButtonStateChange(GenericBase device, ButtonEventArgs args)
        {
          //  ILiveDebug.WriteLine("DigitalInputPortsCount"+c2nicb_BedRoom.DigitalInputPorts.Count.ToString());
            //ILiveDebug.WriteLine("Reg"+c2nicb_BedRoom.DigitalInputPorts[1].Registered.ToString());
           // ILiveDebug.WriteLine(c2nicb_BedRoom.DigitalInputPorts[1].StateChange.Count.ToString());
            if (args.NewButtonState == eButtonState.Pressed)
            {
                if (this._logic.BedRoomScenceIsBusy)
                {
                    return;
                }

                Button button = args.Button;
                switch (button.Number)
                {

                    case 2://左上
                        {
                            new Thread(new ThreadCallbackFunction(this.BedRoomScenceWatchMovie), this, Thread.eThreadStartOptions.Running);
                            break;
                        }

                    case 4://左中
                        {
                            new Thread(new ThreadCallbackFunction(this.BedRoomScenceRomantic), this, Thread.eThreadStartOptions.Running);
                            break;
                        }
                    case 6://左下
                        {

                            new Thread(new ThreadCallbackFunction(this.BedRoomScenceSleep), this, Thread.eThreadStartOptions.Running);
                            break;
                        }
                    case 7://右上
                        {
                            new Thread(new ThreadCallbackFunction(this.BedRoomScenceAlarmClock), this, Thread.eThreadStartOptions.Running);
                            break;
                        }
                    case 9://右中
                        {
                            new Thread(new ThreadCallbackFunction(this.BedRoomScenceWash), this, Thread.eThreadStartOptions.Running);
                            break;
                        }
                    case 11://右下
                        {
                            new Thread(new ThreadCallbackFunction(this.BedRoomScenceLeave), this, Thread.eThreadStartOptions.Running);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        #region 卧室面板
        /// <summary>
        /// 观影模式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object BedRoomScenceWatchMovie(object o)
        {
            this._logic.BedRoomScenceIsBusy = true;
            this.c2nicb_BedRoom.Feedbacks[1].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[2].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[1].State = true;
            this.c2nicb_BedRoom.Feedbacks[2].State = true;
            this.c2nicb_BedRoom.Feedbacks[3].State = false;
            this.c2nicb_BedRoom.Feedbacks[4].State = false;
            this.c2nicb_BedRoom.Feedbacks[5].State = false;
            this.c2nicb_BedRoom.Feedbacks[6].State = false;
            this.c2nicb_BedRoom.Feedbacks[7].State = false;
            this.c2nicb_BedRoom.Feedbacks[8].State = false;
            this.c2nicb_BedRoom.Feedbacks[9].State = false;
            this.c2nicb_BedRoom.Feedbacks[10].State = false;
            this.c2nicb_BedRoom.Feedbacks[11].State = false;
            this.c2nicb_BedRoom.Feedbacks[12].State = false;
            this._logic.ScenceBedRoomWatchMovie();
            this.c2nicb_BedRoom.Feedbacks[1].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_BedRoom.Feedbacks[2].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.BedRoomScenceIsBusy = false;
            return o;
        }
        /// <summary>
        /// 浪漫模式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object BedRoomScenceRomantic(object o)
        {
            this._logic.BedRoomScenceIsBusy = true;
            this.c2nicb_BedRoom.Feedbacks[3].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[4].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[1].State = false;
            this.c2nicb_BedRoom.Feedbacks[2].State = false;
            this.c2nicb_BedRoom.Feedbacks[3].State = true;
            this.c2nicb_BedRoom.Feedbacks[4].State = true;
            this.c2nicb_BedRoom.Feedbacks[5].State = false;
            this.c2nicb_BedRoom.Feedbacks[6].State = false;
            this.c2nicb_BedRoom.Feedbacks[7].State = false;
            this.c2nicb_BedRoom.Feedbacks[8].State = false;
            this.c2nicb_BedRoom.Feedbacks[9].State = false;
            this.c2nicb_BedRoom.Feedbacks[10].State = false;
            this.c2nicb_BedRoom.Feedbacks[11].State = false;
            this.c2nicb_BedRoom.Feedbacks[12].State = false;
            this._logic.ScenceBedRoomRomantic();
            this.c2nicb_BedRoom.Feedbacks[3].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_BedRoom.Feedbacks[4].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.BedRoomScenceIsBusy = false;
            return o;
        }
        /// <summary>
        /// 睡眠模式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object BedRoomScenceSleep(object o)
        {
            this._logic.BedRoomScenceIsBusy = true;
            this.c2nicb_BedRoom.Feedbacks[5].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[6].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[1].State = false;
            this.c2nicb_BedRoom.Feedbacks[2].State = false;
            this.c2nicb_BedRoom.Feedbacks[3].State = false;
            this.c2nicb_BedRoom.Feedbacks[4].State = false;
            this.c2nicb_BedRoom.Feedbacks[5].State = true;
            this.c2nicb_BedRoom.Feedbacks[6].State = true;
            this.c2nicb_BedRoom.Feedbacks[7].State = false;
            this.c2nicb_BedRoom.Feedbacks[8].State = false;
            this.c2nicb_BedRoom.Feedbacks[9].State = false;
            this.c2nicb_BedRoom.Feedbacks[10].State = false;
            this.c2nicb_BedRoom.Feedbacks[11].State = false;
            this.c2nicb_BedRoom.Feedbacks[12].State = false;
                this._logic.ScenceBedRoomSleep();
            this.c2nicb_BedRoom.Feedbacks[5].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_BedRoom.Feedbacks[6].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.BedRoomScenceIsBusy = false;
            return o;
        }
        /// <summary>
        /// 闹钟模式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object BedRoomScenceAlarmClock(object o)
        {
            this._logic.BedRoomScenceIsBusy = true;
            this.c2nicb_BedRoom.Feedbacks[7].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[8].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[1].State = false;
            this.c2nicb_BedRoom.Feedbacks[2].State = false;
            this.c2nicb_BedRoom.Feedbacks[3].State = false;
            this.c2nicb_BedRoom.Feedbacks[4].State = false;
            this.c2nicb_BedRoom.Feedbacks[5].State = false;
            this.c2nicb_BedRoom.Feedbacks[6].State = false;
            this.c2nicb_BedRoom.Feedbacks[7].State = true;
            this.c2nicb_BedRoom.Feedbacks[8].State = true;
            this.c2nicb_BedRoom.Feedbacks[9].State = false;
            this.c2nicb_BedRoom.Feedbacks[10].State = false;
            this.c2nicb_BedRoom.Feedbacks[11].State = false;
            this.c2nicb_BedRoom.Feedbacks[12].State = false;
            this._logic.ScenceBedRoomAlarmClock();
            this.c2nicb_BedRoom.Feedbacks[7].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_BedRoom.Feedbacks[8].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.BedRoomScenceIsBusy = false;
            return o;
        }
        /// <summary>
        /// 起漱模式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object BedRoomScenceWash(object o)
        {
            this._logic.BedRoomScenceIsBusy = true;
            this.c2nicb_BedRoom.Feedbacks[9].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[10].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[1].State = false;
            this.c2nicb_BedRoom.Feedbacks[2].State = false;
            this.c2nicb_BedRoom.Feedbacks[3].State = false;
            this.c2nicb_BedRoom.Feedbacks[4].State = false;
            this.c2nicb_BedRoom.Feedbacks[5].State = false;
            this.c2nicb_BedRoom.Feedbacks[6].State = false;
            this.c2nicb_BedRoom.Feedbacks[7].State = false;
            this.c2nicb_BedRoom.Feedbacks[8].State = false;
            this.c2nicb_BedRoom.Feedbacks[9].State = true;
            this.c2nicb_BedRoom.Feedbacks[10].State = true;
            this.c2nicb_BedRoom.Feedbacks[11].State = false;
            this.c2nicb_BedRoom.Feedbacks[12].State = false;
            this._logic.ScenceBedRoomWash(); 
            this.c2nicb_BedRoom.Feedbacks[9].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_BedRoom.Feedbacks[10].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.BedRoomScenceIsBusy = false;
            return o;
        }

        /// <summary>
        /// 离开模式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object BedRoomScenceLeave(object o)
        {
            this._logic.BedRoomScenceIsBusy = true;
            this.c2nicb_BedRoom.Feedbacks[11].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[12].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_BedRoom.Feedbacks[1].State = false;
            this.c2nicb_BedRoom.Feedbacks[2].State = false;
            this.c2nicb_BedRoom.Feedbacks[3].State = false;
            this.c2nicb_BedRoom.Feedbacks[4].State = false;
            this.c2nicb_BedRoom.Feedbacks[5].State = false;
            this.c2nicb_BedRoom.Feedbacks[6].State = false;
            this.c2nicb_BedRoom.Feedbacks[7].State = false;
            this.c2nicb_BedRoom.Feedbacks[8].State = false;
            this.c2nicb_BedRoom.Feedbacks[9].State = false;
            this.c2nicb_BedRoom.Feedbacks[10].State = false;
            this.c2nicb_BedRoom.Feedbacks[11].State = true;
            this.c2nicb_BedRoom.Feedbacks[12].State = true;
            this._logic.ScenceBedRoomLeave();
            this.c2nicb_BedRoom.Feedbacks[11].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_BedRoom.Feedbacks[12].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.BedRoomScenceIsBusy = false;
            return o;
        }
        void SmartInput_StateChange1(DigitalInput digitalInput, DigitalInputEventArgs args)
        {
            //ILiveDebug.WriteLine("D"+c2nicb_BedRoom.DigitalInputPorts.Count.ToString());
            if (args.State)
            {
                new Thread(new ThreadCallbackFunction(this.ScenceBedRoomGetUpNight), this, Thread.eThreadStartOptions.Running);

              //  this._logic.ScenceBedRoomGetUpNight();
            }
        }
        public object ScenceBedRoomGetUpNight(object o)
        {
            this._logic.ScenceBedRoomGetUpNight();
            return o;
        }

        #endregion

        #region 书房面板
        #region 看书场景
        private void StudyRoomScenceWatchBookStart()
        {
            this._logic.StudyRoomScenceIsBusy = true;
            this.c2nicb_StudyRoom.Feedbacks[1].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[2].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[1].State = true;
            this.c2nicb_StudyRoom.Feedbacks[2].State = true;
            this.c2nicb_StudyRoom.Feedbacks[3].State = false;
            this.c2nicb_StudyRoom.Feedbacks[4].State = false;
            this.c2nicb_StudyRoom.Feedbacks[5].State = false;
            this.c2nicb_StudyRoom.Feedbacks[6].State = false;
        }
        private void StudyRoomScenceWatchBookFinished()
        {
            this.c2nicb_StudyRoom.Feedbacks[1].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_StudyRoom.Feedbacks[2].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.StudyRoomScenceIsBusy = false;
        }
        //public object StudyRoomScenceWatchBook(object o)
        //{
        //    this._logic.StudyRoomScenceIsBusy = true;
        //    this.c2nicb_StudyRoom.Feedbacks[1].BlinkPattern = eButtonBlinkPattern.SlowBlink;
        //    this.c2nicb_StudyRoom.Feedbacks[2].BlinkPattern = eButtonBlinkPattern.SlowBlink;
        //    this.c2nicb_StudyRoom.Feedbacks[1].State = true;
        //    this.c2nicb_StudyRoom.Feedbacks[2].State = true;
        //    this.c2nicb_StudyRoom.Feedbacks[3].State = false;
        //    this.c2nicb_StudyRoom.Feedbacks[4].State = false;
        //    this.c2nicb_StudyRoom.Feedbacks[5].State = false;
        //    this.c2nicb_StudyRoom.Feedbacks[6].State = false;
        //    this._logic.ScenceStudyRoomWatchBook();
        //    this.c2nicb_StudyRoom.Feedbacks[1].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
        //    this.c2nicb_StudyRoom.Feedbacks[2].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
        //    this._logic.StudyRoomScenceIsBusy = false; ;
        //    return o;
        //}

        #endregion
        #region 办公场景
        private void StudyRoomScenceOfficeStart()
        {
            this._logic.StudyRoomScenceIsBusy = true;
            this.c2nicb_StudyRoom.Feedbacks[3].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[4].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[1].State = false;
            this.c2nicb_StudyRoom.Feedbacks[2].State = false;
            this.c2nicb_StudyRoom.Feedbacks[3].State = true;
            this.c2nicb_StudyRoom.Feedbacks[4].State = true;
            this.c2nicb_StudyRoom.Feedbacks[5].State = false;
            this.c2nicb_StudyRoom.Feedbacks[6].State = false;
        }
        private void StudyRoomScenceOfficeFinished()
        {
            this.c2nicb_StudyRoom.Feedbacks[3].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_StudyRoom.Feedbacks[4].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.StudyRoomScenceIsBusy = false;
        }
        public object StudyRoomScenceOffice(object o)
        {
            this._logic.StudyRoomScenceIsBusy = true;
            this.c2nicb_StudyRoom.Feedbacks[3].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[4].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[1].State = false;
            this.c2nicb_StudyRoom.Feedbacks[2].State = false;
            this.c2nicb_StudyRoom.Feedbacks[3].State = true;
            this.c2nicb_StudyRoom.Feedbacks[4].State = true;
            this.c2nicb_StudyRoom.Feedbacks[5].State = false;
            this.c2nicb_StudyRoom.Feedbacks[6].State = false;

            this._logic.ScenceStudyRoomOffice();
            this.c2nicb_StudyRoom.Feedbacks[3].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_StudyRoom.Feedbacks[4].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.StudyRoomScenceIsBusy = false;
            return o;
        }

        #endregion

        public object StudyRoomScenceLeave(object o)
        {
            this._logic.StudyRoomScenceIsBusy = true;
            this.c2nicb_StudyRoom.Feedbacks[5].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[6].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[1].State = false;
            this.c2nicb_StudyRoom.Feedbacks[2].State = false;
            this.c2nicb_StudyRoom.Feedbacks[3].State = false;
            this.c2nicb_StudyRoom.Feedbacks[4].State = false;
            this.c2nicb_StudyRoom.Feedbacks[5].State = true;
            this.c2nicb_StudyRoom.Feedbacks[6].State = true;

            this._logic.ScenceStudyRoomLeave();


            this.c2nicb_StudyRoom.Feedbacks[5].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_StudyRoom.Feedbacks[6].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this._logic.StudyRoomScenceIsBusy = false ;
            return o;
        }
        public object StudyRoomWindowToggle(object o)
        {
            this._logic.StudyRoomScenceIsBusy = true;
            this.c2nicb_StudyRoom.Feedbacks[11].State = true;
            this.c2nicb_StudyRoom.Feedbacks[12].State = true;
            this.c2nicb_StudyRoom.Feedbacks[11].BlinkPattern = eButtonBlinkPattern.SlowBlink;
            this.c2nicb_StudyRoom.Feedbacks[12].BlinkPattern = eButtonBlinkPattern.SlowBlink;

            this._logic.StudyRoomWindowToggle();

            this.c2nicb_StudyRoom.Feedbacks[11].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_StudyRoom.Feedbacks[12].BlinkPattern = eButtonBlinkPattern.AlwaysOn;
            this.c2nicb_StudyRoom.Feedbacks[11].State = false;
            this.c2nicb_StudyRoom.Feedbacks[12].State = false;
            this._logic.StudyRoomScenceIsBusy = false;
            return o;
        }
        #endregion
        #region 会议室面板

        
        #endregion
        #endregion

        #region 16I事件
        void cp3_com_16I_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            //int exeid = 0;
            
            byte[] sendBytes = Encoding.ASCII.GetBytes(args.SerialData);
           // ILiveDebug.WriteLine("485Data:"+ILiveUtil.ToHexString(sendBytes));
            if (sendBytes != null && sendBytes.Length == 3)
            {
                if (sendBytes[0] == 0x1B)//地址27
                {
                    byte iChanIdx = sendBytes[1];
                    bool iChanStatus = Convert.ToBoolean(sendBytes[2]);
                    if (iChanIdx > 8)
                    {
                        if (9 == iChanIdx)/*RD[16]*/
                        {
                            Push_16I(16, iChanStatus);
                        }
                        else if ((iChanIdx <= 22) && (iChanIdx > 15))	/*RD[9] ~ RD[15]*/
                        {
                            /*iChanIdx 属于[16,22]*/
                            Push_16I(31 - iChanIdx, iChanStatus);
                        }
                    }
                    else
                    {
                        if (iChanIdx > 0)/*RD[1] ~ RD[8]*/
                        {
                            Push_16I(9 - iChanIdx, iChanStatus);
                        }

                    }
                }

            }

            //RxQueue.Enqueue(args.SerialData);
        }
        public void Push_16I(int id,bool iChanStatus)
        {
            if (iChanStatus)
            {
                switch (id)
                {
                    case 1:

                        this._logic.OfficeLightToggle();
                        break;
                    case 2:
                        this._logic.FrontDoorLightToggle();
                        break;
                    case 3:
                        this._logic.AisleLightToggle();
                        break;
                    case 4:
                        this._logic.Office2LightToggle();
                        break;
                    case 5:
                        this._logic.Aisle2LightToggle();
                        break;
                    case 6:
                        //this._logic.Office2LightToggle();
                        break;
                    case 7:
                        this._logic.DeputyGeneralLightToggle();
                        break;
                    case 8:
                        this._logic.DeputyGeneralLightToggle();
                        break;
                    case 9:
                        this._logic.DeputyGeneralLightToggle();
                        break;
                    case 10:
                        this._logic.LaboratoryLightToggle();
                        break;
                    case 11:
                        this._logic.LaboratoryLightToggle();
                        break;
                    case 12:
                        this._logic.LaboratoryLightToggle();
                        break;
                    case 13:
                        this._logic.GeneralStoreLightToggle();
                        break;
                    case 14:
                        this._logic.GeneralStoreLightToggle();
                        break;
                    case 15:
                        this._logic.GeneralStoreLightToggle();
                        break;
                    default:
                        break;
                }
            }
            
        }
        #endregion

        #region 手机事件
        /// <summary>
        /// 手机信号
        /// </summary>
        /// <param name="currentDevice"></param>
        /// <param name="args"></param>
        void mobile_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 10寸屏事件
        /// <summary>
        /// 10寸屏信号
        /// </summary>
        /// <param name="currentDevice"></param>
        /// <param name="args"></param>
        void tsw1052_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            ILiveDebug.Instance.WriteLine("TSW1052Sig:"+args.Sig.Number + args.Sig.Type);
            switch (args.Sig.Type)
            {
                case eSigType.Bool:
                    {
                        this.ProcessTsw1052Bool(args.Sig.Number, args.Sig.BoolValue);
                    }
                    break;
                case eSigType.UShort:
                    {
                        {
                            this.ProcessTsw1052UShort(args.Sig.Number, args.Sig.UShortValue);
                        }
                    }
                    break;
            }

        }
        private void ProcessTsw1052UShort(uint sigNumber, ushort shortValue)
        {
            TSW1052UShort t = (TSW1052UShort)sigNumber;
            switch (t)
            {
                #region 客厅灯光
                case TSW1052UShort.Foyer_Light_Level:
                    this._logic.SetLivingFoyerLightLevel(shortValue);
                    break;
                case TSW1052UShort.Living_DropLight_Level:
                    this._logic.SetLivingDropLightLevel(shortValue);
                    break;
                case TSW1052UShort.Living_LightBelt_Level:
                    this._logic.SetLivingLightBeltLevel(shortValue);
                    break;
                case TSW1052UShort.Living_FrontLight_Level:
                    this._logic.SetLivingFrontLightLevel(shortValue);
                    break;
                case TSW1052UShort.Living_RightLight_Level:
                    this._logic.SetLivingRightLightLevel(shortValue);
                    break;
                case TSW1052UShort.Living_BackLight_Level:
                    this._logic.SetLivingBackLightLevel(shortValue);
                    break;
                #endregion
                default:
                    break;
            }

        }
        /// <summary>
        /// 触摸屏数字量处理
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        private void ProcessTsw1052Bool(uint sigNumber, bool boolValue)
        {
            if (boolValue)
            {
                TSW1052Bool t = (TSW1052Bool)sigNumber;
                switch (t)
                {
                    #region 页面切换
                    case TSW1052Bool.Page_Home:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = false;
                        tsw1052.StringInput[11].StringValue = "";
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = false;
                        break;
                    case TSW1052Bool.Page_Camera:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = false;
                        if (this._logic.CameraShow==1)
                        {
                            Thread.Sleep(2000);
                            tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = true;
                            tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_01].BoolValue = true;
                            tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_02].BoolValue = false;
                            
                            tsw1052.StringInput[11].StringValue = "rtsp://admin:ms1234@192.168.1.190:554/main";
                        }
                        else if (this._logic.CameraShow == 2)
                        {
                            Thread.Sleep(2000);
                            tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_01].BoolValue = false;
                            tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_02].BoolValue = true;
                            tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = true;
                           
                            tsw1052.StringInput[11].StringValue = "rtsp://admin:ms1234@192.168.1.191:554/main";
                        }
                        break;
                    case TSW1052Bool.Page_Climate:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = false;
                        tsw1052.StringInput[11].StringValue = "";
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = false;
                        break;
                    case TSW1052Bool.Page_Curtains:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = false;
                        tsw1052.StringInput[11].StringValue = "";
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = false;
                        break;
                    case TSW1052Bool.Page_Leave:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = false;
                        tsw1052.StringInput[11].StringValue = "";
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = false;
                        break;
                    case TSW1052Bool.Page_Lights:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = false;
                        tsw1052.StringInput[11].StringValue = "";
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = false;
                        break;
                    case TSW1052Bool.Page_Music:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = false;
                        tsw1052.StringInput[11].StringValue = "";
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = false;
                        break;
                    case TSW1052Bool.Page_Security:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = false;
                        tsw1052.StringInput[11].StringValue = "";
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = false;
                        break;
                    case TSW1052Bool.Page_Theater:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Home].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Camera].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Climate].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Curtains].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Leave].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Lights].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Music].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Security].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_Theater].BoolValue = true;
                        tsw1052.StringInput[11].StringValue = "";
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = false;
                        break;
                    #endregion
                    #region 客厅场景
                    case TSW1052Bool.Scence_Man:
                        try
                        {
                            this._logic.ScenceLivingMan();
                        }
                        catch (Exception)
                        {
                        }
            
                        break;
                    case TSW1052Bool.Scence_Woman:
                        try
                        {

                            this._logic.ScenceLivingWoman();
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case TSW1052Bool.Scence_Oldman:
                        try
                        {
                            this._logic.ScenceLivingOldMan();
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case TSW1052Bool.Scence_Baby:
                        try
                        {
                            this._logic.ScenceLivingBaby();
                        }
                        catch (Exception)
                        {
                        }
                     
                        break;
                    case TSW1052Bool.Scence_Leave:
                        try
                        {
                            this._logic.ScenceHomeLeave();
                        }
                        catch (Exception)
                        {
                        }
                  
                        break;
                    #endregion
                    #region 灯光场景
                    case TSW1052Bool.Living_Light_AllOpen:
                        this._logic.LivingLightAllOpen();
                        break;
                    case TSW1052Bool.Living_Light_AllClose:
                        this._logic.LivingLightAllClose();
                        break;
                    case TSW1052Bool.Living_Light_Normal://照明灯光
                        this._logic.LivingLightNormal();
                        break;
                    case TSW1052Bool.Living_Light_SaveElectricity://节电灯光
                        this._logic.LivingLightSaveElectricity();
                        break;
                    case TSW1052Bool.Living_Light_Lesure://休闲灯光
                        this._logic.LivingLightLesure();
                        break;
                    case TSW1052Bool.Living_Light_CustomSence_Recall://自定义灯光
                        this._logic.LivingLightCustom();
                        break;
                    case TSW1052Bool.Living_Light_CustomSence_Store://保存为自定义灯光
                        this._logic.LivingLightCustomSave();
                        break;
                    #endregion
                    #region 窗帘
                    case TSW1052Bool.Living_WindowClose:
                        this._logic.LivingCurtainsClose();
                        break;
                    case TSW1052Bool.Living_WindowStop:
                        this._logic.LivingCurtainsStop();
                        break;
                    case TSW1052Bool.Living_WindowOpen:
                        this._logic.LivingCurtainsOpen();
                        break;
                    case TSW1052Bool.Living_ShaChuan_Close:
                        this._logic.LivingCurtains1Close();
                        break;
                    case TSW1052Bool.Living_ShaChuan_Stop:
                        this._logic.LivingCurtains1Stop();
                        break;
                    case TSW1052Bool.Living_ShaChuan_Open:
                        this._logic.LivingCurtains1Open();
                        break;
                    case TSW1052Bool.BedRoom_WindowClose:
                        this._logic.BedRoomCurtainsClose();
                        break;
                    case TSW1052Bool.BedRoom_WindowStop:
                        this._logic.BedRoomCurtainsStop();
                        break;
                    case TSW1052Bool.BedRoom_WindowOpen:
                        this._logic.BedRoomCurtainsOpen();
                        break;
                    case TSW1052Bool.StudyRoom_Curtains_Up:
                        this._logic.StudyRoomWindowUp();
                        break;
                    case TSW1052Bool.StudyRoom_Curtains_Down:
                        this._logic.StudyRoomWindowDown();
                        break;
                    #endregion
                    #region 客厅影院
                    case TSW1052Bool.Living_MovieOn:
                        this._logic.LivingMovieOn();
                        break;
                    case TSW1052Bool.Living_MovieOff:
                        this._logic.LivingMovieOn();
                        break;
                    case TSW1052Bool.Living_Blueray_TopMenu:
                        break;
                    case TSW1052Bool.Living_Blueray_Title:
                        break;
                    case TSW1052Bool.Living_AVR_VolUp:
                        break;
                    case TSW1052Bool.Living_AVR_VolDown:
                        break;
                    case TSW1052Bool.Living_Blueray_Ejct:
                        break;
                    case TSW1052Bool.Living_Blueray_SubTitle:
                        break;
                    case TSW1052Bool.Living_Blueray_Audio:
                        break;
                    case TSW1052Bool.Living_Blueray_Return:
                        break;
                    case TSW1052Bool.Living_Blueray_Up:
                        break;
                    case TSW1052Bool.Living_Blueray_Down:
                        break;
                    case TSW1052Bool.Living_Blueray_Left:
                        break;
                    case TSW1052Bool.Living_Blueray_Right:
                        break;
                    case TSW1052Bool.Living_Blueray_Enter:
                        break;
                    case TSW1052Bool.Living_Blueray_Previous:
                        break;
                    case TSW1052Bool.Living_Blueray_Reverse:
                        break;
                    case TSW1052Bool.Living_Blueray_Play:
                        break;
                    case TSW1052Bool.Living_Blueray_Stop:
                        break;
                    case TSW1052Bool.Living_Blueray_Forward:
                        break;
                    case TSW1052Bool.Living_Blueray_Next:
                        break;
                         case TSW1052Bool.Living_Movie_HuaWei:
                        tsw1052.BooleanInput[101].BoolValue = false;
                        tsw1052.BooleanInput[100].BoolValue = true;
                       
                        break;
                         case TSW1052Bool.Living_Movie_Blueray:
                        tsw1052.BooleanInput[100].BoolValue = false ;
                        tsw1052.BooleanInput[101].BoolValue = true;
                        break;

                    #endregion
                    #region 背景音乐
                    case TSW1052Bool.Music_JueShi_i:
                        this._logic.MusicJueShi();
                        
                        break;
                    case TSW1052Bool.Music_GangQin_i:
                        this._logic.MusicGangQing();
                        break;
                    case TSW1052Bool.Music_XiangCun_i:
                        this._logic.MusicXiangCun();
                        break;
                    case TSW1052Bool.Music_ZOOM1_Switch:
                        this._logic.MusicSetZoom1();
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM1_Switch].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM2_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM3_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM4_Switch].BoolValue = false;
                        break;
                    case TSW1052Bool.Music_ZOOM2_Switch:
                        this._logic.MusicSetZoom2();
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM1_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM2_Switch].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM3_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM4_Switch].BoolValue = false;
                        break;
                    case TSW1052Bool.Music_ZOOM3_Switch:
                        this._logic.MusicSetZoom3();
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM1_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM2_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM3_Switch].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM4_Switch].BoolValue = false;
                        break;
                    case TSW1052Bool.Music_ZOOM4_Switch:
                        this._logic.MusicSetZoom4();
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM1_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM2_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM3_Switch].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Music_ZOOM4_Switch].BoolValue = true;
                        break;
                    case TSW1052Bool.Music_ON:
                        this._logic.MusicSetZoomOn();
                        this._logic.MusicSetZoomAux();
                        break;
                    case TSW1052Bool.Music_OFF:
                        this._logic.MusicSetZoomOff();
                        break;
                    case TSW1052Bool.Music_VOL_Up:
                        this._logic.MusicSetZoomVolUp();
                        break;
                    case TSW1052Bool.Music_VOL_Down:
                        this._logic.MusicSetZoomVolDown();
                        break;
                    #endregion
                    #region 监控
                    case TSW1052Bool.Camera_01:
                        this._logic.CameraSet(1);
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_01].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_02].BoolValue = false;
                        tsw1052.StringInput[11].StringValue = "rtsp://admin:ms1234@192.168.1.190:554/main";
                       // tsw1052.StringInput[11].StringEncoding = eStringEncoding.eEncodingUTF16;
                        break;
                    case TSW1052Bool.Camera_02:
                        this._logic.CameraSet(2);
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_01].BoolValue = false;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_02].BoolValue = true;
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Camera_On].BoolValue = true;
                        tsw1052.StringInput[11].StringValue = "rtsp://admin:ms1234@192.168.1.191:554/main";
                       // tsw1052.StringInput[11].StringEncoding = eStringEncoding.eEncodingASCII;
                        break;
                    #endregion
                    #region 空调
                    case TSW1052Bool.Living_Climate_ON:
                        this._logic.LivingTempOpen();
                        break;
                    case TSW1052Bool.Living_Climate_OFF:
                        this._logic.LivingTempClose();
                        break;
                    case TSW1052Bool.Living_Climate_CoolHight:
                        this._logic.LivingTempCoolHight();
                        break;
                    case TSW1052Bool.Living_Climate_CoolCenter:
                        this._logic.LivingTempCoolCenter();
                        break;
                    case TSW1052Bool.Living_Climate_CoolLower:
                        this._logic.LivingTempCoolLower();
                        break;
                    case TSW1052Bool.Living_Climate_HotHight:
                        this._logic.LivingTempHotHight();
                        break;
                    case TSW1052Bool.Living_Climate_HotCenter:
                        this._logic.LivingTempHotCenter();
                        break;
                    case TSW1052Bool.Living_Climate_HotLower:
                        this._logic.LivingTempHotLower();
                        break;
                    #endregion
                    #region 安防
                    case TSW1052Bool.Security_DoorOpen:
                        this._logic.DoorOpen();
                        break;
                    case TSW1052Bool.Security_DoorClose:
                        this._logic.DoorClose();
                        break;
                    case TSW1052Bool.Security_Answer:
                        tsw1052.ExtenderVoipReservedSigs.Answer();
                        break;
                    case TSW1052Bool.Security_HangUp:
                        tsw1052.BooleanInput[205].BoolValue = false;
                        break;
                    case TSW1052Bool.Security_ShowVideo:
                        tsw1052.BooleanInput[205].BoolValue = true;
                        break;
                    case TSW1052Bool.Security_Start://布防
                        this._logic.SecurityStart();
                        break;
                    case TSW1052Bool.Security_End://撤防
                        this._logic.SecurityEnd();
                        break;
                    #endregion
                    //case TSW1052Bool.Incoming:
                    //    tsw1052.BooleanInput[205].BoolValue = true;
                    //    break;
                    //case TSW1052Bool.Hangup:
                    //    tsw1052.BooleanInput[205].BoolValue = false;
                    //    break;
                    case TSW1052Bool.Security_CloseCallIn:
                        tsw1052.BooleanInput[205].BoolValue = false;
                        break;
                    case TSW1052Bool.SIPHangup:
                        tsw1052.BooleanInput[(uint)TSW1052Bool.Page_SIPIncoming].BoolValue = false;
                        Thread.Sleep(500);
                        tsw1052.BooleanInput[(uint)TSW1052Bool.SIPEnableVideo].BoolValue = false;
                        Thread.Sleep(500);
                        tsw1052.BooleanInput[(uint)TSW1052Bool.SIPShowVideo].BoolValue = false;
                        this.tsw1052.ExtenderVoipReservedSigs.Hangup();
                        break;
                   // case TSW1052Bool.CallActive:
                    //    tsw1052.BooleanInput[205].BoolValue = false;
                     //   break;
                    default:
                        break;
                }
            }

        }
        private void BusyPageShow()
        {
            ILiveDebug.Instance.WriteLine("BusyPageShow");
            tsw1052.BooleanInput[314].BoolValue = true;
        }
        private void BusyPageClose()
        {
            ILiveDebug.Instance.WriteLine("BusyPageClose");
            tsw1052.BooleanInput[314].BoolValue = false;
        }
        #endregion

        #region 感应器事件
        /// <summary>
        /// 门口感应器信号
        /// </summary>
        /// <param name="device"></param>
        /// <param name="args"></param>
        void glsOirCCn_Door_GlsOccupancySensorChange(GlsOccupancySensorBase device, GlsOccupancySensorChangeEventArgs args)
        {
           // ILiveDebug.WriteLine("sig:" + args.SigDetail.Name+"|"+args.SigDetail.ToString());
           // throw new NotImplementedException();
        }
        /// <summary>
        /// 卫生间感应器信号
        /// </summary>
        /// <param name="device"></param>
        /// <param name="args"></param>
        void glsOirCCn_WashRoom_GlsOccupancySensorChange(GlsOccupancySensorBase device, GlsOccupancySensorChangeEventArgs args)
        {
            ILiveDebug.Instance.WriteLine("sig:" + args.SigDetail.Name);

            
           //throw new NotImplementedException();
        }

        #endregion

        void WebSocketUI_DataReceived(object sender, string message, EventArgs e)
        {
            ILiveDebug.Instance.WriteLine("IpadReceiveed:" + message);
            if (!message.StartsWith("CP3IPADCMD:"))
            {
                return;
            }
            message = message.Replace("CP3IPADCMD:", "");

            switch (message)
            {
                #region 场景
                case "MettingLight":
                    this.webui.WSServer_Send("CP3IPADRET:MettingLightToggleStart");
                    this._logic.MettingLightToggle();
                    this.webui.WSServer_Send("CP3IPADRET:MettingLightToggleEnd");
                    break;
                #endregion



                #region 灯光

                #endregion


                default:
                    break;
            }
            //this.WSServer_Send(DateTime.Now.ToLongTimeString() + "CP3:" + message);
            //WebSocket接收消息
        }
    }
}