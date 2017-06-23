using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;

namespace ILiveSmart
{
    public class SMSAPI
    {
        /// <summary>
        /// 消息队列 下一版本使用
        /// </summary>
        private CrestronQueue msgQueue = new CrestronQueue();

        PDUEncoding pe = new PDUEncoding();

        private ComPort _comSMS = null;
        public bool HasNewMsg = false;
        public string ReceivedData = "";
        public SMSAPI(ComPort com)
        {
            this._comSMS = com;
            this._comSMS.SerialDataReceived += new ComPortDataReceivedEvent(_comSMS_SerialDataReceived);
        }

        void _comSMS_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {

            HasNewMsg = true;
            ReceivedData= args.SerialData;
           // throw new NotImplementedException();
        }

        public void Open()
        {

            //初始化设备
            if (this._comSMS.Registered)
            {
                //this._comSMS.SerialDataReceived -= _comSMS_SerialDataReceived;

                //更新添加连接识别
                this._comSMS.Send("AT\r");
                int i = 0;
                while (!HasNewMsg)
                {
                    if (i < 10)
                    {
                        i++;
                        Thread.Sleep(500);
                    }
                    else
                    {
                        break;
                    }

                }
                HasNewMsg = false;
                try
                {
                    
                    SendAT("ATE0");
                    SendAT("AT+CMGF=0");
                  //  SendAT("AT+CNMI=2,1");
                }
                catch { }
            }
        }
        /// <summary>
        /// 发送AT指令 逐条发送AT指令 调用一次发送一条指令
        /// 能返回一个OK或ERROR算一条指令
        /// </summary>
        /// <param name="ATCom">AT指令</param>
        /// <returns>发送指令后返回的字符串</returns>
        public string SendAT(string ATCom)
        {
            string result = string.Empty;

            //注销事件关联，为发送做准备
           // this._comSMS.SerialDataReceived -= _comSMS_SerialDataReceived;
            //发送AT指令
            try
            {
               // ILiveDebug.WriteLine("SMSSend" + ATCom);
                this._comSMS.Send(ATCom + "\r");
            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine(ex.Message);
                //this._comSMS.SerialDataReceived += _comSMS_SerialDataReceived;
              //  throw ex;
            }

            //接收数据 循环读取数据 直至收到“OK”或“ERROR”
            try
            {
                int i = 0;
                while (!HasNewMsg)
                {
                    if (i < 10)
                    {
                        i++;
                        Thread.Sleep(500);
                    }
                    else
                    {
                        break;
                    }

                }
                HasNewMsg = false;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //事件重新绑定 正常监视串口数据
                //this._comSMS.SerialDataReceived += _comSMS_SerialDataReceived;
            }
        }

        /// <summary>
        /// 发送短信
        /// 发送失败将引发异常
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="msg">短信内容</param>
        public void SendMsg(string phone, string msg)
        {

            PDUEncoding pe = new PDUEncoding();
            string temp = pe.PDUEncoder(phone, msg);

            int len = (temp.Length - Convert.ToInt32(temp.Substring(0, 2), 16) * 2 - 2) / 2;  //计算长度
            try
            {

                //注销事件关联，为发送做准备
              //  this._comSMS.SerialDataReceived -= _comSMS_SerialDataReceived;
                this._comSMS.Send("AT+CMGS=" + len.ToString() + "\r");
                int i = 0;
                while (!HasNewMsg)
                {
                    if (i<10)
                    {
                        i++;
                        Thread.Sleep(500);
                    }
                    else
                    {
                        break;
                    }

                } 

                HasNewMsg = false;
                //if (this.ReceivedData == ">")
                //{
                //   // ILiveDebug.WriteLine("SMSRead>");
                //    // HasNewMsg = false; 
                //}
                //事件重新绑定 正常监视串口数据
              //  this._comSMS.SerialDataReceived += _comSMS_SerialDataReceived;

                temp = SendAT(temp + (char)(26));  //26 Ctrl+Z ascii码
                ILiveDebug.Instance.WriteLine(temp);

                if (temp.Substring(temp.Length - 4, 3).Trim() == "OK")
                {
                    return;
                }
            }
            catch (Exception)
            {
                ErrorLog.Error("Send SMS Error");
               // throw new Exception("短信发送失败");
            }
            finally
            {
            }


          //  ILiveDebug.Instance.WriteLine("5");

           // throw new Exception("短信发送失败");
        }

        
    }
}