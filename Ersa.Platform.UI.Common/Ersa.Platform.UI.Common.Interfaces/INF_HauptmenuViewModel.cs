namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_HauptmenuViewModel
	{
		void SUB_RegistriereHauptMenuEintraege(params EDC_HauptmenuEintragSpezifikation[] i_edcEintraege);

		void SUB_HauptMenuEintragHervorheben(object i_objView);
	}
}
