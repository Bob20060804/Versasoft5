using BR.AN.EventDescriptionProviderNet.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BR.AN.PviServices.EventDescription
{
	internal class LoggerCollectionEventDescriptionProvider : IEventDescriptionProvider
	{
		public IEnumerable<string> Languages
		{
			get;
			private set;
		}

		private List<Tuple<uint, string, string>> TextsForErrorNumbers
		{
			get;
			set;
		}

		private Dictionary<string, Dictionary<string, string>> TextsForEventIds
		{
			get;
			set;
		}

		public LoggerCollectionEventDescriptionProvider(LoggerCollection loggerCollection, IEnumerable<string> languages = null)
		{
			Languages = ((languages != null) ? new List<string>(languages)
			{
				"en"
			} : new List<string>
			{
				"en"
			});
			TextsForErrorNumbers = new List<Tuple<uint, string, string>>();
			TextsForEventIds = new Dictionary<string, Dictionary<string, string>>();
			foreach (Logger item in loggerCollection)
			{
				foreach (DictionaryEntry loggerEntry2 in item.LoggerEntries)
				{
					LoggerEntry loggerEntry = loggerEntry2.Value as LoggerEntry;
					if (loggerEntry != null)
					{
						foreach (string language in Languages)
						{
							string errorDescription = loggerEntry.GetErrorDescription(language);
							if (!string.IsNullOrEmpty(errorDescription))
							{
								if (loggerEntry.EventID != 0)
								{
									string keyForSearch = GetKeyForSearch(language, loggerEntry.EventID);
									if (!TextsForEventIds.ContainsKey(keyForSearch))
									{
										TextsForEventIds.Add(keyForSearch, new Dictionary<string, string>());
									}
									string subKeyForSearch = GetSubKeyForSearch(loggerEntry.Binary ?? new byte[0]);
									if (!TextsForEventIds[keyForSearch].ContainsKey(subKeyForSearch))
									{
										TextsForEventIds[keyForSearch].Add(subKeyForSearch, errorDescription);
									}
								}
								else
								{
									TextsForErrorNumbers.Add(new Tuple<uint, string, string>(loggerEntry.ErrorNumber, language, errorDescription));
								}
							}
						}
					}
				}
			}
			TextsForErrorNumbers = TextsForErrorNumbers.Distinct().ToList();
		}

		public string GetFromErrorNr(uint errorNr, string languageId)
		{
			if (TextsForErrorNumbers == null)
			{
				return string.Empty;
			}
			Tuple<uint, string, string> tuple = TextsForErrorNumbers.FirstOrDefault(delegate(Tuple<uint, string, string> item)
			{
				if (item.Item2.Equals(languageId))
				{
					return item.Item1.Equals(errorNr);
				}
				return false;
			});
			if (tuple == null)
			{
				return string.Empty;
			}
			return tuple.Item3;
		}

		public string GetFromEventId(int eventId, string languageId, byte[] data, byte blobType)
		{
			if (TextsForEventIds == null)
			{
				return string.Empty;
			}
			string keyForSearch = GetKeyForSearch(languageId, eventId);
			string subKeyForSearch = GetSubKeyForSearch(data);
			if (!TextsForEventIds.ContainsKey(keyForSearch))
			{
				return null;
			}
			if (TextsForEventIds[keyForSearch].ContainsKey(subKeyForSearch))
			{
				return TextsForEventIds[keyForSearch][subKeyForSearch];
			}
			return null;
		}

		private string GetKeyForSearch(string language, int eventId)
		{
			return $"{language}.{eventId}";
		}

		private string GetSubKeyForSearch(byte[] binary)
		{
			return Encoding.UTF8.GetString(binary ?? new byte[0]);
		}
	}
}
