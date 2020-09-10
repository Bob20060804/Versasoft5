namespace Ersa.Platform.Common.Data.Meldungen
{
	/// <summary>
	/// 流程动作
	/// </summary>
	public enum ENUM_ProzessAktionen
	{
		/// <summary>
		/// 锁定
		/// </summary>
		Einlausperre,
		/// <summary>
		/// 广播
		/// </summary>
		Hupe,
		/// <summary>
		/// 弹出
		/// </summary>
		Popup,
		/// <summary>
		/// 信号灯危险
		/// </summary>
		AmpelGefahr,
		/// <summary>
		/// 信号灯服务
		/// </summary>
		AmpelService,
		/// <summary>
		/// 信号灯中断
		/// </summary>
		AmpelStoerung,
		/// <summary>
		/// 信号灯警告
		/// </summary>
		AmpelWarnung,
		/// <summary>
		/// 信号灯周期性
		/// </summary>
		AmpelZyklisch
	}
}
