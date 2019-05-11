using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
	public class LoggerCollection : SynchronizableBaseCollection
	{
		private LoggerCollection _errorLoggers;

		private LoggerCollection _validLoggers;

		private uint propLoggerUIDBuilder;

		public LogSnapshotCpuInfos SnapshotCpuInfos
		{
			get;
			private set;
		}

		public override Service Service
		{
			get
			{
				if (propParent is Cpu)
				{
					return ((Cpu)propParent).Service;
				}
				if (propParent is Service)
				{
					return (Service)propParent;
				}
				return null;
			}
		}

		public Logger this[string name]
		{
			get
			{
				for (int i = 0; i < propArrayList.Count; i++)
				{
					Logger logger = (Logger)propArrayList[i];
					if (name.Equals(logger.Name))
					{
						return logger;
					}
				}
				return null;
			}
		}

		internal uint LoggerUID => propLoggerUIDBuilder++;

		public event PviEventHandler LoggerUploaded;

		public event CollectionEventHandler CollectionUploaded;

		public event ModuleEventHandler ModuleCreated;

		public event ModuleEventHandler ModuleChanged;

		public event ModuleEventHandler ModuleDeleted;

		public LoggerCollection(object parent, string name)
			: base(CollectionType.ArrayList, parent, name)
		{
			InitLoggerCollection(parent, name);
			if (Parent is Cpu)
			{
				if (((Cpu)Parent).propUserLoggerCollections == null)
				{
					((Cpu)Parent).propUserLoggerCollections = new Hashtable();
				}
				((Cpu)Parent).propUserLoggerCollections.Add(Name, this);
			}
		}

		internal LoggerCollection(CollectionType colType, object parent, string name)
			: base(colType, parent, name)
		{
			InitLoggerCollection(parent, name);
		}

		private void InitLoggerCollection(object parent, string name)
		{
			SnapshotCpuInfos = new LogSnapshotCpuInfos(name);
			propLoggerUIDBuilder = 0u;
			synchronize = true;
			if (!(parent is Service))
			{
				return;
			}
			ArrayList loggerCollections = ((Service)parent).LoggerCollections;
			bool flag = false;
			int i;
			for (i = 0; i < loggerCollections.Count; i++)
			{
				string strA = ((LoggerCollection)loggerCollections[i]).Name.ToString();
				if (string.Compare(strA, name) == 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				LoggerCollection loggerCollection = (LoggerCollection)((Service)parent).LoggerCollections[i];
				foreach (Logger item in loggerCollection)
				{
					item.GlobalMerge = false;
				}
				loggerCollections.Remove(loggerCollections[i]);
				((Service)parent).LoggerCollections.Insert(i, this);
			}
			else
			{
				((Service)parent).LoggerCollections.Add(this);
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (PVIReadAccessTypes.ModuleList == accessType)
			{
				LoggerListFromCB(errorCode, pData, dataLen, isANSL: false);
			}
			else if (PVIReadAccessTypes.ANSL_ModuleList == accessType)
			{
				LoggerListFromCB(errorCode, pData, dataLen, isANSL: true);
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		private void LoggerListFromCB(int errorCode, IntPtr pData, uint dataLen, bool isANSL)
		{
			int updateFlags = 0;
			Hashtable hashtable = new Hashtable();
			if (isANSL)
			{
				Hashtable hashtable2 = ((Cpu)Parent).ReadANSLMODList(pData, dataLen);
				foreach (ModuleInfoDescription value in hashtable2.Values)
				{
					if (value.name != null && ModuleType.Logger == value.type && ValidateAdd(Parent, value.modListed))
					{
						hashtable.Add(value.name, value.name);
						UpdateLoggerInfo(value, errorCode, ref updateFlags);
					}
				}
			}
			else
			{
				uint num = dataLen / 164u;
				int num2 = 164;
				for (uint num3 = 0u; num3 < num; num3++)
				{
					APIFC_ModulInfoRes moduleInfoStruct = PviMarshal.PtrToModulInfoStructure(PviMarshal.GetIntPtr(pData, (ulong)(num3 * num2)), typeof(APIFC_ModulInfoRes));
					if (moduleInfoStruct.name != null && ModuleType.Logger == moduleInfoStruct.type)
					{
						hashtable.Add(moduleInfoStruct.name, moduleInfoStruct.name);
						UpdateLoggerInfo(moduleInfoStruct, errorCode, ref updateFlags);
					}
				}
			}
			if (synchronize || 1 == (updateFlags & 1))
			{
				DoSynchronize((Cpu)propParent, hashtable);
			}
			if (Count == 0)
			{
				if (((Cpu)propParent).IsSG4Target)
				{
					new Logger((Cpu)propParent, "$arlogsys");
					new Logger((Cpu)propParent, "$arlogusr");
				}
				else
				{
					new ErrorLogBook((Cpu)propParent);
				}
			}
			CheckFireUploadEvents((4224 != errorCode) ? errorCode : 0, Action.LoggersUpload, Action.LoggersConnect);
			OnCollectionUploaded(new LoggerCollectionEventArgs(propName, "", 0, Service.Language, Action.LoggersUpload, _validLoggers));
			if (propErrorCount > 0)
			{
				OnCollectionError(new LoggerCollectionEventArgs(propName, "", 0, Service.Language, Action.LoggersUpload, _errorLoggers));
			}
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				object propParent = base.propParent;
				object propUserData = base.propUserData;
				string propName = base.propName;
				if (Parent is Cpu && ((Cpu)Parent).propUserLoggerCollections != null && ((Cpu)Parent).propUserLoggerCollections.ContainsKey(Name))
				{
					((Cpu)Parent).propUserLoggerCollections.Remove(Name);
				}
				CleanUpOnDispose(disposing);
				base.propParent = propParent;
				base.propUserData = propUserData;
				base.propName = propName;
				base.Dispose(disposing, removeFromCollection);
				base.propParent = null;
				base.propUserData = null;
				base.propName = null;
			}
		}

		internal void Disconnect(bool noResponse)
		{
			if (noResponse)
			{
				DisconnectObjects(noResponse: true);
				propSentCount = 0;
				propValidCount = 0;
				propErrorCount = 0;
				propConnectionState = ConnectionStates.Disconnected;
			}
			else
			{
				DisconnectObjects(noResponse: false);
			}
		}

		private int DisconnectObjects(bool noResponse)
		{
			int result = 0;
			int num = 0;
			if (Values == null)
			{
				return 0;
			}
			foreach (Logger value in Values)
			{
				if (value.LinkId == 0)
				{
					if (!noResponse)
					{
						OnDisconnected(value, new PviEventArgs(value.Name, value.Address, 4808, "en", Action.LoggerDisconnect));
					}
				}
				else
				{
					if (ConnectionStates.Connected != value.propConnectionState && ConnectionStates.ConnectedError != value.propConnectionState)
					{
						num++;
						propValidCount++;
					}
					else
					{
						value.Disconnect(noResponse);
					}
					propSentCount++;
				}
			}
			return result;
		}

		private void CleanUpOnDispose(bool disposing)
		{
			ArrayList arrayList = new ArrayList();
			if (Values != null && 0 < Values.Count)
			{
				foreach (Logger value in Values)
				{
					arrayList.Add(value);
					value.DisconnectRet(0u);
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				object obj = arrayList[i];
				if (((Logger)obj).Name != null)
				{
					RemoveFromBaseCollections(((Logger)obj).LogicalName, 1);
				}
				((Logger)obj).Dispose(disposing, removeFromCollection: true);
				obj = null;
			}
			Clear();
		}

		public int Load(StreamReader stream)
		{
			return 0;
		}

		public int Load(string xmlFile)
		{
			int result = 0;
			string text = "";
			string text2 = "";
			if (!File.Exists(xmlFile))
			{
				return 1;
			}
			try
			{
				XmlTextReader xmlTextReader = new XmlTextReader(xmlFile);
				do
				{
					if (string.Compare(xmlTextReader.Name, "Logger") == 0)
					{
						text = xmlTextReader.GetAttribute("Name");
						text2 = xmlTextReader.GetAttribute("ContinuousActive");
						Logger logger = new Logger((Cpu)propParent, text);
						logger.ContinuousActive = (text2.ToLower().Equals("true") ? true : false);
						logger.LoggerEntries.Load(xmlTextReader);
					}
				}
				while (xmlTextReader.Read());
				xmlTextReader.Close();
				return result;
			}
			catch
			{
				return 12054;
			}
		}

		public int Save(string file)
		{
			int result = 0;
			StringBuilder xmlTextBlock = new StringBuilder();
			string executingAssemblyFileVersion = Pvi.GetExecutingAssemblyFileVersion(Assembly.GetExecutingAssembly());
			xmlTextBlock.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<?AutomationStudio Version=\"2.6\"?>\r\n<?AutomationRuntimeIOSystem Version=\"1.0\"?>\r\n<?LoggerControl PviSVersion=\"");
			xmlTextBlock.Append(executingAssemblyFileVersion + "\"?>\r\n");
			xmlTextBlock.Append($"<Loggers Count=\"{Count}\">");
			foreach (Logger propArray in propArrayList)
			{
				xmlTextBlock.Append(string.Format("<Logger Name=\"{0}\" Entries=\"{1}\" ContinuousActive=\"{2}\">", propArray.Name, propArray.LoggerEntries.Count, propArray.ContinuousActive ? "true" : "false"));
				string arg = propArray.LoggerEntries.FormatLogBookInfo(useXMLTags: true);
				xmlTextBlock.Append($"<LoggerEntries formatVersion=\"1\" {arg}>\r\n");
				propArray.LoggerEntries.SaveArlEntries(ref xmlTextBlock);
				xmlTextBlock.Append("</LoggerEntries>");
				xmlTextBlock.Append("</Logger>");
			}
			xmlTextBlock.Append("</Loggers>");
			FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(xmlTextBlock.ToString());
			streamWriter.Close();
			fileStream.Close();
			return result;
		}

		public override void Remove(string key)
		{
			int num = 0;
			for (num = 0; num < propArrayList.Count && ((Logger)propArrayList[num]).Name.CompareTo(key) != 0; num++)
			{
			}
			if (num < propArrayList.Count)
			{
				base.Remove(num);
			}
		}

		public int Add(Logger logger)
		{
			int result = 0;
			for (int i = 0; i < propArrayList.Count; i++)
			{
				if (((Logger)propArrayList[i]).Name.CompareTo(logger.Name) == 0)
				{
					return -1;
				}
			}
			propArrayList.Add(logger);
			logger.propParentCollection = this;
			return result;
		}

		public virtual void OnLoggerUploaded(Logger logMod, PviEventArgs e)
		{
			if (e.ErrorCode != 0)
			{
				propErrorCount++;
				if (_errorLoggers == null)
				{
					_errorLoggers = new LoggerCollection(Parent, "Error loggers");
					_errorLoggers.propInternalCollection = true;
				}
				if (!_errorLoggers.ContainsKey(logMod.Name))
				{
					_errorLoggers.Add(logMod);
				}
			}
			else
			{
				propValidCount++;
				if (_validLoggers == null)
				{
					_validLoggers = new LoggerCollection(Parent, "Valid loggers");
					_validLoggers.propInternalCollection = true;
				}
				if (!_validLoggers.ContainsKey(logMod.Name))
				{
					_validLoggers.Add(logMod);
				}
			}
			if (this.LoggerUploaded != null)
			{
				this.LoggerUploaded(logMod, e);
			}
		}

		public void Upload()
		{
			Upload(ModuleListOptions.INA2000CompatibleMode);
		}

		public void Upload(ModuleListOptions lstOption)
		{
			if (Actions.Uploading != (base.Requests & Actions.Uploading))
			{
				base.Requests |= Actions.Uploading;
				int num = UpdateLoggerList(lstOption);
				if (0 < num)
				{
					base.Requests &= ~Actions.Uploading;
					OnError(new CollectionEventArgs(((Base)propParent).Name, ((Base)propParent).Address, num, Service.Language, Action.ModulesUpload, null));
				}
			}
		}

		internal int UpdateLoggerList(ModuleListOptions lstOption)
		{
			_UploadOption = lstOption;
			if (!((Cpu)propParent).IsConnected)
			{
				if (Actions.Uploading == (base.Requests & Actions.Uploading))
				{
					base.Requests &= ~Actions.Uploading;
				}
				base.Requests |= Actions.Upload;
				return -2;
			}
			propValidCount = 0;
			propErrorCount = 0;
			if (_errorLoggers != null)
			{
				_errorLoggers.Clear();
			}
			if (_validLoggers != null)
			{
				_validLoggers.Clear();
			}
			if (((Cpu)Parent).Connection.DeviceType == DeviceType.ANSLTcp)
			{
				return PInvokePvicom.PviComReadArgumentRequest(Service, ((Cpu)propParent).LinkId, AccessTypes.ANSL_ModuleList, IntPtr.Zero, 0, 624u, base.InternId);
			}
			return PInvokePvicom.PviComReadArgumentRequest(Service, ((Cpu)propParent).LinkId, AccessTypes.ModuleList, IntPtr.Zero, 0, 624u, base.InternId);
		}

		protected void OnError(CollectionEventArgs e)
		{
			if (propParent is Cpu)
			{
				((Cpu)propParent).OnError(e);
			}
		}

		internal virtual void OnModuleChanged(ModuleEventArgs moduleEvent)
		{
			if (this.ModuleChanged != null)
			{
				this.ModuleChanged(this, moduleEvent);
			}
		}

		internal virtual void OnModuleDeleted(ModuleEventArgs moduleEvent)
		{
			if (this.ModuleDeleted != null)
			{
				this.ModuleDeleted(this, moduleEvent);
			}
		}

		internal virtual void OnModuleCreated(ModuleEventArgs moduleEvent)
		{
			if (this.ModuleCreated != null)
			{
				this.ModuleCreated(this, moduleEvent);
			}
		}

		protected void OnCollectionUploaded(CollectionEventArgs e)
		{
			hasBeenUploadedOnce = true;
			if (this.CollectionUploaded != null)
			{
				this.CollectionUploaded(this, e);
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = 0;
			if (Count > 0)
			{
				writer.WriteStartElement(GetType().Name);
				foreach (object value in Values)
				{
					writer.WriteStartElement("Logger");
					result = ((Logger)value).SaveModuleConfiguration(ref writer, flags);
					writer.WriteAttributeString("LoggerEntries", ((Logger)value).LoggerEntries.Count.ToString());
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			return result;
		}

		internal int DiagnosticModeUpdateModuleInfo(APIFC_DiagModulInfoRes diagModInfo, int errorCode, ref int updateFlags)
		{
			Logger logger = null;
			bool flag = false;
			if ((base.Requests & Actions.Upload) != 0)
			{
				flag = true;
				updateFlags |= 1;
			}
			logger = ((Cpu)propParent).Loggers[diagModInfo.name];
			if (flag && logger == null && (diagModInfo.name.CompareTo("$arlogsys") == 0 || diagModInfo.name.CompareTo("$arlogusr") == 0))
			{
				logger = new Logger((Cpu)propParent, diagModInfo.name);
			}
			logger?.updateProperties(diagModInfo);
			return errorCode;
		}

		internal void EmptyCollection()
		{
			Hashtable hashtable = new Hashtable();
			if (0 < Count)
			{
				foreach (Logger value in Values)
				{
					hashtable.Add(value.Name, value);
				}
				if (0 < hashtable.Count)
				{
					foreach (Logger value2 in hashtable.Values)
					{
						RemoveFromCollection(value2, Action.LoggerDelete);
						Remove(value2.Name);
					}
					hashtable.Clear();
				}
			}
			hashtable = null;
		}

		internal int UpdateLoggerInfo(APIFC_ModulInfoRes moduleInfoStruct, int errorCode, ref int updateFlags)
		{
			ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
			moduleInfoDescription.Init(moduleInfoStruct);
			return UpdateLoggerInfo(moduleInfoDescription, errorCode, ref updateFlags);
		}

		internal int UpdateLoggerInfo(ModuleInfoDescription moduleInfoStruct, int errorCode, ref int updateFlags)
		{
			if (errorCode != 0)
			{
				return errorCode;
			}
			bool flag = false;
			if (Actions.Upload == (base.Requests & Actions.Upload) || Actions.Uploading == (base.Requests & Actions.Uploading))
			{
				flag = true;
				updateFlags |= 1;
			}
			Logger logger = null;
			if (moduleInfoStruct.type == ModuleType.Logger)
			{
				logger = ((Cpu)propParent).Loggers[moduleInfoStruct.name];
				if (flag && logger == null)
				{
					logger = new Logger((Cpu)propParent, moduleInfoStruct.name);
				}
				if (logger != null && (flag || Actions.Upload == (logger.Requests & Actions.Upload) || Actions.ModuleInfo == (logger.Requests & Actions.ModuleInfo) || Actions.Uploading == (logger.Requests & Actions.Uploading)))
				{
					logger.updateProperties(moduleInfoStruct, (((Cpu)propParent).BootMode == BootMode.Diagnostics) ? true : false);
				}
			}
			if (logger != null)
			{
				OnLoggerUploaded(logger, new PviEventArgs(propName, "", errorCode, Service.Language, Action.LoggerUpload, Service));
				if (logger.CheckModuleInfo(errorCode))
				{
					logger.Fire_OnConnected(new PviEventArgs(logger.Name, logger.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
			}
			if (!((Cpu)propParent).IsSG4Target && !((Cpu)propParent).Loggers.ContainsKey("$LOG285$"))
			{
				logger = new ErrorLogBook((Cpu)propParent);
			}
			return 0;
		}

		internal void DoSynchronize(Cpu callingCpu, Hashtable newItems)
		{
			Hashtable hashtable = new Hashtable();
			if (Values != null)
			{
				foreach (Logger value in Values)
				{
					if (!newItems.ContainsKey(value.Name) && !hashtable.ContainsKey(value.propName))
					{
						hashtable.Add(value.Name, value);
					}
				}
				if (0 < hashtable.Count)
				{
					foreach (Logger value2 in hashtable.Values)
					{
						if (callingCpu.IsSG4Target)
						{
							if (value2.Name.CompareTo("$arlogsys") != 0 && value2.Name.CompareTo("$arlogusr") != 0 && !value2.IsArchive)
							{
								RemoveFromCollection(value2, Action.LoggerDelete);
							}
						}
						else if (value2.Name.CompareTo("$LOG285$") != 0 && !value2.IsArchive)
						{
							RemoveFromCollection(value2, Action.LoggerDelete);
						}
					}
					hashtable.Clear();
				}
			}
			hashtable = null;
		}
	}
}
