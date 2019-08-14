using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Prism.Mef
{
	public class MefServiceLocatorAdapter : ServiceLocatorImplBase
	{
		private readonly CompositionContainer compositionContainer;

		public MefServiceLocatorAdapter(CompositionContainer compositionContainer)
		{
			this.compositionContainer = compositionContainer;
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			List<object> list = new List<object>();
			IEnumerable<Lazy<object, object>> exports = compositionContainer.GetExports(serviceType, null, null);
			if (exports != null)
			{
				list.AddRange(from export in exports
				select export.Value);
			}
			return list;
		}

		protected override object DoGetInstance(Type serviceType, string key)
		{
			IEnumerable<Lazy<object, object>> exports = compositionContainer.GetExports(serviceType, null, key);
			if (exports != null && exports.Count() > 0)
			{
				return exports.Single().Value;
			}
			throw new ActivationException(FormatActivationExceptionMessage(new CompositionException("Export not found"), serviceType, key));
		}
	}
}
