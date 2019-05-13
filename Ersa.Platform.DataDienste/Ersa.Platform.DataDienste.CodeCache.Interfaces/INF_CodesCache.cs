using Ersa.Platform.Common.Data.CodeCache;
using Ersa.Platform.Common.LeseSchreibGeraete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.CodeCache.Interfaces
{
	public interface INF_CodesCache
	{
		string FUN_strEintragErzeugen(string i_strKey, string i_strSalt);

		Task FUN_fdcCodesZumEintragHinzufuegenAsync(string i_strHash, long i_i64ArrayIndex, string i_strCodeVerwendung, string i_strCodeBedeutung, List<string> i_lstCodes);

		Task FUN_fdcCodesHinzufuegenAsync(string i_strHash, long i_i64ArrayIndex, IEnumerable<EDC_CodeMitVerwendungUndBedeutung> i_lstCodeMitVerwendungUndBedeutung);

		Task<IEnumerable<EDC_CodeMitVerwendungUndBedeutung>> FUN_fdcCodesErmittelnFuerVerwendungAsync(string i_strHash, string i_strCodeverwendung);

		Task<IEnumerable<EDC_CodeMitVerwendungUndBedeutung>> FUN_fdcCodesErmittelnFuerBedeutungAsync(string i_strHash, string i_strCodebedeutung);

		Task<IEnumerable<EDC_CodeMitVerwendungUndBedeutung>> FUN_fdcCodesErmittelnFuerVerwendungUndBedeutungAsync(string i_strHash, string i_strCodeverwendung, string i_strCodebedeutung);

		Task<IEnumerable<EDC_CodeMitVerwendungUndBedeutung>> FUN_fdcAlleCodesErmittelnAsync(string i_strHash);

		Task<IDictionary<string, List<string>>> FUN_fdcCodesFuerProtokollErmittelnAsync(string i_strHash);

		Task FUN_fdcLinksHinzufuegenAsync(string i_strHash, IEnumerable<string> i_enuLinks, ENUM_LinkVerwendung i_enmLinkVerwendung);

		Task<IEnumerable<string>> FUN_fdcLinksFuerHashUndVerwendungErmittelnAsync(string i_strHash, ENUM_LinkVerwendung i_enmLinkVerwendung);

		Task<IEnumerable<string>> FUN_fdcLinksFuerHashErmittelnAsync(string i_strHash);
	}
}
