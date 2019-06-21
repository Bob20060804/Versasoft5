using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren
{
	public static class EDC_EditorKonstanten
	{
		public const string gC_strUiNameEditorElement = "editorElement";

		public const string gC_strUiNamePunktAnfasserElement = "punktAnfasserElement";

		public const int gC_i32ZIndexNullpunkt = -100;

		public const int gC_i32ZIndexHintergrund = -50;

		public const int gC_i32ZIndexAuswahlElement = 45;

		public const int gC_i32ZIndexInfoAnzeige = 50;

		internal static Brush PRO_fdcFarbeHell
		{
			get;
		} = Brushes.LightGray;


		internal static Brush PRO_fdcFarbeDunkel
		{
			get;
		} = Brushes.Black;


		internal static Brush PRO_fdcAuswahlFarbeHell
		{
			get;
		} = Brushes.White;


		internal static Brush PRO_fdcAuswahlFarbeDunkel
		{
			get;
		} = Brushes.Silver;


		internal static Brush PRO_fdcFehlerFarbeHell
		{
			get;
		} = Brushes.Red;


		internal static Brush PRO_fdcFehlerFarbeDunkel
		{
			get;
		} = Brushes.DarkRed;


		internal static Brush PRO_fdcFehlerAuswahlFarbeHell
		{
			get;
		} = Brushes.Pink;


		internal static Brush PRO_fdcFehlerAuswahlFarbeDunkel
		{
			get;
		} = Brushes.Red;

	}
}
