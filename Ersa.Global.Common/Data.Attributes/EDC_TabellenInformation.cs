using System;

namespace Ersa.Global.Common.Data.Attributes
{
	public class EDC_TabellenInformation : Attribute
	{
		public string PRO_strName
		{
			get;
			set;
		}

		public bool PRO_blnNameIstProperty
		{
			get;
			set;
		}

		public bool PRO_blnQueryIstProperty
		{
			get;
			set;
		}

		public bool PRO_blnIsQuery
		{
			get;
			set;
		}

		public string PRO_strTablespace
		{
			get;
			set;
		}

		public string PRO_strQueryStatement
		{
			get;
			set;
		}

		public string PRO_strUniqueCombinedIndex
		{
			get;
			set;
		}

		public string PRO_strNonUniqueCombinedIndex
		{
			get;
			set;
		}

		public EDC_TabellenInformation(string i_strName)
		{
			PRO_strName = i_strName;
			PRO_blnIsQuery = false;
			PRO_blnQueryIstProperty = false;
			PRO_blnNameIstProperty = false;
			PRO_strQueryStatement = string.Empty;
			PRO_strTablespace = string.Empty;
		}

		public EDC_TabellenInformation(string i_strQueryStatement, bool i_blnIsQuery)
		{
			PRO_strName = string.Empty;
			PRO_blnNameIstProperty = false;
			PRO_blnIsQuery = i_blnIsQuery;
			PRO_blnQueryIstProperty = false;
			PRO_strQueryStatement = i_strQueryStatement;
			PRO_strTablespace = string.Empty;
		}
	}
}
