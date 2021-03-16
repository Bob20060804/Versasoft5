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
    /// <summary>
    /// B&R帮助类
    /// </summary>
	[EDC_SpsExportMetadaten(PRO_strSpsTyp = "PviServices")]
	[Export(typeof(INF_Sps))]
	public class EDC_BrSps : EDC_DisposableObject, INF_Sps, IDisposable
	{
        #region ==== Parameter ====
        private const int mC_i32EventRefreshInterval = 100;

		private const int mC_i32NormalrefreshInterval = 400;

		private const string mC_strSimuIp = "127.0.0.1";

		private const int Cm_i32Timeout = 5000;

		private readonly SemaphoreSlim m_fdcGroupSemaphore = new SemaphoreSlim(1);

		private readonly List<VariableCollection> m_lstGroup = new List<VariableCollection>();
		/// <summary>
		/// 
		/// </summary>
		private readonly Dictionary<string, TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>> m_dicGruppenReadCompletionSources = new Dictionary<string, TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>>();

		private Service m_fdcService;

		private Cpu m_fdcCpu;

		private VariableCollection m_fdcGroup;

		private VariableCollection m_fdcGroupEvent;

		private VariableCollection m_fdcGroupTemp;

		private string m_strIpAdresse;
		/// <summary>
		/// 连接是否完成
		/// </summary>
		private TaskCompletionSource<bool> m_fdcConnectionCompletionSource;
		/// <summary>
		/// 变量登记是否完成
		/// </summary>
		private TaskCompletionSource<bool> m_fdcVariableCompletionSource;
		/// <summary>
		/// 组连接是否完成
		/// </summary>
		private TaskCompletionSource<bool> m_fdcGroupConnectedCompletionSource;
		/// <summary>
		/// 组写入是否完成
		/// </summary>
		private TaskCompletionSource<bool> m_fdcGruppeWriteCompletionSource;

        /// <summary>
        /// 错误的变量
        /// </summary>
		private List<string> m_lstFehlerhafteVariable;

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

        /// <summary>
        /// m_fdcCpu 是否连接的
        /// </summary>
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
        /// 建立连接 异步
        /// Connect
        /// </summary>
        /// <param name="i_blnOnline"></param>
        /// <param name="i_strAdresse"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task Fun_ConnectAsync(bool i_blnOnline, string i_strAdresse)
		{
			m_fdcConnectionCompletionSource = new TaskCompletionSource<bool>();
			Sub_DisConnect();
			SUB_InitPvi();
			Sub_InitializeGroup();
			m_strIpAdresse = (i_blnOnline ? i_strAdresse : "127.0.0.1");
			m_fdcService.Connect();
			return m_fdcConnectionCompletionSource.Task.FUN_fdcMitTimeout(5000, delegate
			{
				throw new EDC_SpsVerbindungsAufbauFehlgeschlagenException("Timeout");
			});
		}

        /// <summary>
        /// 变量登陆 异步
        /// Variables sign in
        /// </summary>
        /// <param name="i_lstVariablen">变量</param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		public System.Threading.Tasks.Task Sub_VariablesRegisterAsync(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken)
		{
			if (!PRO_blnVerbunden)
			{
				throw new EDC_AdressRegistrierungsException("Not connected");
			}

			// 开始添加变量标记
			m_fdcVariableCompletionSource = new TaskCompletionSource<bool>();
			IList<string> source = (i_lstVariablen as IList<string>) ?? i_lstVariablen.ToList();

            // 如果变量都已经应用了, 则结束线程
			List<string> lstNeueVars = (from i_strVar in source
										where !FUN_blnVariableSchonAngelegt(i_strVar)
										select i_strVar).ToList();
			if (!lstNeueVars.Any())
			{
				return System.Threading.Tasks.Task.CompletedTask;
			}

			// 把变量移动到新的组  // m_fdcGroupTemp .item -> m_fdcGroup
			SUB_AlteVariablenInNeueGruppeVerschieben(m_fdcGroup, m_fdcGroupTemp);
			m_fdcGroupTemp.Disconnect();

            // 把没有应用的变量添加到临时组
			Sub_GroupFill(lstNeueVars, m_fdcGroupTemp, i_fdcToken);
			m_fdcGroupTemp.Connect();
			SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, string.Empty, "FUN_fdcVariablenAnmeldenAsync() Count =" + lstNeueVars.Count());

			return m_fdcVariableCompletionSource.Task.FUN_fdcMitTimeout(100000, delegate
			{
				EDC_AdressRegistrierungsException obj = new EDC_AdressRegistrierungsException("Timeout registering variables: Incorrect variables " + string.Join(", ", m_lstFehlerhafteVariable) + " - New variables: " + string.Join(", ", lstNeueVars))
				{
					PRO_enuFehlerhafteAdressen = m_lstFehlerhafteVariable
				};
				m_lstFehlerhafteVariable = null;
				throw obj;
			});
		}

        /// <summary>
        /// 删除变量 Log out variables async
        /// </summary>
        /// <param name="i_lstVariablen"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		public System.Threading.Tasks.Task Sub_VariablesUnregister(IEnumerable<string> i_lstVariablen, CancellationToken i_fdcToken)
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
				SUB_VariableAusGruppeEntfernen(m_fdcGroupEvent, text);
				SUB_VariableAusGruppeEntfernen(m_fdcGroup, text);
				SUB_VariableAusGruppeEntfernen(m_fdcGroupTemp, text);
				foreach (VariableCollection item2 in m_lstGroup)
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
        /// 注册事件
        /// EventHandler Register
        /// </summary>
        /// <param name="i_strVariableName">变量名</param>
        /// <param name="i_delHandler"></param>
        /// <returns></returns>
		public IDisposable Fun_fdcRegisterEventHandler(string i_strVariableName, System.Action i_delHandler)
		{
			if (!PRO_blnVerbunden)
			{
				throw new EDC_AdressRegistrierungsException("Not Connected");
			}
			Variable a_Variable = Fun_edcGetVariableFromCpu(i_strVariableName);

			VariableEventHandler delHandler = delegate(object i_objSender, VariableEventArgs i_fdcEventArgs)
			{
				if (i_fdcEventArgs.Action == BR.AN.PviServices.Action.VariableValueChangedEvent)
				{
					i_delHandler();
				}
			};
			a_Variable.ValueChanged += delHandler;

			if (!m_fdcGroupEvent.Contains(a_Variable))
			{
				a_Variable.RefreshTime = 100;
				m_fdcGroupEvent.Add(a_Variable);
				a_Variable.Active = true;
			}
			return EDC_Disposable.FUN_fdcCreate(delegate
			{
				a_Variable.ValueChanged -= delHandler;
			});
		}

        /// <summary>
        /// 读取string值
        /// Read Value
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <returns></returns>
		public string Fun_strReadValue(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return string.Empty;
			}
			Variable variable = Fun_edcGetVariableFromCpu(i_strVarName);
			variable.ReadValue(true);
			return variable.Value.ToString(CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// 读取i32值
		/// </summary>
		/// <param name="i_strVarName"></param>
		/// <returns></returns>
		public int Fun_i32ReadValue(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0;
			}
			return Fun_edcGetVariableFromCpu(i_strVarName).Value.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
		}
        /// <summary>
        /// Read Value u32
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <returns></returns>
		public uint Fun_u32ReadValue(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0u;
			}
			return Fun_edcGetVariableFromCpu(i_strVarName).Value.ToUInt32(CultureInfo.InvariantCulture.NumberFormat);
		}

		public float Fun_sngReadValue(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0f;
			}
			return Fun_edcGetVariableFromCpu(i_strVarName).Value.ToSingle(CultureInfo.InvariantCulture.NumberFormat);
		}

		public short Fun_i16ReadValue(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0;
			}
			return Fun_edcGetVariableFromCpu(i_strVarName).Value.ToInt16(CultureInfo.InvariantCulture.NumberFormat);
		}
		public ushort Fun_u16ReadValue(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0;
			}
			return Fun_edcGetVariableFromCpu(i_strVarName).Value.ToUInt16(CultureInfo.InvariantCulture.NumberFormat);
		}
		public byte Fun_bytReadValue(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return 0;
			}
			return Fun_edcGetVariableFromCpu(i_strVarName).Value.ToByte(CultureInfo.InvariantCulture.NumberFormat);
		}
		public bool Fun_blnReadValue(string i_strVarName)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				return false;
			}
			return Fun_edcGetVariableFromCpu(i_strVarName).Value.ToBoolean(CultureInfo.InvariantCulture.NumberFormat);
		}

        /// <summary>
        /// Write Value
        /// 写值
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <param name="i_strWert"></param>
		public void Sub_WriteValue(string i_strVarName, string i_strWert)
		{
			if (base.PRO_blnIstDisposed || !PRO_blnVerbunden)
			{
				throw new InvalidOperationException("Cannot write if not connected");
			}
			Variable variable = Fun_edcGetVariableFromCpu(i_strVarName);
			FUN_strWertTypGerechtSchreiben(i_strWert, variable);
            // 同步 写值
			variable.WriteValue(true);
		}

        /// <summary>
        /// 注册Group
        /// </summary>
		public void Sub_GroupEventActive()
		{
			if (m_fdcGroupEvent != null && !m_fdcGroupEvent.Active && m_fdcGroupEvent.Count > 0)
			{
				m_fdcGroupEvent.Active = true;
			}
		}

		public void Sub_GroupEventDisactivate()
		{
			VariableCollection fdcEventGruppe = m_fdcGroupEvent;
		}

        /// <summary>
        /// 关闭连接
        /// </summary>
		public void Sub_DisConnect()
		{
			if (PRO_blnVerbunden)
			{
				m_fdcCpu?.Disconnect();
				m_fdcService?.Disconnect();
			}
		}

        /// <summary>
        /// 创建Group变量 
        /// Create Group Variable Async
        /// </summary>
        /// <param name="i_enmVariablen">变量枚举</param>
        /// <param name="i_strGroupName">Group Name</param>
        /// <param name="i_i32CycleTime">CT</param>
        /// <returns></returns>
		public async System.Threading.Tasks.Task Fun_fdcGroupCreateVariableAsync(IEnumerable<string> i_enmVariablen, string i_strGroupName, int i_i32CycleTime = 100)
		{
			List<string> lstVariablen = i_enmVariablen.ToList();
			if (string.IsNullOrEmpty(i_strGroupName) || !lstVariablen.Any())
			{
				throw new ArgumentNullException("i_strGroupName");
			}
			if (PRO_blnVerbunden)
			{
				try
				{
					// 获得组 或 创建组
					await m_fdcGroupSemaphore.WaitAsync().ConfigureAwait(true);
					VariableCollection variableCollection = Fun_fdcGetGroup(i_strGroupName);
					if (variableCollection == null)
					{
						variableCollection = FUN_fdcGruppeAnlegen(i_strGroupName, i_i32CycleTime);
						variableCollection.CollectionValuesRead += (sender, arg) =>
						{
							VariableCollection variableCollection3 = sender as VariableCollection;
							if (variableCollection3 != null && variableCollection3.Name.Equals(i_strGroupName))
							{
								List<EDC_SpsListenElement> list = new List<EDC_SpsListenElement>();
								foreach (Variable value2 in arg.Objects.Values)
								{
									EDC_SpsListenElement item = new EDC_SpsListenElement
									{
										PRO_strGruppenName = i_strGroupName,
										PRO_strVariable = value2.Address,
										PRO_objWert = value2.Value
									};
									list.Add(item);
								}
								m_dicGruppenReadCompletionSources.TryGetValue(i_strGroupName, out TaskCompletionSource<IEnumerable<EDC_SpsListenElement>> value);
								value?.TrySetResult(list);
							}
						};
						variableCollection.CollectionValuesWritten += (sender, arg) =>
						{
							VariableCollection variableCollection2 = sender as VariableCollection;
							if (variableCollection2 != null && variableCollection2.Name.Equals(i_strGroupName))
							{
								m_fdcGruppeWriteCompletionSource.SetResult(true);
							}
						};
						variableCollection.CollectionConnected += SUB_GruppeConnectionChanged;
						variableCollection.CollectionDisconnected += SUB_GruppeConnectionChanged;
					}
					variableCollection.Disconnect();

					// 添加变量
					foreach (string item2 in lstVariablen)
					{
						string text = FUN_strNamenKorrigieren(item2);
						variableCollection.Add(m_fdcCpu.Variables[text] ?? Fun_fdcCreateNewCpuVariable(text, i_i32CycleTime));
					}

					// 激活
					m_fdcGroupConnectedCompletionSource = new TaskCompletionSource<bool>();
					variableCollection.Active = true;
					variableCollection.Connect();
					await m_fdcGroupConnectedCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(true);
				}
				finally
				{
					m_fdcGroupSemaphore.Release();
				}
			}
		}

        /// <summary>
        /// Group 写 异步
        /// </summary>
        /// <param name="i_enmParameter"></param>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		public async System.Threading.Tasks.Task FUN_fdcGruppeSchreibenAsync(IEnumerable<KeyValuePair<string, string>> i_enmParameter, string i_strGroupName)
		{
			// cpu连接
			if (PRO_blnVerbunden)
			{
				// 获得组
				VariableCollection fdcGruppe = Fun_fdcGetGroup(i_strGroupName);
				if (fdcGruppe == null)
				{
					throw new EDC_GruppeZugriffException("B&R sps group " + i_strGroupName + " does not exist");
				}

				List<KeyValuePair<string, string>> lstFehler = new List<KeyValuePair<string, string>>();
				foreach (KeyValuePair<string, string> item in i_enmParameter.ToList())
				{
					Variable variable = Fun_edcGetVariableFromCpu(item.Key);
					if (variable == null)
					{
						lstFehler.Add(new KeyValuePair<string, string>(item.Key, "not registered or not in B&R sps group " + i_strGroupName));
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
					await m_fdcGroupSemaphore.WaitAsync().ConfigureAwait(true);
					m_fdcGruppeWriteCompletionSource = new TaskCompletionSource<bool>();
					fdcGruppe.WriteValues();
					await m_fdcGruppeWriteCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(false);
					if (lstFehler.Any())
					{
						foreach (KeyValuePair<string, string> item2 in lstFehler)
						{
							string i_strEintrag = "B&R PVI error writing variable: " + item2.Key + " cause: " + item2.Value;
							Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
							PRO_edcLogger?.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, i_strEintrag, reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name);
						}
						throw new EDC_GruppeZugriffException("B&R PVI error writing group: " + i_strGroupName);
					}
				}
				finally
				{
					m_fdcGroupSemaphore.Release();
				}
			}
		}

        /// <summary>
        /// Group 读 异步
        /// group Reading Async
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		public async Task<IEnumerable<EDC_SpsListenElement>> Fun_fdcGroupReadAsync(string i_strGroupName)
		{
			if (string.IsNullOrEmpty(i_strGroupName))
			{
				throw new ArgumentNullException("i_strGroupName");
			}
			if (!PRO_blnVerbunden)
			{
				return Enumerable.Empty<EDC_SpsListenElement>();
			}
			VariableCollection fdcGruppe = Fun_fdcGetGroup(i_strGroupName);
			if (fdcGruppe != null)
			{
				try
				{
					await m_fdcGroupSemaphore.WaitAsync().ConfigureAwait(true);

					// 每个组都建立一个TaskCompletionSource
					TaskCompletionSource<IEnumerable<EDC_SpsListenElement>> taskCompletionSource = new TaskCompletionSource<IEnumerable<EDC_SpsListenElement>>();
					// 赋值全局变量
					m_dicGruppenReadCompletionSources[i_strGroupName] = taskCompletionSource;
					
					fdcGruppe.ReadValues();
					
					return await taskCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(true);
				}
				finally
				{
					m_fdcGroupSemaphore.Release();
				}
			}
			throw new EDC_GruppeZugriffException("B&R sps group " + i_strGroupName + " does not exist");
		}

		/// <summary>
		/// Group 激活
		/// </summary>
		/// <param name="i_strGroupName"></param>
		/// <returns></returns>
		public async System.Threading.Tasks.Task Fun_fdcGroupActiveAsync(string i_strGroupName)
		{
			if (string.IsNullOrEmpty(i_strGroupName))
			{
				throw new ArgumentNullException("i_strGroupName");
			}
			if (PRO_blnVerbunden)
			{
				try
				{
					// 只能由一个线程访问plc , 可以更换线程
					await m_fdcGroupSemaphore.WaitAsync().ConfigureAwait(false);

					// 获得组
					VariableCollection variableCollection = Fun_fdcGetGroup(i_strGroupName);
					if (variableCollection == null)
					{
						throw new EDC_GruppeZugriffException("B&R sps group " + i_strGroupName + " does not exist");
					}

					m_fdcGroupConnectedCompletionSource = new TaskCompletionSource<bool>();
					variableCollection.Active = true;
					variableCollection.Connect();

					// 等待5秒后继续之前的线程
					await m_fdcGroupConnectedCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(true);
				}
				finally
				{
					m_fdcGroupSemaphore.Release();
				}
			}
		}

		public async System.Threading.Tasks.Task FUN_fdcGroupDisableAsync(string i_strGroupName)
		{
			if (string.IsNullOrEmpty(i_strGroupName))
			{
				throw new ArgumentNullException("i_strGroupName");
			}
			if (PRO_blnVerbunden)
			{
				try
				{
					await m_fdcGroupSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
					VariableCollection variableCollection = Fun_fdcGetGroup(i_strGroupName);
					if (variableCollection == null)
					{
						throw new EDC_GruppeZugriffException("B&R sps group " + i_strGroupName + " does not exist");
					}
					m_fdcGroupConnectedCompletionSource = new TaskCompletionSource<bool>();
					variableCollection.Active = false;
					variableCollection.Disconnect();
					await m_fdcGroupConnectedCompletionSource.Task.FUN_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
				}
				finally
				{
					m_fdcGroupSemaphore.Release();
				}
			}
		}

		protected override void SUB_InternalDispose()
		{
			SUB_Zerstoeren();
		}

        /// <summary>
        /// Create Group
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <param name="i_i32CycleTime"></param>
        /// <returns></returns>
		private VariableCollection FUN_fdcGruppeAnlegen(string i_strGroupName, int i_i32CycleTime)
		{
			VariableCollection variableCollection = new VariableCollection(m_fdcCpu, i_strGroupName);
			variableCollection.CollectionError += SUB_PviCollectionError;
			variableCollection.RefreshTime = i_i32CycleTime;

			m_lstGroup.Add(variableCollection);
			return variableCollection;
		}

        /// <summary>
        /// Get Group
        /// </summary>
        /// <param name="i_strGroupName"></param>
        /// <returns></returns>
		private VariableCollection Fun_fdcGetGroup(string i_strGroupName)
		{
			return m_lstGroup.FirstOrDefault(s => s.Name.Equals(i_strGroupName));
		}

		/// <summary>
		/// Group 连接状态改变
		/// </summary>
		/// <param name="i_objSender"></param>
		/// <param name="i_fdcCollectionEventArgs"></param>
		private void SUB_GruppeConnectionChanged(object i_objSender, CollectionEventArgs i_fdcCollectionEventArgs)
		{
			VariableCollection variableCollection = i_objSender as VariableCollection;
			if (variableCollection != null)
			{
				string arg = variableCollection.Active ? "activated" : "deactivated";
				SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, $"{variableCollection.Name} was {arg}. Collection-count: {variableCollection.Count}", "SUB_GruppeConnectionChanged");
				m_fdcGroupConnectedCompletionSource?.TrySetResult(result: true);
			}
		}

        /// <summary>
        /// Remove variable from group
        /// </summary>
        /// <param name="i_fdcGruppe"></param>
        /// <param name="i_strVariable"></param>
		private void SUB_VariableAusGruppeEntfernen(VariableCollection i_fdcGruppe, string i_strVariable)
		{
			Variable variable = i_fdcGruppe.Contains(i_strVariable) ? m_fdcGroupEvent[i_strVariable] : null;
			if (variable != null)
			{
				i_fdcGruppe.Remove(variable);
			}
		}

        /// <summary>
        /// 注销
        /// </summary>
		private void SUB_Zerstoeren()
		{
			if (m_fdcGroupEvent != null)
			{
				m_fdcGroupEvent.Error -= SUB_EventGruppeError;
				SUB_GruppeDispose(m_fdcGroupEvent);
			}
			if (m_fdcGroup != null)
			{
				SUB_GruppeDispose(m_fdcGroup);
			}
			if (m_fdcGroupTemp != null)
			{
				Sub_GroupEventsDeRegister(m_fdcGroupTemp);
				SUB_GruppeDispose(m_fdcGroupTemp);
			}
			foreach (VariableCollection item in m_lstGroup)
			{
				item.CollectionConnected -= SUB_GruppeConnectionChanged;
				item.CollectionDisconnected -= SUB_GruppeConnectionChanged;
				SUB_GruppeDispose(item);
			}
			m_lstGroup.Clear();
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

        /// <summary>
        /// 正确的名字 去除字符前面的SELECTIV/.
        /// </summary>
        /// <param name="i_strVar"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 注册组
        /// </summary>
		private void Sub_InitializeGroup()
        {
            m_lstGroup.Clear();

            m_fdcGroup = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString()) { RefreshTime = 400 };

            m_fdcGroupTemp = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
			// Group Temp注册事件
			Sub_GroupEventsRegister(m_fdcGroupTemp);

            m_fdcGroupEvent = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
            m_fdcGroupEvent.Error += SUB_EventGruppeError;
            m_fdcGroupEvent.CollectionPropertyChanged += Sub_EventGroupPropertyChanged;
            m_fdcGroupEvent.RefreshTime = 100;
        }

		private void Sub_EventGroupPropertyChanged(object i_objSender, CollectionEventArgs i_fdcEventArgse)
		{
			SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, "e.Objects.Count =" + i_fdcEventArgse.Objects.Count, "SUB_EventGruppePropertyChanged");
		}

		/// <summary>
		/// 组事件错误
		/// </summary>
		/// <param name="i_objSender"></param>
		/// <param name="i_fdcEventArgs"></param>
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
				m_fdcConnectionCompletionSource.TrySetResult(true);
			}
		}

		private void SUB_LogEintragSchreiben(ENUM_LogLevel i_enmLogLevel, string i_strEintrag, string i_strMethodenName = null, System.Exception i_excException = null)
		{
			if (PRO_edcLogger != null)
			{
				PRO_edcLogger.SUB_LogEintragSchreiben(i_enmLogLevel, i_strEintrag, "Ersa.Platform.Plc", "EDC_BrSps", i_strMethodenName, i_excException);
			}
		}

        /// <summary>
        /// 已应用变量
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <returns></returns>
		private bool FUN_blnVariableSchonAngelegt(string i_strVarName)
		{
			if (string.IsNullOrWhiteSpace(i_strVarName))
			{
				return false;
			}
			if (Fun_edcGetVariableFromCpu(i_strVarName) == null)
			{
				return false;
			}
			return true;
		}

        /// <summary>
        /// Write Value Type 
        /// </summary>
        /// <param name="i_strWert">Value</param>
        /// <param name="i_fdcVar">Variable</param>
        /// <returns></returns>
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

        /// <summary>
        /// 从cpu获取item
        /// Get Item From Cpu
        /// </summary>
        /// <param name="i_strVarName"></param>
        /// <returns></returns>
		private Variable Fun_edcGetVariableFromCpu(string i_strVarName)
		{
			if (!PRO_blnVerbunden)
			{
				return null;
			}
			string name = FUN_strNamenKorrigieren(i_strVarName);
			return m_fdcCpu.Variables[name];
		}

		/// <summary>
		/// 获得组
		/// Get Item From Group Event
		/// </summary>
		/// <param name="i_strVarName"></param>
		/// <returns></returns>
		private Variable FUN_fdcItemAusEventGruppeHolen(string i_strVarName)
		{
			string text = FUN_strNamenKorrigieren(i_strVarName);
			if (!m_fdcGroupEvent.Contains(text))
			{
				return null;
			}
			return m_fdcGroupEvent[text];
		}

        /// <summary>
        /// Fill Group
        /// </summary>
        /// <param name="i_lstVariablen"></param>
        /// <param name="i_fdcVarCollection"></param>
        /// <param name="i_fdcToken"></param>
		private void Sub_GroupFill(IEnumerable<string> i_lstVariablen, VariableCollection i_fdcVarCollection, CancellationToken i_fdcToken)
		{
			foreach (string item in i_lstVariablen)
			{
				i_fdcToken.ThrowIfCancellationRequested();

				Variable variable = Fun_fdcCreateNewCpuVariable(item, 400);
				i_fdcVarCollection.Add(variable);
			}
			i_fdcVarCollection.Connect();
		}

        /// <summary>
        /// 创建新的cpu变量
        /// Create new Cpu variable
        /// </summary>
        /// <param name="i_strVariable">变量</param>
        /// <param name="i_i32CycleTime">CT</param>
        /// <returns>Variable</returns>
        private Variable Fun_fdcCreateNewCpuVariable(string i_strVariable, int i_i32CycleTime)
		{
			string name = FUN_strNamenKorrigieren(i_strVariable);
			Variable variable = new Variable(m_fdcCpu, name)
			{
				Polling = false
			};
			variable.Access |= (Access.Read | Access.Write | Access.FASTECHO);
			variable.RefreshTime = i_i32CycleTime;
			variable.RuntimeObjectIndex = Variable.ROIoptions.NonZeroBasedArrayIndex;
			return variable;
		}

        /// <summary>
        /// 移动Variable
        /// Move old variables to new group
        /// </summary>
        /// <param name="i_fdcNeuGruppe"></param>
        /// <param name="i_fdcAlteGruppe"></param>
		private void SUB_AlteVariablenInNeueGruppeVerschieben(VariableCollection i_fdcNeuGruppe, VariableCollection i_fdcAlteGruppe)
		{
			foreach (object item in i_fdcNeuGruppe)
			{
				i_fdcNeuGruppe.Add(item as Variable);
			}
			i_fdcAlteGruppe.Clear();
		}

        /// <summary>
        /// Service+CPU
        /// </summary>
		private void SUB_InitPvi()
		{
			m_fdcService = new Service("Service_" + DateTime.Now.ToLongTimeString() + DateTime.Now.Millisecond);
			m_fdcService.Connected += SUB_ServiceConnected;
			m_fdcService.Error += SUB_PviError;
			m_fdcCpu = new Cpu(m_fdcService, "Cpu");
			m_fdcCpu.Error += SUB_PviError;
			m_fdcCpu.Connected += SUB_CpuConnected;
		}

        /// <summary>
        /// Group 事件注册
        /// Register group events
        /// </summary>
        /// <param name="i_fdcGruppe"></param>
		private void Sub_GroupEventsRegister(VariableCollection i_fdcGruppe)
		{
			i_fdcGruppe.Error += SUB_PviCollectionError;
			i_fdcGruppe.CollectionConnected += SUB_CollectionConnected;
			i_fdcGruppe.CollectionValuesRead += Sub_GroupValuesRead;
		}

        /// <summary>
        /// Group 事件注销
        /// </summary>
        /// <param name="i_fdcGruppe"></param>
		private void Sub_GroupEventsDeRegister(VariableCollection i_fdcGruppe)
		{
			i_fdcGruppe.Error -= SUB_PviCollectionError;
			i_fdcGruppe.CollectionConnected -= SUB_CollectionConnected;
			i_fdcGruppe.CollectionValuesRead -= Sub_GroupValuesRead;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i_objSender"></param>
		/// <param name="i_fdcCollectionEventArgs"></param>
		private void Sub_GroupValuesRead(object i_objSender, CollectionEventArgs i_fdcCollectionEventArgs)
		{
			VariableCollection variableCollection = i_objSender as VariableCollection;
			if (variableCollection != null)
			{
				SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, $"{variableCollection.Name} read: {variableCollection.Count} variables", "SUB_CollectionValuesRead");
			}
			m_fdcVariableCompletionSource.SetResult(true);
		}

        /// <summary>
        /// Collection Connected
        /// </summary>
        /// <param name="i_objSender"></param>
        /// <param name="i_fdcCollectionEventArgs"></param>
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

        /// <summary>
        /// Collection Error
        /// </summary>
        /// <param name="i_objSender"></param>
        /// <param name="i_fdcPviEventArgs"></param>
		private void SUB_PviCollectionError(object i_objSender, PviEventArgs i_fdcPviEventArgs)
		{
			string i_strEintrag = "Collection Error: " + i_fdcPviEventArgs.Address + " -->" + i_fdcPviEventArgs.ErrorText;
			SUB_LogEintragSchreiben(ENUM_LogLevel.enmSpsKommunkation, i_strEintrag, "SUB_PviCollectionError");
			if (i_fdcPviEventArgs.Action != BR.AN.PviServices.Action.VariableDisconnect)
			{
				SUB_FehlerhafteVariableMerken(i_fdcPviEventArgs.Name);
			}
		}

        /// <summary>
        /// 添加不正确的变量
        /// </summary>
        /// <param name="i_strVarName"></param>
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
				m_fdcConnectionCompletionSource.TrySetException(new EDC_SpsVerbindungsAufbauFehlgeschlagenException(text));
			}
		}
	}
}
