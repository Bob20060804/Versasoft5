using BR.AN.EventDescriptionProviderNet;
using BR.AN.EventDescriptionProviderNet.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace BR.AN.PviServices.EventDescription
{
	public class EventDescriptionConsumer
	{
		private readonly EventDescriptionConsumerData _data;

		private LogBookPackage _logBookPackage;

		public LogBookPackage LogBookPackage
		{
			get
			{
				return _logBookPackage;
			}
			internal set
			{
				_logBookPackage = value;
			}
		}

		public EventDescriptionConsumer(EventDescriptionConsumerData data = null)
		{
			_data = data;
			_logBookPackage = ((data == null) ? new LogBookPackage() : new LogBookPackage
			{
				Version = data.Version
			});
		}

		public EventDescriptionConsumer(EventDescriptionConsumerData data, string logBookPackageFile)
			: this(data)
		{
			LoadLogBookPackage(logBookPackageFile, out _logBookPackage);
		}

		public static void LoadLogBookPackage(string filePath, out LogBookPackage logBookPackage)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			logBookPackage = fileInfo.ToObject<LogBookPackage>();
		}

		public static void SaveLogBookPackage(Stream stream, LogBookPackage logBookPackage)
		{
			logBookPackage.SerializeToStream(stream);
		}

		public void AddLoggerCollection(LoggerCollection loggerCollection, IEventDescriptionProvider eventDescriptionProvider = null, IEnumerable<string> languages = null, IEnumerable<string> plcInfos = null)
		{
			_logBookPackage.AddSnapShot(loggerCollection, eventDescriptionProvider, languages, plcInfos);
		}

		public void AddLoggerCollection(LoggerCollection loggerCollection, IEnumerable<string> textModulePaths = null, IEnumerable<string> languages = null, IEnumerable<string> plcInfos = null)
		{
			IEventDescriptionProvider eventDescriptionProvider = CreateEventDescriptionProvider(textModulePaths);
			_logBookPackage.AddSnapShot(loggerCollection, eventDescriptionProvider, languages, plcInfos);
		}

		public IEventDescriptionProvider CreateEventDescriptionProvider(IEnumerable<string> textModulePaths)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			if (textModulePaths == null)
			{
				return null;
			}
			try
			{
				TextProviderEls val = new TextProviderEls();
				foreach (string textModulePath in textModulePaths)
				{
					val.AddBrFile(textModulePath);
				}
				return new EventDescriptionProviderNet(val);
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		public void SaveLogBookPackage(Stream stream)
		{
			SaveLogBookPackage(stream, _logBookPackage);
		}
	}
}
