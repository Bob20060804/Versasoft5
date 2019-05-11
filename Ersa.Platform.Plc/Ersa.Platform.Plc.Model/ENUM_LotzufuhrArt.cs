using System.ComponentModel;

namespace Ersa.Platform.Plc.Model
{
	public enum ENUM_LotzufuhrArt
	{
		[Description("8_2401")]
		Keine = 0,
		[Description("18_265")]
		Zylinder = 2,
		[Description("18_266")]
		ZylinderMagazin = 3
	}
}
