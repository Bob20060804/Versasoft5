namespace Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente
{
	public class EDC_DoubleVorlageParameter : EDC_VorlageParameter
	{
		private double m_dblWert;

		private double m_dblMinWert;

		private double m_dblMaxWert;

		private int m_i32Nachkommastellen;

		private string m_strEinheit;

		private string m_strOkText;

		private string m_strAbbrechenText;

		public double PRO_dblWert
		{
			get
			{
				return m_dblWert;
			}
			set
			{
				SetProperty(ref m_dblWert, value, "PRO_dblWert");
			}
		}

		public double PRO_dblMinWert
		{
			get
			{
				return m_dblMinWert;
			}
			set
			{
				SetProperty(ref m_dblMinWert, value, "PRO_dblMinWert");
			}
		}

		public double PRO_dblMaxWert
		{
			get
			{
				return m_dblMaxWert;
			}
			set
			{
				SetProperty(ref m_dblMaxWert, value, "PRO_dblMaxWert");
			}
		}

		public int PRO_i32Nachkommastellen
		{
			get
			{
				return m_i32Nachkommastellen;
			}
			set
			{
				SetProperty(ref m_i32Nachkommastellen, value, "PRO_i32Nachkommastellen");
			}
		}

		public string PRO_strEinheit
		{
			get
			{
				return m_strEinheit;
			}
			set
			{
				SetProperty(ref m_strEinheit, value, "PRO_strEinheit");
			}
		}

		public string PRO_strOkText
		{
			get
			{
				return m_strOkText;
			}
			set
			{
				SetProperty(ref m_strOkText, value, "PRO_strOkText");
			}
		}

		public string PRO_strAbbrechenText
		{
			get
			{
				return m_strAbbrechenText;
			}
			set
			{
				SetProperty(ref m_strAbbrechenText, value, "PRO_strAbbrechenText");
			}
		}
	}
}
