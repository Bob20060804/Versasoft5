using System.Windows;

namespace Ersa.Platform.UI.Common
{
	public class EDC_Bildschirm
	{
		public int PRO_i32Nummer
		{
			get;
			private set;
		}

		public string PRO_strName
		{
			get;
			private set;
		}

		public bool PRO_blnIstPrimaerBildschirm
		{
			get;
			private set;
		}

		public Rect PRO_fdcBereich
		{
			get;
			private set;
		}

		public Rect PRO_fdcArbeitsBereich
		{
			get;
			private set;
		}

		public EDC_Bildschirm(int i_i32Nummer, string i_strName, bool i_blnIstPrimaer, Rect i_fdcBereich, Rect i_fdcArbeitsBereich)
		{
			PRO_i32Nummer = i_i32Nummer;
			PRO_strName = i_strName;
			PRO_blnIstPrimaerBildschirm = i_blnIstPrimaer;
			PRO_fdcBereich = i_fdcBereich;
			PRO_fdcArbeitsBereich = i_fdcArbeitsBereich;
		}
	}
}
