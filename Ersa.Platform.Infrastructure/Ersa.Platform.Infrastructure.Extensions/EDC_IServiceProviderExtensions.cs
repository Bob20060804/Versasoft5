using System;

namespace Ersa.Platform.Infrastructure.Extensions
{
	public static class EDC_IServiceProviderExtensions
	{
		public static T GetService<T>(this IServiceProvider i_fdcServiceProvider)
		{
			return (T)i_fdcServiceProvider.GetService(typeof(T));
		}
	}
}
