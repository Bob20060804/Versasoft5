using System.ComponentModel;

namespace Ersa.Platform.Common.Produktionssteuerung
{
	public enum ENUM_CodeNichtGefundenBestaetingungsMoeglichkeit
	{
		[Description("8_1201")]
		LoetgutEntnommen,
		[Description("13_543")]
		ProgrammManuellAuswaehlen,
		[Description("8_2900")]
		CodeInCodetabelleAufnehmen
	}
}
