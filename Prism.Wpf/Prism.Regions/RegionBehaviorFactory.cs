using Microsoft.Practices.ServiceLocation;
using Prism.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Prism.Regions
{
	public class RegionBehaviorFactory : IRegionBehaviorFactory, IEnumerable<string>, IEnumerable
	{
		private readonly IServiceLocator serviceLocator;

		private readonly Dictionary<string, Type> registeredBehaviors = new Dictionary<string, Type>();

		public RegionBehaviorFactory(IServiceLocator serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		public void AddIfMissing(string behaviorKey, Type behaviorType)
		{
			if (behaviorKey == null)
			{
				throw new ArgumentNullException("behaviorKey");
			}
			if (behaviorType == null)
			{
				throw new ArgumentNullException("behaviorType");
			}
			if (!typeof(IRegionBehavior).IsAssignableFrom(behaviorType))
			{
				throw new ArgumentException(string.Format(Thread.CurrentThread.CurrentCulture, Prism.Properties.Resources.CanOnlyAddTypesThatInheritIFromRegionBehavior, new object[1]
				{
					behaviorType.Name
				}), "behaviorType");
			}
			if (!registeredBehaviors.ContainsKey(behaviorKey))
			{
				registeredBehaviors.Add(behaviorKey, behaviorType);
			}
		}

		public IRegionBehavior CreateFromKey(string key)
		{
			if (!ContainsKey(key))
			{
				throw new ArgumentException(string.Format(Thread.CurrentThread.CurrentCulture, Prism.Properties.Resources.TypeWithKeyNotRegistered, new object[1]
				{
					key
				}), "key");
			}
			return (IRegionBehavior)serviceLocator.GetInstance(registeredBehaviors[key]);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return registeredBehaviors.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool ContainsKey(string behaviorKey)
		{
			return registeredBehaviors.ContainsKey(behaviorKey);
		}
	}
}
