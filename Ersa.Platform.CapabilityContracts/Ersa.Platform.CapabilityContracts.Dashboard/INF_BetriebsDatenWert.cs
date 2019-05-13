namespace Ersa.Platform.CapabilityContracts.Dashboard
{
	public interface INF_BetriebsDatenWert
	{
		string PRO_strId
		{
			get;
		}

		string PRO_strNameKey
		{
			get;
		}

		float PRO_sngRealWert
		{
			get;
			set;
		}

		long PRO_i64Wert
		{
			get;
			set;
		}
	}
}
