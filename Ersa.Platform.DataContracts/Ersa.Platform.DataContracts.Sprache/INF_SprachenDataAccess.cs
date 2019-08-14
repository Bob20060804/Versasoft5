using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Sprache
{
	public interface INF_SprachenDataAccess : INF_DataAccess
	{
		Task FUN_fdcLadeSprachenXmlInDatenbankAsync(Dictionary<CultureInfo, Dictionary<string, string>> i_dicSprachenPool);
	}
}
