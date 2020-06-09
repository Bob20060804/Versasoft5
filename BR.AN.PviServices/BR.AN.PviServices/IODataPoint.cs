using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	public class IODataPoint : Base
	{
		private IOVariable propForceVar;

		private IOVariable propConsumerVar;

		private IOVariable propProducerVar;

		private int propIOConnection;

		private int propIOActivation;

		private int propIOValidated;

		private bool propDataValid;

		private int propMissingValueEvent;

		private bool propSimulated;

		private Value propPviValue;

		private Value propInternalValue;

		private Value propPhysicalValue;

		private Value propForceValue;

		private Base propOwner;

		private Cpu propCpu;

		private int propRefreshTime;

		private bool propActive;

		[CLSCompliant(false)]
		public Value PhysicalValue
		{
			get
			{
				return propPhysicalValue;
			}
		}

		[CLSCompliant(false)]
		public Value ForceValue
		{
			get
			{
				return propForceValue;
			}
			set
			{
				propForceVar.WriteIOValue(value);
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
				propForceVar.RefreshTime = propRefreshTime;
				propProducerVar.RefreshTime = propRefreshTime;
				propConsumerVar.RefreshTime = propRefreshTime;
			}
		}

		[CLSCompliant(false)]
		public Value Value
		{
			get
			{
				return propPviValue;
			}
		}

		public bool Force
		{
			get
			{
				return propConsumerVar.Force;
			}
			set
			{
				propConsumerVar.Force = value;
			}
		}

		public override string FullName
		{
			get
			{
				if (base.Name != null && 0 < base.Name.Length)
				{
					return propOwner.FullName + "." + base.Name;
				}
				if (propOwner != null)
				{
					return propOwner.FullName;
				}
				return "";
			}
		}

		public override string PviPathName
		{
			get
			{
				if (base.Name != null && 0 < base.Name.Length)
				{
					return propOwner.PviPathName + "/\"" + propName + "\" OT=Pvar";
				}
				return propOwner.PviPathName;
			}
		}

		public bool Simulated => propSimulated;

		public Direction Direction
		{
			get
			{
				if (-1 != base.Name.IndexOf("%Q"))
				{
					return Direction.Output;
				}
				return Direction.Input;
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
				SetActive(value);
			}
		}

		public event PviEventHandler ForcedOn;

		public event PviEventHandler ForcedOff;

		public event PviEventHandler ForceValueWritten;

		public event PviEventHandler ValueChanged;

		public event PviEventHandler PhysicalValueChanged;

		public event PviEventHandler ForceValueChanged;

		public event PviEventHandler ValueRead;

		public event PviEventHandler Deactivated;

		public event PviEventHandler Activated;

		public event PviEventHandler DataValidated;

		public IODataPoint(Cpu cpu, string name)
			: base(cpu)
		{
			propMissingValueEvent = 0;
			propDataValid = false;
			propInternalValue = new Value();
			propActive = true;
			propName = name;
			propCpu = cpu;
			propCpu.IODataPoints.Add(this);
			propRefreshTime = 100;
			propPviValue = new Value(0);
			propPhysicalValue = new Value(0);
			propForceValue = new Value(0);
			propOwner = cpu;
			propIOConnection = 0;
			propIOActivation = 0;
			propIOValidated = 0;
			propForceVar = new IOVariable(cpu, name, IOVariableTypes.FORCE);
			propConsumerVar = new IOVariable(cpu, name, IOVariableTypes.VALUE);
			propProducerVar = new IOVariable(cpu, name, IOVariableTypes.PHYSICAL);
			propSimulated = false;
			AddIOVariableEvents();
		}

		private void AddIOVariableEvents()
		{
			propForceVar.Error += ForceVar_Error;
			propForceVar.Disconnected += ForceVar_Disconnected;
			propForceVar.DataValidated += ForceVar_DataValidated;
			propForceVar.Deactivated += ForceVar_Deactivated;
			propForceVar.Connected += ForceVar_Connected;
			propForceVar.Activated += ForceVar_Activated;
			propForceVar.ValueChanged += ForceVar_ValueChanged;
			propForceVar.ValueRead += ForceVar_ValueRead;
			propForceVar.ValueWritten += ForceVar_ValueWritten;
			propConsumerVar.Error += ConsumerVar_Error;
			propConsumerVar.Disconnected += ConsumerVar_Disconnected;
			propConsumerVar.DataValidated += ConsumerVar_DataValidated;
			propConsumerVar.Deactivated += ConsumerVar_Deactivated;
			propConsumerVar.Connected += ConsumerVar_Connected;
			propConsumerVar.Activated += ConsumerVar_Activated;
			propConsumerVar.ValueChanged += ConsumerVar_ValueChanged;
			propConsumerVar.ForcedOff += ConsumerVar_ForcedOff;
			propConsumerVar.ForcedOn += ConsumerVar_ForcedOn;
			propConsumerVar.ValueRead += ConsumerVar_ValueRead;
			propConsumerVar.StatusChanged += ConsumerVar_StatusChanged;
			propProducerVar.Error += ProducerVar_Error;
			propProducerVar.Disconnected += ProducerVar_Disconnected;
			propProducerVar.DataValidated += ProducerVar_DataValidated;
			propProducerVar.Deactivated += ProducerVar_Deactivated;
			propProducerVar.Connected += ProducerVar_Connected;
			propProducerVar.Activated += ProducerVar_Activated;
			propProducerVar.ValueChanged += ProducerVar_ValueChanged;
			propProducerVar.ValueRead += ProducerVar_ValueRead;
		}

		internal override void reCreateState()
		{
			if (reCreateActive)
			{
				propConnectionState = ConnectionStates.Unininitialized;
				propReCreateActive = true;
				reCreateActive = false;
				propForceVar.reCreateActive = false;
				propConsumerVar.reCreateActive = false;
				propProducerVar.reCreateActive = false;
				propLinkId = 0u;
				Connect();
			}
		}

		public override void Connect()
		{
			if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
			{
				Fire_ConnectedEvent(this, new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.IODataPointConnect, Service));
			}
			else if (ConnectionStates.Connecting != propConnectionState)
			{
				propConnectionState = ConnectionStates.Connecting;
				propProducerVar.Active = propActive;
				propConsumerVar.Active = propActive;
				propForceVar.Active = propActive;
				propReturnValue = 0;
				propProducerVar.Connect();
				propReturnValue = propProducerVar.ReturnValue;
			}
		}

		public override void Disconnect()
		{
			propIOConnection = 7;
			propConnectionState = ConnectionStates.Disconnecting;
			propReturnValue = 0;
			propForceVar.Disconnect();
			propReturnValue = propForceVar.ReturnValue;
			propConsumerVar.Disconnect();
			propReturnValue = propConsumerVar.ReturnValue;
			propProducerVar.Disconnect();
			propReturnValue = propProducerVar.ReturnValue;
		}

		public override void Disconnect(bool noResponse)
		{
			if (!noResponse)
			{
				Disconnect();
				return;
			}
			propConnectionState = ConnectionStates.Disconnecting;
			propIOConnection = 7;
			propReturnValue = 0;
			if (propForceVar != null)
			{
				propForceVar.Disconnect(noResponse);
				propReturnValue = propForceVar.ReturnValue;
			}
			if (propConsumerVar != null)
			{
				propConsumerVar.Disconnect(noResponse);
				propReturnValue = propConsumerVar.ReturnValue;
			}
			if (propProducerVar != null)
			{
				propProducerVar.Disconnect(noResponse);
				propReturnValue = propProducerVar.ReturnValue;
			}
			propConnectionState = ConnectionStates.Disconnected;
			propIOConnection = 0;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public int Disconnect(int internalAction)
		{
			int result = 0;
			if (propForceVar != null)
			{
				result = propForceVar.Disconnect((uint)internalAction);
			}
			if (propConsumerVar != null)
			{
				result = propConsumerVar.Disconnect((uint)internalAction);
			}
			if (propProducerVar != null)
			{
				result = propProducerVar.Disconnect((uint)internalAction);
			}
			return result;
		}

		public void ReadValue(IOVariableTypes vType)
		{
			switch (vType)
			{
			case IOVariableTypes.FORCE:
				propForceVar.ReadValue();
				break;
			case IOVariableTypes.VALUE:
				propConsumerVar.ReadValue();
				break;
			default:
				propProducerVar.ReadValue();
				break;
			}
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (propDisposed)
			{
				return;
			}
			Base propParent = base.propParent;
			string propLinkName = base.propLinkName;
			string propLogicalName = base.propLogicalName;
			object propUserData = base.propUserData;
			string propName = base.propName;
			string propAddress = base.propAddress;
			base.Dispose(disposing, removeFromCollection);
			if (disposing)
			{
				base.propParent = propParent;
				base.propLinkName = propLinkName;
				base.propLogicalName = propLogicalName;
				base.propUserData = propUserData;
				base.propName = propName;
				base.propAddress = propAddress;
				if (removeFromCollection && Parent is Cpu)
				{
					((Cpu)Parent).IODataPoints.Remove(base.Name);
				}
				if (null != propForceValue)
				{
					propForceValue.Dispose();
					propForceValue = null;
				}
				if (null != propInternalValue)
				{
					propInternalValue.Dispose();
					propInternalValue = null;
				}
				if (null != propPhysicalValue)
				{
					propPhysicalValue.Dispose();
					propPhysicalValue = null;
				}
				if (propConsumerVar != null)
				{
					propConsumerVar.Dispose();
					propConsumerVar = null;
				}
				if (propForceVar != null)
				{
					propForceVar.Dispose();
					propForceVar = null;
				}
				if (propProducerVar != null)
				{
					propProducerVar.Dispose();
					propProducerVar = null;
				}
				if (null != propPviValue)
				{
					propPviValue.Dispose();
					propPviValue = null;
				}
				if (propOwner != null)
				{
					propOwner = null;
				}
				propCpu = null;
				base.propParent = null;
				base.propLinkName = null;
				base.propLogicalName = null;
				base.propUserData = null;
				base.propName = null;
				base.propAddress = null;
			}
		}

		public override void Remove()
		{
			if (propForceVar != null)
			{
				propForceVar.Remove();
			}
			if (propConsumerVar != null)
			{
				propConsumerVar.Remove();
			}
			if (propProducerVar != null)
			{
				propProducerVar.Remove();
			}
			base.Remove();
		}

		public void ReadValue()
		{
			ReadValue(IOVariableTypes.PHYSICAL);
		}

		protected override void OnConnected(PviEventArgs e)
		{
			base.OnConnected(new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, Action.IODataPointConnect, Service));
			if ((base.Requests & Actions.SetValue) != 0)
			{
				propForceVar.Value = propInternalValue;
				base.Requests &= ~Actions.SetValue;
			}
			if (Actions.SetActive == (base.Requests & Actions.SetActive))
			{
				base.Requests &= ~Actions.SetActive;
			}
			if (Actions.SetForce == (base.Requests & Actions.SetForce))
			{
				base.Requests &= ~Actions.SetForce;
				OnForcedOn(0);
			}
			else if (propConsumerVar.Force)
			{
				OnForcedOn(0);
			}
		}

		protected override void OnDisconnected(PviEventArgs e)
		{
			if (Service != null)
			{
				base.OnDisconnected(new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, Action.IODataPointDisconnect, Service));
			}
		}

		protected virtual void OnForcedOn(int error)
		{
			if (ConnectionStates.Connected == propConnectionState)
			{
				if (this.ForcedOn != null)
				{
					this.ForcedOn(this, new PviEventArgs(base.Name, base.Address, error, Service.Language, Action.IODataPointForceOn, Service));
				}
			}
			else
			{
				base.Requests |= Actions.SetForce;
			}
		}

		protected virtual void OnForcedOff(int error)
		{
			if (ConnectionStates.Connecting == propConnectionState)
			{
				base.Requests |= Actions.GetForce;
			}
			else if (this.ForcedOff != null)
			{
				this.ForcedOff(this, new PviEventArgs(base.Name, base.Address, error, Service.Language, Action.IODataPointForceOff, Service));
			}
		}

		protected virtual void OnValueWritten(int error)
		{
			if (this.ForceValueWritten != null)
			{
				this.ForceValueWritten(this, new PviEventArgs(base.Name, base.Address, error, Service.Language, Action.VariableValueWrite, Service));
			}
		}

		protected virtual void OnForceValueChanged(Variable sender, int error)
		{
			if (propDataValid)
			{
				if (this.ForceValueChanged != null)
				{
					this.ForceValueChanged(this, new PviEventArgs(propName, propAddress, error, Service.Language, Action.ForceValueChangedEvent, Service));
				}
			}
			else
			{
				propMissingValueEvent |= 4;
			}
		}

		protected virtual void OnPhysicalValueChanged(Variable sender, int error)
		{
			if ((Direction != 0 || IOVariableTypes.PHYSICAL != ((IOVariable)sender).IOType) && (Direction.Output != Direction || IOVariableTypes.VALUE != ((IOVariable)sender).IOType))
			{
				return;
			}
			propPhysicalValue = sender.Value;
			if (propDataValid)
			{
				if (this.PhysicalValueChanged != null)
				{
					this.PhysicalValueChanged(this, new PviEventArgs(propName, propAddress, error, Service.Language, Action.PhysicalValueChangedEvent, Service));
				}
			}
			else
			{
				propMissingValueEvent |= 1;
			}
		}

		protected virtual void OnValueChanged(Variable sender, int error)
		{
			propPviValue = sender.Value;
			if (Direction.Output == Direction)
			{
				propPhysicalValue = sender.Value;
			}
			if (propDataValid)
			{
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this, new PviEventArgs(propName, propAddress, error, Service.Language, Action.VariableValueChangedEvent, Service));
				}
				if (Direction.Output == Direction)
				{
					OnPhysicalValueChanged(sender, error);
				}
			}
			else
			{
				propMissingValueEvent |= 2;
			}
		}

        protected virtual void OnValueRead(Variable sender, PviEventArgs e, IOVariableTypes ioVType)
        {
            switch (ioVType)
            {
                case IOVariableTypes.PHYSICAL:
                    if (Direction == Direction.Input)
                    {
                        propPhysicalValue = sender.Value;
                    }
                    break;
                case IOVariableTypes.VALUE:
                    propPviValue = sender.Value;
                    if (Direction.Output == Direction)
                    {
                        propPhysicalValue = sender.Value;
                    }
                    break;
                default:
                    propForceValue = sender.Value;
                    break;
            }
            if (this.ValueRead != null)
            {
                this.ValueRead(this, new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, e.Action, Service));
            }
        }

		private void SetActive(bool value)
		{
			if (value != propActive)
			{
				propActive = value;
				propForceVar.Active = propActive;
				propConsumerVar.Active = propActive;
				propProducerVar.Active = propActive;
			}
		}

		private void ForceVar_ValueChanged(object sender, VariableEventArgs e)
		{
			propForceValue = ((Variable)sender).Value;
			OnForceValueChanged((Variable)sender, e.ErrorCode);
		}

		private void ForceVar_ValueRead(object sender, PviEventArgs e)
		{
			propForceValue = ((Variable)sender).Value;
			OnValueRead((Variable)sender, e, IOVariableTypes.FORCE);
		}

		private void ForceVar_ValueWritten(object sender, PviEventArgs e)
		{
			propForceValue = ((Variable)sender).Value;
			OnValueWritten(e.ErrorCode);
		}

		private void OnDeactivated(int error)
		{
			propActive = false;
			if (this.Deactivated != null)
			{
				this.Deactivated(this, new PviEventArgs(base.Name, base.Address, error, Service.Language, Action.VariableDeactivate, Service));
			}
		}

		private void OnActivated(int error)
		{
			propActive = true;
			if (this.Activated != null)
			{
				this.Activated(this, new PviEventArgs(base.Name, base.Address, error, Service.Language, Action.VariableActivate, Service));
			}
		}

		protected virtual void OnDataValidated()
		{
			if (this.DataValidated != null)
			{
				this.DataValidated(this, new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.VariablesDataValid, Service));
			}
			if (Actions.FireActivated == (base.Requests & Actions.FireActivated))
			{
				base.Requests &= ~Actions.FireActivated;
				OnActivated(0);
			}
		}

		private void CheckValidState()
		{
			if (7 != propIOValidated || propDataValid || 7 != propIOConnection)
			{
				return;
			}
			propDataValid = true;
			OnDataValidated();
			if (2 == (2 & propMissingValueEvent))
			{
				propMissingValueEvent ^= 2;
				OnValueChanged(propConsumerVar, 0);
			}
			if (4 == (4 & propMissingValueEvent))
			{
				propMissingValueEvent ^= 4;
				OnForceValueChanged(propForceVar, 0);
			}
			if (1 == (1 & propMissingValueEvent))
			{
				propMissingValueEvent ^= 1;
				if (Direction == Direction.Input)
				{
					OnPhysicalValueChanged(propProducerVar, 0);
				}
				else
				{
					OnPhysicalValueChanged(propConsumerVar, 0);
				}
			}
		}

		private void CheckActiveState(bool doActivate, PviEventArgs e)
		{
			if (doActivate)
			{
				if (7 == propIOActivation && 7 == propIOValidated && 7 == propIOConnection)
				{
					OnActivated(e.ErrorCode);
				}
				else
				{
					base.Requests |= Actions.FireActivated;
				}
			}
			else if (propIOActivation == 0)
			{
				OnDeactivated(e.ErrorCode);
			}
		}

		private void CheckConnectionState(bool doConnect, PviEventArgs e)
		{
			if (doConnect)
			{
				if (7 == propIOConnection)
				{
					OnConnected(e);
					if (propMissingValueEvent != 0)
					{
						CheckValidState();
					}
				}
			}
			else if (propIOConnection == 0)
			{
				OnDisconnected(e);
			}
		}

		private void ForceVar_Activated(object sender, PviEventArgs e)
		{
			propIOActivation |= 4;
			CheckActiveState(doActivate: true, e);
		}

		private void ForceVar_Connected(object sender, PviEventArgs e)
		{
			propIOConnection |= 4;
			CheckConnectionState(doConnect: true, e);
			if (e.ErrorCode == 0)
			{
				propProducerVar.Active = propActive;
				propConsumerVar.Active = propActive;
				propForceVar.Active = propActive;
			}
		}

		private void ForceVar_Deactivated(object sender, PviEventArgs e)
		{
			propIOActivation ^= 4;
			CheckActiveState(doActivate: false, e);
		}

		private void ForceVar_DataValidated(object sender, PviEventArgs e)
		{
			propIOValidated |= 4;
			CheckValidState();
		}

		private void ForceVar_Disconnected(object sender, PviEventArgs e)
		{
			propIOConnection ^= 4;
			CheckConnectionState(doConnect: false, e);
		}

		private void ForceVar_Error(object sender, PviEventArgs e)
		{
			OnError(e, IOVariableTypes.FORCE);
		}

		private void ConsumerVar_ValueChanged(object sender, VariableEventArgs e)
		{
			OnValueChanged((Variable)sender, e.ErrorCode);
		}

		private void ConsumerVar_ValueRead(object sender, PviEventArgs e)
		{
			OnValueRead((Variable)sender, e, IOVariableTypes.VALUE);
		}

		private void ConsumerVar_StatusChanged(object sender, object newValue)
		{
			string text = newValue.ToString();
			int num = text.IndexOf("IO");
			propSimulated = false;
			if (-1 != num && -1 != text.IndexOf("s", num + 3))
			{
				propSimulated = true;
			}
		}

		private void ConsumerVar_Activated(object sender, PviEventArgs e)
		{
			propIOActivation |= 2;
			CheckActiveState(doActivate: true, e);
		}

		private void ConsumerVar_Connected(object sender, PviEventArgs e)
		{
			propIOConnection |= 2;
			if (e.ErrorCode == 0)
			{
				propForceVar.Connect();
				propReturnValue = propForceVar.ReturnValue;
			}
		}

		private void ConsumerVar_Deactivated(object sender, PviEventArgs e)
		{
			propIOActivation ^= 2;
			CheckActiveState(doActivate: false, e);
		}

		private void ConsumerVar_DataValidated(object sender, PviEventArgs e)
		{
			propIOValidated |= 2;
			CheckValidState();
		}

		private void ConsumerVar_Disconnected(object sender, PviEventArgs e)
		{
			propIOConnection ^= 2;
			CheckConnectionState(doConnect: false, e);
		}

		private void ConsumerVar_Error(object sender, PviEventArgs e)
		{
			OnError(e, IOVariableTypes.VALUE);
		}

		private void ProducerVar_ValueChanged(object sender, VariableEventArgs e)
		{
			OnPhysicalValueChanged((Variable)sender, e.ErrorCode);
		}

		private void ProducerVar_ValueRead(object sender, PviEventArgs e)
		{
			OnValueRead((Variable)sender, e, IOVariableTypes.PHYSICAL);
		}

		private void ProducerVar_Activated(object sender, PviEventArgs e)
		{
			propIOActivation |= 1;
			CheckActiveState(doActivate: true, e);
		}

		private void ProducerVar_Connected(object sender, PviEventArgs e)
		{
			propIOConnection |= 1;
			if (e.ErrorCode == 0)
			{
				propConsumerVar.Connect();
				propReturnValue = propConsumerVar.ReturnValue;
			}
		}

		private void ProducerVar_Deactivated(object sender, PviEventArgs e)
		{
			propIOActivation ^= 1;
			CheckActiveState(doActivate: false, e);
		}

		private void ProducerVar_DataValidated(object sender, PviEventArgs e)
		{
			propIOValidated |= 1;
			CheckValidState();
		}

		private void ProducerVar_Disconnected(object sender, PviEventArgs e)
		{
			propIOConnection ^= 1;
			CheckConnectionState(doConnect: false, e);
		}

		private void ProducerVar_Error(object sender, PviEventArgs e)
		{
			OnError(e, IOVariableTypes.PHYSICAL);
		}

		private void OnError(PviEventArgs e, IOVariableTypes ioVType)
		{
			switch (ioVType)
			{
			case IOVariableTypes.PHYSICAL:
				if (e.ErrorCode != 4808)
				{
					base.OnError(new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, e.Action, Service));
				}
				break;
			case IOVariableTypes.VALUE:
				if (e.ErrorCode != 4808)
				{
					base.OnError(new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, e.Action, Service));
				}
				break;
			default:
				base.OnError(new PviEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, e.Action, Service));
				break;
			}
			if (Service.IsRemoteError(e.ErrorCode) && propConnectionState != 0 && ConnectionStates.Disconnected != propConnectionState && ConnectionStates.Disconnecting != propConnectionState)
			{
				reCreateActive = true;
			}
		}

		private void ConsumerVar_ForcedOff(object sender, PviEventArgs e)
		{
			OnForcedOff(e.ErrorCode);
		}

		private void ConsumerVar_ForcedOn(object sender, PviEventArgs e)
		{
			OnForcedOn(e.ErrorCode);
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			base.ToXMLTextWriter(ref writer, flags);
			if (!propActive)
			{
				writer.WriteAttributeString("Active", propActive.ToString());
			}
			if (Direction == Direction.Output)
			{
				writer.WriteAttributeString("Direction", Direction.ToString());
			}
			if (propForceValue != null && propForceValue.ToString() != "" && propForceValue.ToString() != "0")
			{
				writer.WriteAttributeString("ForceValue", propForceValue.ToString());
			}
			writer.WriteAttributeString("RefreshTime", propRefreshTime.ToString());
			if (propPhysicalValue != null && propPhysicalValue.ToString() != "" && propPhysicalValue.ToString() != "0")
			{
				writer.WriteAttributeString("PhysicalValue", propPhysicalValue.ToString());
			}
			if (propPviValue != null && propPviValue.ToString() != "" && propPviValue.ToString() != "0")
			{
				writer.WriteAttributeString("PviValue", propPviValue.ToString());
			}
			if (Simulated)
			{
				writer.WriteAttributeString("Simulated", Simulated.ToString());
			}
			return 0;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base baseObj)
		{
			int result = base.FromXmlTextReader(ref reader, flags, baseObj);
			IODataPoint iODataPoint = (IODataPoint)baseObj;
			if (iODataPoint == null)
			{
				return -1;
			}
			int result2 = 0;
			uint result3 = 0u;
			string text = "";
			text = reader.GetAttribute("Active");
			if (text != null && text.Length > 0 && text.ToLower() == "false")
			{
				iODataPoint.propActive = false;
			}
			text = "";
			text = reader.GetAttribute("ForceValue");
			if (text != null && text.Length > 0)
			{
				iODataPoint.propForceValue.Assign(text);
			}
			text = "";
			text = reader.GetAttribute("PhysicalValue");
			if (text != null && text.Length > 0)
			{
				iODataPoint.propPhysicalValue.Assign(text);
			}
			text = "";
			text = reader.GetAttribute("PviValue");
			if (text != null && text.Length > 0)
			{
				iODataPoint.propPviValue.Assign(text);
			}
			text = "";
			text = reader.GetAttribute("Simulated");
			if (text != null && text.Length > 0 && text.ToLower() == "true")
			{
				iODataPoint.propSimulated = true;
			}
			text = "";
			text = reader.GetAttribute("InternID");
			if (text != null && text.Length > 0 && PviParse.TryParseUInt32(text, out result3))
			{
				iODataPoint._internId = result3;
			}
			text = "";
			text = reader.GetAttribute("RefreshTime");
			if (text != null && text.Length > 0 && PviParse.TryParseInt32(text, out result2))
			{
				iODataPoint.propRefreshTime = result2;
			}
			reader.Read();
			return result;
		}
	}
}
