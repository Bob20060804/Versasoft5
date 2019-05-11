using System.ComponentModel;

namespace Ersa.Platform.Common.LeseSchreibGeraete
{
	public enum ENUM_SchnittstelleTyp
	{
		[Description("11_1029")]
		RS232,
		[Description("13_735")]
		Ethernet,
		Test,
		[Description("13_988")]
		Versascan,
		[Description("11_1715")]
		BalluffEthernet
	}
}
