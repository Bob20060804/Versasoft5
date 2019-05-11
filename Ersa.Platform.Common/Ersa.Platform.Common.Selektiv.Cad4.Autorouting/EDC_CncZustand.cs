namespace Ersa.Platform.Common.Selektiv.Cad4.Autorouting
{
	public class EDC_CncZustand
	{
		private int m_i32SchrittNummer;

		private int m_i32AktivesWerkzeug;

		private double m_dblPosX;

		private double m_dblPosY;

		private double m_dblPosZ;

		private double m_dblWellenHoehe;

		private double m_dblFluxerDosis;

		public int PRO_i32SchrittNummer
		{
			get
			{
				return m_i32SchrittNummer;
			}
			set
			{
				m_i32SchrittNummer = value;
			}
		}

		public int PRO_i32AktivesWerkzeug
		{
			get
			{
				return m_i32AktivesWerkzeug;
			}
			set
			{
				m_i32AktivesWerkzeug = value;
			}
		}

		public double PRO_dblPosX
		{
			get
			{
				return m_dblPosX;
			}
			set
			{
				m_dblPosX = value;
			}
		}

		public double PRO_dblPosY
		{
			get
			{
				return m_dblPosY;
			}
			set
			{
				m_dblPosY = value;
			}
		}

		public double PRO_dblPosZ
		{
			get
			{
				return m_dblPosZ;
			}
			set
			{
				m_dblPosZ = value;
			}
		}

		public double PRO_dblWellenHoehe
		{
			get
			{
				return m_dblWellenHoehe;
			}
			set
			{
				m_dblWellenHoehe = value;
			}
		}

		public double PRO_dblFluxerDosis
		{
			get
			{
				return m_dblFluxerDosis;
			}
			set
			{
				m_dblFluxerDosis = value;
			}
		}

		public int PRO_i32AnzahlMBefehle
		{
			get;
			set;
		}

		public EDC_CncZustand()
		{
			m_i32SchrittNummer = 1001;
			m_dblPosX = 0.0;
			m_dblPosY = 0.0;
			m_dblPosZ = 0.0;
			m_dblWellenHoehe = 0.0;
			m_i32AktivesWerkzeug = 0;
			m_dblFluxerDosis = 0.0;
		}
	}
}
