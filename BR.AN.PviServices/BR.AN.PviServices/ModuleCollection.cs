using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
	public class ModuleCollection : SynchronizableBaseCollection
	{
		private int _percentage;

		private ModuleCollection _errorModules;

		private ModuleCollection _validModules;

		internal ModuleType ModuleFilter;

		public Module this[string name]
		{
			get
			{
				if (propCollectionType == CollectionType.HashTable)
				{
					return (Module)base[name];
				}
				return null;
			}
		}

		public event ModuleEventHandler ModuleCreated;

		public event ModuleEventHandler ModuleChanged;

		public event ModuleEventHandler ModuleDeleted;

		public event PviEventHandler ModuleUploaded;

		public event PviEventHandler ModuleDownloaded;

		public event ModuleEventHandler UploadProgress;

		public event ModuleEventHandler DownloadProgress;

		public event CollectionEventHandler CollectionDownloaded;

		public event CollectionEventHandler CollectionUploaded;

		public event ModuleCollectionEventHandler CollectionUploadProgress;

		public event ModuleCollectionEventHandler CollectionDownloadProgress;

		public ModuleCollection(object parent, string name)
			: base(parent, name)
		{
			_UploadOption = ModuleListOptions.INA2000CompatibleMode;
			synchronize = true;
			ModuleFilter = ModuleType.Unknown;
			if (Parent is Cpu)
			{
				if (((Cpu)Parent).propUserModuleCollections == null)
				{
					((Cpu)Parent).propUserModuleCollections = new Hashtable();
				}
				((Cpu)Parent).propUserModuleCollections.Add(Name, this);
			}
		}

		internal ModuleCollection(CollectionType colType, object parent, string name)
			: base(colType, parent, name)
		{
			ModuleFilter = ModuleType.Unknown;
			_UploadOption = ModuleListOptions.INA2000CompatibleMode;
			synchronize = true;
		}

		internal ModuleCollection(ModuleType modType, CollectionType colType, object parent, string name)
			: base(colType, parent, name)
		{
			ModuleFilter = modType;
			_UploadOption = ModuleListOptions.INA2000CompatibleMode;
			synchronize = true;
		}

		protected void OnError(CollectionEventArgs e)
		{
			if (propParent is Cpu)
			{
				((Cpu)propParent).OnError(e);
			}
		}

		public override void Connect(ConnectionType connectionType)
		{
			propSentCount = 0;
			propValidCount = 0;
			propErrorCount = 0;
			int num = 0;
			if (_validModules == null)
			{
				_validModules = new ModuleCollection(CollectionType.HashTable, Parent, "Valid modules");
				_validModules.propInternalCollection = true;
			}
			else
			{
				_validModules.Clear();
			}
			if (Count == 0)
			{
				OnCollectionConnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesConnect, _validModules));
			}
			else
			{
				foreach (Module value in Values)
				{
					propSentCount++;
					if (ConnectionStates.Connected == value.propConnectionState || ConnectionStates.ConnectedError == value.propConnectionState)
					{
						_validModules.Add(value);
						propValidCount++;
						num++;
						if (num + propErrorCount == Count)
						{
							OnCollectionConnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesConnect, _validModules));
						}
					}
					else
					{
						value.Connect(connectionType);
					}
				}
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
				Disconnect();
			}
		}

		public void Disconnect()
		{
			propSentCount = 0;
			propValidCount = 0;
			propErrorCount = 0;
			if (Values == null || Count == 0)
			{
				OnCollectionDisconnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDisconnect, this));
				return;
			}
			if (_validModules == null)
			{
				_validModules = new ModuleCollection(CollectionType.HashTable, Parent, "Valid modules");
				_validModules.propInternalCollection = true;
			}
			else
			{
				_validModules.Clear();
			}
			if (Count == 0)
			{
				OnCollectionDisconnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDisconnect, _validModules));
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
			foreach (Module value in Values)
			{
				if (value.LinkId == 0)
				{
					if (!noResponse)
					{
						OnDisconnected(value, new PviEventArgs(value.Name, value.Address, 4808, "en", Action.ModuleDisconnect));
					}
				}
				else
				{
					if (ConnectionStates.Connected != value.propConnectionState && ConnectionStates.ConnectedError != value.propConnectionState)
					{
						num++;
						if (num == Count && !noResponse)
						{
							OnCollectionDisconnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModuleDisconnect, _validModules));
						}
						propValidCount++;
						if (!noResponse)
						{
							_validModules.Add(value);
						}
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

		public virtual Module GetItem(int index)
		{
			return (Module)ElementAt(index);
		}

		internal virtual Module GetItem(string name)
		{
			return (Module)base[name];
		}

		public virtual int Add(Module module)
		{
			Module module2 = this[module.Name];
			if (module2 == null)
			{
				Add(module.Name, module);
			}
			else
			{
				switch (module.Type)
				{
				case ModuleType.PlcTask:
				case ModuleType.TimerTask:
				case ModuleType.ExceptionTask:
				case ModuleType.Logger:
					return -1;
				case ModuleType.SystemTask:
					module2.propAddress = module.propAddress;
					break;
				}
			}
			if (!propInternalCollection)
			{
				if (module.propUserCollections == null)
				{
					module.propUserCollections = new Hashtable();
				}
				if (!module.propUserCollections.ContainsKey(Name))
				{
					module.propUserCollections.Add(Name, this);
				}
			}
			return 0;
		}

		public override void Remove(string key)
		{
			if (_validModules != null && _validModules.ContainsKey(key))
			{
				_validModules.Remove(key);
			}
			if (_errorModules != null && _errorModules.ContainsKey(key))
			{
				_errorModules.Remove(key);
			}
			base.Remove(key);
		}

		public override void Remove(object key)
		{
			if (key is Module)
			{
				if (_validModules != null && _validModules.ContainsKey(((Module)key).Name))
				{
					_validModules.Remove(((Module)key).Name);
				}
				if (_errorModules != null && _errorModules.ContainsKey(((Module)key).Name))
				{
					_errorModules.Remove(((Module)key).Name);
				}
			}
			base.Remove(key);
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				object propParent = base.propParent;
				object propUserData = base.propUserData;
				string propName = base.propName;
				if (Parent is Cpu && ((Cpu)Parent).propUserModuleCollections != null && ((Cpu)Parent).propUserModuleCollections.ContainsKey(Name))
				{
					((Cpu)Parent).propUserModuleCollections.Remove(Name);
				}
				CleanUp(disposing);
				if (_validModules != null)
				{
					_validModules.Dispose(disposing, removeFromCollection: false);
				}
				if (_errorModules != null)
				{
					_errorModules.Dispose(disposing, removeFromCollection: false);
				}
				base.propParent = propParent;
				base.propUserData = propUserData;
				base.propName = propName;
				base.Dispose(disposing, removeFromCollection);
				base.propParent = null;
				base.propUserData = null;
				base.propName = null;
			}
		}

		internal void CleanUp(bool disposing)
		{
			int num = 0;
			ArrayList arrayList = new ArrayList();
			if (_validModules != null)
			{
				_validModules.Clear();
			}
			if (_errorModules != null)
			{
				_errorModules.Clear();
			}
			if (Values != null)
			{
				foreach (Module value in Values)
				{
					arrayList.Add(value);
				}
				for (num = 0; num < arrayList.Count; num++)
				{
					Module module = (Module)arrayList[num];
					if (module.LinkId != 0)
					{
						module.DisconnectRet(0u);
					}
					module.Dispose(disposing, removeFromCollection: true);
				}
			}
			Clear();
			if (Parent is Cpu && ((Cpu)Parent).Tasks != null)
			{
				((Cpu)Parent).Tasks.CleanUp(disposing);
			}
		}

		private void ModuleDisconnected(object sender, PviEventArgs e)
		{
			((Module)sender).Disconnected -= ModuleDisconnected;
		}

		private void CheckForModInfoChanges(APIFC_ModulInfoRes moduleInfoStruct)
		{
			ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
			moduleInfoDescription.Init(moduleInfoStruct);
			CheckForModInfoChanges(moduleInfoDescription, 0);
		}

		private void CheckForModInfoChanges(ModuleInfoDescription moduleInfoStruct, int retCode)
		{
			Module module = null;
			if (!(propParent is Cpu))
			{
				return;
			}
			if (Module.isTaskObject(moduleInfoStruct.type))
			{
				Task task = ((Cpu)propParent).Tasks[moduleInfoStruct.name];
				if (task == null)
				{
					task = new Task((Cpu)propParent, moduleInfoStruct.name);
				}
				else if (((Cpu)propParent).Modules[moduleInfoStruct.name] == null)
				{
					((Cpu)propParent).Modules.Add(task);
				}
				task.updateProperties(moduleInfoStruct);
				if (retCode != 0)
				{
					module = task;
				}
			}
			else if (moduleInfoStruct.type == ModuleType.Logger)
			{
				Logger logger = ((Cpu)propParent).Loggers[moduleInfoStruct.name];
				if (logger == null)
				{
					logger = new Logger((Cpu)propParent, moduleInfoStruct.name);
				}
				else if (((Cpu)propParent).Modules[moduleInfoStruct.name] == null)
				{
					((Cpu)propParent).Modules.Add(logger);
				}
				logger.updateProperties(moduleInfoStruct);
				if (retCode != 0)
				{
					module = logger;
				}
			}
			else if (moduleInfoStruct.type == ModuleType.TextSystemModule || moduleInfoStruct.type == ModuleType.TextSystemAddModule)
			{
				Module module2 = ((Cpu)propParent).TextModules[moduleInfoStruct.name];
				if (module2 == null)
				{
					module2 = new Module((Cpu)propParent, moduleInfoStruct.name);
				}
				else if (((Cpu)propParent).Modules[moduleInfoStruct.name] == null)
				{
					((Cpu)propParent).TextModules.Add(module2);
				}
				module2.updateProperties(moduleInfoStruct);
				if (retCode != 0)
				{
					module = module2;
				}
			}
			else if (!((Cpu)Parent).Libraries.ContainsKey(moduleInfoStruct.name) && ModuleFilter == ModuleType.Unknown)
			{
				Module module3 = ((Cpu)propParent).Modules[moduleInfoStruct.name];
				if (module3 == null)
				{
					module3 = new Module((Cpu)propParent, moduleInfoStruct, this);
				}
				module3.updateProperties(moduleInfoStruct);
				if (retCode != 0)
				{
					module = module3;
				}
			}
			if (retCode != 0 && module != null)
			{
				OnModuleChanged(new ModuleEventArgs(module.Name, module.Address, 12054, Service.Language, Action.ModuleChangedEvent, module, 0));
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			int num = 0;
			if (PVIReadAccessTypes.ANSL_ModuleList == accessType)
			{
				if (dataLen == 0 || errorCode != 0)
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.ModulesUpload, Service));
					OnError(new CollectionEventArgs(propName, "", errorCode, Service.Language, Action.ModulesUpload, null));
				}
				else
				{
					try
					{
						ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
						byte[] array = new byte[dataLen];
						Marshal.Copy(pData, array, 0, (int)dataLen);
						MemoryStream input = new MemoryStream(array);
						XmlTextReader xmlTextReader = new XmlTextReader(input);
						xmlTextReader.MoveToContent();
						if (xmlTextReader.Name.CompareTo("ModList") == 0)
						{
							while (!xmlTextReader.EOF && xmlTextReader.NodeType != XmlNodeType.EndElement)
							{
								if (xmlTextReader.Name.CompareTo("ModInfo") == 0 || xmlTextReader.Name.CompareTo("TaskInfo") == 0)
								{
									num = moduleInfoDescription.ReadFromXML(xmlTextReader);
									if (moduleInfoDescription.name != null)
									{
										CheckForModInfoChanges(moduleInfoDescription, num);
									}
								}
								else
								{
									xmlTextReader.Read();
								}
							}
						}
						if (propParent is Cpu && ((Cpu)propParent).Loggers.Count == 0)
						{
							new ErrorLogBook((Cpu)propParent);
							((Cpu)propParent).propIsSG4Target = false;
						}
					}
					catch
					{
						OnError(new CollectionEventArgs(propName, "", 12054, Service.Language, Action.ModuleChangedEvent, null));
					}
				}
			}
			else if (PVIReadAccessTypes.ModuleList == accessType)
			{
				if (errorCode == 0)
				{
					int num2 = (int)(dataLen / 164u);
					for (int i = 0; i < num2; i++)
					{
						APIFC_ModulInfoRes moduleInfoStruct = PviMarshal.PtrToModulInfoStructure(PviMarshal.GetIntPtr(pData, (ulong)(i * 164)), typeof(APIFC_ModulInfoRes));
						CheckForModInfoChanges(moduleInfoStruct);
					}
					if (propParent is Cpu && ((Cpu)propParent).Loggers.Count == 0)
					{
						new ErrorLogBook((Cpu)propParent);
						((Cpu)propParent).propIsSG4Target = false;
					}
				}
				else
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.ModulesUpload, Service));
					OnError(new CollectionEventArgs(propName, "", errorCode, Service.Language, Action.ModulesUpload, null));
				}
			}
			else if (PVIReadAccessTypes.DiagnoseModuleList == accessType)
			{
				if (errorCode == 0)
				{
					int num3 = (int)(dataLen / 57u);
					for (int j = 0; j < num3; j++)
					{
						APIFC_DiagModulInfoRes moduleInfo = PviMarshal.PtrToDiagModulInfoStructure(PviMarshal.GetIntPtr(pData, (ulong)(j * 57)), typeof(APIFC_DiagModulInfoRes));
						if (propParent is Cpu && ModuleFilter != ModuleType.TextSystemModule)
						{
							Module module = null;
							if ((module = ((Cpu)propParent).Modules[moduleInfo.name]) == null)
							{
								module = new Module((Cpu)propParent, moduleInfo, this);
							}
							module.updateProperties(moduleInfo);
						}
					}
				}
				else
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.ModulesUpload, Service));
					OnError(new CollectionEventArgs(propName, "", errorCode, Service.Language, Action.ModulesUpload, null));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		public void Upload()
		{
			Upload(ModuleListOptions.INA2000CompatibleMode);
		}

		public void Upload(ModuleListOptions lstOption)
		{
			_UploadOption = lstOption;
			if (propParent != null)
			{
				base.Requests |= Actions.Upload;
				int num = ((Cpu)propParent).UpdateModuleList(lstOption);
				if (0 < num)
				{
					OnError(new CollectionEventArgs(((Base)propParent).Name, ((Base)propParent).Address, num, Service.Language, Action.ModulesUpload, null));
				}
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = 0;
			int num = 0;
			if (Values.Count > 0)
			{
				writer.WriteStartElement(GetType().Name);
				foreach (object value in Values)
				{
					writer.WriteStartElement("Module");
					num = ((Module)value).SaveModuleConfiguration(ref writer, flags);
					if (num != 0)
					{
						result = num;
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			return result;
		}

		public void Download(MemoryType memoryType, InstallMode installMode)
		{
			propSentCount = 0;
			propValidCount = 0;
			propErrorCount = 0;
			if (_validModules == null)
			{
				_validModules = new ModuleCollection(CollectionType.HashTable, Parent, "Valid modules");
				_validModules.propInternalCollection = true;
			}
			else
			{
				_validModules.Clear();
			}
			if (Count == 0)
			{
				OnCollectionDownloaded(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModuleDownload, _validModules));
			}
			else
			{
				foreach (Module value in Values)
				{
					value.Download(memoryType, installMode);
					propSentCount++;
				}
			}
		}

		public void Upload(string path)
		{
			propSentCount = 0;
			propValidCount = 0;
			if (_validModules == null)
			{
				_validModules = new ModuleCollection(CollectionType.HashTable, Parent, "Valid modules");
				_validModules.propInternalCollection = true;
			}
			else
			{
				_validModules.Clear();
			}
			if (0 < Count)
			{
				foreach (Module value in Values)
				{
					value.Upload($"{path}\\{value.Name}.br");
					propSentCount++;
				}
			}
		}

		protected internal override void OnConnected(Base sender, PviEventArgs e)
		{
			propValidCount++;
			if (_validModules == null)
			{
				_validModules = new ModuleCollection(CollectionType.HashTable, Parent, "Valid modules");
				_validModules.propInternalCollection = true;
			}
			if (!_validModules.ContainsKey(sender.Name))
			{
				_validModules.Add((Module)sender);
			}
			Fire_Connected(sender, e);
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionConnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesConnect, _validModules));
				if (propErrorCount > 0)
				{
					OnCollectionError(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesConnect, _errorModules));
				}
			}
		}

		protected internal override void OnDisconnected(Base sender, PviEventArgs e)
		{
			propConnectionState = ConnectionStates.Disconnected;
			propValidCount++;
			if (_validModules == null)
			{
				_validModules = new ModuleCollection(CollectionType.HashTable, Parent, "Valid modules");
				_validModules.propInternalCollection = true;
			}
			if (!_validModules.ContainsKey(sender.Name))
			{
				_validModules.Add((Module)sender);
			}
			Fire_Disconnected(sender, e);
			if (propValidCount == propSentCount && Service != null)
			{
				OnCollectionDisconnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDisconnect, _validModules));
				if (propErrorCount > 0)
				{
					OnCollectionError(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDisconnect, _errorModules));
				}
			}
		}

		protected internal override void OnError(Base sender, PviEventArgs e)
		{
			if (_errorModules == null)
			{
				_errorModules = new ModuleCollection(CollectionType.HashTable, Parent, "Error modules");
				_errorModules.propInternalCollection = true;
			}
			if (!_errorModules.ContainsKey(sender.Name))
			{
				_errorModules.Add((Module)sender);
				propErrorCount++;
			}
			Fire_Error(sender, e);
			if (propSentCount != 0)
			{
				if (e.Action == Action.ModuleDownload)
				{
					if (propErrorCount + propValidCount == propSentCount)
					{
						_percentage = 100;
					}
					else
					{
						_percentage = 100 / propSentCount * (propValidCount + propErrorCount);
					}
					OnCollectionDownloadProgress(new ModuleCollectionProgressEventArgs(propName, "", e.ErrorCode, Service.Language, Action.ModulesDownload, (Module)sender, _percentage));
				}
				if (e.Action == Action.ModuleUpload)
				{
					if (propErrorCount + propValidCount == propSentCount)
					{
						_percentage = 100;
					}
					else
					{
						_percentage = 100 / propSentCount * (propValidCount + propErrorCount);
					}
					OnCollectionUploadProgress(new ModuleCollectionProgressEventArgs(propName, "", e.ErrorCode, Service.Language, Action.ModulesUpload, (Module)sender, _percentage));
				}
			}
			if (propValidCount + propErrorCount != Count)
			{
				return;
			}
			OnCollectionError(new ModuleCollectionEventArgs(propName, "", e.ErrorCode, Service.Language, e.Action, _errorModules));
			if (propValidCount > 0)
			{
				switch (e.Action)
				{
				case Action.ModuleDownload:
					OnCollectionDownloaded(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDownload, _validModules));
					break;
				case Action.ModuleConnect:
				case Action.LoggerConnect:
					OnCollectionConnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesConnect, _validModules));
					break;
				case Action.ModuleDisconnect:
					OnCollectionDisconnected(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDisconnect, _validModules));
					break;
				case Action.ModuleUpload:
					OnCollectionUploaded(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesUpload, _validModules));
					break;
				}
			}
		}

		public virtual void OnModuleDownloaded(Module module, PviEventArgs e)
		{
			if (e.ErrorCode == 0)
			{
				propValidCount++;
				if (_validModules == null)
				{
					_validModules = new ModuleCollection(CollectionType.HashTable, Parent, "Valid modules");
					_validModules.propInternalCollection = true;
				}
				if (!_validModules.ContainsKey(module.Name))
				{
					_validModules.Add(module);
				}
			}
			else
			{
				propErrorCount++;
				if (_errorModules == null)
				{
					_errorModules = new ModuleCollection(CollectionType.HashTable, Parent, "Error modules");
					_errorModules.propInternalCollection = true;
				}
				if (!_errorModules.ContainsKey(module.Name))
				{
					_errorModules.Add(module);
				}
				Fire_Error(module, e);
			}
			if (this.ModuleDownloaded != null)
			{
				this.ModuleDownloaded(module, e);
			}
			if (propSentCount != 0)
			{
				if (propErrorCount + propValidCount == propSentCount)
				{
					_percentage = 100;
				}
				else
				{
					_percentage = 100 / propSentCount * (propValidCount + propErrorCount);
				}
				OnCollectionDownloadProgress(new ModuleCollectionProgressEventArgs(propName, "", e.ErrorCode, Service.Language, Action.ModulesDownload, module, _percentage));
			}
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionDownloaded(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDownload, _validModules));
				if (propErrorCount > 0)
				{
					OnCollectionError(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDownload, _errorModules));
				}
			}
		}

		protected internal virtual void OnModuleUploaded(Module module, PviEventArgs e)
		{
			if (e.ErrorCode == 0)
			{
				propValidCount++;
				if (_validModules == null)
				{
					_validModules = new ModuleCollection(CollectionType.HashTable, Parent, "Valid modules");
					_validModules.propInternalCollection = true;
				}
				if (!_validModules.ContainsKey(module.Name))
				{
					_validModules.Add(module);
				}
			}
			else
			{
				propErrorCount++;
				if (_errorModules == null)
				{
					_errorModules = new ModuleCollection(CollectionType.HashTable, Parent, "Error modules");
					_errorModules.propInternalCollection = true;
				}
				if (!_errorModules.ContainsKey(module.Name))
				{
					_errorModules.Add(module);
				}
				Fire_Error(module, e);
			}
			if (this.ModuleUploaded != null)
			{
				this.ModuleUploaded(module, e);
			}
			if (propSentCount != 0)
			{
				if (propErrorCount + propValidCount == propSentCount)
				{
					_percentage = 100;
				}
				else
				{
					_percentage = 100 / propSentCount * (propValidCount + propErrorCount);
				}
				OnCollectionUploadProgress(new ModuleCollectionProgressEventArgs(propName, "", e.ErrorCode, Service.Language, Action.ModulesUpload, module, _percentage));
			}
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionUploaded(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesUpload, _validModules));
				if (propErrorCount > 0)
				{
					OnCollectionError(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesUpload, _errorModules));
				}
			}
		}

		internal void CheckUploadedRequest()
		{
			if (Actions.Upload == (base.Requests & Actions.Upload))
			{
				OnCollectionUploaded(new ModuleCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesUpload, this));
			}
		}

		protected void OnCollectionDownloaded(CollectionEventArgs e)
		{
			if (this.CollectionDownloaded != null)
			{
				this.CollectionDownloaded(this, e);
			}
			propErrorCount = 0;
			propValidCount = 0;
			propSentCount = 0;
			if (_validModules != null)
			{
				_validModules.Clear();
			}
			if (_errorModules != null)
			{
				_errorModules.Clear();
			}
		}

		protected void OnCollectionUploaded(CollectionEventArgs e)
		{
			hasBeenUploadedOnce = true;
			if (this.CollectionUploaded != null)
			{
				this.CollectionUploaded(this, e);
			}
			propErrorCount = 0;
			propValidCount = 0;
			propSentCount = 0;
			if (_validModules != null)
			{
				_validModules.Clear();
			}
			if (_errorModules != null)
			{
				_errorModules.Clear();
			}
		}

		protected void OnCollectionDownloadProgress(ModuleCollectionProgressEventArgs e)
		{
			if (this.CollectionDownloadProgress != null)
			{
				this.CollectionDownloadProgress(this, e);
			}
		}

		protected void OnCollectionUploadProgress(ModuleCollectionProgressEventArgs e)
		{
			if (this.CollectionUploadProgress != null)
			{
				this.CollectionUploadProgress(this, e);
			}
		}

		protected internal virtual void OnDownloadProgress(Module module, ModuleEventArgs e)
		{
			if (this.DownloadProgress != null)
			{
				this.DownloadProgress(module, e);
			}
		}

		protected internal virtual void OnUploadProgress(Module module, ModuleEventArgs e)
		{
			if (this.UploadProgress != null)
			{
				this.UploadProgress(module, e);
			}
		}

		internal virtual void OnModuleCreated(ModuleEventArgs moduleEvent)
		{
			if (this.ModuleCreated != null)
			{
				this.ModuleCreated(this, moduleEvent);
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

		internal int DiagnosticModeUpdateModuleInfo(APIFC_DiagModulInfoRes diagModInfo, int errorCode, ref int updateFlags)
		{
			if (errorCode != 0)
			{
				return errorCode;
			}
			bool flag = false;
			if ((base.Requests & Actions.Upload) != 0)
			{
				flag = true;
				updateFlags |= 2;
			}
			Module module = null;
			module = ((ModuleFilter != ModuleType.TextSystemModule) ? ((Cpu)propParent).Modules[diagModInfo.name] : ((Cpu)propParent).TextModules[diagModInfo.name]);
			if (flag && module == null && ModuleFilter == ModuleType.Unknown)
			{
				module = new Module((Cpu)propParent, diagModInfo.name);
			}
			if (module != null && (flag || (module.Requests & Actions.Upload) != 0 || (module.Requests & Actions.ModuleInfo) != 0))
			{
				module.updateProperties(diagModInfo);
			}
			if (module != null)
			{
				if ((module.Requests & Actions.Upload) != 0)
				{
					module.Requests &= ~Actions.Upload;
					OnModuleUploaded(module, new PviEventArgs(propName, "", errorCode, Service.Language, Action.ModuleUpload, Service));
				}
				if (module.CheckModuleInfo(errorCode))
				{
					module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
			}
			return 0;
		}

		internal int UpdateTextModuleInfo(ModuleInfoDescription moduleInfoStruct, int errorCode, ref int updateFlags, bool diagList)
		{
			if (errorCode != 0)
			{
				return errorCode;
			}
			bool flag = false;
			if ((base.Requests & Actions.Upload) != 0)
			{
				flag = true;
				updateFlags |= 4;
			}
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			Module module = null;
			if (Module.isTextModuleObject(moduleInfoStruct.type))
			{
				if (0 < Count)
				{
					foreach (Module value in ((Cpu)propParent).TextModules.Values)
					{
						if (value.AddressEx.CompareTo(moduleInfoStruct.name) == 0)
						{
							value.updateProperties(moduleInfoStruct, diagList);
							if ((value.Requests & Actions.Upload) != 0)
							{
								value.Requests &= ~Actions.Upload;
								arrayList.Add(value);
							}
							if (value.CheckModuleInfo(errorCode))
							{
								arrayList2.Add(value);
							}
						}
					}
				}
				if (((Cpu)propParent).TextModules.ValidateAdd(Parent, moduleInfoStruct.modListed))
				{
					module = ((Cpu)propParent).TextModules[moduleInfoStruct.name];
					if (flag && module == null)
					{
						module = new Module((Cpu)propParent, moduleInfoStruct.name, moduleInfoStruct.type);
					}
					if (module != null && (flag || (module.Requests & Actions.Upload) != 0 || (module.Requests & Actions.ModuleInfo) != 0))
					{
						module.updateProperties(moduleInfoStruct, diagList);
					}
				}
			}
			if (module != null)
			{
				if ((module.Requests & Actions.Upload) != 0)
				{
					module.Requests &= ~Actions.Upload;
					OnModuleUploaded(module, new PviEventArgs(propName, "", errorCode, Service.Language, Action.ModuleUpload, Service));
				}
				if (module.CheckModuleInfo(errorCode))
				{
					module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
			}
			for (int i = 0; i < arrayList2.Count; i++)
			{
				module = (Module)arrayList2[i];
				module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				module = (Module)arrayList[i];
				OnModuleUploaded(module, new PviEventArgs(module.Name, module.Address, errorCode, Service.Language, Action.ModuleUpload, Service));
			}
			return 0;
		}

		internal int UpdateModuleInfo(ModuleInfoDescription moduleInfoStruct, int errorCode, ref int updateFlags, bool diagList)
		{
			if (errorCode != 0)
			{
				return errorCode;
			}
			bool flag = false;
			if ((base.Requests & Actions.Upload) != 0)
			{
				flag = true;
				updateFlags |= 2;
			}
			bool flag2 = diagList && (_UploadOption == ModuleListOptions.INA2000CompatibleMode || _UploadOption == ModuleListOptions.INA2000DiagnosisList);
			ArrayList arrayList = new ArrayList();
			if (0 < ((Cpu)propParent).Modules.Count)
			{
				foreach (Module value in ((Cpu)propParent).Modules.Values)
				{
					if (value.AddressEx.CompareTo(moduleInfoStruct.name) == 0)
					{
						value.updateProperties(moduleInfoStruct, diagList);
						if (flag2)
						{
							value.propType = ModuleType.Unknown;
						}
						if ((value.Requests & Actions.Upload) != 0)
						{
							value.Requests &= ~Actions.Upload;
							OnModuleUploaded(value, new PviEventArgs(value.Name, value.Address, errorCode, Service.Language, Action.ModuleUpload, Service));
						}
						if (value.CheckModuleInfo(errorCode))
						{
							arrayList.Add(value);
						}
					}
				}
			}
			Module module2 = null;
			module2 = ((Cpu)propParent).Modules[moduleInfoStruct.name];
			if (flag && module2 == null)
			{
				if (Module.isTaskObject(moduleInfoStruct.type) || ((Cpu)propParent).Tasks.ContainsKey(moduleInfoStruct.name))
				{
					if (((Cpu)propParent).Tasks.ValidateAdd(Parent, moduleInfoStruct.modListed))
					{
						module2 = ((Cpu)propParent).Tasks[moduleInfoStruct.name];
						if (module2 == null)
						{
							module2 = new Task((Cpu)propParent, moduleInfoStruct.name);
						}
						else
						{
							((Cpu)propParent).Modules.Add(module2);
						}
					}
				}
				else if (Module.isLoggerObject(moduleInfoStruct.type) || ((Cpu)propParent).Loggers.ContainsKey(moduleInfoStruct.name))
				{
					if (((Cpu)propParent).Loggers.ValidateAdd(Parent, moduleInfoStruct.modListed))
					{
						module2 = ((Cpu)propParent).Loggers[moduleInfoStruct.name];
						if (module2 == null)
						{
							module2 = new Logger((Cpu)propParent, moduleInfoStruct.name);
						}
						else
						{
							((Cpu)propParent).Modules.Add(module2);
						}
					}
				}
				else if (Module.isTextModuleObject(moduleInfoStruct.type) || ((Cpu)propParent).TextModules.ContainsKey(moduleInfoStruct.name))
				{
					if (((Cpu)propParent).TextModules.ValidateAdd(Parent, moduleInfoStruct.modListed))
					{
						module2 = ((Cpu)propParent).TextModules[moduleInfoStruct.name];
						if (module2 == null)
						{
							module2 = new Module((Cpu)propParent, moduleInfoStruct.name, moduleInfoStruct.type);
						}
						else
						{
							((Cpu)propParent).TextModules.Add(module2);
						}
					}
				}
				else if (ValidateAdd(Parent, moduleInfoStruct.modListed))
				{
					module2 = new Module((Cpu)propParent, moduleInfoStruct.name);
				}
			}
			if (module2 != null && (flag || (module2.Requests & Actions.Upload) != 0 || (module2.Requests & Actions.ModuleInfo) != 0))
			{
				module2.updateProperties(moduleInfoStruct, diagList);
				if (flag2)
				{
					module2.propType = ModuleType.Unknown;
				}
			}
			if (module2 != null)
			{
				if ((module2.Requests & Actions.Upload) != 0)
				{
					module2.Requests &= ~Actions.Upload;
					OnModuleUploaded(module2, new PviEventArgs(propName, "", errorCode, Service.Language, Action.ModuleUpload, Service));
				}
				if (module2.CheckModuleInfo(errorCode))
				{
					module2.Fire_OnConnected(new PviEventArgs(module2.Name, module2.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				module2 = (Module)arrayList[i];
				module2.Fire_OnConnected(new PviEventArgs(module2.Name, module2.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
			}
			return 0;
		}

		internal void DoSynchronize(Hashtable newItems)
		{
			Hashtable hashtable = new Hashtable();
			if (Values != null)
			{
				foreach (Module value in Values)
				{
					if (value.Address != null && 0 < value.Address.Length)
					{
						if ((newItems == null || !newItems.ContainsKey(value.Address)) && !hashtable.ContainsKey(value.Name))
						{
							hashtable.Add(value.Name, value);
						}
					}
					else if ((newItems == null || !newItems.ContainsKey(value.Name)) && !hashtable.ContainsKey(value.Name))
					{
						hashtable.Add(value.Name, value);
					}
				}
			}
			if (0 < hashtable.Count)
			{
				foreach (Module value2 in hashtable.Values)
				{
					if (((Cpu)propParent).IsSG4Target || value2.Name.CompareTo("$LOG285$") != 0)
					{
						RemoveFromCollection(value2, Action.ModuleDelete);
					}
				}
				hashtable.Clear();
			}
			hashtable = null;
		}
	}
}
