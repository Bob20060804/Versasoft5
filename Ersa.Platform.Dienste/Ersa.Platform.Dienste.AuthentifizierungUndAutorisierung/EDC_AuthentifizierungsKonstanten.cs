namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung
{
	public static class EDC_AuthentifizierungsKonstanten
	{
		public const string mC_strErsaBenutzer = "ersa";

		public const string mC_strErsaBenutzer_PasswortSalt = "1757627144";

		public const string mC_strErsaBenutzer_PasswortHash = "fuZHgnORVBWDpI/SLV4x2lNNqKY=";

		public const int mC_i32ErsaBenutzer_Rechte = int.MaxValue;

		public const string mC_strAdminBenutzer = "admin";

		public const string mC_strAdminBenutzer_PasswortSalt = "2062307414";

		public const string mC_strAdminBenutzer_PasswortHash = "LOpQRnbAKbZ9mAH9v8mWupGkpXA=";

		public const int mC_i32AdminBenutzer_Rechte = int.MaxValue;

		public const string mC_strAnonymerBenutzer = "---";

		public const string mC_strAnonymerBenutzer_PasswortSalt = "";

		public const string mC_strAnonymerBenutzer_PasswortHash = "";

		public const int mC_i32AnonymerBenutzer_Rechte = 0;

		internal static readonly byte[] msa_bytUniversalSchluessel = new byte[16]
		{
			171,
			155,
			143,
			157,
			144,
			163,
			189,
			157,
			149,
			149,
			166,
			162,
			0,
			0,
			0,
			0
		};
	}
}
