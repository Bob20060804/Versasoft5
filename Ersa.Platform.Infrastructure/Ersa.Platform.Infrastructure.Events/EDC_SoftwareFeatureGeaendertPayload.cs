namespace Ersa.Platform.Infrastructure.Events
{
	public class EDC_SoftwareFeatureGeaendertPayload
	{
		public ENUM_SoftwareFeatures PRO_enmSoftwareFeature
		{
			get;
			set;
		}

		public bool PRO_blnSoftwareFeatureVorhanden
		{
			get;
			set;
		}

		public EDC_SoftwareFeatureGeaendertPayload(ENUM_SoftwareFeatures i_enmSoftwareFeature, bool i_blnSoftwareFeatureVorhanden)
		{
			PRO_enmSoftwareFeature = i_enmSoftwareFeature;
			PRO_blnSoftwareFeatureVorhanden = i_blnSoftwareFeatureVorhanden;
		}
	}
}
