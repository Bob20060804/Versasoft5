using Prism.Common;
using Prism.Properties;
using System;
using System.ComponentModel;
using System.Windows;

namespace Prism.Regions.Behaviors
{
	public class SyncRegionContextWithHostBehavior : RegionBehavior, IHostAwareRegionBehavior, IRegionBehavior
	{
		private const string RegionContextPropertyName = "Context";

		private DependencyObject hostControl;

		public static readonly string BehaviorKey = "SyncRegionContextWithHost";

		private ObservableObject<object> HostControlRegionContext => RegionContext.GetObservableContext(hostControl);

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

		protected override void OnAttach()
		{
			if (HostControl != null)
			{
				SynchronizeRegionContext();
				HostControlRegionContext.PropertyChanged += RegionContextObservableObject_PropertyChanged;
				base.Region.PropertyChanged += Region_PropertyChanged;
			}
		}

		private void Region_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Context" && RegionManager.GetRegionContext(HostControl) != base.Region.Context)
			{
				RegionManager.SetRegionContext(hostControl, base.Region.Context);
			}
		}

		private void RegionContextObservableObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
			{
				SynchronizeRegionContext();
			}
		}

		private void SynchronizeRegionContext()
		{
			if (base.Region.Context != HostControlRegionContext.Value)
			{
				base.Region.Context = HostControlRegionContext.Value;
			}
			if (RegionManager.GetRegionContext(HostControl) != HostControlRegionContext.Value)
			{
				RegionManager.SetRegionContext(HostControl, HostControlRegionContext.Value);
			}
		}
	}
}
