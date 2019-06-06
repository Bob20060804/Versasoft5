using System;

namespace Ersa.Global.Controls.Helpers
{
	public class EDC_EventCommandArgs
	{
		public object PRO_objSender
		{
			get;
			private set;
		}

		public EventArgs PRO_fdcEventArgs
		{
			get;
			private set;
		}

		public EDC_EventCommandArgs(object i_objSender, EventArgs i_fdcArgs)
		{
			PRO_objSender = i_objSender;
			PRO_fdcEventArgs = i_fdcArgs;
		}
	}
}
