namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MesContainer
	{
		void SUB_AddObject<T>(object i_objObject);

		void SUB_RemoveObject<T>();

		T FUN_edcGetObject<T>() where T : class;
	}
}
