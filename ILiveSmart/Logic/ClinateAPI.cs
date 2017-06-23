using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace ILiveSmart
{
    /// <summary>
    /// 天气、空调控制
    /// </summary>
    public class ClinateAPI
    {
        IROutputPort irStudyRoom = null;
        IROutputPort irLiving = null;
        public ClinateAPI(CP3Smart smart)
        {
            this.irStudyRoom = smart.myIROutputPort5;//书房空调
            this.irLiving = smart.myIROutputPort6;//客厅空调

            string file = Crestron.SimplSharp.CrestronIO.Directory.GetApplicationDirectory() + "\\IR\\songxia.ir";
           // ILiveDebug.WriteLine(file);
            try
            {
                uint i = irStudyRoom.LoadIRDriver(file);
                uint j = irLiving.LoadIRDriver(file);
              // ILiveDebug.WriteLine("dirver"+i);
            }
            catch (Exception ex)
            {
                
                 ILiveDebug.Instance.WriteLine(ex.Message);
            }
           
        }
        #region 客厅空调
        public void LivingTempOpen()
        {
            this.irLiving.Press("ON");
        }
        public void LivingTempClose()
        {
            this.irLiving.Press("OFF");
        }

        public void LivingTempCoolLower()
        {
            this.irLiving.Press("CLow");
        }
        public void LivingTempCoolCenter()
        {
            this.irLiving.Press("CCenter");
        }
        public void LivingTempCoolHight()
        {
            this.irLiving.Press("CHight");
        }
        public void LivingTempHotLower()
        {
            this.irLiving.Press("HLow");
        }
        public void LivingTempHotCenter()
        {
            this.irLiving.Press("HCenter");
        }
        public void LivingTempHotHight()
        {
            this.irLiving.Press("HHight");
        }
        #endregion

        #region 书房空调
        internal void StudyTempOff()
        {
            this.irStudyRoom.Press("OFF");
        }

        internal void StudyTempOn()
        {
            this.irStudyRoom.Press("ON");
        }
        #endregion


    }
}