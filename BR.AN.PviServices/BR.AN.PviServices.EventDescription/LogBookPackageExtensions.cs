using BR.AN.EventDescriptionProviderNet.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BR.AN.PviServices.EventDescription
{
	public static class LogBookPackageExtensions
	{
		public static void AddAprolInfos(this LogBookSnapshot instance, string xmlString)
		{
			instance.Aprol = xmlString.ToObject<Aprol>();
		}

		public static bool AddPlcInfo(this LogBookSnapshot instance, string xmlString)
		{
			if (instance == null || string.IsNullOrEmpty(xmlString))
			{
				return false;
			}
			if (instance.PlcInfos == null)
			{
				instance.PlcInfos = new PlcInfos();
			}
			List<Func<object>> list = new List<Func<object>>();
			list.Add((Func<object>)xmlString.ToObject<CpuInfo>);
			list.Add((Func<object>)xmlString.ToObject<Hardware>);
			list.Add((Func<object>)xmlString.ToObject<CpuRedInfo>);
			list.Add((Func<object>)xmlString.ToObject<TOC>);
			list.Add((Func<object>)xmlString.ToObject<TechnologyPackages>);
			List<Func<object>> source = list;
			object value = (from action in source
			select action()).FirstOrDefault((object result) => result != null);
			if (value != null)
			{
				PropertyInfo[] properties = instance.PlcInfos.GetType().GetProperties();
				using (IEnumerator<PropertyInfo> enumerator = (from propertyInfo in properties
				where propertyInfo.PropertyType == value.GetType()
				select propertyInfo).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						PropertyInfo current = enumerator.Current;
						current.SetValue(instance.PlcInfos, value);
						return true;
					}
				}
			}
			return false;
		}

		public static void AddPlcInfos(this LogBookSnapshot instance, IEnumerable<string> plcInfos = null)
		{
			if (instance != null && plcInfos != null)
			{
				foreach (string plcInfo in plcInfos)
				{
					instance.AddPlcInfo(plcInfo);
				}
			}
		}

		public static void AddSnapShot(this LogBookPackage instance, LoggerCollection loggerCollection, IEventDescriptionProvider eventDescriptionProvider = null, IEnumerable<string> languages = null, IEnumerable<string> plcInfos = null)
		{
			LogBookSnapshot logBookSnapshot = loggerCollection.ToLogBookSnapshot(eventDescriptionProvider, languages);
			if (logBookSnapshot != null)
			{
				logBookSnapshot.AddPlcInfos(plcInfos);
				if (plcInfos == null)
				{
					logBookSnapshot.PlcInfos = TryGetPlcInfosFromOriginalLogbookPackage(loggerCollection);
				}
				instance.LogBookSnapshot = instance.LogBookSnapshot.Add(logBookSnapshot);
			}
		}

		public static void AddText(this LogBookSnapshot instance, string language, int textId, string text)
		{
			if (instance != null && language != null)
			{
				Language language2 = (instance.Descriptions != null) ? instance.Descriptions.FirstOrDefault((Language item) => string.Equals(item.Id, language)) : null;
				object language3 = language2;
				if (language3 == null)
				{
					Language language4 = new Language();
					language4.Id = language;
					language3 = language4;
				}
				Language language5 = (Language)language3;
				language5.Text = language5.Text.Add(new Text
				{
					Id = textId,
					Value = text
				});
				if (language2 == null)
				{
					instance.Descriptions = instance.Descriptions.Add(language5);
				}
			}
		}

		public static void CreateTexts(this LogBookSnapshot instance, IEventDescriptionProvider eventDescriptionProvider = null, IEnumerable<string> languages = null)
		{
			if (eventDescriptionProvider != null && instance.Items != null)
			{
				object list2;
				if (languages == null)
				{
					List<string> list = new List<string>();
					list.Add("en");
					list2 = list;
				}
				else
				{
					List<string> list3 = new List<string>(languages);
					list3.Add("en");
					list2 = list3;
				}
				List<string> list4 = (List<string>)list2;
				list4.RemoveAll(string.IsNullOrEmpty);
				list4 = list4.Distinct().ToList();
				int num = 1;
				List<LogEntry> list5 = instance.Items.OfType<EventLogBook>().SelectMany((Func<EventLogBook, IEnumerable<LogEntry>>)((EventLogBook item) => item.EventEntry)).ToList();
				list5.AddRange(instance.Items.OfType<ErrorLogBook>().SelectMany((ErrorLogBook item) => item.ErrorEntry));
				foreach (LogEntry item in list5)
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					List<int> list6 = new List<int>();
					foreach (string item2 in list4)
					{
						string text = item.GetText(item2, eventDescriptionProvider);
						dictionary.Add(item2, text);
						list6.Add(instance.GetTextId(item2, text));
					}
					if (!dictionary.Values.All(string.IsNullOrEmpty))
					{
						if (list6.All((int i) => i != -1) && list6.Distinct().Count() == 1)
						{
							item.TextId = (uint)list6.First();
						}
						else
						{
							foreach (KeyValuePair<string, string> item3 in dictionary)
							{
								string key = item3.Key;
								string value = item3.Value;
								if (!string.IsNullOrEmpty(value))
								{
									item.TextId = (uint)num;
									instance.AddText(key, (int)item.TextId, value);
								}
							}
							num++;
						}
					}
				}
			}
		}

		public static IEnumerable<string> GetLanguageIds(this LogBookPackage instance)
		{
			LogBookSnapshot[] source = instance.LogBookSnapshot ?? new LogBookSnapshot[0];
			IEnumerable<Language> source2 = (from snapShot in source
			where snapShot.Descriptions != null
			select snapShot).SelectMany((LogBookSnapshot snapShot) => snapShot.Descriptions);
			List<string> list = (from l in source2
			select l.Id).ToList();
			if (instance.Descriptions != null)
			{
				list.AddRange(from d in instance.Descriptions
				select d.Id);
			}
			return list.Distinct().ToList();
		}

		public static string GetText(this LogEntry instance, LogBookSnapshot logBookSnapshot, string language = "en")
		{
			if (instance == null || logBookSnapshot == null)
			{
				return null;
			}
			Language language2 = (logBookSnapshot.Descriptions != null) ? logBookSnapshot.Descriptions.FirstOrDefault((Language item) => item.Id.Equals(language)) : null;
			return ((language2 != null && language2.Text != null) ? language2.Text.FirstOrDefault((Text item) => ((uint)item.Id).Equals(instance.TextId)) : null)?.Value;
		}

		public static T ToLogBook<T>(this Logger instance) where T : class
		{
			if (instance == null || instance.LoggerEntries == null)
			{
				return null;
			}
			LogBook logBook;
			try
			{
				logBook = (Activator.CreateInstance<T>() as LogBook);
			}
			catch (System.Exception)
			{
				return null;
			}
			if (logBook != null)
			{
				logBook.Name = instance.Name;
				logBook.Version = instance.ContentVersion;
				logBook.ActiveIndex = instance.LoggerEntries._ActIndex;
				logBook.ReferenceIndex = instance.LoggerEntries._ReferenceIndex;
				logBook.Attributes = instance.LoggerEntries._LogAttributes;
			}
			EventLogBook eventLogBook = logBook as EventLogBook;
			ErrorLogBook errorLogBook = instance.HasEventLogEntries ? null : (logBook as ErrorLogBook);
			IEnumerable<LoggerEntry> source = instance.LoggerEntries.Values.OfType<LoggerEntry>();
			if (eventLogBook != null)
			{
				eventLogBook.EventEntry = (from entry in source
				select entry.ToLogEntry<EventEntry>()).ToArray();
			}
			if (errorLogBook != null)
			{
				errorLogBook.ErrorEntry = (from entry in source
				select entry.ToLogEntry<ErrorEntry>()).ToArray();
			}
			return logBook as T;
		}

		public static LogBookSnapshot ToLogBookSnapshot(this LoggerCollection instance, IEventDescriptionProvider eventDescriptionProvider = null, IEnumerable<string> languages = null)
		{
			if (instance == null)
			{
				return null;
			}
			string text = instance.Name.ToString();
			string name = (text.StartsWith("ASCpuObject_") && text.EndsWith(".Loggers")) ? DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss'.'ms") : text;
			LogBookSnapshot logBookSnapshot = new LogBookSnapshot();
			logBookSnapshot.Name = name;
			logBookSnapshot.FallBackLanguage = null;
			LogBookSnapshot logBookSnapshot2 = logBookSnapshot;
			IEventDescriptionProvider eventDescriptionProvider2 = ((object)eventDescriptionProvider) ?? ((object)new LoggerCollectionEventDescriptionProvider(instance, languages));
			foreach (Logger item in instance)
			{
				if (item.GlobalMerge)
				{
					logBookSnapshot2.Items = logBookSnapshot2.Items.Add((LogBook)(((object)item.ToLogBook<EventLogBook>()) ?? ((object)item.ToLogBook<ErrorLogBook>())));
				}
			}
			if (logBookSnapshot2.Items == null)
			{
				return null;
			}
			logBookSnapshot2.CreateTexts(eventDescriptionProvider2, languages);
			return logBookSnapshot2;
		}

		public static T ToLogEntry<T>(this LoggerEntry instance) where T : class
		{
			if (instance == null)
			{
				return null;
			}
			object obj;
			try
			{
				obj = Activator.CreateInstance<T>();
			}
			catch (System.Exception)
			{
				return null;
			}
			LogEntry logEntry = obj as LogEntry;
			if (logEntry != null)
			{
				logEntry.ErrorNumber = instance.ErrorNumber;
				logEntry.ErrorInfo = (uint)instance.ErrorInfo;
				logEntry.Time = instance.DateTime;
				logEntry.Binary = (instance.Binary ?? new byte[0]);
				logEntry.LoggerModule = instance.LoggerModuleName;
				logEntry.Task = instance.Task;
				logEntry.Level = instance.LevelType.ToSeverity();
				logEntry.ASCII = instance.ErrorText;
			}
			EventEntry eventEntry = obj as EventEntry;
			if (eventEntry != null)
			{
				eventEntry.CustomerCode = (uint)instance.CustomerCode;
				eventEntry.FacilityCode = (uint)instance.FacilityCode;
				eventEntry.EventId = instance.EventID;
				eventEntry.RecordId = instance.RecordId;
				eventEntry.OriginRecordId = (uint)instance.OriginRecordId;
				eventEntry.AdditionalDataFormat = instance.AdditionalDataFormat;
			}
			return obj as T;
		}

		public static Severity ToSeverity(this LevelType levelType)
		{
			switch (levelType)
			{
			case LevelType.Debug:
				return Severity.Debug;
			case LevelType.Success:
				return Severity.Success;
			case LevelType.Fatal:
				return Severity.Error;
			case LevelType.Warning:
				return Severity.Warning;
			default:
				return Severity.Information;
			}
		}

		private static T[] Add<T>(this T[] instance, T item) where T : class
		{
			if (item == null)
			{
				return instance;
			}
			List<T> list = (instance == null) ? new List<T>() : instance.ToList();
			list.Add(item);
			return list.ToArray();
		}

		private static string GetText(this LogEntry instance, string language, IEventDescriptionProvider eventDescriptionProvider)
		{
			if (instance == null || string.IsNullOrEmpty(language) || eventDescriptionProvider == null)
			{
				return null;
			}
			EventEntry eventEntry = instance as EventEntry;
			return (eventEntry != null && eventEntry.EventId != 0) ? eventDescriptionProvider.GetFromEventId(eventEntry.EventId, language, instance.Binary, (byte)2) : eventDescriptionProvider.GetFromErrorNr(instance.ErrorNumber, language);
		}

		private static int GetTextId(this LogBookSnapshot instance, string language, string text)
		{
			Language language2 = (instance.Descriptions != null) ? instance.Descriptions.FirstOrDefault((Language item) => item.Id.Equals(language)) : null;
			return ((language2 != null && language2.Text != null) ? language2.Text.FirstOrDefault((Text item) => item.Value.Equals(text)) : null)?.Id ?? (-1);
		}

		private static PlcInfos TryGetPlcInfosFromOriginalLogbookPackage(LoggerCollection loggerCollection)
		{
			IEnumerable<DictionaryEntry> source = loggerCollection.OfType<Logger>().SelectMany((Logger logger) => logger.LoggerEntries.OfType<DictionaryEntry>());
			return (from i in source
			select i.Value as LoggerEntryBase).FirstOrDefault(delegate(LoggerEntryBase l)
			{
				if (l != null)
				{
					return l.LogBookSnapshot != null;
				}
				return false;
			})?.LogBookSnapshot.PlcInfos;
		}
	}
}
