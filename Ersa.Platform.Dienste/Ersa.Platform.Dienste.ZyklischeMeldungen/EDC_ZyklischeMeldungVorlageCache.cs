using Ersa.Global.Common.Extensions;
using Ersa.Platform.Common.Meldungen;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Ersa.Platform.Dienste.ZyklischeMeldungen
{
	[Export]
	public class EDC_ZyklischeMeldungVorlageCache
	{
		private const string C_strTextNichtVorhandenString = "- - -";

		private readonly IList<EDC_ZyklischeMeldungVorlageGruppe> m_lstGruppen = new List<EDC_ZyklischeMeldungVorlageGruppe>();

		public void SUB_CacheInhaltSetzen(IEnumerable<EDC_ZyklischeMeldungVorlageGruppe> i_enuGruppen)
		{
			m_lstGruppen.Clear();
			m_lstGruppen.AddRange(i_enuGruppen);
		}

		public IList<EDC_ZyklischeMeldungVorlageGruppe> FUN_lstCacheInhaltErmitteln()
		{
			return m_lstGruppen.ToList();
		}

		public EDC_ZyklischeMeldungVorlage FUN_edcVorlageErmitteln(int i_i32Id)
		{
			return m_lstGruppen.SelectMany((EDC_ZyklischeMeldungVorlageGruppe i_edcGruppe) => i_edcGruppe.PRO_lstVorlagen).SingleOrDefault((EDC_ZyklischeMeldungVorlage i_edcVorlage) => i_edcVorlage.PRO_i32Id == i_i32Id) ?? FUN_edcDefaultVorlageErstellen(i_i32Id);
		}

		private EDC_ZyklischeMeldungVorlage FUN_edcDefaultVorlageErstellen(int i_i32Id)
		{
			return new EDC_ZyklischeMeldungVorlage
			{
				PRO_i32Id = i_i32Id,
				PRO_strMeldeort1 = "- - -",
				PRO_strMeldeort2 = "- - -",
				PRO_strMeldeort3 = "- - -",
				PRO_strMeldetext = "- - -"
			};
		}
	}
}
