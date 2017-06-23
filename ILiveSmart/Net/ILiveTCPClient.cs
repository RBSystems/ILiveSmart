using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net;
using Crestron.SimplSharp.CrestronSockets;

namespace ILiveSmart
{
    public class ILiveTCPClient
    {
        TCPClient client = new TCPClient();
        public ILiveTCPClient()
        {
            client.SocketStatusChange += new TCPClientSocketStatusChangeEventHandler(client_SocketStatusChange);
           // this.NewConnection(new Binding() { BindingAddress="",Port=5670});

            //this.Binding=new Binding(){d}
        }

        void client_SocketStatusChange(TCPClient myTCPClient, SocketStatus clientSocketStatus)
        {
           // throw new NotImplementedException();
        }
        public void Send(string ip, int port, string senddata)
        {
            try
            {
                client.AddressClientConnectedTo = ip;
                client.PortNumber = port;
                client.ConnectToServer();
                byte[] data = Encoding.GetEncoding(28591).GetBytes(senddata);
                client.SendData(data, 0, data.Length);
                client.DisconnectFromServer();
            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine("ILiveTCPClientSendEx:+"+ex.Message);
            }

        }
    }
}