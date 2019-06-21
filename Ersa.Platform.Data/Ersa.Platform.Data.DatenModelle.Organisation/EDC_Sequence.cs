using Ersa.Global.Common.Data.Attributes;

namespace Ersa.Platform.Data.DatenModelle.Organisation
{
	[EDC_TabellenInformation("Sequences")]
	public class EDC_Sequence
	{
		public const string mC_strTabellenName = "Sequences";

		public const string mC_strSpalteSequence = "Name";

		public const string mC_strSpalteWert = "Value";

		[EDC_AbfrageInformation]
		public string PRO_strWhereStatement
		{
			get;
			private set;
		}

		[EDC_SpaltenInformation("Name", PRO_blnIsPrimary = true, PRO_blnIsRequired = true, PRO_i32Length = 100)]
		public string PRO_strSequence
		{
			get;
			set;
		}

		[EDC_SpaltenInformation("Value", PRO_blnIsRequired = true)]
		public long PRO_i64Wert
		{
			get;
			set;
		}
	}
}
