using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Ersa.Platform.Infrastructure
{
	[Export(typeof(IServiceProvider))]
	public class EDC_MefServiceProvider : IServiceProvider
	{
		private readonly CompositionContainer m_fdcContainer;

		[ImportingConstructor]
		public EDC_MefServiceProvider(CompositionContainer i_fdcContainer)
		{
			m_fdcContainer = i_fdcContainer;
		}

		public object GetService(Type serviceType)
		{
			return FUN_objObjektSicherAusContainerHolen(serviceType);
		}

		private object FUN_objObjektSicherAusContainerHolen(Type i_fdcType)
		{
			object result = null;
			try
			{
				if (m_fdcContainer != null)
				{
					Lazy<object, object> lazy = m_fdcContainer.GetExports(i_fdcType, null, i_fdcType.ToString()).SingleOrDefault();
					return (lazy != null) ? lazy.Value : null;
				}
				return result;
			}
			catch
			{
				return null;
			}
		}
	}
}
