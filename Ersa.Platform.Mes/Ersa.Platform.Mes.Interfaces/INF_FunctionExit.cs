namespace Ersa.Platform.Mes.Interfaces
{
	/// <summary>
	/// ��������
	/// </summary>
	public interface INF_FunctionExit
	{
		/// <summary>
		/// ִ�з���
		/// </summary>
		/// <param name="i_strFunctionName"></param>
		/// <param name="i_strMessage"></param>
		void SUB_FunktionAusfuehren(string i_strFunctionName, ref string i_strMessage);
	}
}
