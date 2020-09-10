namespace Ersa.Platform.Mes.Interfaces
{
	/// <summary>
	/// mes�����ӿ�
	/// </summary>
	public interface INF_MesContainer
	{
		/// <summary>
		/// ���mes����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="i_objObject"></param>
		void SUB_AddObject<T>(object i_objObject);

		/// <summary>
		/// �Ƴ�mes����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void SUB_RemoveObject<T>();

		/// <summary>
		/// ���ĳһ��mes����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T FUN_edcGetObject<T>() where T : class;
	}
}
