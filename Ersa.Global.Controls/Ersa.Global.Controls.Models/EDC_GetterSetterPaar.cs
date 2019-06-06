using System;

namespace Ersa.Global.Controls.Models
{
	public class EDC_GetterSetterPaar
	{
		public Func<object> PRO_delGetter
		{
			get;
			private set;
		}

		public Action<object> PRO_delSetter
		{
			get;
			private set;
		}

		public EDC_GetterSetterPaar(Func<object> i_delGetter, Action<object> i_delSetter)
		{
			PRO_delGetter = i_delGetter;
			PRO_delSetter = i_delSetter;
		}
	}
}
