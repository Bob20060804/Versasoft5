using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace Prism.Mef.Modularity
{
	[Export]
	public class MefFileModuleTypeLoader : IModuleTypeLoader
	{
		private const string RefFilePrefix = "file://";

		private readonly HashSet<Uri> downloadedUris = new HashSet<Uri>();

		[Import(AllowRecomposition = false)]
		private AggregateCatalog aggregateCatalog;

		public virtual event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

		public virtual event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

		public virtual bool CanLoadModuleType(ModuleInfo moduleInfo)
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

		public virtual void LoadModuleType(ModuleInfo moduleInfo)
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
					string text = (!moduleInfo.Ref.StartsWith("file:///", StringComparison.Ordinal)) ? moduleInfo.Ref.Substring("file://".Length) : moduleInfo.Ref.Substring("file://".Length + 1);
					long num = -1L;
					if (File.Exists(text))
					{
						num = new FileInfo(text).Length;
					}
					RaiseModuleDownloadProgressChanged(moduleInfo, 0L, num);
					aggregateCatalog.Catalogs.Add(new AssemblyCatalog(text));
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
	}
}
