using Ersa.Global.Mvvm;

namespace Ersa.AllgemeineEinstellungen.BetriebsmittelVerwaltung
{
	public class EDC_Flussmittel : BindableBase
	{
		private string m_strOriginalName;

		private string m_strName;

		private string m_strOriginalSpezifikation;

		private string m_strSpezifikation;

		private bool m_blnGeloescht;

		public long PRO_i64Id
		{
			get;
			set;
		}

		public string PRO_strName
		{
			get
			{
				return m_strName ?? m_strOriginalName;
			}
			set
			{
				SetProperty(ref m_strName, value, "PRO_strName");
			}
		}

		public string PRO_strSpezifikation
		{
			get
			{
				return m_strSpezifikation ?? m_strOriginalSpezifikation;
			}
			set
			{
				SetProperty(ref m_strSpezifikation, value, "PRO_strSpezifikation");
			}
		}

		public bool PRO_blnIstNeu => PRO_i64Id == 0;

		public bool PRO_blnIstGeloescht
		{
			get
			{
				return m_blnGeloescht;
			}
			set
			{
				SetProperty(ref m_blnGeloescht, value, "PRO_blnIstGeloescht");
			}
		}

		public bool PRO_blnHatAenderung
		{
			get
			{
				if (PRO_strName != m_strOriginalName || PRO_strSpezifikation != m_strOriginalSpezifikation)
				{
					return !PRO_blnIstNeu;
				}
				return false;
			}
		}

		public EDC_Flussmittel(string i_strName, string i_strSpezifikation)
		{
			m_strOriginalName = i_strName;
			m_strOriginalSpezifikation = i_strSpezifikation;
		}

		public EDC_Flussmittel()
		{
		}

		public void SUB_AenderungenUebernehmen()
		{
			m_strOriginalName = PRO_strName;
			m_strName = null;
			RaisePropertyChanged("PRO_strName");
			m_strOriginalSpezifikation = PRO_strSpezifikation;
			m_strSpezifikation = null;
			RaisePropertyChanged("PRO_strSpezifikation");
		}

		public void SUB_AenderungenVerwerfen()
		{
			m_strName = null;
			RaisePropertyChanged("PRO_strName");
			m_strSpezifikation = null;
			RaisePropertyChanged("PRO_strSpezifikation");
			m_blnGeloescht = false;
			RaisePropertyChanged("PRO_blnIstGeloescht");
		}
	}
}
