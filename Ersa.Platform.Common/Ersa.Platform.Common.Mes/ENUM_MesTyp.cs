using System.ComponentModel;

namespace Ersa.Platform.Common.Mes
{
	/// <summary>
	/// MES¿‡–Õ(No MES,Itac,Zvei)
	/// </summary>
	public enum ENUM_MesTyp
	{
		/// <summary>
		/// No MES
		/// </summary>
		[Description("4_12177")]
		KeinMes = 0,
		/// <summary>
		/// Itac
		/// </summary>
		[Description("4_12175")]
		Itac = 1,
		/// <summary>
		/// Zvei
		/// </summary>
		[Description("4_12162")]
		Zvei = 3,
		/// <summary>
		/// Ersa
		/// </summary>
		[Description("4_12116")]
		Ersa = 4,
		/// <summary>
		/// Siemens Vienna
		/// </summary>
		[Description("4_12176")]
		SiemensWien = 5,
		/// <summary>
		/// Zollner
		/// </summary>
		[Description("4_12165")]
		Zollner = 6,
		/// <summary>
		/// Rs232 App
		/// </summary>
		[Description("4_11")]
		Rs232App = 7
	}
}
