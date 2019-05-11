using System.Security;

namespace BR.AN.PviServices
{
	public static class StringExtensions
	{
		public static string TransformIntoValidXmlString(this string instance)
		{
			return SecurityElement.Escape(instance);
		}
	}
}
