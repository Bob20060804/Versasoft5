namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public class EDC_ProduktionsArtMitPrgId : EDC_ProduktionsArten
	{
		private long? m_blnAusgewaehlteId;

		public long PRO_i64GespeicherteId
		{
			get;
			set;
		}

		public long PRO_i64AusgewaehlteId
		{
			get
			{
				return m_blnAusgewaehlteId ?? PRO_i64GespeicherteId;
			}
			set
			{
				m_blnAusgewaehlteId = value;
			}
		}

		public bool PRO_blnPrgIdHatAenderung => PRO_i64GespeicherteId != PRO_i64AusgewaehlteId;

		public EDC_ProduktionsArtMitPrgId(string i_strLocKey, params EDC_ProduktionsUnterart[] ia_edcUnterarten)
			: base(i_strLocKey, ia_edcUnterarten)
		{
		}

		public void SUB_PrgIdAenderungUebernehmen()
		{
			if (m_blnAusgewaehlteId.HasValue)
			{
				PRO_i64GespeicherteId = m_blnAusgewaehlteId.Value;
				m_blnAusgewaehlteId = null;
			}
		}

		public void SUB_PrgIdAenderungVerwerfen()
		{
			m_blnAusgewaehlteId = null;
		}
	}
}
