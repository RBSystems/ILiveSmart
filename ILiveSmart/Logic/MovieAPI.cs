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
    /// 影院控制接口
    /// </summary>
    public class MovieAPI
    {
        IROutputPort irBedRoom = null;
        public Relay relayBedRoomScreenUp;
        public Relay relayBedRoomScreenDown;
        public MovieAPI(CP3Smart smartexe)
        {
            this.relayBedRoomScreenUp = smartexe.relayBedRoomScreenUp;
            this.relayBedRoomScreenDown = smartexe.relayBedRoomScreenDown;

            this.irBedRoom = smartexe.myIROutputPort2;

            try
            {
                 irBedRoom.LoadIRDriver( Crestron.SimplSharp.CrestronIO.Directory.GetApplicationDirectory() + "\\IR\\OnkyoRecv.ir");
                 irBedRoom.LoadIRDriver(Crestron.SimplSharp.CrestronIO.Directory.GetApplicationDirectory() + "\\IR\\LG.ir");
            }
            catch (Exception ex)
            {

                ILiveDebug.Instance.WriteLine(ex.Message);
            }
        }

       // YuTaiExecute yt = new YuTaiExecute();
        GuangYinExecute gy = new GuangYinExecute();
        #region 客厅影院
        public void LivingProjectorUp()
        {

            byte[] code = { 0x00, 0xFE, 0x01, 0x00, 0xFE, 0x01, 0x00, 0xFE, 0x01, 0x00, 0xFE, 0x01 };
            //gy.SendPort10(code);
        }
        public void LivingProjectorDown()
        {
            
            byte[] code = { 0x00,0xFE,0x02,0x00,0xFE,0x02,0x00,0xFE,0x02,0x00,0xFE,0x02 };
            //gy.SendPort10(code);
        }
        public void LivingProjectorOn()
        {
            gy.SendPort1(Encoding.GetEncoding(28591).GetBytes("(PWR1)"));
        }
        public void LivingProjectorOff()
        {
            gy.SendPort1(Encoding.GetEncoding(28591).GetBytes("(PWR0)"));
        }
        public void LivingProjectorHDMI1()
        {
            gy.SendPort1(Encoding.GetEncoding(28591).GetBytes("(SRC5)"));
        }
        public void LivingAvrPowerOn()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:PWR=On"));
        }
        public void LivingAvrPowerOff()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:PWR=Standby"));
        }
        public void LivingAvrVolDown()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:VOL=Down"));
        }
        public void LivingAvrVolUp()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:VOL=Up"));
        }
        public void LivingAvrMuteOn()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:MUTE=On"));
        }
        public void LivingAvrMuteOff()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:MUTE=Off"));
        }
        public void LivingAvrAV1()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:INP=AV1"));
        }
        public void LivingAvrAV2()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:INP=AV2"));
        }
        public void LivingAvrAV3()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:INP=AV3"));
        }
        public void LivingAvrAV4()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:INP=AV4"));
        }
        public void LivingAvrAV5()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:INP=AV5"));
        }
        public void LivingAvrAV6()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:INP=AV6"));
        }
        public void LivingAvrAV7()
        {
            gy.SendPort2(Encoding.GetEncoding(28591).GetBytes("@MAIN:INP=AV7"));
        }
        #endregion
        #region 卧室影院
        public void BedRoomMovie(bool on)
        {
            if (on)
            {
                //开启影院
                Thread th = new Thread(new ThreadCallbackFunction(this.BedRoomMovieOn), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);
            }
            else
            {
                //关闭影院
                Thread th = new Thread(new ThreadCallbackFunction(this.BedRoomMovieOff), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);

            }
        }
        private object BedRoomMovieOn(object o)
        {
           
            if (GlobalSigInfo.Instance.BedRoomMovieStatus && GlobalSigInfo.Instance.BedRoomMovieBusy)
            {
                return o;
            }

            int i = 0;
            do
            {
                i++;
                Thread.Sleep(1000);
            } while (GlobalSigInfo.Instance.BedRoomMovieBusy && i < 10);


            GlobalSigInfo.Instance.BedRoomMovieBusy = true;
            this.BedRoomProjectorOn();//开启投影
            Thread.Sleep(1000);

            this.BedRoomSecreenDown();//屏幕下降
            Thread.Sleep(5000);
            this.BedRoomBluerayPowerOn();//开启碟机
            
            Thread.Sleep(2000);
            this.BedRoomAvrOn();//开启功放
            
            GlobalSigInfo.Instance.BedRoomMovieBusy = false;
            GlobalSigInfo.Instance.BedRoomMovieStatus = true;
            return o;
        }

        private object BedRoomMovieOff(object o)
        {

            if (!GlobalSigInfo.Instance.BedRoomMovieStatus && GlobalSigInfo.Instance.BedRoomMovieBusy)
            {
                return o;
            }

            int i = 0;
            do
            {
                i++;
                Thread.Sleep(1000);
            } while (GlobalSigInfo.Instance.BedRoomMovieBusy && i < 10);


            GlobalSigInfo.Instance.BedRoomMovieBusy = true;
            this.BedRoomProjectorOff();//关闭投影
            Thread.Sleep(1000);
            this.BedRoomSecreenUp();//屏幕上升
            Thread.Sleep(1000);

            this.BedRoomAvrOff();//关闭功放
            Thread.Sleep(1000);

            this.BedRoomBluerayPowerOff();//关闭碟机
            Thread.Sleep(1000);
            this.BedRoomProjectorOff();//关闭投影
            Thread.Sleep(1000);
            GlobalSigInfo.Instance.BedRoomMovieBusy = false;
            GlobalSigInfo.Instance.BedRoomMovieStatus = false;
            return o;
        }
        public void BedRoomProjectorOn()
        {
            byte[] data={0x50,0x57,0x52,0x20,0x4F,0x4E,0x0D};
            gy.SendBedRoomPort1(data);
            Thread.Sleep(1000);
            gy.SendBedRoomPort1(data);
        }
        public void BedRoomProjectorOff()
        {
            byte[] data={0x50,0x57,0x52,0x20,0x4F,0x46,0x46,0x0D};
            gy.SendBedRoomPort1(data);
            Thread.Sleep(1000);
            gy.SendBedRoomPort1(data);
        }
        public void BedRoomSecreenUp()
        {
            try
            {
                this.relayBedRoomScreenUp.Close();
                Thread.Sleep(1000);
                this.relayBedRoomScreenUp.Open();
            }
            catch (Exception)
            {
            }

        }
        public void BedRoomSecreenDown()
        {
            this.relayBedRoomScreenDown.Close();
            Thread.Sleep(1000);
            this.relayBedRoomScreenDown.Open();
        }

        internal void BedRoomAvrOn()
        {
            gy.SendBedRoomIR(2,1);

          //  this.irBedRoom.Press(1, "POWER_ON");
           // throw new NotImplementedException();
        }

        internal void BedRoomAvrOff()
        {
            gy.SendBedRoomIR(2, 2);
          //  this.irBedRoom.Press(1, "POWER_OFF");
        }
        internal void BedRoomBluerayPowerOn()
        {
            gy.SendBedRoomIR(1, 20);

         //   this.irBedRoom.Press(2, "POWER");
        }
        internal void BedRoomBluerayPowerOff()
        {
            gy.SendBedRoomIR(1, 21);

            //   this.irBedRoom.Press(2, "POWER");
        }
        #endregion
        internal void WashRoomTVOn()
        {
           // throw new NotImplementedException();
        }
    }
}
