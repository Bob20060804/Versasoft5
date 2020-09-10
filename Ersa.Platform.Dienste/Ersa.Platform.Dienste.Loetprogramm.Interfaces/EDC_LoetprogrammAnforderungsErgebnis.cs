namespace Ersa.Platform.Dienste.Loetprogramm.Interfaces
{
	public class EDC_LoetprogrammAnforderungsErgebnis<TLoetprogramm> where TLoetprogramm : class
	{
		public ENUM_LpAnforderungsErgebnis PRO_enmAnforderungsErgebnis
		{
			get;
			private set;
		}

		public TLoetprogramm PRO_edcLoetprogramm
		{
			get;
			private set;
		}

		public EDC_LoetprogrammAnforderungsErgebnis(ENUM_LpAnforderungsErgebnis i_enmAnforderungsErgebnis, TLoetprogramm i_edcLoetprogramm = null)
		{
			PRO_enmAnforderungsErgebnis = i_enmAnforderungsErgebnis;
			PRO_edcLoetprogramm = i_edcLoetprogramm;
		}
	}
}
