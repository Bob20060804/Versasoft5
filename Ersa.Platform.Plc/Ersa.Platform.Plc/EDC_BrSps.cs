using BR.AN.PviServices;
using Ersa.Global.Common;
using Ersa.Platform.Infrastructure.Extensions;
using Ersa.Platform.Logging;
using Ersa.Platform.Plc.Exceptions;
using Ersa.Platform.Plc.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc
{
	[EDC_SpsExportMetadaten(PRO_strSpsTyp = "PviServices")]
	[Export(typeof(INF_Sps))]
	public class EDC_BrSps : EDC_DisposableObject, INF_Sps, IDisposable
	{
        #region ==== Parameter ====
        private const int mC_i32EventRefreshInterval = 100;

		private const int mC_i32NormalrefreshInterval = 400;

		private const string mC_strSimuIp = "127.0.0.1";

		private const int Cm_i32Timeout = 5000;

		private readonly List<VariableCollection> m_lstCpuGruppen = new List<VariableCollection>();

		private readonly SemaphoreSlim m_fdcGruppenSemaphore = new SemaphoreSlim(1);

		private readonly Dictionary<string, TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>> m_dicGruppenReadCompletionSources = new Dictionary<string, TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>>();

		private Service m_fdcService;

		private Cpu m_fdcCpu;

		private VariableCollection m_fdcGruppe;

		private VariableCollection m_fdcEventGruppe;

		private VariableCollection m_fdcTempGruppe;

		private string m_strIpAdresse;

		private TaskCompletionSource<bool> m_fdcVerbindungsCompletionSource;

		private TaskCompletionSource<bool> m_fdcVariablenCompletionSource;

		private TaskCompletionSource<bool> m_fdcGruppeConnectedCompletionSource;

		private TaskCompletionSource<bool> m_fdcGruppeWriteCompletionSource;

		private List<string> m_lstFehlerhafteVariable;

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		private bool PRO_blnVerbunden
		{
			get
			{
				if (m_fdcCpu != null)
				{
					return m_fdcCpu.IsConnected;
				}
				return false;
			}
		}
        #endregion ============

        /// <summary>
        /// Build Connection
        /// </summary>
        /// <param name="i_blnOnline"></param>
        /// <param name="i_strAdresse"></param>
        /// <returns></returns>
        public Task FUN_fdcVerbindungAufbauenAsync(bool i_blnOnline, string i_strAdresse)
		{
			m_fdcVerbindungsCompletionSource = new TaskCompletionSource<bool>();
			SUB_VerbindungLoesen();
			SUB_InitPvi();
			SUB_InitGruppen();
			m_strIpAdresse = (i_blnOnline ? i_strAdresse : "127.0.0.1");
			m_fdcService.Connect();
			return m_fdcVerbindungsCompletionSource.Task.FUN_fdcMitTimeout(5000, delegate
			{
				throw new EDC_SpsVerbindungsAufbauFehlgeschlagenException("Timeout");
			});
		}

		public System.Threading.Tasks.Task FUN_fdcVariablenAnmeldenAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken)
		{
			if (!PRO_blnVerbunden)
			{
				throw new EDC_AdressRegistrierungsException("Not connected");
			}
			m_fdcVariablenCompletionSource = new TaskCompletionSource<bool>();
			IList<string> source = (i_lstVariablen as IList<string>) ?? i_lstVariablen.ToList();
			List<string> lstNeueVars = (from i_strVar in source
			where !FUN_blnVariableSchonAngelegt(i_strVar)
			select i_strVar).ToList();
			if (!lstNeueVars.Any())
			{
				return System.Threading.Tasks.Task.CompletedTask;
			}
			SUB_AlteVariablenInNeueGruppeVerschieben(m_fdcGruppe, m_fdcTempGruppe);
			m_fdcTempGruppe.Disconnect();
			SUB_GruppeFuellen(lstNeueVars, m_fdcTempGruppe, i_fdcToken);
			m_fdcTempGruppe.Connect();
			SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, string.Empty, "FUN_fdcVariablenAnmeldenAsync() Count =" + lstNeueVars.Count());
			return m_fdcVariablenCompletionSource.Task.FUN_fdcMitTimeout(100000, delegate
			{
				EDC_AdressRegistrierungsException obj = new EDC_AdressRegistrierungsException("Timeout registering variables: Incorrect variables " + string.Join(", ", m_lstFehlerhafteVariable) + " - New variables: " + string.Join(", ", lstNeueVars))
				{
					PRO_enuFehlerhafteAdressen = m_lstFehlerhafteVariable
				};
				m_lstFehlerhafteVariable = null;
				throw obj;
			});
		}

		public System.Threading.Tasks.Task FUN_fdcVariablenAbmeldenAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken)
		{
			if (!PRO_blnVerbunden)
			{
				throw new EDC_AdressRegistrierungsException("Not connected");
			}
			if (i_lstVariablen == null)
			{
				throw new EDC_AdressRegistrierungsException("List empty");
			}
			int num = 0;
			foreach (string item in i_lstVariablen)
			{
				i_fdcToken.ThrowIfCancellationRequested();
				string text = FUN_strNamenKorrigieren(item);
				SUB_VariableAusGruppeEntfernen(m_fdcEventGruppe, text);
				SUB_VariableAusGruppeEntfernen(m_fdcGruppe, text);
				SUB_VariableAusGruppeEntfernen(m_fdcTempGruppe, text);
				foreach (VariableCollection item2 in m_lstCpuGruppen)
				{
					SUB_VariableAusGruppeEntfernen(item2, text);
				}
				m_fdcCpu.Variables.Remove(text);
				num++;
			}
			SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, string.Empty, "FUN_fdcVariablenAbmeldenAsync() Count =" + num);
			return System.Threading.Tasks.Task.FromResult(result: true);
		}

        /// <summary>
        /// Event Handle register
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <param name="i_delHandler"></param>
        /// <returns></returns>
		public IDisposable FUN_fdcEventHandlerRegistrieren(string i_strVarName, System.Action i_delHandler)
		{
			if (!PRO_blnVerbunden)
			{
				throw new EDC_AdressRegistrierungsException("Not Connected");
			}
			Variable objItem = FUN_fdcItemAusCpuHolen(i_strVarName);
			VariableEventHandler delHandler = delegate(object i_objSender, VariableEventArgs i_fdcEventArgs)
			{
				if (i_fdcEventArgs.Action == BR.AN.PviServices.Action.VariableValueChangedEvent)
				{
					i_delHandler();
				}
			};
			objItem.ValueChanged += delHandler;
			if (!m_fdcEventGruppe.Contains(objItem))
			{
				objItem.RefreshTime = 100;
				m_fdcEventGruppe.Add(objItem);
				objItem.Active = true;
			}
			return EDC_Disposable.FUN_fdcCreate(delegate
			{
				objItem.ValueChanged -= delHandler;
			});
		}

		public string FUN_strWertLesen(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return string.Empty;
			}
			Variable variable = FUN_fdcItemAusCpuHolen(i_strVarName);
			variable.ReadValue(synchronous: true);
			return variable.Value.ToString(CultureInfo.InvariantCulture);
		}

		public int FUN_i32WertLesen(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0;
			}
			return FUN_fdcItemAusCpuHolen(i_strVarName).Value.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
		}

		public uint FUN_u32WertLesen(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0u;
			}
			return FUN_fdcItemAusCpuHolen(i_strVarName).Value.ToUInt32(CultureInfo.InvariantCulture.NumberFormat);
		}

		public float FUN_sngWertLesen(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0f;
			}
			return FUN_fdcItemAusCpuHolen(i_strVarName).Value.ToSingle(CultureInfo.InvariantCulture.NumberFormat);
		}

		public short FUN_i16WertLesen(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0;
			}
			return FUN_fdcItemAusCpuHolen(i_strVarName).Value.ToInt16(CultureInfo.InvariantCulture.NumberFormat);
		}

		public ushort FUN_u16WertLesen(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0;
			}
			return FUN_fdcItemAusCpuHolen(i_strVarName).Value.ToUInt16(CultureInfo.InvariantCulture.NumberFormat);
		}

		public byte FUN_bytWertLesen(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0;
			}
			return FUN_fdcItemAusCpuHolen(i_strVarName).Value.ToByte(CultureInfo.InvariantCulture.NumberFormat);
		}

		public bool FUN_blnWertLesen(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return false;
			}
			return FUN_fdcItemAusCpuHolen(i_strVarName).Value.ToBoolean(CultureInfo.InvariantCulture.NumberFormat);
		}

		public void SUB_WertSchreiben(string i_strVarName, string i_strWert)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				throw new InvalidOperationException("Cannot write if not connected");
			}
			Variable variable = FUN_fdcItemAusCpuHolen(i_strVarName);
			FUN_strWertTypGerechtSchreiben(i_strWert, variable);
			variable.WriteValue(synchronous: true);
		}

		public void SUB_EventGruppeAktivieren()
		{
			if (m_fdcEventGruppe != null && !m_fdcEventGruppe.Active && m_fdcEventGruppe.Count > 0)
			{
				m_fdcEventGruppe.Active = true;
			}
		}

		public void SUB_EventGruppeDeaktivieren()
		{
			VariableCollection fdcEventGruppe = m_fdcEventGruppe;
		}

		public void SUB_VerbindungLoesen()
		{
			if (PRO_blnVerbunden)
			{
				m_fdcCpu?.Disconnect();
				m_fdcService?.Disconnect();
			}
		}

		public async System.Threading.Tasks.Task FUN_fdcVariablenGruppeErstellenAsync(IEnumerable<string> i_enmVariablen, string i_strGruppenName, int i_i32CycleTime = 100)
		{
			List<string> lstVariablen = i_enmVariablen.ToList();
			if (string.IsNullOrEmpty(i_strGruppenName) || !lstVariablen.Any())
			{
				throw new ArgumentNullException("i_strGruppenName");
			}
			if (PRO_blnVerbunden)
			{
				Action<object, CollectionEventArgs> delReadAction = delegate(object sender, CollectionEventArgs arg)
				{
					VariableCollection variableCollection3 = sender as VariableCollection;
					if (variableCollection3 != null && variableCollection3.Name.Equals(i_strGruppenName))
					{
						List<EDC_SpsListenElement> list = new List<EDC_SpsListenElement>();
						foreach (Variable value2 in arg.Objects.Values)
						{
							EDC_SpsListenElement item = new EDC_SpsListenElement
							{
								PRO_strGruppenName = i_strGruppenName,
								PRO_strVariable = value2.Address,
								PRO_objWert = value2.Value
							};
							list.Add(item);
						}
						m_dicGruppenReadCompletionSources.TryGetValue(i_strGruppenName, out TaskCompletionSource<IEnumerable<EDC_SpsListenElement>> value);
						value?.TrySetResult(list);
					}
				};
				Action<object, CollectionEventArgs> delWrittenAction = delegate(object sender, CollectionEventArgs arg)
				{
					VariableCollection variableCollection2 = sender as VariableCollection;
					if (variableCollection2 != null && variableCollection2.Name.Equals(i_strGruppenName))
					{
						m_fdcGruppeWriteCompletionSource.SetResult(result: true);
					}
				};
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: true);
					VariableCollection variableCollection = FUN_fdcGruppeHolen(i_strGruppenName);
					if (variableCollection == null)
					{
						variableCollection = FUN_fdcGruppeAnlegen(i_strGruppenName, i_i32CycleTime);
						variableCollection.CollectionValuesRead += delegate(object sender, CollectionEventArgs arg)
						{
							delReadAction(sender, arg);
						};
						variableCollection.CollectionValuesWritten += delegate(object sender, CollectionEventArgs arg)
						{
							delWrittenAction(sender, arg);
						};
						variableCollection.CollectionConnected += SUB_GruppeConnectionChanged;
						variableCollection.CollectionDisconnected += SUB_GruppeConnectionChanged;
					}
					variableCollection.Disconnect();
					foreach (string item2 in lstVariablen)
					{
						string text = FUN_strNamenKorrigieren(item2);
						variableCollection.Add(m_fdcCpu.Variables[text] ?? FUN_fdcErstelleNeueCpuVariable(text, i_i32CycleTime));
					}
					m_fdcGruppeConnectedCompletionSource = new TaskCompletionSource<bool>();
					variableCollection.Active = true;
					variableCollection.Connect();
					await m_fdcGruppeConnectedCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
		}

		public async System.Threading.Tasks.Task FUN_fdcGruppeSchreibenAsync(IEnumerable<KeyValuePair<string, string>> i_enmParameter, string i_strGruppenName)
		{
			if (PRO_blnVerbunden)
			{
				VariableCollection fdcGruppe = FUN_fdcGruppeHolen(i_strGruppenName);
				if (fdcGruppe == null)
				{
					throw new EDC_GruppeZugriffException("B&R sps group " + i_strGruppenName + " does not exist");
				}
				List<KeyValuePair<string, string>> lstFehler = new List<KeyValuePair<string, string>>();
				foreach (KeyValuePair<string, string> item in i_enmParameter.ToList())
				{
					Variable variable = FUN_fdcItemAusCpuHolen(item.Key);
					if (variable == null)
					{
						lstFehler.Add(new KeyValuePair<string, string>(item.Key, "not registered or not in B&R sps group " + i_strGruppenName));
					}
					else
					{
						string value = FUN_strWertTypGerechtSchreiben(item.Value, variable);
						if (!string.IsNullOrEmpty(value))
						{
							lstFehler.Add(new KeyValuePair<string, string>(item.Key, value));
						}
					}
				}
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: true);
					m_fdcGruppeWriteCompletionSource = new TaskCompletionSource<bool>();
					fdcGruppe.WriteValues();
					await m_fdcGruppeWriteCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: false);
					if (lstFehler.Any())
					{
						foreach (KeyValuePair<string, string> item2 in lstFehler)
						{
							string i_strEintrag = "B&R PVI error writing variable: " + item2.Key + " cause: " + item2.Value;
							Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
							PRO_edcLogger?.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, i_strEintrag, reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name);
						}
						throw new EDC_GruppeZugriffException("B&R PVI error writing group: " + i_strGruppenName);
					}
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
		}

		public async Task<IEnumerable<EDC_SpsListenElement>> FUN_fdcGruppeLesenAsync(string i_strGruppenName)
		{
			if (string.IsNullOrEmpty(i_strGruppenName))
			{
				throw new ArgumentNullException("i_strGruppenName");
			}
			if (!PRO_blnVerbunden)
			{
				return Enumerable.Empty<EDC_SpsListenElement>();
			}
			VariableCollection fdcGruppe = FUN_fdcGruppeHolen(i_strGruppenName);
			if (fdcGruppe != null)
			{
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: true);
					TaskCompletionSource<IEnumerable<EDC_SpsListenElement>> taskCompletionSource = new TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>();
					m_dicGruppenReadCompletionSources[i_strGruppenName] = taskCompletionSource;
					fdcGruppe.ReadValues();
					return await taskCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
			throw new EDC_GruppeZugriffException("B&R sps group " + i_strGruppenName + " does not exist");
		}

		public async System.Threading.Tasks.Task FUN_fdcGruppeAktivierenAsync(string i_strGruppenName)
		{
			if (string.IsNullOrEmpty(i_strGruppenName))
			{
				throw new ArgumentNullException("i_strGruppenName");
			}
			if (PRO_blnVerbunden)
			{
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
					VariableCollection variableCollection = FUN_fdcGruppeHolen(i_strGruppenName);
					if (variableCollection == null)
					{
						throw new EDC_GruppeZugriffException("B&R sps group " + i_strGruppenName + " does not exist");
					}
					m_fdcGruppeConnectedCompletionSource = new TaskCompletionSource<bool>();
					variableCollection.Active = true;
					variableCollection.Connect();
					await m_fdcGruppeConnectedCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					m_fdcGruppenSemaphore.Release();
				}
			}
		}

		public async System.Threading.Tasks.Task FUN_fdcGruppeDeaktivierenAsync(string i_strGruppenName)
		{
			if (string.IsNullOrEmpty(i_strGruppenName))
			{
				throw new ArgumentNullException("i_strGruppenName");
			}
			if (PRO_blnVerbunden)
			{
				try
				{
					await m_fdcGruppenSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
					VariableCollection variableCollection = FUN_fdcGruppeHolen(i_strGruppenName);
					if (variableCollection == null)
					{
						throw new EDC_GruppeZugriffException("B&R sps group " + i_strGruppenName + " does not exist");
					}
					m_fdcGruppeConnectedCompletionSource = new TaskCompletionSource<bool>();
					variableCollection.Active = false;
					variableCollection.Disconnect();
					await m_fdcGruppeConnectedCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
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

		private VariableCollection FUN_fdcGruppeAnlegen(string i_strGruppenName, int i_i32CycleTime)
		{
			VariableCollection variableCollection = new VariableCollection(m_fdcCpu, i_strGruppenName);
			variableCollection.CollectionError += SUB_PviCollectionError;
			variableCollection.RefreshTime = i_i32CycleTime;
			m_lstCpuGruppen.Add(variableCollection);
			return variableCollection;
		}

		private VariableCollection FUN_fdcGruppeHolen(string i_strGruppenName)
		{
			return m_lstCpuGruppen.FirstOrDefault((VariableCollection i_edcItem) => i_edcItem.Name.Equals(i_strGruppenName));
		}

		private void SUB_GruppeConnectionChanged(object i_objSender, CollectionEventArgs i_fdcCollectionEventArgs)
		{
			VariableCollection variableCollection = i_objSender as VariableCollection;
			if (variableCollection != null)
			{
				string arg = variableCollection.Active ? "activated" : "deactivated";
				SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, $"{variableCollection.Name} was {arg}. Collection-count: {variableCollection.Count}", "SUB_GruppeConnectionChanged");
				m_fdcGruppeConnectedCompletionSource?.TrySetResult(result: true);
			}
		}

		private void SUB_VariableAusGruppeEntfernen(VariableCollection i_fdcGruppe, string i_strVariable)
		{
			Variable variable = i_fdcGruppe.Contains(i_strVariable) ? m_fdcEventGruppe[i_strVariable] : null;
			if (variable != null)
			{
				i_fdcGruppe.Remove(variable);
			}
		}

		private void SUB_Zerstoeren()
		{
			if (m_fdcEventGruppe != null)
			{
				m_fdcEventGruppe.Error -= SUB_EventGruppeError;
				SUB_GruppeDispose(m_fdcEventGruppe);
			}
			if (m_fdcGruppe != null)
			{
				SUB_GruppeDispose(m_fdcGruppe);
			}
			if (m_fdcTempGruppe != null)
			{
				SUB_GruppenEventsDeRegistrieren(m_fdcTempGruppe);
				SUB_GruppeDispose(m_fdcTempGruppe);
			}
			foreach (VariableCollection item in m_lstCpuGruppen)
			{
				item.CollectionConnected -= SUB_GruppeConnectionChanged;
				item.CollectionDisconnected -= SUB_GruppeConnectionChanged;
				SUB_GruppeDispose(item);
			}
			m_lstCpuGruppen.Clear();
			if (m_fdcCpu != null)
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, "m_fdcCpu.Connection.TcpIp.SourceStation=" + m_fdcCpu.Connection.TcpIp.SourceStation, "SUB_VerbindungLoesen()");
				m_fdcCpu.Error -= SUB_PviError;
				m_fdcCpu.Connected -= SUB_CpuConnected;
				m_fdcCpu.Disconnect();
				m_fdcCpu.Dispose();
				m_fdcCpu = null;
			}
			if (m_fdcService != null)
			{
				m_fdcService.Error -= SUB_PviError;
				m_fdcService.Connected -= SUB_ServiceConnected;
				m_fdcService.Disconnect();
				m_fdcService.Dispose();
				m_fdcService = null;
			}
		}

		private void SUB_GruppeDispose(VariableCollection i_fdcGruppe)
		{
			i_fdcGruppe.Active = false;
			i_fdcGruppe.Clear();
			i_fdcGruppe.Disconnect();
			i_fdcGruppe.Dispose();
		}

		private string FUN_strNamenKorrigieren(string i_strVar)
		{
			if (i_strVar.StartsWith("SELECTIV/."))
			{
				i_strVar = i_strVar.Substring(10);
			}
			return i_strVar;
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

		private void SUB_InitGruppen()
		{
			m_lstCpuGruppen.Clear();
			m_fdcGruppe = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString())
			{
				RefreshTime = 400
			};
			m_fdcTempGruppe = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
			SUB_GruppenEventsRegistrieren(m_fdcTempGruppe);
			m_fdcEventGruppe = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
			m_fdcEventGruppe.Error += SUB_EventGruppeError;
			m_fdcEventGruppe.CollectionPropertyChanged += SUB_EventGruppePropertyChanged;
			m_fdcEventGruppe.RefreshTime = 100;
		}

		private void SUB_EventGruppePropertyChanged(object i_objSender, CollectionEventArgs i_fdcEventArgse)
		{
			SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, "e.Objects.Count =" + i_fdcEventArgse.Objects.Count, "SUB_EventGruppePropertyChanged");
		}

		private void SUB_EventGruppeError(object i_objSender, PviEventArgs i_fdcEventArgs)
		{
			string text = i_fdcEventArgs.ErrorText;
			Variable variable = i_objSender as Variable;
			if (variable != null)
			{
				text = variable.FullName + " msg= " + text;
			}
			throw new EDC_AdressRegistrierungsException("SUB_EventGruppeError e.ErrorText = " + text);
		}

		private void SUB_ServiceConnected(object i_objSender, PviEventArgs i_fdcPviEventArgs)
		{
			m_fdcCpu.Connection.DeviceType = DeviceType.ANSLTcp;
			m_fdcCpu.Connection.ANSLTcp.DestinationIpAddress = m_strIpAdresse;
			m_fdcCpu.Connect();
		}

		private void SUB_CpuConnected(object i_objSender, PviEventArgs i_fdcPviEventArgs)
		{
			m_fdcCpu.Variables.RefreshTime = 400;
			if (i_fdcPviEventArgs.ErrorCode == 0)
			{
				m_fdcVerbindungsCompletionSource.TrySetResult(result: true);
			}
		}

		private void SUB_LogEintragSchreiben(ENUM_LogLevel i_enmLogLevel, string i_strEintrag, string i_strMethodenName = null, System.Exception i_excException = null)
		{
			if (PRO_edcLogger != null)
			{
				PRO_edcLogger.SUB_LogEintragSchreiben(i_enmLogLevel, i_strEintrag, "Ersa.Platform.Plc", "EDC_BrSps", i_strMethodenName, i_excException);
			}
		}

		private bool FUN_blnVariableSchonAngelegt(string i_strVarName)
		{
			if (string.IsNullOrWhiteSpace(i_strVarName))
			{
				return false;
			}
			if (FUN_fdcItemAusCpuHolen(i_strVarName) == null)
			{
				return false;
			}
			return true;
		}

		private string FUN_strWertTypGerechtSchreiben(string i_strWert, Variable i_fdcVar)
		{
			TypeCode typeCode = i_fdcVar.Value.GetTypeCode();
			string result = string.Empty;
			switch (typeCode)
			{
			case TypeCode.Boolean:
				i_fdcVar.Value.Assign(bool.Parse(i_strWert));
				break;
			case TypeCode.Int32:
				i_fdcVar.Value.Assign(int.Parse(i_strWert));
				break;
			case TypeCode.Single:
				i_fdcVar.Value.Assign(float.Parse(FUN_strNachkommaTest(i_strWert)));
				break;
			case TypeCode.SByte:
				i_fdcVar.Value.Assign(sbyte.Parse(i_strWert));
				break;
			case TypeCode.Int16:
				i_fdcVar.Value.Assign(short.Parse(i_strWert));
				break;
			case TypeCode.Byte:
				i_fdcVar.Value.Assign(byte.Parse(i_strWert));
				break;
			case TypeCode.UInt16:
				i_fdcVar.Value.Assign(ushort.Parse(i_strWert));
				break;
			case TypeCode.UInt32:
				i_fdcVar.Value.Assign(uint.Parse(i_strWert));
				break;
			case TypeCode.String:
				i_fdcVar.Value = i_strWert;
				break;
			case TypeCode.Double:
				i_fdcVar.Value.Assign(double.Parse(FUN_strNachkommaTest(i_strWert)));
				break;
			case TypeCode.Int64:
				i_fdcVar.Value.Assign(long.Parse(i_strWert));
				break;
			case TypeCode.UInt64:
				i_fdcVar.Value.Assign((float)(double)ulong.Parse(i_strWert));
				break;
			default:
				result = "missing datatype: " + typeCode.ToString();
				break;
			}
			return result;
		}

		private Variable FUN_fdcItemAusCpuHolen(string i_strVarName)
		{
			if (!PRO_blnVerbunden)
			{
				return null;
			}
			string name = FUN_strNamenKorrigieren(i_strVarName);
			return m_fdcCpu.Variables[name];
		}

		private Variable FUN_fdcItemAusEventGruppeHolen(string i_strVarName)
		{
			string text = FUN_strNamenKorrigieren(i_strVarName);
			if (!m_fdcEventGruppe.Contains(text))
			{
				return null;
			}
			return m_fdcEventGruppe[text];
		}

		private void SUB_GruppeFuellen(IEnumerable<string> i_lstVariablen, VariableCollection i_fdcVarCollection, CancellationToken i_fdcToken)
		{
			foreach (string item in i_lstVariablen)
			{
				i_fdcToken.ThrowIfCancellationRequested();
				i_fdcVarCollection.Add(FUN_fdcErstelleNeueCpuVariable(item, 400));
			}
			i_fdcVarCollection.Connect();
		}

		private Variable FUN_fdcErstelleNeueCpuVariable(string i_strVariable, int i_i32CycleTime)
		{
			string name = FUN_strNamenKorrigieren(i_strVariable);
			Variable obj = new Variable(m_fdcCpu, name)
			{
				Polling = false
			};
			obj.Access |= (Access.Read | Access.Write | Access.FASTECHO);
			obj.RefreshTime = i_i32CycleTime;
			obj.RuntimeObjectIndex = Variable.ROIoptions.NonZeroBasedArrayIndex;
			return obj;
		}

		private void SUB_AlteVariablenInNeueGruppeVerschieben(VariableCollection i_fdcNeuGruppe, VariableCollection i_fdcAlteGruppe)
		{
			foreach (object item in i_fdcNeuGruppe)
			{
				i_fdcNeuGruppe.Add(item as Variable);
			}
			i_fdcAlteGruppe.Clear();
		}

		private void SUB_InitPvi()
		{
			m_fdcService = new Service("Service_" + DateTime.Now.ToLongTimeString() + DateTime.Now.Millisecond);
			m_fdcService.Connected += SUB_ServiceConnected;
			m_fdcService.Error += SUB_PviError;
			m_fdcCpu = new Cpu(m_fdcService, "Cpu");
			m_fdcCpu.Error += SUB_PviError;
			m_fdcCpu.Connected += SUB_CpuConnected;
		}

		private void SUB_GruppenEventsRegistrieren(VariableCollection i_fdcGruppe)
		{
			i_fdcGruppe.Error += SUB_PviCollectionError;
			i_fdcGruppe.CollectionConnected += SUB_CollectionConnected;
			i_fdcGruppe.CollectionValuesRead += SUB_CollectionValuesRead;
		}

		private void SUB_GruppenEventsDeRegistrieren(VariableCollection i_fdcGruppe)
		{
			i_fdcGruppe.Error -= SUB_PviCollectionError;
			i_fdcGruppe.CollectionConnected -= SUB_CollectionConnected;
			i_fdcGruppe.CollectionValuesRead -= SUB_CollectionValuesRead;
		}

		private void SUB_CollectionValuesRead(object i_objSender, CollectionEventArgs i_fdcCollectionEventArgs)
		{
			VariableCollection variableCollection = i_objSender as VariableCollection;
			if (variableCollection != null)
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, $"{variableCollection.Name} read: {variableCollection.Count} variables", "SUB_CollectionValuesRead");
			}
			m_fdcVariablenCompletionSource.SetResult(result: true);
		}

		private void SUB_CollectionConnected(object i_objSender, CollectionEventArgs i_fdcCollectionEventArgs)
		{
			VariableCollection variableCollection = i_objSender as VariableCollection;
			if (variableCollection != null)
			{
				variableCollection.ReadValues();
				SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, $"{variableCollection.Name} count: {variableCollection.Count}", "SUB_CollectionConnected");
				variableCollection.Active = true;
			}
		}

		private void SUB_PviCollectionError(object i_objSender, PviEventArgs i_fdcPviEventArgs)
		{
			string i_strEintrag = "Collection Error: " + i_fdcPviEventArgs.Address + " -->" + i_fdcPviEventArgs.ErrorText;
			SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, i_strEintrag, "SUB_PviCollectionError");
			if (i_fdcPviEventArgs.Action != BR.AN.PviServices.Action.VariableDisconnect)
			{
				SUB_FehlerhafteVariableMerken(i_fdcPviEventArgs.Name);
			}
		}

		private void SUB_FehlerhafteVariableMerken(string i_strVarName)
		{
			if (m_lstFehlerhafteVariable == null)
			{
				m_lstFehlerhafteVariable = new List<string>();
			}
			if (!m_lstFehlerhafteVariable.Contains(i_strVarName))
			{
				m_lstFehlerhafteVariable.Add(i_strVarName);
			}
		}

		private void SUB_PviError(object i_objSender, PviEventArgs i_fdcPviEventArgs)
		{
			string text = "PVI Error: " + i_fdcPviEventArgs.Address + " -->" + i_fdcPviEventArgs.ErrorText;
			SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, text, "SUB_PviError");
			if (i_fdcPviEventArgs.Action != BR.AN.PviServices.Action.VariableDisconnect)
			{
				m_fdcVerbindungsCompletionSource.TrySetException(new EDC_SpsVerbindungsAufbauFehlgeschlagenException(text));
			}
		}
	}
}
