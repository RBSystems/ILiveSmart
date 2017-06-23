using System;
using System.Collections.Generic;
using System.Text;

namespace SimplSharpPro
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msg= "test";
                byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(msg);

                System.Net.Sockets.UdpClient udpClientB = new System.Net.Sockets.UdpClient();
                udpClientB.Send(sendBytes, sendBytes.Length, "192.168.1.167", 6890);
                udpClientB.Close();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
