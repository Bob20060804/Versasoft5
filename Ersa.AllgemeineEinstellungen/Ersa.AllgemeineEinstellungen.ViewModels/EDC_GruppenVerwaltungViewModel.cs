using Ersa.AllgemeineEinstellungen.GruppenVerwaltung;
using Ersa.Global.Common;
using Ersa.Global.Controls.Extensions;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.UI.ViewModels;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_GruppenVerwaltungViewModel : EDC_NavigationsViewModel
	{
		private const string mC_strPropertyGruppenName = "PRO_strName";

		private readonly INF_MaschinenVerwaltungsDienst m_edcMaschinenVerwaltungsDienst;

		private readonly INF_AutorisierungsDienst m_edcAutorisierungsDienst;

		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcBasisDatenCapability;

		private readonly ICollectionView m_fdcGruppenView;

		private string m_strMaschinenTypKey;

		public EDC_SmartObservableCollection<EDC_Gruppe> PRO_lstGruppen
		{
			get;
			private set;
		}

		public DelegateCommand PRO_cmdGruppeHinzufuegen
		{
			get;
			private set;
		}

		public DelegateCommand<EDC_Gruppe> PRO_cmdGruppeAktivSetzen
		{
			get;
			private set;
		}

		public DelegateCommand<EDC_Gruppe> PRO_cmdGruppeUmbenennen
		{
			get;
			private set;
		}

		public override bool PRO_blnHatAenderung => PRO_lstGruppen.Any(delegate(EDC_Gruppe i_edcGruppe)
		{
			if (!i_edcGruppe.PRO_blnHatAenderung)
			{
				return i_edcGruppe.PRO_blnIstNeu;
			}
			return true;
		});

		public bool PRO_blnDarfEinstellungenEditieren
		{
			get
			{
				if (!base.PRO_edcShellViewModel.PRO_blnIstMaschineInProduktion)
				{
					return m_edcAutorisierungsDienst.FUN_blnIstBenutzerAutorisiert("BerechtigungProduktionSteuern");
				}
				return false;
			}
		}

		public string PRO_strMaschinenTypKey
		{
			get
			{
				return m_strMaschinenTypKey;
			}
			private set
			{
				SetProperty(ref m_strMaschinenTypKey, value, "PRO_strMaschinenTypKey");
			}
		}

		private EDC_Gruppe PRO_edcAusgewaehlteGruppe => m_fdcGruppenView.CurrentItem as EDC_Gruppe;

		[ImportingConstructor]
		public EDC_GruppenVerwaltungViewModel(INF_MaschinenVerwaltungsDienst i_edcMaschinenVerwaltungsDienst, INF_AutorisierungsDienst i_edcAutorisierungsDienst, INF_CapabilityProvider i_edcCapabilityProvider)
		{
			m_edcMaschinenVerwaltungsDienst = i_edcMaschinenVerwaltungsDienst;
			m_edcAutorisierungsDienst = i_edcAutorisierungsDienst;
			m_edcBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
			PRO_lstGruppen = new EDC_SmartObservableCollection<EDC_Gruppe>();
			m_fdcGruppenView = CollectionViewSource.GetDefaultView(PRO_lstGruppen);
			m_fdcGruppenView.SortDescriptions.Add(new SortDescription("PRO_strName", ListSortDirection.Ascending));
			m_fdcGruppenView.SUB_LiveSortingAktivieren("PRO_strName");
			m_fdcGruppenView.CurrentChanged += SUB_OnCurrentChanged;
			PRO_cmdGruppeHinzufuegen = new DelegateCommand(SUB_GruppeHinzufuegen, () => PRO_blnDarfEinstellungenEditieren);
			PRO_cmdGruppeAktivSetzen = new DelegateCommand<EDC_Gruppe>(SUB_GruppeAktivSetzen);
			PRO_cmdGruppeUmbenennen = new DelegateCommand<EDC_Gruppe>(SUB_GruppeUmbenennen, delegate(EDC_Gruppe i_edcGruppe)
			{
				if (i_edcGruppe != null)
				{
					return !i_edcGruppe.PRO_blnIstStandard;
				}
				return false;
			});
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			PRO_strMaschinenTypKey = FUN_strMaschinenTypKeyErmitteln(m_edcBasisDatenCapability.Value.FUN_strHoleMaschinenTyp());
			IEnumerable<EDC_Gruppe> i_enuElemente = await FUN_enuGruppenLadenAsync();
			PRO_lstGruppen.SUB_Reset(i_enuElemente);
		}

		public override async Task FUN_fdcAenderungenSpeichernAsync()
		{
			List<EDC_Gruppe> list = (from i_edcGruppe in PRO_lstGruppen
			where i_edcGruppe.PRO_blnIstNeu
			select i_edcGruppe).ToList();
			foreach (EDC_Gruppe edcGruppe in list)
			{
				edcGruppe.PRO_i64Id = await m_edcMaschinenVerwaltungsDienst.FUN_i64GruppeErstellenAsync(edcGruppe.PRO_strName);
				edcGruppe.SUB_AenderungenUebernehmen();
			}
			foreach (EDC_Gruppe edcGruppe in from i_edcGruppe in PRO_lstGruppen
			where i_edcGruppe.PRO_blnHatAenderung
			select i_edcGruppe)
			{
				await m_edcMaschinenVerwaltungsDienst.FUN_fdcGruppeUmbenennenAsync(edcGruppe.PRO_i64Id, edcGruppe.PRO_strName);
				edcGruppe.SUB_AenderungenUebernehmen();
			}
			long[] ia_i64GruppenIds = (from i_edcGruppe in PRO_lstGruppen
			where i_edcGruppe.PRO_blnIstAktiv
			select i_edcGruppe.PRO_i64Id).ToArray();
			await m_edcMaschinenVerwaltungsDienst.FUN_fdcAktiveGruppenIdsSetzenAsync(ia_i64GruppenIds);
			await base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override Task FUN_fdcAenderungenVerwerfenAsync()
		{
			foreach (EDC_Gruppe item in (from i_edcGruppe in PRO_lstGruppen
			where i_edcGruppe.PRO_blnIstNeu
			select i_edcGruppe).ToList())
			{
				PRO_lstGruppen.Remove(item);
			}
			foreach (EDC_Gruppe item2 in from i_edcGruppe in PRO_lstGruppen
			where i_edcGruppe.PRO_blnHatAenderung
			select i_edcGruppe)
			{
				item2.SUB_AenderungenVerwerfen();
			}
			return base.FUN_fdcAenderungenVerwerfenAsync();
		}

		protected override void SUB_BerechtigungenAuswerten()
		{
			base.SUB_BerechtigungenAuswerten();
			RaisePropertyChanged("PRO_blnDarfEinstellungenEditieren");
			PRO_cmdGruppeHinzufuegen.RaiseCanExecuteChanged();
		}

		private async Task<IEnumerable<EDC_Gruppe>> FUN_enuGruppenLadenAsync()
		{
			string strDefaultGruppenName = await m_edcMaschinenVerwaltungsDienst.FUN_strHoleDefaultGruppenNameAsync();
			long[] a_i64AktiveGruppenIds = await m_edcMaschinenVerwaltungsDienst.FUNa_i64AktiveGruppenIdsErmittelnAsync();
			return (from i_edcGruppe in await m_edcMaschinenVerwaltungsDienst.FUN_enuGruppenFuerMaschinenTypErmittelnAsync()
			select new EDC_Gruppe(i_edcGruppe.PRO_strGruppenName, a_i64AktiveGruppenIds.Contains(i_edcGruppe.PRO_i64GruppenId))
			{
				PRO_i64Id = i_edcGruppe.PRO_i64GruppenId,
				PRO_blnIstStandard = (i_edcGruppe.PRO_strGruppenName == strDefaultGruppenName)
			}).ToList();
		}

		private string FUN_strMaschinenTypKeyErmitteln(string i_strMaschinenTyp)
		{
			if (i_strMaschinenTyp == "S")
			{
				return "13_847";
			}
			if (i_strMaschinenTyp == "W")
			{
				return "18_82";
			}
			return "10_458";
		}

		private void SUB_GruppeHinzufuegen()
		{
			string strFehlerStringFormat = base.PRO_edcLokalisierungsDienst.FUN_strText("13_360");
			string text = base.PRO_edcInteractionController.FUN_strTextEingabeAbfrageAnzeigen(base.PRO_edcLokalisierungsDienst.FUN_strText("1_25"), base.PRO_edcLokalisierungsDienst.FUN_strText("13_359"), null, base.PRO_edcLokalisierungsDienst.FUN_strText("13_353"), null, i_blnReturnErlaubt: false, delegate(string i_strEingabe)
			{
				if (!FUN_blnExistiertGruppeMitNamen(i_strEingabe))
				{
					return string.Empty;
				}
				return string.Format(strFehlerStringFormat, i_strEingabe);
			});
			if (!string.IsNullOrEmpty(text))
			{
				PRO_lstGruppen.Add(new EDC_Gruppe(text, i_blnAktiv: false));
				RaisePropertyChanged("PRO_blnHatAenderung");
			}
		}

		private void SUB_GruppeAktivSetzen(EDC_Gruppe i_edcGruppe)
		{
			if (i_edcGruppe != null)
			{
				foreach (EDC_Gruppe item in PRO_lstGruppen)
				{
					item.PRO_blnIstAktiv = (item == i_edcGruppe);
				}
				RaisePropertyChanged("PRO_blnHatAenderung");
			}
		}

		private void SUB_GruppeUmbenennen(EDC_Gruppe i_edcGruppe)
		{
			if (i_edcGruppe != null && !i_edcGruppe.PRO_blnIstStandard)
			{
				string strFehlerStringFormat = base.PRO_edcLokalisierungsDienst.FUN_strText("13_360");
				string text = base.PRO_edcInteractionController.FUN_strTextEingabeAbfrageAnzeigen(base.PRO_edcLokalisierungsDienst.FUN_strText("13_362"), base.PRO_edcLokalisierungsDienst.FUN_strText("13_318"), i_edcGruppe.PRO_strName, base.PRO_edcLokalisierungsDienst.FUN_strText("1_231"), null, i_blnReturnErlaubt: false, delegate(string i_strEingabe)
				{
					if (!FUN_blnExistiertGruppeMitNamen(i_strEingabe))
					{
						return string.Empty;
					}
					return string.Format(strFehlerStringFormat, i_strEingabe);
				});
				if (!string.IsNullOrEmpty(text))
				{
					i_edcGruppe.PRO_strName = text;
					RaisePropertyChanged("PRO_blnHatAenderung");
				}
			}
		}

		private bool FUN_blnExistiertGruppeMitNamen(string i_strName)
		{
			return PRO_lstGruppen.Any((EDC_Gruppe i_edcGruppe) => i_edcGruppe.PRO_strName == i_strName);
		}

		private void SUB_OnCurrentChanged(object i_objSender, EventArgs i_fdcArgs)
		{
			foreach (EDC_Gruppe item in PRO_lstGruppen)
			{
				item.PRO_blnIstAusgewaehlt = (item == PRO_edcAusgewaehlteGruppe);
			}
		}
	}
}
