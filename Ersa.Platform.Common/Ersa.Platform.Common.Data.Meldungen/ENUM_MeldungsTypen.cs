using System.ComponentModel;

namespace Ersa.Platform.Common.Data.Meldungen
{
	/// <summary>
	/// Message Type
	/// </summary>
	public enum ENUM_MeldungsTypen
	{
		/// <summary>
		/// 未定义
		/// </summary>
		[Description("1_80")]
		enmUndefiniert,
		/// <summary>
		/// 错误
		/// </summary>
		[Description("6_200")]
		enmFehler,
		/// <summary>
		/// Warning
		/// </summary>
		[Description("6_201")]
		enmWarnung,
		/// <summary>
		/// Wait
		/// </summary>
		[Description("6_202")]
		enmWarte,
		/// <summary>
		/// Note
		/// </summary>
		[Description("6_204")]
		enmHinweis,
		/// <summary>
		/// Service
		/// </summary>
		[Description("6_203")]
		enmService,
		/// <summary>
		/// 循环的
		/// </summary>
		[Description("6_346")]
		enmZyklisch,
		/// <summary>
		/// danger
		/// </summary>
		[Description("6_347")]
		enmGefahr,
		/// <summary>
		/// 外部的
		/// </summary>
		[Description("6_340")]
		enmExtern,
		/// <summary>
		/// Visu
		/// </summary>
		enmVisu
	}
}
