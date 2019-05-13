using System.Globalization;

namespace Ersa.Platform.Infrastructure.Sprache
{
	public class EDC_Sprache : EDC_NotificationObjectMitSprachUmschaltung
	{
		public string PRO_strText
		{
			get;
			set;
		}

		public CultureInfo PRO_fdcCultureInfo
		{
			get;
			set;
		}

		public override bool Equals(object i_objSprache)
		{
			EDC_Sprache eDC_Sprache = i_objSprache as EDC_Sprache;
			if (eDC_Sprache != null && eDC_Sprache.PRO_fdcCultureInfo != null)
			{
				return eDC_Sprache.PRO_fdcCultureInfo.Equals(PRO_fdcCultureInfo);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (PRO_fdcCultureInfo != null)
			{
				num ^= PRO_fdcCultureInfo.GetHashCode();
			}
			return num;
		}
	}
}
