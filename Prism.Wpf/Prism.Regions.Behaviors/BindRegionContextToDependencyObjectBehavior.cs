using Prism.Common;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace Prism.Regions.Behaviors
{
	public class BindRegionContextToDependencyObjectBehavior : IRegionBehavior
	{
		public const string BehaviorKey = "ContextToDependencyObject";

		public IRegion Region
		{
			get;
			set;
		}

		public void Attach()
		{
			Region.Views.CollectionChanged += Views_CollectionChanged;
			Region.PropertyChanged += Region_PropertyChanged;
			SetContextToViews(Region.Views, Region.Context);
			AttachNotifyChangeEvent(Region.Views);
		}

		private static void SetContextToViews(IEnumerable views, object context)
		{
			foreach (object view in views)
			{
				DependencyObject dependencyObject = view as DependencyObject;
				if (dependencyObject != null)
				{
					RegionContext.GetObservableContext(dependencyObject).Value = context;
				}
			}
		}

		private void AttachNotifyChangeEvent(IEnumerable views)
		{
			foreach (object view in views)
			{
				DependencyObject dependencyObject = view as DependencyObject;
				if (dependencyObject != null)
				{
					RegionContext.GetObservableContext(dependencyObject).PropertyChanged += ViewRegionContext_OnPropertyChangedEvent;
				}
			}
		}

		private void DetachNotifyChangeEvent(IEnumerable views)
		{
			foreach (object view in views)
			{
				DependencyObject dependencyObject = view as DependencyObject;
				if (dependencyObject != null)
				{
					RegionContext.GetObservableContext(dependencyObject).PropertyChanged -= ViewRegionContext_OnPropertyChangedEvent;
				}
			}
		}

		private void ViewRegionContext_OnPropertyChangedEvent(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Value")
			{
				ObservableObject<object> observableObject = (ObservableObject<object>)sender;
				Region.Context = observableObject.Value;
			}
		}

		private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				SetContextToViews(e.NewItems, Region.Context);
				AttachNotifyChangeEvent(e.NewItems);
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove && Region.Context != null)
			{
				DetachNotifyChangeEvent(e.OldItems);
				SetContextToViews(e.OldItems, null);
			}
		}

		private void Region_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Context")
			{
				SetContextToViews(Region.Views, Region.Context);
			}
		}
	}
}
