using Microsoft.Practices.ServiceLocation;
using Prism.Common;
using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Prism.Regions
{
	public class RegionNavigationContentLoader : IRegionNavigationContentLoader
	{
		private readonly IServiceLocator serviceLocator;

		public RegionNavigationContentLoader(IServiceLocator serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		public object LoadContent(IRegion region, NavigationContext navigationContext)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			if (navigationContext == null)
			{
				throw new ArgumentNullException("navigationContext");
			}
			string contractFromNavigationContext = GetContractFromNavigationContext(navigationContext);
			object obj = GetCandidatesFromRegion(region, contractFromNavigationContext).Where(delegate(object v)
			{
				INavigationAware navigationAware = v as INavigationAware;
				if (navigationAware != null && !navigationAware.IsNavigationTarget(navigationContext))
				{
					return false;
				}
				FrameworkElement frameworkElement = v as FrameworkElement;
				if (frameworkElement == null)
				{
					return true;
				}
				return (frameworkElement.DataContext as INavigationAware)?.IsNavigationTarget(navigationContext) ?? true;
			}).FirstOrDefault();
			if (obj != null)
			{
				return obj;
			}
			obj = CreateNewRegionItem(contractFromNavigationContext);
			region.Add(obj);
			return obj;
		}

		protected virtual object CreateNewRegionItem(string candidateTargetContract)
		{
			try
			{
				return serviceLocator.GetInstance<object>(candidateTargetContract);
			}
			catch (ActivationException innerException)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.CannotCreateNavigationTarget, new object[1]
				{
					candidateTargetContract
				}), innerException);
			}
		}

		protected virtual string GetContractFromNavigationContext(NavigationContext navigationContext)
		{
			if (navigationContext == null)
			{
				throw new ArgumentNullException("navigationContext");
			}
			return UriParsingHelper.GetAbsolutePath(navigationContext.Uri).TrimStart('/');
		}

		protected virtual IEnumerable<object> GetCandidatesFromRegion(IRegion region, string candidateNavigationContract)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			return region.Views.Where(delegate(object v)
			{
				if (!string.Equals(v.GetType().Name, candidateNavigationContract, StringComparison.Ordinal))
				{
					return string.Equals(v.GetType().FullName, candidateNavigationContract, StringComparison.Ordinal);
				}
				return true;
			});
		}
	}
}
