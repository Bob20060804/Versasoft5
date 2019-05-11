using System.ComponentModel;

namespace Ersa.Platform.Common.Data.Loetprogrammverwaltung
{
	public enum ENUM_LoetprogrammStatus
	{
		Undefiniert,
		Unsichtbar,
		[Description("11_12")]
		Versioniert,
		[Description("13_956")]
		Freigegeben,
		[Description("13_959")]
		Arbeitsversion
	}
}
