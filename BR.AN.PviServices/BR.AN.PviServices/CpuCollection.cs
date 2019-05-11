using System.Collections;

namespace BR.AN.PviServices
{
	public class CpuCollection : BaseCollection
	{
		private CpuCollection propValidCpus;

		private CpuCollection propErrorCpus;

		public Cpu this[string address]
		{
			get
			{
				return (Cpu)base[address];
			}
		}

		public override Service Service
		{
			get
			{
				if (propParent is Service)
				{
					return (Service)propParent;
				}
				return null;
			}
		}

		public event PviEventHandler Restarted;

		public event CollectionEventHandler CollectionRestarted;

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				object propParent = base.propParent;
				object propUserData = base.propUserData;
				string propName = base.propName;
				CleanUp(disposing);
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
			ArrayList arrayList = new ArrayList();
			if (Values != null && 0 < Values.Count)
			{
				foreach (object value in Values)
				{
					arrayList.Add(value);
					if (((Cpu)value).LinkId != 0)
					{
						((Cpu)value).Disconnect(0u);
					}
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				object obj = arrayList[i];
				((Cpu)obj).Dispose(disposing, removeFromCollection: true);
				obj = null;
			}
			Clear();
		}

		public override void Remove(string key)
		{
			if (base.ContainsKey(key))
			{
				base.Remove(key);
				if (propValidCpus != null)
				{
					propValidCpus.Remove(key);
				}
				if (propErrorCpus != null)
				{
					propErrorCpus.Remove(key);
				}
			}
		}

		public virtual void Remove(Cpu cpuObj)
		{
			if (base.ContainsKey(cpuObj.Name))
			{
				Remove(cpuObj.Name);
			}
		}

		public CpuCollection(object parent, string name)
			: base(parent, name)
		{
		}

		internal CpuCollection(CollectionType colType, object parent, string name)
			: base(colType, parent, name)
		{
		}

		public override void Connect()
		{
			propValidCount = 0;
			propErrorCount = 0;
			propSentCount = 0;
			if (ConnectionStates.Connecting != propConnectionState)
			{
				propConnectionState = ConnectionStates.Connecting;
				if (propValidCpus == null)
				{
					propValidCpus = new CpuCollection(propParent, "Valid cpus");
					propValidCpus.propInternalCollection = true;
				}
				else
				{
					propValidCpus.Clear();
				}
				if (propErrorCpus == null)
				{
					propErrorCpus = new CpuCollection(propParent, "Error cpus");
					propErrorCpus.propInternalCollection = true;
				}
				else
				{
					propErrorCpus.Clear();
				}
				if (propCollectionType == CollectionType.HashTable && Values != null)
				{
					foreach (Cpu value in Values)
					{
						propSentCount++;
						if (ConnectionStates.Connected == value.propConnectionState)
						{
							propValidCpus.Add(value);
							propValidCount++;
						}
						else
						{
							value.Connected += cpu_Connected;
							value.Connect();
						}
					}
				}
			}
		}

		private void cpu_Connected(object sender, PviEventArgs e)
		{
			((Cpu)sender).Connected -= cpu_Connected;
			OnConnected((Cpu)sender, e);
		}

