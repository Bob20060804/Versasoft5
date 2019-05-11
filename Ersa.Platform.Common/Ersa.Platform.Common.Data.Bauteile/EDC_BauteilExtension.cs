namespace Ersa.Platform.Common.Data.Bauteile
{
	public static class EDC_BauteilExtension
	{
		public static string FUN_strBauteilIdErstellen(this EDC_BauteilData i_edcBauteil)
		{
			return $"{i_edcBauteil.PRO_strPackageName}_{FUN_strErstelleNummerischenTeil(i_edcBauteil.PRO_sngLaenge)}.{FUN_strErstelleNummerischenTeil(i_edcBauteil.PRO_sngBreite)}.{FUN_strErstelleNummerischenTeil(i_edcBauteil.PRO_sngHoehe)}.{FUN_strErstelleNummerischenTeil(i_edcBauteil.PRO_sngPitch)}.{i_edcBauteil.PRO_i32AnzahlPins}";
		}

		private static string FUN_strErstelleNummerischenTeil(float i_sngZahl)
		{
			return i_sngZahl.ToString("000.00").Replace(".", string.Empty).Replace(",", string.Empty);
		}
	}
}
