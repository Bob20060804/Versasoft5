using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices.EventDescription
{
	public static class PviServicesExtensions
	{
		public static Dictionary<string, int> SaveAs(this LoggerCollection instance, string directory, LogExportFormat exportFormat)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directory);
			directoryInfo.Create();
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (Logger item in instance)
			{
				string text = Path.Combine(directoryInfo.FullName, item.Name + ".html");
				switch (exportFormat)
				{
				case LogExportFormat.ARL:
					text = Path.ChangeExtension(text, ".arl");
					break;
				case LogExportFormat.HTML:
					text = Path.ChangeExtension(text, ".html");
					break;
				case LogExportFormat.CSV:
					text = Path.ChangeExtension(text, ".csv");
					break;
				}
				int value = item.LoggerEntries.SaveAs(text, exportFormat);
				dictionary.Add(text, value);
			}
			return dictionary;
		}

		public static Logger ToLogger(this LogBook instance, object parent, LogBookSnapshot logBookSnapshot = null)
		{
			Logger logger = (parent is Cpu) ? new Logger(parent as Cpu, instance.Name) : new Logger(parent as Service, instance.Name);
			string s = instance.SerializeToString();
			StringReader input = new StringReader(s);
			XmlTextReader xmlTextReader = new XmlTextReader(input);
			xmlTextReader.MoveToContent();
			logger.LoggerEntries.ReadLoggerEntriesSection(xmlTextReader);
			logger.propContentVersion = logger.LoggerEntries.ContentVersion;
			EventLogBook eventLogBook = instance as EventLogBook;
			if (eventLogBook != null && eventLogBook.EventEntry != null)
			{
				EventEntry[] eventEntry = eventLogBook.EventEntry;
				foreach (EventEntry instance2 in eventEntry)
				{
					logger.LoggerEntries.Add(instance2.ToLoggerEntry(logger, logBookSnapshot));
				}
			}
			ErrorLogBook errorLogBook = instance as ErrorLogBook;
			if (errorLogBook != null && errorLogBook.ErrorEntry != null)
			{
				ErrorEntry[] errorEntry = errorLogBook.ErrorEntry;
				foreach (ErrorEntry instance3 in errorEntry)
				{
					logger.LoggerEntries.Add(instance3.ToLoggerEntry(logger, logBookSnapshot));
				}
			}
			return logger;
		}

		public static LoggerCollection ToLoggerCollection(this LogBookSnapshot instance, object parent)
		{
			LoggerCollection loggerCollection = new LoggerCollection(parent, instance.Name);
			if (instance.Items == null)
			{
				return loggerCollection;
			}
			LogBook[] items = instance.Items;
			foreach (LogBook instance2 in items)
			{
				loggerCollection.Add(instance2.ToLogger(parent, instance));
			}
			return loggerCollection;
		}

		public static IEnumerable<LoggerCollection> ToLoggerCollections(this LogBookPackage instance, object parent)
		{
			List<LoggerCollection> list = new List<LoggerCollection>();
			LogBookSnapshot[] logBookSnapshot = instance.LogBookSnapshot;
			foreach (LogBookSnapshot instance2 in logBookSnapshot)
			{
				list.Add(instance2.ToLoggerCollection(parent));
			}
			return list;
		}

		public static LoggerEntry ToLoggerEntry(this LogEntry instance, Logger loggerObject, LogBookSnapshot logBookSnapshot)
		{
			if (instance == null)
			{
				return null;
			}
			try
			{
				string s = instance.SerializeToString();
				StringReader input = new StringReader(s);
				XmlTextReader xmlTextReader = new XmlTextReader(input);
				xmlTextReader.MoveToContent();
				LoggerXMLInterpreter logXMLParser = new LoggerXMLInterpreter();
				string attributeLevel = "";
				LoggerEntryCollection loggerEntryCollection = new LoggerEntryCollection("Dummy");
				loggerEntryCollection.propContentVersion = loggerObject.ContentVersion;
				LoggerEntry loggerEntry = loggerEntryCollection.MakeNewLoggEntry(xmlTextReader, logXMLParser, loggerObject, loggerObject.Name, isServiceBased: false, adjustTimeStamp: false, ref attributeLevel);
				loggerEntry.UpdateUKey(loggerEntry.ID);
				loggerEntry.propBinary = instance.Binary;
				if (loggerEntry.propBinary.Length > 0)
				{
					LoggerXMLInterpreter loggerXMLInterpreter = new LoggerXMLInterpreter();
					loggerXMLInterpreter.DecodeAdditionalData(loggerEntryCollection.ContentVersion, loggerEntry, loggerEntry.AdditionalDataFormat, loggerEntry.Binary.Length, loggerEntry.Binary, useBinForI386exc: true, cutOffAscii: false);
				}
				loggerEntry.propASCIIData = instance.ASCII;
				loggerEntry.LogBookSnapshot = logBookSnapshot;
				loggerEntry.LogBookEntry = instance;
				return loggerEntry;
			}
			catch (System.Exception)
			{
				return null;
			}
		}
	}
}
