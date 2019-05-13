using Ersa.Platform.Infrastructure.Extensions;
using System;

namespace Ersa.Platform.Infrastructure.Prism
{
	public abstract class EDC_ServiceLocatorBase
	{
		private IServiceProvider m_edcServiceProvider;

		public void SUB_ServiceProviderSetzen(IServiceProvider i_edcServiceProvider)
		{
			m_edcServiceProvider = i_edcServiceProvider;
		}

		public T FUN_objObjektSicherAusContainerHolen<T>()
		{
			if (m_edcServiceProvider == null)
			{
				return default(T);
			}
			return m_edcServiceProvider.GetService<T>();
		}

		public object FUN_objObjektSicherAusContainerHolen(Type i_fdcType)
		{
			if (m_edcServiceProvider == null)
			{
				return null;
			}
			return m_edcServiceProvider.GetService(i_fdcType);
		}
	}
}
