using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpProInternal;
using Crestron.SimplSharpPro.DeviceSupport;

namespace ILiveSmart.UI
{
    public class TswVoipReservedSigs : DeviceExtender,ISmartObject
    {
        internal TswVoipReservedSigs(CrestronDevice A_0)
            : base(A_0)
        {
        }
        protected override void InitializeJoins()
        {
           // base.InitializeJoins();
            base.MapDigitalInputJoins(new uint[]
	{
		27204u,
		27205u,
		27206u,
		27207u,
		27208u,
		27209u,
		27210u,
		27211u,
		27212u,
		27213u,
		27214u,
		27215u,
		27216u,
		27217u,
		27218u,
		27219u,
		27220u,
		27221u,
		27236u,
		27257u
	});
            base.MapDigitalOutputJoins(new uint[]
	{
		27203u,
		27222u,
		27223u,
		27224u,
		27225u,
		27226u,
		27233u,
		27236u,
		27238u,
		27256u
	});
            base.MapSerialInputJoins(new uint[]
	{
		18601u,
		18602u,
		32991u,
		32992u,
		32993u,
		32994u
	});
            base.MapSerialOutputJoins(new uint[]
	{
		18603u,
		18606u,
		18607u,
		18609u
	});
        }
        /// <summary>
        /// Method to dial digit "1".
        /// </summary>
        public void Dial1()
        {
            base.BooleanInput[27204u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "2".
        /// </summary>
        public void Dial2()
        {
            base.BooleanInput[27205u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "3".
        /// </summary>
        public void Dial3()
        {
            base.BooleanInput[27206u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "4".
        /// </summary>
        public void Dial4()
        {
            base.BooleanInput[27207u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "5".
        /// </summary>
        public void Dial5()
        {
            base.BooleanInput[27208u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "6".
        /// </summary>
        public void Dial6()
        {
            base.BooleanInput[27209u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "7".
        /// </summary>
        public void Dial7()
        {
            base.BooleanInput[27210u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "8".
        /// </summary>
        public void Dial8()
        {
            base.BooleanInput[27211u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "9".
        /// </summary>
        public void Dial9()
        {
            base.BooleanInput[27212u].Pulse();
        }
        /// <summary>
        /// Method to dial digit "0".
        /// </summary>
        public void Dial0()
        {
            base.BooleanInput[27213u].Pulse();
        }
        /// <summary>
        /// Method to dial an asterisk.
        /// </summary>
        public void DialAsterisk()
        {
            base.BooleanInput[27214u].Pulse();
        }
        /// <summary>
        /// Method to dial a pound sign "#".
        /// </summary>
        public void DialPound()
        {
            base.BooleanInput[27215u].Pulse();
        }
        /// <summary>
        /// Method to issue a backspace on the current dialed number.
        /// </summary>
        public void Backspace()
        {
            base.BooleanInput[27216u].Pulse();
        }
        /// <summary>
        /// Method to clear the entire dial string.
        /// </summary>
        public void Clear()
        {
            base.BooleanInput[27217u].Pulse();
        }
        /// <summary>
        /// Method to dial the current number string in the buffer.
        /// </summary>
        public void DialCurrentNumber()
        {
            base.BooleanInput[27218u].Pulse();
        }
        /// <summary>
        /// Method to answer an incoming call.
        /// </summary>
        public void Answer()
        {
            base.BooleanInput[27219u].Pulse();
        }
        /// <summary>
        /// Method to terminate the current call or reject any incoming call.
        /// </summary>
        public void Reject()
        {
            base.BooleanInput[27220u].Pulse();
        }
        /// <summary>
        /// Method to terminate the current call or reject any incoming call.
        /// </summary>
        public void Hangup()
        {
            base.BooleanInput[27221u].Pulse();
        }
        /// <summary>
        /// Method toggle the Do Not Disturb setting.
        /// </summary>
        public void DoNotDisturb()
        {
            base.BooleanInput[27236u].Pulse();
        }
        /// <summary>
        /// Method to page all groups.
        /// </summary>
        public void PageAll()
        {
            base.BooleanInput[27257u].Pulse();
        }

        		/// <summary>
		/// Triggers PTT (push-to-talk) transmission when connected to a PTT device. 'true' - Trigger PTT transmission. 'false' - Stop PTT transmission.
		/// </summary>
		public BoolInputSig PTT
		{
			get
			{
				return base.BooleanInput[27235u];
			}
		}
		/// <summary>
		/// Mutes the outgoing audio from the device when set to true.
		/// </summary>
		public BoolInputSig Muted
		{
			get
			{
				return base.BooleanInput[27237u];
			}
		}
		/// <summary>
		/// When set to 'true', enables push-to-talk (PTT) mode and switches the touch screen into half-duplex mode—allowing voice transmission in only one direction at a time while the cue is 'true'. Set to 'false' to disable PTT.
		/// </summary>
		public BoolInputSig PTTMode
		{
			get
			{
				return base.BooleanInput[27253u];
			}
		}
		/// <summary>
		/// Indicated that PTT transmission is active. 'true' - PTT active. 'false' - PTT inactive.
		/// <para>The <see cref="E:Crestron.SimplSharpPro.DeviceExtender.DeviceExtenderSigChange" /> event will trigger with <see cref="F:Crestron.SimplSharpPro.eSigEvent.BoolChange" /> and <see cref="P:Crestron.SimplSharpPro.DeviceSupport.Tswx52VoipReservedSigs.PTTFeedback" />.</para>
		/// </summary>
		public BoolOutputSig PTTFeedback
		{
			get
			{
				return base.BooleanOutput[27235u];
			}
		}
		/// <summary>
		/// Indicates that audio transmission to the target device is muted. 'true' - muted.
		/// <para>The <see cref="E:Crestron.SimplSharpPro.DeviceExtender.DeviceExtenderSigChange" /> event will trigger with <see cref="F:Crestron.SimplSharpPro.eSigEvent.BoolChange" /> and <see cref="P:Crestron.SimplSharpPro.DeviceSupport.Tswx52VoipReservedSigs.MutedFeedback" />.</para>
		/// </summary>
		public BoolOutputSig MutedFeedback
		{
			get
			{
				return base.BooleanOutput[27237u];
			}
		}
		/// <summary>
		/// Indicates that the target device is PTT. The output is 'true' for as long as PTT mode is enabled.
		/// In addition, a BPT system pulsing this cue will enable audio to a door stations connected via the PREVIEW function.
		/// When the output is 'false', this indicates a full-duplex device.
		/// <para>The <see cref="E:Crestron.SimplSharpPro.DeviceExtender.DeviceExtenderSigChange" /> event will trigger with <see cref="F:Crestron.SimplSharpPro.eSigEvent.BoolChange" /> and <see cref="P:Crestron.SimplSharpPro.DeviceSupport.Tswx52VoipReservedSigs.PTTModeFeedback" />.</para>
		/// </summary>
		public BoolOutputSig PTTModeFeedback
		{
			get
			{
				return base.BooleanOutput[27253u];
			}
		}
		/// <summary>
		/// Reserved for future use.
		/// </summary>
		public StringInputSig CommandString
		{
			get
			{
				return base.StringInput[18608u];
			}
		}
		/// <summary>
		/// Used to dial multiple devices at once. All devices in the group will ring. Once one answers, the remaining calls will be terminated
		/// </summary>
		public StringInputSig GroupToDial
		{
			get
			{
				return base.StringInput[18612u];
			}
		}
		/// <summary>
		/// Reserved for future use.
		/// <para>The <see cref="E:Crestron.SimplSharpPro.DeviceExtender.DeviceExtenderSigChange" /> event will trigger with <see cref="F:Crestron.SimplSharpPro.eSigEvent.StringChange" /> and <see cref="P:Crestron.SimplSharpPro.DeviceSupport.Tswx52VoipReservedSigs.CommandStringFeedback" />.</para>
		/// </summary>
		public StringOutputSig CommandStringFeedback
		{
			get
			{
				return base.StringOutput[18608u];
			}
		}
		/// <summary>
		/// Parameter to display the Incoming Display Name.
		/// <para>The <see cref="E:Crestron.SimplSharpPro.DeviceExtender.DeviceExtenderSigChange" /> event will trigger with <see cref="F:Crestron.SimplSharpPro.eSigEvent.StringChange" /> and <see cref="P:Crestron.SimplSharpPro.DeviceSupport.Tswx52VoipReservedSigs.IncomingDisplayNameFeedback" />.</para>
		/// </summary>
		public StringOutputSig IncomingDisplayNameFeedback
		{
			get
			{
				return base.StringOutput[18613u];
			}
		}

        ///// <summary>
        ///// Override of the base class to configure what Sigs to override. 
        ///// </summary>
        //protected override void InitializeJoins()
        //{
        //    base.InitializeJoins();
        //    base.MapDigitalInputJoins(new uint[]
        //    {
        //        27235u,
        //        27237u,
        //        27253u
        //    });
        //    base.MapDigitalOutputJoins(new uint[]
        //    {
        //        27235u,
        //        27237u,
        //        27253u
        //    });
        //    base.MapSerialInputJoins(new uint[]
        //    {
        //        18608u,
        //        18612u
        //    });
        //    base.MapSerialOutputJoins(new uint[]
        //    {
        //        18608u,
        //        18613u
        //    });
        //}
        public override void Use()
        {
            base.Use();

    //if (this._InUse)
    //{
    //    return;
    //}
    //if (this.ParentDevice.Registered)
    //{
    //    throw new InvalidOperationException("Unable to use extender on device that is already registered.");
    //}
    //this._InUse = true;
    //this.InitializeJoins();

        }
        #region ISmartObject 成员

        public uint LoadSmartObjects(ISmartObject deviceWithSmartObjects)
        {
          //  this._InUse
          //  throw new NotImplementedException();
            return 0;
        }

        public uint LoadSmartObjects(Crestron.SimplSharp.CrestronIO.Stream paramSgdStream)
        {
          //  throw new NotImplementedException();
            return 0;
        }

        public uint LoadSmartObjects(string paramSgdFilename)
        {
          //  throw new NotImplementedException();
            return 0;
        }

        public SmartObjectCollection SmartObjects
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}