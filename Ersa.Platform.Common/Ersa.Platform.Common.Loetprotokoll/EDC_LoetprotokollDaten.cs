using System.Collections.Generic;

namespace Ersa.Platform.Common.Loetprotokoll
{
	public class EDC_LoetprotokollDaten
	{
		public EDC_LoetprotokollKopfElemente PRO_edcKopf
		{
			get;
			set;
		}

		public List<EDC_LoetprotokollElement> PRO_lstModulElemente
		{
			get;
			set;
		}

		public void SUB_Init()
		{
			PRO_edcKopf = new EDC_LoetprotokollKopfElemente();
			PRO_edcKopf.SUB_Init();
			PRO_lstModulElemente = new List<EDC_LoetprotokollElement>();
		}
	}
}
