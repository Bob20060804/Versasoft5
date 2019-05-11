using System.Collections.Generic;

namespace Ersa.Global.Common.Data.Filter
{
	public struct STRUCT_FilterKonkret
	{
		public int PRO_i32FilterPosition
		{
			get;
			set;
		}

		public int PRO_i32SortierReihenfolge
		{
			get;
			set;
		}

		public ENUM_FilterVerknuepfung PRO_edcFilterVerknüpfung
		{
			get;
			set;
		}

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

		public ENUM_FilterOperatoren PRO_edcFilterOperation
		{
			get;
			set;
		}

		public IEnumerable<object> PRO_lstExacterWerte
		{
			get;
			set;
		}

		public object PRO_objUntererWert
		{
			get;
			set;
		}

		public object PRO_objObererWert
		{
			get;
			set;
		}

		public ENUM_FilterVerknuepfung PRO_edcListenVerknüpfung
		{
			get;
			set;
		}

		public IEnumerable<STRUCT_FilterKonkret> PRO_lstKonkreteFilter
		{
			get;
			set;
		}
	}
}
