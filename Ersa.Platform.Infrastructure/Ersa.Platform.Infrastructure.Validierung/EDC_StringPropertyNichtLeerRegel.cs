using System;

namespace Ersa.Platform.Infrastructure.Validierung
{
	public class EDC_StringPropertyNichtLeerRegel : EDC_PropertyValidierungsRegel
	{
		public EDC_StringPropertyNichtLeerRegel(string i_strNameKey, Func<string> i_delPropertyErmittler, bool i_blnLeerzeichenGueltig = false)
			: base(i_strNameKey, FUN_delValidierungsAktionErstellen(i_delPropertyErmittler, i_blnLeerzeichenGueltig))
		{
		}

		public EDC_StringPropertyNichtLeerRegel(string i_strNameKey, Func<string> i_delPropertyErmittler, Func<bool> i_delPruefbedingung, bool i_blnLeerzeichenGueltig = false)
			: base(i_strNameKey, FUN_delValidierungsAktionErstellen(i_delPropertyErmittler, i_blnLeerzeichenGueltig), i_delPruefbedingung)
		{
		}

		private static Func<string> FUN_delValidierungsAktionErstellen(Func<string> i_delPropertyErmittler, bool i_blnLeerzeichenGueltig)
		{
			return delegate
			{
				if (!(i_blnLeerzeichenGueltig ? string.IsNullOrEmpty(i_delPropertyErmittler()) : string.IsNullOrWhiteSpace(i_delPropertyErmittler())))
				{
					return string.Empty;
				}
				return "13_364";
			};
		}
	}
}
