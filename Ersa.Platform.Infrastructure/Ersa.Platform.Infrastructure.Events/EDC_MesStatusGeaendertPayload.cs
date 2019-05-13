namespace Ersa.Platform.Infrastructure.Events
{
	public class EDC_MesStatusGeaendertPayload
	{
		public ENUM_VerbindungsStatus PRO_enmStatus
		{
			get;
			set;
		}

		public EDC_MesStatusGeaendertPayload(ENUM_VerbindungsStatus i_enmStatus)
		{
			PRO_enmStatus = i_enmStatus;
		}
	}
}
