using System;
using System.Collections.Generic;

namespace Ersa.Global.Common.Data.Attributes
{
	public class EDC_FilterInformationen : Attribute
	{
		public string PRO_strSpaltenName
		{
			get;
			set;
		}

		public string PRO_strTabellenName
		{
			get;
			set;
		}

		public string PRO_strFilterAnzeigeNameKey
		{
			get;
			set;
		}

		public string PRO_strFilterKategorieNameKey
		{
			get;
			set;
		}

		public object PRO_objMinimalWert
		{
			get;
			set;
		}

		public object PRO_objMaximalWert
		{
			get;
			set;
		}

		public IEnumerable<object> PRO_lstWerteListe
		{
			get;
			set;
		}

		public string PRO_strFilterOperationen
		{
			get;
			set;
		}

		public string PRO_strFilterVerknuepfungen
		{
			get;
			set;
		}

		public EDC_FilterInformationen()
		{
		}

		public EDC_FilterInformationen(string i_strTabellenName, string i_strSpaltenName)
		{
			PRO_strTabellenName = i_strTabellenName;
			PRO_strSpaltenName = i_strSpaltenName;
		}
	}
}
