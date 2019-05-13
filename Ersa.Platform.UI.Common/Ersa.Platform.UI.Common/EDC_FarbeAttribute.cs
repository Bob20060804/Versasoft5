using System;

namespace Ersa.Platform.UI.Common
{
	[AttributeUsage(AttributeTargets.All)]
	public class EDC_FarbeAttribute : Attribute
	{
		public string PRO_strFarbe
		{
			get;
			private set;
		}

		public EDC_FarbeAttribute()
			: this(null)
		{
		}

		public EDC_FarbeAttribute(string i_strFarbe)
		{
			PRO_strFarbe = i_strFarbe;
		}
	}
}
