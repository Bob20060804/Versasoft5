using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.UI.Common
{
	public class EDC_HauptmenuEintragSpezifikation : BindableBase
	{
		public string PRO_strNameKey
		{
			get;
			set;
		}

		public Uri PRO_uriIcon
		{
			get;
			set;
		}

		public string PRO_strRecht
		{
			get;
			set;
		}

		public string PRO_strRechtNameKey
		{
			get;
			set;
		}

		public int PRO_i32Reihenfolge
		{
			get;
			set;
		}

		public int PRO_i32StartPrioritaet
		{
			get;
			set;
		}

		public bool PRO_blnIstStandardmaessigAktiviert
		{
			get;
			set;
		}

		public object PRO_objView
		{
			get;
			set;
		}

		public EDC_HauptmenuEintragSpezifikation()
		{
			PRO_blnIstStandardmaessigAktiviert = true;
			PRO_i32Reihenfolge = 5;
		}
	}
}
