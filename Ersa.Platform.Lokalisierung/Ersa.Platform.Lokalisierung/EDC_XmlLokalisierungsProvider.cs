using Ersa.Platform.Logging;
using Ersa.Platform.Lokalisierung.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPFLocalizeExtension.Providers;

namespace Ersa.Platform.Lokalisierung
{
	[Export(typeof(INF_LocalizationProvider))]
	[Export(typeof(ILocalizationProvider))]
	public class EDC_XmlLokalisierungsProvider : INF_LocalizationProvider, ILocalizationProvider
	{
		private const string mC_strTyp = "Type";

		private const string mC_strIndex = "Index";

		private static readonly SemaphoreSlim ms_fdcZugriffSemaphore = new SemaphoreSlim(1);

		private readonly INF_Logger m_edcLogger;

		private readonly Dictionary<CultureInfo, string> m_dicSprachen;

		private readonly Dictionary<CultureInfo, Dictionary<string, string>> m_dicSprachenPool;

		private readonly CultureInfo m_fdcDefaultCulture;

		private readonly CultureInfo m_fdcLastChanceCulture;

		public ObservableCollection<CultureInfo> AvailableCultures
		{
			get;
			private set;
		}

		public event ProviderChangedEventHandler ProviderChanged;

		public event ProviderErrorEventHandler ProviderError;

		public event ValueChangedEventHandler ValueChanged;

		[ImportingConstructor]
		public EDC_XmlLokalisierungsProvider(INF_Logger i_edcLogger)
		{
			m_edcLogger = i_edcLogger;
			m_dicSprachen = new Dictionary<CultureInfo, string>
			{
				{
					new CultureInfo("de"),
					"Deutsch"
				},
				{
					new CultureInfo("en"),
					"Englisch"
				},
				{
					new CultureInfo("fi"),
					"Finnisch"
				},
				{
					new CultureInfo("fr"),
					"Franzoesisch"
				},
				{
					new CultureInfo("ja"),
					"Japanisch"
				},
				{
					new CultureInfo("pl"),
					"Polnisch"
				},
				{
					new CultureInfo("ro"),
					"Rumaenisch"
				},
				{
					new CultureInfo("cs"),
					"Tschechisch"
				},
				{
					new CultureInfo("hu"),
					"Ungarisch"
				},
				{
					new CultureInfo("es"),
					"Spanisch"
				},
				{
					new CultureInfo("zh-CHS"),
					"Chinesisch"
				},
				{
					new CultureInfo("sv"),
					"Schwedisch"
				},
				{
					new CultureInfo("pt"),
					"Portugiesisch"
				},
				{
					new CultureInfo("it"),
					"Italienisch"
				},
				{
					new CultureInfo("ru"),
					"Russisch"
				},
				{
					new CultureInfo("ko"),
					"Koreanisch"
				},
				{
					new CultureInfo("sr"),
					"Serbisch"
				},
				{
					new CultureInfo("nl"),
					"Niederlaendisch"
				},
				{
					new CultureInfo("lt"),
					"Litauisch"
				},
				{
					new CultureInfo("sl"),
					"Slowenisch"
				},
				{
					new CultureInfo("da"),
					"Daenisch"
				}
			};
			AvailableCultures = new ObservableCollection<CultureInfo>(m_dicSprachen.Keys);
			m_fdcDefaultCulture = new CultureInfo("en");
			m_fdcLastChanceCulture = new CultureInfo("de");
			m_dicSprachenPool = new Dictionary<CultureInfo, Dictionary<string, string>>();
			foreach (CultureInfo availableCulture in AvailableCultures)
			{
				m_dicSprachenPool.Add(availableCulture, new Dictionary<string, string>());
			}
		}

