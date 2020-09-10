using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.Mes.Modell
{
	/// <summary>
	/// MES״̬
	/// Mes Session
	/// </summary>
	[Serializable]
	public class EDC_MesSession : BindableBase
	{
		private string m_strStationNumber;

		private string m_strStationPassword;

		private string m_strUser;

		private string m_strPassword;

		private string m_strClient;

		private string m_strRegistrationType;

		private string m_strSystemIdentifier;

		public string PRO_strStationNumber
		{
			get
			{
				return m_strStationNumber;
			}
			set
			{
				SetProperty(ref m_strStationNumber, value, "PRO_strStationNumber");
			}
		}

		public string PRO_strStationPassword
		{
			get
			{
				return m_strStationPassword;
			}
			set
			{
				SetProperty(ref m_strStationPassword, value, "PRO_strStationPassword");
			}
		}

		public string PRO_strUser
		{
			get
			{
				return m_strUser;
			}
			set
			{
				SetProperty(ref m_strUser, value, "PRO_strUser");
			}
		}

		public string PRO_strPassword
		{
			get
			{
				return m_strPassword;
			}
			set
			{
				SetProperty(ref m_strPassword, value, "PRO_strPassword");
			}
		}

		public string PRO_strClient
		{
			get
			{
				return m_strClient;
			}
			set
			{
				SetProperty(ref m_strClient, value, "PRO_strClient");
			}
		}

		public string PRO_strRegistrationType
		{
			get
			{
				return m_strRegistrationType;
			}
			set
			{
				SetProperty(ref m_strRegistrationType, value, "PRO_strRegistrationType");
			}
		}

		public string PRO_strSystemIdentifier
		{
			get
			{
				return m_strSystemIdentifier;
			}
			set
			{
				SetProperty(ref m_strSystemIdentifier, value, "PRO_strSystemIdentifier");
			}
		}

		public int PRO_i32MaxReconnectAttempt
		{
			get;
			set;
		}

		public int PRO_i32Layer
		{
			get;
			set;
		}
	}
}
