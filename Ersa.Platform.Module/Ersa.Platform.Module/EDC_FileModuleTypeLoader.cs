using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace Ersa.Platform.Module
{
	[Export]
	public sealed class EDC_FileModuleTypeLoader : IModuleTypeLoader
	{
		private const string mC_strRefFilePrefix = "file://";

		private readonly HashSet<Uri> m_fdcDownloadedUris = new HashSet<Uri>();

		[Import(AllowRecomposition = false)]
		private AggregateCatalog m_edcAggregateCatalog;

		public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

		public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

		public bool CanLoadModuleType(ModuleInfo i_fdcModuleInfo)
		{
			if (i_fdcModuleInfo == null)
			{
				throw new ArgumentNullException("i_fdcModuleInfo");
			}
			if (i_fdcModuleInfo.Ref != null)
			{
				return i_fdcModuleInfo.Ref.StartsWith("file://", StringComparison.Ordinal);
			}
			return false;
		}

		public void LoadModuleType(ModuleInfo i_fdcModuleInfo)
		{
			if (i_fdcModuleInfo == null)
			{
				throw new ArgumentNullException("i_fdcModuleInfo");
			}
			try
			{
				Uri i_fdcUri = new Uri(i_fdcModuleInfo.Ref, UriKind.RelativeOrAbsolute);
				if (IsSuccessfullyDownloaded(i_fdcUri))
				{
					RaiseLoadModuleCompleted(i_fdcModuleInfo, null);
				}
				else
				{
					string text = i_fdcModuleInfo.Ref.Substring("file://".Length + 1);
					long num = -1L;
					if (File.Exists(text))
					{
						num = new FileInfo(text).Length;
					}
					RaiseModuleDownloadProgressChanged(i_fdcModuleInfo, 0L, num);
					m_edcAggregateCatalog.Catalogs.Add(new TypeCatalog(Type.GetType(i_fdcModuleInfo.ModuleType)));
					RaiseModuleDownloadProgressChanged(i_fdcModuleInfo, num, num);
					RecordDownloadSuccess(i_fdcUri);
					RaiseLoadModuleCompleted(i_fdcModuleInfo, null);
				}
			}
			catch (Exception i_fdcException)
			{
				RaiseLoadModuleCompleted(i_fdcModuleInfo, i_fdcException);
			}
		}

		private void RaiseModuleDownloadProgressChanged(ModuleInfo i_edcModuleInfo, long i_lngBytesReceived, long i_lngTotalBytesToReceive)
		{
			RaiseModuleDownloadProgressChanged(new ModuleDownloadProgressChangedEventArgs(i_edcModuleInfo, i_lngBytesReceived, i_lngTotalBytesToReceive));
		}

		private void RaiseModuleDownloadProgressChanged(ModuleDownloadProgressChangedEventArgs i_fdcChangedEventArgs)
		{
			if (this.ModuleDownloadProgressChanged != null)
			{
				this.ModuleDownloadProgressChanged(this, i_fdcChangedEventArgs);
			}
		}

		private void RaiseLoadModuleCompleted(ModuleInfo i_fdcModuleInfo, Exception i_fdcException)
		{
			RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(i_fdcModuleInfo, i_fdcException));
		}

		private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs i_fdcEventArgs)
		{
			if (this.LoadModuleCompleted != null)
			{
				this.LoadModuleCompleted(this, i_fdcEventArgs);
			}
		}

		private bool IsSuccessfullyDownloaded(Uri i_fdcUri)
		{
			lock (m_fdcDownloadedUris)
			{
				return m_fdcDownloadedUris.Contains(i_fdcUri);
			}
		}

		private void RecordDownloadSuccess(Uri i_fdcUri)
		{
			lock (m_fdcDownloadedUris)
			{
				m_fdcDownloadedUris.Add(i_fdcUri);
			}
		}
	}
}
