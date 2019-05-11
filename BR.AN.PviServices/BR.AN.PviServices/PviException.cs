using System;

namespace BR.AN.PviServices
{
	public class PviException : System.Exception
	{
		private int error;

		private object sender;

		private PviEventArgs pviEvent;

		public virtual int Error
		{
			get
			{
				return error;
			}
			set
			{
				error = value;
			}
		}

		public virtual object Sender => sender;

		public virtual PviEventArgs PlcEvent => pviEvent;

		public PviException(string message, int error, object sender, PviEventArgs pviEvent)
			: base(message)
		{
			this.error = error;
			this.sender = sender;
			this.pviEvent = pviEvent;
		}
	}
}
