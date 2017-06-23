using System;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;                       				// For Basic SIMPL#Pro classes

namespace ILiveSmart.Music
{
    /// <summary>
    /// 背景音乐控制 多线程执行
    /// </summary>
    public class MusicAPI
    {
        public static readonly MusicAPI Instance = new MusicAPI();
        public BackAudio BackAudio = null;
        public MusicPlayer MusicPlayer = null;
        #region 状态
        /// <summary>
        /// 客厅背景音乐开启状态
        /// </summary>
        private bool LivingMusicIsOn = false;
        /// <summary>
        /// 书房背景音乐状态
        /// </summary>
        private bool StudyRoomMusicIsOn = false;
        /// <summary>
        /// 卧室背景音乐状态
        /// </summary>
        private bool BedRoomMusicIsOn = false;
        /// <summary>
        /// 卫生间背景音乐状态
        /// </summary>
        private bool WashRoomMusicIsOn = false;
        private bool MusicIsBusy = false;
        #endregion
        
        private MusicAPI()
        {
            this.BackAudio = new BackAudio("192.168.1.21", 8001);
            this.MusicPlayer = new MusicPlayer("192.168.1.21", 8002);
        }
        #region 客厅背景音乐
        public void LivingMusicOn()
        {
            LivingMusicIsOn = true;
            this.BackAudio.Zoom0On = true;
            Thread.Sleep(100);
        }
        public void LivingMusicOff()
        {
            LivingMusicIsOn = false;
            this.BackAudio.Zoom0On = false;
            Thread.Sleep(100);
        }
        public void LivingMusicVolUp()
        {
            this.BackAudio.Zoom0VolUp();
            Thread.Sleep(100);
        }
        public void LivingMusicVolDown()
        {
            this.BackAudio.Zoom0VolDown();
            Thread.Sleep(100);
        }
        public void LivingMusicAUX()
        {
            this.BackAudio.Zoom0Source = AudioSource.AUX;
            Thread.Sleep(100);
        }
        #endregion
        #region 书房背景音乐
        public void StudyRoomMusicToggle()
        {
            if (StudyRoomMusicIsOn)
            {
                this.StudyRoomMusicOff();
            }
            else
            {
                this.StudyRoomMusicOn();
            }
        }
        public void StudyRoomMusicOn()
        {
            this.StudyRoomMusicIsOn = true;
            this.BackAudio.Zoom1On = true;
            Thread.Sleep(100);
            this.BackAudio.Zoom1Status = AudioSource.AUX;
            Thread.Sleep(100);
            this.MusicPlayer.PlayGangQing();
            Thread.Sleep(100);
        }
        public void StudyRoomMusicOff()
        {
            this.StudyRoomMusicIsOn = false;
            this.BackAudio.Zoom1On = false;
            Thread.Sleep(100);
        }
        public void StudyRoomMusicVolUp()
        {
            this.BackAudio.Zoom1VolUp();
            Thread.Sleep(100);
        }
        public void StudyRoomMusicVolDown()
        {
            this.BackAudio.Zoom1VolDown();
            Thread.Sleep(100);
        }
        public void StudyRoomMusicAUX()
        {
            this.BackAudio.Zoom1Status = AudioSource.AUX;
            Thread.Sleep(100);
        }
        #endregion
        #region 卧室背景音乐
        public void BedRoomMusicToggle()
        {
            if (BedRoomMusicIsOn)
            {
                this.BedRoomMusicOff();
            }
            else
            {
                this.BedRoomMusicOn();
            }
        }
        public void BedRoomMusicOn()
        {
            this.BedRoomMusicIsOn = true;
            this.MusicPlayer.PlayGangQing();
            Thread.Sleep(100);
            this.BackAudio.Zoom2On = true;
            Thread.Sleep(100);
            this.BackAudio.Zoom2Status = AudioSource.AUX;
            Thread.Sleep(100);
        }
        public void BedRoomMusicOff()
        {
            this.BedRoomMusicIsOn = false;
            this.BackAudio.Zoom2On = false;
            Thread.Sleep(100);
        }
        public void BedRoomMusicVolUp()
        {
            this.BackAudio.Zoom2VolUp();
            Thread.Sleep(100);
        }
        public void BedRoomMusicVolDown()
        {
            this.BackAudio.Zoom2VolDown();
            Thread.Sleep(100);
        }
        public void BedRoomMusicAUX()
        {
            this.BackAudio.Zoom2Status = AudioSource.AUX;
            Thread.Sleep(100);
        }

