namespace Ersa.Global.DataAdapter.DatabaseModel.Model
{
	public class EDC_UpdateInformation
	{
		public string PRO_strScript
		{
			get;
			set;
		}

		public int PRO_i32Zielversion
		{
			get;
			set;
		}

		public int PRO_i32Startversion
		{
			get;
			set;
		}

		public EDC_UpdateInformation(int i_i32Zielversion, string i_strScript, int i_i32Startversion = 1)
		{
			PRO_strScript = i_strScript;
			PRO_i32Zielversion = i_i32Zielversion;
			PRO_i32Startversion = i_i32Startversion;
		}
	}
}
