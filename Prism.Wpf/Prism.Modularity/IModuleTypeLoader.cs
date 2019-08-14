using System;

namespace Prism.Modularity
{
	public interface IModuleTypeLoader
	{
		event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

		event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

		bool CanLoadModuleType(ModuleInfo moduleInfo);

		void LoadModuleType(ModuleInfo moduleInfo);
	}
}
