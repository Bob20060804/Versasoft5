using Newtonsoft.Json;

namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_JsonSerialisierungsDienst : INF_SerialisierungsDienst
	{
		T FUN_objDeserialisieren<T>(string i_strFormatierterString, JsonSerializerSettings i_fdcSettings);

		string FUN_strSerialisierenMitTypInformationen<T>(T i_objObjekt);

		T FUN_objDeserialisierenMitTypInformationen<T>(string i_strFormatierterString);
	}
}
