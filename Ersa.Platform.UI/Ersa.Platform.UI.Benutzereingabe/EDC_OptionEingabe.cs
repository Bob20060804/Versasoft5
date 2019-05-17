namespace Ersa.Platform.UI.Benutzereingabe
{
	public class EDC_OptionEingabe
	{
		public object PRO_objId
		{
			get;
			private set;
		}

		public string PRO_strName
		{
			get;
			set;
		}

		public bool PRO_blnAusgewaehlt
		{
			get;
			set;
		}

		public EDC_OptionEingabe(object i_objId)
		{
			PRO_objId = i_objId;
		}
	}
}
