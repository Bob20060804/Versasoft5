using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
	public class VariableCollection : BaseCollection
	{
		private int propActiveCount;

		private int propDeactiveCount;

		private int propDatavalidCount;

		private VariableCollection propErrorVariables;

		private VariableCollection propValidVariables;

		private VariableCollection propDataValidVariables;

		private VariableCollection propActiveVariables;

		private VariableCollection propDeactiveVariables;

		private bool propWriteValueAutomatic;

		private double propHysteresis;

		private Scaling propScaling;

		private bool propPolling;

		private Access propVariableAccess;

		public Variable this[string name]
		{
			get
			{
				return (Variable)base[name];
			}
		}

		internal bool DataValid
		{
			get
			{
				if (Count == propDatavalidCount)
				{
					return true;
				}
				return false;
			}
		}

		public bool Active
		{
			get
			{
				return propActive;
			}
			set
			{
				base.Requests |= Actions.SetActive;
				propActive = value;
				propActiveCount = 0;
				propDeactiveCount = 0;
				propErrorCount = 0;
				propSentCount = 0;
				if (propActiveVariables != null)
				{
					propActiveVariables.Clear();
				}
				if (propDeactiveVariables != null)
				{
					propDeactiveVariables.Clear();
				}
				if (propErrorVariables != null)
				{
					propErrorVariables.Clear();
				}
				if (Count == 0)
				{
					base.Requests &= ~Actions.SetActive;
					if (value)
					{
						OnCollectionActivated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesActivate, propActiveVariables));
					}
					else
					{
						OnCollectionDeactivated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDeactivate, propDeactiveVariables));
					}
				}
				else
				{
					if (propCollectionType != CollectionType.HashTable)
					{
						return;
					}
					foreach (Variable value2 in Values)
					{
						if (propActive)
						{
							propSentCount++;
							if (value2.Active)
							{
								if (value2.ErrorCode != 0)
								{
									propErrorCount++;
								}
								else
								{
									propActiveCount++;
								}
							}
							else if (propValidVariables != null)
							{
								value2.Active = propActive;
							}
						}
						else
						{
							if (!value2.Active)
							{
								if (value2.ErrorCode != 0)
								{
									propErrorCount++;
								}
								else
								{
									propDeactiveCount++;
								}
							}
							else if (propValidVariables != null)
							{
								value2.Active = propActive;
							}
							propSentCount++;
						}
					}
					if (propSentCount == propErrorCount)
					{
						if (value)
						{
							OnCollectionActivated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesActivate, propActiveVariables));
						}
						else
						{
							OnCollectionDeactivated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDeactivate, propDeactiveVariables));
						}
					}
				}
			}
		}

		public int RefreshTime
		{
			get
			{
				return propRefreshTime;
			}
			set
			{
				propRefreshTime = value;
				propValidCount = 0;
				propSentCount = 0;
				propErrorCount = 0;
				if (propValidVariables != null)
				{
					propValidVariables.Clear();
				}
				else
				{
					propValidVariables = new VariableCollection(Parent, "Valid variables");
					propValidVariables.propInternalCollection = true;
				}
				if (propErrorVariables != null)
				{
					propErrorVariables.Clear();
				}
				else
				{
					propErrorVariables = new VariableCollection(Parent, "Error variables");
					propErrorVariables.propInternalCollection = true;
				}
				if (propCollectionType == CollectionType.HashTable && 0 < Count)
				{
					foreach (Variable value2 in Values)
					{
						propSentCount++;
						value2.RefreshTime = propRefreshTime;
					}
				}
			}
		}

		public bool WriteValueAutomatic
		{
			get
			{
				return propWriteValueAutomatic;
			}
			set
			{
				propWriteValueAutomatic = value;
				if (0 < Count)
				{
					foreach (Variable value2 in Values)
					{
						value2.WriteValueAutomatic = propWriteValueAutomatic;
					}
				}
			}
		}

		public double Hysteresis
		{
			get
			{
				return propHysteresis;
			}
			set
			{
				propHysteresis = value;
				propValidCount = 0;
				propSentCount = 0;
				propErrorCount = 0;
				if (propValidVariables != null)
				{
					propValidVariables.Clear();
				}
				else
				{
					propValidVariables = new VariableCollection(Parent, "Valid variables");
					propValidVariables.propInternalCollection = true;
				}
				if (propErrorVariables != null)
				{
					propErrorVariables.Clear();
				}
				else
				{
					propErrorVariables = new VariableCollection(Parent, "Error variables");
					propErrorVariables.propInternalCollection = true;
				}
				if (propCollectionType == CollectionType.HashTable && 0 < Count)
				{
					foreach (Variable value2 in Values)
					{
						propSentCount++;
						value2.Hysteresis = propHysteresis;
					}
				}
			}
		}

		[CLSCompliant(false)]
		public Scaling Scaling
		{
			get
			{
				return propScaling;
			}
			set
			{
				propScaling = value;
				if (propCollectionType == CollectionType.HashTable && 0 < Count)
				{
					foreach (Variable value2 in Values)
					{
						value2.propScaling = propScaling;
					}
				}
			}
		}

		public bool Polling
		{
			get
			{
				return propPolling;
			}
			set
			{
				propPolling = value;
				propValidCount = 0;
				propSentCount = 0;
				propErrorCount = 0;
				if (propValidVariables != null)
				{
					propValidVariables.Clear();
				}
				else
				{
					propValidVariables = new VariableCollection(Parent, "Valid variables");
					propValidVariables.propInternalCollection = true;
				}
				if (propErrorVariables != null)
				{
					propErrorVariables.Clear();
				}
				else
				{
					propErrorVariables = new VariableCollection(Parent, "Error variables");
					propErrorVariables.propInternalCollection = true;
				}
				if (propCollectionType == CollectionType.HashTable && 0 < Count)
				{
					foreach (Variable value2 in Values)
					{
						propSentCount++;
						value2.Polling = propPolling;
					}
				}
			}
		}

		public Access Access
		{
			get
			{
				return propVariableAccess;
			}
			set
			{
				propVariableAccess = value;
				propValidCount = 0;
				propSentCount = 0;
				propErrorCount = 0;
				if (propValidVariables != null)
				{
					propValidVariables.Clear();
				}
				else
				{
					propValidVariables = new VariableCollection(Parent, "Valid variables");
					propValidVariables.propInternalCollection = true;
				}
				if (propErrorVariables != null)
				{
					propErrorVariables.Clear();
				}
				else
				{
					propErrorVariables = new VariableCollection(Parent, "Error variables");
					propErrorVariables.propInternalCollection = true;
				}
				if (propCollectionType == CollectionType.HashTable && 0 < Count)
				{
					foreach (Variable value2 in Values)
					{
						propSentCount++;
						value2.Access = propVariableAccess;
					}
				}
			}
		}

		public event CollectionEventHandler CollectionDataValidated;

		public event CollectionEventHandler CollectionActivated;

		public event CollectionEventHandler CollectionDeactivated;

		public event CollectionEventHandler CollectionValuesRead;

		public event CollectionEventHandler CollectionValuesWritten;

		public event CollectionEventHandler CollectionPropertyChanged;

		public event PviEventHandler DataValidated;

		public event PviEventHandler Activated;

		public event PviEventHandler Deactivated;

		public event PviEventHandler ValueRead;

		public event PviEventHandler ValueWritten;

		public event VariableEventHandler ValueChanged;

		public event PviEventHandler PropertyChanged;

		public VariableCollection(object parent, string name)
			: base(parent, name)
		{
			propParent = parent;
		}

		internal VariableCollection(CollectionType colType, object parent, string name)
			: base(colType, parent, name)
		{
			propParent = parent;
		}

		public override void Connect(ConnectionType connectionType)
		{
			propValidCount = 0;
			propErrorCount = 0;
			propSentCount = 0;
			base.Requests |= Actions.Connect;
			int num = 0;
			if (ConnectionStates.Connected == propConnectionState)
			{
				OnCollectionConnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesConnect, propValidVariables));
				return;
			}
			if (propValidVariables != null)
			{
				propValidVariables.Clear();
			}
			else
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (propErrorVariables != null)
			{
				propErrorVariables.Clear();
			}
			else
			{
				propErrorVariables = new VariableCollection(Parent, "Error variables");
				propErrorVariables.propInternalCollection = true;
			}
			if (propDataValidVariables != null)
			{
				propDataValidVariables.Clear();
			}
			else
			{
				propDataValidVariables = new VariableCollection(Parent, "Valid variables");
				propDataValidVariables.propInternalCollection = true;
			}
			if (Count == 0)
			{
				base.Requests &= ~Actions.Connect;
				OnCollectionConnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesConnect, propValidVariables));
			}
			else if (propCollectionType == CollectionType.HashTable)
			{
				foreach (Variable value in Values)
				{
					propSentCount++;
					if (value.IsConnected)
					{
						num++;
						propValidVariables.Add(value);
						propValidCount++;
						if (num + propErrorCount == Count)
						{
							OnCollectionConnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesConnect, propValidVariables));
						}
					}
					else
					{
						if (propActive)
						{
							value.Active = true;
						}
						value.Connect(connectionType);
					}
				}
			}
		}

		protected internal override void OnCollectionConnected(CollectionEventArgs e)
		{
			base.OnCollectionConnected(e);
		}

		public void Disconnect(bool noResponse)
		{
			if (noResponse)
			{
				DisconnectObjects(noResponse: true);
				propValidCount = 0;
				propErrorCount = 0;
				propSentCount = 0;
				propDatavalidCount = 0;
				propConnectionState = ConnectionStates.Disconnected;
			}
			else
			{
				Disconnect();
			}
		}

		public void Disconnect()
		{
			propValidCount = 0;
			propErrorCount = 0;
			propSentCount = 0;
			propDatavalidCount = 0;
			if (Values == null || Count == 0)
			{
				OnCollectionDisconnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, this));
				return;
			}
			base.Requests |= Actions.Disconnect;
			if (propValidVariables != null)
			{
				propValidVariables.Clear();
			}
			else
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (propErrorVariables != null)
			{
				propErrorVariables.Clear();
			}
			else
			{
				propErrorVariables = new VariableCollection(Parent, "Error variables");
				propErrorVariables.propInternalCollection = true;
			}
			if (Count == 0)
			{
				base.Requests &= ~Actions.Disconnect;
				OnCollectionDisconnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, propValidVariables));
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
			if (propCollectionType == CollectionType.HashTable)
			{
				if (Values == null)
				{
					return 0;
				}
				propDisconnectedCount = Count;
				result = 0;
				if (propValidVariables != null)
				{
					propValidVariables.Clear();
				}
				{
					foreach (Variable value in Values)
					{
						if (!noResponse)
						{
							num++;
							if (!value.IsConnected && propValidVariables != null)
							{
								propValidCount++;
								propValidVariables.Add(value);
							}
						}
						result = value.Disconnect(602u, noResponse);
						propSentCount++;
						if (result != 0)
						{
							if (!noResponse)
							{
								value.FireDisconnected(result, Action.VariablesDisconnect);
							}
							propDisconnectedCount--;
							if (propDisconnectedCount == 0)
							{
								if (noResponse)
								{
									return result;
								}
								OnCollectionDisconnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, propValidVariables));
								return result;
							}
						}
						if (num == Count)
						{
							return result;
						}
					}
					return result;
				}
			}
			return result;
		}

		protected internal override void OnCollectionDisconnected(CollectionEventArgs e)
		{
			base.OnCollectionDisconnected(e);
		}

		public override void Add(object key, object value)
		{
			Variable variable = (Variable)value;
			if (!base.ContainsKey(key))
			{
				base.Add(key, variable);
			}
			if (propParent is Cpu)
			{
				variable.propParent = (Base)propParent;
			}
			if (!propInternalCollection)
			{
				if (variable.propUserCollections == null)
				{
					variable.propUserCollections = new Hashtable();
				}
				if (!variable.propUserCollections.ContainsKey(Name))
				{
					variable.propUserCollections.Add(Name, this);
				}
			}
		}

		public virtual void Add(Variable variable)
		{
			if (variable == null || variable.Name == null)
			{
				return;
			}
			if (!base.ContainsKey(variable.Name))
			{
				base.Add(variable.Name, variable);
			}
			if (propParent is Cpu)
			{
				variable.propParent = (Base)propParent;
			}
			if (!propInternalCollection)
			{
				if (variable.propUserCollections == null)
				{
					variable.propUserCollections = new Hashtable();
				}
				if (!variable.propUserCollections.ContainsKey(Name))
				{
					variable.propUserCollections.Add(Name, this);
				}
			}
		}

		public override void Remove(string key)
		{
			if (base.ContainsKey(key))
			{
				base.Remove(key);
				if (propValidVariables != null)
				{
					propValidVariables.Remove(key);
				}
				if (propErrorVariables != null)
				{
					propErrorVariables.Remove(key);
				}
				if (propDataValidVariables != null)
				{
					propDataValidVariables.Remove(key);
				}
				if (propActiveVariables != null)
				{
					propActiveVariables.Remove(key);
				}
				if (propDeactiveVariables != null)
				{
					propDeactiveVariables.Remove(key);
				}
			}
		}

		public virtual void Remove(Variable variable)
		{
			if (base.ContainsKey(variable.Name))
			{
				Remove(variable.Name);
			}
		}

		public void WriteScaling()
		{
			propValidCount = 0;
			propSentCount = 0;
			propErrorCount = 0;
			if (propValidVariables != null)
			{
				propValidVariables.Clear();
			}
			else
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (propErrorVariables != null)
			{
				propErrorVariables.Clear();
			}
			else
			{
				propErrorVariables = new VariableCollection(Parent, "Error variables");
				propErrorVariables.propInternalCollection = true;
			}
			if (propCollectionType == CollectionType.HashTable && 0 < Count)
			{
				foreach (Variable value in Values)
				{
					propSentCount++;
					value.WriteScaling();
				}
			}
		}

		public void WriteValues()
		{
			int num = 0;
			propValidCount = 0;
			propSentCount = 0;
			propErrorCount = 0;
			base.Requests |= Actions.SetValue;
			if (propValidVariables != null)
			{
				propValidVariables.Clear();
			}
			else
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (propErrorVariables != null)
			{
				propErrorVariables.Clear();
			}
			else
			{
				propErrorVariables = new VariableCollection(Parent, "Error variables");
				propErrorVariables.propInternalCollection = true;
			}
			if (Count == 0)
			{
				base.Requests &= ~Actions.SetValue;
				OnCollectionValuesWritten(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariableValueWrite, propValidVariables));
			}
			else if (propCollectionType == CollectionType.HashTable)
			{
				foreach (Variable value in Values)
				{
					propSentCount++;
					num = value.WriteValue();
					if (num != 0)
					{
						OnError(value, new PviEventArgs(value.propName, value.propAddress, num, Service.Language, Action.VariableValueWrite, Service));
						OnValueWritten(value, new PviEventArgs(value.Name, value.Address, num, Service.Language, Action.VariableValueWrite, Service));
					}
				}
			}
		}

		protected internal virtual void OnConnected(Variable variable, PviEventArgs e)
		{
			propValidCount++;
			if (propValidVariables == null)
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (!propValidVariables.ContainsKey(variable.Name))
			{
				propValidVariables.Add(variable);
			}
			Fire_Connected(variable, e);
			if (propValidCount + propErrorCount == propSentCount && (base.Requests & Actions.Connect) != 0)
			{
				base.Requests &= ~Actions.Connect;
				OnCollectionConnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesConnect, propValidVariables));
				if (propErrorCount > 0)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesConnect, propErrorVariables));
				}
			}
		}

		protected internal virtual void OnDisconnected(Variable variable, PviEventArgs e)
		{
			propConnectionState = ConnectionStates.Disconnected;
			propValidCount++;
			if (propValidVariables == null)
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (!propValidVariables.ContainsKey(variable.Name))
			{
				propValidVariables.Add(variable);
			}
			Fire_Disconnected(variable, e);
			if (propValidCount == propSentCount && (base.Requests & Actions.Disconnect) != 0)
			{
				base.Requests &= ~Actions.Disconnect;
				OnCollectionDisconnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, propValidVariables));
				if (propErrorCount > 0)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, propErrorVariables));
				}
			}
		}

		protected internal virtual void OnError(Variable variable, PviEventArgs e)
		{
			if (propErrorVariables == null)
			{
				propErrorVariables = new VariableCollection(Parent, "Error variables");
				propErrorVariables.propInternalCollection = true;
			}
			if (variable != null && !propErrorVariables.ContainsKey(variable.Name))
			{
				propErrorVariables.Add(variable);
			}
			Fire_Error(variable, e);
			if (propValidCount + propErrorCount == Count)
			{
				if (e.Action == Action.VariableConnect)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesConnect, propErrorVariables));
				}
				else if (e.Action == Action.VariableDisconnect)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, propErrorVariables));
				}
				else if (e.Action == Action.VariablesDisconnect)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, propErrorVariables));
				}
				else if (e.Action == Action.VariableActivate)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesActivate, propErrorVariables));
				}
				else if (e.Action == Action.VariableDeactivate)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDeactivate, propErrorVariables));
				}
				else
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.NONE, propErrorVariables));
				}
				propErrorVariables.Clear();
				if (propValidCount > 0 && e.Action == Action.VariableConnect)
				{
					OnCollectionConnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesConnect, propValidVariables));
				}
				else if (propValidCount > 0 && e.Action == Action.VariableDisconnect)
				{
					OnCollectionDisconnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, propValidVariables));
				}
				else if (propValidCount > 0 && e.Action == Action.VariablesDisconnect)
				{
					OnCollectionDisconnected(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDisconnect, propValidVariables));
				}
				else if (propValidCount > 0 && e.Action == Action.VariableActivate)
				{
					OnCollectionActivated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesActivate, propValidVariables));
				}
				else if (propValidCount > 0 && e.Action == Action.VariableDeactivate)
				{
					OnCollectionDeactivated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDeactivate, propValidVariables));
				}
			}
		}

		protected internal virtual void OnValueChanged(Variable variable, VariableEventArgs e)
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged(variable, e);
			}
		}

		protected internal virtual void OnDataValidated(Variable variable, PviEventArgs e)
		{
			propDatavalidCount++;
			if (propDataValidVariables == null)
			{
				propDataValidVariables = new VariableCollection(Parent, "Data validated variables");
				propDataValidVariables.propInternalCollection = true;
			}
			if (!propDataValidVariables.ContainsKey(variable.Name))
			{
				propDataValidVariables.Add(variable);
			}
			if (this.DataValidated != null)
			{
				this.DataValidated(variable, e);
			}
			if (propDatavalidCount + propErrorCount == Count)
			{
				OnCollectionDataValidated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDataValid, propDataValidVariables));
			}
		}

		protected internal virtual void OnActivated(Variable variable, PviEventArgs e)
		{
			propActiveCount++;
			if (propActiveVariables == null)
			{
				propActiveVariables = new VariableCollection(Parent, "Active variables");
				propActiveVariables.propInternalCollection = true;
			}
			if (!propActiveVariables.ContainsKey(variable.Name))
			{
				propActiveVariables.Add(variable);
			}
			if (this.Activated != null)
			{
				this.Activated(variable, e);
			}
			if (propActiveCount + propErrorCount == propSentCount && (base.Requests & Actions.SetActive) != 0)
			{
				base.Requests &= ~Actions.SetActive;
				OnCollectionActivated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesActivate, propActiveVariables));
			}
		}

		protected internal virtual void OnDeactivated(Variable variable, PviEventArgs e)
		{
			propDeactiveCount++;
			if (propDeactiveVariables == null)
			{
				propDeactiveVariables = new VariableCollection(Parent, "Deactive variables");
				propDeactiveVariables.propInternalCollection = true;
			}
			if (!propDeactiveVariables.ContainsKey(variable.Name))
			{
				propDeactiveVariables.Add(variable);
			}
			if (this.Deactivated != null)
			{
				this.Deactivated(variable, e);
			}
			if (propDeactiveCount + propErrorCount == propSentCount && (base.Requests & Actions.SetActive) != 0)
			{
				base.Requests &= ~Actions.SetActive;
				OnCollectionDeactivated(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesDeactivate, propDeactiveVariables));
			}
		}

		protected internal virtual void OnValueRead(Variable variable, PviEventArgs e)
		{
			propValidCount++;
			if (propValidVariables == null)
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (!propValidVariables.ContainsKey(variable.Name))
			{
				propValidVariables.Add(variable);
			}
			if (this.ValueRead != null)
			{
				this.ValueRead(variable, e);
			}
			if (propValidCount + propErrorCount == propSentCount && (base.Requests & Actions.GetValue) != 0)
			{
				base.Requests &= ~Actions.GetValue;
				OnCollectionValuesRead(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesValuesRead, propValidVariables));
				if (propErrorCount > 0)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesValuesRead, propErrorVariables));
				}
			}
		}

		protected internal virtual void OnValueWritten(Variable variable, PviEventArgs e)
		{
			propValidCount++;
			if (propValidVariables == null)
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (!propValidVariables.ContainsKey(variable.Name))
			{
				propValidVariables.Add(variable);
			}
			if (this.ValueWritten != null)
			{
				this.ValueWritten(variable, e);
			}
			if (propValidCount + propErrorCount == propSentCount && (base.Requests & Actions.SetValue) != 0)
			{
				base.Requests &= ~Actions.SetValue;
				OnCollectionValuesWritten(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariableValueWrite, propValidVariables));
				if (propErrorCount > 0)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, Action.VariablesValuesRead, propErrorVariables));
				}
			}
		}

		protected internal virtual void OnPropertyChanged(Variable variable, PviEventArgs e)
		{
			propValidCount++;
			if (propValidVariables == null)
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (!propValidVariables.ContainsKey(variable.Name))
			{
				propValidVariables.Add(variable);
			}
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(variable, e);
			}
			if (propValidCount + propErrorCount == propSentCount)
			{
				OnCollectionPropertyChanged(new VariableCollectionEventArgs(propName, "", 0, Service.Language, e.Action, propValidVariables));
				if (propErrorCount > 0)
				{
					OnCollectionError(new VariableCollectionEventArgs(propName, "", 0, Service.Language, e.Action, propErrorVariables));
				}
			}
		}

		protected internal virtual void OnCollectionDataValidated(CollectionEventArgs e)
		{
			if (this.CollectionDataValidated != null)
			{
				this.CollectionDataValidated(this, e);
			}
		}

		protected internal virtual void OnCollectionActivated(CollectionEventArgs e)
		{
			if (this.CollectionActivated != null)
			{
				this.CollectionActivated(this, e);
			}
		}

		protected internal virtual void OnCollectionDeactivated(CollectionEventArgs e)
		{
			if (this.CollectionDeactivated != null)
			{
				this.CollectionDeactivated(this, e);
			}
		}

		protected internal virtual void OnCollectionValuesRead(CollectionEventArgs e)
		{
			if (this.CollectionValuesRead != null)
			{
				this.CollectionValuesRead(this, e);
			}
		}

		protected internal virtual void OnCollectionValuesWritten(CollectionEventArgs e)
		{
			if (this.CollectionValuesWritten != null)
			{
				this.CollectionValuesWritten(this, e);
			}
		}

		protected internal virtual void OnCollectionPropertyChanged(CollectionEventArgs e)
		{
			if (this.CollectionPropertyChanged != null)
			{
				this.CollectionPropertyChanged(this, e);
			}
		}

		public void Upload()
		{
			int num = 0;
			if (propParent is Cpu && !((Cpu)propParent).IsConnected && Service.WaitForParentConnection)
			{
				base.Requests |= Actions.Upload;
				return;
			}
			if (propParent is Service && !((Service)propParent).IsConnected && Service.WaitForParentConnection)
			{
				base.Requests |= Actions.Upload;
				return;
			}
			if (propParent is Task && !((Task)propParent).IsConnected && Service.WaitForParentConnection)
			{
				base.Requests |= Actions.Upload;
				return;
			}
			num = ((propParent is Cpu) ? PInvokePvicom.PviComReadArgumentRequest(((Cpu)propParent).Service, ((Cpu)propParent).LinkId, AccessTypes.ListVariable, IntPtr.Zero, 0, 614u, base.InternId) : ((propParent is Task) ? PInvokePvicom.PviComReadArgumentRequest(((Task)propParent).Service, ((Task)propParent).LinkId, AccessTypes.ListVariable, IntPtr.Zero, 0, 614u, base.InternId) : ((!(propParent is Service)) ? (-1) : PInvokePvicom.PviComReadArgumentRequest(((Service)propParent).Service, ((Service)propParent).LinkId, AccessTypes.List, IntPtr.Zero, 0, 614u, base.InternId))));
			if (num != 0)
			{
				OnError(null, new VariableCollectionEventArgs(((Base)propParent).Name, ((Base)propParent).Address, num, Service.Language, Action.VariablesUpload, null));
			}
		}

		protected internal override void OnUploaded(PviEventArgs e)
		{
			base.OnUploaded(e);
		}

		public void ReadValues()
		{
			propValidCount = 0;
			propSentCount = 0;
			propErrorCount = 0;
			base.Requests |= Actions.GetValue;
			if (propValidVariables != null)
			{
				propValidVariables.Clear();
			}
			else
			{
				propValidVariables = new VariableCollection(Parent, "Valid variables");
				propValidVariables.propInternalCollection = true;
			}
			if (propErrorVariables != null)
			{
				propErrorVariables.Clear();
			}
			else
			{
				propErrorVariables = new VariableCollection(Parent, "Error variables");
				propErrorVariables.propInternalCollection = true;
			}
			if (propCollectionType == CollectionType.HashTable && 0 < Count)
			{
				foreach (Variable value in Values)
				{
					propSentCount++;
					value.ReadValue();
				}
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			string[] array = null;
			string[] array2 = null;
			bool isMDimArray = false;
			if (PVIReadAccessTypes.Variables == accessType)
			{
				if (errorCode == 0 && dataLen != 0)
				{
					string text = "";
					Variable variable = null;
					text = PviMarshal.PtrToStringAnsi(pData, dataLen);
					array = null;
					if (text != null && 1 < text.Length)
					{
						array = text.Split("\t".ToCharArray());
						for (int i = 0; i < array.Length; i++)
						{
							string text2 = array[i];
							int num = text2.IndexOf("\0");
							if (-1 != num)
							{
								text2 = text2.Substring(0, num);
							}
							if (propParent is Cpu && !((Cpu)propParent).Variables.ContainsKey(Variable.GetVariableName(text2)))
							{
								variable = new Variable((Cpu)propParent, expandMembers: true, Variable.GetVariableName(text2), this);
							}
							if (propParent is Task)
							{
								if (Scope.Global != Variable.EvaluateScope(text2))
								{
									if (!((Task)propParent).Variables.ContainsKey(Variable.GetVariableName(text2)))
									{
										variable = new Variable((Task)propParent, expandMembers: true, Variable.GetVariableName(text2), this);
										variable.propScope = Scope.Local;
									}
								}
								else
								{
									Variable variable2 = null;
									if ((variable2 = ((Task)propParent).propCpu.Variables[Variable.GetVariableName(text2)]) == null)
									{
										variable = new Variable(((Task)propParent).Cpu, expandMembers: true, Variable.GetVariableName(text2), ((Task)propParent).Cpu.Variables);
										variable.propScope = Scope.Global;
										((Task)propParent).propGlobals.Add(variable);
									}
									else if (variable2.Name != null && !((Task)propParent).propGlobals.ContainsKey(variable2.Name))
									{
										((Task)propParent).propGlobals.Add(variable2);
									}
								}
							}
							if (variable != null)
							{
								variable.GetScope(text2, ref variable.propScope);
								variable.GetExtendedAttributes(text2, ref isMDimArray, ref variable.propPviValue);
								if (isMDimArray)
								{
									variable.propPviValue.SetArrayIndex(text2);
								}
								variable.propPviValue.SetDataType(Variable.GetDataType(text2, variable.Value.IsBitString, ref variable.propPviValue.propTypeLength));
								variable.propPviValue.SetArrayLength(Variable.GetArrayLength(text2));
								if (variable.propPviValue.ArrayMinIndex == 0)
								{
									variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
								}
								variable.propPviValue.propTypeLength = Variable.GetDataTypeLength(text2, variable.propPviValue.propTypeLength);
							}
							variable = null;
						}
					}
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.VariablesUpload, Service));
				}
				else
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.VariablesUpload, Service));
					OnError(null, new VariableCollectionEventArgs(propName, "", errorCode, Service.Language, Action.VariablesUpload, null));
				}
			}
			else if (PVIReadAccessTypes.ChildObjects == accessType)
			{
				if (errorCode == 0 && dataLen != 0)
				{
					string text3 = "";
					text3 = PviMarshal.PtrToStringAnsi(pData, dataLen);
					int num2 = text3.IndexOf("\0");
					if (-1 != num2)
					{
						text3 = text3.Substring(0, num2);
					}
					array = null;
					if (text3 != "")
					{
						array = text3.Split("\t".ToCharArray());
						foreach (string text2 in array)
						{
							array2 = text2.Split(" ".ToCharArray());
							if (array2[1].CompareTo("OT=Pvar") == 0 && propParent is Service && !((Service)propParent).Variables.ContainsKey(Variable.GetVariableName(text2)))
							{
								new Variable((Service)propParent, Variable.GetVariableName(text2), isUploded: true);
							}
						}
					}
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.VariablesUpload, Service));
				}
				else
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.VariablesUpload, Service));
					OnError(null, new VariableCollectionEventArgs(propName, "", errorCode, Service.Language, Action.VariablesUpload, null));
				}
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
			if (Count > 0)
			{
				writer.WriteStartElement(GetType().Name);
				foreach (object value in Values)
				{
					num = ((Variable)value).ToXMLTextWriter(ref writer, flags);
					if (num != 0)
					{
						result = num;
					}
				}
				writer.WriteEndElement();
			}
			return result;
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				if (propErrorVariables != null)
				{
					propErrorVariables.Dispose(disposing, removeFromCollection);
				}
				if (propValidVariables != null)
				{
					propValidVariables.Dispose(disposing, removeFromCollection);
				}
				if (propDataValidVariables != null)
				{
					propDataValidVariables.Dispose(disposing, removeFromCollection);
				}
				if (propActiveVariables != null)
				{
					propActiveVariables.Dispose(disposing, removeFromCollection);
				}
				if (propDeactiveVariables != null)
				{
					propDeactiveVariables.Dispose(disposing, removeFromCollection);
				}
				propScaling = null;
				CleanUp(disposing);
				base.Dispose(disposing, removeFromCollection);
			}
		}

		public override void Clear()
		{
			if (!propInternalCollection)
			{
				foreach (Variable value in Values)
				{
					if (value.propUserCollections != null)
					{
						value.propUserCollections.Remove(Name);
					}
				}
			}
			base.Clear();
		}

		internal void CleanUp(bool disposing)
		{
			propCounter = 0;
			ArrayList arrayList = new ArrayList();
			if (Values != null)
			{
				foreach (Variable value in Values)
				{
					arrayList.Add(value);
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					Variable variable = (Variable)arrayList[i];
					if (variable.LinkId != 0)
					{
						variable.Disconnect(0u);
					}
					variable.Dispose(disposing, removeFromCollection: true);
				}
			}
			arrayList.Clear();
			arrayList = null;
			Clear();
		}
	}
}
