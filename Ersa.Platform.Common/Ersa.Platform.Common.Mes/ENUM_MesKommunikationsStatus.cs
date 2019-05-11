using System.ComponentModel;

namespace Ersa.Platform.Common.Mes
{
	public enum ENUM_MesKommunikationsStatus
	{
		[Description("MesDisabled")]
		MesDeaktiviert,
		[Description("MesNotConfigured")]
		MesNichtKonfiguriert,
		[Description("FunctionDisabled")]
		FunktionDeaktiviert,
		[Description("successful")]
		Erfolgreich,
		[Description("faulty")]
		Fehlerhaft,
		[Description("NotConnected")]
		NichtVerbunden
	}
}
