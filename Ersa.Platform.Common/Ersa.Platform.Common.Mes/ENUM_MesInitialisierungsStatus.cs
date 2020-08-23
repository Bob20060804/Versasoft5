using System.ComponentModel;

namespace Ersa.Platform.Common.Mes
{
	/// <summary>
	/// MES初始化状态
	/// </summary>
	public enum ENUM_MesInitialisierungsStatus
	{
		/// <summary>
		/// Successfully
		/// </summary>
		[Description("successful")]
		Erfolgreich,
		/// <summary>
		/// 已停用
		/// Mes Deactivated
		/// </summary>
		[Description("MesDisabled")]
		MesDeaktiviert,
		/// <summary>
		/// 未配置
		/// Mes Not Configured
		/// </summary>
		[Description("MesNotConfigured")]
		MesNichtKonfiguriert,
		/// <summary>
		/// 有问题
		/// </summary>
		[Description("Faulty")]
		Fehlerhaft
	}
}
