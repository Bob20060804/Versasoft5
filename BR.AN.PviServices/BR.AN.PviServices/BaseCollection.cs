using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace BR.AN.PviServices
{
	public abstract class BaseCollection : PviCBEvents, IDisposable, ICollection, IEnumerable
	{
		internal CollectionType propCollectionType;

		internal bool propDisposed;

		internal object propParent;

		internal string propName;

		internal SortedList propSortedList;

		internal SortedDictionary<object, object> propSortedDict;

		internal Hashtable propHashTable;

		internal ArrayList propArrayList;

		internal Actions propResponses;

		internal Actions propRequests;

		internal IEnumerator propEnumer;

		internal ConnectionStates propConnectionState;

		internal bool propActive;

		internal int propRefreshTime;

		internal int propCounter;

		internal bool propInternalCollection;

		private Service propService;

		internal object propUserData;

		internal int propValidCount;

		internal int propSentCount;

		internal int propErrorCount;

		internal int propDisconnectedCount;

		public virtual int Count
		{
			get
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
					if (propArrayList != null)
					{
						return propArrayList.Count;
					}
					return 0;
				case CollectionType.SortedList:
					if (propSortedList != null)
					{
						return propSortedList.Count;
					}
					return 0;
				case CollectionType.SortedDictionary:
					if (propSortedDict != null)
					{
						return propSortedDict.Count;
					}
					return 0;
				default:
					if (propHashTable != null)
					{
						return propHashTable.Count;
					}
					return 0;
				}
			}
		}

		public object this[object indexer]
		{
			get
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
					if (propArrayList != null)
					{
						return propArrayList[Convert.ToInt32(indexer)];
					}
					return null;
				case CollectionType.SortedList:
					if (propSortedList != null)
					{
						return propSortedList[indexer];
					}
					return null;
				case CollectionType.SortedDictionary:
					if (propSortedDict != null)
					{
						return propSortedDict[indexer];
					}
					return null;
				default:
					if (propHashTable != null)
					{
						return propHashTable[indexer];
					}
					return null;
				}
			}
		}

		public virtual object SyncRoot
		{
			get
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
					return propArrayList.SyncRoot;
				case CollectionType.SortedList:
					return propSortedList.SyncRoot;
				case CollectionType.SortedDictionary:
					return null;
				default:
					return propHashTable.SyncRoot;
				}
			}
		}

		public virtual object Parent => propParent;

		public virtual object Name => propName;

		public virtual ICollection Values
		{
			get
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
					return propArrayList;
				case CollectionType.SortedList:
					if (propSortedList != null)
					{
						return propSortedList.Values;
					}
					return propSortedList;
				case CollectionType.SortedDictionary:
					if (propSortedDict != null)
					{
						return propSortedDict.Values;
					}
					return propSortedDict;
				default:
					if (propHashTable != null)
					{
						return propHashTable.Values;
					}
					return propHashTable;
				}
			}
		}

		public virtual ICollection Keys
		{
			get
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
				{
					Hashtable hashtable = new Hashtable();
					for (int i = 0; i < propArrayList.Count; i++)
					{
						hashtable.Add(i, i);
					}
					return hashtable.Keys;
				}
				case CollectionType.SortedList:
					return propSortedList.Keys;
				case CollectionType.SortedDictionary:
					return propSortedDict.Keys;
				default:
					return propHashTable.Keys;
				}
			}
		}

		public virtual bool IsSynchronized
		{
			get
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
					return propArrayList.IsSynchronized;
				case CollectionType.SortedList:
					return propSortedList.IsSynchronized;
				case CollectionType.SortedDictionary:
					return false;
				default:
					return propHashTable.IsSynchronized;
				}
			}
		}

		internal Actions Requests
		{
			get
			{
				return propRequests;
			}
			set
			{
				propRequests = value;
			}
		}

		internal Actions Responses
		{
			get
			{
				return propResponses;
			}
			set
			{
				propResponses = value;
			}
		}

		internal uint InternId => _internId;

		public bool HasError
		{
			get
			{
				if (propErrorCount > 0)
				{
					return true;
				}
				return false;
			}
		}

		public virtual Service Service => propService;

		public object UserData
		{
			get
			{
				return propUserData;
			}
			set
			{
				propUserData = value;
			}
		}

		internal bool IsConnected
		{
			get
			{
				if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
				{
					return true;
				}
				return false;
			}
		}

		public event DisposeEventHandler Disposing;

		public event PviEventHandler Connected;

		public event PviEventHandler Disconnected;

		public event PviEventHandler Error;

		public event CollectionEventHandler CollectionConnected;

		public event CollectionEventHandler CollectionDisconnected;

		public event CollectionEventHandler CollectionError;

		public event PviEventHandler Uploaded;

		internal BaseCollection()
		{
			propConnectionState = ConnectionStates.Unininitialized;
			propInternalCollection = false;
			propUserData = null;
			propService = null;
			propDisposed = false;
			propCollectionType = CollectionType.HashTable;
			propParent = null;
			propName = "";
			propHashTable = new Hashtable();
			propSortedList = null;
			propSortedDict = null;
		}

		internal BaseCollection(object parentObj, string name)
		{
			propConnectionState = ConnectionStates.Unininitialized;
			propUserData = null;
			propDisposed = false;
			propCollectionType = CollectionType.HashTable;
			propParent = parentObj;
			propService = null;
			if (parentObj is Cpu)
			{
				propService = ((Cpu)parentObj).Service;
			}
			else if (parentObj is Task)
			{
				propService = ((Task)parentObj).Service;
			}
			else if (parentObj is Variable)
			{
				propService = ((Variable)parentObj).Service;
			}
			else if (parentObj is Service)
			{
				propService = ((Service)parentObj).Service;
			}
			propName = name;
			propHashTable = new Hashtable();
			propSortedList = null;
			propSortedDict = null;
			AddToCBReceivers();
		}

		internal BaseCollection(CollectionType colType, object parentObj, string name)
		{
			propConnectionState = ConnectionStates.Unininitialized;
			propUserData = null;
			propService = null;
			propParent = parentObj;
			if (parentObj is Cpu)
			{
				propService = ((Cpu)parentObj).Service;
			}
			else if (parentObj is Task)
			{
				propService = ((Task)parentObj).Service;
			}
			else if (parentObj is Variable)
			{
				propService = ((Variable)parentObj).Service;
			}
			else if (parentObj is Service)
			{
				propService = ((Service)parentObj).Service;
			}
			propDisposed = false;
			propCollectionType = colType;
			propName = name;
			switch (colType)
			{
			case CollectionType.SortedList:
				propSortedList = new SortedList(new Comparer());
				break;
			case CollectionType.ArrayList:
				propArrayList = new ArrayList(1);
				break;
			case CollectionType.SortedDictionary:
				propSortedDict = new SortedDictionary<object, object>();
				break;
			default:
				propHashTable = new Hashtable();
				break;
			}
			AddToCBReceivers();
		}

		internal void InitItems(BaseCollection copyElements)
		{
			if (copyElements != null)
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
					propArrayList = (ArrayList)copyElements.Clone();
					break;
				case CollectionType.SortedList:
					propSortedList = (SortedList)copyElements.Clone();
					break;
				case CollectionType.SortedDictionary:
					propSortedDict = (SortedDictionary<object, object>)copyElements.Clone();
					break;
				default:
					propHashTable = (Hashtable)copyElements.Clone();
					break;
				}
			}
		}

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		public virtual void Connect()
		{
			Connect(ConnectionType.CreateAndLink);
		}

		public virtual void Connect(ConnectionType connectionType)
		{
			propValidCount = 0;
			propErrorCount = 0;
			propSentCount = 0;
			Requests |= Actions.Connect;
			int num = 0;
			if (Count == 0)
			{
				Requests &= ~Actions.Connect;
				OnCollectionConnected(new CollectionEventArgs(propName, "", 0, Service.Language, Action.ConnectedEvent, null), forceEvent: true);
			}
			else
			{
				foreach (Base value in Values)
				{
					propSentCount++;
					if (ConnectionStates.Connected == value.propConnectionState)
					{
						num++;
						propValidCount++;
						if (num + propErrorCount == Count)
						{
							OnCollectionConnected(new CollectionEventArgs(propName, "", 0, Service.Language, Action.ConnectedEvent, null), forceEvent: true);
						}
					}
					else if (ConnectionStates.ConnectedError == value.propConnectionState)
					{
						num++;
						propErrorCount++;
						if (num + propErrorCount == Count)
						{
							OnCollectionConnected(new CollectionEventArgs(propName, "", 0, Service.Language, Action.ConnectedEvent, null), forceEvent: true);
						}
					}
					else
					{
						value.Connect(connectionType);
					}
				}
			}
		}

		public void Dispose()
		{
			RemoveFromCBReceivers();
			if (!propDisposed)
			{
				Dispose(disposing: true, removeFromCollection: false);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing, bool removeFromCollection)
		{
			RemoveFromCBReceivers();
			if (!propDisposed)
			{
				OnDisposing(disposing);
				if (disposing)
				{
					propDisposed = true;
				}
				propName = null;
				propParent = null;
				if (propArrayList != null)
				{
					propArrayList.Clear();
					propArrayList = null;
				}
				if (propEnumer != null)
				{
					propEnumer = null;
				}
				if (propHashTable != null)
				{
					propHashTable.Clear();
					propHashTable = null;
				}
				if (propSortedList != null)
				{
					propSortedList.Clear();
					propSortedList = null;
				}
				propUserData = null;
			}
		}

		protected internal virtual void OnUploaded(PviEventArgs e)
		{
			if (this.Uploaded != null)
			{
				this.Uploaded(this, e);
			}
		}

		protected internal virtual void OnDisconnected(Base sender, PviEventArgs e)
		{
			propValidCount++;
			Fire_Disconnected(sender, e);
		}

		protected internal virtual void OnError(Base sender, PviEventArgs e)
		{
			if (this.Error != null)
			{
				this.Error(sender, e);
			}
			if (propValidCount + propErrorCount == Count)
			{
				OnCollectionError(new CollectionEventArgs(propName, "", 0, Service.Language, Action.NONE, null));
			}
		}

		protected internal virtual void OnCollectionConnected(CollectionEventArgs e)
		{
			OnCollectionConnected(e, forceEvent: false);
		}

		protected internal virtual void OnCollectionConnected(CollectionEventArgs e, bool forceEvent)
		{
			propValidCount = 0;
			propSentCount = 0;
			if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
			{
				if (ConnectionStates.Connected != propConnectionState || forceEvent)
				{
					propConnectionState = ConnectionStates.Connected;
					if (this.CollectionConnected != null)
					{
						this.CollectionConnected(this, e);
					}
				}
			}
			else
			{
				propConnectionState = ConnectionStates.ConnectedError;
				if (this.CollectionConnected != null)
				{
					this.CollectionConnected(this, e);
				}
			}
		}

		protected internal virtual void OnConnected(Base sender, PviEventArgs e)
		{
			propValidCount++;
			if (this.Connected != null)
			{
				this.Connected(sender, e);
			}
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionConnected(new CollectionEventArgs(propName, "", 0, Service.Language, Action.ConnectedEvent, null));
				if (propErrorCount > 0)
				{
					OnCollectionError(new CollectionEventArgs(propName, "", 0, Service.Language, Action.ConnectedEvent, null));
				}
			}
		}

		protected internal virtual void OnCollectionDisconnected(CollectionEventArgs e)
		{
			propValidCount = 0;
			propSentCount = 0;
			propConnectionState = ConnectionStates.Disconnected;
			if (this.CollectionDisconnected != null)
			{
				this.CollectionDisconnected(this, e);
			}
		}

		protected internal virtual void OnCollectionError(CollectionEventArgs e)
		{
			propErrorCount = 0;
			if (this.CollectionError != null)
			{
				this.CollectionError(this, e);
			}
		}

		internal object Clone()
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				return propArrayList.Clone();
			case CollectionType.HashTable:
				return propHashTable.Clone();
			case CollectionType.SortedList:
				return propSortedList.Clone();
			case CollectionType.SortedDictionary:
				return new SortedDictionary<object, object>(propSortedDict);
			default:
				return null;
			}
		}

		internal void RemoveFromCollection(Base remObj, Action nAction)
		{
			remObj.DisconnectRet(2821u);
			remObj.RemoveReferences();
			remObj.RemoveFromBaseCollections();
			remObj.RemoveObject();
			RemoveFromBaseCollections(remObj.Name, 0);
			remObj.Fire_Deleted(new PviEventArgs(remObj.Name, remObj.Address, 0, remObj.Service.Language, nAction, Service));
			Remove(remObj.Name);
		}

		internal virtual void RemoveFromBaseCollections(string logicalName, int mode)
		{
			if (Service != null)
			{
				Service.LogicalObjects.Remove(logicalName);
				if (Service.Services != null)
				{
					Service.Services.LogicalObjects.Remove(logicalName);
				}
			}
		}

		private void RemoveFromCBReceivers()
		{
			if (Service != null)
			{
				Service.RemoveID(_internId);
			}
			if (0 < Count)
			{
				foreach (Base value in Values)
				{
					value.RemoveFromCBReceivers();
				}
			}
		}

		private bool AddToCBReceivers()
		{
			if (Service != null)
			{
				return Service.AddID(this, ref _internId);
			}
			return false;
		}

		public virtual void Clear()
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				if (propArrayList != null)
				{
					propArrayList.Clear();
				}
				break;
			case CollectionType.HashTable:
				if (propHashTable != null)
				{
					propHashTable.Clear();
				}
				break;
			case CollectionType.SortedList:
				if (propSortedList != null)
				{
					propSortedList.Clear();
				}
				break;
			case CollectionType.SortedDictionary:
				if (propSortedDict != null)
				{
					propSortedDict.Clear();
				}
				break;
			}
		}

		public virtual bool Contains(string key)
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				return propArrayList.Contains(key);
			case CollectionType.SortedList:
				return propSortedList.Contains(key);
			case CollectionType.SortedDictionary:
				return propSortedDict.Keys.FirstOrDefault((object item) => key.Equals(item.ToString())) != null;
			default:
				return propHashTable.Contains(key);
			}
		}

		public virtual bool Contains(object valObj)
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				return propArrayList.Contains(valObj);
			case CollectionType.SortedList:
				return propSortedList.Contains(valObj);
			case CollectionType.SortedDictionary:
				return propSortedDict.ContainsValue(valObj);
			default:
				return propHashTable.Contains(valObj);
			}
		}

		public virtual bool ContainsKey(object key)
		{
			if (key != null)
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
					break;
				case CollectionType.SortedList:
					if (propSortedList == null)
					{
						return false;
					}
					return propSortedList.ContainsKey(key);
				case CollectionType.SortedDictionary:
					if (propSortedDict == null)
					{
						return false;
					}
					return propSortedDict.ContainsKey(key);
				default:
					if (propHashTable == null)
					{
						return false;
					}
					return propHashTable.ContainsKey(key);
				}
				if (propArrayList == null)
				{
					return false;
				}
				if (!(key is string))
				{
					return propArrayList.Contains(key);
				}
				for (int i = 0; i < propArrayList.Count; i++)
				{
					if (((string)key).CompareTo(((Base)propArrayList[i]).Name) == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual void Remove(string key)
		{
			if (key != null)
			{
				switch (propCollectionType)
				{
				case CollectionType.ArrayList:
					propArrayList.Remove(Convert.ToInt32(key));
					break;
				case CollectionType.SortedList:
					propSortedList.Remove(key);
					break;
				case CollectionType.SortedDictionary:
					propSortedDict.Remove(key);
					break;
				default:
					propHashTable.Remove(key);
					break;
				}
			}
		}

		public virtual void Remove(object key)
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				if (key is int)
				{
					propArrayList.RemoveAt((int)key);
				}
				else
				{
					propArrayList.Remove(key);
				}
				break;
			case CollectionType.SortedList:
				propSortedList.Remove(key);
				break;
			case CollectionType.SortedDictionary:
				propSortedDict.Remove(key);
				break;
			default:
				propHashTable.Remove(key);
				break;
			}
		}

		public object ElementAt(int index)
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				if (propArrayList != null)
				{
					return propArrayList[index];
				}
				return null;
			case CollectionType.SortedList:
				if (propSortedList != null)
				{
					return propSortedList.GetByIndex(index);
				}
				return null;
			case CollectionType.SortedDictionary:
				if (propSortedDict != null)
				{
					return propSortedDict.ElementAt(index).Value;
				}
				return null;
			default:
				if (propHashTable != null)
				{
					return propHashTable[index.ToString()];
				}
				return null;
			}
		}

		public virtual void Add(object key, object value)
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				if (propArrayList != null)
				{
					propArrayList.Add(value);
				}
				break;
			case CollectionType.SortedList:
				if (propSortedList != null)
				{
					propSortedList.Add(key, value);
				}
				break;
			case CollectionType.SortedDictionary:
				if (propSortedDict != null && !propSortedDict.ContainsKey(key))
				{
					propSortedDict.Add(key, value);
				}
				break;
			default:
				if (propHashTable != null)
				{
					propHashTable.Add(key, value);
				}
				break;
			}
		}

		internal void CheckFireUploadEvents(int errorCode, Action actEvent, Action actCon)
		{
			bool flag = false;
			if ((Requests & Actions.Upload) != 0)
			{
				Requests &= ~Actions.Upload;
				flag = true;
			}
			if ((Requests & Actions.Uploading) != 0)
			{
				Requests &= ~Actions.Uploading;
				flag = true;
			}
			if (flag)
			{
				OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, actEvent, Service));
			}
			if ((Requests & Actions.Connect) != 0)
			{
				OnCollectionConnected(new CollectionEventArgs(propName, "", errorCode, Service.Language, actCon, this));
			}
		}

		public virtual void Add(object primKey, object secKey, object value)
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				propArrayList.Add(value);
				break;
			case CollectionType.SortedList:
				propSortedList.Add(secKey, value);
				break;
			case CollectionType.SortedDictionary:
				propSortedDict.Add(secKey, value);
				break;
			default:
				propHashTable.Add(secKey, value);
				break;
			}
		}

		public virtual IEnumerator GetEnumerator()
		{
			switch (propCollectionType)
			{
			case CollectionType.ArrayList:
				return propArrayList.GetEnumerator();
			case CollectionType.SortedList:
				return propSortedList.GetEnumerator();
			case CollectionType.SortedDictionary:
				return propSortedDict.GetEnumerator();
			default:
				return propHashTable.GetEnumerator();
			}
		}

		public virtual void CopyTo(Array array, int count)
		{
		}

		public int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base parentObj)
		{
			bool flag = false;
			Variable variable = null;
			Variable variable2 = null;
			Module module = null;
			Base @base = parentObj;
			if (reader.Name.ToLower().CompareTo("module") != 0 && reader.Name.ToLower().CompareTo("task") != 0 && reader.Name.ToLower().CompareTo("variable") != 0 && reader.Name.ToLower().CompareTo("logger") != 0 && reader.Name.ToLower().CompareTo("taskclass") != 0 && reader.Name.ToLower().CompareTo("iodatapoint") != 0 && reader.Name.ToLower().CompareTo("memory") != 0 && reader.Name.ToLower().CompareTo("library") != 0 && reader.Name.ToLower().CompareTo("members") != 0)
			{
				reader.Read();
			}
			int depth = reader.Depth;
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				if (reader.NodeType == XmlNodeType.Comment)
				{
					reader.Read();
					continue;
				}
				string attribute = reader.GetAttribute("Name");
				switch (reader.Name)
				{
				case "Module":
					module = ((Cpu)parentObj).Modules[attribute];
					if (module != null)
					{
						module.FromXmlTextReader(ref reader, flags, module);
						break;
					}
					module = new Module((Cpu)parentObj, attribute);
					module.FromXmlTextReader(ref reader, flags, module);
					break;
				case "Task":
				{
					Task task = new Task((Cpu)parentObj, attribute);
					task.FromXmlTextReader(ref reader, flags, task);
					break;
				}
				case "TaskClass":
				{
					APIFC_TkInfoRes taskClassInfo = default(APIFC_TkInfoRes);
					TaskClass.FromXmlTextReader(ref reader, flags, ref taskClassInfo);
					new TaskClass(taskClassInfo);
					break;
				}
				case "Variable":
					if (depth < reader.Depth)
					{
						@base = variable2;
					}
					else if (depth > reader.Depth)
					{
						for (int num = depth; num > reader.Depth; num--)
						{
							@base = @base.propParent;
						}
					}
					depth = reader.Depth;
					if (!flag)
					{
						variable2 = ((@base is Cpu) ? new Variable((Cpu)@base, attribute) : ((@base is Service) ? new Variable((Service)@base, attribute) : ((!(@base is Variable)) ? new Variable((Task)@base, attribute) : new Variable((Variable)@base, attribute))));
						variable2.FromXmlTextReader(ref reader, flags, variable2);
					}
					else
					{
						variable = new Variable(variable2, attribute);
						variable.FromXmlTextReader(ref reader, flags, variable);
					}
					break;
				case "Logger":
				{
					Logger logger = new Logger((Cpu)parentObj, attribute);
					logger.FromXmlTextReader(ref reader, flags, logger);
					break;
				}
				case "IODataPoint":
				{
					IODataPoint iODataPoint = new IODataPoint((Cpu)parentObj, attribute);
					iODataPoint.FromXmlTextReader(ref reader, flags, iODataPoint);
					break;
				}
				case "Memory":
				{
					Memory memory = new Memory((Cpu)parentObj, attribute);
					memory.FromXmlTextReader(ref reader, flags, memory);
					break;
				}
				case "Library":
				{
					Library library = new Library((Cpu)parentObj, attribute);
					library.FromXmlTextReader(ref reader, flags, library);
					break;
				}
				case "Members":
					if (reader.NodeType == XmlNodeType.Element)
					{
						flag = true;
					}
					reader.Read();
					break;
				default:
					return 0;
				}
				if (reader.Name == "Members" && reader.NodeType == XmlNodeType.EndElement)
				{
					flag = false;
					reader.Read();
				}
				while (reader.Name == "Variable" && reader.NodeType == XmlNodeType.EndElement)
				{
					reader.Read();
				}
			}
			reader.Read();
			return 0;
		}

		internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = 0;
			int num = 0;
			if (0 < Count && Values != null)
			{
				writer.WriteStartElement(GetType().Name);
				foreach (object value in Values)
				{
					num = ((!(value is TaskClass)) ? ((Base)value).ToXMLTextWriter(ref writer, flags) : ((TaskClass)value).ToXMLTextWriter(ref writer, flags));
					if (num != 0)
					{
						result = num;
					}
				}
				writer.WriteEndElement();
			}
			return result;
		}

		internal void Fire_Connected(object sender, PviEventArgs e)
		{
			if (this.Connected != null)
			{
				this.Connected(sender, e);
			}
		}

		internal void Fire_Disconnected(object sender, PviEventArgs e)
		{
			if (this.Disconnected != null)
			{
				this.Disconnected(sender, e);
			}
		}

		internal void Fire_CollectionDisconnected(CollectionEventArgs e)
		{
			OnCollectionDisconnected(e);
		}

		internal void Fire_Error(object sender, PviEventArgs e)
		{
			if (this.Error != null)
			{
				this.Error(sender, e);
			}
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
		}

		internal override void OnPviLinked(int errorCode, uint linkID, int option)
		{
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
		}

		internal override void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
		}

		internal override void OnPviUnLinked(int errorCode, int option)
		{
		}

		internal override void OnPviDeleted(int errorCode)
		{
		}

		internal override void OnPviChangedLink(int errorCode)
		{
		}
	}
}
