namespace Ersa.Platform.Mes.Interfaces
{
	/// <summary>
	/// 方法出口
	/// </summary>
	public interface INF_FunctionExit
	{
		/// <summary>
		/// 执行方法
		/// </summary>
		/// <param name="i_strFunctionName"></param>
		/// <param name="i_strMessage"></param>
		void SUB_FunktionAusfuehren(string i_strFunctionName, ref string i_strMessage);
	}
}
