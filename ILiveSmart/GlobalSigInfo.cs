using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmart
{
    /// <summary>
    /// 系统状态信息
    /// </summary>
    public sealed class GlobalSigInfo
    {
       public static readonly GlobalSigInfo Instance = new GlobalSigInfo();


        private GlobalSigInfo()
        {
            
        }
        /// <summary>
        /// 判断是否正在进行安防操作
        /// </summary>
        public bool SecurityBusy
        {
            get;
            set;
        }
        public bool BedRoomMovieBusy { get; set; }
        public bool BedRoomMovieStatus { get; set; }
        /// <summary>
        /// 客厅背景音乐状态
        /// </summary>
        public bool LivingMusic { get; set; }
        /// <summary>
        /// 卧室背景音乐状态
        /// </summary>
        public bool BedRoomMusic { get; set; }
        /// <summary>
        /// 书房背景音乐状态
        /// </summary>
        public bool StudyRoomMusic { get; set; }
        /// <summary>
        /// 卫生间背景音乐状态
        /// </summary>
        public bool WashRoomMusic { get; set; }
    }
}