		public FullyQualifiedResourceKeyBase GetFullyQualifiedResourceKey(string i_strKey, DependencyObject i_fdcTarget)
		{
			try
			{
				ms_fdcZugriffSemaphore.Wait();
				if (string.IsNullOrEmpty(i_strKey))
				{
					return null;
				}
				return new FQAssemblyDictionaryKey(i_strKey);
			}
			finally
			{
				ms_fdcZugriffSemaphore.Release(1);
			}
		}

		public object GetLocalizedObject(string i_strSchluessel, DependencyObject i_objTarget, CultureInfo i_objCulture)
		{
			try
			{
				ms_fdcZugriffSemaphore.Wait();
				while (!object.Equals(i_objCulture, CultureInfo.InvariantCulture) && !m_dicSprachen.ContainsKey(i_objCulture))
				{
					i_objCulture = i_objCulture.Parent;
				}
				if (string.IsNullOrWhiteSpace(i_strSchluessel))
				{
					m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Warnung, "Warning: the requested key is null", FUN_strGibMeinenNamespace(), FUN_strGibMeinenKlassennamen(), MethodBase.GetCurrentMethod().Name);
					return string.Empty;
				}
				string text = FUN_strLeseDictionaryEintrag(i_strSchluessel, i_objCulture);
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
				text = FUN_strLeseDictionaryEintrag(i_strSchluessel, m_fdcDefaultCulture);
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
				text = FUN_strLeseDictionaryEintrag(i_strSchluessel, m_fdcLastChanceCulture);
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
				return i_strSchluessel;
			}
			finally
			{
				ms_fdcZugriffSemaphore.Release(1);
			}
		}

