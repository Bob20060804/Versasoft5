using System;

namespace Ersa.Platform.Infrastructure.Validierung
{
	public class EDC_PropertyValidierungsRegel
	{
		private readonly string m_strNameKey;

		private readonly Func<string> m_delValidierungsAktion;

		private readonly Func<bool> m_delPruefbedingung;

		public string PRO_strPropertyName
		{
			get;
			set;
		}

		public EDC_PropertyValidierungsRegel(string i_strNameKey, Func<string> i_delValidierungsAktion, Func<bool> i_delPruefbedingung)
		{
			m_strNameKey = i_strNameKey;
			m_delValidierungsAktion = i_delValidierungsAktion;
			m_delPruefbedingung = i_delPruefbedingung;
		}

		public EDC_PropertyValidierungsRegel(string i_strNameKey, Func<string> i_delValidierungsAktion)
			: this(i_strNameKey, i_delValidierungsAktion, () => true)
		{
		}

		public EDC_PropertyValidierungsFehler FUN_edcValidieren()
		{
			if (!m_delPruefbedingung())
			{
				return null;
			}
			string text = m_delValidierungsAktion();
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return new EDC_PropertyValidierungsFehler(PRO_strPropertyName, m_strNameKey, text);
		}
	}
}
