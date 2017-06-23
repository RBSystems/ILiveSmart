using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmartTest
{
    public class ILiveDebug
    {

        public static void WriteLine(string msg)
        {
          //  UDPAPI.SendData();
            UDPAPI.Instance.SendData(msg + "\r\n");
            //byte[] sendBytes = Encoding.ASCII.GetBytes();
            //UDPClient client = new UDPClient(h, p);
            //client.Connect();
            //client.SendData(sendBytes);
            //client.DisConnect();

        }
        public static void WriteLine(string msg, params object[] args)
        {
            string message = string.Format(msg, args);
            UDPAPI.Instance.SendData(message);
        }
    }
}