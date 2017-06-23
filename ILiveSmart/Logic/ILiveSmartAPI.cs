using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.Lighting;
using Crestron.SimplSharpPro;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharpPro.CrestronThread;
using System.Collections;
using ILiveSmart.Light;
using ILiveSmart.Music;
namespace ILiveSmart
{
    public class ILiveSmartAPI
    {
        #region 初始化
        public delegate void ScenceEventHandler();
        //public delegate void ScenceButtonEventHandler(string s);
        public event ScenceEventHandler LivingScenceFinished;
        public event ScenceEventHandler LivingScenceStart;

        public event ScenceEventHandler StudyRoomScenceFinished;
        public event ScenceEventHandler StudyRoomScenceStart;

      //  public event ScenceEventHandler BedRoomScenceFinished;
      //  public event ScenceEventHandler BedRoomScenceStart;

        private CrestronControlSystem _controlSystem = null;

        private ILiveLight _lightExec = null;
       // private MusicAPI _music = null;
        private SecurityAPI _security = null;
        private MovieAPI _movie = null;
        private ClinateAPI _clinate = null;
        private ALiYunSMS _sms = null;
        private CurtainsAPI _curtains = new CurtainsAPI();
        private CP3Smart smartExec = null;
        public ILiveSmartAPI(CrestronControlSystem system)
        {
            this._controlSystem = system;

            this.Init();

        }

        private void Init()
        {
            smartExec = new CP3Smart(this._controlSystem);
            _lightExec = new ILiveLight(this._controlSystem);
           

            _lightExec.RegisterDevices();//注册灯光模块
           // Queue<Thread> dd = null;
            smartExec.RegisterDevices();//注册快思聪模块


            //this._music = new MusicAPI();
            this._security = new SecurityAPI(smartExec);
            this._security.SetYelaPressEvent(this.YelaPress);
            this._movie = new MovieAPI(smartExec);
            this._clinate = new ClinateAPI(smartExec);
           // this._sms = new SMSAPI(smartExec.comSMS);
            this._sms = new ALiYunSMS();
        }
        #endregion


        #region 场景
        #region 进门场景
        public bool BackHomeBusy = false;
        /// <summary>
        /// 正常开门回家场景
        /// 1、安防撤防 2、玄光灯70% 3、通知保姆（卧室背景音乐）
        /// </summary>
        public object ScenceHomeNormal(object o)
        {
            this.WaitBackHomeBusy();//判断系统是否正忙，否则就等待10秒
            this.BackHomeBusy = true;
            this._lightExec.LivingFoyerLightLevel = ushort.MaxValue / 100 * 50;//玄关灯亮度50%

