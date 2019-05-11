namespace Ersa.Global.Common.Data.Model
{
	public class EDC_DynamischeSpalte
	{
		private const int mC_i32DefaultFeldlaenge = 200;

		public string PRO_strSpaltenName
		{
			get;
			set;
		}

		public string PRO_strDatenTyp
		{
			get;
			set;
		}

		public int PRO_i32DatenLaenge
		{
			get;
			set;
		}

		public object PRO_objWert
		{
			get;
			set;
		}

		public EDC_DynamischeSpalte()
		{
			PRO_i32DatenLaenge = 200;
		}

		public EDC_DynamischeSpalte(string i_strSpaltenName, string i_objDatenTyp, object i_objWert)
		{
			PRO_strSpaltenName = i_strSpaltenName;
			PRO_strDatenTyp = i_objDatenTyp;
			PRO_i32DatenLaenge = 200;
			PRO_objWert = i_objWert;
		}

		public EDC_DynamischeSpalte(string i_strSpaltenName, string i_objDatenTyp, int i_i32DatenLaenge, object i_objWert)
		{
			PRO_strSpaltenName = i_strSpaltenName;
			PRO_strDatenTyp = i_objDatenTyp;
			PRO_i32DatenLaenge = i_i32DatenLaenge;
			PRO_objWert = i_objWert;
		}
	}
}
