using Ersa.Global.Common;
using Ersa.Global.Common.Extensions;
using Ersa.Global.Mvvm;
using Ersa.Platform.UI.Common;
using Ersa.Platform.UI.Common.Interfaces;
using Ersa.Platform.UI.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Ersa.Platform.UI.ParallelBedienung
{
	[Export]
	public class EDC_AnwendungsBildschirmeEinstellungenViewModel : BindableBase
	{
		private readonly INF_BildschirmVerwaltungsDienst m_edcBildschirmVerwaltungsDienst;

		private readonly INF_AnwendungsBildschirmeDienst m_edcAnwendungsBildschirmeDienst;

		private readonly INF_ShellViewModel m_edcShellViewModel;

		public EDC_SmartObservableCollection<EDC_BildschirmEinstellungen> PRO_lstBildschirmEinstellungen
		{
			get;
			private set;
		}

		public bool PRO_blnHatAenderung
		{
			get
			{
				List<int> i_enuListe = m_edcAnwendungsBildschirmeDienst.FUN_enuAnwendungsBildschirmeErmitteln().ToList();
				List<int> i_enuListe2 = FUN_enuAktiveBilschirmNummernErmitteln(PRO_lstBildschirmEinstellungen).ToList();
				return !i_enuListe.FUN_blnIdentisch(i_enuListe2);
			}
		}

		[ImportingConstructor]
		public EDC_AnwendungsBildschirmeEinstellungenViewModel(INF_BildschirmVerwaltungsDienst i_edcBildschirmVerwaltungsDienst, INF_AnwendungsBildschirmeDienst i_edcAnwendungsBildschirmeDienst, INF_ShellViewModel i_edcShellViewModel)
		{
			m_edcBildschirmVerwaltungsDienst = i_edcBildschirmVerwaltungsDienst;
			m_edcAnwendungsBildschirmeDienst = i_edcAnwendungsBildschirmeDienst;
			m_edcShellViewModel = i_edcShellViewModel;
			PRO_lstBildschirmEinstellungen = new EDC_SmartObservableCollection<EDC_BildschirmEinstellungen>();
		}

		public void SUB_Initialisieren()
		{
			List<EDC_BildschirmEinstellungen> i_enuElemente = FUN_lstBildschirmEinstellungenErmitteln();
			PRO_lstBildschirmEinstellungen.SUB_Reset(i_enuElemente);
		}

		public void SUB_AenderungenSpeichern()
		{
			SUB_AenderungenSpeichern(PRO_lstBildschirmEinstellungen);
			m_edcShellViewModel.SUB_AnwendungsBildschirmeAktualisieren();
		}

		public void SUB_AenderungenVerwerfen()
		{
			SUB_Initialisieren();
		}

		private static IEnumerable<int> FUN_enuAktiveBilschirmNummernErmitteln(IEnumerable<EDC_BildschirmEinstellungen> i_enuEinstellungen)
		{
			return from i_edcBildschirm in i_enuEinstellungen
			where i_edcBildschirm.PRO_blnIstAktiv
			select i_edcBildschirm.PRO_i32Nummer;
		}

		private static bool FUN_blnEinstellungenKorrigierenWennNoetig(List<EDC_BildschirmEinstellungen> i_lstBildschirmEinstellungen)
		{
			bool result = false;
			if (!i_lstBildschirmEinstellungen.Any((EDC_BildschirmEinstellungen i_edcBildschirm) => i_edcBildschirm.PRO_blnIstAktiv))
			{
				EDC_BildschirmEinstellungen eDC_BildschirmEinstellungen = i_lstBildschirmEinstellungen.FirstOrDefault((EDC_BildschirmEinstellungen i_edcBildschirm) => i_edcBildschirm.PRO_blnIstPrimaer) ?? i_lstBildschirmEinstellungen.FirstOrDefault();
				if (eDC_BildschirmEinstellungen != null)
				{
					eDC_BildschirmEinstellungen.PRO_blnIstAktiv = true;
					result = true;
				}
			}
			EDC_BildschirmEinstellungen eDC_BildschirmEinstellungen2 = i_lstBildschirmEinstellungen.FirstOrDefault((EDC_BildschirmEinstellungen i_edcBildschirm) => i_edcBildschirm.PRO_blnIstPrimaer);
			if (eDC_BildschirmEinstellungen2 != null && !eDC_BildschirmEinstellungen2.PRO_blnIstAktiv)
			{
				eDC_BildschirmEinstellungen2.PRO_blnIstAktiv = true;
				result = true;
			}
			return result;
		}

		private List<EDC_BildschirmEinstellungen> FUN_lstBildschirmEinstellungenErmitteln()
		{
			List<EDC_Bildschirm> source = m_edcBildschirmVerwaltungsDienst.FUN_enuAlleBildschirmeErmitteln().ToList();
			List<int> lstBildschirmeAusEinstellungen = m_edcAnwendungsBildschirmeDienst.FUN_enuAnwendungsBildschirmeErmitteln().ToList();
			List<EDC_BildschirmEinstellungen> list = (from i_edcBildschirm in source
			select new EDC_BildschirmEinstellungen(i_edcBildschirm.PRO_i32Nummer, i_edcBildschirm.PRO_blnIstPrimaerBildschirm, lstBildschirmeAusEinstellungen.Contains(i_edcBildschirm.PRO_i32Nummer))).ToList();
			if (FUN_blnEinstellungenKorrigierenWennNoetig(list))
			{
				SUB_AenderungenSpeichern(list);
			}
			return list;
		}

		private void SUB_AenderungenSpeichern(IEnumerable<EDC_BildschirmEinstellungen> i_enuEinstellungen)
		{
			IEnumerable<int> i_enuBildschirme = FUN_enuAktiveBilschirmNummernErmitteln(i_enuEinstellungen);
			m_edcAnwendungsBildschirmeDienst.SUB_AnwendungsBildchirmeFestlegen(i_enuBildschirme);
		}
	}
}
