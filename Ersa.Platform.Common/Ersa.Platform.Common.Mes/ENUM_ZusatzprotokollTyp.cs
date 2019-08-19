using System.ComponentModel;

namespace Ersa.Platform.Common.Mes
{
	public enum ENUM_ZusatzprotokollTyp
	{
        /// <summary>
        /// 无协议
        /// </summary>
		[Description("13_672")]
		KeinProtokoll,
        /// <summary>
        /// Zvei 标准
        /// </summary>
		[Description("13_673")]
		ZveiStandard,
        /// <summary>
        /// Zvei 变种
        /// </summary>
		[Description("13_749")]
		ZveiVariante
	}
}
