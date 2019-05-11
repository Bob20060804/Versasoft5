using Ersa.Global.Common.Data.Attributes;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Meldungen
{
	[EDC_TabellenInformation("Select Messages.*,MessagesCyclic.*, MessageContext.Details, MessageContext.Context,  case when Users.Username is null Then '' else Users.Username END as Username From Messages Left Outer Join MessagesCyclic On Messages.MessageId = MessagesCyclic.MessageId Left Outer Join MessageContext On Messages.MessageId = MessageContext.MessageId Left Outer Join Users On Messages.UserId = Users.UserId", true)]
	public class EDC_MeldungenAbfrageData : EDC_MeldungData
	{
		public const string mC_strQuery = "Select Messages.*,MessagesCyclic.*, MessageContext.Details, MessageContext.Context,  case when Users.Username is null Then '' else Users.Username END as Username From Messages Left Outer Join MessagesCyclic On Messages.MessageId = MessagesCyclic.MessageId Left Outer Join MessageContext On Messages.MessageId = MessageContext.MessageId Left Outer Join Users On Messages.UserId = Users.UserId";

		private const string mC_strStartzeitpunktParameterName = "pStart";

		private const string mC_strEndzeitpunktParameterName = "pEnde";

		[EDC_SpaltenInformation("Message", PRO_i32Length = 200, PRO_blnIsRequired = true)]
		public string PRO_strMeldung
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility1", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt1
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility2", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt2
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Facility3", PRO_i32Length = 100)]
		public string PRO_strMeldungsOrt3
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("InfeedBlocked")]
		public bool PRO_blnEinlaufSperreAktiv
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Username", PRO_i32Length = 20, PRO_blnIsRequired = true)]
		public string PRO_strBenutzername
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Details")]
		public string PRO_strDetails
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Context")]
		public string PRO_strContext
		{
			get;
			set;
		}

		public EDC_MeldungenAbfrageData()
		{
		}

		public EDC_MeldungenAbfrageData(string i_strWhereStatement)
			: this()
		{
			base.PRO_strWhereStatement = i_strWhereStatement;
		}

		public static string FUN_strAlleAufgetretenWhereStatementErstellen(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, DateTime i_fdcStartzeitpunkt, DateTime i_fdcEndzeitpunkt, long i_i64MaschinenId, Dictionary<string, object> i_fdcDictionary)
		{
			i_fdcDictionary.Add("pStart", i_fdcStartzeitpunkt);
			i_fdcDictionary.Add("pEnde", i_fdcEndzeitpunkt);
			string text = string.Format("Where {0}.{1} between @{2} and @{3} and {4}.{5} = {6}", "Messages", "Occurred", "pStart", "pEnde", "Messages", "MachineId", i_i64MaschinenId);
			if (!ENUM_MeldungProduzent.Alle.Equals(i_enmMeldungProduzent))
			{
				text = string.Format("{0} and {1}.{2}={3}", text, "Messages", "System", (int)i_enmMeldungProduzent);
			}
			if (!ENUM_MeldungsTypen.enmUndefiniert.Equals(i_enmMeldungsTyp))
			{
				text = string.Format("{0} and {1}.{2}={3}", text, "Messages", "MessageType", (int)i_enmMeldungsTyp);
			}
			return text;
		}

		public static string FUN_strAlleQuittiertenWhereStatementErstellen(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, DateTime i_fdcStartzeitpunkt, DateTime i_fdcEndzeitpunkt, long i_i64MaschinenId, Dictionary<string, object> i_fdcDictionary)
		{
			string text = FUN_strAlleAufgetretenWhereStatementErstellen(i_enmMeldungProduzent, i_enmMeldungsTyp, i_fdcStartzeitpunkt, i_fdcEndzeitpunkt, i_i64MaschinenId, i_fdcDictionary);
			return string.Format("{0} and ({1}.{2} is not null or {3}.{4} is not null)", text, "Messages", "Acknowledged", "Messages", "MessageReset");
		}

		public static string FUN_strNichtQittierteMeldungenWhereStatementErstellen(ENUM_MeldungProduzent i_enmMeldungProduzent, long i_i64MaschinenId)
		{
			string text = string.Format("Where {0}.{1} = {2} and {3}.{4} is null and {5}.{6} is null", "Messages", "MachineId", i_i64MaschinenId, "Messages", "Acknowledged", "Messages", "MessageReset");
			if (!ENUM_MeldungProduzent.Alle.Equals(i_enmMeldungProduzent))
			{
				text = string.Format("{0} and {1}.{2}={3}", text, "Messages", "System", (int)i_enmMeldungProduzent);
			}
			return string.Format("{0} order by {1}.{2}", text, "Messages", "Occurred");
		}
	}
}
