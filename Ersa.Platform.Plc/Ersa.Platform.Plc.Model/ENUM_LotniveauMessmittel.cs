using System.ComponentModel;

namespace Ersa.Platform.Plc.Model
{
	public enum ENUM_LotniveauMessmittel
	{
		/// <summary>
		/// û��
		/// </summary>
		[Description("8_2401")]
		Keine,
		/// <summary>
		/// ��С����
		/// </summary>
		[Description("18_268")]
		DigitalMin,
		/// <summary>
		/// ������С���
		/// </summary>
		[Description("18_269")]
		DigitalMinMax,
		/// <summary>
		/// ����
		/// </summary>
		[Description("18_267")]
		Analog
	}
}
