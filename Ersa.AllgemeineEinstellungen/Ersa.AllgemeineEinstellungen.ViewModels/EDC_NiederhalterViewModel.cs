using Ersa.Global.Mvvm;
using Ersa.Platform.Common.Data.Betriebsmittel;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public class EDC_NiederhalterViewModel : BindableBase
	{
		private string m_strOriginalIdentifikation;

		private string m_strIdentifikation;

		private bool m_blnIstAusgewaehlt;

		private bool m_blnGeloescht;

		private bool m_blnIstNeu;

		public long PRO_i64RuestwerkzeugId
		{
			get;
			set;
		}

		public long PRO_i64RuestkomponentenId
		{
			get;
			set;
		}

		public string PRO_strOriginalIdentifikation
		{
			get
			{
				return m_strOriginalIdentifikation;
			}
			set
			{
				m_strOriginalIdentifikation = value;
			}
		}

		public string PRO_strIdentifikation
		{
			get
			{
				return m_strIdentifikation ?? m_strOriginalIdentifikation;
			}
			set
			{
				if (!(PRO_strIdentifikation == value))
				{
					if (m_strOriginalIdentifikation == null)
					{
						m_strOriginalIdentifikation = value;
					}
					else
					{
						m_strIdentifikation = value;
					}
					RaisePropertyChanged("PRO_strIdentifikation");
				}
			}
		}

		public bool PRO_blnIstAusgewaehlt
		{
			get
			{
				return m_blnIstAusgewaehlt;
			}
			set
			{
				SetProperty(ref m_blnIstAusgewaehlt, value, "PRO_blnIstAusgewaehlt");
			}
		}

		public bool PRO_blnGeloescht
		{
			get
			{
				return m_blnGeloescht;
			}
			set
			{
				if (SetProperty(ref m_blnGeloescht, value, "PRO_blnGeloescht"))
				{
					RaisePropertyChanged("PRO_blnHatAenderung");
				}
			}
		}

		public bool PRO_blnHatAenderung
		{
			get
			{
				if (!PRO_blnGeloescht && !PRO_blnIstNeu)
				{
					if (!string.IsNullOrEmpty(m_strIdentifikation))
					{
						return !m_strIdentifikation.Equals(m_strOriginalIdentifikation);
					}
					return false;
				}
				return true;
			}
		}

		public bool PRO_blnIstNeu
		{
			get
			{
				return m_blnIstNeu;
			}
			set
			{
				m_blnIstNeu = value;
				RaisePropertyChanged("PRO_blnIstNeu");
				RaisePropertyChanged("PRO_blnHatAenderung");
			}
		}

		public EDC_RuestwerkzeugeData FUN_edcWerkzeugeEintragHolen()
		{
			return new EDC_RuestwerkzeugeData
			{
				PRO_i64RuestwerkzeugId = PRO_i64RuestwerkzeugId,
				PRO_i64RuestkomponentenId = PRO_i64RuestkomponentenId,
				PRO_strIdentifikation = PRO_strIdentifikation
			};
		}

		public void SUB_WerkzeugEintragSetzen(EDC_RuestwerkzeugeData i_edcWerkzeugeintrag)
		{
			PRO_i64RuestwerkzeugId = i_edcWerkzeugeintrag.PRO_i64RuestwerkzeugId;
			PRO_i64RuestkomponentenId = i_edcWerkzeugeintrag.PRO_i64RuestkomponentenId;
			PRO_strIdentifikation = i_edcWerkzeugeintrag.PRO_strIdentifikation;
		}

		public void SUB_AenderungenUebernehmen()
		{
			if (!string.IsNullOrEmpty(m_strIdentifikation))
			{
				m_strOriginalIdentifikation = m_strIdentifikation;
				m_strIdentifikation = null;
			}
			PRO_blnIstNeu = false;
		}

		public void SUB_AenderungenVerwerfen()
		{
			m_strIdentifikation = null;
			PRO_blnGeloescht = false;
			PRO_blnIstNeu = false;
		}
	}
}
