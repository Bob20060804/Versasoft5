namespace Ersa.Platform.Mes.Interfaces
{
	/// <summary>
	/// mes容器接口
	/// </summary>
	public interface INF_MesContainer
	{
		/// <summary>
		/// 添加mes方法
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="i_objObject"></param>
		void SUB_AddObject<T>(object i_objObject);

		/// <summary>
		/// 移除mes方法
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void SUB_RemoveObject<T>();

		/// <summary>
		/// 获得某一个mes方法
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T FUN_edcGetObject<T>() where T : class;
	}
}
