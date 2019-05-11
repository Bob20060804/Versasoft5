using System.ComponentModel;

namespace Ersa.Platform.Common.Data.Maschinenverwaltung
{
	public enum ENUM_BetriebsdatenTyp
	{
		[Description("Zeit")]
		Betriebszeit,
		[Description("DatenEditInt64")]
		Loegutzaehler,
		[Description("DatenInt64")]
		Wegstreckenzaehler,
		[Description("DatenReal")]
		Energiezaehler,
		[Description("DatenReal")]
		Stickstoffmenge,
		[Description("DatenReal")]
		Flussmittel,
		[Description("DatenInt64")]
		TiegelWerte
	}
}
