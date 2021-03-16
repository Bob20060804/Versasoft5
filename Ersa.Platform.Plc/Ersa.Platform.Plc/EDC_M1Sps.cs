using Ersa.Global.Common;
using Ersa.Platform.Infrastructure.Extensions;
using Ersa.Platform.Logging;
using Ersa.Platform.Plc.Exceptions;
using Ersa.Platform.Plc.Model;
using M1ComNET;
using M1ComNET.Local;
using M1ComNET.M1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc
{
	[EDC_SpsExportMetadaten(PRO_strSpsTyp = "M1Com")]
	[Export(typeof(INF_Sps))]
	public class EDC_M1Sps : EDC_DisposableObject, INF_Sps, IDisposable
	{
		private const int mC_i32CycleTime = 100;

		private const int Cm_i32Timeout = 5000;

		private const string mC_strEventGruppenName = "EVENT_GROUP";

		private readonly SemaphoreSlim m_fdcGruppenSemaphore = new SemaphoreSlim(1);

		private readonly Dictionary<string, TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>> m_dicGruppenReadCompletionSources = new Dictionary<string, TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>>();

		private M1Device m_objDevice;

		private LocalDevice m_objOfflineDevice;

		private Group m_mdgEventGruppe;

		public bool PRO_blnOnline
		{
			get;
			set;
		}

		private bool PRO_blnVerbunden
		{
			get
			{
				if (PRO_blnOnline)
				{
					if (m_objDevice != null)
					{
						return m_objDevice.State == ConnectionState.Online;
					}
				}
				else if (m_objOfflineDevice != null)
				{
					return m_objOfflineDevice.State == ConnectionState.Online;
				}
				return false;
			}
		}

		[Import]
		private INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		public void SUB_EventGruppeAktivieren()
		{
			if (!PRO_blnVerbunden)
			{
				return;
			}
			Group group = FUN_fdcGruppeHolen("EVENT_GROUP");
			if (group != null)
			{
				if (group.IsRunning)
				{
					group.Update();
				}
				else
				{
					group.Start();
				}
			}
		}

		public void Sub_GroupEventDisactivate()
		{
		}

		public Task Fun_ConnectAsync(bool i_blnOnline, string i_strAdresse)
		{
			Sub_DisConnect();
			PRO_blnOnline = i_blnOnline;
			if (i_blnOnline)
			{
				try
				{
					DeviceSettings deviceSettings = new DeviceSettings
					{
						Address = i_strAdresse,
						Protocol = DeviceSettings.ProtocolType.TCP,
						Timeout = 3000u,
						VHDSessionName = DateTime.Now.Millisecond.ToString(CultureInfo.InvariantCulture)
					};
					m_objDevice = new M1Device(deviceSettings);
					PropertyChangedEventManager.AddHandler(m_objDevice, SUB_objDevice_PropertyChanged, string.Empty);
					M1Credentials m1Credentials = new M1Credentials
					{
						UserName = "M1",
						Password = "bachmann"
					};
					m_objDevice.Connect(m1Credentials);
					if (m_objDevice.AppState != AppState.RUN)
					{
						SUB_WerfeVerbindungsAufbauFehlgeschlagenException(i_strAdresse);
					}
				}
				catch (M1ComIOException i_edcException)
				{
					SUB_WerfeVerbindungsAufbauFehlgeschlagenException(i_strAdresse, i_edcException);
				}
			}
			else
			{
				m_objOfflineDevice = new LocalDevice();
				PropertyChangedEventManager.AddHandler(m_objOfflineDevice, SUB_objDevice_PropertyChanged, string.Empty);
			}
			return Task.FromResult(result: true);
		}

		public void Sub_DisConnect()
		{
			if (!PRO_blnVerbunden)
			{
				return;
			}
			if (m_objDevice != null)
			{
				foreach (Group group in m_objDevice.Groups)
				{
					if (group.IsRunning)
					{
						group.Stop();
					}
				}
			}
			if (PRO_blnOnline)
			{
				if (m_objDevice != null && ConnectionState.Online.Equals(m_objDevice.State))
				{
					m_objDevice.Close();
				}
			}
			else
			{
				m_objOfflineDevice?.Close();
			}
		}

		public bool FUN_blnVariableSchonAngelegt(string i_strVarName)
		{
			if (string.IsNullOrWhiteSpace(i_strVarName))
			{
				return false;
			}
			return FUN_itmItemHolen(i_strVarName) != null;
		}

		public Task Sub_VariablesRegisterAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken)
		{
			if (!PRO_blnVerbunden)
			{
				return Task.FromResult(result: false);
			}
			List<string> list = new List<string>();
			List<Exception> list2 = new List<Exception>();
			foreach (string item in from i_strVar in i_lstVariablen
			where !FUN_blnVariableSchonAngelegt(i_strVar)
			select i_strVar)
			{
				i_fdcToken.ThrowIfCancellationRequested();
				try
				{
					if (PRO_blnOnline)
					{
						m_objDevice.CreateItem(item);
					}
					else
					{
						ItemDescription itemDescription = default(ItemDescription);
						itemDescription.DataType = DataType.STRING8;
						itemDescription.Name = item;
						itemDescription.Readable = true;
						itemDescription.Writeable = true;
						itemDescription.Source = item;
						ItemDescription itemInfo = itemDescription;
						m_objOfflineDevice.CreateItem(itemInfo).Write("0");
					}
				}
				catch (Exception ex)
				{
					if (ex is OperationCanceledException)
					{
						throw;
					}
					list.Add(item);
					list2.Add(ex);
				}
			}
			if (list.Any())
			{
				throw new EDC_AdressRegistrierungsException("Could not register Addresses", new AggregateException(list2))
				{
					PRO_enuFehlerhafteAdressen = list
				};
			}
			return Task.FromResult(result: true);
		}

		public Task Sub_VariablesUnregister(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken)
		{
			if (!PRO_blnVerbunden)
			{
				return Task.FromResult(result: false);
			}
			if (i_lstVariablen == null)
			{
				return Task.FromResult(result: false);
			}
			foreach (string item3 in i_lstVariablen)
			{
				i_fdcToken.ThrowIfCancellationRequested();
				if (FUN_blnVariableSchonAngelegt(item3))
				{
					if (PRO_blnOnline)
					{
						if (m_objDevice != null)
						{
							foreach (Group group in m_objDevice.Groups)
							{
								SUB_VariableAusGruppeEntfernen(group, item3);
							}
							Item item = m_objDevice.GetItem(item3);
							if (item != null)
							{
								m_objDevice.RemoveItem(item);
							}
						}
					}
					else if (m_objOfflineDevice != null)
					{
						foreach (Group group2 in m_objOfflineDevice.Groups)
						{
							SUB_VariableAusGruppeEntfernen(group2, item3);
						}
						Item item2 = m_objOfflineDevice.GetItem(item3);
						if (item2 != null)
						{
							m_objOfflineDevice.RemoveItem(item2);
						}
					}
				}
			}
			SUB_EventGruppeAktivieren();
			return Task.FromResult(result: true);
		}

		void INF_Sps.Sub_GroupEventActive()
		{
			SUB_EventGruppeAktivieren();
		}

		public Item FUN_itmItemHolen(string i_strVarName)
		{
			Item result = null;
			if (!PRO_blnVerbunden)
			{
				return null;
			}
			if (PRO_blnOnline)
			{
				if (m_objDevice != null)
				{
					result = m_objDevice.GetItem(i_strVarName);
				}
			}
			else if (m_objOfflineDevice != null)
			{
				result = m_objOfflineDevice.GetItem(i_strVarName);
			}
			return result;
		}

		public string Fun_strReadValue(string i_strVarName)
		{
			return FUN_itmItemHolen(i_strVarName).ReadAsObject().ToString();
		}

		public int Fun_i32ReadValue(string i_strVarName)
		{
			Item item = FUN_itmItemHolen(i_strVarName);
			if (PRO_blnOnline)
			{
				return (item as Item<int>).Read();
			}
			return int.Parse((item as Item<string>).Read());
		}

		public uint Fun_u32ReadValue(string i_strVarName)
		{
			Item item = FUN_itmItemHolen(i_strVarName);
			if (PRO_blnOnline)
			{
				return (item as Item<uint>).Read();
			}
			return uint.Parse((item as Item<string>).Read());
		}

		public float Fun_sngReadValue(string i_strVarName)
		{
			Item item = FUN_itmItemHolen(i_strVarName);
			if (PRO_blnOnline)
			{
				return (item as Item<float>).Read();
			}
			return float.Parse((item as Item<string>).Read());
		}

		public short Fun_i16ReadValue(string i_strVarName)
		{
			Item item = FUN_itmItemHolen(i_strVarName);
			if (PRO_blnOnline)
			{
				return (item as Item<short>).Read();
			}
			return short.Parse((item as Item<string>).Read());
		}

		public ushort Fun_u16ReadValue(string i_strVarName)
		{
			Item item = FUN_itmItemHolen(i_strVarName);
			if (PRO_blnOnline)
			{
				return (item as Item<ushort>).Read();
			}
			return ushort.Parse((item as Item<string>).Read());
		}

		public byte Fun_bytReadValue(string i_strVarName)
		{
			Item item = FUN_itmItemHolen(i_strVarName);
			if (PRO_blnOnline)
			{
				return (item as Item<byte>).Read();
			}
			return byte.Parse((item as Item<string>).Read());
		}

		public bool Fun_blnReadValue(string i_strVarName)
		{
			Item item = FUN_itmItemHolen(i_strVarName);
			if (PRO_blnOnline)
			{
				return (item as Item<bool>).Read();
			}
			bool result;
			return bool.TryParse((item as Item<string>).Read(), out result) && result;
		}

		public void Sub_WriteValue(string i_strVarName, string i_strWert)
		{
			if (!PRO_blnVerbunden)
			{
				return;
			}
			Item item = FUN_itmItemHolen(i_strVarName);
			if (item != null && item.State != ConnectionState.Error)
			{
				if (item.DataType == DataType.REAL32 || item.DataType == DataType.REAL64)
				{
					i_strWert = FUN_strNachkommaTest(i_strWert);
				}
				item.Write(i_strWert);
			}
		}

		public IDisposable Fun_fdcRegisterEventHandler(string i_strVarName, Action i_delHandler)
		{
			if (string.IsNullOrEmpty(i_strVarName))
			{
				throw new ArgumentNullException("i_strVarName");
			}
			if (i_delHandler == null)
			{
				throw new ArgumentNullException("i_delHandler");
			}
			if (!PRO_blnVerbunden)
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.Warnung, "!PRO_blnVerbunden", "FUN_fdcEventHandlerRegistrieren()");
				return null;
			}
			Item objItem = FUN_itmItemHolen(i_strVarName);
			if (objItem == null)
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.Warnung, "objItem == null", "FUN_fdcEventHandlerRegistrieren()");
				return null;
			}
			EventHandler<PropertyChangedEventArgs> delHandler = delegate(object i_objSender, PropertyChangedEventArgs i_fdcArgs)
			{
				if (!(i_fdcArgs.PropertyName != "Value"))
				{
					i_delHandler();
				}
			};
			PropertyChangedEventManager.AddHandler(objItem, delHandler, "Value");
			if (m_mdgEventGruppe == null)
			{
				m_mdgEventGruppe = (PRO_blnOnline ? m_objDevice.CreateGroup("EVENT_GROUP") : m_objOfflineDevice.CreateGroup("EVENT_GROUP"));
				m_mdgEventGruppe.CycleTime = 100;
				m_mdgEventGruppe.ObservationMode = ObservationMode.PollingChanges;
				m_mdgEventGruppe.ReadError += SUB_EventGruppe_ReadErrorHandler;
				m_mdgEventGruppe.GroupUpdated += SUB_EventGruppe_GroupUpdated;
			}
			bool isRunning = m_mdgEventGruppe.IsRunning;
			if (m_mdgEventGruppe.GetItem(i_strVarName) == null)
			{
				if (isRunning)
				{
					m_mdgEventGruppe.Stop();
				}
				m_mdgEventGruppe.Add(objItem);
			}
			if (isRunning)
			{
				m_mdgEventGruppe.Start();
			}
			return EDC_Disposable.FUN_fdcCreate(delegate
			{
				PropertyChangedEventManager.RemoveHandler(objItem, delHandler, "Value");
			});
		}

		public int FUN_i32VariablenAnzahl()
		{
			if (!PRO_blnVerbunden)
			{
				return 0;
			}
			if (PRO_blnOnline)
			{
				return m_objDevice.Items.Count();
			}
			return m_objOfflineDevice.Items.Count();
		}

		public async Task Fun_fdcGroupCreateVariableAsync(IEnumerable<string> i_enmVariablen, string i_strGroupName, int i_i32CycleTime = 100)
		{
			List<string> lstVariablen = i_enmVariablen.ToList();
			if (string.IsNullOrEmpty(i_strGroupName) || !lstVariablen.Any())
			{
				throw new ArgumentNullException("i_strGroupName");
			}
			if (PRO_blnVerbunden)
			{
				Action<object, GroupUpdatedEventArgs> delUpdatedAction = delegate(object sender, GroupUpdatedEventArgs arg)
				{
					Group group3 = (Group)sender;
					if (i_strGroupName.Equals(group3.Name))
					{
						List<EDC_SpsListenElement> list = new List<EDC_SpsListenElement>();
						foreach (Item changedItem in arg.ChangedItems)
						{
							EDC_SpsListenElement item = new EDC_SpsListenElement
							{
								PRO_strGruppenName = i_strGroupName,
								PRO_strVariable = changedItem.Name,
								PRO_objWert = changedItem.ValueAsObject
							};
							list.Add(item);
						}
						m_dicGruppenReadCompletionSources.TryGetValue(i_strGroupName, out TaskCompletionSource<IEnumerable<EDC_SpsListenElement>> value2);
						value2?.TrySetResult(list);
					}
				};
				Action<object, ErrorEventArgs> delErrorHandler = delegate(object sender, ErrorEventArgs arg)
				{
					Group group2 = (Group)sender;
					if (i_strGroupName.Equals(group2.Name))
					{
						Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
						PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "M1 read error", reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name, arg.Exception);
						m_dicGruppenReadCompletionSources.TryGetValue(i_strGroupName, out TaskCompletionSource<IEnumerable<EDC_SpsListenElement>> value);
						value?.TrySetResult(Enumerable.Empty<EDC_SpsListenElement>());
					}
				};
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: true);
					Group group = FUN_fdcGruppeHolen(i_strGroupName);
					if (group == null)
					{
						group = FUN_fdcGruppeAnlegen(i_strGroupName, i_i32CycleTime);
						group.GroupUpdated += delegate(object sender, GroupUpdatedEventArgs arg)
						{
							delUpdatedAction(sender, arg);
						};
						group.ReadError += delegate(object sender, ErrorEventArgs arg)
						{
							delErrorHandler(sender, arg);
						};
					}
					if (group.IsRunning)
					{
						group.Stop();
					}
					foreach (string item2 in lstVariablen)
					{
						group.Add(FUN_itmItemHolen(item2) ?? m_objDevice.CreateItem(item2));
					}
					group.Start();
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
		}

		public async Task FUN_fdcGruppeSchreibenAsync(IEnumerable<KeyValuePair<string, string>> i_enmParameter, string i_strGroupName)
		{
			if (PRO_blnVerbunden)
			{
				Group group = FUN_fdcGruppeHolen(i_strGroupName);
				if (group == null)
				{
					throw new EDC_GruppeZugriffException("M1 sps group " + i_strGroupName + " does not exist");
				}
				List<KeyValuePair<string, string>> lstFehler = new List<KeyValuePair<string, string>>();
				ItemValueList lstItems = new ItemValueList();
				foreach (KeyValuePair<string, string> item2 in i_enmParameter)
				{
					Item item = group.GetItem(item2.Key);
					if (item == null)
					{
						lstFehler.Add(new KeyValuePair<string, string>(item2.Key, "not registered"));
					}
					else
					{
						switch (item.DataType)
						{
						case DataType.CHAR8:
							lstItems.Add((Item<byte>)item, item2.Value);
							break;
						case DataType.BOOL8:
							lstItems.Add((Item<bool>)item, item2.Value);
							break;
						case DataType.UINT16:
							lstItems.Add((Item<ushort>)item, item2.Value);
							break;
						case DataType.SINT16:
							lstItems.Add((Item<short>)item, item2.Value);
							break;
						case DataType.UINT32:
							lstItems.Add((Item<uint>)item, item2.Value);
							break;
						case DataType.SINT32:
							lstItems.Add((Item<int>)item, item2.Value);
							break;
						case DataType.REAL32:
							lstItems.Add((Item<float>)item, FUN_strNachkommaTest(item2.Value));
							break;
						case DataType.REAL64:
							lstItems.Add((Item<double>)item, FUN_strNachkommaTest(item2.Value));
							break;
						case DataType.STRING8:
							lstItems.Add((Item<string>)item, item2.Value);
							break;
						default:
							lstFehler.Add(new KeyValuePair<string, string>(item2.Key, "missing datatype: " + item.DataType.ToString()));
							break;
						}
					}
				}
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: true);
					foreach (ItemWriteError item3 in m_objDevice.writeItemValueList(lstItems))
					{
						lstFehler.Add(new KeyValuePair<string, string>(item3.Item.Name, item3.ErrorCause.ToString()));
					}
					if (lstFehler.Any())
					{
						foreach (KeyValuePair<string, string> item4 in lstFehler)
						{
							string i_strEintrag = "M1-COM error writing variable: " + item4.Key + " cause: " + item4.Value;
							Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
							PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, i_strEintrag, reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name);
						}
						throw new EDC_GruppeZugriffException("M1-COM error writing group: " + i_strGroupName);
					}
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
		}

		public async Task<IEnumerable<EDC_SpsListenElement>> Fun_fdcGroupReadAsync(string i_strGroupName)
		{
			if (string.IsNullOrEmpty(i_strGroupName))
			{
				throw new ArgumentNullException("i_strGroupName");
			}
			if (PRO_blnVerbunden)
			{
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: true);
					Group group = FUN_fdcGruppeHolen(i_strGroupName);
					if (group == null)
					{
						throw new EDC_GruppeZugriffException("M1 sps group " + i_strGroupName + " does not exist");
					}
					TaskCompletionSource<IEnumerable<EDC_SpsListenElement>> taskCompletionSource = new TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>();
					m_dicGruppenReadCompletionSources[i_strGroupName] = taskCompletionSource;
					group.Reset();
					group.Update();
					return await taskCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
			return Enumerable.Empty<EDC_SpsListenElement>();
		}

		public async Task Fun_fdcGroupActiveAsync(string i_strGroupName)
		{
			if (string.IsNullOrEmpty(i_strGroupName))
			{
				throw new ArgumentNullException("i_strGroupName");
			}
			if (PRO_blnVerbunden)
			{
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
					Group group = FUN_fdcGruppeHolen(i_strGroupName);
					if (group == null)
					{
						throw new EDC_GruppeZugriffException("M1 sps group " + i_strGroupName + " does not exist");
					}
					if (group.IsRunning)
					{
						group.Update();
					}
					else
					{
						group.Start();
					}
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
		}

		public async Task FUN_fdcGroupDisableAsync(string i_strGroupName)
		{
			if (string.IsNullOrEmpty(i_strGroupName))
			{
				throw new ArgumentNullException("i_strGroupName");
			}
			if (PRO_blnVerbunden)
			{
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
					Group group = FUN_fdcGruppeHolen(i_strGroupName);
					if (group == null)
					{
						throw new EDC_GruppeZugriffException("M1 sps group " + i_strGroupName + " does not exist");
					}
					if (group.IsRunning)
					{
						group.Stop();
					}
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
		}

		protected override void SUB_InternalDispose()
		{
			SUB_Zerstoeren();
		}

		private Group FUN_fdcGruppeAnlegen(string i_strGroupName, int i_i32CycleTime)
		{
			Group group = m_objDevice.CreateGroup(i_strGroupName);
			group.CycleTime = i_i32CycleTime;
			group.ObservationMode = ObservationMode.PollingChanges;
			return group;
		}

		private Group FUN_fdcGruppeHolen(string i_strGroupName)
		{
			return m_objDevice?.GetGroup(i_strGroupName);
		}

		private void SUB_VariableAusGruppeEntfernen(Group i_fdcGruppe, string i_strVariable)
		{
			Item item = i_fdcGruppe.GetItem(i_strVariable);
			if (item != null)
			{
				bool isRunning = i_fdcGruppe.IsRunning;
				if (isRunning)
				{
					i_fdcGruppe.Stop();
				}
				if (!i_fdcGruppe.Remove(item))
				{
					SUB_LogEintragSchreiben(ENUM_LogLevel.Warnung, "SUBa_VariablenAbmelden():Var=" + i_strVariable + " aus " + i_fdcGruppe.Name + " entfernen fehlgeschlagen!!");
				}
				if (isRunning)
				{
					i_fdcGruppe.Start();
				}
			}
		}

		private void SUB_LogEintragSchreiben(ENUM_LogLevel i_enmLogLevel, string i_strEintrag, string i_strMethodenName = null, Exception i_excException = null)
		{
			INF_Logger pRO_edcLogger = PRO_edcLogger;
		}

		private void SUB_WerfeVerbindungsAufbauFehlgeschlagenException(string i_strAdresse, Exception i_edcException = null)
		{
			throw new EDC_SpsVerbindungsAufbauFehlgeschlagenException("Could not connect to SPS using IP Address " + i_strAdresse, i_edcException);
		}

		private void SUB_Zerstoeren()
		{
			if (m_objDevice != null)
			{
				foreach (Group group in m_objDevice.Groups)
				{
					if (group.IsRunning)
					{
						group.Stop();
						group.Dispose();
					}
				}
				m_objDevice.Dispose();
			}
		}

		private void SUB_objDevice_PropertyChanged(object i_sender, PropertyChangedEventArgs i_e)
		{
			M1Device m1Device = (M1Device)i_sender;
			if (i_e.PropertyName == "State")
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "State of device changed to: " + m1Device.State);
			}
			else if (i_e.PropertyName == "AppState")
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "AppState of device changed to: " + m1Device.AppState);
			}
			else if (i_e.PropertyName == "RebootCounter")
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "RebootCounter of device changed to: " + m1Device.RebootCount);
			}
			else
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "unknown device event: " + i_e.PropertyName);
			}
		}

		private void SUB_ItemChanged(object i_objSender, PropertyChangedEventArgs i_objEventArgs)
		{
			Item item = (Item)i_objSender;
			string propertyName = i_objEventArgs.PropertyName;
			if (!(propertyName == "State"))
			{
				if (propertyName == "Value")
				{
					SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "itemevent: " + item.Name + ": " + item.ValueAsObject);
				}
				else
				{
					SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "unknown item event: " + i_objEventArgs.PropertyName);
				}
			}
			else
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "State of item " + item.Name + " changed to: " + item.State);
			}
		}

		private string FUN_strNachkommaTest(string i_strWert)
		{
			string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			if (i_strWert.Contains(numberDecimalSeparator))
			{
				return i_strWert;
			}
			if (numberDecimalSeparator == ".")
			{
				return i_strWert.Replace(",", ".");
			}
			return i_strWert.Replace(".", ",");
		}

		private void SUB_EventGruppe_ReadErrorHandler(object i_objSender, ErrorEventArgs i_objEventArgs)
		{
			Group group = (Group)i_objSender;
			if (group != null)
			{
				Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
				PRO_edcLogger?.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "M1 read error event group: " + group.Name, reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name, i_objEventArgs.Exception);
			}
		}

		private void SUB_EventGruppe_GroupUpdated(object i_objSender, GroupUpdatedEventArgs i_objEventArgs)
		{
		}
	}
}
