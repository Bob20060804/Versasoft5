using System.ComponentModel;

namespace Ersa.Platform.Common.Mes
{
	public enum ENUM_MesInitialisierungsStatus
	{
		[Description("successful")]
		Erfolgreich,
		[Description("MesDisabled")]
		MesDeaktiviert,
		[Description("MesNotConfigured")]
		MesNichtKonfiguriert,
		[Description("Faulty")]
		Fehlerhaft
	}
}
