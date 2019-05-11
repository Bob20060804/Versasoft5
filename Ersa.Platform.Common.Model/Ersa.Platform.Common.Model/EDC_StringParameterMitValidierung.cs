using System;
using System.ComponentModel;

namespace Ersa.Platform.Common.Model
{
	public class EDC_StringParameterMitValidierung : EDC_StringParameter, IDataErrorInfo
	{
		private readonly Func<string, bool> m_delValidierung;

		public string Error => string.Empty;

		public bool PRO_blnIsValide => string.IsNullOrEmpty(this["PRO_strAnzeigeWert"]);

		public string this[string i_strPropertyName]
		{
			get
			{
				if (i_strPropertyName == "PRO_strAnzeigeWert")
				{
					if (!m_delValidierung(base.PRO_strAnzeigeWert))
					{
						return "10_10";
					}
					return string.Empty;
				}
				if (i_strPropertyName == "PRO_strWert")
				{
					if (!m_delValidierung(base.PRO_strWert))
					{
						return "10_10";
					}
					return string.Empty;
				}
				return string.Empty;
			}
		}

		public EDC_StringParameterMitValidierung(Func<string, bool> i_delValidierung)
		{
			m_delValidierung = i_delValidierung;
		}
	}
}