		public async Task FUN_fdcRegistriereZusatzSprachenResourcenAsync(string i_strZusatzDatei)
		{
			if (File.Exists(i_strZusatzDatei))
			{
				await FUN_fdcRegistriereZusatzResourcenAsync(i_strZusatzDatei).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public Dictionary<CultureInfo, Dictionary<string, string>> FUN_dicHoleDasSprachenDictionary()
		{
			return m_dicSprachenPool;
		}

		public async Task FUN_fdcInitialisiereSprachDictionaryAsync(string i_strDatei)
		{
			try
			{
				if (!new FileInfo(i_strDatei).Exists)
				{
					m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "Translation file " + i_strDatei + " doesn't exist in path " + Environment.CurrentDirectory + ".", FUN_strGibMeinenNamespace(), FUN_strGibMeinenKlassennamen(), MethodBase.GetCurrentMethod().Name);
				}
				else
				{
					await ms_fdcZugriffSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
					DataTable i_fdcSprachTabelle = (await Task.Factory.StartNew(delegate
					{
						DataSet dataSet = new DataSet();
						dataSet.ReadXml(i_strDatei, XmlReadMode.InferSchema);
						return dataSet;
					}).ConfigureAwait(continueOnCapturedContext: false)).Tables[0];
					foreach (KeyValuePair<CultureInfo, string> item in m_dicSprachen)
					{
						SUB_SpeichereTabledatenInsDictionary(i_fdcSprachTabelle, "Type", "Index", item.Value, item.Key);
					}
				}
			}
			catch (Exception i_excException)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "Translation file " + i_strDatei + " in path " + Environment.CurrentDirectory + " could not be loaded.", FUN_strGibMeinenNamespace(), FUN_strGibMeinenKlassennamen(), MethodBase.GetCurrentMethod().Name, i_excException);
			}
			finally
			{
				ms_fdcZugriffSemaphore.Release(1);
			}
		}

		private Dictionary<string, string> FUN_dicHoleDasSprachenDictionary(CultureInfo i_objCulture)
		{
			if (i_objCulture == null)
			{
				return null;
			}
			try
			{
				if (m_dicSprachenPool.TryGetValue(i_objCulture, out Dictionary<string, string> value))
				{
					return value;
				}
			}
			catch (Exception i_excException)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, $"No resources available for requested language '{i_objCulture}'.", FUN_strGibMeinenNamespace(), FUN_strGibMeinenKlassennamen(), MethodBase.GetCurrentMethod().Name, i_excException);
			}
			return null;
		}

		private void SUB_SpeichereTabledatenInsDictionary(DataTable i_fdcSprachTabelle, string i_strTypenSpalte, string i_strIndexSpalte, string i_strDatenSpalte, CultureInfo i_edcInfo)
		{
			Dictionary<string, string> dictionary = FUN_dicHoleDasSprachenDictionary(i_edcInfo);
			if (dictionary != null)
			{
				foreach (DataRow row in i_fdcSprachTabelle.Rows)
				{
					string text = string.Empty;
					try
					{
						text = $"{row[i_strTypenSpalte]}_{row[i_strIndexSpalte]}";
						dictionary.Add(text, row[i_strDatenSpalte].ToString());
					}
					catch (Exception innerException)
					{
						throw new Exception("strKey = " + text, innerException);
					}
				}
			}
		}

		private void SUB_AktualisiereDatenImDictionary(DataTable i_fdcSprachTabelle, string i_strTypenSpalte, string i_strIndexSpalte, string i_strSprachName, CultureInfo i_fdcCultureInfo)
		{
			Dictionary<string, string> dictionary = FUN_dicHoleDasSprachenDictionary(i_fdcCultureInfo);
			if (dictionary != null)
			{
				foreach (DataRow row in i_fdcSprachTabelle.Rows)
				{
					string key = $"{row[i_strTypenSpalte]}_{row[i_strIndexSpalte]}";
					if (dictionary.ContainsKey(key))
					{
						dictionary[key] = row[i_strSprachName].ToString();
					}
					else
					{
						dictionary.Add(key, row[i_strSprachName].ToString());
					}
				}
			}
		}

		private string FUN_strLeseDictionaryEintrag(string i_strSchluessel, CultureInfo i_objCulture)
		{
			Dictionary<string, string> dictionary = FUN_dicHoleDasSprachenDictionary(i_objCulture);
			try
			{
				if (dictionary.TryGetValue(i_strSchluessel, out string value))
				{
					return value;
				}
			}
			catch (Exception i_excException)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, $"No language resources found for key {i_strSchluessel} in '{i_objCulture}' dictionary", FUN_strGibMeinenNamespace(), FUN_strGibMeinenKlassennamen(), MethodBase.GetCurrentMethod().Name, i_excException);
			}
			return string.Empty;
		}

		private async Task FUN_fdcRegistriereZusatzResourcenAsync(string i_strDatei)
		{
			try
			{
				await ms_fdcZugriffSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
				DataTable dataTable = (await Task.Factory.StartNew(delegate
				{
					DataSet dataSet = new DataSet();
					dataSet.ReadXml(i_strDatei, XmlReadMode.InferSchema);
					return dataSet;
				}).ConfigureAwait(continueOnCapturedContext: false)).Tables[0];
				foreach (KeyValuePair<CultureInfo, string> item in m_dicSprachen)
				{
					string value = item.Value;
					if (dataTable.Columns.Contains(value))
					{
						SUB_AktualisiereDatenImDictionary(dataTable, "Type", "Index", value, item.Key);
					}
				}
			}
			catch (Exception i_excException)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "Addition resource file " + i_strDatei + " in path " + Environment.CurrentDirectory + " could not be loaded.", FUN_strGibMeinenNamespace(), FUN_strGibMeinenKlassennamen(), MethodBase.GetCurrentMethod().Name, i_excException);
			}
			finally
			{
				ms_fdcZugriffSemaphore.Release(1);
			}
		}

		private string FUN_strGibMeinenNamespace()
		{
			string result = string.Empty;
			Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
			if (reflectedType != null)
			{
				result = reflectedType.Namespace;
			}
			return result;
		}

		private string FUN_strGibMeinenKlassennamen()
		{
			string result = string.Empty;
			Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
			if (reflectedType != null)
			{
				result = reflectedType.Name;
			}
			return result;
		}
	}
}
