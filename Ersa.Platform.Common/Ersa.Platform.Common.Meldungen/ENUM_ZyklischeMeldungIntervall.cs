using System.ComponentModel;

namespace Ersa.Platform.Common.Meldungen
{
	public enum ENUM_ZyklischeMeldungIntervall
	{
		[Description("6_310")]
		enmTaeglich = 0,
		[Description("6_311")]
		enmWoechentlich = 1,
		[Description("6_312")]
		enmMonatlich = 2,
		[Description("6_313")]
		enmBetriebszeit = 3,
		[Description("6_314")]
		enmEingelaufenePcb = 4,
		[Description("1_491")]
		enmProduktionszeit = 5,
		enmUndefiniert = 555
	}
}
