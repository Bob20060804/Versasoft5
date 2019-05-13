using Ersa.Platform.Common.LeseSchreibGeraete.Ruestkontrolle;

namespace Ersa.Platform.CapabilityContracts.LeseSchreibeGeraete
{
	public class EDC_AuftragRuestkontrolle
	{
		public bool PRO_blnLesen
		{
			get;
		}

		public bool PRO_blnTestLesen
		{
			get;
		}

		public ENUM_RuestWerkzeug? PRO_enmWerkzeug
		{
			get;
		}

		private EDC_AuftragRuestkontrolle(bool i_blnLesen, ENUM_RuestWerkzeug? i_enmWerkzeug = default(ENUM_RuestWerkzeug?), bool i_blnTestLesen = false)
		{
			PRO_blnLesen = i_blnLesen;
			PRO_enmWerkzeug = i_enmWerkzeug;
			PRO_blnTestLesen = i_blnTestLesen;
		}

		public static EDC_AuftragRuestkontrolle FUN_edcStartLesen(ENUM_RuestWerkzeug i_enmWerkzeug)
		{
			return new EDC_AuftragRuestkontrolle(i_blnLesen: true, i_enmWerkzeug);
		}

		public static EDC_AuftragRuestkontrolle FUN_edcStartTestLesen(ENUM_RuestWerkzeug i_enmWerkzeug)
		{
			return new EDC_AuftragRuestkontrolle(i_blnLesen: true, i_enmWerkzeug, i_blnTestLesen: true);
		}

		public static EDC_AuftragRuestkontrolle FUN_edcStopLesen()
		{
			return new EDC_AuftragRuestkontrolle(i_blnLesen: false);
		}
	}
}
