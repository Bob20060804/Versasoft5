using System;

namespace Prism.Modularity
{
	public interface IModuleManager
	{
		event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

		event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

		void Run();

		void LoadModule(string moduleName);
	}
}
