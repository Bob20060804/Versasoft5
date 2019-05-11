using System.Runtime.InteropServices;

namespace BR.AN.Logging
{
	[Guid("0A2F098D-BF99-492f-8986-E6F557C1083B")]
	[ComVisible(true)]
	public interface IComLogger
	{
		void Write(string uniqueLoggerName, string caller, string text);
	}
}
