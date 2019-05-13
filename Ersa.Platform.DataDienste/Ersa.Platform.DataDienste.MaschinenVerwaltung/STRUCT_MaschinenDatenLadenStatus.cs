namespace Ersa.Platform.DataDienste.MaschinenVerwaltung
{
	public struct STRUCT_MaschinenDatenLadenStatus
	{
		public ENUM_MaschinenDatenLadenStatus PRO_enmStatus
		{
			get;
			set;
		}

		public string PRO_strStatusKey
		{
			get;
			set;
		}
	}
}