            //安防撤防
            this.SecurityEnd();
            MusicAPI.Instance.MusicWelcome();
            Thread.Sleep(3000);
            this.BackHomeBusy = false;
            return o;
           // this._music.ZOOMALL_OFF();
        }
        /// <summary>
        /// 挟持回家场景
        /// 1、直接发送挟持短信
        /// </summary>
        public void ScenceHomeHolding(string phone)
        {
            this._sms.SendUnLockMsg(phone, phone);
          //  this.SMSSend(phone, "您的朋友可能被挟持，请妥善处理！！！[艾力智能展厅]");
        }
        private void WaitBackHomeBusy()
        {
            int i = 0;
            while (this.BackHomeBusy)
            {
                if (i < 10)
                {
                    i++;
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }
        }
        #endregion

        #region 客厅场景
        public bool ScenceBusy = false;
        private void WaitScenceBusy()
        {
            int i = 0;
            while (this.ScenceBusy)
            {
                if (i < 10)
                {
                    i++;
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 男主人回家场景
        /// 1、灯光明亮 2、纱窗关闭 3、窗帘打开 4、开启电视
        /// </summary>
        public void ScenceLivingMan()
        {
            new Thread(new ThreadCallbackFunction(this.ScenceLivingMan),this,Thread.eThreadStartOptions.Running);
        }
        /// <summary>
        /// 男主人回家场景
        /// 1、灯光明亮 2、纱窗打开 3、窗帘打开 4、开启电视
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object ScenceLivingMan(object o)
        {
            

            if (this.LivingScenceStart != null)
            {
                this.LivingScenceStart();//通知场景已经开始 用于显示屏显示正在执行页面
            }
            this.WaitScenceBusy();
            this.ScenceBusy = true;
            this.LivingLightNormal();
            //ILiveDebug.Instance.WriteLine("11");

            this.LivingCurtains1Open();
            //ILiveDebug.Instance.WriteLine("12");

            this.LivingCurtainsOpen();
            ILiveDebug.Instance.WriteLine("13");

            //this.LivingMovieOn();//取消影院场景
            this.LivingMusicOff();
           // ILiveDebug.Instance.WriteLine("1");
            Thread.Sleep(5000);
            //ILiveDebug.Instance.WriteLine("2");

            if (this.LivingScenceFinished != null)
            {
              //  ILiveDebug.Instance.WriteLine("3");

                this.LivingScenceFinished();//通知场景已经结束 用于显示屏停止显示正在执行页面
                //ILiveDebug.Instance.WriteLine("4");

            } 
            this.ScenceBusy = false;
            return o;
        }
        /// <summary>
        /// 女主人回家场景
        /// 1、灯光柔和 2、纱窗关闭 3、窗帘打开 4、背景音乐响起
        /// </summary>
        public void ScenceLivingWoman()
        {
            new Thread(new ThreadCallbackFunction(this.ScenceLivingWoman), this, Thread.eThreadStartOptions.Running);
        }

        /// <summary>
        /// 女主人回家场景
        /// 1、灯光柔和 2、纱窗关闭 3、窗帘打开 4、背景音乐响起
        /// </summary>
        public object ScenceLivingWoman(object o)
        {
            

            if (this.LivingScenceStart != null)
            {
                this.LivingScenceStart();
            }
            this.WaitScenceBusy();
            this.ScenceBusy = true;
            this.LivingLightLesure();
            this.LivingMusicOn();
            this.LivingCurtains1Close();
            this.LivingCurtainsOpen();
            //this.LivingMovieOff();//取消影院场景
            Thread.Sleep(5000);
            if (this.LivingScenceFinished != null)
            {
                this.LivingScenceFinished();
            }
            this.ScenceBusy = false;
            return o;
        }
        /// <summary>
        /// 小孩回家场景
        /// 1、灯光明亮 2、纱窗关闭 3、窗帘关闭 4、电视开启
        /// </summary>
        public void ScenceLivingBaby()
        {
            new Thread(new ThreadCallbackFunction(this.ScenceLivingBaby), this, Thread.eThreadStartOptions.Running);
        }

        /// <summary>
        /// 小孩回家场景
        /// 1、灯光明亮 2、纱窗关闭 3、窗帘关闭 4、电视开启
        /// </summary>
        public object ScenceLivingBaby(object o)
        {
           

            if (this.LivingScenceStart != null)
            {
                this.LivingScenceStart();
            }
            this.WaitScenceBusy();
            this.ScenceBusy = true;
            this.LivingLightSaveElectricity();
            this.LivingMusicOff();
            this.LivingCurtains1Close();
            this.LivingCurtainsClose();
            //this.LivingMovieOn();
            Thread.Sleep(5000);
            if (this.LivingScenceFinished != null)
            {
                this.LivingScenceFinished();
            }
            this.ScenceBusy = false;
            return o;
        }
        /// <summary>
        /// 老人回家场景
        /// 1、灯光明亮 2、纱窗关闭 3、窗帘关闭 4、电视关闭
        /// </summary>
        public void ScenceLivingOldMan()
        {
            new Thread(new ThreadCallbackFunction(this.ScenceLivingOldMan), this, Thread.eThreadStartOptions.Running);
        }

        /// <summary>
        /// 老人回家场景
        /// 1、灯光明亮 2、纱窗关闭 3、窗帘关闭 4、电视关闭
        /// </summary>
        public object ScenceLivingOldMan(object o)
        {
            
            if (this.LivingScenceStart != null)
            {
                this.LivingScenceStart();
            }
            
            this.WaitScenceBusy();
            this.ScenceBusy = true;
            this.LivingLightAllOpen();
            this.LivingMusicOff();
            this.LivingCurtains1Close();
            this.LivingCurtainsClose();
            // this.LivingMovieOff();
            Thread.Sleep(5000);
            if (this.LivingScenceFinished != null)
            {
                this.LivingScenceFinished();
            }
            this.ScenceBusy = false;
            return o;
        }
        /// <summary>
        /// 离家场景
        /// 1、安防启动 2、关闭所有设备
        /// </summary>
        public void ScenceHomeLeave()
        {
            new Thread(new ThreadCallbackFunction(this.ScenceHomeLeave), this, Thread.eThreadStartOptions.Running);
        }
        /// <summary>
        /// 离家场景
        /// 1、安防启动 2、关闭所有设备
        /// </summary>
        public object ScenceHomeLeave(object o)
        {
            
            if (this.LivingScenceStart != null)
            {
                this.LivingScenceStart();
            }
            this.WaitScenceBusy();
            this.ScenceBusy = true;
            this.ScenceBedRoomLeave();//卧室离家
            MusicAPI.Instance.MusicBuFang();
            this.ScenceStudyRoomLeave();//书房离家
            this.ScenceLivingLeave();//客厅离家
            Thread.Sleep(10000);
            if (this.LivingScenceFinished != null)
            {
                this.LivingScenceFinished();
            }
            this.ScenceBusy = false;
            return o;
        }
        /// <summary>
        /// 客厅离家场景
        /// </summary>
        public void ScenceLivingLeave()
        {
            this.LivingTempClose();
            this.LivingLightAllClose();
            this.LivingCurtains1Close();
            this.LivingCurtainsClose();
            
            this.LivingMovieOff();
            this.LivingMusicOff();

            
        }

        public string GetTianQi()
        {
            string url = string.Format("weather.yahooapis.com/forecastrss?w={0}&u=c", 2132582);
            // url = "http://www.baidu.com";
            HttpClient client = new HttpClient();
            UDPAPI.SendData("Start");
            string ret = client.Get(url);
            UDPAPI.SendData("END");
            return ret;

        }

        public void LivingMovieOn()
        {
            this._movie.LivingProjectorDown();
            Thread.Sleep(10000);
            this._movie.LivingProjectorOn();
            Thread.Sleep(1000);
            this._movie.LivingAvrPowerOn();
        }
        public void LivingMovieOff()
        {
            this._movie.LivingProjectorUp();
            Thread.Sleep(1000);
            this._movie.LivingProjectorOff();
            Thread.Sleep(1000);
            this._movie.LivingAvrPowerOff();
        }
        #endregion

        #region 书房场景
        public bool StudyRoomScenceIsBusy = false;
        /// <summary>
        /// 看书场景
        /// 1、看书灯光开启 2、背景音乐开启 3、窗帘打开
        /// </summary>
        public void ScenceStudyRoomWatchBook()
        {
            new Thread(new ThreadCallbackFunction(this.ScenceStudyRoomWatchBook), this, Thread.eThreadStartOptions.Running);

        }
        /// <summary>
        /// 看书场景
        /// 1、看书灯光开启 2、背景音乐开启 3、窗帘打开
        /// </summary>
        public object ScenceStudyRoomWatchBook(object o)
        {
            if (this.StudyRoomScenceStart != null)
            {
                this.StudyRoomScenceStart();
                this.LivingScenceStart -= this.StudyRoomScenceStart;
            }
            this.StudyRoomLightWatchBook();
            Thread.Sleep(300);
            MusicAPI.Instance.StudyRoomMusicOn();
            Thread.Sleep(300);
            this.StudyRoomWindowUp();
            Thread.Sleep(300);
            this.StudyRoomComputerOff();
            Thread.Sleep(3000);
            if (this.StudyRoomScenceFinished != null)
            {
                this.StudyRoomScenceFinished();
                this.StudyRoomScenceFinished -= this.StudyRoomScenceFinished;
            }
            return o;

        }
        /// <summary>
        /// 办公场景
        /// 1、灯光全开 2、背景音乐关闭 3、窗帘关闭 4、电脑开启
        /// </summary>
        public void ScenceStudyRoomOffice()
        {
            new Thread(new ThreadCallbackFunction(this.ScenceStudyRoomOffice), this, Thread.eThreadStartOptions.Running);

                //this.StudyRoomLightOffice();
                //Thread.Sleep(300);
                //this.StudyRoomMusicOff();
                //Thread.Sleep(300);
                //this.StudyRoomWindowDown();
                //Thread.Sleep(300);
                //this.StudyRoomComputerOn();
                //Thread.Sleep(3000);
        }
        public object ScenceStudyRoomOffice(object o)
        {
            if (this.StudyRoomScenceStart != null)
            {
                this.StudyRoomScenceStart();
                this.StudyRoomScenceStart -= this.StudyRoomScenceStart;
            }
            this.StudyRoomLightOffice();
            Thread.Sleep(300);
            MusicAPI.Instance.StudyRoomMusicOff();
            Thread.Sleep(300);
            this.StudyRoomWindowDown();
            Thread.Sleep(300);
            this.StudyRoomComputerOn();
            Thread.Sleep(3000);
            if (this.StudyRoomScenceFinished != null)
            {
                this.StudyRoomScenceFinished();
                this.StudyRoomScenceFinished -= this.StudyRoomScenceFinished;
            }
            return o;
        }
        /// <summary>
        /// 书房离开
        /// 灯光关闭、背景音乐关闭、窗帘放下、电脑关闭
        /// </summary>
        public void ScenceStudyRoomLeave()
        {

                this.StudyRoomLightClose();
                Thread.Sleep(300);
                MusicAPI.Instance.StudyRoomMusicOff();
                Thread.Sleep(300);
                this.StudyRoomWindowDown();
                Thread.Sleep(300);
                this.StudyRoomComputerOff();
                Thread.Sleep(10000);


        }

        #endregion

        #region 主卧场景

        #region 观影模式
        /// <summary>
        /// 观影模式
        /// 1、幕布降下 2、投影开启 3、窗帘关闭 4、背景音乐关闭 
        /// 5、功放开启 6、碟机开启 7、灯光关闭
        /// </summary>
        public void ScenceBedRoomWatchMovie()
        {
            try
            {
                BedRoomStatus = 1;
                this._movie.BedRoomMovie(true);
                this._curtains.BedRoomWindowClose();
                MusicAPI.Instance.BedRoomMusicOff();
                this.BedRoomLightClose();
                Thread.Sleep(40000);
            }
            catch (Exception)
            {
            }

        }

        #endregion
        #region 烂漫模式
        /// <summary>
        /// 烂漫模式
        /// 1、幕布升起 2、投影关闭 3、功放关闭 4、碟机关闭 5、灯光柔和
        /// </summary>
        public void ScenceBedRoomRomantic()
        {
            try
            {
                BedRoomStatus = 2;
                this.BedRoomLightLanMan();
                this._movie.BedRoomMovie(false);
                this._curtains.BedRoomWindowClose();
                MusicAPI.Instance.BedRoomMusicOn();
                
                Thread.Sleep(10000);
            }
            catch (Exception)
            {
            }

        }
        #endregion
        #region 睡眠模式
        /// <summary>
        /// 睡眠场景
        /// 1、灯光关闭 2、音乐关闭 3、 幕布升起 4、投影关闭 5、功放关闭 6、碟机关闭 7、窗帘关闭 8、安防启动
        /// </summary>
        public void ScenceBedRoomSleep()
        {
            BedRoomStatus = 3;
            try
            {


                this._movie.BedRoomSecreenUp();
                this._movie.BedRoomProjectorOff();
                this._curtains.BedRoomWindowClose();

                MusicAPI.Instance.BedRoomMusicOff();
                this._movie.BedRoomAvrOff();
                this.BedRoomLightClose();
                this.WashRoomLightClose();
                this.ScenceStudyRoomLeave();
                this.ScenceLivingLeave();
                this.SecurityStart();

                this._lightExec.din8sw8_05.SwitchedLoads[7].FullOff();
                Thread.Sleep(5000);
            }
            catch (Exception)
            {
                this._movie.BedRoomSecreenUp();
                this._movie.BedRoomProjectorOff();
                this._curtains.BedRoomWindowClose();
                MusicAPI.Instance.BedRoomMusicOff();
                this._movie.BedRoomAvrOff();
                this.BedRoomLightClose();
                this.WashRoomLightClose();
                this.ScenceStudyRoomLeave();
                this.ScenceLivingLeave();
                this.SecurityStart();

                this._lightExec.din8sw8_05.SwitchedLoads[7].FullOff();
                Thread.Sleep(5000);
            }

        }
        #endregion

        #region 起夜模式
        /// <summary>
        /// 起夜模式
        /// </summary>
        public void ScenceBedRoomGetUpNight()
        {
            if (BedRoomStatus == 3)
            {
                this.WashRoomLightGetUp();
                Thread.Sleep(30000);
                this.WashRoomLightClose();
            }
        }
        #endregion

        #region 晨起模式
        /// <summary>
        /// 晨起模式，模拟闹钟叫起
        /// </summary>
        public void ScenceBedRoomAlarmClock()
        {
            BedRoomStatus = 4;
            this._lightExec.din8sw8_05.SwitchedLoads[7].FullOn();
            MusicAPI.Instance.BedRoomMusicGetUp();
            Thread.Sleep(4000);
            this._curtains.BedRoomWindow20();
            Thread.Sleep(5000);
            this._lightExec.BedRoomLightGetUp();
            Thread.Sleep(5000);
            this._curtains.BedRoomWindow40();
            Thread.Sleep(10000);
            this._curtains.BedRoomWindow70();
            Thread.Sleep(10000);
            this._curtains.BedRoomWindowOpen();
            Thread.Sleep(5000);
        }

        #endregion
        #region 起漱模式

        /// <summary>
        /// 起漱模式
        /// </summary>
        public void ScenceBedRoomWash()
        {
            BedRoomStatus = 5;
            this._lightExec.BedRoomLightAllOpen();
            this._lightExec.WashRoomLightGetUp();
            this._movie.WashRoomTVOn();
            Thread.Sleep(2000);
        }
        #endregion
        #region 离开模式
        /// <summary>
        /// 离开模式 幕布升起 2、投影关闭 3、功放关闭 4、碟机关闭 5、背景音乐关闭 6、窗帘关闭 7、安防启动
        /// </summary>
        public void ScenceBedRoomLeave()
        {
            BedRoomStatus = 6;
            this._movie.BedRoomMovie(false);
            this._curtains.BedRoomWindowClose();
            MusicAPI.Instance.BedRoomMusicOff();
            this.BedRoomLightClose();
            this._lightExec.din8sw8_05.SwitchedLoads[7].FullOff();
            this.WashRoomLightClose();
            Thread.Sleep(10000);
        }
        #endregion

        public int BedRoomStatus = 0;
        public bool BedRoomScenceIsBusy = false;








      
        #endregion
        #endregion

        #region 客厅
        #region 客厅灯光
        public void SetFoyerLightWatchEvent(LightWatchEventHandler eventhandler)
        {
            this._lightExec.FoyerLightWatchEvent += eventhandler;
        }
        public void SetDropLightWatchEvent(LightWatchEventHandler eventhandler)
        {
            this._lightExec.DropLightWatchEvent += eventhandler;
        }
        public void SetRightLightWatchEvent(LightWatchEventHandler eventhandler)
        {
            this._lightExec.RightLightWatchEvent += eventhandler;
        }
        public void SetFrontLightWatchEvent(LightWatchEventHandler eventhandler)
        {
            this._lightExec.FrontLightWatchEvent += eventhandler;
        }
        public void SetBackLightWatchEvent(LightWatchEventHandler eventhandler)
        {
            this._lightExec.BackLightWatchEvent += eventhandler;
        }
        public void SetBeltLightWatchEvent(LightWatchEventHandler eventhandler)
        {
            this._lightExec.BeltLightWatchEvent += eventhandler;
        }

        internal void LivingLightAllOpen()
        {
            for (int i = 0; i < 100; i++)
            {
                this._lightExec.LivingFoyerLightLevel = this._lightExec.LivingFrontLightLevel > (ushort)(65535 - 655.35) ? ushort.MaxValue : (ushort)(this._lightExec.LivingFrontLightLevel + 655.35);
                this._lightExec.LivingFrontLightLevel = this._lightExec.LivingFrontLightLevel > (ushort)(65535 - 655.35) ? ushort.MaxValue : (ushort)(this._lightExec.LivingFrontLightLevel + 655.35);
                this._lightExec.LivingRightLightLevel = this._lightExec.LivingRightLightLevel > (ushort)(65535 - 655.35) ? ushort.MaxValue : (ushort)(this._lightExec.LivingRightLightLevel + 655.35);
                this._lightExec.LivingBackLightLevel = this._lightExec.LivingBackLightLevel > (ushort)(65535 - 655.35) ? ushort.MaxValue : (ushort)(this._lightExec.LivingBackLightLevel + 655.35);
                this._lightExec.LivingBeltLightLevel = this._lightExec.LivingBeltLightLevel > (ushort)(65535 - 655.35) ? ushort.MaxValue : (ushort)(this._lightExec.LivingBeltLightLevel + 655.35);
                this._lightExec.LivingDropLightLevel = this._lightExec.LivingDropLightLevel > (ushort)(65535 - 655.35) ? ushort.MaxValue : (ushort)(this._lightExec.LivingDropLightLevel + 655.35);
                Thread.Sleep(15);
            }
        }

        internal void LivingLightAllClose()
        {
            for (int i = 0; i < 100; i++)
            {
                this._lightExec.LivingFoyerLightLevel = this._lightExec.LivingFrontLightLevel < (ushort)(655.35) ? ushort.MinValue : (ushort)(this._lightExec.LivingFrontLightLevel - 655.35);
                this._lightExec.LivingFrontLightLevel = this._lightExec.LivingFrontLightLevel < (ushort)(655.35) ? ushort.MinValue : (ushort)(this._lightExec.LivingFrontLightLevel - 655.35);
                this._lightExec.LivingRightLightLevel = this._lightExec.LivingRightLightLevel < (ushort)(655.35) ? ushort.MinValue : (ushort)(this._lightExec.LivingRightLightLevel - 655.35);
                this._lightExec.LivingBackLightLevel = this._lightExec.LivingBackLightLevel < (ushort)(655.35) ? ushort.MinValue : (ushort)(this._lightExec.LivingBackLightLevel - 655.35);
                this._lightExec.LivingBeltLightLevel = this._lightExec.LivingBeltLightLevel < (ushort)(655.35) ? ushort.MinValue : (ushort)(this._lightExec.LivingBeltLightLevel - 655.35);
                this._lightExec.LivingDropLightLevel = this._lightExec.LivingDropLightLevel < (ushort)(655.35) ? ushort.MinValue : (ushort)(this._lightExec.LivingDropLightLevel - 655.35);
                Thread.Sleep(15);
            }
        }

        internal void LivingLightNormal()
        {
            this.LivingLightRamp(0, 65535, 65535, 65535, 65535, 0);
        }

        internal void LivingLightSaveElectricity()
        {

            this.LivingLightRamp(0, 0, 0, 0, 65535, 0);

        }
        private void LivingLightRamp(ushort foyerLevel, ushort frontLevel, ushort rightLevel, ushort backLevel, ushort beltLevel, ushort dropLevel)
        {
            for (int i = 0; i < 100; i++)
            {
                #region 进门灯
                if (this._lightExec.LivingFoyerLightLevel >= foyerLevel + 655.35)
                {
                    this._lightExec.LivingFoyerLightLevel = (ushort)(this._lightExec.LivingFoyerLightLevel - 655.35);
                }
                if (this._lightExec.LivingFoyerLightLevel <= foyerLevel - 655.35)
                {
                    this._lightExec.LivingFoyerLightLevel = (ushort)(this._lightExec.LivingFoyerLightLevel + 655.35);
                }
                #endregion

                #region 前射灯
                if (this._lightExec.LivingFrontLightLevel >= frontLevel + 655.35)
                {
                    this._lightExec.LivingFrontLightLevel = (ushort)(this._lightExec.LivingFrontLightLevel - 655.35);
                }
                if (this._lightExec.LivingFrontLightLevel <= frontLevel - 655.35)
                {
                    this._lightExec.LivingFrontLightLevel = (ushort)(this._lightExec.LivingFrontLightLevel + 655.35);
                }
                #endregion

                #region 右射灯
                if (this._lightExec.LivingRightLightLevel >= rightLevel + 655.35)
                {
                    this._lightExec.LivingRightLightLevel = (ushort)(this._lightExec.LivingRightLightLevel - 655.35);
                }
                if (this._lightExec.LivingRightLightLevel <= rightLevel - 655.35)
                {
                    this._lightExec.LivingRightLightLevel = (ushort)(this._lightExec.LivingRightLightLevel + 655.35);
                }
                #endregion

                #region 后射灯
                if (this._lightExec.LivingBackLightLevel >= backLevel + 655.35)
                {
                    this._lightExec.LivingBackLightLevel = (ushort)(this._lightExec.LivingBackLightLevel - 655.35);
                }
                if (this._lightExec.LivingBackLightLevel <= backLevel - 655.35)
                {
                    this._lightExec.LivingBackLightLevel = (ushort)(this._lightExec.LivingBackLightLevel + 655.35);
                }
                #endregion

                #region 灯带
                if (this._lightExec.LivingBeltLightLevel >= beltLevel + 655.35)
                {
                    this._lightExec.LivingBeltLightLevel = (ushort)(this._lightExec.LivingBeltLightLevel - 655.35);
                }
                if (this._lightExec.LivingBeltLightLevel <= beltLevel - 655.35)
                {
                    this._lightExec.LivingBeltLightLevel = (ushort)(this._lightExec.LivingBeltLightLevel + 655.35);
                }
                #endregion

                #region 吊灯
                if (this._lightExec.LivingDropLightLevel >= dropLevel + 655.35)
                {
                    this._lightExec.LivingDropLightLevel = (ushort)(this._lightExec.LivingDropLightLevel - 655.35);
                }
                if (this._lightExec.LivingDropLightLevel <= dropLevel - 655.35)
                {
                    this._lightExec.LivingDropLightLevel = (ushort)(this._lightExec.LivingDropLightLevel + 655.35);
                }
                #endregion

                Thread.Sleep(15);

            }
        }
        internal void LivingLightLesure()
        {
            this.LivingLightRamp(65535, 65535, 65535, 65535, 0, 65535);
        }

        internal void LivingLightCustom()
        {
            this._lightExec.LivingLightCustom();
        }

        internal void LivingLightCustomSave()
        {
            this._lightExec.LivingLightCustomSave();
        }
        internal void SetLivingFoyerLightLevel(ushort shortVlaue)
        {
            this._lightExec.LivingFoyerLightLevel = shortVlaue;
        }

        internal void SetLivingDropLightLevel(ushort shortVlaue)
        {
            this._lightExec.LivingDropLightLevel = shortVlaue;
        }

        internal void SetLivingRightLightLevel(ushort shortVlaue)
        {
            this._lightExec.LivingRightLightLevel = shortVlaue;
        }
        internal void SetLivingFrontLightLevel(ushort shortVlaue)
        {
            this._lightExec.LivingFrontLightLevel = shortVlaue;

        }
        internal void SetLivingBackLightLevel(ushort shortVlaue)
        {
            this._lightExec.LivingBackLightLevel = shortVlaue;
        }
        internal void SetLivingLightBeltLevel(ushort shortVlaue)
        {
            this._lightExec.LivingBeltLightLevel = shortVlaue;
        }
        #endregion
        #region 客厅背景音乐

        public void LivingMusicOn()
        {
            //LivingMusicIsOn = true;
            GlobalSigInfo.Instance.LivingMusic = true;
            MusicAPI.Instance.BackAudio.Zoom0On = true;//.ZOOM0_ON();
            Thread.Sleep(100);
        }
        public void LivingMusicOff()
        {
            GlobalSigInfo.Instance.LivingMusic = false;

            MusicAPI.Instance.BackAudio.Zoom0On = false;
            Thread.Sleep(100);
        }
        public void LivingMusicVolUp()
        {
            MusicAPI.Instance.BackAudio.Zoom0VolUp();
            Thread.Sleep(100);
        }
        public void LivingMusicVolDown()
        {
            MusicAPI.Instance.BackAudio.Zoom0VolDown();
            Thread.Sleep(100);
        }
        public void LivingMusicAUX()
        {
            MusicAPI.Instance.BackAudio.Zoom0Source = AudioSource.AUX;
            Thread.Sleep(100);
        }
        #endregion
        #region 客厅窗帘
        /// <summary>
        /// 打开客厅窗帘
        /// </summary>
        public void LivingCurtainsOpen()
        {
            this._curtains.LivingWindowOpen();
        }
        public void LivingCurtainsStop()
        {
            this._curtains.LivingWindowStop();
        }
        /// <summary>
        /// 关闭客厅窗帘
        /// </summary>
        public void LivingCurtainsClose()
        {
            this._curtains.LivingWindowClose();
        }

        /// <summary>
        /// 打开客厅纱窗
        /// </summary>
        public void LivingCurtains1Open()
        {
            this._curtains.LivingWindow1Open();
        }
        public void LivingCurtains1Stop()
        {
            this._curtains.LivingWindow1Stop();
        }
        /// <summary>
        /// 关闭客厅纱窗
        /// </summary>
        public void LivingCurtains1Close()
        {
            this._curtains.LivingWindow1Close();
        }
        #endregion
        #region 客厅空调
        public void LivingTempOpen()
        {
            this._clinate.LivingTempOpen();
        }
        public void LivingTempClose()
        {
            this._clinate.LivingTempClose();
        }

        public void LivingTempCoolLower()
        {
            this._clinate.LivingTempCoolLower();
        }
        public void LivingTempCoolCenter()
        {
            this._clinate.LivingTempCoolCenter();
        }
        public void LivingTempCoolHight()
        {
            this._clinate.LivingTempCoolHight();
        }
        public void LivingTempHotLower()
        {
            this._clinate.LivingTempHotLower();
        }
        public void LivingTempHotCenter()
        {
            this._clinate.LivingTempHotCenter();
        }
        public void LivingTempHotHight()
        {
            this._clinate.LivingTempHotHight();
        }
        #endregion
        #endregion

        #region 书房
        private bool StudyRoomTempIsOn = false;
        internal void StudyRoomTempToggle()
        {
            if (StudyRoomTempIsOn)
            {
                StudyRoomTempIsOn = false;
                this._clinate.StudyTempOff();
            }
            else
            {
                StudyRoomTempIsOn = true;
                this._clinate.StudyTempOn();
            }
        }

        internal void StudyRoomWindowToggle()
        {
            this._curtains.StudyRoomWindowToggle();
            Thread.Sleep(3000);
        }
        internal void StudyRoomWindowUp()
        {

            this._curtains.StudyRoomWindowUp();
        }

        internal void StudyRoomWindowDown()
        {

            this._curtains.StudyRoomWindowDown();
        }
        internal void StudyRoomComputerOn()
        {
            ComputerAPI api = new ComputerAPI();
            api.StudyRoomComputerOpen();
            Thread.Sleep(1000);
            api.StudyRoomComputerOpen();
        }
        internal void StudyRoomComputerOff()
        {
            ComputerAPI api = new ComputerAPI();
            api.StudyRoomComputerClose();
        }

        #region 书房灯光
        public bool StudyRoomLightIsOpen = false;
        public void StudyRoomLightToggle()
        {
            if (this.StudyRoomLightIsOpen)
            {
                this.StudyRoomLightClose();
            }
            else
            {
                this.StudyRoomLightOpen();
            }

        }
        public void StudyRoomLightWatchBook()
        {
            this._lightExec.StudyRoomLightWatchBook();
        }
        public void StudyRoomLightOffice()
        {
            this._lightExec.StudyRoomLightOffice();
        }
        public void StudyRoomLightClose()
        {
            this.StudyRoomLightIsOpen = false;
            _lightExec.StudyRoomLightAllClose();
        }
        public void StudyRoomLightOpen()
        {
            this.StudyRoomLightIsOpen = true;
            _lightExec.StudyRoomLightAllOpen();
        }
        #endregion
        
        #endregion

        #region 卧室

        #region 卧室灯光
        private bool BedRoomLightIsOpen = false;
        public void BedRoomLightToggle()
        {
            if (this.BedRoomLightIsOpen)
            {
                this.BedRoomLightClose();
            }
            else
            {
                this.BedRoomLightOpen();
            }
        }
        public void BedRoomLightOpen()
        {
            this.BedRoomLightIsOpen = true;
            _lightExec.BedRoomLightAllOpen(); 
        }
        public void BedRoomLightClose()
        {
            this.BedRoomLightIsOpen = false;
            _lightExec.BedRoomLightAllClose();
        }
        public void BedRoomLightLanMan()
        {
            this.BedRoomLightIsOpen = false;
            _lightExec.BedRoomLightLanMan();
        }
        #endregion
        #region 卧室窗帘
        /// <summary>
        /// 打开卧室窗帘
        /// </summary>
        public void BedRoomCurtainsOpen()
        {
            this._curtains.BedRoomWindowOpen();
        }
        public void BedRoomCurtainsStop()
        {
            this._curtains.BedRoomWindowStop();
        }
        /// <summary>
        /// 关闭卧室窗帘
        /// </summary>
        public void BedRoomCurtainsClose()
        {
            this._curtains.BedRoomWindowClose();
        }
        public void BedRoomCurtains20()
        {
            this._curtains.BedRoomWindow20();
        }
        public void BedRoomCurtains30()
        {
            this._curtains.BedRoomWindow30();
        }
        public void BedRoomCurtains40()
        {
            this._curtains.BedRoomWindow40();
        }
        public void BedRoomCurtains50()
        {
            this._curtains.BedRoomWindow50();
        }
        public void BedRoomCurtains60()
        {
            this._curtains.BedRoomWindow60();
        }
        public void BedRoomCurtains70()
        {
            this._curtains.BedRoomWindow70();
        }
        public void BedRoomCurtains80()
        {
            this._curtains.BedRoomWindow100();
        }
        #endregion
        #endregion

        #region 卫生间
        
        #region 卫生间灯光
        private void WashRoomLightGetUp()
        {
            this._lightExec.WashRoomLightGetUp();
           // throw new NotImplementedException();
        }
        private void WashRoomLightClose()
        {
            this._lightExec.WashRoomLightAllClose();
        }

        #endregion
        #endregion

        #region 全区
        #region 指纹锁
        /// <summary>
        /// 指纹锁返回数据
        /// </summary>
        /// <param name="button"></param>
        public void YelaPress(int button)
        {
            ILiveDebug.Instance.WriteLine("YelaPressButton:" + button);
            string btn = button.ToString();
            
            switch (btn)
            {
                case "1":
                    new Thread(new ThreadCallbackFunction(this.ScenceHomeNormal), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);
                    break;
                case "2":
                    this.ScenceHomeHolding("18969781225");
                    break;
                case "3":
                    new Thread(new ThreadCallbackFunction(this.ScenceHomeNormal), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);
                    break;
                case "4":
                    this.ScenceHomeHolding("1356619992");
                    break;
                case "5":
                    new Thread(new ThreadCallbackFunction(this.ScenceHomeNormal), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);
                    break;
                case "6":
                    
                    this.ScenceHomeHolding("13867911360");
                    break;
                case "7":
                    new Thread(new ThreadCallbackFunction(this.ScenceHomeNormal), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);
                    break;
                case "8":
                    this.ScenceHomeHolding("18957767067");
                    break;
                default:
                    break;
            }
        }
        public void DoorOpen()
        {
            this._security.YelaOpenDoor();
        }
        public void DoorClose()
        {
            this._security.YelaCloseDoor();
        }
        #endregion
        #region 短信
        //public void SMSSend(string phone, string msg)
        //{
        //    try
        //    {
        //       // this._sms.Open();

        //        this._sms.SendMsg(phone, msg);

        //    }
        //    catch (Exception)
        //    {
        //    }

        //}
        #endregion
        
        #region 定时开关户外灯
        //CTimer tmrOutdoorLight = null;
        //public void Start()
        //{
        //    tmrOutdoorLight = new CTimer(
        //         OutdoorLightTimerCallback,    //定时回调函数 
        //         null,
        //         40000,
        //         40000);
        //    tmrOutdoorLight.Reset();
        //}

        //void OutdoorLightTimerCallback(object o)
        //{
        //    string today = DateTime.Now.ToString("HH:mm");
        //    //ILiveDebug.WriteLine(today);
        //    // (要定时执行的代码写在这里) 
        //    if (today == "22:30")
        //    {
        //        this._lightExec.OutdoorClose();
        //    }
        //    if (today == "18:00")
        //    {
        //        this._lightExec.OutdoorOpen();
        //    }
        //    tmrOutdoorLight.Reset(40000, 40000);   //<--每10分钟启动一次！ ,可以改变下次启动时间
        //}

        #endregion
        #region 音乐
        public void MusicJueShi()
        {
            MusicAPI.Instance.MusicPlayer.PlayJueShi();
        }
        public void MusicGangQing()
        {
            MusicAPI.Instance.MusicPlayer.PlayGangQing();
        }
        public void MusicXiangCun()
        {
            MusicAPI.Instance.MusicPlayer.PlayXiangCun();
        }
        private int MusicZoom = 0;
        public void MusicSetZoom1()
        {
            this.MusicZoom = 1;
        }
        public void MusicSetZoom2()
        {
            this.MusicZoom = 2;
        }
        public void MusicSetZoom3()
        {
            this.MusicZoom = 3;
        }
        public void MusicSetZoom4()
        {
            this.MusicZoom = 4;
        }
        public void MusicSetZoomOn()
        {
            switch (this.MusicZoom)
            {
                case 1:
                    GlobalSigInfo.Instance.LivingMusic = true;
                    MusicAPI.Instance.BackAudio.Zoom0On = true;
                    break;
                case 2:
                    GlobalSigInfo.Instance.StudyRoomMusic = true;
                    MusicAPI.Instance.BackAudio.Zoom1On = true;
                    break;
                case 3:
                    GlobalSigInfo.Instance.BedRoomMusic = true;
                    MusicAPI.Instance.BackAudio.Zoom2On = true;
                    break;
                case 4:
                    GlobalSigInfo.Instance.WashRoomMusic = true;
                    MusicAPI.Instance.BackAudio.Zoom3On = true;
                    break;
                default:
                    break;
            }
        }
        public void MusicSetZoomOff()
        {
            switch (this.MusicZoom)
            {
                case 1:
                    GlobalSigInfo.Instance.LivingMusic = false;
                    MusicAPI.Instance.BackAudio.Zoom0On = false;
                    break;
                case 2:
                    GlobalSigInfo.Instance.BedRoomMusic = false;

                    MusicAPI.Instance.BackAudio.Zoom1On = false;
                    break;
                case 3:
                    GlobalSigInfo.Instance.StudyRoomMusic = false;

                    MusicAPI.Instance.BackAudio.Zoom2On = false;
                    break;
                case 4:
                    GlobalSigInfo.Instance.WashRoomMusic = false;

                    MusicAPI.Instance.BackAudio.Zoom3On = false;
                    break;
                default:
                    break;
            }
        }
        public void MusicSetZoomVolUp()
        {
            switch (this.MusicZoom)
            {
                case 1:
                    MusicAPI.Instance.BackAudio.Zoom0VolUp();
                    break;
                case 2:
                    MusicAPI.Instance.BackAudio.Zoom1VolUp();
                    break;
                case 3:
                    MusicAPI.Instance.BackAudio.Zoom2VolUp();
                    break;
                case 4:
                    MusicAPI.Instance.BackAudio.Zoom3VolUp();
                    break;
                default:
                    break;
            }
        }
        public void MusicSetZoomVolDown()
        {
            switch (this.MusicZoom)
            {
                case 1:
                    MusicAPI.Instance.BackAudio.Zoom0VolDown();
                    break;
                case 2:
                    MusicAPI.Instance.BackAudio.Zoom1VolDown();
                    break;
                case 3:
                    MusicAPI.Instance.BackAudio.Zoom2VolDown();
                    break;
                case 4:
                    MusicAPI.Instance.BackAudio.Zoom3VolDown();
                    break;
                default:
                    break;
            }
        }
        public void MusicSetZoomAux()
        {
            switch (this.MusicZoom)
            {
                case 1:
                    MusicAPI.Instance.BackAudio.Zoom0Source = AudioSource.AUX;
                    break;
                case 2:
                    MusicAPI.Instance.BackAudio.Zoom1Status = AudioSource.AUX;
                    break;
                case 3:
                    MusicAPI.Instance.BackAudio.Zoom2Status = AudioSource.AUX;
                    break;
                case 4:
                    MusicAPI.Instance.BackAudio.Zoom3Status = AudioSource.AUX;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 安防
        /// <summary>
        /// 布防
        /// </summary>
        public void SecurityStart()
        {
            if (!GlobalSigInfo.Instance.SecurityBusy)
            {
                GlobalSigInfo.Instance.SecurityBusy = true;
                try
                {
                    this._security.BuFang();
                    MusicAPI.Instance.MusicBuFang();
                }
                catch (Exception)
                {
                }

                Thread.Sleep(5000);
                GlobalSigInfo.Instance.SecurityBusy = false;
            }

        }
        /// <summary>
        /// 撤防
        /// </summary>
        public void SecurityEnd()
        {
            if (!GlobalSigInfo.Instance.SecurityBusy)
            {
                GlobalSigInfo.Instance.SecurityBusy = true;
                try
                {
                    this._security.CheFang();
                    MusicAPI.Instance.MusicCeFang();
                }
                catch (Exception)
                {
                }
 
                Thread.Sleep(3000);
                GlobalSigInfo.Instance.SecurityBusy = false;
            }

        }
        #endregion
        #region 监控
        public uint CameraShow = 1;
        public void CameraSet(uint i)
        {
            this.CameraShow = i;
        }
        #endregion
        #endregion

        #region 办公区
        #region 办公区灯光

        #region 一楼办公区
        private bool OfficeLightIsOpen = false;
        public void OfficeLightToggle()
        {
            if (this.OfficeLightIsOpen)
            {
                this.OfficeLightIsOpen = false;
                _lightExec.OfficeLightOpen();
                _lightExec.PrintLightOpen();
            }
            else
            {
                this.OfficeLightIsOpen = true;
                _lightExec.OfficeLightClose();
                _lightExec.PrintLightClose();
            }

        }

        #endregion

        #region 一楼走廊

        private bool AisleLightIsOpen = false;
        public void AisleLightToggle()
        {
            if (this.AisleLightIsOpen)
            {
                this.AisleLightIsOpen = false;
                _lightExec.AisleLightOpen();
            }
            else
            {
                this.AisleLightIsOpen = true;
                _lightExec.AisleLightClose();
            }

        }
        #endregion

        #region 会议室
        private bool MettingLightIsOpen = false;
        public void MettingLightToggle()
        {
            if (this.MettingLightIsOpen)
            {
                this.MettingLightIsOpen = false;
                _lightExec.MettingLightOpen();
            }
            else
            {
                this.MettingLightIsOpen = true;
                _lightExec.MettingLightClose();
            }
        }
        #endregion

        #region 二楼走廊

        private bool Aisle2LightIsOpen = false;
        public void Aisle2LightToggle()
        {
            if (this.Aisle2LightIsOpen)
            {
                this.Aisle2LightIsOpen = false;
                _lightExec.Aisle2LightOpen();
            }
            else
            {
                this.Aisle2LightIsOpen = true;
                _lightExec.Aisle2LightClose();
            }
        }

        #endregion

        #region 门口

        private bool FrontDoorLightIsOpen = false;
        public void FrontDoorLightToggle()
        {
            if (this.FrontDoorLightIsOpen)
            {
                this.FrontDoorLightIsOpen = false;
                _lightExec.FrontDoorLightOpen();
            }
            else
            {
                this.FrontDoorLightIsOpen = true;
                _lightExec.FrontDoorLightClose();
            }
        }

        #endregion

        #region 副总经理室

        private bool DeputyGeneralLightIsOpen = false;
        public void DeputyGeneralLightToggle()
        {
            if (this.DeputyGeneralLightIsOpen)
            {
                this.DeputyGeneralLightIsOpen = false;
                _lightExec.DeputyGeneralLightOpen();
            }
            else
            {
                this.DeputyGeneralLightIsOpen = true;
                _lightExec.DeputyGeneralOfficeLightClose();
            }
        }

        #endregion

        #region 二楼办公区

        private bool Office2LightIsOpen = false;
        public void Office2LightToggle()
        {
            if (this.Office2LightIsOpen)
            {
                this.Office2LightIsOpen = false;
                _lightExec.Office2LightOpen();
            }
            else
            {
                this.Office2LightIsOpen = true;
                _lightExec.Office2LightClose();
            }
        }

        #endregion

        #region  实验室
        private bool LaboratoryLightIsOpen = false;
        public void LaboratoryLightToggle()
        {
            if (this.LaboratoryLightIsOpen)
            {
                this.LaboratoryLightIsOpen = false;
                _lightExec.LaboratoryLightOpen();
            }
            else
            {
                this.LaboratoryLightIsOpen = true;
                _lightExec.LaboratoryLightClose();
            }
        }

        #endregion

        #region 储藏室

        private bool GeneralStoreLightIsOpen = false;
        public void GeneralStoreLightToggle()
        {
            if (this.GeneralStoreLightIsOpen)
            {
                this.LaboratoryLightIsOpen = false;
                _lightExec.GeneralStoreLightOpen();
            }
            else
            {
                this.GeneralStoreLightIsOpen = true;
                _lightExec.GeneralStoreLightClose();
            }
        }

        #endregion

        #region 总办
        private bool GeneralOfficeLightIsOpen = false;
        public void GeneralOfficeLightToggle()
        {
            if (this.GeneralOfficeLightIsOpen)
            {
                this.GeneralOfficeLightIsOpen = false;
                _lightExec.GeneralOfficeLightOpen();
            }
            else
            {
                this.GeneralOfficeLightIsOpen = true;
                _lightExec.GeneralOfficeLightClose();
            }

        }
        #endregion


        #endregion
        #endregion

        internal void Test1()
        {
            this.StudyRoomComputerOn();
           // throw new NotImplementedException();
        }

        internal void Test2()
        {
            this.StudyRoomComputerOff();

            //throw new NotImplementedException();
        }
    }
}


