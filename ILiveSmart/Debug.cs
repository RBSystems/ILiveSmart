using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmart
{
    public class ILiveDebug
    {
        public static readonly ILiveDebug Instance = new ILiveDebug();

        private UDPClient client = null;

        private ILiveDebug()
        {
            client = new UDPClient("192.168.1.41", 8800);
            
        }
        private void SendData(string data)
        {
            CrestronConsole.PrintLine(data);

          /*  byte[] sendBytes = Encoding.ASCII.GetBytes(data);
            
            client.Connect();
            client.SendData(sendBytes);
            client.DisConnect();*/
        }

        public void WriteLine(string msg)
        {
            this.SendData(msg+"\r\n");
        }
        public void WriteLine(string msg, params object[] args)
        {
            string message = string.Format(msg, args);
            this.SendData(message);
        }
    }
}