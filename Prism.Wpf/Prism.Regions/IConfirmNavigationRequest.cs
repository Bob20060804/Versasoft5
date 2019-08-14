using System;

namespace Prism.Regions
{
	public interface IConfirmNavigationRequest : INavigationAware
	{
		void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback);
	}
}
