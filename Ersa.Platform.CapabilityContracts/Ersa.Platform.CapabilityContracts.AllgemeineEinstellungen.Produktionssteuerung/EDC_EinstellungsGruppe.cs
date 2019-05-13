using Ersa.Platform.Common.Model;
using System.Collections.Generic;

namespace Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.Produktionssteuerung
{
	public class EDC_EinstellungsGruppe
	{
		private readonly string m_strNameKey;

		private readonly IEnumerable<EDC_BooleanParameter> m_enuEinstellungen;

		public string PRO_strNameKey => m_strNameKey;

		public IEnumerable<EDC_BooleanParameter> PRO_edcEinstellungen => m_enuEinstellungen;

		public EDC_EinstellungsGruppe(string i_strNameKey, IEnumerable<EDC_BooleanParameter> i_enuEinstellungen)
		{
			m_strNameKey = i_strNameKey;
			m_enuEinstellungen = i_enuEinstellungen;
		}
	}
}
