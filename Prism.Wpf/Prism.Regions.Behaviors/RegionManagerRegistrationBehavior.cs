using Prism.Properties;
using System;
using System.ComponentModel;
using System.Windows;

namespace Prism.Regions.Behaviors
{
	public class RegionManagerRegistrationBehavior : RegionBehavior, IHostAwareRegionBehavior, IRegionBehavior
	{
		public static readonly string BehaviorKey = "RegionManagerRegistration";

		private WeakReference attachedRegionManagerWeakReference;

		private DependencyObject hostControl;

		public IRegionManagerAccessor RegionManagerAccessor
		{
			get;
			set;
		}

		public DependencyObject HostControl
		{
			get
			{
				return hostControl;
			}
			set
			{
				if (base.IsAttached)
				{
					throw new InvalidOperationException(Prism.Properties.Resources.HostControlCannotBeSetAfterAttach);
				}
				hostControl = value;
			}
		}

		public RegionManagerRegistrationBehavior()
		{
			RegionManagerAccessor = new DefaultRegionManagerAccessor();
		}

		protected override void OnAttach()
		{
			if (string.IsNullOrEmpty(base.Region.Name))
			{
				base.Region.PropertyChanged += Region_PropertyChanged;
			}
			else
			{
				StartMonitoringRegionManager();
			}
		}

		private void Region_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name" && !string.IsNullOrEmpty(base.Region.Name))
			{
				base.Region.PropertyChanged -= Region_PropertyChanged;
				StartMonitoringRegionManager();
			}
		}

		private void StartMonitoringRegionManager()
		{
			RegionManagerAccessor.UpdatingRegions += OnUpdatingRegions;
			TryRegisterRegion();
		}

		private void TryRegisterRegion()
		{
			DependencyObject dependencyObject = HostControl;
			if (!dependencyObject.CheckAccess())
			{
				return;
			}
			IRegionManager regionManager = FindRegionManager(dependencyObject);
			IRegionManager attachedRegionManager = GetAttachedRegionManager();
			if (regionManager != attachedRegionManager)
			{
				if (attachedRegionManager != null)
				{
					attachedRegionManagerWeakReference = null;
					attachedRegionManager.Regions.Remove(base.Region.Name);
				}
				if (regionManager != null)
				{
					attachedRegionManagerWeakReference = new WeakReference(regionManager);
					regionManager.Regions.Add(base.Region);
				}
			}
		}

		public void OnUpdatingRegions(object sender, EventArgs e)
		{
			TryRegisterRegion();
		}

		private IRegionManager FindRegionManager(DependencyObject dependencyObject)
		{
			IRegionManager regionManager = RegionManagerAccessor.GetRegionManager(dependencyObject);
			if (regionManager != null)
			{
				return regionManager;
			}
			DependencyObject dependencyObject2 = null;
			dependencyObject2 = LogicalTreeHelper.GetParent(dependencyObject);
			if (dependencyObject2 != null)
			{
				return FindRegionManager(dependencyObject2);
			}
			return null;
		}

		private IRegionManager GetAttachedRegionManager()
		{
			if (attachedRegionManagerWeakReference != null)
			{
				return attachedRegionManagerWeakReference.Target as IRegionManager;
			}
			return null;
		}
	}
}
