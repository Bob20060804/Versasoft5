using Ersa.Global.Common.Data.Attributes;
using System.Collections.Generic;
using System.Globalization;

namespace Ersa.Platform.Common.Data.Loetprotokoll
{
	[EDC_TabellenInformation("ProtocolVariables", PRO_strTablespace = "ess5_protocol")]
	public class EDC_LoetprotokollVariablenData
	{
		public const string gC_blnTabellenName = "ProtocolVariables";

		public const string mC_strSpalteVariable = "Variable";

		private const string mC_strSpalteMaschinenId = "MachineId";

		private const string mC_strSpalteNameKeyArray = "NameKeyArray";

		private const string mC_strSpalteEinheitenKey = "UnitKey";

		private const string mC_strSpalteSpaltenName = "ColumnName";

		private const string mC_strSpalteDatentyp = "DataType";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true, PRO_blnIsPrimary = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Variable", PRO_blnIsRequired = true, PRO_blnIsPrimary = true, PRO_i32Length = 200)]
		public string PRO_strVariable
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("NameKeyArray", PRO_i32Length = 160)]
		public string PRO_strNameKeyArray
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("UnitKey", PRO_i32Length = 10)]
		public string PRO_strEinheitKey
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ColumnName", PRO_blnIsRequired = true, PRO_i32Length = 4)]
		public string PRO_strSpaltenName
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("DataType", PRO_blnIsRequired = true, PRO_i32Length = 15)]
		public string PRO_strDatentyp
		{
			get;
			set;
		}

		public int PRO_i32SpaltenDatenLaenge
		{
			get;
			set;
		}

		public object PRO_objWert
		{
			get;
			set;
		}

		public EDC_LoetprotokollVariablenData()
		{
		}

		public EDC_LoetprotokollVariablenData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMaschinenVariablenWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} ", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strMaschinenVariablenWhereStatementErstellen(IEnumerable<long> i_lst64VersionsId)
		{
			string text = string.Empty;
			foreach (long item in i_lst64VersionsId)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += item.ToString(CultureInfo.InvariantCulture);
			}
			return string.Format("Where {0} in ({1}) order by {0}", "MachineId", text);
		}

		public static string FUN_strSelectCountStatementAufMaschinenIdErstellen(long i_i64MaschinenId)
		{
			return string.Format("Select count(*) From {0} Where {1} = {2} ", "ProtocolVariables", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strSelectMaxSpaltennummerFuerMaschinenIdErstellen(long i_i64MaschinenId)
		{
			return string.Format("Select max(substring({0}, 2, 3)) from {1} where {2} = {3}", "ColumnName", "ProtocolVariables", "MachineId", i_i64MaschinenId);
		}

		public override bool Equals(object i_objVergleichsObjekt)
		{
			EDC_LoetprotokollVariablenData eDC_LoetprotokollVariablenData = i_objVergleichsObjekt as EDC_LoetprotokollVariablenData;
			if (eDC_LoetprotokollVariablenData == null)
			{
				return false;
			}
			if (PRO_i64MaschinenId == eDC_LoetprotokollVariablenData.PRO_i64MaschinenId)
			{
				return PRO_strVariable == eDC_LoetprotokollVariablenData.PRO_strVariable;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return PRO_i64MaschinenId.GetHashCode() + PRO_strVariable.GetHashCode();
		}

		public void SUB_SetzeNamenKeyAnteil(string i_strKey)
		{
			if (!string.IsNullOrEmpty(i_strKey))
			{
				if (string.IsNullOrEmpty(PRO_strNameKeyArray))
				{
					PRO_strNameKeyArray = i_strKey;
				}
				else
				{
					PRO_strNameKeyArray = $"{i_strKey}|{PRO_strNameKeyArray}";
				}
			}
		}
	}
}