		public void Disconnect()
		{
			int num = 0;
			if (ConnectionStates.Disconnecting == propConnectionState)
			{
				return;
			}
			propConnectionState = ConnectionStates.Disconnecting;
			if (Values == null || Values.Count == 0)
			{
				OnCollectionDisconnected(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusDisconnect, this));
				return;
			}
			propValidCount = 0;
			propErrorCount = 0;
			propSentCount = 0;
			if (propValidCpus != null)
			{
				propValidCpus.Clear();
			}
			else
			{
				propValidCpus = new CpuCollection(Parent, "Valid cpus");
			}
			if (propErrorCpus != null)
			{
				propErrorCpus.Clear();
			}
			else
			{
				propErrorCpus = new CpuCollection(Parent, "Error cpus");
			}
			if (propCollectionType != CollectionType.HashTable || Values == null || 0 >= Values.Count)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			foreach (Cpu value in Values)
			{
				arrayList.Add(value);
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				Cpu cpu = (Cpu)arrayList[i];
				cpu.Disconnected += cpu_Disconnected;
				num = cpu.DisconnectRet(202u);
				propSentCount++;
				if (num != 0)
				{
					cpu.FireDisconnected(num, Action.CpuDisconnect);
				}
			}
			if (propSentCount == 0 || (propValidCount + propErrorCount == propSentCount && propSentCount == Count))
			{
				OnCollectionDisconnected(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusDisconnect, this));
			}
		}

		private void cpu_Disconnected(object sender, PviEventArgs e)
		{
			((Cpu)sender).Disconnected -= cpu_Disconnected;
			OnDisconnected((Cpu)sender, e);
		}

		public void Restart(BootMode bootMode)
		{
			propValidCount = 0;
			propErrorCount = 0;
			propSentCount = 0;
			if (propValidCpus != null)
			{
				propValidCpus.Clear();
			}
			else
			{
				propValidCpus = new CpuCollection(Parent, "Valid cpus");
			}
			if (propErrorCpus != null)
			{
				propErrorCpus.Clear();
			}
			else
			{
				propErrorCpus = new CpuCollection(Parent, "Error cpus");
			}
			if (propCollectionType == CollectionType.HashTable && Values != null)
			{
				foreach (Cpu value in Values)
				{
					if (value.ErrorCode == 0 || value.ErrorCode == 12002)
					{
						if (!value.IsConnected)
						{
							propValidCount++;
						}
						else
						{
							value.Restart(bootMode);
						}
						propSentCount++;
					}
				}
			}
		}

		internal virtual Cpu GetItem(int index)
		{
			return (Cpu)ElementAt(index);
		}

		internal virtual Cpu GetItem(string address)
		{
			return (Cpu)base[address];
		}

		public virtual void Add(Cpu cpu)
		{
			if (cpu == null || cpu.Name == null)
			{
				return;
			}
			if (!base.ContainsKey(cpu.Name))
			{
				base.Add(cpu.Name, cpu);
			}
			if (!propInternalCollection)
			{
				if (cpu.propUserCollections == null)
				{
					cpu.propUserCollections = new Hashtable();
				}
				if (!cpu.propUserCollections.ContainsKey(Name))
				{
					cpu.propUserCollections.Add(Name, this);
				}
			}
		}

		protected internal virtual void OnConnected(Cpu cpu, PviEventArgs e)
		{
			propValidCount++;
			if (propValidCpus == null)
			{
				propValidCpus = new CpuCollection(Parent, "Valid cpus");
				propValidCpus.propInternalCollection = true;
			}
			if (!propValidCpus.ContainsKey(cpu.Name))
			{
				propValidCpus.Add(cpu);
			}
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionConnected(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusConnect, propValidCpus));
				if (propErrorCount > 0)
				{
					OnCollectionError(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusConnect, propErrorCpus));
				}
			}
		}

		protected internal virtual void OnDisconnected(Cpu cpu, PviEventArgs e)
		{
			propValidCount++;
			if (propValidCpus == null)
			{
				propValidCpus = new CpuCollection(Parent, "Valid cpus");
				propValidCpus.propInternalCollection = true;
			}
			if (!propValidCpus.ContainsKey(cpu.Name))
			{
				propValidCpus.Add(cpu);
			}
			if (propValidCount + propErrorCount == propSentCount && propSentCount == Count)
			{
				OnCollectionDisconnected(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusDisconnect, propValidCpus));
				if (propErrorCount > 0)
				{
					OnCollectionError(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusConnect, propErrorCpus));
				}
			}
		}

		protected internal virtual void OnRestarted(Cpu cpu, PviEventArgs e)
		{
			propValidCount++;
			if (propValidCpus == null)
			{
				propValidCpus = new CpuCollection(Parent, "Valid cpus");
				propValidCpus.propInternalCollection = true;
			}
			if (!propValidCpus.ContainsKey(cpu.Name))
			{
				propValidCpus.Add(cpu);
			}
			if (this.Restarted != null)
			{
				this.Restarted(cpu, e);
			}
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionRestarted(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusRestart, propValidCpus));
				if (propErrorCount > 0)
				{
					OnCollectionError(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusRestart, propErrorCpus));
				}
			}
		}

		protected internal virtual void OnError(Cpu cpu, PviEventArgs e)
		{
			if (cpu == null || cpu.Name == null)
			{
				return;
			}
			if (propErrorCpus == null)
			{
				propErrorCpus = new CpuCollection(Parent, "Error cpus");
				propErrorCpus.propInternalCollection = true;
			}
			if (!propErrorCpus.ContainsKey(cpu.Name))
			{
				propErrorCpus.Add(cpu);
				propErrorCount++;
			}
			Fire_Error(cpu, e);
			if (propValidCount + propErrorCount == Count)
			{
				OnCollectionError(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusConnect, propErrorCpus));
				if (propValidCount > 0 && e.Action == Action.CpuConnect)
				{
					OnCollectionConnected(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusConnect, propValidCpus));
				}
				else if (propValidCount > 0 && e.Action == Action.CpusDisconnect)
				{
					OnCollectionDisconnected(new CpuCollectionEventArgs(propName, "", 0, Service.Language, Action.CpusDisconnect, propValidCpus));
				}
			}
		}

		protected internal virtual void OnCollectionRestarted(CollectionEventArgs e)
		{
			propValidCount = 0;
			propSentCount = 0;
			if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
			{
				if (ConnectionStates.Connected != propConnectionState)
				{
					propConnectionState = ConnectionStates.Connected;
					if (this.CollectionRestarted != null)
					{
						this.CollectionRestarted(this, e);
					}
				}
			}
			else if (ConnectionStates.ConnectedError > propConnectionState && ConnectionStates.Unininitialized < propConnectionState)
			{
				propConnectionState = ConnectionStates.ConnectedError;
				if (this.CollectionRestarted != null)
				{
					this.CollectionRestarted(this, e);
				}
			}
		}
	}
}
