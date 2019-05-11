using System.ComponentModel;

namespace Ersa.Platform.Common.Data.Meldungen
{
	public enum ENUM_MeldungsTypen
	{
		[Description("1_80")]
		enmUndefiniert,
		[Description("6_200")]
		enmFehler,
		[Description("6_201")]
		enmWarnung,
		[Description("6_202")]
		enmWarte,
		[Description("6_204")]
		enmHinweis,
		[Description("6_203")]
		enmService,
		[Description("6_346")]
		enmZyklisch,
		[Description("6_347")]
		enmGefahr,
		[Description("6_340")]
		enmExtern,
		enmVisu
	}
}
