using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism.Events
{
	internal class WeakDelegatesManager
	{
		private readonly List<DelegateReference> listeners = new List<DelegateReference>();

		public void AddListener(Delegate listener)
		{
			listeners.Add(new DelegateReference(listener, keepReferenceAlive: false));
		}

		public void RemoveListener(Delegate listener)
		{
			listeners.RemoveAll(delegate(DelegateReference reference)
			{
				Delegate target = reference.Target;
				if (!listener.Equals(target))
				{
					return (object)target == null;
				}
				return true;
			});
		}

		public void Raise(params object[] args)
		{
			listeners.RemoveAll((DelegateReference listener) => (object)listener.Target == null);
			foreach (Delegate item in from listener in listeners.ToList()
			select listener.Target into listener
			where (object)listener != null
			select listener)
			{
				item.DynamicInvoke(args);
			}
		}
	}
}
