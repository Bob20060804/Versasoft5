using Ersa.Platform.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common
{
	public class EDC_HauptmenuEintrag : EDC_NotificationObjectMitSprachUmschaltung
	{
		private readonly EDC_HauptmenuEintragSpezifikation m_edcSpezifikation;

		private bool m_blnIstZugriffEingeschraenkt;

		private bool m_blnIstAktiviert;

		private bool m_blnIstAusgewaehlt;

		private bool m_blnIstHervorgehoben;

		public string PRO_strNameKey => m_edcSpezifikation.PRO_strNameKey;

		public Uri PRO_uriIcon => m_edcSpezifikation.PRO_uriIcon;

		public bool PRO_blnIstZugriffEingeschraenkt
		{
			get
			{
				return m_blnIstZugriffEingeschraenkt;
			}
			set
			{
				m_blnIstZugriffEingeschraenkt = value;
				RaisePropertyChanged("PRO_blnIstZugriffEingeschraenkt");
			}
		}

		public object PRO_objView => m_edcSpezifikation.PRO_objView;

		public int PRO_i32StartPrioritaet => m_edcSpezifikation.PRO_i32StartPrioritaet;

		public bool PRO_blnIstAktiviert
		{
			get
			{
				return m_blnIstAktiviert;
			}
			set
			{
				m_blnIstAktiviert = value;
				RaisePropertyChanged("PRO_blnIstAktiviert");
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

		public bool PRO_blnIstHervorgehoben
		{
			get
			{
				return m_blnIstHervorgehoben;
			}
			private set
			{
				SetProperty(ref m_blnIstHervorgehoben, value, "PRO_blnIstHervorgehoben");
			}
		}

		public string PRO_strRecht => m_edcSpezifikation.PRO_strRecht;

		public string PRO_strRechtNameKey => m_edcSpezifikation.PRO_strRechtNameKey;

		public EDC_HauptmenuEintrag(EDC_HauptmenuEintragSpezifikation i_edcSpezifikation)
		{
			m_edcSpezifikation = i_edcSpezifikation;
			m_blnIstAktiviert = i_edcSpezifikation.PRO_blnIstStandardmaessigAktiviert;
			m_blnIstZugriffEingeschraenkt = false;
		}

		public void SUB_EintragHervorheben()
		{
			if (!PRO_blnIstHervorgehoben)
			{
				PRO_blnIstHervorgehoben = true;
				Task.Delay(2000).ContinueWith(delegate
				{
					EDC_Dispatch.SUB_AktionStarten(delegate
					{
						PRO_blnIstHervorgehoben = false;
					});
				});
			}
		}
	}
}
