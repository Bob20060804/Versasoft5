using System;
using System.Collections.Generic;
using System.IO;

namespace Prism.Modularity
{
	public class FileModuleTypeLoader : IModuleTypeLoader, IDisposable
	{
		private const string RefFilePrefix = "file://";

		private readonly IAssemblyResolver assemblyResolver;

		private HashSet<Uri> downloadedUris = new HashSet<Uri>();

		public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

		public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

		public FileModuleTypeLoader()
			: this(new AssemblyResolver())
		{
		}

		public FileModuleTypeLoader(IAssemblyResolver assemblyResolver)
		{
			this.assemblyResolver = assemblyResolver;
		}

		private void RaiseModuleDownloadProgressChanged(ModuleInfo moduleInfo, long bytesReceived, long totalBytesToReceive)
		{
			RaiseModuleDownloadProgressChanged(new ModuleDownloadProgressChangedEventArgs(moduleInfo, bytesReceived, totalBytesToReceive));
		}

		private void RaiseModuleDownloadProgressChanged(ModuleDownloadProgressChangedEventArgs e)
		{
			if (this.ModuleDownloadProgressChanged != null)
			{
				this.ModuleDownloadProgressChanged(this, e);
			}
		}

		private void RaiseLoadModuleCompleted(ModuleInfo moduleInfo, Exception error)
		{
			RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, error));
		}

		private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
		{
			if (this.LoadModuleCompleted != null)
			{
				this.LoadModuleCompleted(this, e);
			}
		}

		public bool CanLoadModuleType(ModuleInfo moduleInfo)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			if (moduleInfo.Ref != null)
			{
				return moduleInfo.Ref.StartsWith("file://", StringComparison.Ordinal);
			}
			return false;
		}

		public void LoadModuleType(ModuleInfo moduleInfo)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			try
			{
				Uri uri = new Uri(moduleInfo.Ref, UriKind.RelativeOrAbsolute);
				if (IsSuccessfullyDownloaded(uri))
				{
					RaiseLoadModuleCompleted(moduleInfo, null);
				}
				else
				{
					string localPath = uri.LocalPath;
					long num = -1L;
					if (File.Exists(localPath))
					{
						num = new FileInfo(localPath).Length;
					}
					RaiseModuleDownloadProgressChanged(moduleInfo, 0L, num);
					assemblyResolver.LoadAssemblyFrom(moduleInfo.Ref);
					RaiseModuleDownloadProgressChanged(moduleInfo, num, num);
					RecordDownloadSuccess(uri);
					RaiseLoadModuleCompleted(moduleInfo, null);
				}
			}
			catch (Exception error)
			{
				RaiseLoadModuleCompleted(moduleInfo, error);
			}
		}

		private bool IsSuccessfullyDownloaded(Uri uri)
		{
			lock (downloadedUris)
			{
				return downloadedUris.Contains(uri);
			}
		}

		private void RecordDownloadSuccess(Uri uri)
		{
			lock (downloadedUris)
			{
				downloadedUris.Add(uri);
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			(assemblyResolver as IDisposable)?.Dispose();
		}
	}
}
