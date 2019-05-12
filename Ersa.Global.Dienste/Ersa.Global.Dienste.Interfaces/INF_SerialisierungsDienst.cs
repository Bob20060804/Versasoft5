namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_SerialisierungsDienst
	{
		string FUN_strSerialisieren<T>(T i_objObjekt);

		void SUB_ValidiereGegenSchemaDatei(string i_strInhalt, string i_strSchemaDatei);

		byte[] FUNa_bytSerialisieren<T>(T i_objObjekt);

		T FUN_objDeserialisieren<T>(string i_strFormatierterString);

		T FUN_objDeserialisieren<T>(byte[] ia_bytInhalt);
	}
}
