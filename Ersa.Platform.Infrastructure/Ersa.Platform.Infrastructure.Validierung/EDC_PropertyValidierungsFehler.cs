namespace Ersa.Platform.Infrastructure.Validierung
{
	public class EDC_PropertyValidierungsFehler : EDC_NotificationObjectMitSprachUmschaltung
	{
		public string PRO_strPropertyName
		{
			get;
			private set;
		}

		public string PRO_strNameKey
		{
			get;
			private set;
		}

		public string PRO_strFehlerKey
		{
			get;
			private set;
		}

		public EDC_PropertyValidierungsFehler(string i_strPropertyName, string i_strNameKey, string i_strFehlerKey)
		{
			PRO_strPropertyName = i_strPropertyName;
			PRO_strNameKey = i_strNameKey;
			PRO_strFehlerKey = i_strFehlerKey;
		}
	}
}
