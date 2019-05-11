using System.Collections.Generic;

namespace Ersa.Global.Common.Data.Filter
{
	public struct STRUCT_FilterBasisDefinition
	{
		public string PRO_strTabellenname
		{
			get;
			set;
		}

		public string PRO_strSpaltenName
		{
			get;
			set;
		}

		public string PRO_strDatentyp
		{
			get;
			set;
		}

		public string PRO_strKategorieNameKey
		{
			get;
			set;
		}

		public string PRO_strAnzeigeNameKey
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

		public IEnumerable<ENUM_FilterOperatoren> PRO_lstFilterOperationen
		{
			get;
			set;
		}

		public IEnumerable<ENUM_FilterVerknuepfung> PRO_lstFilterVerknuepfungen
		{
			get;
			set;
		}
	}
}