        /// <summary>
        /// 闹钟模式
        /// </summary>
        public void BedRoomMusicGetUp()
        {
            try
            {
                Thread th = new Thread(new ThreadCallbackFunction(this.BedRoomMusicGetUpExe), this, Thread.eThreadStartOptions.Running);
                // th.Start();
            }
            catch (Exception)
            {
            }


        }
        public object BedRoomMusicGetUpExe(object o)
        {
            this.BedRoomMusicIsOn = true;
            this.MusicPlayer.PlayGangQing();
            this.BackAudio.Zoom2SetVol(0x01);
            Thread.Sleep(100);
            this.BackAudio.Zoom2On = true;
            Thread.Sleep(100);
            this.BackAudio.Zoom2Status = AudioSource.AUX;
            Thread.Sleep(100);
            for (int i = 1; i < 15; i++)
            {
                this.BackAudio.Zoom2SetVol(0x01);
                // this._music.SetVol(i);
                //  ILiveDebug.WriteLine("VOL" + i.ToString());
                Thread.Sleep(3000);
            }
            return o;
        }
        #endregion
        #region 卫生间背景音乐
        public void WashRoomMusicOn()
        {
            this.BackAudio.Zoom3On = true;
            Thread.Sleep(100);
        }
        public void WashRoomMusicOff()
        {
            this.BedRoomMusicIsOn = false;
            this.BackAudio.Zoom3On = false;
            Thread.Sleep(100);
        }
        public void WashRoomMusicVolUp()
        {
            this.BackAudio.Zoom3VolUp();
            Thread.Sleep(100);
        }
        public void WashRoomMusicVolDown()
        {
            this.BackAudio.Zoom3VolDown();
            Thread.Sleep(100);
        }
        public void WashRoomMusicAUX()
        {
            this.BackAudio.Zoom3Status = AudioSource.AUX;
            Thread.Sleep(100);
        }
        #endregion
        #region 提示音
        public void MusicBuFang()
        {
            Thread th = new Thread(new ThreadCallbackFunction(this.MusicBuFangExe), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);
        }
        public void MusicCeFang()
        {
            Thread th = new Thread(new ThreadCallbackFunction(this.MusicCeFangExe), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);

            //Thread th = new Thread(new ThreadCallbackFunction(this.MusicCeFangExe), this);
            //th.Start();
        }
        public void MusicBaoJing()
        {
            Thread th = new Thread(new ThreadCallbackFunction(this.MusicBaoJingExe), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);

            //Thread th = new Thread(new ThreadCallbackFunction(this.MusicBaoJingExe), this);
            //th.Start();
        }
        public void MusicRuQing()
        {
            Thread th = new Thread(new ThreadCallbackFunction(this.MusicRuQingExe), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);

        }
        public void MusicTongZhiBaoMu()
        {
            Thread th = new Thread(new ThreadCallbackFunction(this.MusicTongZhiBaoMuExe), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);

        }
        public void MusicWelcome()
        {
            Thread th = new Thread(new ThreadCallbackFunction(this.MusicWelcomeExe), this, Crestron.SimplSharpPro.CrestronThread.Thread.eThreadStartOptions.Running);
            //th.Start();
        }
        private object MusicBuFangExe(object o)
        {
            int i = 0;
            while (this.MusicIsBusy)
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

            this.MusicIsBusy = true;
            this.BackAudio.ZoomOn = true;
            this.MusicPlayer.PlayAnFangStart();
            Thread.Sleep(1700);
            if (!this.LivingMusicIsOn)
            {
                this.LivingMusicOff();
            }
            if (!this.StudyRoomMusicIsOn)
            {
                this.StudyRoomMusicOff();
            }
            if (!this.BedRoomMusicIsOn)
            {
                this.BedRoomMusicOff();
            }
            if (!this.WashRoomMusicIsOn)
            {
                this.WashRoomMusicOff();
            }
            this.MusicIsBusy = false;
            return o;
        }
        public object MusicCeFangExe(object o)
        {
            int i = 0;
            while (this.MusicIsBusy)
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

