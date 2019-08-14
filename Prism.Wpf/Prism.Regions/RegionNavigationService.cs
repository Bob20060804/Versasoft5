using Microsoft.Practices.ServiceLocation;
using Prism.Common;
using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Prism.Regions
{
	public class RegionNavigationService : IRegionNavigationService, INavigateAsync
	{
		private readonly IServiceLocator serviceLocator;

		private readonly IRegionNavigationContentLoader regionNavigationContentLoader;

		private IRegionNavigationJournal journal;

		private NavigationContext currentNavigationContext;

		public IRegion Region
		{
			get;
			set;
		}

		public IRegionNavigationJournal Journal => journal;

		public event EventHandler<RegionNavigationEventArgs> Navigating;

		public event EventHandler<RegionNavigationEventArgs> Navigated;

		public event EventHandler<RegionNavigationFailedEventArgs> NavigationFailed;

		public RegionNavigationService(IServiceLocator serviceLocator, IRegionNavigationContentLoader regionNavigationContentLoader, IRegionNavigationJournal journal)
		{
			if (serviceLocator == null)
			{
				throw new ArgumentNullException("serviceLocator");
			}
			if (regionNavigationContentLoader == null)
			{
				throw new ArgumentNullException("regionNavigationContentLoader");
			}
			if (journal == null)
			{
				throw new ArgumentNullException("journal");
			}
			this.serviceLocator = serviceLocator;
			this.regionNavigationContentLoader = regionNavigationContentLoader;
			this.journal = journal;
			this.journal.NavigationTarget = this;
		}

		private void RaiseNavigating(NavigationContext navigationContext)
		{
			if (this.Navigating != null)
			{
				this.Navigating(this, new RegionNavigationEventArgs(navigationContext));
			}
		}

		private void RaiseNavigated(NavigationContext navigationContext)
		{
			if (this.Navigated != null)
			{
				this.Navigated(this, new RegionNavigationEventArgs(navigationContext));
			}
		}

		private void RaiseNavigationFailed(NavigationContext navigationContext, Exception error)
		{
			if (this.NavigationFailed != null)
			{
				this.NavigationFailed(this, new RegionNavigationFailedEventArgs(navigationContext, error));
			}
		}

		public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback)
		{
			RequestNavigate(target, navigationCallback, null);
		}

		public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
		{
			if (navigationCallback == null)
			{
				throw new ArgumentNullException("navigationCallback");
			}
			try
			{
				DoNavigate(target, navigationCallback, navigationParameters);
			}
			catch (Exception e)
			{
				NotifyNavigationFailed(new NavigationContext(this, target), navigationCallback, e);
			}
		}

		private void DoNavigate(Uri source, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (Region == null)
			{
				throw new InvalidOperationException(Prism.Properties.Resources.NavigationServiceHasNoRegion);
			}
			currentNavigationContext = new NavigationContext(this, source, navigationParameters);
			RequestCanNavigateFromOnCurrentlyActiveView(currentNavigationContext, navigationCallback, Region.ActiveViews.ToArray(), 0);
		}

		private void RequestCanNavigateFromOnCurrentlyActiveView(NavigationContext navigationContext, Action<NavigationResult> navigationCallback, object[] activeViews, int currentViewIndex)
		{
			if (currentViewIndex < activeViews.Length)
			{
				IConfirmNavigationRequest confirmNavigationRequest = activeViews[currentViewIndex] as IConfirmNavigationRequest;
				if (confirmNavigationRequest != null)
				{
					confirmNavigationRequest.ConfirmNavigationRequest(navigationContext, delegate(bool canNavigate)
					{
						if (currentNavigationContext == navigationContext && canNavigate)
						{
							RequestCanNavigateFromOnCurrentlyActiveViewModel(navigationContext, navigationCallback, activeViews, currentViewIndex);
						}
						else
						{
							NotifyNavigationFailed(navigationContext, navigationCallback, null);
						}
					});
				}
				else
				{
					RequestCanNavigateFromOnCurrentlyActiveViewModel(navigationContext, navigationCallback, activeViews, currentViewIndex);
				}
			}
			else
			{
				ExecuteNavigation(navigationContext, activeViews, navigationCallback);
			}
		}

		private void RequestCanNavigateFromOnCurrentlyActiveViewModel(NavigationContext navigationContext, Action<NavigationResult> navigationCallback, object[] activeViews, int currentViewIndex)
		{
			FrameworkElement frameworkElement = activeViews[currentViewIndex] as FrameworkElement;
			if (frameworkElement != null)
			{
				IConfirmNavigationRequest confirmNavigationRequest = frameworkElement.DataContext as IConfirmNavigationRequest;
				if (confirmNavigationRequest != null)
				{
					confirmNavigationRequest.ConfirmNavigationRequest(navigationContext, delegate(bool canNavigate)
					{
						if (currentNavigationContext == navigationContext && canNavigate)
						{
							RequestCanNavigateFromOnCurrentlyActiveView(navigationContext, navigationCallback, activeViews, currentViewIndex + 1);
						}
						else
						{
							NotifyNavigationFailed(navigationContext, navigationCallback, null);
						}
					});
					return;
				}
			}
			RequestCanNavigateFromOnCurrentlyActiveView(navigationContext, navigationCallback, activeViews, currentViewIndex + 1);
		}

		private void ExecuteNavigation(NavigationContext navigationContext, object[] activeViews, Action<NavigationResult> navigationCallback)
		{
			try
			{
				NotifyActiveViewsNavigatingFrom(navigationContext, activeViews);
				object view = regionNavigationContentLoader.LoadContent(Region, navigationContext);
				RaiseNavigating(navigationContext);
				Region.Activate(view);
				IRegionNavigationJournalEntry instance = serviceLocator.GetInstance<IRegionNavigationJournalEntry>();
				instance.Uri = navigationContext.Uri;
				instance.Parameters = navigationContext.Parameters;
				journal.RecordNavigation(instance);
				Action<INavigationAware> action = delegate(INavigationAware n)
				{
					n.OnNavigatedTo(navigationContext);
				};
				MvvmHelpers.ViewAndViewModelAction(view, action);
				navigationCallback(new NavigationResult(navigationContext, true));
				RaiseNavigated(navigationContext);
			}
			catch (Exception e)
			{
				NotifyNavigationFailed(navigationContext, navigationCallback, e);
			}
		}

		private void NotifyNavigationFailed(NavigationContext navigationContext, Action<NavigationResult> navigationCallback, Exception e)
		{
			NavigationResult obj = (e != null) ? new NavigationResult(navigationContext, e) : new NavigationResult(navigationContext, false);
			navigationCallback(obj);
			RaiseNavigationFailed(navigationContext, e);
		}

		private static void NotifyActiveViewsNavigatingFrom(NavigationContext navigationContext, object[] activeViews)
		{
			InvokeOnNavigationAwareElements(activeViews, delegate(INavigationAware n)
			{
				n.OnNavigatedFrom(navigationContext);
			});
		}

		private static void InvokeOnNavigationAwareElements(IEnumerable<object> items, Action<INavigationAware> invocation)
		{
			foreach (object item in items)
			{
				MvvmHelpers.ViewAndViewModelAction(item, invocation);
			}
		}
	}
}
