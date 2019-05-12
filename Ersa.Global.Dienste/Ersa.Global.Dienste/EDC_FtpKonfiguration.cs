namespace Ersa.Global.Dienste
{
	public class EDC_FtpKonfiguration
	{
		private bool? m_blnUsePassive;

		private bool? m_blnUseBinary;

		private bool? m_blnKeepAlive;

		public string PRO_strRemoteHost
		{
			get;
			set;
		}

		public string PRO_strRemotePartition
		{
			get;
			set;
		}

		public string PRO_strRemoteUser
		{
			get;
			set;
		}

		public string PRO_strRemotePass
		{
			get;
			set;
		}

		public bool PRO_blnUsePassive
		{
			get
			{
				return m_blnUsePassive ?? true;
			}
			set
			{
				m_blnUsePassive = value;
			}
		}

		public bool PRO_blnUseBinary
		{
			get
			{
				return m_blnUseBinary ?? true;
			}
			set
			{
				m_blnUseBinary = value;
			}
		}

		public bool PRO_blnKeepAlive
		{
			get
			{
				return m_blnKeepAlive ?? false;
			}
			set
			{
				m_blnKeepAlive = value;
			}
		}
	}
}
