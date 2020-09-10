using System.ComponentModel;

namespace Ersa.Platform.Common.Data.Meldungen
{
	/// <summary>
	/// 消息产生者
	/// Message producer
	/// </summary>
	public enum ENUM_MeldungProduzent
	{
		/// <summary>
		/// ALL
		/// </summary>
		[Description("1_80")]
		Alle,
		/// <summary>
		/// Machine
		/// </summary>
		[Description("4_501")]
		Maschine,
		/// <summary>
		/// MES
		/// </summary>
		[Description("4_529")]
		Mes,
		/// <summary>
		/// 图像处理
		/// Image processing
		/// </summary>
		[Description("13_1009")]
		Bildeverarbeitung,
		/// <summary>
		/// Versascan
		/// </summary>
		[Description("13_1010")]
		Versascan,
		/// <summary>
		/// Versaeye
		/// </summary>
		[Description("13_1010")]
		Versaeye
	}
}
