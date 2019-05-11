using Ersa.Global.Common.Data.Attributes;
using System;

namespace Ersa.Platform.Common.Data.Produktionssteuerung
{
	[EDC_TabellenInformation("ProductionControl", PRO_strTablespace = "ess5_production")]
	public class EDC_ProduktionssteuerungData
	{
		public const string gC_strTabellenName = "ProductionControl";

		public const string gC_strSpalteProduktionssteuerungId = "ProductionControlId";

		public const string gC_strSpalteMaschinenId = "MachineId";

		public const string gC_strSpalteIstAktiv = "IsActive";

		public const string gC_strSpalteBearbeitetAm = "ChangeDate";

		public const string gC_strSpalteBearbeitetVon = "ChangeUser";

		private const string mC_strSpalteBeschreibung = "Description";

		private const string mC_strSpalteEinstellungen = "Adjustments";

		private const string mC_strSpalteAngelegtAm = "CreationDate";

		private const string mC_strSpalteAngelegtVon = "CreationUser";

		public const string gC_strParameterBearbeitetAm = "pChangeDate";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ProductionControlId", PRO_blnIsPrimary = true, PRO_blnIsRequired = true)]
		public long PRO_i64ProduktionssteuerungId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("MachineId", PRO_blnIsRequired = true)]
		public long PRO_i64MaschinenId
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Description", PRO_i32Length = 250)]
		public string PRO_strBeschreibung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Adjustments")]
		public string PRO_strEinstellungen
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("IsActive")]
		public bool PRO_blnIstAktiv
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationDate")]
		public DateTime PRO_dtmAngelegtAm
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("CreationUser")]
		public long PRO_i64AngelegtVon
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeDate")]
		public DateTime PRO_dtmBearbeitetAm
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("ChangeUser")]
		public long PRO_i64BearbeitetVon
		{
			get;
			set;
		}

		public EDC_ProduktionssteuerungData()
		{
		}

		public EDC_ProduktionssteuerungData(string i_strWhereStatement)
			: this()
		{
			PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strMaschinenIdWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = {1}", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strProduktionssteuerungIdWhereStatementErstellen(long i_i64ProduktionssteuerungId)
		{
			return string.Format("Where {0} = {1}", "ProductionControlId", i_i64ProduktionssteuerungId);
		}

		public static string FUN_strMaschinenIdUndAktivWhereStatementErstellen(long i_i64MaschinenId)
		{
			return string.Format("Where {0} = '1' AND {1} = {2}", "IsActive", "MachineId", i_i64MaschinenId);
		}

		public static string FUN_strUpdateStatementAktivErstellen(long i_i64ProduktionssteuerungId, long i_i64BenutzerId)
		{
			return string.Format("Update {0} Set {1} = '1', {2} = {3}, {4} = @{5} where {6} = {7}", "ProductionControl", "IsActive", "ChangeUser", i_i64BenutzerId, "ChangeDate", "pChangeDate", "ProductionControlId", i_i64ProduktionssteuerungId);
		}

		public static string FUN_strUpdateStatementNichtAktivErstellen(long i_i64ProduktionssteuerungId, long i_i64BenutzerId, long i_i64MaschinenId)
		{
			return string.Format("Update {0} Set {1} = '0', {2} = {3}, {4} = @{5} where {6} = {7} and {8} <> {9}", "ProductionControl", "IsActive", "ChangeUser", i_i64BenutzerId, "ChangeDate", "pChangeDate", "MachineId", i_i64MaschinenId, "ProductionControlId", i_i64ProduktionssteuerungId);
		}
	}
}
