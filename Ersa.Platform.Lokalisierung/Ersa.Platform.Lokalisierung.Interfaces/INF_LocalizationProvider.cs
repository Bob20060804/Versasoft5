using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Ersa.Platform.Lokalisierung.Interfaces
{
	public interface INF_LocalizationProvider
	{
		Task FUN_fdcRegistriereZusatzSprachenResourcenAsync(string i_strZusatzDatei);

		Task FUN_fdcInitialisiereSprachDictionaryAsync(string i_strDatei);

		Dictionary<CultureInfo, Dictionary<string, string>> FUN_dicHoleDasSprachenDictionary();
	}
}
