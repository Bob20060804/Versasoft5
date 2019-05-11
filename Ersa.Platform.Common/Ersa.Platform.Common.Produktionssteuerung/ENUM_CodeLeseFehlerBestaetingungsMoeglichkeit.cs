using System.ComponentModel;

namespace Ersa.Platform.Common.Produktionssteuerung
{
	public enum ENUM_CodeLeseFehlerBestaetingungsMoeglichkeit
	{
		[Description("8_1201")]
		LoetgutEntnommen,
		[Description("13_543")]
		ProgrammManuellAuswaehlen,
		[Description("13_676")]
		CodeManuellEintragen,
		[Description("13_675")]
		CodeLesenWiederholen
	}
}
