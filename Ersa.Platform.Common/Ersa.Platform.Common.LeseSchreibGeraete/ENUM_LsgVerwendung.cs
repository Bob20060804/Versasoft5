using System;
using System.ComponentModel;

namespace Ersa.Platform.Common.LeseSchreibGeraete
{
	[Flags]
	public enum ENUM_LsgVerwendung
	{
		[Description("11_837")]
		LesenProLoetgut = 0x1,
		[Description("13_763")]
		LesenBeiProduktionsstart = 0x2,
		[Description("13_1057")]
		LesenFuerBenutzerAnmeldung = 0x3,
		Ruestkontrolle = 0x4
	}
}
