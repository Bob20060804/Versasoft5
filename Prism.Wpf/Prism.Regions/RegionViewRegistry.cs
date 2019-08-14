using Microsoft.Practices.ServiceLocation;
using Prism.Common;
using Prism.Events;
using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Prism.Regions
{
	public class RegionViewRegistry : IRegionViewRegistry
	{
		private readonly IServiceLocator locator;

		private readonly ListDictionary<string, Func<object>> registeredContent = new ListDictionary<string, Func<object>>();

		private readonly WeakDelegatesManager contentRegisteredListeners = new WeakDelegatesManager();

		public event EventHandler<ViewRegisteredEventArgs> ContentRegistered
		{
			add
			{
				contentRegisteredListeners.AddListener(value);
			}
			remove
			{
				contentRegisteredListeners.RemoveListener(value);
			}
		}

		public RegionViewRegistry(IServiceLocator locator)
		{
			this.locator = locator;
		}

		public IEnumerable<object> GetContents(string regionName)
		{
			List<object> list = new List<object>();
			foreach (Func<object> item in registeredContent[regionName])
			{
				list.Add(item());
			}
			return list;
		}

		public void RegisterViewWithRegion(string regionName, Type viewType)
		{
			RegisterViewWithRegion(regionName, () => CreateInstance(viewType));
		}

		public void RegisterViewWithRegion(string regionName, Func<object> getContentDelegate)
		{
			registeredContent.Add(regionName, getContentDelegate);
			OnContentRegistered(new ViewRegisteredEventArgs(regionName, getContentDelegate));
		}

		protected virtual object CreateInstance(Type type)
		{
			return locator.GetInstance(type);
		}

		private void OnContentRegistered(ViewRegisteredEventArgs e)
		{
			try
			{
				contentRegisteredListeners.Raise(this, e);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = (ex.InnerException == null) ? ex.GetRootException() : ex.InnerException.GetRootException();
				throw new ViewRegistrationException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.OnViewRegisteredException, new object[2]
				{
					e.RegionName,
					ex2
				}), ex.InnerException);
			}
		}
	}
}
