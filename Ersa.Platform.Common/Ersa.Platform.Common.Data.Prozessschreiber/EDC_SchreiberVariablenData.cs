using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Prozessschreiber
{
	[EDC_TabellenInformation("RecorderVariables", PRO_strTablespace = "ess5_recorder")]
	public class EDC_SchreiberVariablenData
	{
		public const string mC_strTabellenName = "RecorderVariables";

		public const string mC_strSpalteMachineId = "MachineId";

		public const string mC_strSpalteVariable = "Variable";

		public const string mC_strSpalteEinheitKey = "UnitKey";

		public const string mC_strSpalteNameKeyArray = "NameKeyArray";

		public const string mC_strSpalteColumnName = "ColumnName";

		public const string mC_strSpalteDataType = "DataType";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Variable", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 200)]
		public string PRO_strVariable
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

		[EDC_SpaltenInformation("NameKeyArray", PRO_i32Length = 160)]
		public string PRO_strNameKeyArray
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
		public string PRO_strSpaltenDatenTyp
		{
			get;
			set;
		}

		public int PRO_i32SpaltenDatenLaenge
		{
			get;
			set;
		}

		public DateTime PRO_dtmTimeStamp
		{
			get;
			set;
		}

		public object PRO_objWert
		{
			get;
			set;
		}

		public double PRO_dblWert => Convert.ToDouble(PRO_objWert);

		public EDC_SchreiberVariablenData()
		{
		}

		public EDC_SchreiberVariablenData(string i_strWhereStatement)
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMaschinenVariablenWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1} ", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strSelectCountStatementAufMaschinenIdErstellen(long i_i64MaschinenId)
		{
			return string.Format("Select max(SUBSTRING({0}, 2, 3)) From {1} Where {2} = {3} ", "ColumnName", "RecorderVariables", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strSelectCountStatementExactAufMaschinenIdErstellen(long i_i64MaschinenId)
		{
			return string.Format("Select Count(*) From {0} Where {1} = {2} ", "RecorderVariables", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strLoescheMaschinenVariablenStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Delete From {0} Where {1} = {2}", "RecorderVariables", "MachineId", i_i64MaschinenId);
		}

		public override bool Equals(object i_objVergleichsObjekt)
		{
			EDC_SchreiberVariablenData eDC_SchreiberVariablenData = i_objVergleichsObjekt as EDC_SchreiberVariablenData;
			if (eDC_SchreiberVariablenData == null)
			{
				return false;
			}
			if (PRO_i64MaschinenId == eDC_SchreiberVariablenData.PRO_i64MaschinenId)
			{
				return PRO_strVariable == eDC_SchreiberVariablenData.PRO_strVariable;
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
