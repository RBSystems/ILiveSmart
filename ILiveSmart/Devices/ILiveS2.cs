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
    /// S2电源时序器
    /// </summary>
    public class ILiveS2
    {
        //public IROutputPort myPort;//S2电源时序器

        public ILiveS2()
        {
           // this.myPort = sys.IROutputPorts[1];
           // this.myPort.SetIRSerialSpec(eIRSerialBaudRates.ComspecBaudRate19200, eIRSerialDataBits.ComspecDataBits8, eIRSerialParityType.ComspecParityNone, eIRSerialStopBits.ComspecStopBits1, Encoding.ASCII);

        }
        public static string S2Open1()
        {
            return S2Relay(1, true);
        }
        public static string S2Close1()
        {
            return S2Relay(1, false);
        }
        /// <param name="port">第几路 1-8</param>
        /// <param name="status">true:闭合 false：断开</param>
        public static string S2Relay(int port, bool status)
        {
            if (status)
            {
                //ILiveDebug.Instance.WriteLine(string.Format("*001O{0}#", port));
                
                //this.myPort.SendSerialData(string.Format("*001O{0}#", port));//
                return string.Format("*001O{0}#", port);
            }
            else
            {
                return string.Format("*001C{0}#", port);

               // ILiveDebug.Instance.WriteLine(string.Format("*001C{0}#", port));
               // this.myPort.SendSerialData(string.Format("*001C{0}#",port));//电脑
            }
        }
       
    }
}