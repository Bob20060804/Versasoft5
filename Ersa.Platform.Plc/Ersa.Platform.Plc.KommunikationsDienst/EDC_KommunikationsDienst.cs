using Ersa.Global.Common;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Logging;
using Ersa.Platform.Plc.Exceptions;
using Ersa.Platform.Plc.Interfaces;
using Ersa.Platform.Plc.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc.KommunikationsDienst
{
    /// <summary>
    /// Communication Service
    /// </summary>
	[Export]
	public class EDC_KommunikationsDienst : EDC_DisposableObject, Inf_CommunicationService, IDisposable
	{
        #region ==== Parameter ====
        /// <summary>
        /// 
        /// </summary>
        private readonly INF_SpsProvider m_edcSpsProvider;

        private INF_Sps m_edcSpsService;
        private INF_Sps PRO_edcSpsService => m_edcSpsService ?? (m_edcSpsService = m_edcSpsProvider.Fun_edcActiveSps());


        private readonly INF_Logger m_edcLogger;

        private const int mC_i32LockTimeout = 60000;

		private static readonly SemaphoreSlim m_fdcSemaphore = new SemaphoreSlim(1);

        /// <summary>
        /// 读操作
        /// Reading operations
        /// </summary>
		private readonly Dictionary<EDC_PrimitivParameter, Action<EDC_PrimitivParameter>> m_dicLeseOperationen;
        /// <summary>
        /// 写操作
        /// Writing operations
        /// </summary>
		private readonly Dictionary<EDC_PrimitivParameter, Action<EDC_PrimitivParameter>> m_dicSchreibeOperationen;
        /// <summary>
        /// 事件描述
        /// EventHandle Subscriptions
        /// </summary>
		private readonly IDictionary<EDC_PrimitivParameter, IDisposable> m_dicEventHandlerSubscriptions;

		/// <summary>
		/// 读 变量 策略
		/// parameter read strategy
		/// </summary>
		private readonly EDC_ParameterLeseStrategie m_edcLeseStrategie;
        /// <summary>
        /// 注册地址策略
        /// </summary>
		private readonly EDC_AdressRegistrierungsStrategie m_edcAdressRegistrierungsStrategie;
        /// <summary>
        /// 注册事件策略
        /// </summary>
		private readonly EDC_EventHandlerRegistrierungsStrategie m_edcEvenHandlerRegStrategie;
        /// <summary>
        /// 参数写策略
        /// </summary>
		private readonly EDC_ParameterSchreibeStrategie m_edcSchreibeStrategie;

		

        /// <summary>
        /// Lock Connection 
        /// </summary>
		private readonly object m_objLockConnection = new object();
        /// <summary>
        /// Lock Reading 
        /// </summary>
		private readonly object m_objLockReading = new object();
        /// <summary>
        /// Lock Write 
        /// </summary>
		private readonly object m_objLockWrite = new object();
		/// <summary>
		/// Group
		/// </summary>
		private readonly Dictionary<string, IEnumerable<EDC_PrimitivParameter>> m_dicGroupParameter = new Dictionary<string, IEnumerable<EDC_PrimitivParameter>>();

		
        /// <summary>
        /// 是否解决连接
        /// </summary>
		private bool m_blnIstVerbindungGeloest;



        [Import("Ersa.Platform.Plc.KommunikationsDienst.EDC_PrimaerSekundaerModusSteuerungsDienst.FUN_blnIstVisuAlsPrimaerAngemeldet")]
		public Lazy<Func<bool>> PRO_delIstVisuAlsPrimaerAngemeldet
		{
			get;
			set;
		}
		#endregion ============


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="i_edcLeseStrategie">读取参数策略</param>
		/// <param name="i_edcAdressRegistrierungsStrategie">注册地址策略</param>
		/// <param name="i_edcEvenHandlerRegStrategie">注册事件策略</param>
		/// <param name="i_edcSchreibeStrategie">参数写入策略</param>
		/// <param name="i_edcSpsProvider"></param>
		/// <param name="i_edcLogger"></param>
		[ImportingConstructor]
		public EDC_KommunikationsDienst(EDC_ParameterLeseStrategie i_edcLeseStrategie, EDC_AdressRegistrierungsStrategie i_edcAdressRegistrierungsStrategie, EDC_EventHandlerRegistrierungsStrategie i_edcEvenHandlerRegStrategie, EDC_ParameterSchreibeStrategie i_edcSchreibeStrategie, INF_SpsProvider i_edcSpsProvider, INF_Logger i_edcLogger)
		{
			m_edcLeseStrategie = i_edcLeseStrategie;
			m_edcAdressRegistrierungsStrategie = i_edcAdressRegistrierungsStrategie;
			m_edcEvenHandlerRegStrategie = i_edcEvenHandlerRegStrategie;
			m_edcSchreibeStrategie = i_edcSchreibeStrategie;
			m_edcSpsProvider = i_edcSpsProvider;
			m_edcLogger = i_edcLogger;

			// 读取参数策略
			m_dicLeseOperationen = new Dictionary<EDC_PrimitivParameter, Action<EDC_PrimitivParameter>>(new EDC_PrimitivParameterEqualityComparer());
			// 写策略
			m_dicSchreibeOperationen = new Dictionary<EDC_PrimitivParameter, Action<EDC_PrimitivParameter>>(new EDC_PrimitivParameterEqualityComparer());
			// 事件描述
			m_dicEventHandlerSubscriptions = new Dictionary<EDC_PrimitivParameter, IDisposable>();
		}


        /// <summary>
        /// 建立控制连接
        /// Establish connection to control
        /// </summary>
        /// <param name="i_blnOnline"></param>
        /// <returns></returns>
		public async Task Fun_fdcConnect(bool i_blnOnline)
		{
			try
			{
				m_dicGroupParameter.Clear();
				await PRO_edcSpsService.Fun_ConnectAsync(i_blnOnline, EDC_KommunikationsHelfer.PRO_strSpsIpAdresse).ConfigureAwait(true);
				lock (m_objLockConnection)
				{
					m_blnIstVerbindungGeloest = false;
				}
			}
			catch (Exception i_fdcEx)
			{
				SUB_FehlerLoggen("Error connecting to the PLC.", "FUN_fdcVerbindungZurSteuerungAufbauen", i_fdcEx);
				throw;
			}
		}

        /// <summary>
        /// 注销 连接
        /// </summary>
		public void SUB_VerbindungLoesen()
		{
			try
			{
				lock (m_objLockConnection)
				{
					m_blnIstVerbindungGeloest = true;
				}
				SUB_EventHandlerDicValueDispose();
				PRO_edcSpsService.Sub_DisConnect();
				m_dicGroupParameter.Clear();
			}
			catch (Exception i_fdcEx)
			{
				SUB_FehlerLoggen("Error disconnecting from the PLC.", "SUB_VerbindungLoesen", i_fdcEx);
				throw;
			}
		}

        /// <summary>
        /// Read Value
        /// </summary>
        /// <param name="i_edcParameter"></param>
		public void Sub_ReadValue(EDC_PrimitivParameter i_edcParameter)
		{
            // 建立连接
			if (FUN_blnIstVerbindungAufgebaut())
			{
				try
				{
					Action<EDC_PrimitivParameter> value;
					lock (m_objLockReading)
					{
						if (!m_dicLeseOperationen.TryGetValue(i_edcParameter, out value))
						{
							Action<EDC_PrimitivParameter> action = FUN_objParameterBehandlung(i_edcParameter, m_edcLeseStrategie);
							// 添加方法
							m_dicLeseOperationen.Add(i_edcParameter, action);
							value = action;
						}
					}
					value(i_edcParameter);
				}
				catch (Exception i_fdcEx)
				{
					string i_strEintrag = string.Format("Error reading value from the PLC. Variable: {0}, Value: {1}", (i_edcParameter == null) ? "null" : i_edcParameter.PRO_strAdresse, (i_edcParameter == null) ? "null" : i_edcParameter.PRO_objValue);
					SUB_FehlerLoggen(i_strEintrag, "SUB_WertLesen", i_fdcEx);
					throw;
				}
			}
		}

        /// <summary>
        /// Read Value Async
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		public Task Fun_fdcReadValueAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken)
		{
			return Task.Run(()=>
			{
				foreach (EDC_PrimitivParameter item in i_lstPrimitivParameter)
				{
					i_fdcCancellationToken.ThrowIfCancellationRequested();
					Sub_ReadValue(item);
				}
			}, i_fdcCancellationToken);
		}

        /// <summary>
        /// Write Value
        /// </summary>
        /// <param name="i_edcParameter"></param>
		public void SUB_WertSchreiben(EDC_PrimitivParameter i_edcParameter)
		{
			if (FUN_blnIstVerbindungAufgebaut() && PRO_delIstVisuAlsPrimaerAngemeldet.Value())
			{
				try
				{
					Action<EDC_PrimitivParameter> value;
					lock (m_objLockWrite)
					{
						if (!m_dicSchreibeOperationen.TryGetValue(i_edcParameter, out value))
						{
							Action<EDC_PrimitivParameter> action = FUN_objParameterBehandlung(i_edcParameter, m_edcSchreibeStrategie);
							m_dicSchreibeOperationen.Add(i_edcParameter, action);
							value = action;
						}
					}
					value(i_edcParameter);
				}
				catch (Exception i_fdcEx)
				{
					string i_strEintrag = string.Format("Error writing value to the PLC. Variable: {0}, Value: {1}", (i_edcParameter == null) ? "null" : i_edcParameter.PRO_strAdresse, (i_edcParameter == null) ? "null" : i_edcParameter.PRO_objValue);
					SUB_FehlerLoggen(i_strEintrag, "SUB_WertSchreiben", i_fdcEx);
					throw;
				}
			}
		}
        /// <summary>
        /// Write Value Async
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		public Task FUN_fdcWerteSchreibenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken)
		{
			return Task.Run(delegate
			{
				foreach (EDC_PrimitivParameter item in i_lstPrimitivParameter)
				{
					i_fdcCancellationToken.ThrowIfCancellationRequested();
					SUB_WertSchreiben(item);
				}
			}, i_fdcCancellationToken);
		}

        /// <summary>
        /// 物理地址存储 异步
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		public async Task FUN_fdcPhysischeAdressenRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancellationToken)
		{
			if (FUN_blnIstVerbindungAufgebaut())
			{
				IList<EDC_PrimitivParameter> lstPrimitivParameter = (i_lstPrimitivParameter as IList<EDC_PrimitivParameter>) ?? i_lstPrimitivParameter.ToList();
				try
				{
					try
					{
						if (!await m_fdcSemaphore.WaitAsync(60000, i_fdcCancellationToken).ConfigureAwait(true))
						{
							throw new InvalidOperationException("Error registering variables. Another registration is in progress and the timeout expired.");
						}
						IEnumerable<string> i_lstVariablen = FUN_lstPhysischeAdressenHolen(lstPrimitivParameter, i_fdcCancellationToken);
						await PRO_edcSpsService.Sub_VariablesRegisterAsync(i_lstVariablen, i_fdcCancellationToken).ConfigureAwait(true);
					}
					finally
					{
						m_fdcSemaphore.Release();
					}
				}
				catch (Exception i_fdcEx)
				{
					SUB_FehlerLoggen("Error registering variables at the PLC. Variables: " + FUN_strParameterAusgabeErstellen(lstPrimitivParameter), "FUN_fdcPhysischeAdressenRegistrierenAsync", i_fdcEx);
					throw;
				}
			}
		}

        /// <summary>
        /// 物理地址注销 异步
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		public async Task FUN_fdcPhysischeAdressenDeRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken)
		{
			if (FUN_blnIstVerbindungAufgebaut())
			{
				IList<EDC_PrimitivParameter> lstPrimitivParameter = (i_lstPrimitivParameter as IList<EDC_PrimitivParameter>) ?? i_lstPrimitivParameter.ToList();
				try
				{
					await PRO_edcSpsService.Sub_VariablesUnregister(FUN_lstPhysischeAdressenHolen(lstPrimitivParameter), i_fdcToken).ConfigureAwait(continueOnCapturedContext: true);
					foreach (EDC_PrimitivParameter item in lstPrimitivParameter)
					{
						lock (m_objLockReading)
						{
							if (m_dicLeseOperationen.ContainsKey(item))
							{
								m_dicLeseOperationen.Remove(item);
							}
						}
						lock (m_objLockWrite)
						{
							if (m_dicSchreibeOperationen.ContainsKey(item))
							{
								m_dicSchreibeOperationen.Remove(item);
							}
						}
					}
				}
				catch (Exception i_fdcEx)
				{
					SUB_FehlerLoggen("Error deregistering variables from the PLC. Variables: " + FUN_strParameterAusgabeErstellen(lstPrimitivParameter), "FUN_fdcPhysischeAdressenDeRegistrierenAsync", i_fdcEx);
					throw;
				}
			}
		}

        /// <summary>
		/// BR plc 注册事件
        /// sps envent handle register
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		public Task FUN_fdcSPSEventHandlerRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken)
		{
			EDC_PrimitivParameter edcParameter;
			return Task.Run(delegate
			{
				if (FUN_blnIstVerbindungAufgebaut())
				{
					IList<EDC_PrimitivParameter> list = (i_lstPrimitivParameter as IList<EDC_PrimitivParameter>) ?? i_lstPrimitivParameter.ToList();
					try
					{
						PRO_edcSpsService.Sub_GroupEventDisactivate();
						foreach (EDC_PrimitivParameter item in list)
						{
							i_fdcToken.ThrowIfCancellationRequested();
							if (string.IsNullOrEmpty(item.PRO_strPhysischeAdresse))
							{
								// 处理item 
								item.PRO_strPhysischeAdresse = FUN_objParameterBehandlung(item, m_edcEvenHandlerRegStrategie);
							}
							edcParameter = item;

							IDisposable value = PRO_edcSpsService.Fun_fdcRegisterEventHandler(item.PRO_strPhysischeAdresse, () => Sub_ReadValue(edcParameter));
							m_dicEventHandlerSubscriptions.Add(edcParameter, value);
						}
						PRO_edcSpsService.Sub_GroupEventActive();
					}
					catch (Exception i_fdcEx)
					{
						string i_strEintrag = "Error registering event handlers at the PLC. Variables: " + FUN_strParameterAusgabeErstellen(list);
						SUB_FehlerLoggen(i_strEintrag, "FUN_fdcSPSEventHandlerRegistrierenAsync", i_fdcEx);
						throw;
					}
				}
			}, i_fdcToken);
		}

        /// <summary>
        /// BR PLC 注销事件
        /// sps envent handle Deregister
        /// </summary>
        /// <param name="i_lstPrimitivParameter"></param>
        /// <param name="i_fdcToken"></param>
        /// <returns></returns>
		public Task FUN_fdcSPSEventHandlerDeRegistrierenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcToken)
		{
			return Task.Run(delegate
			{
				if (FUN_blnIstVerbindungAufgebaut())
				{
					IList<EDC_PrimitivParameter> list = (i_lstPrimitivParameter as IList<EDC_PrimitivParameter>) ?? i_lstPrimitivParameter.ToList();
					try
					{
						foreach (EDC_PrimitivParameter item in list)
						{
							i_fdcToken.ThrowIfCancellationRequested();
							if (m_dicEventHandlerSubscriptions.TryGetValue(item, out IDisposable value))
							{
								value.Dispose();
								m_dicEventHandlerSubscriptions.Remove(item);
							}
						}
					}
					catch (Exception i_fdcEx)
					{
						string i_strEintrag = "Error deregistering event handlers from the PLC. Variables: " + FUN_strParameterAusgabeErstellen(list);
						SUB_FehlerLoggen(i_strEintrag, "FUN_fdcSPSEventHandlerDeRegistrierenAsync", i_fdcEx);
						throw;
					}
				}
			}, i_fdcToken);
		}

		public async Task FUN_fdcParameterAbbildAdressenRegistrierenAsync(IEnumerable<string> i_enuAdressen)
		{
			if (FUN_blnIstVerbindungAufgebaut())
			{
				IList<string> lstAdressen = (i_enuAdressen as IList<string>) ?? i_enuAdressen.ToList();
				try
				{
					await PRO_edcSpsService.Sub_VariablesRegisterAsync(lstAdressen, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: true);
				}
				catch (Exception i_fdcEx)
				{
					SUB_FehlerLoggen("Error registering variables at the PLC. Variables: " + string.Join(", ", lstAdressen), "FUN_fdcParameterAbbildAdressenRegistrierenAsync", i_fdcEx);
					throw;
				}
			}
		}

        /// <summary>
        /// parameter image write
        /// </summary>
        /// <param name="i_dicAbbild"></param>
		public void SUB_ParameterAbbildSchreiben(IEnumerable<KeyValuePair<string, object>> i_dicAbbild)
		{
            // 建立连接
			if (FUN_blnIstVerbindungAufgebaut())
			{
				foreach (KeyValuePair<string, object> item in i_dicAbbild)
				{
					try
					{
                        // 写
						PRO_edcSpsService.Sub_WriteValue(item.Key, item.Value.ToString());
					}
					catch (Exception i_fdcEx)
					{
						string i_strEintrag = $"Error writing variable to the PLC. Variable: {item.Key}, Value: {item.Value}";
						SUB_FehlerLoggen(i_strEintrag, "SUB_ParameterAbbildSchreiben", i_fdcEx);
						throw;
					}
				}
			}
		}

		/// <summary>
		/// 在组里创建变量
		/// Create Variable In Group Async
		/// </summary>
		/// <param name="i_lstPrimitivParameter">PLC地址</param>
		/// <param name="i_strGroupName"></param>
		/// <param name="i_i32CycleTime"></param>
		/// <returns></returns>
		public Task FUN_fdcVariablenGruppeErstellenAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGroupName, int i_i32CycleTime = 100)
		{
            // 是否在释放资源
			if (!FUN_blnIstVerbindungAufgebaut())
			{
				return Task.CompletedTask;
			}

			List<EDC_PrimitivParameter> list = i_lstPrimitivParameter.ToList();
			List<string> list2 = new List<string>();
			foreach (EDC_PrimitivParameter item in list)
			{
                // 如果实际地址为空
				if (string.IsNullOrEmpty(item.PRO_strPhysischeAdresse))
				{
					item.PRO_strPhysischeAdresse = FUN_objParameterBehandlung(item, m_edcEvenHandlerRegStrategie);
				}
				list2.Add(item.PRO_strPhysischeAdresse);
			}

			if (!m_dicGroupParameter.ContainsKey(i_strGroupName))
			{
				m_dicGroupParameter.Add(i_strGroupName, list);
			}
			else
			{
				List<EDC_PrimitivParameter> list3 = m_dicGroupParameter[i_strGroupName].ToList();
				foreach (EDC_PrimitivParameter item2 in list)
				{
					if (!list3.Contains(item2))
					{
						list3.Add(item2);
					}
				}
			}
			return m_edcSpsService.Fun_fdcGroupCreateVariableAsync(list2, i_strGroupName, i_i32CycleTime);
		}

		/// <summary>
		/// 组 写值
		/// </summary>
		/// <param name="i_lstPrimitivParameter"></param>
		/// <param name="i_strGroupName"></param>
		/// <returns></returns>
		public Task FUN_fdcGroupValueWriteAsync(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, string i_strGroupName)
		{
			// 没有连接, 结束线程
			if (!FUN_blnIstVerbindungAufgebaut())
			{
				return Task.CompletedTask;
			}
			// 没有这个组
			if (!m_dicGroupParameter.ContainsKey(i_strGroupName))
			{
				throw new EDC_GruppeZugriffException("unknown group " + i_strGroupName);
			}

			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (EDC_PrimitivParameter item in i_lstPrimitivParameter.ToList())
			{
				if (string.IsNullOrEmpty(item.PRO_strPhysischeAdresse))
				{
					item.PRO_strPhysischeAdresse = FUN_objParameterBehandlung(item, m_edcEvenHandlerRegStrategie);
				}
				list.Add(new KeyValuePair<string, string>(item.PRO_strPhysischeAdresse, item.PRO_objValue.ToString()));
			}
			return m_edcSpsService.FUN_fdcGruppeSchreibenAsync(list, i_strGroupName);
		}

		/// <summary>
		/// Group读取异步
		/// </summary>
		/// <param name="i_strGroupName"></param>
		/// <returns></returns>
		public async Task FUN_fdcGruppeLesenAsync(string i_strGroupName)
		{
			// 建立连接
			if (FUN_blnIstVerbindungAufgebaut())
			{
				// Group集合中不存在
				if (!m_dicGroupParameter.ContainsKey(i_strGroupName))
				{
					throw new EDC_GruppeZugriffException("unknown group " + i_strGroupName);
				}

				// 等待获取Group中的所有地址
				List<EDC_SpsListenElement> list = (await m_edcSpsService.Fun_fdcGroupReadAsync(i_strGroupName).ConfigureAwait(true)).ToList();

				if (list.Any() && m_dicGroupParameter.TryGetValue(i_strGroupName, out IEnumerable<EDC_PrimitivParameter> value))
				{
					List<EDC_PrimitivParameter> source = value.ToList();
					foreach (EDC_SpsListenElement edcGelesen in list)
					{
						EDC_PrimitivParameter eDC_PrimitivParameter = source.FirstOrDefault((EDC_PrimitivParameter i_edcItem) => i_edcItem.PRO_strPhysischeAdresse.EndsWith(edcGelesen.PRO_strVariable));
						if (eDC_PrimitivParameter == null)
						{
							throw new EDC_GruppeZugriffException("varibale " + edcGelesen.PRO_strVariable + " not found in registered primitive list");
						}
						if (eDC_PrimitivParameter is EDC_BooleanParameter)
						{
							eDC_PrimitivParameter.PRO_objValue = Convert.ToBoolean(edcGelesen.PRO_objWert);
						}
						else if (eDC_PrimitivParameter is EDC_NumerischerParameter)
						{
							eDC_PrimitivParameter.PRO_objValue = Convert.ToSingle(edcGelesen.PRO_objWert);
						}
						else if (eDC_PrimitivParameter is EDC_StringParameter)
						{
							eDC_PrimitivParameter.PRO_objValue = Convert.ToString(edcGelesen.PRO_objWert);
						}
						else if (eDC_PrimitivParameter is EDC_UIntegerParameter)
						{
							eDC_PrimitivParameter.PRO_objValue = Convert.ToUInt32(edcGelesen.PRO_objWert);
						}
						else
						{
							eDC_PrimitivParameter.PRO_objValue = Convert.ToInt32(edcGelesen.PRO_objWert);
						}
					}
				}
			}
		}

		/// <summary>
		/// 激活Group 异步
		/// </summary>
		/// <param name="i_strGroupName"></param>
		/// <returns></returns>
		public Task FUN_fdcGruppeAktivierenAsync(string i_strGroupName)
		{
			if (!FUN_blnIstVerbindungAufgebaut())
			{
				return Task.CompletedTask;
			}
			return m_edcSpsService.Fun_fdcGroupActiveAsync(i_strGroupName);
		}

		/// <summary>
		/// 禁用组 异步
		/// </summary>
		/// <param name="i_strGroupName"></param>
		/// <returns></returns>
		public Task FUN_fdcGruppeDeaktivierenAsync(string i_strGroupName)
		{
			if (!FUN_blnIstVerbindungAufgebaut())
			{
				return Task.CompletedTask;
			}
			return m_edcSpsService.FUN_fdcGroupDisableAsync(i_strGroupName);
		}

		protected override void SUB_InternalDispose()
		{
			try
			{
				PRO_edcSpsService.Dispose();
			}
			catch (Exception i_fdcEx)
			{
				SUB_FehlerLoggen("Error disposing the object", "SUB_InternalDispose", i_fdcEx);
				throw;
			}
		}

        /// <summary>
        /// 处理参数
        /// Handling Parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i_edcParameter"></param>
        /// <param name="i_edcStrategie"></param>
        /// <returns></returns>
		private T FUN_objParameterBehandlung<T>(EDC_PrimitivParameter i_edcParameter, INF_ParameterBehandlungsStrategie<T> i_edcStrategie)
		{
			string a = (i_edcParameter.PROa_objAdresse.Length != 1) ? i_edcParameter.PROa_objAdresse[1].ToString() : i_edcParameter.PROa_objAdresse[0].ToString();
			if (a != "enmAktuelleZeit")
			{
				if (a == "enmSollZeit")
				{
					return i_edcStrategie.FUN_objBehandleSollZeit(i_edcParameter);
				}
				return i_edcStrategie.FUN_objBehandleDefault(i_edcParameter);
			}
			return i_edcStrategie.FUN_objBehandleAktuelleZeit(i_edcParameter);
		}

		private IEnumerable<string> FUN_lstPhysischeAdressenHolen(IEnumerable<EDC_PrimitivParameter> i_lstPrimitivParameter, CancellationToken i_fdcCancelationToken = default(CancellationToken))
		{
			List<string> list = new List<string>();
			foreach (EDC_PrimitivParameter item in i_lstPrimitivParameter)
			{
				i_fdcCancelationToken.ThrowIfCancellationRequested();
				if (!string.IsNullOrEmpty(item.PRO_strPhysischeAdresse))
				{
					list.Add(item.PRO_strPhysischeAdresse);
				}
				else
				{
					list.AddRange(FUN_objParameterBehandlung(item, m_edcAdressRegistrierungsStrategie));
				}
			}
			return list.Distinct();
		}

        /// <summary>
        /// 注销事件
        /// </summary>
		private void SUB_EventHandlerDicValueDispose()
		{
			foreach (KeyValuePair<EDC_PrimitivParameter, IDisposable> dicEventHandlerSubscription in m_dicEventHandlerSubscriptions)
			{
				dicEventHandlerSubscription.Value.Dispose();
			}
		}

        /// <summary>
        /// 建立连接
        /// Connection is established
        /// </summary>
        /// <returns></returns>
		private bool FUN_blnIstVerbindungAufgebaut()
		{
            // 如果正在释放资源, 返回false
			if (base.PRO_blnIstDisposed)
			{
				return false;
			}

            // 连接锁
			lock (m_objLockConnection)
			{
                // 解决连接
                return !m_blnIstVerbindungGeloest;
			}
		}

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="i_strEintrag"></param>
        /// <param name="i_strMethode"></param>
        /// <param name="i_fdcEx"></param>
		private void SUB_FehlerLoggen(string i_strEintrag, string i_strMethode, Exception i_fdcEx)
		{
			m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, i_strEintrag, "Ersa.Selectiv.Kommunikation", "EDC_KommunikationsDienst", i_strMethode, i_fdcEx);
		}

        /// <summary>
        /// 创建输出参数
        /// Create parameter output
        /// </summary>
        /// <param name="i_enuParameter"></param>
        /// <returns></returns>
		private string FUN_strParameterAusgabeErstellen(IEnumerable<EDC_PrimitivParameter> i_enuParameter)
		{
			return string.Join(", ", i_enuParameter.Select(delegate(EDC_PrimitivParameter i_edcParameter)
			{
				if (i_edcParameter != null)
				{
					return i_edcParameter.PRO_strAdresse;
				}
				return "null";
			}));
		}
	}
}
