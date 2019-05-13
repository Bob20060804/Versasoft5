using System.ComponentModel;

namespace Ersa.Platform.Infrastructure
{
	public enum ENUM_ModulStatus
	{
		[Description("13_437")]
		enmInitializing,
		[Description("13_438")]
		enmWaiting,
		[Description("13_439")]
		enmFaulted,
		[Description("13_440")]
		enmCompleted
	}
}
