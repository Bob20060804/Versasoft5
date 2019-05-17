using Ersa.Platform.Common.LeseSchreibGeraete.Ruestkontrolle;

namespace Ersa.Platform.UI.Codeleser
{
	public class EDC_RuestkontrolleLeser : EDC_CodeleserBasis
	{
		private string m_strGeleseneDaten;

		public ENUM_RuestWerkzeug PRO_enmRuestWerkzeug
		{
			get;
		}

		public string PRO_strGeleseneDaten
		{
			get
			{
				return m_strGeleseneDaten;
			}
			set
			{
				SetProperty(ref m_strGeleseneDaten, value, "PRO_strGeleseneDaten");
			}
		}

		public EDC_RuestkontrolleLeser(long i_i64ArrayIndex, ENUM_RuestWerkzeug i_enmRuestWerkzeug, string i_strBezeichnung)
			: base(i_i64ArrayIndex, i_strBezeichnung)
		{
			PRO_enmRuestWerkzeug = i_enmRuestWerkzeug;
		}
	}
}
