namespace Prism.Regions
{
	public interface INavigationAware
	{
		void OnNavigatedTo(NavigationContext navigationContext);

		bool IsNavigationTarget(NavigationContext navigationContext);

		void OnNavigatedFrom(NavigationContext navigationContext);
	}
}
