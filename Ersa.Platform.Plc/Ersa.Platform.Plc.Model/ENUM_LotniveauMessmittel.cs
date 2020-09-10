using System.ComponentModel;

namespace Ersa.Platform.Plc.Model
{
	public enum ENUM_LotniveauMessmittel
	{
		/// <summary>
		/// 没有
		/// </summary>
		[Description("8_2401")]
		Keine,
		/// <summary>
		/// 最小数字
		/// </summary>
		[Description("18_268")]
		DigitalMin,
		/// <summary>
		/// 数字最小最大
		/// </summary>
		[Description("18_269")]
		DigitalMinMax,
		/// <summary>
		/// 类似
		/// </summary>
		[Description("18_267")]
		Analog
	}
}
