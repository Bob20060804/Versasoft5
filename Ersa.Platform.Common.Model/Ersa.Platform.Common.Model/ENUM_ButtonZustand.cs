using System;

namespace Ersa.Platform.Common.Model
{
	[Flags]
	public enum ENUM_ButtonZustand
	{
		enmNormalZustand = 0x0,
		enmEingabeGesperrt = 0x1,
		enmTeilablaufAktiv = 0x2,
		enmGesamtablaufAktiv = 0x4,
		enmToggleQuit = 0x8
	}
}
