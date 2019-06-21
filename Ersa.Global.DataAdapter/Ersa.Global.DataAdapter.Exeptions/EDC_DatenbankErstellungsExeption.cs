using System;

namespace Ersa.Global.DataAdapter.Exeptions
{
	public class EDC_DatenbankErstellungsExeption : Exception
	{
		public EDC_DatenbankErstellungsExeption()
		{
		}

		public EDC_DatenbankErstellungsExeption(string i_strMeldung)
			: base(i_strMeldung)
		{
		}

		public EDC_DatenbankErstellungsExeption(string i_strMeldung, Exception i_fdcInnerExeption)
			: base(i_strMeldung, i_fdcInnerExeption)
		{
		}
	}
}
