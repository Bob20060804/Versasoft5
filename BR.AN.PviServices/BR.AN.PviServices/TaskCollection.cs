using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
	public class TaskCollection : SynchronizableBaseCollection
	{
		private int _percentage;

		private TaskCollection _errorTasks;

		private TaskCollection _validTasks;

		public Task this[string name]
		{
			get
			{
				return (Task)base[name];
			}
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

		public event ModuleEventHandler ModuleCreated;

		public event ModuleEventHandler ModuleChanged;

		public event ModuleEventHandler ModuleDeleted;

		public event PviEventHandler TaskUploaded;

		public event PviEventHandler Downloaded;

		public event ModuleEventHandler UploadProgress;

		public event ModuleEventHandler DownloadProgress;

		public event CollectionEventHandler CollectionDownloaded;

		public event CollectionEventHandler CollectionUploaded;

		public event ModuleCollectionEventHandler CollectionUploadProgress;

		public event ModuleCollectionEventHandler CollectionDownloadProgress;

		public TaskCollection(object parent, string name)
			: base(parent, name)
		{
			synchronize = true;
			propParent = parent;
			if (Parent is Cpu)
			{
				if (((Cpu)Parent).propUserTaskCollections == null)
				{
					((Cpu)Parent).propUserTaskCollections = new Hashtable();
				}
				if (!((Cpu)Parent).propUserTaskCollections.ContainsKey(Name))
				{
					((Cpu)Parent).propUserTaskCollections.Add(Name, this);
				}
			}
		}

		internal TaskCollection(CollectionType colType, object parent, string name)
			: base(colType, parent, name)
		{
			synchronize = true;
			propParent = parent;
		}

		public override void Connect(ConnectionType connectionType)
		{
			propSentCount = 0;
			propValidCount = 0;
			propErrorCount = 0;
			int num = 0;
			if (_validTasks == null)
			{
				_validTasks = new TaskCollection(CollectionType.HashTable, Parent, "Valid tasks");
				_validTasks.propInternalCollection = true;
			}
			else
			{
				_validTasks.Clear();
			}
			if (Count == 0)
			{
				OnCollectionConnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesConnect, _validTasks));
			}
			else
			{
				foreach (Task value in Values)
				{
					propSentCount++;
					if (ConnectionStates.Connected == value.propConnectionState || ConnectionStates.ConnectedError == value.propConnectionState)
					{
						_validTasks.Add(value);
						propValidCount++;
						num++;
						if (num + propErrorCount == Count)
						{
							OnCollectionConnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesConnect, _validTasks));
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
				OnCollectionDisconnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDisconnect, this));
				return;
			}
			if (_validTasks == null)
			{
				_validTasks = new TaskCollection(Parent, "Valid tasks");
				_validTasks.propInternalCollection = true;
			}
			else
			{
				_validTasks.Clear();
			}
			if (Count == 0)
			{
				OnCollectionDisconnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDisconnect, _validTasks));
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
			foreach (Task value in Values)
			{
				if (value.LinkId == 0)
				{
					if (!noResponse)
					{
						OnDisconnected(value, new PviEventArgs(value.Name, value.Address, 4808, "en", Action.TaskDisconnect));
					}
				}
				else
				{
					if (ConnectionStates.Connected != value.propConnectionState && ConnectionStates.ConnectedError != value.propConnectionState)
					{
						num++;
						if (num == Count && !noResponse)
						{
							OnCollectionDisconnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.ModulesDisconnect, _validTasks));
						}
						propValidCount++;
						if (!noResponse)
						{
							_validTasks.Add(value);
						}
					}
					else
					{
						if (noResponse)
						{
							value.Variables.Disconnect(noResponse: true);
						}
						value.Disconnect(noResponse);
					}
					propSentCount++;
				}
			}
			return result;
		}

		protected internal override void OnConnected(Base sender, PviEventArgs e)
		{
			propValidCount++;
			if (_validTasks == null)
			{
				_validTasks = new TaskCollection(CollectionType.HashTable, Parent, "Valid tasks");
				_validTasks.propInternalCollection = true;
			}
			if (!_validTasks.ContainsKey(sender.Name))
			{
				_validTasks.Add((Task)sender);
			}
			Fire_Connected(sender, e);
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionConnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksConnect, _validTasks));
				if (propErrorCount > 0)
				{
					OnCollectionError(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksConnect, _errorTasks));
				}
			}
		}

		protected internal virtual void OnDisconnected(Task task, PviEventArgs e)
		{
			propConnectionState = ConnectionStates.Disconnected;
			propValidCount++;
			if (_validTasks == null)
			{
				_validTasks = new TaskCollection(Parent, "Valid tasks");
				_validTasks.propInternalCollection = true;
			}
			if (!_validTasks.ContainsKey(task.Name))
			{
				_validTasks.Add(task);
			}
			Fire_Disconnected(task, e);
			if (propValidCount == propSentCount)
			{
				OnCollectionDisconnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksDisconnect, _validTasks));
				if (propErrorCount > 0)
				{
					OnCollectionError(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksDisconnect, _errorTasks));
				}
			}
		}

		protected internal virtual void OnError(Task task, PviEventArgs e)
		{
			propErrorCount++;
			if (_errorTasks == null)
			{
				_errorTasks = new TaskCollection(Parent, "Error tasks");
				_errorTasks.propInternalCollection = true;
			}
			if (!_errorTasks.ContainsKey(task.Name))
			{
				_errorTasks.Add(task);
			}
			Fire_Error(task, e);
			if (propValidCount + propErrorCount != Count)
			{
				return;
			}
			OnCollectionError(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.NONE, _errorTasks));
			if (propValidCount > 0)
			{
				switch (e.Action)
				{
				case Action.TasksDownload:
					OnCollectionDownloaded(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksDownload, _validTasks));
					break;
				case Action.TaskConnect:
					OnCollectionConnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksConnect, _validTasks));
					break;
				case Action.TaskDisconnect:
					OnCollectionDisconnected(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksDisconnect, _validTasks));
					break;
				case Action.TaskUpload:
					OnCollectionUploaded(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksUpload, _validTasks));
					break;
				}
			}
		}

		public virtual void OnTaskDownloaded(Task task, PviEventArgs e)
		{
			propValidCount++;
			if (_validTasks == null)
			{
				_validTasks = new TaskCollection(Parent, "Valid tasks");
				_validTasks.propInternalCollection = true;
			}
			if (!_validTasks.ContainsKey(task.Name))
			{
				_validTasks.Add(task);
			}
			if (this.Downloaded != null)
			{
				this.Downloaded(task, e);
			}
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionDownloaded(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksDownload, _validTasks));
				if (propErrorCount > 0)
				{
					OnCollectionError(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksDownload, _errorTasks));
				}
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
				OnCollectionDownloadProgress(new ModuleCollectionProgressEventArgs(propName, "", 0, Service.Language, Action.TasksDownload, task, _percentage));
			}
		}

		internal virtual void OnTaskDeleted(ModuleEventArgs moduleEvent)
		{
			if (this.ModuleDeleted != null)
			{
				this.ModuleDeleted(this, moduleEvent);
			}
		}

		public virtual void OnTaskUploaded(Task task, PviEventArgs e)
		{
			propValidCount++;
			if (_validTasks == null)
			{
				_validTasks = new TaskCollection(Parent, "Valid tasks");
				_validTasks.propInternalCollection = true;
			}
			if (!_validTasks.ContainsKey(task.Name))
			{
				_validTasks.Add(task);
			}
			if (this.TaskUploaded != null)
			{
				this.TaskUploaded(task, e);
			}
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionUploaded(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksUpload, _validTasks));
				if (propErrorCount > 0)
				{
					OnCollectionError(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksUpload, _errorTasks));
				}
			}
		}

		protected void OnCollectionDownloaded(CollectionEventArgs e)
		{
			if (this.CollectionDownloaded != null)
			{
				this.CollectionDownloaded(this, e);
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

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				object propParent = base.propParent;
				object propUserData = base.propUserData;
				string propName = base.propName;
				if (Parent is Cpu && ((Cpu)Parent).propUserTaskCollections != null && ((Cpu)Parent).propUserTaskCollections.ContainsKey(Name))
				{
					((Cpu)Parent).propUserTaskCollections.Remove(Name);
				}
				CleanUp(disposing);
				if (_validTasks != null)
				{
					_validTasks.Dispose(disposing, removeFromCollection: false);
				}
				if (_errorTasks != null)
				{
					_errorTasks.Dispose(disposing, removeFromCollection: false);
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

		public void CleanUp()
		{
			CleanUp(disposing: false);
		}

		internal void CleanUp(bool disposing)
		{
			int num = 0;
			ArrayList arrayList = new ArrayList();
			if (_validTasks != null)
			{
				_validTasks.Clear();
			}
			if (_errorTasks != null)
			{
				_errorTasks.Clear();
			}
			if (Values != null)
			{
				foreach (Task value in Values)
				{
					arrayList.Add(value);
				}
				for (num = 0; num < arrayList.Count; num++)
				{
					Task task = (Task)arrayList[num];
					if (task.LinkId != 0)
					{
						task.DisconnectRet(0u);
					}
					task.Dispose(disposing, removeFromCollection: true);
				}
			}
			base.Clear();
		}

		protected internal virtual void OnUploadProgress(Module module, ModuleEventArgs e)
		{
			if (this.UploadProgress != null)
			{
				this.UploadProgress(module, e);
			}
		}

		internal virtual Task GetItem(int index)
		{
			return (Task)ElementAt(index);
		}

		internal virtual Task GetItem(string address)
		{
			return (Task)base[address];
		}

		public virtual int Add(Task task)
		{
			if (this[task.Name] == null)
			{
				base.Add(task.Name, task);
			}
			else
			{
				this[task.Name].Initialize(task);
			}
			if (!propInternalCollection)
			{
				if (task.propUserCollections == null)
				{
					task.propUserCollections = new Hashtable();
				}
				if (!task.propUserCollections.ContainsKey(Name))
				{
					task.propUserCollections.Add(Name, this);
				}
			}
			return 0;
		}

		public override void Remove(string key)
		{
			if (_validTasks != null && _validTasks.ContainsKey(key))
			{
				_validTasks.Remove(key);
			}
			if (_errorTasks != null && _errorTasks.ContainsKey(key))
			{
				_errorTasks.Remove(key);
			}
			base.Remove(key);
		}

		public override void Remove(object key)
		{
			if (key is Task)
			{
				Remove(((Task)key).Name);
			}
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

		public void Upload()
		{
			Upload(ModuleListOptions.INA2000CompatibleMode);
		}

		public void Upload(ModuleListOptions lstOption)
		{
			base.Requests |= Actions.Upload;
			int num = UpdateTaskList(lstOption);
			if (0 < num)
			{
				OnError(new CollectionEventArgs(((Base)propParent).Name, ((Base)propParent).Address, num, Service.Language, Action.ModulesUpload, null));
			}
		}

		internal int UpdateTaskList(ModuleListOptions lstOption)
		{
			_UploadOption = lstOption;
			if (!((Cpu)propParent).IsConnected)
			{
				base.Requests |= Actions.Upload;
				return -2;
			}
			propValidCount = 0;
			propErrorCount = 0;
			if (_errorTasks != null)
			{
				_errorTasks.Clear();
			}
			if (_validTasks != null)
			{
				_validTasks.Clear();
			}
			if (((Cpu)Parent).Connection.DeviceType == DeviceType.ANSLTcp)
			{
				return PInvokePvicom.PviComReadArgumentRequest(Service, ((Cpu)propParent).LinkId, AccessTypes.ANSL_ModuleList, IntPtr.Zero, 0, 415u, base.InternId);
			}
			return PInvokePvicom.PviComReadArgumentRequest(Service, ((Cpu)propParent).LinkId, AccessTypes.ModuleList, IntPtr.Zero, 0, 415u, base.InternId);
		}

		public void Upload(string path)
		{
			propSentCount = 0;
			propValidCount = 0;
			if (_validTasks == null)
			{
				_validTasks = new TaskCollection(Parent, "Valid tasks");
				_validTasks.propInternalCollection = true;
			}
			else
			{
				_validTasks.Clear();
			}
			if (0 < Count)
			{
				foreach (Task value in Values)
				{
					value.Upload($"{path}\\{value.Name}.br");
					propSentCount++;
				}
			}
		}

		private void TaskListFromCB(int errorCode, IntPtr pData, uint dataLen, bool isANSL)
		{
			int updateFlags = 0;
			Hashtable hashtable = new Hashtable();
			if (isANSL)
			{
				Hashtable hashtable2 = new Hashtable();
				hashtable2 = ((Cpu)Parent).ReadANSLMODList(pData, dataLen);
				foreach (ModuleInfoDescription value in hashtable2.Values)
				{
					if (value.name != null && Module.isTaskObject(value.type) && ValidateAdd(Parent, value.modListed))
					{
						hashtable.Add(value.name, value.name);
						UpdateModuleInfo(value, errorCode, ref updateFlags, (((Cpu)propParent).BootMode == BootMode.Diagnostics) ? true : false);
					}
				}
			}
			else
			{
				int num = (int)(dataLen / 164u);
				for (int i = 0; i < num; i++)
				{
					APIFC_ModulInfoRes moduleInfoStruct = PviMarshal.PtrToModulInfoStructure(PviMarshal.GetIntPtr(pData, (ulong)(i * 164)), typeof(APIFC_ModulInfoRes));
					if (moduleInfoStruct.name != null && Module.isTaskObject(moduleInfoStruct.type))
					{
						hashtable.Add(moduleInfoStruct.name, moduleInfoStruct.name);
						UpdateModuleInfo(moduleInfoStruct, errorCode, ref updateFlags);
					}
				}
			}
			if (synchronize || 1 == (updateFlags & 1))
			{
				DoSynchronize(hashtable);
			}
			CheckFireUploadEvents((4224 != errorCode) ? errorCode : 0, Action.TasksUpload, Action.TasksConnect);
			OnCollectionUploaded(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksUpload, _validTasks));
			if (propErrorCount > 0)
			{
				OnCollectionError(new TaskCollectionEventArgs(propName, "", 0, Service.Language, Action.TasksUpload, _errorTasks));
			}
		}

		private void OldTaskListFromCB(int errorCode, IntPtr pData, uint dataLen)
		{
			if (errorCode == 0 && 0 < dataLen)
			{
				string text = "";
				text = PviMarshal.PtrToStringAnsi(pData, dataLen);
				int num = text.IndexOf("\0");
				if (-1 != num)
				{
					text = text.Substring(0, num);
				}
				string[] array = null;
				if (text != "")
				{
					array = text.Split("\t".ToCharArray());
					for (int i = 0; i < array.Length; i++)
					{
						if (propParent is Cpu)
						{
							new Task((Cpu)propParent, array[i], this);
						}
					}
				}
				OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.TasksUpload, Service));
			}
			else
			{
				OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.TasksUpload, Service));
				OnError(new CollectionEventArgs(propName, "", errorCode, Service.Language, Action.TasksUpload, null));
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (PVIReadAccessTypes.ModuleList == accessType)
			{
				TaskListFromCB(errorCode, pData, dataLen, isANSL: false);
			}
			else if (PVIReadAccessTypes.ANSL_ModuleList == accessType)
			{
				TaskListFromCB(errorCode, pData, dataLen, isANSL: true);
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
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
					num = ((Task)value).ToXMLTextWriter(ref writer, flags);
					if (num != 0)
					{
						result = num;
					}
				}
				writer.WriteEndElement();
			}
			return result;
		}

		internal int DiagnosticModeUpdateModuleInfo(APIFC_DiagModulInfoRes diagModInfo, int errorCode, ref int updateFlags)
		{
			if (errorCode != 0)
			{
				return errorCode;
			}
			if ((base.Requests & Actions.Upload) != 0)
			{
				updateFlags |= 4;
			}
			Task task = null;
			task = ((Cpu)propParent).Tasks[diagModInfo.name];
			if (task != null)
			{
				task.updateProperties(diagModInfo);
				if ((task.Requests & Actions.Upload) != 0)
				{
					task.Requests &= ~Actions.Upload;
					OnTaskUploaded(task, new PviEventArgs(propName, "", errorCode, Service.Language, Action.TaskUpload, Service));
				}
				if (task.CheckModuleInfo(errorCode))
				{
					task.Fire_OnConnected(new PviEventArgs(task.Name, task.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
			}
			return 0;
		}

		internal int UpdateModuleInfo(APIFC_ModulInfoRes moduleInfoStruct, int errorCode, ref int updateFlags)
		{
			ModuleInfoDescription moduleInfoDescription = new ModuleInfoDescription();
			moduleInfoDescription.Init(moduleInfoStruct);
			return UpdateModuleInfo(moduleInfoDescription, errorCode, ref updateFlags, diagList: false);
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
				updateFlags |= 4;
			}
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			Task task = null;
			if (Module.isTaskObject(moduleInfoStruct.type))
			{
				if (0 < Count)
				{
					foreach (Task value in ((Cpu)propParent).Tasks.Values)
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
				task = ((Cpu)propParent).Tasks[moduleInfoStruct.name];
				if (flag && task == null && ValidateAdd(Parent, moduleInfoStruct.modListed))
				{
					task = new Task((Cpu)propParent, moduleInfoStruct.name);
				}
				if (task != null && (flag || (task.Requests & Actions.Upload) != 0 || (task.Requests & Actions.ModuleInfo) != 0))
				{
					task.updateProperties(moduleInfoStruct, diagList);
				}
			}
			if (task != null)
			{
				if ((task.Requests & Actions.Upload) != 0)
				{
					task.Requests &= ~Actions.Upload;
					OnTaskUploaded(task, new PviEventArgs(propName, "", errorCode, Service.Language, Action.TaskUpload, Service));
				}
				if (task.CheckModuleInfo(errorCode))
				{
					task.Fire_OnConnected(new PviEventArgs(task.Name, task.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
			}
			for (int i = 0; i < arrayList2.Count; i++)
			{
				task = (Task)arrayList2[i];
				task.Fire_OnConnected(new PviEventArgs(task.Name, task.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				task = (Task)arrayList[i];
				OnTaskUploaded(task, new PviEventArgs(task.Name, task.Address, errorCode, Service.Language, Action.TaskUpload, Service));
			}
			return 0;
		}

		internal void DoSynchronize(Hashtable newItems)
		{
			Hashtable hashtable = new Hashtable();
			if (Values != null)
			{
				foreach (Task value in Values)
				{
					if (value.Address == null && 0 < value.Address.Length)
					{
						if ((newItems != null || !newItems.ContainsKey(value.Address)) && !hashtable.ContainsKey(value.Name))
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
				foreach (Task value2 in hashtable.Values)
				{
					RemoveFromCollection(value2, Action.TaskDelete);
				}
				hashtable.Clear();
			}
			hashtable = null;
		}
	}
}
