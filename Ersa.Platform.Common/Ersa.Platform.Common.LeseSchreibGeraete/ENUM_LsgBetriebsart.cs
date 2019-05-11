using System.ComponentModel;

namespace Ersa.Platform.Common.LeseSchreibGeraete
{
	public enum ENUM_LsgBetriebsart
	{
		[Description("10_10")]
		Ungueltig,
		[Description("13_738")]
		LesenEinmalig,
		[Description("11_837")]
		LesenProPcb
	}
}
