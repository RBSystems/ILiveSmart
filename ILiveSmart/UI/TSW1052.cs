using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpProInternal;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace ILiveSmart.UI
{
    public enum TSW1052Bool
    {
        Living_Light_AllOpen=10,
        Living_Light_AllClose=11,
        /// <summary>
        /// 照明
        /// </summary>
        Living_Light_Normal=12,
        /// <summary>
        /// 节电
        /// </summary>
        Living_Light_SaveElectricity=13,
        /// <summary>
        /// 休闲
        /// </summary>
        Living_Light_Lesure=14,
        /// <summary>
        /// 自定义
        /// </summary>
        Living_Light_CustomSence_Recall=15,
        Living_Light_CustomSence_Store=16,

        Living_WindowClose=50,
        Living_WindowStop=51,
        Living_WindowOpen=52,
        Living_ShaChuan_Close=53,
        Living_ShaChuan_Stop=54,
        Living_ShaChuan_Open=55,
        BedRoom_WindowClose=56,
        BedRoom_WindowStop=57,
        BedRoom_WindowOpen=58,
        StudyRoom_Curtains_Up=59,
        StudyRoom_Curtains_Stop=60,
        StudyRoom_Curtains_Down=61,

        Living_MovieOn=70,
        Living_MovieOff = 71,
        Living_Blueray_TopMenu = 72,
        Living_Blueray_Title = 73,
        Living_AVR_VolUp = 74,
        Living_AVR_VolDown = 75,
        Living_Blueray_Ejct = 76,
        Living_Blueray_SubTitle = 77,
        Living_Blueray_Audio = 78,
        Living_Blueray_Return = 79,
        Living_Blueray_Up = 80,
        Living_Blueray_Down = 81,
        Living_Blueray_Left = 82,
        Living_Blueray_Right = 83,
        Living_Blueray_Enter = 84,
        Living_Blueray_Previous = 85,
        Living_Blueray_Reverse = 86,
        Living_Blueray_Play=88,
        Living_Blueray_Stop=89,
        Living_Blueray_Forward = 90,
        Living_Blueray_Next = 91,

        Living_Movie_HuaWei=100,
        Living_Movie_Blueray=101,

        Music_JueShi_i=130,
        Music_XiangCun_i=131,
        Music_GangQin_i=132,

        Music_ZOOM1_Switch = 140,
        Music_ZOOM2_Switch = 141,
        Music_ZOOM3_Switch = 142,
        Music_ZOOM4_Switch = 143,

        Music_ON=145,
        Music_OFF=146,
        Music_VOL_Up=147,
        Music_VOL_Down=148,

        Camera_On = 160,
        Camera_01=161,
        Camera_02=162,

        Living_Climate_ON=180,
        Living_Climate_OFF=181,
        Living_Climate_CoolHight=182,
        Living_Climate_CoolCenter=183,
        Living_Climate_CoolLower=184,
        Living_Climate_HotHight=185,
        Living_Climate_HotCenter=186,
        Living_Climate_HotLower=187,

        Security_DoorOpen = 200,
        Security_DoorClose = 201,
        Security_Start=202,
        Security_End=203,
        Security_ShowVideo=204,
        Security_ShowCallIn = 205,
        Security_CloseCallIn = 206,
        Security_Answer=207,
        Security_HangUp=208,

        Scence_Man=250,
        Scence_Woman=251,
        Scence_Oldman=252,
        Scence_Baby=253,
        Scence_Leave=254,

        Page_Home=305,
        Page_Lights=306,
        Page_Curtains=307,
        Page_Theater=308,
        Page_Music=309,
        Page_Camera=310,
        Page_Climate=311,
        Page_Security=312,
        Page_Leave=313,
        Page_Wait=314,
        Page_SIPIncoming=315,

        SIPHangup=400,
        SIPShowVideo=401,
        SIPEnableVideo=402,

        Incoming=27226,
        Hangup = 27221,
        CallActive = 27224,
        Answer=27219,

    }

    public enum TSW1052UShort
    {
        Foyer_Light_Level=20,
        Living_DropLight_Level=21,
        Living_LightBelt_Level=22,
        Living_FrontLight_Level=23,
        Living_RightLight_Level=24,
        Living_BackLight_Level=25
    }
    public enum TSW1052String
    {
        Foyer_Light_Percent_FB = 20,
        Living_DropLight_Percent_FB = 21,
        Living_LightBelt_Percent_FB = 22,
        Living_FrontLight_Percent_FB = 23,
        Living_RightLight_Percent_FB = 24,
        Living_BackLight_Percent_FB = 25
    }

    internal sealed class Class63 : EthernetDevice
    {
        private sealed class Class64 : CrestronDeviceWithEvents
        {
            internal Class64(CrestronControlSystem crestronControlSystem_0, CrestronDevice crestronDevice_0)
                : base(crestronDevice_0)
            {
                this.NotRegisterable = true;
                this.ID = 1u;
                base.InitializeParamBitFieldOne(1335040u);
                base.InitializeParamBitFieldTwo(196864u);
                base.InitializeParamBitFieldThree(81u);
                base.InitializeParamBitFieldFour(0u);
                base.InitializeDigitalOutputJoins(5);
            }
        }
        //internal TsxCcsUcCodec100AudioReservedSigs tsxCcsUcCodec100AudioReservedSigs_0;
        //internal TsxCcsUcCodec100EthernetReservedSigs tsxCcsUcCodec100EthernetReservedSigs_0;
        //internal TswFtSystemReservedSigs tswFtSystemReservedSigs_0;
        //internal Tswx52System3ReservedSigs tswx52System3ReservedSigs_0;
        //internal Tswx52VoipReservedSigs tswx52VoipReservedSigs_0;
        //internal Tswx52VoiceControlReservedSigs tswx52VoiceControlReservedSigs_0;
        //internal TsxScreenSaverReservedSigs tsxScreenSaverReservedSigs_0;
        //internal Tstx02ApplicationControlReservedSigs tstx02ApplicationControlReservedSigs_0;
        internal Class63(uint uint_0, CrestronControlSystem crestronControlSystem_0)
            : base(uint_0, "127.0.0.1", crestronControlSystem_0)
        {
            this.DeviceRegistrationFlags = 9u;
            base.InitializeParamBitFieldOne(1400594u);
            base.InitializeParamBitFieldTwo(131328u);
            base.InitializeParamBitFieldThree(66u);
            base.InitializeParamBitFieldFour(0u);
            base.LockDeviceTypes();
            base.InitializeDeviceSymbolType(-12);
            base.InitializeParametersOfDeviceRegistrationForEthernetDevices();
            base.DeviceRegistrationMethod = 6u;
            Class63.Class64 deviceToAdd = new Class63.Class64(crestronControlSystem_0, this);
            CrestronDevice.AddToDeviceCollection(this.Slots, 1u, deviceToAdd);
            base.InitializeDigitalInputJoins(16000);
            base.InitializeDigitalOutputJoins(16000);
            base.InitializeAnalogInputJoins(16000);
            base.InitializeAnalogOutputJoins(16000);
            base.InitializeSerialInputJoins(16000);
            base.InitializeSerialOutputJoins(16000);
           // this.tsxCcsUcCodec100AudioReservedSigs_0 = new TsxCcsUcCodec100AudioReservedSigs(this);
            //this.tsxCcsUcCodec100EthernetReservedSigs_0 = new TsxCcsUcCodec100EthernetReservedSigs(this);
            //this.tswFtSystemReservedSigs_0 = new TswFtSystemReservedSigs(this);
            //this.tswx52System3ReservedSigs_0 = new Tswx52System3ReservedSigs(this);
            //this.tswx52VoipReservedSigs_0 = new Tswx52VoipReservedSigs(this);
            //this.tswx52VoiceControlReservedSigs_0 = new Tswx52VoiceControlReservedSigs(this);
            //this.tsxScreenSaverReservedSigs_0 = new TsxScreenSaverReservedSigs(this);
            //this.tstx02ApplicationControlReservedSigs_0 = new Tstx02ApplicationControlReservedSigs(this);
        }
    }

}
