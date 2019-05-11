using System.Collections.Generic;

namespace Ersa.Platform.Common.Data.Benutzer
{
	public class EDC_ExternerBenutzer
	{
		public string PRO_strBenutzername
		{
			get;
			set;
		}

		public string PRO_strBarcode
		{
			get;
			set;
		}

		public string PRO_strPasswort
		{
			get;
			set;
		}

		public int PRO_i32Rechte
		{
			get;
			set;
		}

		public bool PRO_blnIstAktiv
		{
			get;
			set;
		}

		public override bool Equals(object i_edcOther)
		{
			EDC_ExternerBenutzer eDC_ExternerBenutzer = i_edcOther as EDC_ExternerBenutzer;
			if (eDC_ExternerBenutzer == null)
			{
				return false;
			}
			if (PRO_strBarcode == eDC_ExternerBenutzer.PRO_strBarcode && PRO_strBenutzername == eDC_ExternerBenutzer.PRO_strBenutzername && PRO_strPasswort == eDC_ExternerBenutzer.PRO_strPasswort)
			{
				return PRO_i32Rechte == eDC_ExternerBenutzer.PRO_i32Rechte;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (1298990319 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PRO_strBenutzername)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PRO_strPasswort);
		}
	}
}
