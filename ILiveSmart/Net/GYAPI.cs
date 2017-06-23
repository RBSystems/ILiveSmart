using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmart.Net
{
    public class GYAPI
    {
        #region 客厅投影
        /// <summary>
        /// 客厅投影机开机
        /// </summary>
        public void LivingPorjectorOpen()
        {
            UDPAPI.SendData("192.168.1.22", 8001, "(PWR1)");
        }
        /// <summary>
        /// 客厅投影机关机
        /// </summary>
        public void LivingPorjectorClose()
        {
            UDPAPI.SendData("192.168.1.22", 8001, "(PWR0)");
        }
        #endregion
        #region 客厅功放
        public void LivingAvrOpen()
        {
            UDPAPI.SendData("192.168.1.22", 8002, "@MAIN:PWR=On"+"\x0D\x0A");
        }
        public void LivingAvrClose()
        {
            UDPAPI.SendData("192.168.1.22", 8002, "@MAIN:PWR=Standby" + "\x0D\x0A");
            //@MAIN:VOL=Down
            //@MAIN:VOL=Up
            //@MAIN:MUTE=On
            //@MAIN:MUTE=Off
            //@MAIN:INP=AV1
        }
        #endregion
        #region 卧室投影机

        #endregion
        #region 客厅窗帘

        #endregion
        #region 客厅纱窗

        #endregion
        #region 卧室窗帘


        #endregion
        #region 背景音乐主机

        #endregion
        #region 串口MP3播放器

        #endregion
        #region 客厅投影吊架

        #endregion
    }
}

