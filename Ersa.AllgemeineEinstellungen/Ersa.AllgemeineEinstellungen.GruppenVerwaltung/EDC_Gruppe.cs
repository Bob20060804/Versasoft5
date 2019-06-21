using Ersa.Global.Mvvm;

namespace Ersa.AllgemeineEinstellungen.GruppenVerwaltung
{
	public class EDC_Gruppe : BindableBase
	{
		private string m_strOriginalName;

		private string m_strName;

		private bool m_blnOriginalIstAktiv;

		private bool? m_blnIstAktiv;

		private bool m_blnIstAusgewaehlt;

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

		public bool PRO_blnIstAktiv
		{
			get
			{
				return m_blnIstAktiv ?? m_blnOriginalIstAktiv;
			}
			set
			{
				SetProperty(ref m_blnIstAktiv, value, "PRO_blnIstAktiv");
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

		public bool PRO_blnIstStandard
		{
			get;
			set;
		}

		public bool PRO_blnIstNeu => PRO_i64Id == 0;

		public bool PRO_blnHatAenderung
		{
			get
			{
				if (!(PRO_strName != m_strOriginalName))
				{
					return PRO_blnIstAktiv != m_blnOriginalIstAktiv;
				}
				return true;
			}
		}

		public EDC_Gruppe(string i_strName, bool i_blnAktiv)
		{
			m_strOriginalName = i_strName;
			m_blnOriginalIstAktiv = i_blnAktiv;
		}

		public void SUB_AenderungenUebernehmen()
		{
			m_strOriginalName = PRO_strName;
			m_strName = null;
			RaisePropertyChanged("PRO_strName");
			m_blnOriginalIstAktiv = PRO_blnIstAktiv;
			m_blnIstAktiv = null;
			RaisePropertyChanged("PRO_blnIstAktiv");
		}

		public void SUB_AenderungenVerwerfen()
		{
			m_strName = null;
			RaisePropertyChanged("PRO_strName");
			m_blnIstAktiv = null;
			RaisePropertyChanged("PRO_blnIstAktiv");
		}
	}
}
