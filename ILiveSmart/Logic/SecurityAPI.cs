using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmart
{
    /// <summary>
    /// 安防控制
    /// </summary>
    public class SecurityAPI
    {
        private CP3Smart _smartExec = null;
        public SecurityAPI(CP3Smart smartExec)
        {
            this._smartExec = smartExec;
        }

        public void SetYelaPressEvent(ILiveSmart.DYelaLock.YelaPressHandler yelaPress)
        {
           // this._smartExec.YelaPressEvent += yelaPress;
            this._smartExec.YelaLock.YelaPressEvent += yelaPress;
        }
        /// <summary>
        /// 开门
        /// </summary>
        public void YelaOpenDoor()
        {
            byte[] b = { 0x05, 0x91, 0x02, 0x11, 0x82, 0x0F };
            this._smartExec.YelaLock.SendYela(b);
        }
        /// <summary>
        /// 关门
        /// </summary>
        public void YelaCloseDoor()
        {
            byte[] b = { 0x05, 0x91, 0x02, 0x12, 0x81, 0x0F };

            this._smartExec.YelaLock.SendYela(b);
        }

        public void BuFang()
        { 
        }
        public void CheFang()
        { }
    }
}