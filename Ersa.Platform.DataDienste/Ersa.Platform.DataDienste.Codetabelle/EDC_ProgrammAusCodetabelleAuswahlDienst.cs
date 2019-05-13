using Ersa.Global.Common.Helper;
using Ersa.Platform.Common.Codetabellen;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.DataDienste.Codetabelle.Interfaces;
using Ersa.Platform.DataDienste.Loetprogramm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Codetabelle
{
	[Export(typeof(INF_ProgrammAusCodetabelleAuswahlDienst))]
	public class EDC_ProgrammAusCodetabelleAuswahlDienst : INF_ProgrammAusCodetabelleAuswahlDienst
	{
		[Import]
		public INF_CodetabellenVerwaltungsDienst PRO_edcCodetabellenVerwaltungsDienst
		{
			get;
			set;
		}

		[Import]
		public INF_LoetprogrammVerwaltungsDienst PRO_edcLoetprogrammVerwaltungsDienst
		{
			get;
			set;
		}

		public async Task<IEnumerable<long>> FUN_fdcErmittleProgrammIdsFuerCodeAsync(string i_strSuchCode)
		{
			if (string.IsNullOrEmpty(i_strSuchCode))
			{
				throw new ArgumentNullException(i_strSuchCode, "invalid Code");
			}
			EDC_Codetabelle edcAktiveCodetabelle = await PRO_edcCodetabellenVerwaltungsDienst.FUN_edcAktiveCodetabelleErmittelnAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (edcAktiveCodetabelle == null)
			{
				throw new ArgumentNullException(i_strSuchCode, "no active code table");
			}
			_003C_003Ec__DisplayClass8_0 _003C_003Ec__DisplayClass8_;
			IEnumerable<EDC_BibliothekInfo> enuBib = _003C_003Ec__DisplayClass8_.enuBibs;
			IEnumerable<EDC_BibliothekInfo> enuBibs = await PRO_edcLoetprogrammVerwaltungsDienst.FUN_fdcBibliothekenAuslesenAsync().ConfigureAwait(continueOnCapturedContext: false);
			return FUN_enuErmittleGueltigeCodes(i_strSuchCode, edcAktiveCodetabelle.PRO_lstEintraege).SelectMany((EDC_Codeeintrag i_edcCodeEintrag) => from _003C_003Eh__TransparentIdentifier0 in (from edcBibinfo in enuBibs
			where edcBibinfo.PRO_strBibliotheksName.Equals(i_edcCodeEintrag.PRO_strBibliothek)
			from edcProgrammInfo in edcBibinfo.PRO_lstProgramme
			select new
			{
				edcBibinfo,
				edcProgrammInfo
			}).Where(_003C_003Eh__TransparentIdentifier0 =>
			{
				if (_003C_003Eh__TransparentIdentifier0.edcProgrammInfo.PRO_strProgrammName.Equals(i_edcCodeEintrag.PRO_strProgramm))
				{
					return !_003C_003Eh__TransparentIdentifier0.edcProgrammInfo.PRO_blnIstFehlerhaft;
				}
				return false;
			})
			select _003C_003Eh__TransparentIdentifier0.edcProgrammInfo.PRO_i64Id);
		}

		private IEnumerable<EDC_Codeeintrag> FUN_enuErmittleGueltigeCodes(string i_strSuchCode, IEnumerable<EDC_Codeeintrag> i_edcCodeEintraege)
		{
			HashSet<EDC_Codeeintrag> hashSet = new HashSet<EDC_Codeeintrag>();
			foreach (EDC_Codeeintrag item in i_edcCodeEintraege)
			{
				if (EDC_PatternVergleichsHelfer.FUN_blnStimmtDerCodeUeberein(item.PRO_strCode, i_strSuchCode))
				{
					hashSet.Add(item);
				}
			}
			return hashSet;
		}
	}
}
