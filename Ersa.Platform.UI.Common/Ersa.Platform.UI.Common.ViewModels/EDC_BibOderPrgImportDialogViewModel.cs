using Ersa.Global.Mvvm;
using Ersa.Global.Mvvm.Commands;
using Ersa.Platform.Common.Exceptions;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Common.Helfer;
using Ersa.Platform.UI.Common.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ersa.Platform.UI.Common.ViewModels
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDC_BibOderPrgImportDialogViewModel : BindableBase
	{
		private readonly INF_IoDialogHelfer m_edcIoDialogHelfer;

		private string m_strStandardVerzeichnis;

		private IEnumerable<EDC_BibliothekId> m_enuExistierendeBibs;

		private IDictionary<ENUM_ImportFormat, EDC_BibOderPrgImportAbfrageItem> m_dicAbfrageItems;

		private bool m_blnIstBusy;

		private ENUM_BibOderPrgImportSchritt m_enmSchritt;

		private bool m_blnIstBibliothekAusgewaehlt;

		private string m_strImportDatei;

		private string m_strImportPfad;

		private string m_strImportErgebnis;

		private string m_strValidierungsFehler;

		private string m_strNeuerName;

		private EDC_BibliothekId m_edcBibAuswahl;

		private int m_i32FormatAuswahl;

		private IEnumerable<EDC_EnumMember> m_enuImportFormate;

		[Import]
		public INF_LokalisierungsDienst PRO_edcLokalisierungsDienst
		{
			get;
			set;
		}

		public AsyncCommand PRO_cmdWeiter
		{
			get;
			private set;
		}

		public DelegateCommand PRO_cmdZurueck
		{
			get;
			private set;
		}

		public DelegateCommand PRO_cmdPfadAuswahl
		{
			get;
			private set;
		}

		public ICommand PRO_cmdFormatGeaendert
		{
			get;
			private set;
		}

		public bool PRO_blnIstBusy
		{
			get
			{
				return m_blnIstBusy;
			}
			private set
			{
				SetProperty(ref m_blnIstBusy, value, "PRO_blnIstBusy");
			}
		}

		public ENUM_BibOderPrgImportSchritt PRO_enmSchritt
		{
			get
			{
				return m_enmSchritt;
			}
			private set
			{
				SetProperty(ref m_enmSchritt, value, "PRO_enmSchritt");
			}
		}

		public string PRO_strValidierungsFehler
		{
			get
			{
				return m_strValidierungsFehler;
			}
			private set
			{
				SetProperty(ref m_strValidierungsFehler, value, "PRO_strValidierungsFehler");
			}
		}

		public bool PRO_blnIstBibliothekAusgewaehlt
		{
			get
			{
				return m_blnIstBibliothekAusgewaehlt;
			}
			set
			{
				if (SetProperty(ref m_blnIstBibliothekAusgewaehlt, value, "PRO_blnIstBibliothekAusgewaehlt"))
				{
					PRO_strValidierungsFehler = string.Empty;
				}
				SUB_ImportEinstellungenGeaendert();
			}
		}

		public string PRO_strImportDatei
		{
			get
			{
				return m_strImportDatei;
			}
			set
			{
				if (SetProperty(ref m_strImportDatei, value, "PRO_strImportDatei"))
				{
					PRO_strValidierungsFehler = string.Empty;
				}
			}
		}

		public string PRO_strImportPfad
		{
			get
			{
				return m_strImportPfad;
			}
			set
			{
				if (SetProperty(ref m_strImportPfad, value, "PRO_strImportPfad"))
				{
					PRO_strValidierungsFehler = string.Empty;
				}
			}
		}

		public string PRO_strNeuerName
		{
			get
			{
				return m_strNeuerName;
			}
			set
			{
				if (SetProperty(ref m_strNeuerName, value, "PRO_strNeuerName"))
				{
					PRO_strValidierungsFehler = string.Empty;
				}
			}
		}

		public IEnumerable<EDC_EnumMember> PRO_enuImportFormate => m_enuImportFormate;

		public int PRO_i32FormatAuswahl
		{
			get
			{
				return m_i32FormatAuswahl;
			}
			set
			{
				SetProperty(ref m_i32FormatAuswahl, value, "PRO_i32FormatAuswahl");
			}
		}

		public EDC_BibliothekId PRO_edcBibAuswahl
		{
			get
			{
				return m_edcBibAuswahl;
			}
			set
			{
				if (SetProperty(ref m_edcBibAuswahl, value, "PRO_edcBibAuswahl"))
				{
					PRO_strValidierungsFehler = string.Empty;
				}
			}
		}

		public IEnumerable<EDC_BibliothekId> PRO_enuBibliotheken => m_enuExistierendeBibs;

		public string PRO_strImportErgebnis
		{
			get
			{
				return m_strImportErgebnis;
			}
			private set
			{
				SetProperty(ref m_strImportErgebnis, value, "PRO_strImportErgebnis");
			}
		}

		public string PRO_strVerzeichnisAuswahlText
		{
			get
			{
				string i_strKey = (!PRO_blnIstBibliothekAusgewaehlt) ? (FUN_edcHoleBibOderPrgAbfrageFuerFormat((ENUM_ImportFormat)PRO_i32FormatAuswahl).PRO_blnPrgAuswahlAlsVerzeichnis ? "13_711" : "13_710") : "13_712";
				return PRO_edcLokalisierungsDienst.FUN_strText(i_strKey);
			}
		}

		[ImportingConstructor]
		public EDC_BibOderPrgImportDialogViewModel(INF_IoDialogHelfer i_edcIoDialogHelfer)
		{
			m_edcIoDialogHelfer = i_edcIoDialogHelfer;
			PRO_cmdWeiter = new AsyncCommand(FUN_fdcWeiterAsync);
			PRO_cmdZurueck = new DelegateCommand(SUB_Zurueck);
			PRO_cmdPfadAuswahl = new DelegateCommand(SUB_PfadAuswahl);
			PRO_cmdFormatGeaendert = new DelegateCommand(SUB_FormatGeaendert);
		}

		public void SUB_Initialisieren(string i_strStandardVerzeichnis, IEnumerable<EDC_BibliothekId> i_enuExistierendeBibs, IDictionary<ENUM_ImportFormat, EDC_BibOderPrgImportAbfrageItem> i_dicAbfrageItems)
		{
			PRO_strValidierungsFehler = string.Empty;
			m_strStandardVerzeichnis = i_strStandardVerzeichnis;
			m_enuExistierendeBibs = i_enuExistierendeBibs;
			m_dicAbfrageItems = i_dicAbfrageItems;
			IEnumerable<EDC_EnumMember> source = EDC_EnumHelfer.FUN_enmBeschreibungenErmitteln(typeof(ENUM_ImportFormat));
			List<ENUM_ImportFormat> lstVerfuegbareImportFormate = i_dicAbfrageItems.Keys.ToList();
			m_enuImportFormate = from i_edcImportFormat in source
			where lstVerfuegbareImportFormate.Contains((ENUM_ImportFormat)i_edcImportFormat.PRO_enmValue)
			select i_edcImportFormat;
			ENUM_ImportFormat eNUM_ImportFormat = (ENUM_ImportFormat)(m_i32FormatAuswahl = (int)(lstVerfuegbareImportFormate.Contains(ENUM_ImportFormat.FormatErsasoft5) ? ENUM_ImportFormat.FormatErsasoft5 : lstVerfuegbareImportFormate.FirstOrDefault()));
		}

		private void SUB_FormatGeaendert()
		{
			PRO_strImportPfad = string.Empty;
			if (FUN_edcHoleBibOderPrgAbfrageFuerFormat((ENUM_ImportFormat)PRO_i32FormatAuswahl) == null)
			{
				PRO_strValidierungsFehler = PRO_edcLokalisierungsDienst.FUN_strText("13_663");
			}
			SUB_ImportEinstellungenGeaendert();
		}

		private void SUB_PfadAuswahl()
		{
			string i_strInitialesVerzeichnis = string.IsNullOrEmpty(PRO_strImportPfad) ? m_strStandardVerzeichnis : PRO_strImportPfad;
			EDC_BibOderPrgImportAbfrageItem eDC_BibOderPrgImportAbfrageItem = FUN_edcHoleBibOderPrgAbfrageFuerFormat((ENUM_ImportFormat)PRO_i32FormatAuswahl);
			string text = (PRO_blnIstBibliothekAusgewaehlt || eDC_BibOderPrgImportAbfrageItem.PRO_blnPrgAuswahlAlsVerzeichnis) ? m_edcIoDialogHelfer.FUN_strBrowseFolderDialog(i_strInitialesVerzeichnis) : m_edcIoDialogHelfer.FUN_strOpenFileDialog(i_strInitialesVerzeichnis, eDC_BibOderPrgImportAbfrageItem.PRO_strPrgDateiFilter);
			if (!string.IsNullOrEmpty(text))
			{
				PRO_strImportPfad = text;
			}
		}

		private void SUB_Zurueck()
		{
			if (PRO_enmSchritt == ENUM_BibOderPrgImportSchritt.EingabeImportOptionen)
			{
				PRO_strValidierungsFehler = string.Empty;
				PRO_enmSchritt = ENUM_BibOderPrgImportSchritt.AuswahlImportPfad;
			}
		}

		private async Task FUN_fdcWeiterAsync()
		{
			switch (PRO_enmSchritt)
			{
			case ENUM_BibOderPrgImportSchritt.AuswahlImportPfad:
				SUB_ZuEingabeImportOptionenWechseln();
				break;
			case ENUM_BibOderPrgImportSchritt.EingabeImportOptionen:
				await FUN_fdcZuImportWechselnAsync();
				break;
			}
		}

		private void SUB_ZuEingabeImportOptionenWechseln()
		{
			PRO_strValidierungsFehler = FUN_strAuswahlImportPfadValidieren();
			if (!string.IsNullOrEmpty(PRO_strValidierungsFehler))
			{
				return;
			}
			EDC_BibOderPrgImportAbfrageItem eDC_BibOderPrgImportAbfrageItem = FUN_edcHoleBibOderPrgAbfrageFuerFormat((ENUM_ImportFormat)PRO_i32FormatAuswahl);
			if (eDC_BibOderPrgImportAbfrageItem.PRO_delAuswahlValidierer != null)
			{
				string[] array = eDC_BibOderPrgImportAbfrageItem.PRO_delAuswahlValidierer(PRO_blnIstBibliothekAusgewaehlt, PRO_strImportPfad);
				if (array.Any())
				{
					PRO_strValidierungsFehler = string.Format("{0}:{1}{2}", PRO_edcLokalisierungsDienst.FUN_strText("13_627"), Environment.NewLine, string.Join(Environment.NewLine, array));
					return;
				}
			}
			if (PRO_blnIstBibliothekAusgewaehlt && eDC_BibOderPrgImportAbfrageItem.PRO_delBibNameAusPfad != null)
			{
				PRO_strNeuerName = eDC_BibOderPrgImportAbfrageItem.PRO_delBibNameAusPfad(PRO_strImportPfad);
			}
			if (!PRO_blnIstBibliothekAusgewaehlt && eDC_BibOderPrgImportAbfrageItem.PRO_delPrgNameAusPfad != null)
			{
				PRO_strNeuerName = eDC_BibOderPrgImportAbfrageItem.PRO_delPrgNameAusPfad(PRO_strImportPfad);
			}
			PRO_enmSchritt = ENUM_BibOderPrgImportSchritt.EingabeImportOptionen;
		}

		private async Task FUN_fdcZuImportWechselnAsync()
		{
			PRO_strValidierungsFehler = (PRO_blnIstBibliothekAusgewaehlt ? FUN_strEingabeImportOptionenFuerBibValidieren() : FUN_strEingabeImportOptionenFuerProgrammValidieren());
			if (!string.IsNullOrEmpty(PRO_strValidierungsFehler))
			{
				return;
			}
			EDC_BibOderPrgImportAbfrageItem eDC_BibOderPrgImportAbfrageItem = FUN_edcHoleBibOderPrgAbfrageFuerFormat((ENUM_ImportFormat)PRO_i32FormatAuswahl);
			if (eDC_BibOderPrgImportAbfrageItem.PRO_delImportOptionenValidierer != null)
			{
				PRO_strValidierungsFehler = eDC_BibOderPrgImportAbfrageItem.PRO_delImportOptionenValidierer(PRO_blnIstBibliothekAusgewaehlt, PRO_edcBibAuswahl, PRO_strNeuerName);
				if (!string.IsNullOrEmpty(PRO_strValidierungsFehler))
				{
					return;
				}
			}
			PRO_enmSchritt = ENUM_BibOderPrgImportSchritt.Import;
			await FUN_fdcImportDurchfuehrenAsync();
		}

		private string FUN_strAuswahlImportPfadValidieren()
		{
			if (!string.IsNullOrEmpty(PRO_strImportPfad))
			{
				return string.Empty;
			}
			return PRO_edcLokalisierungsDienst.FUN_strText("13_635");
		}

		private string FUN_strEingabeImportOptionenFuerBibValidieren()
		{
			if (!string.IsNullOrEmpty(PRO_strNeuerName))
			{
				return string.Empty;
			}
			return PRO_edcLokalisierungsDienst.FUN_strText("13_636");
		}

		private string FUN_strEingabeImportOptionenFuerProgrammValidieren()
		{
			if (!string.IsNullOrEmpty(PRO_strNeuerName))
			{
				if (PRO_edcBibAuswahl != null)
				{
					return string.Empty;
				}
				return PRO_edcLokalisierungsDienst.FUN_strText("4_11804");
			}
			return PRO_edcLokalisierungsDienst.FUN_strText("13_235");
		}

		private async Task FUN_fdcImportDurchfuehrenAsync()
		{
			PRO_blnIstBusy = true;
			try
			{
				EDC_BibOderPrgImportAbfrageItem edcAbfrageItem = FUN_edcHoleBibOderPrgAbfrageFuerFormat((ENUM_ImportFormat)PRO_i32FormatAuswahl);
				string text;
				if (PRO_blnIstBibliothekAusgewaehlt)
				{
					string[] array = await edcAbfrageItem.PRO_delBibImport(PRO_strImportPfad, PRO_strNeuerName);
					text = (array.Any() ? string.Join(Environment.NewLine, array) : string.Empty);
				}
				else
				{
					text = await edcAbfrageItem.PRO_delPrgImport(PRO_strImportPfad, PRO_edcBibAuswahl, PRO_strNeuerName);
				}
				if (string.IsNullOrEmpty(text))
				{
					PRO_strValidierungsFehler = string.Empty;
					PRO_strImportErgebnis = ((PRO_i32FormatAuswahl != 0) ? PRO_edcLokalisierungsDienst.FUN_strText("14_89") : (PRO_blnIstBibliothekAusgewaehlt ? string.Format("{0}{1}{1}{2}", PRO_edcLokalisierungsDienst.FUN_strText("14_89"), Environment.NewLine, PRO_edcLokalisierungsDienst.FUN_strText("13_838")) : string.Format("{0}{1}{1}{2}", PRO_edcLokalisierungsDienst.FUN_strText("14_89"), Environment.NewLine, PRO_edcLokalisierungsDienst.FUN_strText("13_837"))));
				}
				else
				{
					PRO_strValidierungsFehler = PRO_edcLokalisierungsDienst.FUN_strText("14_90");
					PRO_strImportErgebnis = text;
				}
			}
			catch (EDC_LoetprogrammImportMeldungException ex)
			{
				PRO_strImportErgebnis = PRO_edcLokalisierungsDienst.FUN_strText("14_89");
				PRO_strValidierungsFehler = ex.Message;
			}
			catch (Exception ex2)
			{
				PRO_strValidierungsFehler = PRO_edcLokalisierungsDienst.FUN_strText("14_90");
				PRO_strImportErgebnis = ex2.Message;
			}
			finally
			{
				PRO_blnIstBusy = false;
			}
		}

		private void SUB_ImportEinstellungenGeaendert()
		{
			RaisePropertyChanged("PRO_strVerzeichnisAuswahlText");
		}

		private EDC_BibOderPrgImportAbfrageItem FUN_edcHoleBibOderPrgAbfrageFuerFormat(ENUM_ImportFormat i_enmFormat)
		{
			if (m_dicAbfrageItems.TryGetValue(i_enmFormat, out EDC_BibOderPrgImportAbfrageItem value))
			{
				return value;
			}
			return null;
		}
	}
}