            this.MusicIsBusy = true;
            this.BackAudio.ZoomOn = true;
            this.MusicPlayer.PlayAnFangEnd();
            Thread.Sleep(2500);
            if (!this.LivingMusicIsOn)
            {
                this.LivingMusicOff();
            }
            if (!this.StudyRoomMusicIsOn)
            {
                this.StudyRoomMusicOff();
            }
            if (!this.BedRoomMusicIsOn)
            {
                this.BedRoomMusicOff();
            }
            if (!this.WashRoomMusicIsOn)
            {
                this.WashRoomMusicOff();
            }
            this.MusicIsBusy = false;
            return o;
        }
        public object MusicBaoJingExe(object o)
        {
            while (this.MusicIsBusy)
            {
                Thread.Sleep(1000);
            }
            this.MusicIsBusy = true;
            this.BackAudio.ZoomOn = true;
            this.MusicPlayer.PlayBaoJing();
            Thread.Sleep(3000);
            if (!this.LivingMusicIsOn)
            {
                this.LivingMusicOff();
            }
            if (!this.StudyRoomMusicIsOn)
            {
                this.StudyRoomMusicOff();
            }
            if (!this.BedRoomMusicIsOn)
            {
                this.BedRoomMusicOff();
            }
            if (!this.WashRoomMusicIsOn)
            {
                this.WashRoomMusicOff();
            }
            this.MusicIsBusy = false;
            return o;
        }

        public object MusicRuQingExe(object o)
        {
            while (this.MusicIsBusy)
            {
                Thread.Sleep(1000);
            }
            this.MusicIsBusy = true;
            this.BackAudio.ZoomOn = true;
            this.MusicPlayer.PlayRuQing();
            Thread.Sleep(4000);
            if (!this.LivingMusicIsOn)
            {
                this.LivingMusicOff();
            }
            if (!this.StudyRoomMusicIsOn)
            {
                this.StudyRoomMusicOff();
            }
            if (!this.BedRoomMusicIsOn)
            {
                this.BedRoomMusicOff();
            }
            if (!this.WashRoomMusicIsOn)
            {
                this.WashRoomMusicOff();
            }
            this.MusicIsBusy = false;
            return o;
        }
        public object MusicTongZhiBaoMuExe(object o)
        {
            while (this.MusicIsBusy)
            {
                Thread.Sleep(1000);
            }
            this.MusicIsBusy = true;
            this.BackAudio.ZoomOn = true;
            this.MusicPlayer.PlayTongZhiBaoMu();
            Thread.Sleep(4000);
            if (!this.LivingMusicIsOn)
            {
                this.LivingMusicOff();
            }
            if (!this.StudyRoomMusicIsOn)
            {
                this.StudyRoomMusicOff();
            }
            if (!this.BedRoomMusicIsOn)
            {
                this.BedRoomMusicOff();
            }
            if (!this.WashRoomMusicIsOn)
            {
                this.WashRoomMusicOff();
            }
            this.MusicIsBusy = false;
            return o;
        }
        public object MusicWelcomeExe(object o)
        {
            while (this.MusicIsBusy)
            {
                Thread.Sleep(1000);
            }
            this.MusicIsBusy = true;
            this.BackAudio.ZoomOn = true;
            this.MusicPlayer.PlayWelcome();
            Thread.Sleep(3000);
            if (!this.LivingMusicIsOn)
            {
                this.LivingMusicOff();
            }
            if (!this.StudyRoomMusicIsOn)
            {
                this.StudyRoomMusicOff();
            }
            if (!this.BedRoomMusicIsOn)
            {
                this.BedRoomMusicOff();
            }
            if (!this.WashRoomMusicIsOn)
            {
                this.WashRoomMusicOff();
            }
            this.MusicIsBusy = false;
            return o;
        }

        #endregion

    }
}

