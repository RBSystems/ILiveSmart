
//Please uncomment the #define line below if you want to include the sample code 
// in the compiled output.
// for the sample to work, you'll have to add a reference to the SimplSharpPro.UI dll to your project.
//#define IncludeSampleCode

using System;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       				// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using Crestron.SimplSharpPro.Diagnostics;		    		// For System Monitor Access
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.Lighting;
using Crestron.SimplSharpPro.Lighting.Din;
using Crestron.SimplSharpPro.Keypads;
using Crestron.SimplSharpPro.GeneralIO;
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharp.Net;
using System.Text;
using ILiveSmart.UI;
using ILiveSmart.Music;
#if IncludeSampleCode
using Crestron.SimplSharpPro.UI;                    			// For UI Devices. Please include the 
																// Crestron.SimplSharpPro.UI DLL as a reference to your project.
#endif

namespace ILiveSmart
{
    public class ControlSystem : CrestronControlSystem
    {
        //SmartExecute smartExec = null;
        //LightExecute lightExec = null;
        ILiveSmartAPI logic = null;
        #region 定义消息
        /// <summary>
        /// 接收消息队列
        /// </summary>
        private CrestronQueue<String> RxQueue = new CrestronQueue<string>();
        /// <summary>
        /// 接收事件
        /// </summary>
        private Thread tcpListenHandler;
        #endregion

        #region 初始化设备

        #endregion
        /// <summary>
        /// Constructor of the Control System Class. Make sure the constructor always exists.
        /// If it doesn't exit, the code will not run on your 3-Series processor.
        /// </summary>
        public ControlSystem()
            : base()
        {
            //此系统使用的最大线程数
            Thread.MaxNumberOfUserThreads = 30;
            
            //定义快思聪系统事件，例如：重启，开机完成等等
            CrestronEnvironment.SystemEventHandler += new SystemEventHandler(ControlSystem_ControllerSystemEventHandler);
            //定义程序事件，例如：停止、暂停、恢复等。
            CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(ControlSystem_ControllerProgramEventHandler);
            //定义快思聪网络事件。
            CrestronEnvironment.EthernetEventHandler += new EthernetEventHandler(ControlSystem_ControllerEthernetEventHandler);

        }

        /// <summary>
        /// Overridden function... Invoked before any traffic starts flowing back and forth between the devices and the 
        /// user program. 
        /// This is used to start all the user threads and create all events / mutexes etc.
        /// This function should exit ... If this function does not exit then the program will not start
        /// </summary>
        public override void InitializeSystem()
        {
            this.DefaultStringEncoding = eStringEncoding.eEncodingUTF16;

            //初始化逻辑
            logic = new ILiveSmartAPI(this);
            // System.Reflection.Assembly.Load("");
            
            //logic.Start();
            //初始化UI模块
            SmartInput input = new SmartInput(this, logic);
            input.Start();

            tcpListenHandler = new Thread(tcpListenMethod, null, Thread.eThreadStartOptions.Running);
  
            ILiveDebug.Instance.WriteLine("InitializeSystem Success");
            // This should always return   
          //  if (this.SupportsComPort && myComPort1.Registered)
            //    RxHandler = new Thread(RxMethod, null, Thread.eThreadStartOptions.Running);

        }

        /// <summary>
        /// 数据队列处理
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        object tcpListenMethod(object obj)
        {
            ILiveTCPServer tcp = new ILiveTCPServer(8800);
            tcp.TcpDataEvent += this.OnTcpReceived;
            tcp.Listen();
            return null;
        }
        public void OnTcpReceived(string data)
        {
            ILiveDebug.Instance.WriteLine("OnTcpReceived:" + data);
            switch (data)
            {
                case "m1":
                    this.logic.Test1();
                    break;
                case "m2":
                    this.logic.Test2();
                    break;
                default:
                    break;
            }


        }

        #region 网络事件
        /// <summary>
        /// This event is triggered whenever an Ethernet event happens. 
        /// </summary>
        /// <param name="ethernetEventArgs">Holds all the data needed to properly parse</param>
        void ControlSystem_ControllerEthernetEventHandler(EthernetEventArgs ethernetEventArgs)
        {
            switch (ethernetEventArgs.EthernetEventType)
            {//Determine the event type Link Up or Link Down
                case (eEthernetEventType.LinkDown):
                    //Next need to determine which adapter the event is for. 
                    //LAN is the adapter is the port connected to external networks.
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {
                        //
                    }
                    break;
                case (eEthernetEventType.LinkUp):
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {

                    }
                    break;
            }
        }

        #endregion

        #region 程序事件
        /// <summary>
        /// This event is triggered whenever a program event happens (such as stop, pause, resume, etc.)
        /// </summary>
        /// <param name="programEventType">These event arguments hold all the data to properly parse the event</param>
        void ControlSystem_ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
        {
            ILiveDebug.Instance.WriteLine(programStatusEventType.ToString());
            switch (programStatusEventType)
            {
                case (eProgramStatusEventType.Paused):
                    //The program has been paused.  Pause all user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Resumed):
                    //The program has been resumed. Resume all the user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Stopping):
                   // Crestron.SimplSharp.CrestronLogger.CrestronLogger.Initialize(10,Crestron.SimplSharp.CrestronLogger.LoggerModeEnum.CONSOLE);
                   // Crestron.SimplSharp.CrestronLogger.CrestronLogger.Clear(true);
                    //The program has been stopped.
                    //Close all threads. 
                    //Shutdown all Client/Servers in the system.
                    //General cleanup.
                    //Unsubscribe to all System Monitor events

                    //RxQueue.Enqueue(null); // The RxThread will terminate when it receives a null
                    break;
            }

        }

        #endregion

        #region 系统事件
        /// <summary>
        /// This handler is triggered for system events
        /// </summary>
        /// <param name="systemEventType">The event argument needed to parse.</param>
        void ControlSystem_ControllerSystemEventHandler(eSystemEventType systemEventType)
        {
           // ILiveDebug.WriteLine(systemEventType.ToString());
            switch (systemEventType)
            {
                case (eSystemEventType.DiskInserted):
                    //Removable media was detected on the system
                    break;
                case (eSystemEventType.DiskRemoved):
                    //Removable media was detached from the system
                    break;
                case (eSystemEventType.Rebooting):
                    //The system is rebooting. 
                    //Very limited time to preform clean up and save any settings to disk.
                    break;
            }

        }
        #endregion
    }
}
