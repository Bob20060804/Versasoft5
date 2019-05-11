using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
	public class Variable : Base
	{
		[Flags]
		public enum ROIoptions
		{
			OFF = 0x0,
			NonZeroBasedArrayIndex = 0x1
		}

		protected StructMemberCollection mapNameToMember;

		private bool propWaitingOnReadEvent;

		internal int propErrorState;

		private bool isMemberVar;

		private bool propReadingState;

		private bool isStruct;

		private IntPtr pWriteData;

		private IntPtr pReadData;

		private int propStrDataLen;

		private byte[] propWriteByteField;

		private byte[] propReadByteField;

		internal bool propSendChangedEvent;

		private bool propExpandMembers;

		private Value propInitialValue;

		private ROIoptions propROI;

		private string propInitValue;

		private CastModes propCastMode;

		private int propBitOffset;

		private string[] propChangedStructMembers;

		private string propStructName;

		internal Value propInternalValue;

		internal byte[] propInternalByteField;

		internal Base propOwner;

		internal Value propPviValue;

		internal bool propDataValid;

		internal int propRefreshTime;

		internal double propHysteresis;

		internal MemberCollection propMembers;

		internal bool propForceValue;

		internal VariableAttribute propAttribute;

		internal bool propActive;

		internal bool isServiceUploaded;

		internal bool propWriteValueAutomatic;

		internal Scope propScope;

		internal IConvert propConvert;

		internal int propAlignment;

		internal int propOffset;

		internal int propInternalOffset;

		internal MemberCollection propUserMembers;

		internal bool propReadOnly;

		internal ConnectionType propPVState;

		internal bool propSendUploadEvent;

		internal Hashtable propUserCollections;

		internal Access propVariableAccess;

		internal bool propPolling;

		internal ScalingPointCollection propScalingPoints;

		internal IODataPointCollection propIODataPoints;

		internal Scaling propScaling;

		internal string propUserTag;

		internal bool propWaitForUserTag;

		internal bool propStatusRead;

		private uint propPviInternStructElement;

		public Variable this[params int[] indices]
		{
			get
			{
				return GetVariable(indices);
			}
		}

		public Variable this[int index]
		{
			get
			{
				return GetVariable(index);
			}
		}

		public Variable this[string varName]
		{
			get
			{
				string[] array = varName.Split('.');
				if (array.Length > 1)
				{
					Variable variable = this;
					for (int i = 0; i < array.Length; i++)
					{
						if (variable != null)
						{
							variable = variable.Members[array[i]];
						}
					}
					return variable;
				}
				if (Members != null)
				{
					return Members[varName];
				}
				throw new InvalidOperationException();
			}
		}

		internal Base Owner
		{
			get
			{
				return propOwner;
			}
			set
			{
				propOwner = value;
			}
		}

		public string OwnerName
		{
			get
			{
				if (propOwner != null)
				{
					return propOwner.Address;
				}
				return null;
			}
		}

		public bool ExpandMembers
		{
			get
			{
				return propExpandMembers;
			}
			set
			{
				propExpandMembers = value;
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
				if (ConnectionStates.Connected == propConnectionState)
				{
					int num = 0;
					int num2 = 0;
					Marshal.WriteInt32(Service.RequestBuffer, propRefreshTime);
					num = Marshal.SizeOf(typeof(int));
					num2 = WriteRequest(Service.hPvi, base.LinkId, AccessTypes.Refresh, Service.RequestBuffer, num, 512u);
					if (num2 != 0)
					{
						OnError(new PviEventArgs(propName, propAddress, num2, Service.Language, Action.VariableSetRefreshTime, Service));
					}
				}
				else if (Service.WaitForParentConnection)
				{
					base.Requests |= Actions.SetRefresh;
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
				if ((ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState) && base.ConnectionType == ConnectionType.Link)
				{
					Disconnect(2714u);
					Connect(ConnectionType.Link, 2712);
				}
				else if (ConnectionStates.Connected == propConnectionState)
				{
					int num = 0;
					string text = value.ToString(CultureInfo.InvariantCulture);
					if ((propPviValue.DataType == DataType.Single || propPviValue.DataType == DataType.Double) && -1 == text.IndexOf('.'))
					{
						text += ".0";
					}
					Service.BuildRequestBuffer(text);
					propPviValue.propDataSize = text.Length;
					num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Hysteresis, Service.RequestBuffer, text.Length, 514u);
					if (num != 0)
					{
						OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableSetHysteresis, Service));
					}
				}
				else if (Service.WaitForParentConnection)
				{
					base.Requests |= Actions.SetHysteresis;
				}
			}
		}

		public override bool IsConnected
		{
			get
			{
				if (base.IsConnected)
				{
					return true;
				}
				if (propParent is Variable)
				{
					return ((Variable)propParent).IsConnected;
				}
				return base.IsConnected;
			}
		}

		public bool ReadOnly
		{
			get
			{
				return propReadOnly;
			}
			set
			{
				propReadOnly = value;
				if (propReadOnly)
				{
					propVariableAccess |= Access.Read;
					if (Access.Write == (propVariableAccess & Access.Write))
					{
						propVariableAccess &= ~Access.Write;
					}
				}
				else
				{
					propVariableAccess |= Access.Write;
				}
			}
		}

		public IECDataTypes IECDataType
		{
			get
			{
				if (null != propPviValue)
				{
					return propPviValue.IECDataType;
				}
				return IECDataTypes.UNDEFINED;
			}
		}

		[CLSCompliant(false)]
		public Value Value
		{
			get
			{
				return InternalGetValue();
			}
			set
			{
				InternalSetValue(value);
			}
		}

		[CLSCompliant(false)]
		public Value InitialValue
		{
			get
			{
				return propInitialValue;
			}
		}

		public MemberCollection Members => propMembers;

		public StructMemberCollection StructureMembers => mapNameToMember;

		public bool ForceValue
		{
			get
			{
				return propForceValue;
			}
			set
			{
				int num = 0;
				if (propForceValue != value)
				{
					propForceValue = value;
					if (propForceValue)
					{
						propPviValue = Value;
					}
					else
					{
						num = WriteValueForced(force: false);
					}
					if (num != 0)
					{
						OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableValueWrite, Service));
					}
				}
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

		public bool WriteValueAutomatic
		{
			get
			{
				return propWriteValueAutomatic;
			}
			set
			{
				propWriteValueAutomatic = value;
				if (propExpandMembers && propMembers != null)
				{
					for (int i = 0; i < propMembers.Count; i++)
					{
						propMembers[i].WriteValueAutomatic = propWriteValueAutomatic;
					}
				}
			}
		}

		public Scope Scope => propScope;

		public bool DataValid => propDataValid;

		public ROIoptions RuntimeObjectIndex
		{
			get
			{
				return propROI;
			}
			set
			{
				propROI = value;
			}
		}

		public override string FullName
		{
			get
			{
				if (base.Name != null)
				{
					if (base.Name.Length > 0)
					{
						if (Owner is Variable && null != ((Variable)Owner).Value && ((Variable)Owner).Value.IsOfTypeArray)
						{
							return Owner.FullName + base.Name;
						}
						if (Owner != null)
						{
							return Owner.FullName + "." + base.Name;
						}
						if (propSNMPParent != null)
						{
							return propSNMPParent.FullName + "." + base.Name;
						}
						return base.Name;
					}
					if (Owner != null)
					{
						return Owner.FullName + "." + base.Name;
					}
					return base.Name;
				}
				return "";
			}
		}

		public override string PviPathName
		{
			get
			{
				if (propOwner is Variable)
				{
					return Parent.PviPathName + "/\"" + propAddress + "\" OT=Pvar";
				}
				if (base.Name != null && 0 < base.Name.Length)
				{
					return Parent.PviPathName + "/\"" + propName + "\" OT=Pvar";
				}
				return Parent.PviPathName;
			}
		}

		public virtual string StructMemberName => getMemberName();

		internal Variable PVRoot
		{
			get
			{
				if (propParent != null)
				{
					if (propParent is Variable)
					{
						return ((Variable)propParent).PVRoot;
					}
					return this;
				}
				return null;
			}
		}

		public override Base Parent
		{
			get
			{
				if (propParent is Cpu || propParent is Task || propParent is Service)
				{
					return propParent;
				}
				if (propParent != null)
				{
					return propParent.Parent;
				}
				return null;
			}
		}

		[CLSCompliant(false)]
		public IConvert Convert
		{
			get
			{
				return propConvert;
			}
			set
			{
				propConvert = value;
			}
		}

		public string InitValue
		{
			get
			{
				return propInitValue;
			}
			set
			{
				propInitValue = value;
			}
		}

		public CastModes CastMode
		{
			get
			{
				return propCastMode;
			}
			set
			{
				propCastMode = value;
			}
		}

		public int BitOffset
		{
			get
			{
				return propBitOffset;
			}
			set
			{
				propBitOffset = value;
			}
		}

		public string[] ChangedStructMembers => propChangedStructMembers;

		public string StructName => propStructName;

		internal Cpu Cpu
		{
			get
			{
				if (propParent is Cpu)
				{
					return (Cpu)propParent;
				}
				if (propParent is Task)
				{
					return ((Task)propParent).Cpu;
				}
				if (propParent is Variable)
				{
					return ((Variable)propParent).Cpu;
				}
				return null;
			}
		}

		public int DataAlignment => propAlignment;

		public Access Access
		{
			get
			{
				return propVariableAccess;
			}
			set
			{
				if (value != propVariableAccess)
				{
					propVariableAccess = value;
					propReadOnly = false;
					if (Access.Read == (propVariableAccess & Access.Read) && Access.Write != (propVariableAccess & Access.Write))
					{
						propReadOnly = true;
					}
					propPolling = true;
					if (Access.EVENT == (value & Access.EVENT))
					{
						propPolling = false;
					}
					if (IsConnected)
					{
						string attributeParameters = GetAttributeParameters();
						string eventMaskParameters = GetEventMaskParameters(ConnectionType.Link, useParamMarker: false);
						Service.BuildRequestBuffer(attributeParameters);
						WriteRequest(Service.hPvi, propLinkId, AccessTypes.Type, Service.RequestBuffer, attributeParameters.Length, 551u);
						Service.BuildRequestBuffer(eventMaskParameters);
						WriteRequest(Service.hPvi, propLinkId, AccessTypes.EventMask, Service.RequestBuffer, eventMaskParameters.Length, 555u);
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
				if (propPolling != value)
				{
					propPolling = value;
					propVariableAccess |= Access.EVENT;
					if (propPolling)
					{
						propVariableAccess &= ~Access.EVENT;
					}
					if (IsConnected)
					{
						string attributeParameters = GetAttributeParameters();
						Service.BuildRequestBuffer(attributeParameters);
						WriteRequest(Service.hPvi, propLinkId, AccessTypes.Type, Service.RequestBuffer, attributeParameters.Length, 552u);
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
			}
		}

		public IODataPointCollection IODataPoints
		{
			get
			{
				if (propIODataPoints == null)
				{
					propIODataPoints = new IODataPointCollection(this, propAddress);
				}
				return propIODataPoints;
			}
		}

		public string UserTag
		{
			get
			{
				return propUserTag;
			}
			set
			{
				IntPtr zero = IntPtr.Zero;
				if (!(propUserTag == value))
				{
					propUserTag = value;
					if (IsConnected)
					{
						zero = PviMarshal.StringToHGlobal(propUserTag);
						WriteRequest(Service.hPvi, propLinkId, AccessTypes.UserTag, zero, propUserTag.Length, 554u);
						PviMarshal.FreeHGlobal(ref zero);
					}
				}
			}
		}

		public event VariableEventHandler ValueChanged;

		public event PviEventHandler ValueWritten;

		public event PviEventHandler ValueRead;

		public event PviEventHandler ExtendedTypeInfoRead;

		public event PviEventHandler DataValidated;

		public event PviEventHandler Activated;

		public event PviEventHandler Deactivated;

		public event PviEventHandler Uploaded;

		public event PviEventHandler ForcedOn;

		public event PviEventHandler ForcedOff;

		private void InitializeMembers()
		{
			isStruct = false;
			propReadingState = false;
			isMemberVar = false;
			propWaitingOnReadEvent = false;
			propWriteByteField = null;
			propReadByteField = null;
			propErrorState = 0;
			isServiceUploaded = false;
			propExpandMembers = false;
			pWriteData = IntPtr.Zero;
			pReadData = IntPtr.Zero;
			propStrDataLen = 0;
			propBitOffset = -1;
			propSendChangedEvent = false;
			propChangedStructMembers = new string[0];
			propStructName = null;
			propInitValue = null;
			propCastMode = CastModes.DEFAULT;
			mapNameToMember = null;
			propOwner = null;
			propDataValid = false;
			propRefreshTime = 100;
			propOffset = 0;
			propInternalOffset = 0;
		}

		protected void Initialize(Base pObj, Base oObj, bool expandMembers, bool automaticWrite)
		{
			propROI = ROIoptions.OFF;
			propAlignment = 1;
			propInitialValue = null;
			InitializeMembers();
			propExpandMembers = expandMembers;
			propWriteValueAutomatic = automaticWrite;
			propOwner = oObj;
		}

		private void ResetReadDataPtr(int newSize)
		{
			if (propStrDataLen < newSize)
			{
				if (pReadData == IntPtr.Zero)
				{
					PviMarshal.FreeHGlobal(ref pReadData);
				}
				propStrDataLen = newSize;
				pReadData = PviMarshal.AllocHGlobal(propStrDataLen);
				propReadByteField = null;
				propReadByteField = new byte[propStrDataLen];
				if (propPviValue.propByteField == null)
				{
					propPviValue.propByteField = new byte[propPviValue.DataSize];
				}
			}
			else
			{
				if (propReadByteField == null)
				{
					propReadByteField = new byte[propPviValue.DataSize];
				}
				for (int i = 0; i < propReadByteField.Length; i++)
				{
					propReadByteField.SetValue((byte)0, i);
				}
			}
		}

		private void ResetWriteDataPtr(Variable var, int newSize)
		{
			ResetWriteDataPtr(var, newSize, setZero: false);
		}

		private void ResetWriteDataPtr(Variable var, int newSize, bool setZero)
		{
			if (var.propStrDataLen < newSize || var.propWriteByteField == null)
			{
				if (var.pWriteData != IntPtr.Zero)
				{
					PviMarshal.FreeHGlobal(ref var.pWriteData);
				}
				var.propStrDataLen = newSize;
				var.pWriteData = PviMarshal.AllocHGlobal(var.propStrDataLen);
				var.propWriteByteField = null;
				var.propWriteByteField = new byte[var.propStrDataLen];
				if (var.propPviValue.propByteField == null)
				{
					var.propPviValue.propByteField = new byte[var.propPviValue.DataSize];
				}
			}
			if (setZero)
			{
				for (int i = 0; i < var.propWriteByteField.Length; i++)
				{
					var.propWriteByteField[i] = 0;
				}
			}
		}

		internal void ResizePviDataPtr(int newSize)
		{
			if (propPviValue.propDataSize < newSize || propPviValue.pData == IntPtr.Zero)
			{
				PviMarshal.FreeHGlobal(ref propPviValue.pData);
				propPviValue.propDataSize = newSize;
				propPviValue.pData = PviMarshal.AllocHGlobal(propPviValue.propDataSize);
				propPviValue.propHasOwnDataPtr = true;
			}
		}

		internal Variable()
		{
			Initialize(null, null, expandMembers: true, automaticWrite: true);
		}

		internal Variable(Cpu cpu)
			: base(cpu)
		{
			Initialize(null, null, expandMembers: true, automaticWrite: true);
		}

		public Variable(Service service, string name)
			: base(service, name)
		{
			if (service != null)
			{
				Variable variable = service.Variables[name];
				if (variable != null)
				{
					throw new ArgumentException("There is already an object in \"" + service.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + service.Name + ".Variables\".", name);
				}
			}
			Initialize(service, service, expandMembers: true, automaticWrite: true);
			Init(name);
			service.Variables.Add(this);
		}

		internal Variable(SimpleNetworkManagementProtocol snmp, string name)
			: base(snmp.Service, name)
		{
			InitializeMembers();
			propSNMPParent = snmp;
			Init(name);
		}

		internal Variable(NetworkAdapter nwAdapter, string name)
			: base(nwAdapter.Service, name)
		{
			InitializeMembers();
			propSNMPParent = nwAdapter;
			Init(name);
		}

		public Variable(Service service, string name, bool isUploded)
			: base(service, name)
		{
			if (service != null)
			{
				Variable variable = service.Variables[name];
				if (variable != null)
				{
					throw new ArgumentException("There is already an object in \"" + service.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + service.Name + ".Variables\".", name);
				}
			}
			Initialize(service, service, expandMembers: true, automaticWrite: true);
			Init(name, isUploded);
			service.Variables.Add(this);
		}

		public Variable(Service service, bool expandMembers, string name)
			: base(service, name)
		{
			if (service != null)
			{
				Variable variable = service.Variables[name];
				if (variable != null)
				{
					throw new ArgumentException("There is already an object in \"" + service.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + service.Name + ".Variables\".", name);
				}
			}
			Initialize(service, service, expandMembers, automaticWrite: true);
			Init(name);
			service.Variables.Add(this);
		}

		public Variable(Cpu cpu, string name)
			: base(cpu, name)
		{
			if (cpu != null)
			{
				Variable variable = cpu.Variables[name];
				if (variable != null)
				{
					throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Variables\".", name);
				}
			}
			Initialize(cpu, cpu, expandMembers: true, automaticWrite: true);
			Init(name);
			cpu.Variables.Add(this);
		}

		public Variable(Cpu cpu, bool expandMembers, string name)
			: base(cpu, name)
		{
			if (cpu != null)
			{
				Variable variable = cpu.Variables[name];
				if (variable != null)
				{
					throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Variables\".", name);
				}
			}
			Initialize(cpu, cpu, expandMembers, automaticWrite: true);
			Init(name);
			cpu.Variables.Add(this);
		}

		public Variable(Task task, string name)
			: base(task, name)
		{
			if (task != null)
			{
				Variable variable = task.Variables[name];
				if (variable != null)
				{
					throw new ArgumentException("There is already an object in \"" + task.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + task.Name + ".Variables\".", name);
				}
			}
			Initialize(task, task, expandMembers: true, automaticWrite: true);
			Init(name);
			task.Variables.Add(this);
		}

		public Variable(Task task, bool expandMembers, string name)
			: base(task, name)
		{
			if (task != null)
			{
				Variable variable = task.Variables[name];
				if (variable != null)
				{
					throw new ArgumentException("There is already an object in \"" + task.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + task.Name + ".Variables\".", name);
				}
			}
			Initialize(task, task, expandMembers, automaticWrite: true);
			Init(name);
			task.Variables.Add(this);
		}

		public Variable(Variable variable, string name)
			: base(variable, name)
		{
			if (variable != null)
			{
				Variable variable2 = variable.propMembers[name];
				if (variable2 != null)
				{
					throw new ArgumentException("There is already an object in \"" + variable.Name + ".Members\" which has the same name! Use the object from \"" + variable.Name + ".Members\".", name);
				}
			}
			Initialize(variable.Parent, variable, expandMembers: true, variable.WriteValueAutomatic);
			Init(name);
			propWriteValueAutomatic = variable.WriteValueAutomatic;
			if (variable.Members == null)
			{
				variable.propMembers = new MemberCollection(variable, variable.Address);
			}
			variable.AddMember(this);
		}

		public Variable(Variable variable, bool expandMembers, string name)
			: base(variable, name)
		{
			if (variable != null)
			{
				Variable variable2 = variable.propMembers[name];
				if (variable2 != null)
				{
					throw new ArgumentException("There is already an object in \"" + variable.Name + ".Members\" which has the same name! Use the object from \"" + variable.Name + ".Members\".", name);
				}
			}
			Initialize(variable.Parent, variable, expandMembers, variable.WriteValueAutomatic);
			Init(name);
			propWriteValueAutomatic = variable.WriteValueAutomatic;
			variable.AddMember(this);
		}

		public Variable(bool isMember, Variable variable, string name, bool addToVColls)
			: base(variable, name, addToVColls)
		{
			Initialize(variable.Parent, variable, expandMembers: true, variable.WriteValueAutomatic);
			Init(name);
			propWriteValueAutomatic = variable.WriteValueAutomatic;
			isMemberVar = isMember;
			variable.AddMember(this);
			propPviValue.InitializeExtendedAttributes();
		}

		internal Variable(string name, Variable parentVar, bool addToVCollections, int offset, int alignment, Scope vScope)
			: base(parentVar, name, addToVCollections)
		{
			Initialize(parentVar.Parent, parentVar, expandMembers: true, parentVar.WriteValueAutomatic);
			Init(name);
			propWriteValueAutomatic = parentVar.WriteValueAutomatic;
			isMemberVar = true;
			propAlignment = alignment;
			propScope = vScope;
			base.Address = parentVar.Address + name;
			propPviValue.propTypeLength = parentVar.Value.TypeLength;
			propPviValue.SetArrayLength(1);
			if (propPviValue.ArrayMinIndex == 0)
			{
				propPviValue.propArrayMaxIndex = propPviValue.ArrayLength - 1;
			}
			propOffset = offset;
			propPviValue.SetDataType(parentVar.Value.DataType);
			propStructName = parentVar.propStructName;
			propPviValue.propDerivedFrom = parentVar.Value.DerivedFrom;
			propPviValue.propEnumerations = parentVar.Value.Enumerations;
			propPviValue.SetDataType(parentVar.Value.DataType);
			parentVar.AddMember(this);
			propPviValue.InitializeExtendedAttributes();
		}

		internal void CloneVariable(Variable varClone, Variable root, Variable parentVar, bool addToVCollections, bool bAddToAll)
		{
			int offset = propOffset;
			if (bAddToAll)
			{
				AddStructMembers(parentVar, this);
			}
			else
			{
				root.AddStructMember(GetStructMemberName(root), this);
			}
			AddToParentCollection(this, propParent, addToVCollections);
			if (DataType.Structure == propPviValue.DataType)
			{
				CreateNestedStructClone(root, parentVar, this, varClone, ref offset, 0, addToVCollections, bAddToAll);
				return;
			}
			propAlignment = varClone.propAlignment;
			propScope = root.propScope;
			propPviValue.Clone(varClone.propPviValue);
			propStructName = varClone.propStructName;
			propOffset = offset;
			if (propPviValue.IsOfTypeArray)
			{
				CreateNestedStructClone(root, parentVar, this, varClone, ref offset, 0, addToVCollections, bAddToAll);
			}
		}

		public Variable(Variable variable, string name, bool addToVColls)
			: base(variable, name, addToVColls)
		{
			if (variable != null)
			{
				Variable variable2 = variable.propMembers[name];
				if (variable2 != null)
				{
					throw new ArgumentException("There is already an object in \"" + variable.Name + ".Members\" which has the same name! Use the object from \"" + variable.Name + ".Members\".", name);
				}
			}
			Initialize(variable.Parent, variable, expandMembers: true, variable.WriteValueAutomatic);
			Init(name);
			propWriteValueAutomatic = variable.WriteValueAutomatic;
			variable.AddMember(this);
		}

		public Variable(Variable variable, bool expandMembers, string name, bool memberOnly)
			: base(variable, name, memberOnly)
		{
			if (variable != null)
			{
				Variable variable2 = variable.propMembers[name];
				if (variable2 != null)
				{
					throw new ArgumentException("There is already an object in \"" + variable.Name + ".Members\" which has the same name! Use the object from \"" + variable.Name + ".Members\".", name);
				}
			}
			Initialize(variable.Parent, variable, expandMembers, variable.WriteValueAutomatic);
			Init(name);
			propWriteValueAutomatic = variable.WriteValueAutomatic;
			variable.AddMember(this);
		}

		internal Variable(Cpu cpu, bool expandMembers, string name, VariableCollection collection)
			: base(cpu, name)
		{
			Initialize(cpu, cpu, expandMembers, automaticWrite: true);
			Init(name);
			collection.Add(this);
		}

		internal Variable(Task task, bool expandMembers, string name, VariableCollection collection)
			: base(task, name)
		{
			Initialize(task, task, expandMembers, automaticWrite: true);
			Init(name);
			collection.Add(this);
		}

		internal Variable(Base parentObj, bool expandMembers, string name, string address)
			: base(parentObj, name)
		{
			Initialize(parentObj, parentObj, expandMembers, automaticWrite: true);
			string[] array = address.Split('.');
			Variable variable = null;
			Variable variable2 = null;
			int num = 0;
			int num2 = 0;
			string text = "";
			num = array[0].IndexOf("[");
			if (num != -1)
			{
				num2 = System.Convert.ToInt32(array[0].Substring(num + 1, array[0].IndexOf("]") - num - 1));
				text = ((num2 <= 0) ? array[0] : array[0].Substring(0, array[0].IndexOf("[")));
			}
			else
			{
				text = array[0];
			}
			if (parentObj is Cpu)
			{
				Cpu cpu = (Cpu)parentObj;
				if ((variable = cpu.Variables[text]) == null)
				{
					variable = ((num == -1) ? new Variable(cpu, array[0])
					{
						propAlignment = propAlignment
					} : new Variable(cpu, array[0].Substring(0, array[0].IndexOf("[")))
					{
						propAlignment = propAlignment,
						propPviValue = 
						{
							propArrayLength = System.Convert.ToInt32(array[0].Substring(num + 1, array[0].IndexOf("]") - num - 1))
						}
					});
					variable.propPviValue.SetDataType(DataType.Structure);
				}
			}
			else
			{
				Task task = (Task)parentObj;
				if ((variable = task.Variables[text]) == null)
				{
					variable = ((num == -1) ? new Variable(task, array[0])
					{
						propAlignment = propAlignment
					} : new Variable(task, array[0].Substring(0, array[0].IndexOf("[")))
					{
						propAlignment = propAlignment,
						propPviValue = 
						{
							propArrayLength = System.Convert.ToInt32(array[0].Substring(num + 1, array[0].IndexOf("]") - num - 1))
						}
					});
					variable.propPviValue.SetDataType(DataType.Structure);
				}
			}
			object obj = variable;
			variable2 = variable;
			for (int i = 1; i < array.Length; i++)
			{
				num2 = 0;
				num = array[i].IndexOf("[");
				if (num != -1)
				{
					num2 = System.Convert.ToInt32(array[i].Substring(num + 1, array[i].IndexOf("]") - num - 1));
					text = ((num2 <= 0) ? array[i] : array[i].Substring(0, array[i].IndexOf("[")));
				}
				else
				{
					text = array[i];
				}
				if (variable2.Members == null || (variable2 = variable2.Members[text]) == null)
				{
					if (i != array.Length - 1)
					{
						variable2 = new Variable((Variable)obj, text)
						{
							propAlignment = propAlignment
						};
						((Variable)obj).Members.Add(variable2);
						variable2.propPviValue.propArrayLength = num2;
						variable2.propPviValue.SetDataType(DataType.Structure);
					}
					else
					{
						propName = text;
						propParent = (Base)obj;
						Init(propName);
						propPviValue.propArrayLength = num2;
						((Variable)propParent).Members.Add(this);
						variable2 = this;
					}
				}
				obj = variable2;
			}
		}

		internal void Init(string name)
		{
			Init(name, isServiceUploadedVar: false);
		}

		internal void Init(string name, bool isServiceUploadedVar)
		{
			propScope = Scope.UNDEFINED;
			propReadingFormat = false;
			propLinkId = 0u;
			propMembers = null;
			propUserMembers = null;
			propPviValue = new Value();
			propPviValue.Parent = this;
			propPviValue.propDataType = DataType.Unknown;
			propPviValue.propArrayMinIndex = 0;
			propPviValue.propArrayMaxIndex = 0;
			propPviValue.propDataSize = 0;
			propDataValid = false;
			propRefreshTime = 100;
			propPviValue.Parent = this;
			propWriteValueAutomatic = true;
			propReadOnly = false;
			propPVState = ConnectionType.None;
			propInternalValue = null;
			propVariableAccess = Access.ReadAndWrite;
			propReadOnly = false;
			propPolling = true;
			propScalingPoints = new ScalingPointCollection();
			propScalingPoints.propParent = this;
			propIODataPoints = null;
			if (propAddToLogicalObjects)
			{
				switch (Service.LogicalObjectsUsage)
				{
				case LogicalObjectsUsage.FullName:
					Service.AddLogicalObject(FullName, this);
					break;
				case LogicalObjectsUsage.ObjectName:
					Service.AddLogicalObject(name, this);
					break;
				case LogicalObjectsUsage.ObjectNameWithType:
					Service.AddLogicalObject(PviPathName, this);
					break;
				}
			}
			isServiceUploaded = isServiceUploadedVar;
			if (isServiceUploadedVar)
			{
				propAddress = name;
			}
		}

		internal void AddMember(Variable newVar)
		{
			if (propMembers == null)
			{
				propMembers = new MemberCollection(this, base.Name + ".Variables");
			}
			propMembers.Add(newVar);
		}

		internal void AddStructMembers(Variable parentVar, Variable memberVar)
		{
			Base owner = parentVar.Owner;
			if (owner is Variable)
			{
				AddStructMembers((Variable)owner, memberVar);
			}
			parentVar.AddStructMember(memberVar.GetStructMemberName(parentVar), memberVar);
		}

		internal void AddStructMember(string name, Variable memberVar)
		{
			if (mapNameToMember == null)
			{
				mapNameToMember = new StructMemberCollection();
			}
			mapNameToMember.Add(name, memberVar);
		}

		public void SetTypeInfo(DataType type)
		{
			propPviValue.propDataType = type;
			propBitOffset = -1;
			propPviValue.propArrayLength = 0;
			propPviValue.TypePreset = true;
		}

		public void SetTypeInfo(int bitOffset, DataType type)
		{
			propPviValue.propDataType = type;
			propBitOffset = bitOffset;
			propPviValue.propArrayLength = 0;
			propPviValue.TypePreset = true;
		}

		public void SetTypeInfo(DataType type, int arraySize)
		{
			propPviValue.propDataType = type;
			propBitOffset = -1;
			propPviValue.propArrayLength = arraySize;
			propPviValue.TypePreset = true;
		}

		public void SetTypeInfo(DataType type, int arraySize, int bitOffset)
		{
			propPviValue.propDataType = type;
			propBitOffset = bitOffset;
			propPviValue.propArrayLength = arraySize;
			propPviValue.TypePreset = true;
		}

		internal override void reCreateState()
		{
			if (reCreateActive)
			{
				reCreateActive = false;
				propLinkId = 0u;
				if (propActive)
				{
					base.Requests |= (Actions.SetActive | Actions.FireActivated);
				}
				propConnectionState = ConnectionStates.Unininitialized;
				Connect(forceConnect: true);
			}
		}

		public override int ChangeConnection()
		{
			int num = 0;
			string connectionDescription = GetConnectionDescription();
			return WriteRequest(Service.hPvi, base.LinkId, AccessTypes.Connect, connectionDescription.Substring(0, connectionDescription.IndexOf(" ")), 3000u, _internId);
		}

		public override void Connect()
		{
			propReturnValue = 0;
			Connect(base.ConnectionType);
		}

		public override void Connect(ConnectionType conType)
		{
			propReturnValue = 0;
			Connect(conType, 0);
		}

		internal override void Connect(bool forceConnect)
		{
			propReturnValue = 0;
			Connect(forceConnection: true, base.ConnectionType, 0u);
		}

		protected virtual string GetLinkParameters(ConnectionType conType, string dt, string fs, string lp, string va, string cm, string vL, string vN)
		{
			string text = "";
			if (propSNMPParent != null)
			{
				if (base.Address.CompareTo("MacAddresses") == 0)
				{
					if (propActive)
					{
						if (Service.UserTagEvents)
						{
							return "VT=string" + vL + " VN=1 EV=eufd";
						}
						return "VT=string" + vL + " VN=1 EV=efd";
					}
					if (Service.UserTagEvents)
					{
						return "VT=string" + vL + " VN=1 EV=euf";
					}
					return "VT=string" + vL + " VN=1 EV=ef";
				}
				return "EV=";
			}
			string eventMaskParameters = GetEventMaskParameters(conType);
			text = ((0 >= dt.Length) ? (eventMaskParameters + lp + cm) : (dt + vL + vN + " " + eventMaskParameters + lp + cm));
			if (va != null && 0 < va.Length)
			{
				text = "VT=boolean " + eventMaskParameters + lp + va;
			}
			if (base.ConnectionType == ConnectionType.Link)
			{
				text += fs;
			}
			string str = "";
			if (propHysteresis > 0.0)
			{
				str = string.Format(" {0}{1}", "HY=", propHysteresis.ToString(CultureInfo.InvariantCulture));
			}
			if (!Service.IsStatic || base.ConnectionType == ConnectionType.Link)
			{
				if ((base.Requests & Actions.SetRefresh) != 0)
				{
					base.Requests &= ~Actions.SetRefresh;
				}
				if (propActive)
				{
					text = ((0 >= dt.Length) ? (eventMaskParameters + lp + cm) : (dt + vL + vN + " " + eventMaskParameters + lp + cm));
					if (va != null && 0 < va.Length)
					{
						text = "VT=boolean " + eventMaskParameters + lp + va;
					}
					if (base.ConnectionType == ConnectionType.Link)
					{
						text = text + fs + str;
					}
				}
				base.Requests &= ~Actions.SetActive;
			}
			if (isStruct)
			{
				text += " VT=struct VL=0 AL=1";
			}
			return text;
		}

		protected virtual string GetEventMaskParameters(ConnectionType conType, bool useParamMarker)
		{
			string str = "";
			if (useParamMarker)
			{
				str = "EV=";
				if (ConnectionType.Create != conType)
				{
					str = ((!Service.UserTagEvents) ? (str + "ef") : (str + "euf"));
				}
			}
			else if (propErrorState == 0)
			{
				str = ((!Service.UserTagEvents) ? (str + "e") : (str + "eu"));
			}
			else
			{
				str = ((!Service.UserTagEvents) ? (str + "ef") : (str + "euf"));
				propErrorState = 0;
			}
			if ((!Service.IsStatic || ConnectionType.Link == conType) && propActive && Access != 0 && Access.Write != Access)
			{
				str += "d";
			}
			return str;
		}

		private string GetEventMaskParameters(ConnectionType conType)
		{
			return GetEventMaskParameters(conType, useParamMarker: true);
		}

		protected virtual string GetObjectParameters(string rf, string hy, string at, string fs, string ut, string dt, string vL, string vN)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			string text8 = dt;
			if (0 < dt.Length)
			{
				text8.Trim();
				text8 = " " + text8;
				text5 = vL;
				text6 = vN;
			}
			if (propInitValue != null && propInitValue.Length != 0)
			{
				text3 = " DV=" + propInitValue.ToString();
			}
			if (propSNMPParent != null)
			{
				text2 = propSNMPParent.Name;
				if (base.Address.CompareTo("MacAddresses") == 0 && Value.DataType == DataType.Unknown)
				{
					if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
					{
						return string.Format("\"{0}\" VT=string VL=1024 VN=1 {1}{2}", propAddress, "RF=", rf);
					}
					return string.Format("\"{0}\"/\"{1}\" VT=string VL=1024 VN=1 {2}{3}", text2, propAddress, "RF=", rf);
				}
				text = ((ConnectionType.Link == base.ConnectionType) ? ((LogicalObjectsUsage.ObjectNameWithType != Service.LogicalObjectsUsage) ? string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}", text2, propAddress, text5, text6, "RF=", rf, hy, at, ut, text3) : string.Format("\"{0}\"{1}{2} {3}{4}{5}{6}{7}{8}", propAddress, text5, text6, "RF=", rf, hy, at, ut, text3)) : ((0 < text8.Length) ? ((LogicalObjectsUsage.ObjectNameWithType != Service.LogicalObjectsUsage) ? string.Format("\"{0}\"/\"{1}\"{2}{3}{4}{5} {6}{7}{8}{9}{10}{11}{12}", text2, propAddress, text8, text5, text6, text4, "RF=", rf, hy, at, fs, ut, text3) : string.Format("\"{0}\"{1}{2}{3}{4} {5}{6}{7}{8}{9}{10}{11}", propAddress, text8, text5, text6, text4, "RF=", rf, hy, at, fs, ut, text3)) : ((LogicalObjectsUsage.ObjectNameWithType != Service.LogicalObjectsUsage) ? string.Format("\"{0}\"/\"{1}\"{2}{3}{4} {5}{6}{7}{8}{9}{10}{11}", text2, propAddress, text5, text6, text4, "RF=", rf, hy, at, fs, ut, text3) : string.Format("\"{0}\"{1}{2}{3} {4}{5}{6}{7}{8}{9}{10}", propAddress, text5, text6, text4, "RF=", rf, hy, at, fs, ut, text3))));
			}
			else if (propParent is Service)
			{
				text = string.Format("\"{0}\" {1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", propAddress, "RF=", rf, hy, at, fs, ut, text8, text6, text5, text3);
			}
			else if (propParent is Variable)
			{
				text2 = propParent.Parent.LinkName;
				text = ((ConnectionType.Link != base.ConnectionType) ? string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}{10}", text2, propAddress, text5, text6, "RF=", rf, hy, at, fs, ut, text3) : string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}", text2, propAddress, text5, text6, "RF=", rf, hy, at, ut, text3));
			}
			else if (propParent is Cpu)
			{
				text2 = propParent.LinkName;
				if (((Cpu)propParent).Connection.DeviceType >= DeviceType.TcpIpMODBUS)
				{
					text2 = ((Cpu)propParent).Connection.pviStationObj.Name;
					if (0 < propAddress.Length)
					{
						if (0 < vN.Length)
						{
							if (0 < vL.Length)
							{
								text5.Insert(1, "/");
							}
							text6.Insert(1, "/");
						}
						string arg = dt;
						if (0 < dt.Length)
						{
							arg = "/" + dt;
						}
						text4 = string.Format(" /{0}{1} {2}", "VA=", propAddress, arg);
					}
				}
				text = ((ConnectionType.Link == base.ConnectionType) ? ((LogicalObjectsUsage.ObjectNameWithType != Service.LogicalObjectsUsage) ? string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}", text2, propAddress, text6, text5, "RF=", rf, hy, at, ut, text3) : string.Format("\"{0}\"{1}{2} {3}{4}{5}{6}{7}{8}", propAddress, text6, text5, "RF=", rf, hy, at, ut, text3)) : ((LogicalObjectsUsage.ObjectNameWithType != Service.LogicalObjectsUsage) ? string.Format("\"{0}\"/\"{1}{2}\"{3}{4} {5}{6}{7}{8}{9}{10}{11}", text2, propAddress, text4, text6, text5, "RF=", rf, hy, at, fs, ut, text3) : string.Format("\"{0}\"{1}{2}{3} {4}{5}{6}{7}{8}{9}{10}", propAddress, text4, text6, text5, "RF=", rf, hy, at, fs, ut, text3)));
			}
			else if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
			{
				text = string.Format("\"{0}\"{1}{2} {3}{4}{5}{6}{7}{8}{9}", propAddress, text6, text5, "RF=", rf, hy, at, fs, ut, text3);
			}
			else
			{
				text2 = propParent.LinkName;
				text = string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}{10}", text2, propAddress, text6, text5, "RF=", rf, hy, at, fs, ut, text3);
			}
			if (propROI != 0)
			{
				string[] array = new string[5]
				{
					"\"/RO=",
					propAddress,
					" /ROI=",
					null,
					null
				};
				string[] array2 = array;
				int num = (int)propROI;
				array2[3] = num.ToString();
				array[4] = "\"";
				text7 = string.Concat(array);
				string oldValue = "\"" + propAddress + "\"";
				text = text.Replace(oldValue, text7);
			}
			return text;
		}

		private string GetDataTypParameters()
		{
			string result = "";
			if (propParent is Service && propPviValue.DataType != 0 && propPviValue.DataType != DataType.Structure && !propPviValue.IsOfTypeArray)
			{
				result = ((propScaling == null || propScaling.ScalingPoints.Count <= 0) ? string.Format("{0}{1}", "VT=", GetPviDataTypeText(propPviValue.DataType)) : ((propScaling.ScalingPoints.propUserDataType == DataType.Unknown || propPviValue.DataType != 0) ? string.Format("{0}{1}", "VT=", GetPviDataTypeText(propPviValue.DataType)) : string.Format("{0}{1}", "VT=", GetPviDataTypeText(propScaling.ScalingPoints.propUserDataType))));
			}
			return result;
		}

		private string GetVAParameters()
		{
			string result = "";
			if (-1 < propBitOffset)
			{
				result = string.Format(" {0}{1}", "VA=", propBitOffset.ToString());
			}
			return result;
		}

		private string GetVNParameter()
		{
			string result = "";
			if (1 < propPviValue.ArrayLength)
			{
				result = string.Format(" {0}{1}", "VN=", propPviValue.ArrayLength.ToString());
			}
			return result;
		}

		private string GetVLParameter()
		{
			string result = "";
			if (DataType.String == propPviValue.DataType)
			{
				result = ((propPviValue.propTypeLength != 0 || !(propParent is Service)) ? string.Format(" {0}{1}", "VL=", propPviValue.propTypeLength.ToString()) : string.Format(" {0}32", "VL="));
			}
			else if (1 < propPviValue.DataSize)
			{
				result = string.Format(" {0}{1}", "VL=", propPviValue.DataSize.ToString());
			}
			return result;
		}

		private string GetHysteresisParameters()
		{
			string result = "";
			if (propHysteresis > 0.0)
			{
				result = string.Format(" {0}{1}", "HY=", propHysteresis.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		private string GetCastModeParameters()
		{
			string result = "";
			if (propCastMode != 0)
			{
				int num = (int)propCastMode;
				result = " CM=" + num.ToString();
			}
			return result;
		}

		private string GetAttributeParameters()
		{
			string text = string.Format(" {0}", "AT=");
			if (Access.ReadAndWrite == propVariableAccess)
			{
				text += "rw";
			}
			else if (propVariableAccess == Access.No)
			{
				text = string.Format(" {0}", "AT=");
			}
			else
			{
				if (Access.Read == (propVariableAccess & Access.Read))
				{
					text += "r";
				}
				if (Access.Write == (propVariableAccess & Access.Write))
				{
					text += "w";
				}
				if (Access.DIRECT == (propVariableAccess & Access.DIRECT))
				{
					text += "d";
				}
				if (Access.FASTECHO == (propVariableAccess & Access.FASTECHO))
				{
					text += "h";
				}
				if (Access.EVENT == (propVariableAccess & Access.EVENT))
				{
					text += "e";
				}
			}
			return text;
		}

		private string GetUserTagParameters()
		{
			string result = "";
			if (propUserTag != null)
			{
				result = string.Format(" {0}{1}", "UT=", propUserTag);
			}
			return result;
		}

		private string GetScalingFunctionParameters()
		{
			string result = "";
			if (propPviValue.DataType != DataType.Structure && !propPviValue.IsOfTypeArray)
			{
				if (propScaling != null && propScaling.ScalingType == ScalingType.Factor)
				{
					propScaling.Factor = propScaling.propFactor;
				}
				if (propScaling != null && propScaling.ScalingPoints.Count > 0)
				{
					result = string.Format(" {0}", "FS=");
					{
						foreach (ScalingPoint scalingPoint in propScaling.ScalingPoints)
						{
							string text = scalingPoint.XValue.ToString(CultureInfo.InvariantCulture);
							string text2 = scalingPoint.YValue.ToString(CultureInfo.InvariantCulture);
							if (DataType.Single == propPviValue.propDataType || DataType.Double == propPviValue.propDataType)
							{
								if (-1 == text.IndexOf('.'))
								{
									text += ".0";
								}
								if (-1 == text2.IndexOf('.'))
								{
									text2 += ".0";
								}
							}
							result += $"{text},{text2};";
						}
						return result;
					}
				}
			}
			return result;
		}

		private string UpdateAddress(ConnectionType conType)
		{
			string text = propAddress;
			if (propAddress == null || (propAddress.Length == 0 && ConnectionType.Link != conType))
			{
				text = propName;
				Base @base = (!(propOwner is Variable) || !((Variable)propOwner).propPviValue.IsOfTypeArray) ? propOwner : ((Variable)propOwner).propOwner;
				while (@base is Variable)
				{
					text = ((@base.propAddress.Length != 0) ? (@base.propAddress + "." + text) : (@base.propName + "." + text));
					@base = ((Variable)@base).propOwner;
				}
			}
			return text;
		}

		private string GetLTParameters()
		{
			string result = "";
			if (propHysteresis > 0.0 || (propScaling != null && propScaling.ScalingPoints.Count > 0 && propPviValue.DataType != DataType.Structure && !propPviValue.IsOfTypeArray))
			{
				result = string.Format(" {0}{1}", "LT=", "prc");
			}
			return result;
		}

		private string GetDataTypParameter()
		{
			string result = "";
			if (propPviValue.TypePreset)
			{
				switch (propPviValue.propDataType)
				{
				case DataType.Boolean:
					result = "VT=boolean";
					break;
				case DataType.Byte:
				case DataType.UInt8:
					result = "VT=u8";
					break;
				case DataType.SByte:
					result = "VT=i8";
					break;
				case DataType.Int16:
					result = "VT=i16";
					break;
				case DataType.UInt16:
					result = "VT=u16";
					break;
				case DataType.WORD:
					result = "VT=WORD";
					break;
				case DataType.Int32:
					result = "VT=i32";
					break;
				case DataType.UInt32:
					result = "VT=u32";
					break;
				case DataType.DWORD:
					result = "VT=DWORD";
					break;
				case DataType.Int64:
					result = "VT=i64";
					break;
				case DataType.UInt64:
					result = "VT=u64";
					break;
				case DataType.Single:
					result = "VT=f32";
					break;
				case DataType.Double:
					result = "VT=f64";
					break;
				case DataType.DateTime:
				case DataType.DT:
					result = "VT=dt";
					break;
				case DataType.Date:
					result = "VT=date";
					break;
				case DataType.TimeSpan:
					result = "VT=time";
					break;
				case DataType.TimeOfDay:
				case DataType.TOD:
					result = "VT=tod";
					break;
				case DataType.String:
					result = "VT=string";
					break;
				case DataType.WString:
					result = "VT=wstring";
					break;
				}
			}
			return result;
		}

		protected override string GetConnectionDescription()
		{
			string text = "";
			string attributeParameters = GetAttributeParameters();
			string userTagParameters = GetUserTagParameters();
			string hysteresisParameters = GetHysteresisParameters();
			string vNParameter = GetVNParameter();
			string vLParameter = GetVLParameter();
			string dt = GetDataTypParameter();
			string scalingFunctionParameters = GetScalingFunctionParameters();
			propAddress = UpdateAddress(ConnectionType.Create);
			if (propParent is Service && propSNMPParent == null)
			{
				dt = GetDataTypParameters();
			}
			return GetObjectParameters(propRefreshTime.ToString(), hysteresisParameters, attributeParameters, scalingFunctionParameters, userTagParameters, dt, vLParameter, vNParameter);
		}

		public virtual void Connect(ConnectionType conType, int internalAction)
		{
			Connect(forceConnection: false, conType, (uint)internalAction);
		}

		private void Connect(bool forceConnection, ConnectionType conType, uint internalAction)
		{
			propReturnValue = 0;
			if (reCreateActive || base.LinkId != 0)
			{
				return;
			}
			if (base.LinkId != 0 && (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState))
			{
				Fire_ConnectedEvent(this, new PviEventArgs(base.Name, base.Address, propErrorCode, Service.Language, Action.VariableConnect, Service));
			}
			else
			{
				if ((!forceConnection && ConnectionStates.Unininitialized < propConnectionState && ConnectionStates.Disconnecting > propConnectionState) || ConnectionStates.Connecting == propConnectionState)
				{
					return;
				}
				if (null != propPviValue && !propPviValue.TypePreset)
				{
					propPviValue.propDataType = DataType.Unknown;
					propDataValid = false;
					propWaitForUserTag = false;
					isObjectConnected = false;
				}
				base.ConnectionType = conType;
				if (propParent is Variable)
				{
					if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
					{
						Call_Connected(new PviEventArgs(base.Name, base.Address, 12002, Service.Language, Action.VariableConnect, Service));
						propReturnValue = 12002;
					}
				}
				else if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
				{
					Call_Connected(new PviEventArgs(base.Name, base.Address, 12002, Service.Language, Action.VariableConnect, Service));
					propReturnValue = 12002;
				}
				if (string.IsNullOrEmpty(propUserTag))
				{
					propWaitForUserTag = false;
				}
				else
				{
					propWaitForUserTag = true;
				}
				propAddress = UpdateAddress(conType);
				if (propParent is Cpu && !((Cpu)propParent).IsConnected && ((Cpu)propParent).ErrorCode == 0)
				{
					if (!forceConnection)
					{
						if (Service.WaitForParentConnection)
						{
							propReturnValue = 0;
							base.Requests |= Actions.Connect;
							return;
						}
					}
					else if (Actions.Connect == (base.Requests & Actions.Connect))
					{
						base.Requests &= ~Actions.Connect;
					}
				}
				if (propParent is Task && !((Task)propParent).IsConnected && ((Task)propParent).ErrorCode == 0)
				{
					if (!forceConnection)
					{
						if (Service.WaitForParentConnection)
						{
							propReturnValue = 0;
							base.Requests |= Actions.Connect;
							return;
						}
					}
					else if (Actions.Connect == (base.Requests & Actions.Connect))
					{
						base.Requests &= ~Actions.Connect;
					}
				}
				propConnectionState = ConnectionStates.Connecting;
				string dt = GetDataTypParameter();
				if (propPviValue.DataType == DataType.Unknown)
				{
					if (base.ConnectionType == ConnectionType.Link && (propHysteresis > 0.0 || (propScaling != null && propScaling.ScalingPoints.Count > 0)))
					{
						propReturnValue = XLinkRequest(Service.hPvi, base.LinkName, 711u, "EV=f", 709u);
						return;
					}
				}
				else if (propParent is Service && propSNMPParent == null)
				{
					dt = GetDataTypParameters();
				}
				if (Service.IsStatic)
				{
					base.Requests |= Actions.Link;
				}
				string scalingFunctionParameters = GetScalingFunctionParameters();
				string lTParameters = GetLTParameters();
				string vAParameters = GetVAParameters();
				string castModeParameters = GetCastModeParameters();
				string vNParameter = GetVNParameter();
				string vLParameter = GetVLParameter();
				propLinkParam = GetLinkParameters(conType, dt, scalingFunctionParameters, lTParameters, vAParameters, castModeParameters, vLParameter, vNParameter);
				propObjectParam = "CD=" + GetConnectionDescription();
				string objectName = GetObjectName();
				if (!Service.IsStatic && base.ConnectionType == ConnectionType.CreateAndLink)
				{
					propReturnValue = XCreateRequest(Service.hPvi, objectName, ObjectType.POBJ_PVAR, propObjectParam, 550u, propLinkParam, 501u);
				}
				else if (ConnectionType.Link != base.ConnectionType && ConnectionType.Create != propPVState)
				{
					propReturnValue = XCreateRequest(Service.hPvi, objectName, ObjectType.POBJ_PVAR, propObjectParam, 0u, "", 501u);
				}
				else
				{
					uint action = 701u;
					if (internalAction != 0)
					{
						action = internalAction;
					}
					propReturnValue = PviLinkObject(action);
				}
				if (propReturnValue != 0)
				{
					OnError(new PviEventArgs(propName, propAddress, propReturnValue, Service.Language, Action.VariableConnect, Service));
				}
			}
		}

		protected override string GetObjectName()
		{
			if (propSNMPParent != null)
			{
				return propSNMPParent.FullName + "." + base.Name;
			}
			if (isServiceUploaded)
			{
				return "@Pvi/" + base.Name;
			}
			return base.LinkName;
		}

		public virtual void Upload()
		{
			Upload(bSendEvent: true);
		}

		internal virtual void Upload(bool bSendEvent)
		{
			int num = 0;
			if (bSendEvent)
			{
				propSendUploadEvent = true;
			}
			else
			{
				propSendUploadEvent = false;
			}
			if (ConnectionStates.Connected != propConnectionState && ConnectionStates.ConnectedError != propConnectionState)
			{
				base.Requests |= Actions.Upload;
				Connect();
				return;
			}
			if (Members != null && Members.Count > 0)
			{
				propUserMembers = new MemberCollection();
				Members.CopyTo(propUserMembers);
				Members.CleanUp(disposing: false);
			}
			num = ((!(Parent is Service)) ? ReadRequest(Service.hPvi, propLinkId, AccessTypes.TypeExtern, 700u) : ReadRequest(Service.hPvi, propLinkId, AccessTypes.TypeIntern, 700u));
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.NONE, Service));
			}
		}

		protected override void OnConnected(PviEventArgs e)
		{
			if (ConnectionStates.Connecting < propConnectionState && ConnectionStates.ConnectedError != propConnectionState)
			{
				if (ConnectionStates.Disconnected == propConnectionState)
				{
					FireConnectedEvents(e);
				}
				return;
			}
			if (propErrorState != 0)
			{
				if (!propActive)
				{
					if (Cpu != null)
					{
						if (!Cpu.IsSG4Target)
						{
							base.Requests |= Actions.ReadPVFormat;
							Read_State(propLinkId, 2812u);
						}
						else
						{
							Read_FormatEX(propLinkId);
						}
					}
					else
					{
						Read_FormatEX(propLinkId);
					}
					return;
				}
				Activate();
				propErrorState = 0;
			}
			if ((e.ErrorCode == 0 || 12002 == e.ErrorCode) && propInitValue != null && propInitValue.Length != 0)
			{
				propPviValue.Assign(propInitValue);
				base.Requests |= Actions.SetInitValue;
			}
			FireConnectedEvents(e);
		}

		private void FireConnectedEvents(PviEventArgs e)
		{
			if (0.0 != propHysteresis && (propPviValue.DataType == DataType.Single || propPviValue.DataType == DataType.Double) && ConnectionStates.Connecting == propConnectionState && -1 == propHysteresis.ToString().IndexOf('.') && -1 == propHysteresis.ToString().IndexOf(','))
			{
				propConnectionState = ConnectionStates.Connected;
				Hysteresis = propHysteresis;
				propConnectionState = ConnectionStates.Connecting;
			}
			base.OnConnected(e);
			if (Parent is Cpu)
			{
				((Cpu)Parent).Variables.OnConnected(this, e);
			}
			else if (Parent is Task)
			{
				((Task)Parent).Variables.OnConnected(this, e);
			}
			else if (Parent is Service && propSNMPParent == null)
			{
				((Service)Parent).Variables.OnConnected(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnConnected(this, e);
				}
			}
			if (e.ErrorCode == 0)
			{
				CheckActiveRequests(e);
			}
			if (null != propPviValue && !Active && propPviValue.DataType != 0 && propSNMPParent == null)
			{
				DeactivateInternal();
			}
		}

		protected void CheckActiveRequests(PviEventArgs e)
		{
			if (propLinkId == 0)
			{
				return;
			}
			if ((base.Requests & Actions.SetActive) != 0)
			{
				Active = propActive;
				base.Requests &= ~Actions.SetActive;
			}
			if ((base.Requests & Actions.SetRefresh) != 0)
			{
				RefreshTime = RefreshTime;
				base.Requests &= ~Actions.SetRefresh;
			}
			Actions action = base.Requests & Actions.SetHysteresis;
			if ((base.Requests & Actions.SetInitValue) != 0)
			{
				base.Requests &= ~Actions.SetInitValue;
				WriteInitialValue();
			}
			if ((base.Requests & Actions.SetValue) != 0)
			{
				propPviValue.Assign(propInternalValue.ToString());
				if (WriteValueAutomatic)
				{
					WriteValue();
				}
				propInternalValue.Dispose();
				propInternalValue = null;
				base.Requests &= ~Actions.SetValue;
			}
			if ((base.Requests & Actions.GetValue) != 0)
			{
				base.Requests &= ~Actions.GetValue;
				ReadInternalValue();
			}
			if ((base.Requests & Actions.Upload) != 0)
			{
				Upload();
				base.Requests &= ~Actions.Upload;
			}
			if (Actions.FireActivated == (base.Requests & Actions.FireActivated))
			{
				OnActivated(e);
			}
			if (Actions.FireDataValidated == (base.Requests & Actions.FireDataValidated))
			{
				OnDataValidated(e);
			}
			if (Actions.FireValueChanged == (base.Requests & Actions.FireValueChanged))
			{
				base.Requests &= ~Actions.FireValueChanged;
				OnValueChanged(new VariableEventArgs(base.Name, base.Address, e.ErrorCode, Service.Language, e.Action, new string[0]));
			}
		}

		public override void Disconnect()
		{
			propReturnValue = 0;
			propReadingFormat = false;
			propReadingState = false;
			Disconnect(502u);
		}

		public override void Disconnect(bool noResponse)
		{
			propReturnValue = 0;
			propReadingFormat = false;
			propReadingState = false;
			Disconnect(502u, noResponse);
		}

		internal int Disconnect(uint internalAction)
		{
			return Disconnect(internalAction, noResponse: false);
		}

		internal virtual int Disconnect(uint internalAction, bool noResponse)
		{
			propNoDisconnectedEvent = noResponse;
			propWaitingOnReadEvent = false;
			int result = 12004;
			propConnectionState = ConnectionStates.Disconnecting;
			if (propLinkId != 0)
			{
				if (Service != null)
				{
					if (propNoDisconnectedEvent)
					{
						result = Unlink();
						propConnectionState = ConnectionStates.Unininitialized;
					}
					else
					{
						result = UnlinkRequest(internalAction);
					}
				}
				else
				{
					propLinkId = 0u;
					result = 0;
				}
			}
			if (propNoDisconnectedEvent)
			{
				propConnectionState = ConnectionStates.Unininitialized;
				propReadingFormat = false;
				propReadingState = false;
			}
			return result;
		}

		protected override void OnDisconnected(PviEventArgs e)
		{
			propReadingState = false;
			propReadingFormat = false;
			if (ConnectionStates.Connected != propConnectionState && ConnectionStates.ConnectedError != propConnectionState)
			{
				OnConnected(e);
				isObjectConnected = false;
				if (propConnectionState != ConnectionStates.Disconnecting)
				{
					propConnectionState = ConnectionStates.Connected;
				}
			}
			base.OnDisconnected(e);
			if (Parent is Cpu && ((Cpu)Parent).Variables != null)
			{
				((Cpu)Parent).Variables.OnDisconnected(this, e);
			}
			else if (Parent is Task && ((Task)Parent).Variables != null)
			{
				((Task)Parent).Variables.OnDisconnected(this, e);
			}
			if (propParent is Service && ((Service)Parent).Variables != null && propSNMPParent == null)
			{
				((Service)propParent).Variables.OnDisconnected(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnDisconnected(this, e);
					if (propUserCollections == null)
					{
						break;
					}
				}
			}
			propPVState = ConnectionType.None;
			propDataValid = false;
			if (!propDisposed && mapNameToMember != null)
			{
				for (int i = 0; i < mapNameToMember.Count; i++)
				{
					object obj = mapNameToMember[i];
					if (obj is Variable)
					{
						((Variable)obj).propDataValid = false;
					}
				}
			}
			propStatusRead = false;
			if (Actions.Connect == (base.Requests & Actions.Connect))
			{
				base.Requests = Actions.NONE;
				Connect(ConnectionType.CreateAndLink);
			}
			else
			{
				propConnectionState = ConnectionStates.Unininitialized;
			}
		}

		protected internal override void OnError(PviEventArgs e)
		{
			base.OnError(e);
			if (Parent is Cpu)
			{
				if (Parent != null && ((Cpu)Parent).Variables != null)
				{
					((Cpu)Parent).Variables.OnError(this, e);
				}
			}
			else if (Parent is Task && Parent != null && ((Task)Parent).Variables != null)
			{
				((Task)Parent).Variables.OnError(this, e);
			}
			if (propParent is Service && propSNMPParent == null)
			{
				((Service)propParent).Variables.OnError(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnError(this, e);
				}
			}
			propStatusRead = false;
		}

		protected virtual void OnDataValidated(PviEventArgs e)
		{
			if (Actions.FireDataValidated == (base.Requests & Actions.FireDataValidated))
			{
				base.Requests &= ~Actions.FireDataValidated;
			}
			if (propDataValid)
			{
				return;
			}
			propDataValid = true;
			if (mapNameToMember != null)
			{
				for (int i = 0; i < mapNameToMember.Count; i++)
				{
					object obj = mapNameToMember[i];
					if (obj is Variable)
					{
						((Variable)obj).propDataValid = true;
					}
				}
			}
			if (this.DataValidated != null)
			{
				this.DataValidated(this, e);
			}
			if (Parent is Cpu)
			{
				((Cpu)Parent).Variables.OnDataValidated(this, e);
			}
			else if (Parent is Task)
			{
				((Task)Parent).Variables.OnDataValidated(this, e);
			}
			else if (Parent is Service && propSNMPParent == null)
			{
				((Service)Parent).Variables.OnDataValidated(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnDataValidated(this, e);
				}
			}
		}

		protected virtual void OnValueChanged(VariableEventArgs e)
		{
			if (ConnectionStates.Connected != propConnectionState)
			{
				base.Requests |= (Actions.FireDataValidated | Actions.FireValueChanged);
				return;
			}
			Fire_ValueChanged(this, e);
			if (Parent is Task && ((Task)Parent).Variables != null)
			{
				((Task)Parent).Variables.OnValueChanged(this, e);
			}
			else if (Parent is Cpu && ((Cpu)Parent).Variables != null)
			{
				((Cpu)Parent).Variables.OnValueChanged(this, e);
			}
			if (Parent is Service && ((Service)Parent).Variables != null)
			{
				((Service)Parent).Variables.OnValueChanged(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnValueChanged(this, e);
				}
			}
		}

		protected virtual void OnValueWritten(PviEventArgs e)
		{
			Fire_ValueWritten(this, e);
			if (Parent is Cpu)
			{
				((Cpu)Parent).Variables.OnValueWritten(this, e);
			}
			else if (Parent is Task)
			{
				((Task)Parent).Variables.OnValueWritten(this, e);
			}
			if (propParent is Service)
			{
				((Service)propParent).Variables.OnValueWritten(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnValueWritten(this, e);
				}
			}
		}

		protected virtual void OnValueRead(PviEventArgs e)
		{
			propErrorCode = e.ErrorCode;
			Fire_ValueRead(this, e);
			if (Parent is Cpu)
			{
				((Cpu)Parent).Variables.OnValueRead(this, e);
			}
			else if (Parent is Task)
			{
				((Task)Parent).Variables.OnValueRead(this, e);
			}
			if (propParent is Service && propSNMPParent == null)
			{
				((Service)propParent).Variables.OnValueRead(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnValueRead(this, e);
				}
			}
		}

		internal void Fire_ValueChanged(object sender, VariableEventArgs e)
		{
			OnDataValidated(e);
			if (this.ValueChanged != null)
			{
				this.ValueChanged(sender, e);
			}
		}

		internal void Fire_ValueWritten(object sender, PviEventArgs e)
		{
			if (this.ValueWritten != null)
			{
				this.ValueWritten(sender, e);
			}
		}

		internal void Fire_ValueRead(object sender, PviEventArgs e)
		{
			propWaitingOnReadEvent = false;
			if (e.ErrorCode == 0)
			{
				OnDataValidated(e);
			}
			if (this.ValueRead != null)
			{
				this.ValueRead(sender, e);
			}
		}

		internal void Fire_Activated(object sender, PviEventArgs e)
		{
			propActive = true;
			if (e.ErrorCode != 0)
			{
				base.Requests |= (Actions.SetActive | Actions.FireActivated);
			}
			else if (this.Activated != null)
			{
				this.Activated(sender, e);
			}
		}

		internal void Fire_Deactivated(object sender, PviEventArgs e)
		{
			propReadingFormat = false;
			propReadingState = false;
			propActive = false;
			base.Requests = Actions.NONE;
			if (this.Deactivated != null)
			{
				this.Deactivated(sender, e);
			}
		}

		protected virtual void OnExtendedTypeInfoRead(PviEventArgs e)
		{
			if (this.ExtendedTypeInfoRead != null)
			{
				this.ExtendedTypeInfoRead(this, e);
			}
		}

		protected virtual void OnActivated(PviEventArgs e)
		{
			if (Actions.FireActivated == (base.Requests & Actions.FireActivated))
			{
				base.Requests &= ~Actions.FireActivated;
				Fire_Activated(this, e);
				if (propParent is Cpu)
				{
					((Cpu)propParent).Variables.OnActivated(this, e);
				}
				else if (propParent is Task)
				{
					((Task)propParent).Variables.OnActivated(this, e);
				}
				else if (propParent is Service)
				{
					((Service)propParent).Variables.OnActivated(this, e);
				}
				if (propUserCollections != null && propUserCollections.Count > 0)
				{
					foreach (VariableCollection value in propUserCollections.Values)
					{
						value.OnActivated(this, e);
					}
				}
				if (propPviValue.DataType == DataType.Unknown || ((DataType.Structure == propPviValue.DataType || 1 < propPviValue.ArrayLength) && propExpandMembers && StructureMembers != null && StructureMembers.Count == 0))
				{
					Read_FormatEX(propLinkId);
				}
			}
		}

		protected virtual void OnDeactivated(PviEventArgs e)
		{
			Fire_Deactivated(this, e);
			if (propParent is Cpu)
			{
				((Cpu)propParent).Variables.OnDeactivated(this, e);
			}
			else if (propParent is Task)
			{
				((Task)propParent).Variables.OnDeactivated(this, e);
			}
			else if (propParent is Service)
			{
				((Service)propParent).Variables.OnDeactivated(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnDeactivated(this, e);
				}
			}
		}

		protected override void OnPropertyChanged(PviEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (propParent is Cpu)
			{
				((Cpu)propParent).Variables.OnPropertyChanged(this, e);
			}
			else if (propParent is Task)
			{
				((Task)propParent).Variables.OnPropertyChanged(this, e);
			}
			else if (propParent is Service)
			{
				((Service)propParent).Variables.OnPropertyChanged(this, e);
			}
			if (propUserCollections != null && propUserCollections.Count > 0)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.OnPropertyChanged(this, e);
				}
			}
		}

		protected virtual void OnUploaded(PviEventArgs e)
		{
			if (e.ErrorCode != 0)
			{
				if (propActive)
				{
					base.Requests |= Actions.Upload;
				}
				return;
			}
			if (this.Uploaded != null)
			{
				this.Uploaded(this, e);
			}
			if (propMembers != null && 0 < propMembers.Count)
			{
				foreach (Variable value in propMembers.Values)
				{
					if ((value.Requests & Actions.Connect) != 0)
					{
						value.Connect();
					}
				}
			}
		}

		protected virtual void OnForcedOn(PviEventArgs e)
		{
			if (this.ForcedOn != null)
			{
				this.ForcedOn(this, e);
			}
		}

		protected virtual void OnForcedOff(PviEventArgs e)
		{
			if (this.ForcedOff != null)
			{
				this.ForcedOff(this, e);
			}
		}

		internal int Read_State(uint linkId, uint uiAction)
		{
			if (!propReadingState)
			{
				propReadingState = true;
				return ReadRequest(Service.hPvi, linkId, AccessTypes.Status, uiAction);
			}
			return 0;
		}

		private void ExtractExtendedTypeInfo(int errorCode, IntPtr pData, uint dataLen)
		{
			string text = "";
			ArrayList changedMembers = new ArrayList();
			if (0 < dataLen)
			{
				propPviValue.InitializeExtendedAttributes();
				text = PviMarshal.PtrToStringAnsi(pData, dataLen);
				int num = text.IndexOf("\0");
				if (-1 != num)
				{
					text = text.Substring(0, num);
				}
				string[] array = text.Split(' ');
				for (int i = 0; i < array.Length; i++)
				{
					array.GetValue(i).ToString();
					GetVSParameters(text, ref propPviValue, propPviValue.DataType);
				}
			}
			else
			{
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableExtendedTypeInfoRead, Service));
			}
			if (null == propInitialValue)
			{
				propInitialValue = new Value();
			}
			ConvertSimpleValues(pData, dataLen, ref changedMembers, ref propInitialValue);
			OnExtendedTypeInfoRead(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableExtendedTypeInfoRead, Service));
		}

		private bool DataFormat_Changed(int errorCode, IntPtr pData, uint dataLen, Action cbAction)
		{
			bool result = true;
			int retVal = 0;
			if (0 < dataLen)
			{
				string text = "";
				text = PviMarshal.PtrToStringAnsi(pData, dataLen);
				GetScope(text, ref propScope);
				result = UpdateDataFormat(text, cbAction, errorCode, initOnly: false, ref retVal);
				if (retVal != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, retVal, Service.Language, cbAction, Service));
				}
			}
			else
			{
				propReadingFormat = false;
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, cbAction, Service));
				base.Requests |= Actions.ReadPVFormat;
			}
			return result;
		}

		internal bool GetExtendedAttributes(string strFormat, ref bool isMDimArray, ref Value pPviValue)
		{
			bool result = false;
			pPviValue.propIsEnum = 0;
			pPviValue.propIsBitString = 0;
			pPviValue.propIsDerived = 0;
			string[] array = strFormat.Split('{');
			isMDimArray = false;
			if (-1 != strFormat.IndexOf("VS="))
			{
				result = true;
				string text = array[0];
				if (1 < array.GetLength(0))
				{
					text.Trim();
					if (text.Length == 0)
					{
						text = array[1];
					}
				}
				if (-1 != text.IndexOf("VS=e") || -1 != text.IndexOf(";e"))
				{
					pPviValue.propIsEnum = 1;
					if (-1 != text.IndexOf("SN="))
					{
						if (pPviValue.propEnumerations != null)
						{
							pPviValue.propEnumerations.Clear();
							pPviValue.propEnumerations = null;
						}
						pPviValue.propEnumerations = new EnumArray(GetSNParameter(text));
					}
				}
				if (-1 != text.IndexOf("VS=b") || -1 != text.IndexOf(";b"))
				{
					pPviValue.propIsBitString = 1;
				}
				if (-1 != text.IndexOf("VS=a") || -1 != text.IndexOf(";a"))
				{
					isMDimArray = true;
				}
				if (-1 != text.IndexOf("VS=v") || -1 != text.IndexOf(";v"))
				{
					pPviValue.propIsDerived = 1;
				}
			}
			return result;
		}

		private bool ExtendedTypeInfoAutoUpdateEnabled()
		{
			if (Service != null && Service.ExtendedTypeInfoAutoUpdateForVariables && (1 == propPviValue.propIsEnum || 1 == propPviValue.propIsDerived || 1 == propPviValue.propIsBitString))
			{
				return true;
			}
			return false;
		}

		private int UpdateDataFormatForInit(string strFormat, bool initOnly, Action cbAction, int errorCode)
		{
			int result = 0;
			if (initOnly)
			{
				return 15;
			}
			if (propPviValue.propDataType != DataType.Structure)
			{
				if (!propWaitForUserTag && (!(this is IOVariable) || propStatusRead))
				{
					if (Cpu != null && !Cpu.IsSG4Target)
					{
						Read_State(propLinkId, 2812u);
					}
					else
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
					}
				}
			}
			else if (ConnectionStates.Connected != propConnectionState)
			{
				if (Parent is Service)
				{
					GetVSParameters(strFormat, ref propPviValue, propPviValue.DataType);
					if (propPviValue.propDataType == DataType.Structure)
					{
						propPviInternStructElement = 0u;
						CreateMembers(strFormat, ref propPviValue.propTypeLength, propExpandMembers);
					}
					else if (propPviValue.IsOfTypeArray)
					{
						CreateMembersNOExpand(strFormat, ref propPviValue.propTypeLength, Service.AddMembersToVariableCollection);
					}
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
					if (Actions.GetValue == (base.Requests & Actions.GetValue))
					{
						base.Requests &= ~Actions.GetValue;
						ReadInternalValue();
					}
				}
				else
				{
					result = InterpretTypInfo(errorCode, strFormat, (int)cbAction);
				}
			}
			else
			{
				result = InterpretTypInfo(errorCode, strFormat, (int)cbAction, skipConnected: true);
			}
			if (Actions.SetActive == (base.Requests & Actions.SetActive))
			{
				base.Requests &= ~Actions.SetActive;
				Active = propActive;
			}
			return result;
		}

		private void UpdateDataFormatForReRead(string strFormat, DataType tmpDataType, int tmpArrayLength, Action cbAction)
		{
			bool flag = false;
			if (propScaling != null && propPviValue.DataType == DataType.Unknown && tmpDataType != DataType.Structure && tmpArrayLength <= 1)
			{
				flag = propPviValue.SetDataType(GetDataType(strFormat, Value.IsBitString, ref propPviValue.propTypeLength));
				propPviValue.propTypeLength = GetDataTypeLength(strFormat, propPviValue.propTypeLength);
				propPviValue.SetArrayLength(GetArrayLength(strFormat));
				if (propPviValue.ArrayMinIndex == 0)
				{
					propPviValue.propArrayMaxIndex = propPviValue.ArrayLength - 1;
				}
				if (!flag)
				{
					WriteScaling();
				}
			}
			else if (propPviValue.DataType == DataType.Unknown || (IsConnected && cbAction != Action.VariableReadFormatInternal))
			{
				propPviValue.SetDataType(GetDataType(strFormat, Value.IsBitString, ref propPviValue.propTypeLength));
				propPviValue.propTypeLength = GetDataTypeLength(strFormat, propPviValue.propTypeLength);
				propPviValue.SetArrayLength(GetArrayLength(strFormat));
				if (propPviValue.ArrayMinIndex == 0)
				{
					propPviValue.propArrayMaxIndex = propPviValue.ArrayLength - 1;
				}
				if (propPviValue.IsOfTypeArray && DataType.Structure != propPviValue.DataType)
				{
					propExpandMembers = false;
					CreateMembers(strFormat, ref propPviValue.propTypeLength, expandMembers: false);
				}
			}
			else if (!IsConnected)
			{
				if (!propPviValue.TypePreset)
				{
					propPviValue.SetDataType(GetDataType(strFormat, Value.IsBitString, ref propPviValue.propTypeLength));
					propPviValue.propTypeLength = GetDataTypeLength(strFormat, propPviValue.propTypeLength);
					propPviValue.SetArrayLength(GetArrayLength(strFormat));
					if (propPviValue.ArrayMinIndex == 0)
					{
						propPviValue.propArrayMaxIndex = propPviValue.ArrayLength - 1;
					}
				}
				else
				{
					propPviValue.DataType = propPviValue.DataType;
				}
				if (propPviValue.IsOfTypeArray && DataType.Structure != propPviValue.DataType)
				{
					propExpandMembers = false;
					CreateMembers(strFormat, ref propPviValue.propTypeLength, expandMembers: false);
				}
			}
			propPviValue.SetArrayLength(GetArrayLength(strFormat));
			if (propPviValue.ArrayMinIndex == 0)
			{
				propPviValue.propArrayMaxIndex = propPviValue.ArrayLength - 1;
			}
		}

		private bool UpdateDataFormatForInternalRead(bool hasVsParameters, DataType tmpDataType, Action cbAction)
		{
			int num = 0;
			if (propReadingFormat)
			{
				return true;
			}
			if (Parent is Service)
			{
				if (DataType.Structure == tmpDataType || hasVsParameters || ExtendedTypeInfoAutoUpdateEnabled() || (1 == propPviValue.propIsEnum && (propPviValue.propEnumerations == null || propPviValue.propEnumerations.Count == 0)))
				{
					propPviValue.propDataType = DataType.Unknown;
					if (!isStruct && DataType.Structure == tmpDataType)
					{
						isStruct = true;
						if (UnlinkRequest(2813u) == 0)
						{
							return false;
						}
					}
					if (Read_FormatEX(propLinkId) == 0)
					{
						return false;
					}
				}
			}
			else if (DataType.Structure == tmpDataType || hasVsParameters || ExtendedTypeInfoAutoUpdateEnabled() || (1 == propPviValue.propIsEnum && (propPviValue.propEnumerations == null || propPviValue.propEnumerations.Count == 0)))
			{
				propPviValue.propDataType = DataType.Unknown;
				if (!isStruct && DataType.Structure == tmpDataType)
				{
					isStruct = true;
					if (UnlinkRequest(2813u) == 0)
					{
						return false;
					}
				}
				Read_FormatEX(propLinkId);
				return false;
			}
			return true;
		}

		internal bool UpdateDataFormat(string strFormat, Action cbAction, int errorCode, bool initOnly, ref int retVal)
		{
			return UpdateDataFormat(strFormat, cbAction, errorCode, initOnly, createNew: false, ref retVal);
		}

		internal bool UpdateDataFormat(string strFormat, Action cbAction, int errorCode, bool initOnly, bool createNew, ref int retVal)
		{
			bool flag = false;
			bool isMDimArray = false;
			bool propReadingFormat = base.propReadingFormat;
			base.propReadingFormat = false;
			int num = strFormat.IndexOf("\0");
			try
			{
				if (-1 != num)
				{
					strFormat = strFormat.Substring(0, num);
				}
				if (0 < strFormat.Length)
				{
					propPviValue.InitializeExtendedAttributes();
					flag = GetExtendedAttributes(strFormat, ref isMDimArray, ref propPviValue);
					if (isMDimArray)
					{
						propPviValue.SetArrayIndex(strFormat);
					}
					DataType dataType = GetDataType(strFormat, Value.IsBitString, ref propPviValue.propTypeLength);
					GetVSParameters(strFormat, ref propPviValue, dataType);
					bool flag2 = true;
					if (Action.VariableReadFormatInternal != cbAction && !propReadingFormat)
					{
						flag2 = UpdateDataFormatForInternalRead(flag, dataType, cbAction);
					}
					if (!flag2)
					{
						return false;
					}
					int arrayLength = GetArrayLength(strFormat);
					UpdateDataFormatForReRead(strFormat, dataType, arrayLength, cbAction);
					if (Action.VariableInternLink == cbAction || Action.VariableInternFormat == cbAction)
					{
						if (createNew)
						{
							DeleteRequest(silent: true);
							base.Requests = Actions.Connect;
						}
						else
						{
							propNoDisconnectedEvent = true;
							Disconnect(2717u);
							propNoDisconnectedEvent = false;
							Connect(ConnectionType.Link);
						}
						return true;
					}
					if (propReadingFormat && cbAction == Action.VariableFormatChanged)
					{
						return true;
					}
					retVal = UpdateDataFormatForInit(strFormat, initOnly, cbAction, errorCode);
					return true;
				}
			}
			catch (OutOfMemoryException ex)
			{
				string message = ex.Message;
				CleanupMemory();
				retVal = 14;
			}
			catch (System.Exception ex2)
			{
				string message2 = ex2.Message;
				retVal = 14;
			}
			return false;
		}

		private Access GetAccessType(string attribDesc)
		{
			Access access = propVariableAccess;
			int num = attribDesc.IndexOf("AT=");
			if (-1 < num)
			{
				int num2 = attribDesc.IndexOf(" ", 1 + num);
				string text = (-1 != num2) ? attribDesc.Substring(num + 3, num2 - 3 - num).ToLower() : attribDesc.Substring(num + 3);
				text.Trim();
				if (0 < text.Length)
				{
					access = Access.No;
					if (-1 != text.IndexOf("r"))
					{
						access |= Access.Read;
					}
					if (-1 != text.IndexOf("w"))
					{
						access |= Access.Write;
					}
					if (-1 != text.IndexOf("e"))
					{
						access |= Access.EVENT;
					}
					if (-1 != text.IndexOf("d"))
					{
						access |= Access.DIRECT;
					}
					if (-1 != text.IndexOf("h"))
					{
						access |= Access.FASTECHO;
					}
				}
			}
			return access;
		}

		[CLSCompliant(false)]
		protected void UpdateValueData(IntPtr pData, uint dataLen, int error)
		{
			int num = 0;
			ArrayList changedMembers = new ArrayList(1);
			propChangedStructMembers = new string[changedMembers.Count];
			if (0 < dataLen)
			{
				num = ConvertPviValue(pData, dataLen, ref changedMembers);
				propChangedStructMembers = new string[changedMembers.Count];
				for (int i = 0; i < changedMembers.Count; i++)
				{
					propChangedStructMembers.SetValue(changedMembers[i], i);
				}
			}
			if (propActive)
			{
				OnValueChanged(new VariableEventArgs(base.Name, base.Address, (num != 0) ? num : error, Service.Language, Action.VariableValueChangedEvent, propChangedStructMembers));
			}
			changedMembers = null;
		}

		private void ValidateConnection(int errorCode)
		{
			if ((!Service.IsStatic || ConnectionType.Link == base.ConnectionType) && !IsConnected)
			{
				if (this is IOVariable)
				{
					Read_State(base.LinkId, 557u);
				}
				else if (!propReadingState && errorCode == 0 && propPviValue.DataType != 0 && !propWaitForUserTag)
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
				}
			}
			if (!propReadingState && propPviValue.DataType != 0 && DataType.Structure != propPviValue.DataType && !IsConnected && !propWaitForUserTag)
			{
				OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ConnectedEvent, Service));
			}
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
			propErrorCode = errorCode;
			if (propErrorCode == 0 || 12002 == propErrorCode)
			{
				if (ConnectionType.CreateAndLink == base.ConnectionType)
				{
					propLinkId = linkID;
					if (Service.IsStatic)
					{
						propPVState = ConnectionType.Create;
						if (int.MinValue == ((int)base.Requests & int.MinValue))
						{
							base.Requests &= ~Actions.Link;
						}
						if (propActive && !propLinkParam.Contains("d") && Access != 0 && Access.Write != Access)
						{
							if (propLinkParam.Contains("VT="))
							{
								propLinkParam = propLinkParam.Replace("f VT", "fd VT");
							}
							else
							{
								propLinkParam += "d";
							}
						}
						propReturnValue = PviLinkObject(701u);
					}
					else if (propSNMPParent != null)
					{
						Read_FormatEX(propLinkId);
					}
				}
				else
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
				}
			}
			else
			{
				if (Service.IsRemoteError(errorCode))
				{
					base.Requests |= Actions.Connect;
				}
				OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
			}
		}

		internal override void OnPviLinked(int errorCode, uint linkID, int option)
		{
			propErrorCode = errorCode;
			if (propErrorCode == 0 || 12002 == propErrorCode)
			{
				propLinkId = linkID;
				if (1 == option)
				{
					if (errorCode != 0)
					{
						Service.OnPVIObjectsAttached(new PviEventArgs(propName, propAddress, errorCode, Service.Language, Action.TaskReadVariablesList));
					}
					return;
				}
				if (!IsConnected && !(Parent is Service) && 2 != option)
				{
					if (this is IOVariable)
					{
						Read_State(linkID, 557u);
					}
					else if (propPviValue.propDataType == DataType.Unknown)
					{
						Read_FormatEX(propLinkId);
					}
					return;
				}
				if (!IsConnected && !(Parent is Service))
				{
					Read_FormatEX(propLinkId);
					return;
				}
				PviEventArgs e;
				switch (option)
				{
				case 3:
					e = new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableScalingChange, Service);
					OnPropertyChanged(e);
					break;
				case 4:
					e = new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableSetHysteresis, Service);
					OnPropertyChanged(e);
					break;
				case 5:
					e = new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableSetType, Service);
					OnPropertyChanged(e);
					break;
				default:
					e = new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableLink, Service);
					break;
				}
				CheckActiveRequests(e);
			}
			else
			{
				OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
			}
		}

		private void ErrorEventHandling(int errorCode, int option, ConnectionStates conState)
		{
			int retVal = 0;
			if (12033 == errorCode && propSNMPParent != null)
			{
				propPviValue.TypePreset = true;
				if (1 == option)
				{
					UpdateDataFormat("VT=string VL=1024 VN=1", Action.VariableInternLink, 0, initOnly: false, createNew: true, ref retVal);
				}
				else
				{
					UpdateDataFormat("VT=string VL=1024 VN=1", Action.VariableFormatChanged, 0, initOnly: false, createNew: true, ref retVal);
				}
				return;
			}
			if (IsConnected)
			{
				OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.DisconnectedEvent, Service));
				if (ConnectionStates.Connected == conState)
				{
					propConnectionState = ConnectionStates.ConnectedError;
				}
			}
			else if (ConnectionStates.Connecting == propConnectionState)
			{
				OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.DisconnectedEvent, Service));
			}
			OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ErrorEvent, Service));
		}

		private void ZeroErrorEventHandling(int option, ConnectionStates conState)
		{
			if (this is IOVariable)
			{
				if (IsConnected && !(propParent is Service))
				{
					Read_State(base.LinkId, 557u);
				}
			}
			else if (propPviValue.DataType == DataType.Unknown || Actions.ReadPVFormat == (base.Requests & Actions.ReadPVFormat))
			{
				if (Cpu != null)
				{
					if (!Cpu.IsSG4Target)
					{
						base.Requests |= Actions.ReadPVFormat;
						Read_State(propLinkId, 2812u);
					}
					else
					{
						Read_FormatEX(propLinkId);
					}
				}
				else
				{
					Read_FormatEX(propLinkId);
				}
			}
			else if (Cpu != null && !Cpu.IsSG4Target)
			{
				Read_State(propLinkId, 2812u);
			}
			else
			{
				OnConnected(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.VariableConnect, Service));
			}
		}

		private void OnPviEventError(int errorCode, int option, ConnectionStates conState)
		{
			if (errorCode != 0)
			{
				ErrorEventHandling(errorCode, option, conState);
			}
			else
			{
				ZeroErrorEventHandling(option, conState);
			}
		}

		private void OnPviEventData(int errorCode, IntPtr pData, uint dataLen)
		{
			int num = 0;
			ValidateConnection(errorCode);
			if (ExpandMembers)
			{
				if (propPviValue.DataType == DataType.Unknown)
				{
					base.Requests |= Actions.GetValue;
					Read_FormatEX(propLinkId);
				}
				else if (propPviValue.DataType != 0)
				{
					UpdateValueData(pData, dataLen, errorCode);
				}
				else
				{
					ReadInternalValue();
				}
				return;
			}
			if (propPviValue.DataType == DataType.Unknown)
			{
				base.Requests |= Actions.GetValue;
				Read_FormatEX(propLinkId);
				return;
			}
			ArrayList changedMembers = new ArrayList(1);
			if (0 < dataLen)
			{
				num = ConvertPviValue(pData, dataLen, ref changedMembers);
			}
			propChangedStructMembers = new string[changedMembers.Count];
			for (int i = 0; i < changedMembers.Count; i++)
			{
				propChangedStructMembers.SetValue(changedMembers[i], i);
			}
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableValueChangedEvent, Service));
				OnValueChanged(new VariableEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableValueChangedEvent, propChangedStructMembers));
			}
			else
			{
				OnValueChanged(new VariableEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableValueChangedEvent, propChangedStructMembers));
			}
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			int propErrorCode = propErrorCode;
			propErrorCode = errorCode;
			ConnectionStates propConnectionState = base.propConnectionState;
			if (errorCode != 0 && EventTypes.Error != eventType)
			{
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
				return;
			}
			switch (eventType)
			{
			case EventTypes.Error:
				OnPviEventError(errorCode, option, propConnectionState);
				break;
			case EventTypes.Dataform:
				if (1 == option)
				{
					DataFormat_Changed(errorCode, pData, dataLen, Action.VariableInternLink);
				}
				else
				{
					DataFormat_Changed(errorCode, pData, dataLen, Action.VariableFormatChanged);
				}
				break;
			case EventTypes.Status:
			{
				if (0 >= dataLen)
				{
					break;
				}
				string text3 = "";
				text3 = PviMarshal.ToAnsiString(pData, dataLen);
				if (GetForceStatus(text3, ref propForceValue))
				{
					string address = base.Address;
					if (propParent is Task)
					{
						string text4 = propParent.Address + ":" + base.Address;
					}
				}
				propAttribute = GetAttribute(text3);
				propStatusRead = true;
				if (propPviValue.DataType == DataType.Unknown)
				{
					Read_FormatEX(propLinkId);
				}
				else
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
				}
				break;
			}
			case EventTypes.UserTag:
			{
				if (base.ErrorCode != 0)
				{
					break;
				}
				string text = "";
				string text2 = propUserTag;
				text = PviMarshal.ToAnsiString(pData, dataLen);
				if (0 < dataLen)
				{
					propUserTag = text;
				}
				else
				{
					propUserTag = null;
				}
				if (propWaitForUserTag)
				{
					if ((propPviValue.DataType == DataType.Structure && Members != null && Members.Count > 0) || (propParent is Service && base.ConnectionType != ConnectionType.Link))
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
					}
					propWaitForUserTag = false;
				}
				else if (IsConnected && text2 != null && !text2.Equals(text))
				{
					OnPropertyChanged(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableSetUserTag, Service));
				}
				break;
			}
			case EventTypes.Data:
				OnPviEventData(errorCode, pData, dataLen);
				break;
			default:
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
				break;
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			int num = 0;
			string text = "";
			propErrorCode = errorCode;
			if (errorCode != 0)
			{
				switch (accessType)
				{
				case PVIReadAccessTypes.Data:
					OnValueRead(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableValueRead, Service));
					break;
				case PVIReadAccessTypes.State:
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
					break;
				case PVIReadAccessTypes.LinkNodeList:
					IODataPoints.OnPviRead(errorCode, PVIReadAccessTypes.LinkNodeList, dataState, pData, dataLen, option);
					break;
				case PVIReadAccessTypes.BasicAttributes:
				case PVIReadAccessTypes.ExtendedAttributes:
				case PVIReadAccessTypes.ExtendedInternalAttributes:
					propErrorState = errorCode;
					switch (option)
					{
					case 1:
						OnUploaded(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableUpload, Service));
						break;
					case 2:
						if (Parent is Service)
						{
							ReadRequest(Service.hPvi, base.LinkId, AccessTypes.Type, 2810u);
							break;
						}
						propPviValue.DataType = DataType.Unknown;
						propReadingFormat = false;
						OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableExtendedTypeInfoRead, Service));
						break;
					case 5:
						OnExtendedTypeInfoRead(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableExtendedTypeInfoRead, Service));
						break;
					default:
						base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
						break;
					}
					break;
				default:
					base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
					break;
				}
				return;
			}
			switch (accessType)
			{
			case PVIReadAccessTypes.Data:
			{
				ValidateConnection(errorCode);
				ArrayList changedMembers = new ArrayList(1);
				if (0 < dataLen && propPviValue.propDataType != 0)
				{
					num = ConvertPviValue(pData, dataLen, ref changedMembers);
				}
				propChangedStructMembers = new string[changedMembers.Count];
				for (int i = 0; i < changedMembers.Count; i++)
				{
					propChangedStructMembers.SetValue(changedMembers[i], i);
				}
				if (5 == option && Active)
				{
					OnValueChanged(new VariableEventArgs(base.Name, base.Address, (num != 0) ? num : errorCode, Service.Language, Action.VariableValueChangedEvent, propChangedStructMembers));
				}
				else
				{
					OnValueRead(new PviEventArgs(base.Name, base.Address, (num != 0) ? num : errorCode, Service.Language, Action.VariableValueRead, Service));
				}
				break;
			}
			case PVIReadAccessTypes.State:
				if (0 >= dataLen)
				{
					break;
				}
				text = PviMarshal.ToAnsiString(pData, dataLen);
				propReadingState = false;
				if (GetForceStatus(text, ref propForceValue))
				{
					string address = base.Address;
					if (propParent is Task)
					{
						string text2 = propParent.Address + ":" + base.Address;
					}
				}
				propStatusRead = true;
				propAttribute = GetAttribute(text);
				if (propPviValue.DataType != 0)
				{
					OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
				}
				else if (Actions.ReadPVFormat == (base.Requests & Actions.ReadPVFormat))
				{
					Read_FormatEX(propLinkId);
				}
				break;
			case PVIReadAccessTypes.BasicAttributes:
			case PVIReadAccessTypes.ExtendedAttributes:
			case PVIReadAccessTypes.ExtendedInternalAttributes:
				propErrorState = errorCode;
				switch (option)
				{
				case 1:
					if (0 < dataLen)
					{
						text = ((PVIReadAccessTypes.BasicAttributes != accessType) ? PviMarshal.PtrToStringAnsi(pData, dataLen) : PviMarshal.ToAnsiString(pData, dataLen));
						num = InterpretTypInfo(errorCode, text, 700);
						if (propSendUploadEvent)
						{
							if (num != 0)
							{
								OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableUpload, Service));
								OnUploaded(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableUpload, Service));
							}
							else
							{
								OnUploaded(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableUpload, Service));
							}
						}
						else if (num != 0)
						{
							OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableConnect, Service));
							OnConnected(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableConnect, Service));
						}
					}
					else
					{
						OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableExtendedTypeInfoRead, Service));
					}
					break;
				case 2:
					DataFormat_Changed(errorCode, pData, dataLen, Action.VariableReadFormatInternal);
					break;
				case 5:
					ExtractExtendedTypeInfo(errorCode, pData, dataLen);
					break;
				default:
					propReadingFormat = false;
					if (0 < dataLen)
					{
						text = PviMarshal.PtrToStringAnsi(pData, dataLen);
						num = InterpretTypInfo(errorCode, text, 516);
					}
					else
					{
						OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableExtendedTypeInfoRead, Service));
					}
					break;
				}
				break;
			case PVIReadAccessTypes.LinkNodeList:
				IODataPoints.OnPviRead(errorCode, PVIReadAccessTypes.LinkNodeList, dataState, pData, dataLen, option);
				break;
			default:
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
				break;
			}
		}

		private int InterpretTypInfo(int errorCode, string pviText, int actionCode)
		{
			return InterpretTypInfo(errorCode, pviText, actionCode, skipConnected: false);
		}

		private int InterpretTypInfo(int errorCode, string pviText, int actionCode, bool skipConnected)
		{
			bool isMDimArray = false;
			int retVal = 0;
			try
			{
				propPviValue.InitializeExtendedAttributes();
				int num = pviText.IndexOf("\0");
				if (-1 != num)
				{
					pviText = pviText.Substring(0, num);
				}
				GetScope(pviText, ref propScope);
				GetExtendedAttributes(pviText, ref isMDimArray, ref propPviValue);
				if (isMDimArray)
				{
					propPviValue.SetArrayIndex(pviText);
				}
				propPviValue.SetDataType(GetDataType(pviText, Value.IsBitString, ref propPviValue.propTypeLength));
				propPviValue.SetArrayLength(GetArrayLength(pviText));
				if (propPviValue.ArrayMinIndex == 0)
				{
					propPviValue.propArrayMaxIndex = propPviValue.ArrayLength - 1;
				}
				GetVSParameters(pviText, ref propPviValue, propPviValue.DataType);
				propPviValue.propTypeLength = GetDataTypeLength(pviText, propPviValue.propTypeLength);
				if (propPviValue.propDataType == DataType.Structure)
				{
					CreateMembers(pviText, ref propPviValue.propTypeLength, propExpandMembers);
				}
				else
				{
					if (!propPviValue.IsOfTypeArray)
					{
						UpdateDataFormat(pviText, (Action)actionCode, errorCode, initOnly: false, ref retVal);
						return retVal;
					}
					CreateMembersNOExpand(pviText, ref propPviValue.propTypeLength, Service.AddMembersToVariableCollection);
				}
				if (propSendUploadEvent)
				{
					return retVal;
				}
				if (!propWaitForUserTag && !IsConnected)
				{
					if (!skipConnected)
					{
						OnConnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableConnect, Service));
					}
					if (Actions.GetValue != (base.Requests & Actions.GetValue))
					{
						return retVal;
					}
					base.Requests &= ~Actions.GetValue;
					ReadInternalValue();
					return retVal;
				}
				if (Actions.GetValue != (base.Requests & Actions.GetValue))
				{
					return retVal;
				}
				base.Requests &= ~Actions.GetValue;
				ReadInternalValue();
				return retVal;
			}
			catch (OutOfMemoryException ex)
			{
				string message = ex.Message;
				CleanupMemory();
				return 14;
			}
			catch (System.Exception ex2)
			{
				string message2 = ex2.Message;
				return 14;
			}
		}

		internal override void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
			propErrorCode = errorCode;
			switch (accessType)
			{
			case PVIWriteAccessTypes.EventMask:
				switch (option)
				{
				case 2:
					break;
				case 1:
					OnActivated(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableActivate, Service));
					break;
				default:
					OnDeactivated(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableDeactivate, Service));
					break;
				}
				break;
			case PVIWriteAccessTypes.Refresh:
				OnPropertyChanged(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableSetRefreshTime, Service));
				break;
			case PVIWriteAccessTypes.Hysteresis:
				OnPropertyChanged(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableSetHysteresis, Service));
				break;
			case PVIWriteAccessTypes.ConversionFunction:
				OnPropertyChanged(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableScalingChange));
				break;
			case PVIWriteAccessTypes.ForceOn:
				OnForcedOn(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableSetForce, Service));
				break;
			case PVIWriteAccessTypes.ForceOff:
				OnForcedOff(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableSetForce, Service));
				break;
			case PVIWriteAccessTypes.BasicAttributes:
				OnPropertyChanged(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableSetType, Service));
				break;
			case PVIWriteAccessTypes.UserTag:
				OnPropertyChanged(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.UserTag, Service));
				break;
			case PVIWriteAccessTypes.Data:
				ValidateConnection(errorCode);
				if (!propActive && !propPviValue.isAssigned && PVRoot.propWriteByteField != null)
				{
					PVRoot.propPviValue.propByteField = (byte[])PVRoot.propWriteByteField.Clone();
				}
				OnValueWritten(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableValueWrite, Service));
				if (propSendChangedEvent)
				{
					propSendChangedEvent = false;
					if (errorCode == 0)
					{
						ReadInternalValue();
					}
				}
				break;
			default:
				base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
				break;
			}
		}

		internal override void OnPviUnLinked(int errorCode, int option)
		{
			propErrorCode = errorCode;
			switch (option)
			{
			case 3:
				break;
			case 1:
			case 2:
				if (ConnectionStates.Disconnecting == propConnectionState)
				{
					propLinkId = 0u;
					if (Service != null)
					{
						if (2 == option)
						{
							OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariablesDisconnect, Service));
						}
						else
						{
							OnDisconnected(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableDisconnect, Service));
						}
					}
				}
				else if (Service != null)
				{
					OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.VariableDisconnect, Service));
				}
				break;
			case 4:
				propConnectionState = ConnectionStates.Unininitialized;
				propLinkId = 0u;
				Connect();
				break;
			}
		}

		internal override void OnPviDeleted(int errorCode)
		{
			propErrorCode = errorCode;
			base.OnPviDeleted(errorCode);
		}

		internal override void OnPviChangedLink(int errorCode)
		{
			propErrorCode = errorCode;
			base.OnPviChangedLink(errorCode);
		}

		internal static Value GetValueFromString(string strValue, DataType dataType)
		{
			switch (dataType)
			{
			case DataType.Boolean:
				return new Value(System.Convert.ToBoolean(strValue));
			case DataType.UInt8:
				return new Value(System.Convert.ToByte(strValue));
			case DataType.Byte:
				return new Value(System.Convert.ToByte(strValue));
			case DataType.SByte:
				return new Value(System.Convert.ToSByte(strValue));
			case DataType.Int16:
				return new Value(System.Convert.ToInt16(strValue));
			case DataType.UInt16:
				return new Value(System.Convert.ToUInt16(strValue));
			case DataType.WORD:
				return new Value(System.Convert.ToUInt16(strValue));
			case DataType.Int32:
				return new Value(System.Convert.ToInt32(strValue));
			case DataType.Int64:
				return new Value(System.Convert.ToInt64(strValue));
			case DataType.UInt32:
				return new Value(System.Convert.ToUInt32(strValue));
			case DataType.DWORD:
				return new Value(System.Convert.ToUInt32(strValue));
			case DataType.UInt64:
				return new Value(System.Convert.ToUInt64(strValue));
			case DataType.Single:
				return new Value(System.Convert.ToSingle(strValue));
			case DataType.Double:
				return new Value(System.Convert.ToDouble(strValue));
			case DataType.DateTime:
				return new Value(System.Convert.ToDateTime(strValue));
			case DataType.DT:
				return new Value(System.Convert.ToDateTime(strValue));
			case DataType.Date:
				return new Value(System.Convert.ToDateTime(strValue));
			case DataType.TimeSpan:
				return new Value(System.Convert.ToDateTime(strValue));
			case DataType.TOD:
				return new Value(System.Convert.ToDateTime(strValue));
			case DataType.TimeOfDay:
				return new Value(System.Convert.ToDateTime(strValue));
			case DataType.String:
				return new Value(strValue);
			default:
				return null;
			}
		}

		private unsafe int ConvertComplexValue(IntPtr pData, ref ArrayList changedMembers)
		{
			int num = 0;
			Variable variable = null;
			if (propPviValue.DataType == DataType.Structure)
			{
				for (int i = 0; i < Members.Count; i++)
				{
					byte* ptr = (byte*)pData.ToPointer() + Members[i].propOffset;
					if (propPviValue.ArrayLength <= 0)
					{
						continue;
					}
					int num2 = 0;
					string text = "";
					for (int j = 0; j < propPviValue.ArrayLength; j++)
					{
						for (int k = 0; k < Members[i].propPviValue.propTypeLength * Members[i].propPviValue.propArrayLength; k++)
						{
							if ((ptr + k)[(long)j * (long)propPviValue.propTypeLength] == propPviValue.propByteField[Members[i].propOffset + k + j * propPviValue.propTypeLength] && propDataValid)
							{
								continue;
							}
							if (Members[i].propPviValue.DataType == DataType.Structure)
							{
								string text2 = null;
								if (propPviValue.ArrayLength > 1)
								{
									text2 = $"{base.Name}[{j.ToString()}].";
								}
								variable = Members[i];
								int num3 = k % variable.propPviValue.propTypeLength;
								variable = variable.GetMemberByOffset(k % variable.propPviValue.propTypeLength);
								if (variable != null)
								{
									text = variable.Name;
									Variable variable2 = variable;
									do
									{
										variable2 = (Variable)variable2.Owner;
										text = text.Insert(0, ".");
										int num4 = 0;
										int num5 = 0;
										if (variable2.propPviValue.ArrayLength > 1)
										{
											num4 = ((Members[i].Name == variable2.Name) ? (k % ((Variable)variable2.propOwner).propPviValue.propTypeLength) : ((variable2.propOffset >= num3) ? (num3 % ((Variable)variable2.propOwner).propPviValue.propTypeLength) : ((num3 - variable2.propOffset) % ((Variable)variable2.propOwner).propPviValue.propTypeLength)));
											text = text.Insert(0, $"[{(num4 / variable2.propPviValue.TypeLength).ToString()}]");
										}
										text = text.Insert(0, variable2.Name);
									}
									while (Members[i].Name != variable2.Name);
									if (text2 != null)
									{
										text = text.Insert(0, text2);
									}
									bool flag = false;
									if (variable.propPviValue.ArrayLength > 1)
									{
										num2 = ((((Variable)variable.propOwner).propOffset > num3 || !(((Variable)variable.propOwner).Name != variable2.Name)) ? System.Convert.ToInt32((num3 - variable.propOffset) % ((Variable)variable.propOwner).propPviValue.TypeLength / variable.propPviValue.propTypeLength) : System.Convert.ToInt32((num3 - ((Variable)variable.propOwner).propOffset - variable.propOffset) % ((Variable)variable.propOwner).propPviValue.TypeLength / variable.propPviValue.propTypeLength));
										if (num2 > -1 && num2 < variable.propPviValue.ArrayLength)
										{
											text = $"{text}[{num2.ToString()}]";
										}
									}
									foreach (string changedMember in changedMembers)
									{
										if (text.Equals(changedMember))
										{
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										changedMembers[num++] = text;
									}
								}
							}
							else
							{
								text = Members[i].Name;
								bool flag2 = false;
								num2 = System.Convert.ToInt32(k / Members[i].propPviValue.propTypeLength);
								if (num2 > -1)
								{
									text = ((Members[i].propPviValue.ArrayLength > 1) ? ((propPviValue.ArrayLength <= 1) ? $"{Members[i].Name}[{num2.ToString()}]" : $"{base.Name}[{j.ToString()}].{Members[i].Name}[{num2.ToString()}]") : ((propPviValue.ArrayLength <= 1) ? $"{Members[i].Name}" : $"{base.Name}[{j.ToString()}].{Members[i].Name}"));
								}
								foreach (string changedMember2 in changedMembers)
								{
									if (text.Equals(changedMember2))
									{
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									changedMembers[num++] = text;
								}
							}
							propPviValue.propByteField[Members[i].propOffset + k + j * propPviValue.propTypeLength] = (ptr + k)[(long)j * (long)propPviValue.propTypeLength];
						}
					}
				}
			}
			else
			{
				for (int l = 0; l < propPviValue.propArrayLength; l++)
				{
					byte* ptr2 = (byte*)pData.ToPointer() + (long)l * (long)propPviValue.propTypeLength;
					bool flag3 = false;
					for (int m = 0; m < propPviValue.propTypeLength; m++)
					{
						if (ptr2[m] != propPviValue.propByteField[l * propPviValue.propTypeLength + m] || !propDataValid)
						{
							propPviValue.propByteField[l * propPviValue.propTypeLength + m] = ptr2[m];
							flag3 = true;
						}
					}
					if (flag3)
					{
						changedMembers[num++] = l.ToString();
					}
				}
			}
			PviMarshal.FreeHGlobal(ref propPviValue.pData);
			return 0;
		}

		private void ConvertStructAndArrayValues(IntPtr pData, uint dataLen, ref ArrayList changedMembers)
		{
			if (propPviValue.propByteField == null || propPviValue.propByteField.Length != propPviValue.propArrayLength * propPviValue.propTypeLength)
			{
				propPviValue.propByteField = new byte[propPviValue.propArrayLength * propPviValue.propTypeLength];
			}
			Marshal.Copy(pData, propPviValue.propByteField, 0, (int)dataLen);
			if (!propPviValue.isAssigned)
			{
				if (IntPtr.Zero == propPviValue.pData)
				{
					propPviValue.pData = PviMarshal.AllocHGlobal(propPviValue.DataSize);
					propPviValue.propHasOwnDataPtr = true;
				}
				Marshal.Copy(propPviValue.propByteField, 0, propPviValue.pData, propPviValue.DataSize);
			}
			if (DataType.Structure != propPviValue.DataType)
			{
				if (propPviValue.IsOfTypeArray)
				{
					for (int i = propPviValue.ArrayMinIndex; i < propPviValue.ArrayMaxIndex + 1; i++)
					{
						if (HasDataChanged(propPviValue.propByteField, (i - propPviValue.ArrayMinIndex) * propPviValue.TypeLength, propPviValue.TypeLength))
						{
							changedMembers.Add("[" + i + "]");
						}
					}
				}
				else if (HasDataChanged(propPviValue.propByteField, 0, propPviValue.TypeLength))
				{
					changedMembers.Add("[0]");
				}
			}
			else if (ExpandMembers && propPviValue.DataType != 0 && mapNameToMember != null)
			{
				for (int i = 0; i < mapNameToMember.Count; i++)
				{
					Variable variable = (Variable)mapNameToMember[i];
					if (DataType.Structure != variable.Value.DataType && !variable.Value.IsOfTypeArray)
					{
						if (HasDataChanged(propPviValue.propByteField, variable.propOffset, variable.Value.TypeLength))
						{
							changedMembers.Add(variable.StructMemberName);
						}
						variable.InternalSetValue(pData, dataLen, variable.propOffset);
					}
					else
					{
						variable.Value.UpdateByteField(pData, dataLen, variable.propOffset);
					}
				}
			}
			if (propInternalByteField == null)
			{
				propInternalByteField = new byte[propPviValue.propByteField.Length];
			}
			Marshal.Copy(pData, propInternalByteField, 0, (int)dataLen);
		}

		private int ConvertPviValue(IntPtr pData, uint dataLen, ref ArrayList changedMembers)
		{
			int result = 0;
			try
			{
				if (1 >= propPviValue.ArrayLength && propPviValue.propDataType != DataType.Structure)
				{
					ConvertSimpleValues(pData, dataLen, ref changedMembers, ref propPviValue);
					return result;
				}
				ConvertStructAndArrayValues(pData, dataLen, ref changedMembers);
				return result;
			}
			catch (OutOfMemoryException ex)
			{
				string message = ex.Message;
				CleanupMemory();
				return 14;
			}
			catch (System.Exception ex2)
			{
				string message2 = ex2.Message;
				return 14;
			}
		}

		private void ConvertSimpleValues(IntPtr pData, uint dataLen, ref ArrayList changedMembers, ref Value ppValue)
		{
			if (0 < BitOffset)
			{
				ppValue.propObjValue = System.Convert.ToBoolean(Marshal.ReadByte(pData));
				return;
			}
			switch (propPviValue.propDataType)
			{
			case (DataType)6:
			case (DataType)11:
			case DataType.Structure:
			case DataType.Data:
				break;
			case DataType.Boolean:
			{
				byte value = Marshal.ReadByte(pData);
				ppValue.propObjValue = System.Convert.ToBoolean(value);
				break;
			}
			case DataType.SByte:
				ppValue.propObjValue = (sbyte)Marshal.ReadByte(pData);
				break;
			case DataType.Int16:
				ppValue.propObjValue = Marshal.ReadInt16(pData);
				break;
			case DataType.Int32:
				ppValue.propObjValue = Marshal.ReadInt32(pData);
				break;
			case DataType.Int64:
				ppValue.propObjValue = PviMarshal.ReadInt64(pData);
				break;
			case DataType.Byte:
			case DataType.UInt8:
				ppValue.propObjValue = Marshal.ReadByte(pData);
				break;
			case DataType.UInt16:
			case DataType.WORD:
				ppValue.propObjValue = (ushort)Marshal.ReadInt16(pData);
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				ppValue.propObjValue = (uint)Marshal.ReadInt32(pData);
				break;
			case DataType.UInt64:
			case DataType.LWORD:
				ppValue.propObjValue = (ulong)PviMarshal.ReadInt64(pData);
				break;
			case DataType.Single:
			{
				float[] array2 = new float[1];
				Marshal.Copy(pData, array2, 0, 1);
				ppValue.propObjValue = array2[0];
				break;
			}
			case DataType.Double:
			{
				double[] array = new double[1];
				Marshal.Copy(pData, array, 0, 1);
				ppValue.propObjValue = array[0];
				break;
			}
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
			{
				uint num2 = (uint)Marshal.ReadInt32(pData);
				ppValue.propUInt32Val = num2;
				long num3 = num2;
				num3 *= 10000;
				ppValue.propObjValue = new TimeSpan(num3);
				break;
			}
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
			{
				uint num = (uint)Marshal.ReadInt32(pData);
				new DateTime(55L);
				ppValue.propUInt32Val = num;
				ppValue.propObjValue = Pvi.UInt32ToDateTime(num);
				break;
			}
			case DataType.String:
			{
				string text2 = "";
				text2 = PviMarshal.ToAnsiString(pData, dataLen);
				ppValue.propObjValue = text2;
				break;
			}
			case DataType.WString:
			{
				string text = "";
				text = PviMarshal.ToWString(pData, dataLen);
				ppValue.propObjValue = text;
				break;
			}
			}
		}

		private bool HasDataChanged(byte[] bCompare, int offset, int size)
		{
			if (propInternalByteField == null)
			{
				return true;
			}
			for (int i = offset; i < offset + size; i++)
			{
				if (bCompare[i] != propInternalByteField[i])
				{
					return true;
				}
			}
			return false;
		}

		internal virtual void CheckStatus(object state)
		{
		}

		internal static string GetDataTypeString(DataType dataType)
		{
			switch (dataType)
			{
			case DataType.Boolean:
				return "Boolean";
			case DataType.SByte:
				return "SByte";
			case DataType.Int16:
				return "Int16";
			case DataType.Int32:
				return "Int32";
			case DataType.Int64:
				return "Int64";
			case DataType.UInt8:
				return "USINT";
			case DataType.Byte:
				return "BYTE";
			case DataType.UInt16:
				return "UInt16";
			case DataType.WORD:
				return "WORD";
			case DataType.UInt32:
				return "UInt32";
			case DataType.DWORD:
				return "DWORD";
			case DataType.UInt64:
				return "UInt64";
			case DataType.LWORD:
				return "LWORD";
			case DataType.Single:
				return "Single";
			case DataType.Double:
				return "Double";
			case DataType.TimeSpan:
				return "Timespan";
			case DataType.TimeOfDay:
				return "TIME_OF_DAY";
			case DataType.DateTime:
				return "DateTime";
			case DataType.TOD:
				return "TOD";
			case DataType.DT:
				return "DT";
			case DataType.Date:
				return "DATE";
			case DataType.String:
				return "String";
			case DataType.WString:
				return "WString";
			case DataType.Structure:
				return "Structure";
			default:
				return "";
			}
		}

		internal static DataType GetDataTypeFromString(string name)
		{
			if (name.IndexOf("Structure") != -1)
			{
				return DataType.Structure;
			}
			if (name.Equals("Boolean"))
			{
				return DataType.Boolean;
			}
			if (name.Equals("SByte"))
			{
				return DataType.SByte;
			}
			if (name.Equals("Int16"))
			{
				return DataType.Int16;
			}
			if (name.Equals("Int32"))
			{
				return DataType.Int32;
			}
			if (name.Equals("Int64"))
			{
				return DataType.Int64;
			}
			if (name.Equals("Byte"))
			{
				return DataType.Byte;
			}
			if (name.Equals("BYTE"))
			{
				return DataType.Byte;
			}
			if (name.Equals("USINT"))
			{
				return DataType.UInt8;
			}
			if (name.Equals("UInt8"))
			{
				return DataType.UInt8;
			}
			if (name.Equals("UInt16"))
			{
				return DataType.UInt16;
			}
			if (name.Equals("UINT"))
			{
				return DataType.UInt16;
			}
			if (name.Equals("WORD"))
			{
				return DataType.WORD;
			}
			if (name.Equals("LWORD"))
			{
				return DataType.LWORD;
			}
			if (name.Equals("UInt32"))
			{
				return DataType.UInt32;
			}
			if (name.Equals("DWORD"))
			{
				return DataType.DWORD;
			}
			if (name.Equals("UInt64"))
			{
				return DataType.UInt64;
			}
			if (name.Equals("Single"))
			{
				return DataType.Single;
			}
			if (name.Equals("Double"))
			{
				return DataType.Double;
			}
			if (name.Equals("TimeSpan"))
			{
				return DataType.TimeSpan;
			}
			if (name.Equals("TimeOfDay"))
			{
				return DataType.TimeOfDay;
			}
			if (name.Equals("TIME_OF_DAY"))
			{
				return DataType.TimeOfDay;
			}
			if (name.Equals("TOD"))
			{
				return DataType.TOD;
			}
			if (name.Equals("DateTime"))
			{
				return DataType.DateTime;
			}
			if (name.Equals("Date"))
			{
				return DataType.Date;
			}
			if (name.Equals("DATE_AND_TIME"))
			{
				return DataType.DateTime;
			}
			if (name.Equals("DT"))
			{
				return DataType.DT;
			}
			if (name.Equals("DATE"))
			{
				return DataType.Date;
			}
			if (name.Equals("D"))
			{
				return DataType.Date;
			}
			if (name.Equals("String"))
			{
				return DataType.String;
			}
			if (name.Equals("WString"))
			{
				return DataType.WString;
			}
			if (name.Equals("WSTRING"))
			{
				return DataType.WString;
			}
			return DataType.Unknown;
		}

		internal static DataType GetDataType(string pviString, sbyte isBitString, ref int typeLength)
		{
			int num = 0;
			DataType result = DataType.Unknown;
			if (pviString.Length == 0)
			{
				return DataType.Unknown;
			}
			num = pviString.IndexOf("VT=");
			if (num != -1)
			{
				if (string.Compare(pviString, num + 3, "i8", 0, "i8".Length) == 0)
				{
					result = DataType.SByte;
				}
				else if (string.Compare(pviString, num + 3, "i16", 0, "i16".Length) == 0)
				{
					result = DataType.Int16;
					typeLength = 2;
				}
				else if (string.Compare(pviString, num + 3, "i32", 0, "i32".Length) == 0)
				{
					result = DataType.Int32;
					typeLength = 4;
				}
				else if (string.Compare(pviString, num + 3, "i64", 0, "i64".Length) == 0)
				{
					result = DataType.Int64;
					typeLength = 8;
				}
				else if (string.Compare(pviString, num + 3, "u8", 0, "u8".Length) == 0)
				{
					result = DataType.UInt8;
					typeLength = 1;
					if (1 == isBitString)
					{
						result = DataType.Byte;
					}
				}
				else if (string.Compare(pviString, num + 3, "byte", 0, "u8".Length) == 0)
				{
					result = DataType.Byte;
					typeLength = 1;
				}
				else if (string.Compare(pviString, num + 3, "u16", 0, "u16".Length) == 0)
				{
					result = DataType.UInt16;
					typeLength = 2;
					if (1 == isBitString)
					{
						result = DataType.WORD;
					}
				}
				else if (string.Compare(pviString, num + 3, "WORD", 0, "WORD".Length) == 0)
				{
					result = DataType.WORD;
					typeLength = 2;
				}
				else if (string.Compare(pviString, num + 3, "u32", 0, "u32".Length) == 0)
				{
					result = DataType.UInt32;
					typeLength = 4;
					if (1 == isBitString)
					{
						result = DataType.DWORD;
					}
				}
				else if (string.Compare(pviString, num + 3, "DWORD", 0, "DWORD".Length) == 0)
				{
					result = DataType.DWORD;
					typeLength = 4;
				}
				else if (string.Compare(pviString, num + 3, "u64", 0, "u64".Length) == 0)
				{
					result = DataType.UInt64;
					typeLength = 8;
					if (1 == isBitString)
					{
						result = DataType.LWORD;
					}
				}
				else if (string.Compare(pviString, num + 3, "f32", 0, "f32".Length) == 0)
				{
					result = DataType.Single;
					typeLength = 4;
				}
				else if (string.Compare(pviString, num + 3, "f64", 0, "f64".Length) == 0)
				{
					result = DataType.Double;
					typeLength = 8;
				}
				else if (string.Compare(pviString, num + 3, "string", 0, "string".Length) == 0)
				{
					result = DataType.String;
				}
				else if (string.Compare(pviString, num + 3, "wstring", 0, "wstring".Length) == 0)
				{
					result = DataType.WString;
				}
				else if (string.Compare(pviString, num + 3, "boolean", 0, "boolean".Length) == 0)
				{
					result = DataType.Boolean;
					typeLength = 1;
				}
				else if (string.Compare(pviString, num + 3, "struct", 0, "struct".Length) == 0)
				{
					result = DataType.Structure;
				}
				else if (string.Compare(pviString, num + 3, "dt", 0, "dt".Length) == 0)
				{
					result = DataType.DT;
					typeLength = 4;
				}
				else if (string.Compare(pviString, num + 3, "date", 0, "date".Length) == 0)
				{
					result = DataType.Date;
					typeLength = 4;
				}
				else if (string.Compare(pviString, num + 3, "time", 0, "time".Length) == 0)
				{
					result = DataType.TimeSpan;
					typeLength = 4;
				}
				else if (string.Compare(pviString, num + 3, "tod", 0, "tod".Length) != 0)
				{
					result = ((string.Compare(pviString, num + 3, "data", 0, "data".Length) == 0) ? DataType.Data : DataType.Unknown);
				}
				else
				{
					result = DataType.TOD;
					typeLength = 4;
				}
			}
			return result;
		}

		internal static int GetDataTypeLength(string pviString, int defaultTypeLen)
		{
			int result = defaultTypeLen;
			int num = 0;
			int num2 = 0;
			if (0 < pviString.Length)
			{
				num = pviString.IndexOf("VL=");
				if (-1 != num)
				{
					num2 = pviString.IndexOf(" ", num + 3);
					pviString = ((num2 == -1) ? pviString.Substring(num + 3, pviString.Length - num - 3) : pviString.Substring(num + 3, num2 - num - 3));
					result = System.Convert.ToInt32(pviString);
				}
			}
			return result;
		}

		internal static int GetArrayLength(string pviString)
		{
			int num = 0;
			int num2 = 0;
			if (pviString.Length == 0)
			{
				return -1;
			}
			num = pviString.IndexOf("VN=");
			num2 = pviString.IndexOf(" ", num + 3);
			pviString = ((num2 == -1) ? pviString.Substring(num + 3, pviString.Length - num - 3) : pviString.Substring(num + 3, num2 - num - 3));
			num = pviString.IndexOf('}');
			if (-1 != num)
			{
				pviString = pviString.Substring(0, num);
			}
			return System.Convert.ToInt32(pviString);
		}

		internal static string GetVariableName(string pviString)
		{
			int num = 0;
			num = pviString.IndexOf(" ");
			if (num != -1)
			{
				return pviString.Substring(0, num);
			}
			return null;
		}

		internal static bool GetForceStatus(string pviString, ref bool forceValue)
		{
			int num = 0;
			if (pviString.Length == 0)
			{
				return false;
			}
			num = pviString.IndexOf("FC=");
			if (num == -1)
			{
				return false;
			}
			pviString = pviString.Substring(num + 3, 1);
			forceValue = System.Convert.ToBoolean(System.Convert.ToByte(pviString));
			return true;
		}

		internal static Access GetAccessStatus(string pviString)
		{
			int num = 0;
			if (pviString.Length == 0)
			{
				return Access.No;
			}
			num = pviString.IndexOf("IO=");
			if (num == -1)
			{
				return Access.No;
			}
			pviString = pviString.Substring(num + 3, 1);
			if (-1 != pviString.IndexOf("r"))
			{
				return Access.Read;
			}
			return Access.Write;
		}

		internal static VariableAttribute GetAttribute(string pviString)
		{
			int num = 0;
			int num2 = 0;
			VariableAttribute variableAttribute = VariableAttribute.None;
			string text = "";
			if (pviString.Length == 0)
			{
				return VariableAttribute.None;
			}
			num = pviString.IndexOf("IO=");
			if (num != -1)
			{
				text = pviString.Substring(num + 3, 1);
			}
			if (-1 != text.IndexOf("w"))
			{
				variableAttribute |= VariableAttribute.Output;
			}
			if (-1 != text.IndexOf("r"))
			{
				variableAttribute |= VariableAttribute.Input;
			}
			num = pviString.IndexOf("ST=");
			num2 = pviString.IndexOf(" ", num);
			text = ((num2 == -1) ? pviString.Substring(num + 3) : pviString.Substring(num + 3, num2 - num - 3));
			if (string.Compare(text, "Const") == 0)
			{
				variableAttribute |= VariableAttribute.Constant;
			}
			else if (string.Compare(text, "Var") == 0)
			{
				variableAttribute |= VariableAttribute.Variable;
			}
			return variableAttribute;
		}

		internal void GetScope(string pviString, ref Scope sopeValue)
		{
			int num = 0;
			num = pviString.IndexOf("SC=");
			pviString = pviString.Substring(num + 3, 1);
			if (string.Compare(pviString, "g") == 0)
			{
				sopeValue = Scope.Global;
			}
			else if (string.Compare(pviString, "l") == 0)
			{
				sopeValue = Scope.Local;
			}
			else if (string.Compare(pviString, "d") == 0)
			{
				sopeValue = Scope.Dynamic;
			}
		}

		internal static Scope EvaluateScope(string pviString)
		{
			int num = 0;
			num = pviString.IndexOf("SC=");
			pviString = pviString.Substring(num + 3, 1);
			if (string.Compare(pviString, "g") == 0)
			{
				return Scope.Global;
			}
			if (string.Compare(pviString, "l") == 0)
			{
				return Scope.Local;
			}
			if (string.Compare(pviString, "d") == 0)
			{
				return Scope.Dynamic;
			}
			return Scope.UNDEFINED;
		}

		internal int CreateMembers(string pviText, ref int typeLength, bool expandMembers)
		{
			int num = 0;
			if (expandMembers)
			{
				return CreateMembersExpanded(pviText, ref typeLength, Service.AddMembersToVariableCollection);
			}
			return CreateMembersNOExpand(pviText, ref typeLength, Service.AddMembersToVariableCollection);
		}

		private int CreateMembersNOExpand(string pviText, ref int typeLength, bool addToVCollections)
		{
			int result = 0;
			string[] array = pviText.Split('{');
			if (propMembers != null)
			{
				propMembers.CleanUp(disposing: false);
			}
			if (mapNameToMember != null)
			{
				mapNameToMember.Clear();
				mapNameToMember = null;
			}
			propPviValue.propArrayLength = GetArrayLength(array[0]);
			propAlignment = GetAlignment(array[0]);
			propPviValue.propTypeLength = GetDataTypeLength(array[0], propPviValue.propTypeLength);
			propOffset = 0;
			propInternalOffset = 0;
			if (CastModes.PG2000String == (CastMode & CastModes.PG2000String) && propPviValue.IsOfTypeArray)
			{
				return -1;
			}
			if (addToVCollections)
			{
				AddComplexVariableItem();
			}
			return result;
		}

		internal void AddComplexVariableItem()
		{
			Variable variable = null;
			if (1 < propPviValue.propArrayLength)
			{
				for (int i = propPviValue.ArrayMinIndex; i < propPviValue.ArrayMaxIndex + 1; i++)
				{
					string text = "[" + i.ToString() + "]";
					string text2 = base.Address + text;
					variable = LookupInParentCollection(text, text2, this);
					if (variable == null)
					{
						variable = new Variable(this, text, addToVColls: true);
						variable.Address = text2;
						AddToParentCollection(variable, propParent, Service.AddMembersToVariableCollection);
					}
					variable.propAlignment = propAlignment;
					variable.propScope = propScope;
					variable.Address = base.Address + text;
					variable.propPviValue.propTypeLength = propPviValue.propTypeLength;
					variable.propPviValue.SetArrayLength(1);
					if (variable.propPviValue.ArrayMinIndex == 0)
					{
						variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
					}
					variable.propOffset = propOffset + i * variable.propPviValue.propTypeLength;
					variable.propPviValue.propArryOne = propPviValue.propArryOne;
					variable.propPviValue.propDimensions = propPviValue.propDimensions;
					variable.propPviValue.SetDataType(propPviValue.DataType);
				}
			}
			else if (propPviValue.IsOfTypeArray)
			{
				string text = "[0]";
				string text2 = base.Address + text;
				variable = LookupInParentCollection(text, text2, this);
				if (variable == null)
				{
					variable = new Variable(this, text, addToVColls: true);
					variable.Address = text2;
					AddToParentCollection(variable, propParent, Service.AddMembersToVariableCollection);
				}
				variable.propAlignment = propAlignment;
				variable.propScope = propScope;
				variable.propPviValue.propTypeLength = propPviValue.propTypeLength;
				variable.propPviValue.SetArrayLength(1);
				if (variable.propPviValue.ArrayMinIndex == 0)
				{
					variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
				}
				variable.propOffset = propOffset;
				variable.propPviValue.propArryOne = propPviValue.propArryOne;
				variable.propPviValue.propDimensions = propPviValue.propDimensions;
				variable.propPviValue.SetDataType(propPviValue.DataType);
			}
		}

		private int CreateMembersExpanded(string pviText, ref int typeLength, bool addToVCollections)
		{
			int result = 0;
			Variable variable = null;
			int num = 1;
			Variable variable2 = this;
			int num2 = 0;
			int byteOffset = 0;
			int num3 = 0;
			bool isMDimArray = false;
			string[] array = pviText.Split('{');
			if (propMembers != null)
			{
				propMembers.CleanUp(disposing: false);
			}
			if (mapNameToMember != null)
			{
				mapNameToMember.Clear();
				mapNameToMember = null;
			}
			propPviValue.propArrayLength = GetArrayLength(array[0]);
			if (CastModes.PG2000String == (CastMode & CastModes.PG2000String) && propPviValue.IsOfTypeArray)
			{
				return -1;
			}
			propAlignment = GetAlignment(array[0]);
			GetVSParameters(array[0], ref propPviValue, propPviValue.DataType);
			propStructName = GetSNParameter(array[0]);
			propPviValue.propTypeLength = GetDataTypeLength(array[0], propPviValue.propTypeLength);
			propOffset = 0;
			propInternalOffset = 0;
			if (propPviValue.IsOfTypeArray)
			{
				num3 = 0;
				CreateStructArray(this, this, array, propOffset, ref byteOffset, ref num3, addToVCollections);
				byteOffset = propPviValue.propTypeLength;
			}
			else
			{
				for (num3 = 1; num3 < array.Length; num3++)
				{
					string structElementName = GetStructElementName(array[num3]);
					string[] array2 = structElementName.Split('.');
					num2 = array2.Length;
					if (num > num2)
					{
						for (int num4 = num; num4 > array2.Length; num4--)
						{
							variable2 = (Variable)variable2.propOwner;
						}
					}
					num = array2.Length;
					string text = array2[array2.Length - 1];
					if (text.Length == 0)
					{
						text = propPviInternStructElement.ToString();
						propPviInternStructElement++;
					}
					string text2 = variable2.Address + "." + text;
					if (propUserMembers != null && propUserMembers.Count > 0)
					{
						if (propUserMembers.ContainsKey(text))
						{
							Variable variable3 = propUserMembers[text];
							variable = variable3;
							variable.propOwner = variable2;
							variable.propPviValue.SetDataType(GetDataType(array[num3], variable.Value.IsBitString, ref variable.propPviValue.propTypeLength));
							variable.GetVSParameters(array[num3], ref variable.propPviValue, variable.propPviValue.DataType);
							variable.propPviValue.propTypeLength = GetDataTypeLength(array[num3], variable.propPviValue.propTypeLength);
							variable.propPviValue.SetArrayLength(GetArrayLength(array[num3]));
							if (variable.propPviValue.ArrayMinIndex == 0)
							{
								variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
							}
							variable.propOffset = byteOffset;
							byteOffset = variable.propOffset + variable.propPviValue.propTypeLength * variable.propPviValue.ArrayLength;
							if (variable.propPviValue.DataType == DataType.Structure)
							{
								variable.propStructName = GetSNParameter(array[num3]);
								variable2 = variable;
							}
							AddToParentCollection(variable, propParent, addToVCollections);
							AddMember(variable);
							propUserMembers.Remove(variable3.Name);
							continue;
						}
						variable = LookupInParentCollection(text, text2, this);
						if (variable == null)
						{
							variable = new Variable(isMember: true, variable2, text, addToVCollections);
							variable.Address = text2;
							AddToParentCollection(variable, propParent, addToVCollections);
						}
						variable.propAlignment = propAlignment;
						variable.propScope = propScope;
						variable.propOwner = variable2;
						variable.propPviValue.SetDataType(GetDataType(array[num3], variable.Value.IsBitString, ref variable.propPviValue.propTypeLength));
						variable.GetVSParameters(array[num3], ref variable.propPviValue, variable.propPviValue.DataType);
						variable.propPviValue.propTypeLength = GetDataTypeLength(array[num3], variable.propPviValue.propTypeLength);
						variable.propPviValue.SetArrayLength(GetArrayLength(array[num3]));
						if (variable.propPviValue.ArrayMinIndex == 0)
						{
							variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
						}
						variable.propOffset = byteOffset;
						byteOffset = variable.propOffset + variable.propPviValue.propTypeLength * variable.propPviValue.ArrayLength;
						if (variable.propPviValue.DataType == DataType.Structure)
						{
							variable.propStructName = GetSNParameter(array[num3]);
							variable2 = variable;
						}
						continue;
					}
					variable = LookupInParentCollection(text, text2, this);
					if (variable == null)
					{
						variable = new Variable(isMember: true, variable2, text, addToVCollections);
						variable.Address = text2;
						AddToParentCollection(variable, propParent, addToVCollections);
					}
					variable.propAlignment = propAlignment;
					variable.propScope = propScope;
					variable.propOwner = variable2;
					if (Service != null && Service.AddStructMembersToMembersToo)
					{
						AddStructMembers(this, variable);
					}
					else
					{
						AddStructMember(variable.StructMemberName, variable);
					}
					GetExtendedAttributes(array[num3], ref isMDimArray, ref variable.propPviValue);
					if (isMDimArray)
					{
						variable.propPviValue.SetArrayIndex(array[num3]);
					}
					variable.propPviValue.SetDataType(GetDataType(array[num3], variable.Value.IsBitString, ref variable.propPviValue.propTypeLength));
					variable.GetVSParameters(array[num3], ref variable.propPviValue, variable.propPviValue.DataType);
					variable.propPviValue.propTypeLength = GetDataTypeLength(array[num3], variable.propPviValue.propTypeLength);
					variable.propPviValue.SetArrayLength(GetArrayLength(array[num3]));
					if (variable.propPviValue.ArrayMinIndex == 0)
					{
						variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
					}
					variable.propOffset = byteOffset;
					if (DataType.Structure != variable.propPviValue.DataType)
					{
						byteOffset = variable.propOffset + variable.propPviValue.propTypeLength * variable.propPviValue.ArrayLength;
					}
					if (variable2.propMembers == null)
					{
						variable2.propMembers = new MemberCollection(variable2, "");
					}
					variable2.propMembers.Add(variable);
					if (DataType.Structure == variable.propPviValue.DataType)
					{
						variable.propStructName = GetSNParameter(array[num3]);
					}
					if (variable.propPviValue.IsOfTypeArray)
					{
						CreateStructArray(this, variable, array, variable.propOffset, ref byteOffset, ref num3, addToVCollections);
						num3--;
					}
					else if (variable.propPviValue.DataType == DataType.Structure)
					{
						num3++;
						if (num3 >= array.GetLength(0))
						{
							break;
						}
						CreateNestedStruct(this, variable, array, variable.propOffset, num2, ref num3, addToVCollections);
						byteOffset = variable.propOffset + variable.propPviValue.propTypeLength * variable.propPviValue.ArrayLength;
						num3--;
					}
				}
			}
			if (propUserMembers != null)
			{
				propUserMembers.CopyTo(Members);
			}
			typeLength = byteOffset;
			return result;
		}

		private Variable LookupInParentCollection(string vName, string vAddress, Variable vOwner)
		{
			Variable variable = null;
			Base parent = vOwner.Parent;
			if (parent is Cpu)
			{
				variable = ((Cpu)parent).Variables[vAddress];
			}
			else if (propParent is Task)
			{
				variable = ((Task)parent).Variables[vAddress];
			}
			if (variable != null && variable.Name.CompareTo(vName) == 0)
			{
				return variable;
			}
			return null;
		}

		internal bool ContainedInParentCollection()
		{
			Base propParent = base.propParent;
			while (propParent is Variable)
			{
				propParent = propParent.propParent;
			}
			if (propParent is Cpu)
			{
				return ((Cpu)propParent).Variables.ContainsKey(base.AddressEx);
			}
			if (propParent is Task)
			{
				return ((Task)propParent).Variables.ContainsKey(base.AddressEx);
			}
			return false;
		}

		private void AddToParentCollection(Variable memberVar, Base parentObj, bool addToVCollections)
		{
			if (!addToVCollections)
			{
				return;
			}
			if (parentObj is Cpu)
			{
				if (!((Cpu)parentObj).Variables.ContainsKey(memberVar.AddressEx))
				{
					((Cpu)parentObj).Variables.Add(memberVar.AddressEx, memberVar);
				}
			}
			else if (propParent is Task && !((Task)parentObj).Variables.ContainsKey(memberVar.AddressEx))
			{
				((Task)parentObj).Variables.Add(memberVar.AddressEx, memberVar);
			}
		}

		private void CreateNestedStruct(Variable root, Variable varParent, string[] typeInfo, int offset, int nesting, ref int infoIdx, bool addToVCollections)
		{
			Variable variable = null;
			int byteOffset = offset;
			bool isMDimArray = false;
			if (infoIdx >= typeInfo.GetLength(0))
			{
				return;
			}
			string[] array = GetStructElementName(typeInfo[infoIdx]).Split('.');
			int num = array.Length;
			while (infoIdx < typeInfo.Length && num > nesting)
			{
				string text = array[num - 1];
				if (text.Length == 0)
				{
					text = propPviInternStructElement.ToString();
					propPviInternStructElement++;
				}
				string text2 = varParent.Address + "." + text;
				variable = LookupInParentCollection(text, text2, this);
				if (variable == null)
				{
					variable = new Variable(isMember: true, varParent, text, addToVCollections);
					variable.Address = text2;
					AddToParentCollection(variable, propParent, addToVCollections);
				}
				variable.propAlignment = propAlignment;
				variable.propScope = root.propScope;
				GetExtendedAttributes(typeInfo[infoIdx], ref isMDimArray, ref variable.propPviValue);
				if (isMDimArray)
				{
					variable.propPviValue.SetArrayIndex(typeInfo[infoIdx]);
				}
				variable.propPviValue.SetDataType(GetDataType(typeInfo[infoIdx], variable.Value.IsBitString, ref variable.propPviValue.propTypeLength));
				variable.GetVSParameters(typeInfo[infoIdx], ref variable.propPviValue, variable.propPviValue.DataType);
				variable.propPviValue.propTypeLength = GetDataTypeLength(typeInfo[infoIdx], variable.propPviValue.propTypeLength);
				variable.propPviValue.SetArrayLength(GetArrayLength(typeInfo[infoIdx]));
				if (variable.propPviValue.ArrayMinIndex == 0)
				{
					variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
				}
				variable.propOffset = byteOffset;
				if (DataType.Structure == variable.propPviValue.DataType)
				{
					variable.propStructName = GetSNParameter(typeInfo[infoIdx]);
				}
				if (Service != null && Service.AddStructMembersToMembersToo)
				{
					AddStructMembers(varParent, variable);
				}
				else
				{
					root.AddStructMember(variable.GetStructMemberName(root), variable);
				}
				if (variable.propPviValue.IsOfTypeArray)
				{
					CreateStructArray(root, variable, typeInfo, variable.propOffset, ref byteOffset, ref infoIdx, addToVCollections);
				}
				else if (variable.propPviValue.DataType == DataType.Structure)
				{
					infoIdx++;
					CreateNestedStruct(root, variable, typeInfo, variable.propOffset, num, ref infoIdx, addToVCollections);
					byteOffset = variable.propOffset + variable.propPviValue.propTypeLength * variable.propPviValue.ArrayLength;
				}
				else
				{
					byteOffset = variable.propOffset + variable.propPviValue.propTypeLength * variable.propPviValue.ArrayLength;
					infoIdx++;
				}
				if (infoIdx < typeInfo.Length)
				{
					array = GetStructElementName(typeInfo[infoIdx]).Split('.');
					num = array.Length;
				}
			}
			varParent.propPviValue.propTypeLength = byteOffset - offset;
		}

		private void CreateNestedStructClone(Variable root, Variable varParent, Variable varOwner, Variable varClone, ref int offset, int nesting, bool addToVCollections, bool addStructMembersToMembersToo)
		{
			Variable variable = null;
			int num = nesting;
			if (varClone.Members != null)
			{
				varOwner.propMembers = new MemberCollection(varOwner, varOwner.Address);
				foreach (Variable value in varClone.Members.Values)
				{
					if (-1 == value.Name.IndexOf('.'))
					{
						string text = (value.Name.IndexOf('[') != 0) ? (varOwner.Address + "." + value.Name) : (varOwner.Address + value.Name);
						variable = LookupInParentCollection(value.Name, text, this);
						if (variable == null)
						{
							variable = new Variable(isMember: true, varOwner, value.Name, addToVCollections);
							variable.Address = text;
							AddToParentCollection(variable, varOwner.propParent, addToVCollections);
						}
						variable.propAlignment = varOwner.propAlignment;
						variable.propScope = root.propScope;
						variable.propPviValue.Clone(value.propPviValue);
						variable.propStructName = value.propStructName;
						variable.propOffset = offset;
						if (addStructMembersToMembersToo)
						{
							AddStructMembers(varOwner, variable);
						}
						else
						{
							root.AddStructMember(variable.GetStructMemberName(root), variable);
						}
						if (variable.propPviValue.IsOfTypeArray || variable.propPviValue.DataType == DataType.Structure)
						{
							num++;
							CreateNestedStructClone(root, varOwner, variable, value, ref offset, num, addToVCollections, addStructMembersToMembersToo);
						}
						else
						{
							offset = variable.propOffset + variable.propPviValue.propTypeLength;
						}
					}
				}
			}
		}

		private void AddNextDimensionItem(string strDims, ArrayDimensionArray aDims, int numDim, int totalDims, ref ArrayList lstOfItems)
		{
			if (numDim < totalDims)
			{
				for (int i = aDims[numDim].StartIndex; i < aDims[numDim].EndIndex + 1; i++)
				{
					AddNextDimensionItem(strDims + i.ToString() + ",", aDims, numDim + 1, totalDims, ref lstOfItems);
				}
			}
			else
			{
				string str = strDims.Substring(0, strDims.Length - 1);
				lstOfItems.Add("[" + str + "]");
			}
		}

		private void AddDimensionIndexs(ArrayDimensionArray aDims, ref ArrayList lstOfItems)
		{
			string text = "";
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			for (int i = 0; i < aDims.Count; i++)
			{
				arrayList.Add(aDims[i].StartIndex);
				arrayList2.Add(aDims[i].EndIndex);
			}
			for (int i = aDims[0].StartIndex; i < aDims[0].EndIndex + 1; i++)
			{
				text = i.ToString() + ",";
				AddNextDimensionItem(text, aDims, 1, aDims.Count, ref lstOfItems);
			}
		}

		private void CreateStructArray(Variable root, Variable parentVar, string[] typeInfo, int offset, ref int byteOffset, ref int infoIdx, bool addToVCollections)
		{
			int propTypeLength = parentVar.propPviValue.propTypeLength;
			Variable variable = null;
			Variable variable2 = null;
			int nesting = 0;
			if (root != parentVar)
			{
				string[] array = GetStructElementName(typeInfo[infoIdx]).Split('.');
				nesting = array.Length;
			}
			infoIdx++;
			if (1 < parentVar.propPviValue.propArrayLength)
			{
				ArrayList lstOfItems = new ArrayList();
				if (parentVar.propPviValue.ArrayDimensions != null)
				{
					AddDimensionIndexs(parentVar.propPviValue.ArrayDimensions, ref lstOfItems);
				}
				else
				{
					for (int i = parentVar.propPviValue.ArrayMinIndex; i <= parentVar.propPviValue.ArrayMaxIndex; i++)
					{
						lstOfItems.Add("[" + i.ToString() + "]");
					}
				}
				variable2 = CreateStructArrayElement(root, typeInfo, ref infoIdx, offset, nesting, parentVar, addToVCollections);
				propTypeLength = variable2.propPviValue.propTypeLength;
				parentVar.propPviValue.propTypeLength = propTypeLength;
				for (int i = 1; i < lstOfItems.Count; i++)
				{
					CreateStructArrayElementClone(i, variable2, root, typeInfo, offset, nesting, parentVar, addToVCollections);
				}
				if (DataType.Structure != variable2.propPviValue.DataType)
				{
				}
			}
			else
			{
				string text = "[0]";
				string text2 = parentVar.Address + text;
				variable = LookupInParentCollection(text, text2, this);
				if (variable == null)
				{
					variable = new Variable(isMember: true, parentVar, text, addToVCollections);
					variable.Address = text2;
					AddToParentCollection(variable, propParent, addToVCollections);
				}
				variable.propAlignment = propAlignment;
				variable.propScope = root.propScope;
				variable.propPviValue.propTypeLength = parentVar.Value.TypeLength;
				variable.propPviValue.SetArrayLength(1);
				if (variable.propPviValue.ArrayMinIndex == 0)
				{
					variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
				}
				variable.propOffset = offset;
				variable.propPviValue.propArryOne = false;
				variable.propPviValue.SetDataType(parentVar.Value.DataType);
				if (DataType.Structure == variable.propPviValue.DataType)
				{
					variable.propStructName = GetSNParameter(typeInfo[infoIdx - 1]);
				}
				if (Service != null && Service.AddStructMembersToMembersToo)
				{
					AddStructMembers(root, variable);
				}
				else
				{
					root.AddStructMember(variable.GetStructMemberName(root), variable);
				}
				if (variable.propPviValue.DataType == DataType.Structure)
				{
					CreateNestedStruct(root, variable, typeInfo, variable.propOffset, nesting, ref infoIdx, addToVCollections);
				}
				parentVar.propPviValue.propTypeLength = variable.propPviValue.propTypeLength * variable.propPviValue.propArrayLength;
			}
			if (root != parentVar)
			{
				byteOffset = offset + parentVar.propPviValue.propTypeLength * parentVar.propPviValue.propArrayLength;
			}
		}

		private void CreateStructArrayElementClone(int arrayIdx, Variable cloneRoot, Variable root, string[] typeInfo, int offset, int nesting, Variable parentVar, bool addToVCollections)
		{
			Variable variable = null;
			bool bAddToAll = false;
			int num = 0;
			int num2 = 0;
			num = cloneRoot.propPviValue.propTypeLength;
			string name = "[" + arrayIdx.ToString() + "]";
			num2 = offset + arrayIdx * num;
			variable = new Variable(name, parentVar, addToVCollections, num2, propAlignment, root.propScope);
			if (Service != null && Service.AddStructMembersToMembersToo)
			{
				bAddToAll = true;
			}
			variable.CloneVariable(cloneRoot, root, parentVar, addToVCollections, bAddToAll);
		}

		private Variable CreateStructArrayElement(Variable root, string[] typeInfo, ref int rootInfoIdx, int offset, int nesting, Variable parentVar, bool addToVCollections)
		{
			Variable variable = null;
			string text = "[0]";
			string text2 = parentVar.Address + text;
			variable = LookupInParentCollection(text, text2, this);
			if (variable == null)
			{
				variable = new Variable(isMember: true, parentVar, text, addToVCollections);
				variable.Address = text2;
				AddToParentCollection(variable, propParent, addToVCollections);
			}
			variable.propAlignment = propAlignment;
			variable.propScope = root.propScope;
			variable.propPviValue.propTypeLength = parentVar.Value.TypeLength;
			variable.propPviValue.SetArrayLength(1);
			if (variable.propPviValue.ArrayMinIndex == 0)
			{
				variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
			}
			variable.propOffset = offset;
			variable.propPviValue.SetDataType(parentVar.Value.DataType);
			variable.propStructName = parentVar.propStructName;
			variable.propPviValue.propDerivedFrom = parentVar.Value.DerivedFrom;
			variable.propPviValue.propEnumerations = parentVar.Value.Enumerations;
			variable.propPviValue.SetDataType(parentVar.Value.DataType);
			if (Service != null && Service.AddStructMembersToMembersToo)
			{
				AddStructMembers(parentVar, variable);
			}
			else
			{
				root.AddStructMember(variable.GetStructMemberName(root), variable);
			}
			if (variable.propPviValue.DataType == DataType.Structure)
			{
				CreateNestedStruct(root, variable, typeInfo, variable.propOffset, nesting, ref rootInfoIdx, addToVCollections);
			}
			return variable;
		}

		internal static string GetStructElementName(string pviText)
		{
			int num = 0;
			num = pviText.IndexOf(" ");
			return pviText.Substring(1, num - 1);
		}

		internal static string GetStructureName(string pviText)
		{
			int num = 0;
			int num2 = 0;
			num = pviText.IndexOf("SN=") + 3;
			num2 = pviText.IndexOf(" ", num);
			return pviText.Substring(num, num2 - num);
		}

		internal string GetSNParameter(string pviText)
		{
			string result = null;
			int num = pviText.IndexOf("SN=");
			if (-1 != num)
			{
				int num2 = pviText.IndexOf(" ", num);
				if (-1 == num2)
				{
					num2 = pviText.Length;
				}
				result = pviText.Substring(num + 3, num2 - 3 - num);
			}
			return result;
		}

		private void GetVSParameters(string pviText, ref Value ppVal, DataType basicType)
		{
			string text = null;
			string[] array = null;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text2 = pviText;
			int num4 = text2.IndexOf("\0");
			if (-1 != num4)
			{
				text2 = text2.Substring(0, num4);
			}
			if (-1 != text2.IndexOf('{'))
			{
				text2 = text2.Substring(0, text2.IndexOf('{'));
			}
			num = text2.IndexOf("VS=");
			if (-1 == num)
			{
				return;
			}
			num2 = text2.IndexOf("TN=");
			string[] array2;
			if (-1 != num2)
			{
				text = text2.Substring(num2 + 3);
				array2 = text.Split(' ');
				text = array2.GetValue(0).ToString();
				text.Trim();
				if (text.Length == 1 + text.LastIndexOf(','))
				{
					text = text.Substring(0, text.Length - 1);
				}
				array = text.Split(',');
				ppVal.propIsDerived = 1;
			}
			string text3 = text2.Substring(num + 3);
			string[] array3 = text3.Split(' ');
			string text4 = array3.GetValue(0).ToString();
			num = text4.IndexOf('}');
			if (-1 != num)
			{
				text4 = text4.Substring(0, num);
			}
			array2 = text4.Split(';');
			for (int i = 0; i < array2.Length; i++)
			{
				string text5 = array2.GetValue(i).ToString();
				array3 = text5.Split(',');
				if (1 == array3.Length && "v".CompareTo(array3.GetValue(0).ToString()) == 0)
				{
					ppVal.propIsDerived = 1;
					if (array != null && num3 < array.Length)
					{
						ppVal.SetDerivation(new DerivationBase(array.GetValue(num3).ToString(), basicType));
						num3++;
					}
					else
					{
						ppVal.SetDerivation(new DerivationBase("", basicType));
					}
				}
				else
				{
					if (0 >= array3.Length)
					{
						continue;
					}
					if ("a".CompareTo(array3.GetValue(0).ToString()) == 0)
					{
						if (1 == array2.Length)
						{
							ppVal.propArryOne = ppVal.SetArrayIndex("VS=" + text5);
						}
					}
					else if ("v".CompareTo(array3.GetValue(0).ToString()) == 0)
					{
						ppVal.propIsDerived = 1;
						if (array != null && num3 < array.Length)
						{
							ppVal.SetDerivation(new Int32MinMaxDerivation(array.GetValue(num3).ToString(), basicType, array3));
							num3++;
						}
						else
						{
							ppVal.SetDerivation(new Int32MinMaxDerivation("", basicType, array3));
						}
					}
					else if ("e".CompareTo(array3.GetValue(0).ToString()) == 0)
					{
						ppVal.propIsEnum = 1;
						if (1 >= array3.Length)
						{
							continue;
						}
						if (ppVal.propEnumerations == null)
						{
							if (array != null && num3 < array.Length)
							{
								ppVal.propEnumerations = new EnumArray(array.GetValue(num3).ToString());
								num3++;
							}
							else
							{
								ppVal.propEnumerations = new EnumArray(GetSNParameter(text2));
							}
						}
						ppVal.propEnumerations.AddEnum(new Int32Enum(array3));
					}
					else if ("b".CompareTo(array3.GetValue(0).ToString()) == 0)
					{
						ppVal.propIsBitString = 1;
					}
				}
			}
			if (ppVal.propDimensions != null && 1 == ppVal.propDimensions.Count)
			{
				ppVal.propArrayMaxIndex = ppVal.propDimensions[0].EndIndex;
				ppVal.propArrayMinIndex = ppVal.propDimensions[0].StartIndex;
				ppVal.propArryOne = true;
				ppVal.propDimensions = null;
			}
		}

		internal static int GetAlignment(string pviText)
		{
			return GetAttributeValue(pviText, "AL=");
		}

		internal static int GetOffset(string pviText, int alignment, int byteOffset, int typeLength, ref int internalOffset)
		{
			bool attribIsContained = false;
			internalOffset = GetAttributeValue(pviText, "VO=", ref attribIsContained);
			if (!attribIsContained)
			{
				internalOffset = byteOffset;
			}
			return byteOffset;
		}

		private static int GetAttributeValue(string pviText, string attribute)
		{
			bool attribIsContained = false;
			return GetAttributeValue(pviText, attribute, ref attribIsContained);
		}

		private static int GetAttributeValue(string pviText, string attribute, ref bool attribIsContained)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			attribIsContained = false;
			if (-1 != pviText.IndexOf(attribute))
			{
				num3 = pviText.IndexOf(attribute) + attribute.Length;
				num = pviText.IndexOf(" ", num3);
				num2 = pviText.IndexOf("}", num3);
				if (-1 != num && num < num2)
				{
					num2 = num;
				}
				if (-1 == num2)
				{
					num2 = ((num >= pviText.Length) ? pviText.Length : num);
				}
				attribIsContained = true;
				return System.Convert.ToInt32(pviText.Substring(num3, num2 - num3));
			}
			return 0;
		}

		public void ReadValue()
		{
			ReadValueEx();
		}

		public int ReadExtendedTypeInfo()
		{
			int num = 0;
			if (Parent is Service)
			{
				return ReadRequest(Service.hPvi, base.LinkId, AccessTypes.TypeIntern, 2811u);
			}
			return ReadRequest(Service.hPvi, base.LinkId, AccessTypes.TypeExtern, 2811u);
		}

		public int ReadValueEx()
		{
			int num = 0;
			if (propPviValue.DataType == DataType.Unknown)
			{
				base.Requests |= Actions.GetValue;
				num = Read_FormatEX(propLinkId);
			}
			else
			{
				num = ReadRequest(Service.hPvi, propLinkId, AccessTypes.Data, 505u);
			}
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableValueRead, Service));
			}
			return num;
		}

		public int ReadValue(bool synchronous)
		{
			ArrayList changes = null;
			return ReadValue(synchronous, ref changes);
		}

		public int ReadValue(bool synchronous, ref ArrayList changes)
		{
			int num = 0;
			if (!synchronous)
			{
				num = 0;
				if (!propWaitingOnReadEvent)
				{
					propWaitingOnReadEvent = true;
					if (propPviValue.DataType == DataType.Unknown)
					{
						Read_FormatEX(propLinkId);
					}
					num = ReadRequest(Service.hPvi, propLinkId, AccessTypes.Data, 505u);
				}
			}
			else if (propPviValue.DataType == DataType.Unknown)
			{
				Read_FormatEX(propLinkId);
				num = 12012;
			}
			else
			{
				SyncReadData syncReadData = new SyncReadData(propPviValue.DataSize);
				num = Read(Service.hPvi, propLinkId, AccessTypes.Data, syncReadData);
				if (num == 0)
				{
					changes = new ArrayList(1);
					num = ConvertPviValue(syncReadData.PtrData, (uint)syncReadData.DataLength, ref changes);
					propChangedStructMembers = new string[changes.Count];
					for (int i = 0; i < changes.Count; i++)
					{
						propChangedStructMembers.SetValue(changes[i], i);
					}
					OnDataValidated(new VariableEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableValueRead, propChangedStructMembers));
				}
				syncReadData.FreeBuffers();
				syncReadData = null;
			}
			return num;
		}

		internal void ReadInternalValue()
		{
			int num = 0;
			num = ReadRequest(Service.hPvi, propLinkId, AccessTypes.Data, 2809u);
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableValueRead, Service));
			}
		}

		internal Variable GetMemberByOffset(int offset)
		{
			if (offset < 0)
			{
				return null;
			}
			int num = 0;
			Variable variable = null;
			foreach (Variable member in Members)
			{
				if (member.propOffset == offset && member.propPviValue.DataType != DataType.Structure)
				{
					return member;
				}
				if (member.propPviValue.DataType == DataType.Structure && member.propPviValue.ArrayLength * member.propPviValue.propTypeLength + member.propOffset > offset)
				{
					if (member.propPviValue.ArrayLength > 1)
					{
						num = (offset - member.propOffset) % member.propPviValue.propTypeLength;
						variable = member.GetMemberByOffset(num);
					}
					else
					{
						variable = member.GetMemberByOffset(offset - member.propOffset);
					}
					if (variable != null)
					{
						return variable;
					}
				}
				if (member.propPviValue.DataType != DataType.Structure)
				{
					if (offset > member.propOffset && offset < member.propOffset + member.propPviValue.TypeLength * member.propPviValue.ArrayLength)
					{
						return member;
					}
					if (offset < propOffset + propPviValue.propTypeLength * propPviValue.propArrayLength)
					{
						int num2 = offset % propPviValue.propTypeLength;
						if (num2 >= member.propOffset && num2 < member.propOffset + member.propPviValue.TypeLength * member.propPviValue.propArrayLength)
						{
							return member;
						}
					}
				}
			}
			return null;
		}

		private int WriteInitialValue()
		{
			int num = 0;
			if (DataType.String == propPviValue.DataType)
			{
				ResetWriteDataPtr(this, propPviValue.TypeLength, setZero: true);
				if (propPviValue.propDataSize > propPviValue.TypeLength)
				{
					Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propPviValue.TypeLength);
				}
				else
				{
					ResizePviDataPtr(propPviValue.DataSize);
					Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propPviValue.propDataSize);
				}
				Marshal.Copy(propWriteByteField, 0, pWriteData, propStrDataLen);
				propPviValue.isAssigned = false;
				num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Data, pWriteData, propStrDataLen, 556u);
			}
			else if (propPviValue.IsOfTypeArray && CastModes.PG2000String == (CastMode & CastModes.PG2000String))
			{
				if (!propPviValue.isAssigned)
				{
					ResetWriteDataPtr(this, propPviValue.DataSize, setZero: true);
					propWriteByteField = (byte[])propPviValue.propByteField.Clone();
					for (int i = 0; i < StructureMembers.Count; i++)
					{
						if (StructureMembers.IsVirtual)
						{
							Marshal.Copy(propPviValue.DataPtr, propWriteByteField, propPviValue.TypeLength * i, propPviValue.TypeLength);
							continue;
						}
						Variable variable = (Variable)StructureMembers[i];
						if (0 < variable.propPviValue.propDataSize)
						{
							variable.propPviValue.isAssigned = false;
							if (1 <= variable.propPviValue.ArrayLength && DataType.Structure != variable.Value.DataType)
							{
								Marshal.Copy(variable.propPviValue.DataPtr, propWriteByteField, variable.propOffset, variable.Value.propDataSize);
							}
						}
					}
					Marshal.Copy(propWriteByteField, 0, pWriteData, propStrDataLen);
					propPviValue.isAssigned = false;
					num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Data, pWriteData, propStrDataLen, 556u);
				}
				else
				{
					ResetWriteDataPtr(this, propPviValue.ArrayLength);
					if (propPviValue.propDataSize > propPviValue.ArrayLength)
					{
						Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propPviValue.ArrayLength);
					}
					else
					{
						Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propPviValue.propDataSize);
					}
					Marshal.Copy(propWriteByteField, 0, pWriteData, propStrDataLen);
					propPviValue.isAssigned = false;
					num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Data, pWriteData, propStrDataLen, 556u);
				}
			}
			else if (propPviValue.IsOfTypeArray || DataType.Structure == propPviValue.DataType)
			{
				ResetWriteDataPtr(this, propPviValue.DataSize);
				propWriteByteField = (byte[])propPviValue.propByteField.Clone();
				for (int i = 0; i < StructureMembers.Count; i++)
				{
					if (StructureMembers.IsVirtual)
					{
						Marshal.Copy(propPviValue.DataPtr, propWriteByteField, propPviValue.TypeLength * i, propPviValue.TypeLength);
						continue;
					}
					Variable variable = (Variable)StructureMembers[i];
					if (0 < variable.propPviValue.propDataSize)
					{
						variable.propPviValue.isAssigned = false;
						if (1 <= variable.propPviValue.ArrayLength && DataType.Structure != variable.propPviValue.DataType)
						{
							Marshal.Copy(variable.propPviValue.DataPtr, propWriteByteField, variable.propOffset, variable.propPviValue.propDataSize);
						}
					}
					variable.propPviValue.isAssigned = false;
				}
				Marshal.Copy(propWriteByteField, 0, pWriteData, propStrDataLen);
				propPviValue.isAssigned = false;
				num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Data, pWriteData, propStrDataLen, 556u);
			}
			else
			{
				num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Data, propPviValue.pData, propPviValue.propDataSize, 556u);
			}
			PviMarshal.FreeHGlobal(ref propPviValue.pData);
			if (propInternalByteField != null)
			{
				propPviValue.propByteField = propInternalByteField;
			}
			if (num != 0)
			{
				OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableValueWrite, Service));
			}
			return num;
		}

		private int WriteValueFromPtr(bool sync)
		{
			int num = 0;
			ResetWriteDataPtr(this, propPviValue.DataSize);
			propInternalByteField = (byte[])propPviValue.propByteField.Clone();
			propPviValue.isAssigned = false;
			if (sync)
			{
				return Write(Service.hPvi, base.LinkId, AccessTypes.Data, propPviValue.DataPtr, propPviValue.DataSize);
			}
			return WriteRequest(Service.hPvi, propLinkId, AccessTypes.Data, propPviValue.DataPtr, propPviValue.DataSize, 506u);
		}

		internal void UpdateAssignedData(IntPtr pD, int byteOffset, int dSize)
		{
			if (propPviValue.propByteField == null)
			{
				propPviValue.propByteField = new byte[propPviValue.DataSize];
			}
			if (IntPtr.Zero == propPviValue.pData)
			{
				propPviValue.pData = PviMarshal.AllocHGlobal(propPviValue.propByteField.Length);
				propPviValue.propHasOwnDataPtr = true;
			}
			Marshal.Copy(propPviValue.pData, propPviValue.propByteField, byteOffset, dSize);
		}

		internal int WriteValue(Array values, int offset)
		{
			int num = 0;
			if (Access.Read == Access)
			{
				return 12034;
			}
			ResetWriteDataPtr(PVRoot, PVRoot.propPviValue.DataSize);
			PVRoot.propInternalByteField = (byte[])PVRoot.propPviValue.propByteField.Clone();
			Marshal.Copy(PVRoot.propInternalByteField, 0, PVRoot.pWriteData, PVRoot.propStrDataLen);
			if (values.Length == 0)
			{
				return -2;
			}
			object value = values.GetValue(0);
			if (value is float)
			{
				if (0 < offset)
				{
					int num2 = values.Length * 4;
					IntPtr hMemory = PviMarshal.AllocHGlobal(num2);
					PVRoot.propWriteByteField = (byte[])PVRoot.propPviValue.propByteField.Clone();
					Marshal.Copy((float[])values, 0, hMemory, values.Length);
					Marshal.Copy(hMemory, PVRoot.propWriteByteField, offset, num2);
					Marshal.Copy(PVRoot.propWriteByteField, 0, PVRoot.pWriteData, PVRoot.propStrDataLen);
					PviMarshal.FreeHGlobal(ref hMemory);
				}
				else
				{
					Marshal.Copy((float[])values, 0, pWriteData, values.Length);
				}
			}
			else if (value is double)
			{
				if (0 < offset)
				{
					int num2 = values.Length * 8;
					IntPtr hMemory = PviMarshal.AllocHGlobal(num2);
					PVRoot.propWriteByteField = (byte[])PVRoot.propPviValue.propByteField.Clone();
					Marshal.Copy((double[])values, 0, hMemory, values.Length);
					Marshal.Copy(hMemory, PVRoot.propWriteByteField, offset, num2);
					Marshal.Copy(PVRoot.propWriteByteField, 0, PVRoot.pWriteData, PVRoot.propStrDataLen);
					PviMarshal.FreeHGlobal(ref hMemory);
				}
				else
				{
					Marshal.Copy((double[])values, 0, PVRoot.pWriteData, values.Length);
				}
			}
			else if (value is int)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Marshal.WriteInt32(PVRoot.pWriteData, offset + 4 * i, (int)values.GetValue(i));
				}
			}
			else if (value is short)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Marshal.WriteInt16(PVRoot.pWriteData, offset + 2 * i, (short)values.GetValue(i));
				}
			}
			else if (value is byte)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Marshal.WriteByte(PVRoot.pWriteData, offset + i, (byte)values.GetValue(i));
				}
			}
			else if (value is char)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Marshal.WriteByte(PVRoot.pWriteData, offset + i, PviMarshal.Convert.ToByte((char)values.GetValue(i)));
				}
			}
			else if (value is sbyte)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Marshal.WriteByte(PVRoot.pWriteData, offset + i, PviMarshal.Convert.ToByte((sbyte)values.GetValue(i)));
				}
			}
			else if (value is ushort)
			{
				for (int i = 0; i < values.Length; i++)
				{
					PviMarshal.WriteUInt16(PVRoot.pWriteData, offset + 2 * i, (ushort)values.GetValue(i));
				}
			}
			else if (value is short)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Marshal.WriteInt16(PVRoot.pWriteData, offset + 2 * i, (short)values.GetValue(i));
				}
			}
			else if (value is int)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Marshal.WriteInt32(PVRoot.pWriteData, offset + 4 * i, (int)values.GetValue(i));
				}
			}
			else if (value is uint)
			{
				for (int i = 0; i < values.Length; i++)
				{
					PviMarshal.WriteUInt32(PVRoot.pWriteData, offset + 4 * i, (uint)values.GetValue(i));
				}
			}
			else if (value is long)
			{
				for (int i = 0; i < values.Length; i++)
				{
					PviMarshal.WriteInt64(PVRoot.pWriteData, offset + 8 * i, (long)values.GetValue(i));
				}
			}
			else if (value is ulong)
			{
				for (int i = 0; i < values.Length; i++)
				{
					PviMarshal.WriteInt64(PVRoot.pWriteData, offset + 8 * i, (long)values.GetValue(i));
				}
			}
			else if (value is DateTime)
			{
				for (int i = 0; i < values.Length; i++)
				{
					PviMarshal.WriteUInt32(PVRoot.pWriteData, offset + 4 * i, Pvi.DateTimeToUInt32((DateTime)values.GetValue(i)));
				}
			}
			else if (value is TimeSpan)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Marshal.WriteInt32(PVRoot.pWriteData, offset + 4 * i, (int)(((TimeSpan)values.GetValue(i)).Ticks / 10000));
				}
			}
			else if (value is bool)
			{
				for (int i = 0; i < values.Length; i++)
				{
					byte val = 1;
					if (!(bool)values.GetValue(i))
					{
						val = 0;
					}
					Marshal.WriteByte(PVRoot.pWriteData, offset + i, val);
				}
			}
			else if (value is string)
			{
				for (int i = 0; i < values.Length; i++)
				{
					string text = (string)values.GetValue(i);
					char[] array = null;
					array = new char[text.Length];
					array = text.ToCharArray(0, text.Length);
					for (int j = 0; j < array.Length; j++)
					{
						Marshal.WriteByte(PVRoot.pWriteData, offset + propPviValue.propTypeLength * i + j, (byte)array[j]);
					}
					for (int j = array.Length; j < propPviValue.propTypeLength; j++)
					{
						Marshal.WriteByte(PVRoot.pWriteData, offset + propPviValue.propTypeLength * i + j, 0);
					}
				}
			}
			propPviValue.isAssigned = false;
			if (!PVRoot.propActive)
			{
				Marshal.Copy(PVRoot.pWriteData, PVRoot.propWriteByteField, 0, PVRoot.propStrDataLen);
			}
			return WriteRequest(Service.hPvi, PVRoot.LinkId, AccessTypes.Data, PVRoot.pWriteData, PVRoot.propStrDataLen, 506u);
		}

		internal int WriteValue(Array values)
		{
			return WriteValue(values, 0);
		}

		public int WriteValue()
		{
			return SendWriteValue(sync: false);
		}

		private int ValueCBWriteRequest(IntPtr pData, int dataLen, uint msgID, bool sync)
		{
			if (propLinkId == 0)
			{
				return 2;
			}
			if (sync)
			{
				return Write(Service.hPvi, base.LinkId, AccessTypes.Data, pData, dataLen);
			}
			return WriteRequest(Service.hPvi, propLinkId, AccessTypes.Data, pData, dataLen, 506u);
		}

		private int SendWriteValue(bool sync)
		{
			int num = 0;
			if (Access.Read == Access)
			{
				num = 12034;
			}
			else
			{
				if (propPviValue.DataType == DataType.Unknown)
				{
					return 12036;
				}
				if (propForceValue)
				{
					num = WriteValueForced(force: true);
				}
				else if (!propExpandMembers || (DataType.Structure != propPviValue.DataType && propPviValue.IsOfTypeArray))
				{
					num = WriteValueFromPtr(sync);
				}
				else if (DataType.String == propPviValue.DataType)
				{
					ResetWriteDataPtr(this, propPviValue.TypeLength, setZero: true);
					if (propPviValue.propDataSize > propStrDataLen)
					{
						Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propPviValue.TypeLength);
					}
					else
					{
						ResizePviDataPtr(propPviValue.DataSize);
						Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propPviValue.propDataSize);
					}
					Marshal.Copy(propWriteByteField, 0, pWriteData, propStrDataLen);
					propPviValue.isAssigned = false;
					num = ValueCBWriteRequest(pWriteData, propStrDataLen, _internId, sync);
				}
				else if (propPviValue.IsOfTypeArray && CastModes.PG2000String == (CastMode & CastModes.PG2000String))
				{
					if (!propPviValue.isAssigned)
					{
						ResetWriteDataPtr(this, propPviValue.DataSize, setZero: true);
						propInternalByteField = (byte[])propPviValue.propByteField.Clone();
						for (int i = 0; i < StructureMembers.Count; i++)
						{
							if (StructureMembers.IsVirtual)
							{
								Marshal.Copy(propPviValue.DataPtr, propWriteByteField, propPviValue.TypeLength * i, propPviValue.TypeLength);
								continue;
							}
							Variable variable = (Variable)StructureMembers[i];
							if (0 < variable.Value.propDataSize)
							{
								variable.Value.isAssigned = false;
								if (1 <= variable.Value.ArrayLength && DataType.Structure != variable.Value.DataType)
								{
									Marshal.Copy(variable.Value.DataPtr, propWriteByteField, variable.propOffset, variable.Value.propDataSize);
									variable.Value.propDataSize = 0;
								}
							}
							variable.Value.isAssigned = false;
						}
						Marshal.Copy(propWriteByteField, 0, pWriteData, propStrDataLen);
						propPviValue.isAssigned = false;
						num = ValueCBWriteRequest(pWriteData, propStrDataLen, _internId, sync);
					}
					else
					{
						ResetWriteDataPtr(this, propPviValue.ArrayLength);
						if (propPviValue.propDataSize > propStrDataLen)
						{
							Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propStrDataLen);
						}
						else
						{
							Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propPviValue.propDataSize);
						}
						Marshal.Copy(propWriteByteField, 0, pWriteData, propStrDataLen);
						propPviValue.isAssigned = false;
						num = ValueCBWriteRequest(pWriteData, propStrDataLen, _internId, sync);
					}
				}
				else if (propPviValue.IsOfTypeArray || DataType.Structure == propPviValue.DataType)
				{
					ResetWriteDataPtr(this, propPviValue.DataSize);
					propInternalByteField = (byte[])propPviValue.propByteField.Clone();
					propWriteByteField = (byte[])propPviValue.propByteField.Clone();
					for (int i = 0; i < StructureMembers.Count; i++)
					{
						if (StructureMembers.IsVirtual)
						{
							Marshal.Copy(propPviValue.DataPtr, propWriteByteField, propPviValue.TypeLength * i, propPviValue.TypeLength);
							continue;
						}
						Variable variable = (Variable)StructureMembers[i];
						if (((variable.Value.isAssigned && (0 < variable.Value.propDataSize || 1 < variable.Value.ArrayLength)) || (Access.Write == Access && 0 < variable.Value.propDataSize)) && DataType.Structure != variable.Value.DataType && 0 < variable.Value.DataSize)
						{
							if (IntPtr.Zero != variable.Value.DataPtr)
							{
								Marshal.Copy(variable.Value.DataPtr, propWriteByteField, variable.propOffset, variable.Value.DataSize);
							}
							else if (IntPtr.Zero != Value.DataPtr)
							{
								Marshal.Copy(Value.DataPtr, propWriteByteField, variable.propOffset, variable.Value.DataSize);
							}
							else
							{
								for (int j = variable.propOffset; j < variable.propOffset + variable.Value.DataSize && j < propWriteByteField.GetLength(0); j++)
								{
									propWriteByteField[j] = 0;
								}
							}
							if (Access.Write != Access)
							{
								variable.Value.propDataSize = 0;
							}
						}
						variable.Value.isAssigned = false;
					}
					Marshal.Copy(propWriteByteField, 0, pWriteData, propStrDataLen);
					propPviValue.isAssigned = false;
					num = ValueCBWriteRequest(pWriteData, propStrDataLen, _internId, sync);
				}
				else
				{
					num = ValueCBWriteRequest(propPviValue.pData, propPviValue.propDataSize, _internId, sync);
				}
			}
			if (num > 0)
			{
				OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableValueWrite, Service));
			}
			return num;
		}

		public int WriteValue(bool synchronous)
		{
			int num = 0;
			if (Access.Read == Access)
			{
				return 12034;
			}
			return SendWriteValue(synchronous);
		}

		internal int WriteValueForced(bool force)
		{
			int num = 0;
			if (force)
			{
				num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.ForceOn, propPviValue.pData, propPviValue.propDataSize, 508u);
				if (propInternalByteField != null)
				{
					propPviValue.propByteField = propInternalByteField;
				}
			}
			else
			{
				num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.ForceOff, IntPtr.Zero, 0, 508u);
				if (propInternalByteField != null)
				{
					propPviValue.propByteField = propInternalByteField;
				}
			}
			return num;
		}

		internal unsafe int SetArrayIndexValue(int index, Value value)
		{
			ResizePviDataPtr(propPviValue.propArrayLength * propPviValue.propTypeLength);
			Marshal.Copy(propPviValue.propByteField, 0, propPviValue.pData, propPviValue.propDataSize);
			byte* ptr = (byte*)propPviValue.pData.ToPointer() + (long)index * (long)propPviValue.propTypeLength;
			switch (propPviValue.propDataType)
			{
			case DataType.Boolean:
				*ptr = (((bool)value) ? ((byte)1) : ((byte)0));
				break;
			case DataType.SByte:
				*ptr = (byte)(sbyte)value;
				break;
			case DataType.Int16:
				*(short*)ptr = value;
				break;
			case DataType.Int32:
				*(int*)ptr = value;
				break;
			case DataType.Int64:
				*(long*)ptr = value;
				break;
			case DataType.UInt8:
				*ptr = value;
				break;
			case DataType.Byte:
				*ptr = value;
				break;
			case DataType.UInt16:
				*(ushort*)ptr = value;
				break;
			case DataType.WORD:
				*(ushort*)ptr = value;
				break;
			case DataType.UInt32:
				*(uint*)ptr = value;
				break;
			case DataType.DWORD:
				*(uint*)ptr = value;
				break;
			case DataType.UInt64:
				*(ulong*)ptr = value;
				break;
			case DataType.Single:
				*(float*)ptr = value;
				break;
			case DataType.Double:
				*(double*)ptr = value;
				break;
			case DataType.TimeSpan:
				*(uint*)ptr = value;
				break;
			case DataType.TimeOfDay:
				*(uint*)ptr = value;
				break;
			case DataType.DateTime:
				*(uint*)ptr = value;
				break;
			case DataType.TOD:
				*(uint*)ptr = value;
				break;
			case DataType.DT:
				*(uint*)ptr = value;
				break;
			case DataType.Date:
				*(uint*)ptr = value;
				break;
			case DataType.String:
			{
				string text = value.ToString();
				for (int i = 0; i < propPviValue.propTypeLength - 1; i++)
				{
					if (text.Length > i)
					{
						ptr[i] = System.Convert.ToByte(text[i]);
					}
					else
					{
						ptr[i] = 0;
					}
				}
				*(ptr + propPviValue.propTypeLength - 1) = 0;
				break;
			}
			default:
				return 0;
			}
			if (WriteValueAutomatic)
			{
				return WriteValue();
			}
			return 0;
		}

		internal void InternalSetValue(IntPtr pData, uint dataLen, int offset)
		{
			InternalSetValue(pData, dataLen, offset, propPviValue.propTypeLength);
		}

		internal void InternalSetValue(IntPtr pData, uint dataLen, int offset, int len)
		{
			if (offset >= dataLen)
			{
				return;
			}
			switch (propPviValue.propDataType)
			{
			case (DataType)6:
			case (DataType)11:
			case DataType.Structure:
			case DataType.Data:
			case DataType.LWORD:
				break;
			case DataType.Boolean:
			{
				byte b = Marshal.ReadByte(pData, offset);
				propPviValue.propObjValue = true;
				if (b == 0)
				{
					propPviValue.propObjValue = false;
				}
				break;
			}
			case DataType.SByte:
			{
				byte b = Marshal.ReadByte(pData, offset);
				propPviValue.propObjValue = (sbyte)b;
				break;
			}
			case DataType.Int16:
				propPviValue.propObjValue = Marshal.ReadInt16(pData, offset);
				break;
			case DataType.Int32:
				propPviValue.propObjValue = Marshal.ReadInt32(pData, offset);
				break;
			case DataType.Int64:
				propPviValue.propObjValue = PviMarshal.ReadInt64(pData, offset);
				break;
			case DataType.Byte:
			case DataType.UInt8:
				propPviValue.propObjValue = Marshal.ReadByte(pData, offset);
				break;
			case DataType.UInt16:
			case DataType.WORD:
				propPviValue.propObjValue = (ushort)Marshal.ReadInt16(pData, offset);
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				propPviValue.propObjValue = (uint)Marshal.ReadInt32(pData, offset);
				break;
			case DataType.UInt64:
				propPviValue.propObjValue = (ulong)PviMarshal.ReadInt64(pData, offset);
				break;
			case DataType.Single:
			{
				ResetReadDataPtr(4);
				for (int i = 0; i < 4; i++)
				{
					propReadByteField[i] = Marshal.ReadByte(pData, offset + i);
				}
				Marshal.Copy(propReadByteField, 0, pReadData, propStrDataLen);
				float[] array2 = new float[1];
				Marshal.Copy(pReadData, array2, 0, 1);
				propPviValue.propObjValue = array2[0];
				break;
			}
			case DataType.Double:
			{
				ResetReadDataPtr(8);
				for (int i = 0; i < 8; i++)
				{
					propReadByteField[i] = Marshal.ReadByte(pData, offset + i);
				}
				Marshal.Copy(propReadByteField, 0, pReadData, propStrDataLen);
				double[] array = new double[1];
				Marshal.Copy(pReadData, array, 0, 1);
				propPviValue.propObjValue = array[0];
				break;
			}
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
			{
				uint num = (uint)Marshal.ReadInt32(pData, offset);
				propPviValue.propObjValue = new TimeSpan((long)num * 10000L);
				break;
			}
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				new DateTime(55L);
				propPviValue.propObjValue = Pvi.UInt32ToDateTime((uint)Marshal.ReadInt32(pData, offset));
				break;
			case DataType.String:
			{
				string text2 = "";
				ResetReadDataPtr(len);
				for (int i = 0; i < len && dataLen >= offset + i; i++)
				{
					propReadByteField[i] = Marshal.ReadByte(pData, offset + i);
				}
				text2 = PviMarshal.ToAnsiString(propReadByteField);
				propPviValue.propObjValue = text2;
				break;
			}
			case DataType.WString:
			{
				string text = "";
				ResetReadDataPtr(len);
				for (int i = 0; i < len && dataLen >= offset + i; i++)
				{
					propReadByteField[i] = Marshal.ReadByte(pData, offset + i);
				}
				text = PviMarshal.ToWString(propReadByteField, 0, len);
				propPviValue.propObjValue = text;
				break;
			}
			}
		}

		private string GetOldStyleValueName(string varName)
		{
			int num = varName.IndexOf('[', 1);
			int num2 = varName.IndexOf('.', 0);
			string result = varName;
			if (-1 != num)
			{
				result = varName.Substring(num);
				if (-1 != num2 && num > num2)
				{
					result = varName.Substring(num2 + 1);
				}
			}
			else if (num2 == 0)
			{
				result = varName.Substring(num2 + 1);
			}
			return result;
		}

		private Value GetValueUseOldStyleName(string varName)
		{
			string oldStyleValueName = GetOldStyleValueName(varName);
			if (mapNameToMember != null && mapNameToMember.ContainsKey(oldStyleValueName))
			{
				return mapNameToMember[oldStyleValueName].Value;
			}
			if (!propExpandMembers)
			{
				if (-1 == oldStyleValueName.IndexOf('['))
				{
					if (-1 == oldStyleValueName.IndexOf(','))
					{
						return propPviValue[System.Convert.ToInt32(oldStyleValueName)];
					}
					string[] array = oldStyleValueName.Substring(0, oldStyleValueName.Length - 1).Split(',');
					if (1 < array.Length)
					{
						int[] array2 = new int[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array2.SetValue(System.Convert.ToInt32(array.GetValue(i).ToString()), i);
						}
						return propPviValue[array2];
					}
					return null;
				}
				if (oldStyleValueName.IndexOf('[') == 0)
				{
					string[] array = oldStyleValueName.Substring(1, oldStyleValueName.IndexOf(']') - 1).Split(',');
					if (1 < array.Length)
					{
						int[] array2 = new int[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array2.SetValue(System.Convert.ToInt32(array.GetValue(i).ToString()), i);
						}
						return propPviValue[array2];
					}
					return propPviValue[System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.IndexOf(']') - 1))];
				}
				return new Value(-1);
			}
			if (-1 == oldStyleValueName.IndexOf('['))
			{
				return propPviValue[System.Convert.ToInt32(oldStyleValueName)];
			}
			if (oldStyleValueName.IndexOf('[') == 0)
			{
				string[] array = oldStyleValueName.Substring(1, oldStyleValueName.Length - 2).Split(',');
				if (1 < array.Length)
				{
					int[] array2 = new int[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array2.SetValue(System.Convert.ToInt32(array.GetValue(i).ToString()), i);
					}
					if (Members != null)
					{
						int num = CalculateIndex(array2, propPviValue);
						if (Members.ContainsKey("[" + num.ToString() + "]"))
						{
							return Members["[" + num.ToString() + "]"].Value;
						}
					}
					return propPviValue[array2];
				}
				return propPviValue[System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.Length - 2))];
			}
			return new Value(-1);
		}

		internal Value GetStructureMemberValue(int index)
		{
			return GetStructureMemberValue("[" + index.ToString() + "]");
		}

		private int GetFlatIndex(string varName, ArrayDimensionArray aDims, ref string preVarName, ref string remVarName)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			bool flag = false;
			int num5 = 0;
			int num6 = 0;
			preVarName = "";
			remVarName = "";
			string[] array = varName.Split(']');
			for (num = 0; num < array.Length; num++)
			{
				string text = array.GetValue(num).ToString();
				if (flag)
				{
					remVarName += text;
					continue;
				}
				if (-1 == text.IndexOf(','))
				{
					preVarName = preVarName + text + "]";
					continue;
				}
				flag = true;
				string[] array2 = text.Split(',');
				int[] array3 = new int[array2.Length];
				for (num2 = 0; num2 < array2.Length; num2++)
				{
					text = array2.GetValue(num2).ToString();
					text = text.Replace('[', ' ');
					text = text.Replace(',', ' ');
					text = text.Trim();
					array3[num2] = System.Convert.ToInt32(text);
				}
				for (num2 = 0; num2 < array3.Length - 1; num2++)
				{
					num5 = (int)array3.GetValue(num2);
					if (num2 < aDims.Count)
					{
						num6 = 1;
						for (num3 = num2 + 1; num3 < aDims.Count; num3++)
						{
							num6 *= aDims[num3].NumOfElements;
						}
					}
					num4 += (num5 - aDims[num2].StartIndex) * num6;
				}
				num5 = (int)array3.GetValue(num2);
				num4 += num5;
				if (num2 < aDims.Count)
				{
					num4 -= aDims[num2].StartIndex;
				}
			}
			return num4;
		}

		internal Value GetStructureMemberValue(string varName)
		{
			int num = 0;
			string remVarName = "";
			string preVarName = "";
			string text = "";
			Variable variable = this;
			if (mapNameToMember != null)
			{
				if (mapNameToMember.ContainsKey(varName))
				{
					return mapNameToMember[varName].Value;
				}
				if (propPviValue.ArrayDimensions != null && -1 != varName.IndexOf(','))
				{
					variable = null;
					num = GetFlatIndex(varName, propPviValue.ArrayDimensions, ref preVarName, ref remVarName);
					string text2 = preVarName + "[" + num.ToString() + "]" + remVarName;
					if (-1 != text2.IndexOf(','))
					{
						preVarName = text2.Substring(0, 1 + text2.IndexOf(']', text2.IndexOf(',')));
						text = preVarName.Substring(0, preVarName.LastIndexOf('['));
						variable = mapNameToMember[text];
						if (variable != null)
						{
							remVarName = text2.Substring(text.Length);
							return variable.GetStructureMemberValue(remVarName);
						}
					}
					else
					{
						variable = mapNameToMember[text2];
					}
					return variable?.Value;
				}
				int num2 = varName.LastIndexOf(']');
				if (variable.Value.IsOfTypeArray)
				{
					return GetValueUseOldStyleName(varName);
				}
				if (num2 + 1 == varName.Length)
				{
					num2 = varName.LastIndexOf('[');
					string text3 = varName.Substring(0, num2);
					if (mapNameToMember.ContainsKey(text3))
					{
						variable = mapNameToMember[text3];
						return variable.GetStructureMemberValue(varName.Substring(num2));
					}
				}
				return null;
			}
			if (Members != null)
			{
				variable = Members[varName];
			}
			if (variable == null)
			{
				variable = this;
			}
			if (!variable.Value.IsOfTypeArray)
			{
				return variable.Value;
			}
			return GetValueUseOldStyleName(varName);
		}

		private void ValidateDataPtr(int size)
		{
			if (IntPtr.Zero == propPviValue.pData)
			{
				propPviValue.pData = PviMarshal.AllocHGlobal(size);
				propPviValue.propHasOwnDataPtr = true;
				propPviValue.propDataSize = size;
			}
		}

		private int SetStructBoolValue(Value newValue, int byteOffset)
		{
			ValidateDataPtr(1);
			if (false == newValue || newValue.ToInt32(null) == 0)
			{
				Marshal.WriteByte(propPviValue.pData, byteOffset, 0);
				propPviValue.isAssigned = true;
			}
			else
			{
				Marshal.WriteByte(propPviValue.pData, byteOffset, 1);
				propPviValue.isAssigned = true;
			}
			return 0;
		}

		private int SetStructUnsignedInt8Value(Value newValue, int byteOffset)
		{
			byte val = 0;
			ValidateDataPtr(1);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					val = System.Convert.ToByte(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				val = newValue;
			}
			Marshal.WriteByte(propPviValue.pData, byteOffset, val);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructInt8Value(Value newValue, int byteOffset)
		{
			sbyte b = 0;
			ValidateDataPtr(1);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					b = System.Convert.ToSByte(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				b = newValue;
			}
			Marshal.WriteByte(propPviValue.pData, byteOffset, (byte)b);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructInt16Value(Value newValue, int byteOffset)
		{
			short val = 0;
			ValidateDataPtr(2);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					val = System.Convert.ToInt16(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				val = newValue;
			}
			Marshal.WriteInt16(propPviValue.pData, byteOffset, val);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructUnsignedInt16Value(Value newValue, int byteOffset)
		{
			ushort num = 0;
			ValidateDataPtr(2);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					num = System.Convert.ToUInt16(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				num = newValue;
			}
			Marshal.WriteInt16(propPviValue.pData, byteOffset, (short)num);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructInt32Value(Value newValue, int byteOffset)
		{
			int val = 0;
			ValidateDataPtr(4);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					val = System.Convert.ToInt32(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				val = newValue;
			}
			Marshal.WriteInt32(propPviValue.pData, byteOffset, val);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructUnsignedInt32Value(Value newValue, int byteOffset)
		{
			uint val = 0u;
			ValidateDataPtr(4);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					val = System.Convert.ToUInt32(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				val = newValue;
			}
			Marshal.WriteInt32(propPviValue.pData, byteOffset, (int)val);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructInt64Value(Value newValue, int byteOffset)
		{
			long val = 0L;
			ValidateDataPtr(8);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					val = System.Convert.ToInt64(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				val = newValue;
			}
			Marshal.WriteInt64(propPviValue.pData, byteOffset, val);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructUnsignedInt64Value(Value newValue, int byteOffset)
		{
			ulong val = 0uL;
			ValidateDataPtr(8);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					val = System.Convert.ToUInt64(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				val = newValue;
			}
			Marshal.WriteInt64(propPviValue.pData, byteOffset, (long)val);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructFloating32Value(Value newValue, int byteOffset)
		{
			float val = 0f;
			ValidateDataPtr(4);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					val = System.Convert.ToSingle(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				val = newValue;
			}
			PviMarshal.WriteSingle(propPviValue.pData, byteOffset, val);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStructFloating64Value(Value newValue, int byteOffset)
		{
			double val = 0.0;
			ValidateDataPtr(8);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					val = System.Convert.ToDouble(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
				}
			}
			else
			{
				val = (float)newValue;
			}
			PviMarshal.WriteDouble(propPviValue.pData, byteOffset, val);
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetStringValue(Value newValue, int byteOffset)
		{
			ValidateDataPtr(propPviValue.DataSize);
			propPviValue.propDataSize = propPviValue.DataSize;
			if (null != newValue)
			{
				string text = newValue.ToString();
				for (int i = 0; i < propPviValue.propDataSize; i++)
				{
					if (i < text.Length)
					{
						Marshal.WriteByte(propPviValue.pData, byteOffset + i, (byte)text[i]);
					}
					else
					{
						Marshal.WriteByte(propPviValue.pData, byteOffset + i, 0);
					}
				}
			}
			else
			{
				for (int i = 0; i < propPviValue.propDataSize; i++)
				{
					Marshal.WriteByte(propPviValue.pData, byteOffset + i, 0);
				}
			}
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetTimeValue(Value newValue, int byteOffset)
		{
			ValidateDataPtr(4);
			if (DataType.DateTime == newValue.DataType)
			{
				Marshal.WriteInt32(propPviValue.pData, byteOffset, Pvi.GetTimeSpanInt32((DateTime)newValue.propObjValue));
			}
			else if (DataType.TimeSpan == newValue.DataType)
			{
				Marshal.WriteInt32(propPviValue.pData, byteOffset, Pvi.GetTimeSpanInt32((TimeSpan)newValue.propObjValue));
			}
			else
			{
				Marshal.WriteInt32(propPviValue.pData, byteOffset, (int)newValue / 10000);
			}
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetDateValue(Value newValue, int byteOffset)
		{
			ValidateDataPtr(4);
			if (DataType.DateTime == newValue.DataType)
			{
				Marshal.WriteInt32(propPviValue.pData, byteOffset, (int)Pvi.DateTimeToUInt32((DateTime)newValue.propObjValue));
			}
			else if (DataType.TimeSpan == newValue.DataType)
			{
				Marshal.WriteInt32(propPviValue.pData, byteOffset, (int)Pvi.DateTimeToUInt32(new DateTime(((TimeSpan)newValue.propObjValue).Ticks)));
			}
			else
			{
				Marshal.WriteInt32(propPviValue.pData, byteOffset, newValue);
			}
			propPviValue.isAssigned = true;
			return 0;
		}

		private int SetWStringValue(Value newValue, int byteOffset)
		{
			ValidateDataPtr(propPviValue.DataSize);
			propPviValue.propDataSize = propPviValue.DataSize;
			if (null != newValue)
			{
				byte[] array;
				if (DataType.WString == newValue.DataType)
				{
					array = new byte[newValue.DataSize];
					Marshal.Copy(newValue.pData, array, 0, newValue.DataSize);
				}
				else
				{
					string text = newValue.ToString();
					array = new byte[text.Length * 2 + 2];
					int num = 0;
					for (int i = 0; i < text.Length; i++)
					{
						array.SetValue((byte)text[i], num);
						num++;
						array.SetValue((byte)0, num);
						num++;
					}
				}
				for (int i = 0; i < propPviValue.propDataSize; i++)
				{
					if (i < array.Length)
					{
						Marshal.WriteByte(propPviValue.pData, byteOffset + i, (byte)array.GetValue(i));
					}
					else
					{
						Marshal.WriteByte(propPviValue.pData, byteOffset + i, 0);
					}
				}
			}
			else
			{
				for (int i = 0; i < propPviValue.propDataSize; i++)
				{
					Marshal.WriteByte(propPviValue.pData, byteOffset + i, 0);
				}
			}
			propPviValue.isAssigned = true;
			return 0;
		}

		internal int SetStructValue(Variable owner, Value newValue)
		{
			int result = -1;
			int byteOffset = 0;
			switch (propPviValue.DataType)
			{
			case DataType.Boolean:
				result = SetStructBoolValue(newValue, byteOffset);
				break;
			case DataType.Byte:
			case DataType.UInt8:
				result = SetStructUnsignedInt8Value(newValue, byteOffset);
				break;
			case DataType.SByte:
				result = SetStructInt8Value(newValue, byteOffset);
				break;
			case DataType.Int16:
				result = SetStructInt16Value(newValue, byteOffset);
				break;
			case DataType.UInt16:
			case DataType.WORD:
				result = SetStructUnsignedInt16Value(newValue, byteOffset);
				break;
			case DataType.Int32:
				result = SetStructInt32Value(newValue, byteOffset);
				break;
			case DataType.Int64:
				result = SetStructInt64Value(newValue, byteOffset);
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				result = SetStructUnsignedInt32Value(newValue, byteOffset);
				break;
			case DataType.UInt64:
				result = SetStructUnsignedInt64Value(newValue, byteOffset);
				break;
			case DataType.Single:
				result = SetStructFloating32Value(newValue, byteOffset);
				break;
			case DataType.Double:
				result = SetStructFloating64Value(newValue, byteOffset);
				break;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				result = SetTimeValue(newValue, byteOffset);
				break;
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				result = SetDateValue(newValue, byteOffset);
				break;
			case DataType.String:
				byteOffset = propOffset;
				result = SetStringValue(newValue, byteOffset);
				break;
			case DataType.WString:
				byteOffset = propOffset;
				result = SetWStringValue(newValue, byteOffset);
				break;
			}
			return result;
		}

		protected void UpdatedStructValue()
		{
			propPviValue.propDataSize = propPviValue.propByteField.Length;
			ResizePviDataPtr(propPviValue.propByteField.Length);
			Marshal.Copy(propPviValue.propByteField, 0, propPviValue.pData, propPviValue.propByteField.Length);
		}

		protected void UpdatingStructValue()
		{
			propPviValue.propDataSize = propPviValue.propByteField.Length;
			propInternalByteField = (byte[])propPviValue.propByteField.Clone();
		}

		internal static int CalculateByteOffset(int[] indices, Value pPviValue)
		{
			int num = CalculateIndex(indices, pPviValue);
			return num * pPviValue.propTypeLength;
		}

		internal static int CalculateIndex(int[] indices, Value pPviValue)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (pPviValue.propDimensions != null && indices.Length <= pPviValue.propDimensions.Count)
			{
				int i;
				for (i = 0; i < indices.Length - 1; i++)
				{
					num = indices[i] - pPviValue.propDimensions[i].StartIndex;
					num2 = 1;
					for (int j = i + 1; j < pPviValue.propDimensions.Count; j++)
					{
						num2 = pPviValue.propDimensions[j].NumOfElements * num2;
					}
					num3 += num * num2;
				}
				num = ((indices.Length != pPviValue.propDimensions.Count) ? indices[i] : (indices[i] - pPviValue.propDimensions[i].StartIndex));
				num3 += num;
				if (pPviValue.ArrayLength <= num)
				{
					num3 = indices[0];
				}
			}
			else
			{
				num = indices[0] - pPviValue.ArrayMinIndex;
				for (int i = 1; i < indices.Length; i++)
				{
					num += indices[i];
				}
				if (pPviValue.ArrayLength <= num)
				{
					num = indices[0];
				}
				num3 = num;
			}
			return num3;
		}

		internal int SetStructureMemberValue(Value value, params int[] indices)
		{
			int result = -1;
			string text = "";
			if (Access.Read == Access)
			{
				if (WriteValueAutomatic)
				{
					for (int i = 0; i < indices.Length; i++)
					{
						text = text + "[" + (System.Convert.ToInt32(indices.GetValue(i).ToString()) - propPviValue.ArrayMinIndex).ToString() + "]";
					}
					OnValueWritten(new PviEventArgs(base.Name + text, base.Address + text, 12034, Service.Language, Action.VariableValueWrite, Service));
				}
				return 12034;
			}
			if (1 < indices.Length && (propPviValue.ArrayDimensions == null || propPviValue.ArrayDimensions.Count == 0))
			{
				for (int i = 0; i < indices.Length; i++)
				{
					Value[System.Convert.ToInt32(indices.GetValue(i).ToString())].Assign(value.ToSystemDataTypeValue(null));
				}
				if (WriteValueAutomatic)
				{
					result = WriteValue();
				}
				return result;
			}
			if (IntPtr.Zero == propPviValue.pData)
			{
				propPviValue.pData = PviMarshal.AllocHGlobal(Value.DataSize);
				propPviValue.propHasOwnDataPtr = true;
			}
			if (!propPviValue.IsOfTypeArray || (propPviValue.propArryOne && Value.ArrayLength == value.ArrayLength))
			{
				Value = value;
				return 0;
			}
			int num = CalculateByteOffset(indices, propPviValue);
			switch (propPviValue.DataType)
			{
			case DataType.Boolean:
				if (!(bool)value)
				{
					Marshal.WriteByte(propPviValue.pData, num, 0);
				}
				else
				{
					Marshal.WriteByte(propPviValue.pData, num, 1);
				}
				break;
			case DataType.SByte:
				PviMarshal.WriteSByte(propPviValue.pData, num, PviMarshal.toSByte(value));
				break;
			case DataType.Byte:
			case DataType.UInt8:
				Marshal.WriteByte(propPviValue.pData, num, PviMarshal.toByte(value));
				break;
			case DataType.Int16:
				Marshal.WriteInt16(propPviValue.pData, num, PviMarshal.toInt16(value));
				break;
			case DataType.UInt16:
			case DataType.WORD:
				PviMarshal.WriteUInt16(propPviValue.pData, num, PviMarshal.toUInt16(value));
				break;
			case DataType.Int32:
				Marshal.WriteInt32(propPviValue.pData, num, PviMarshal.toInt32(value));
				break;
			case DataType.Int64:
				PviMarshal.WriteInt64(propPviValue.pData, num, PviMarshal.toInt64(value));
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				PviMarshal.WriteUInt32(propPviValue.pData, num, PviMarshal.toUInt32(value));
				break;
			case DataType.UInt64:
				PviMarshal.WriteUInt64(propPviValue.pData, num, PviMarshal.toUInt64(value));
				break;
			case DataType.Single:
				Service.cpyFltToBuffer(value);
				Marshal.WriteByte(propPviValue.pData, num, Service.ByteBuffer[0]);
				Marshal.WriteByte(propPviValue.pData, num + 1, Service.ByteBuffer[1]);
				Marshal.WriteByte(propPviValue.pData, num + 2, Service.ByteBuffer[2]);
				Marshal.WriteByte(propPviValue.pData, num + 3, Service.ByteBuffer[3]);
				break;
			case DataType.Double:
				Service.cpyDblToBuffer(value);
				Marshal.WriteByte(propPviValue.pData, num, Service.ByteBuffer[0]);
				Marshal.WriteByte(propPviValue.pData, num + 1, Service.ByteBuffer[1]);
				Marshal.WriteByte(propPviValue.pData, num + 2, Service.ByteBuffer[2]);
				Marshal.WriteByte(propPviValue.pData, num + 3, Service.ByteBuffer[3]);
				Marshal.WriteByte(propPviValue.pData, num + 4, Service.ByteBuffer[4]);
				Marshal.WriteByte(propPviValue.pData, num + 5, Service.ByteBuffer[5]);
				Marshal.WriteByte(propPviValue.pData, num + 6, Service.ByteBuffer[6]);
				Marshal.WriteByte(propPviValue.pData, num + 7, Service.ByteBuffer[7]);
				break;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				Marshal.WriteInt32(propPviValue.pData, num, Pvi.GetTimeSpanInt32(value));
				break;
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				PviMarshal.WriteUInt32(propPviValue.pData, num, Pvi.GetDateTimeUInt32(value));
				break;
			case DataType.String:
				for (int i = 0; i < propPviValue.propTypeLength; i++)
				{
					Marshal.WriteByte(propPviValue.pData, num + i, 0);
					if (i < value.TypeLength)
					{
						byte val = Marshal.ReadByte(value.pData, i);
						Marshal.WriteByte(propPviValue.pData, num + i, val);
					}
				}
				break;
			case DataType.WString:
			{
				Value value2;
				if (DataType.WString == value.DataType)
				{
					value2 = value;
				}
				else
				{
					value2 = new Value();
					value2.propDataType = DataType.WString;
					value2.propTypeLength = value.ToString().Length * 2;
					value2.Assign(value.ToString());
				}
				for (int i = 0; i < propPviValue.propTypeLength; i++)
				{
					Marshal.WriteByte(propPviValue.pData, num + i, 0);
					if (i < value2.TypeLength)
					{
						byte val = Marshal.ReadByte(value2.pData, i);
						Marshal.WriteByte(propPviValue.pData, num + i, val);
					}
				}
				break;
			}
			}
			if (WriteValueAutomatic)
			{
				result = WriteValue();
			}
			else
			{
				Value.isAssigned = true;
			}
			return result;
		}

		internal int SetStructureMemberValue(string varName, Value value)
		{
			int result = 0;
			if (Access.Read == Access)
			{
				if (WriteValueAutomatic)
				{
					if (varName.IndexOf("[") == 0)
					{
						OnValueWritten(new PviEventArgs(base.Name + "." + varName, base.Address + "." + varName, 12034, Service.Language, Action.VariableValueWrite, Service));
					}
					else
					{
						OnValueWritten(new PviEventArgs(base.Name + varName, base.Address + varName, 12034, Service.Language, Action.VariableValueWrite, Service));
					}
				}
				return 12034;
			}
			if (IntPtr.Zero == propPviValue.pData)
			{
				propPviValue.pData = PviMarshal.AllocHGlobal(Value.DataSize);
				propPviValue.propHasOwnDataPtr = true;
			}
			if (StructureMembers != null && StructureMembers.ContainsKey(varName))
			{
				result = StructureMembers[varName].SetStructValue(this, value);
				if (result == 0 && WriteValueAutomatic)
				{
					result = WriteValue();
				}
				return result;
			}
			string oldStyleValueName = GetOldStyleValueName(varName);
			if (StructureMembers != null && StructureMembers.ContainsKey(oldStyleValueName))
			{
				result = StructureMembers[oldStyleValueName].SetStructValue(this, value);
				if (result == 0 && WriteValueAutomatic)
				{
					result = WriteValue();
				}
				return result;
			}
			if (!propPviValue.IsOfTypeArray)
			{
				Value = value;
				return 0;
			}
			if (propPviValue.IsOfTypeArray)
			{
				int num;
				if (-1 == oldStyleValueName.IndexOf("["))
				{
					num = System.Convert.ToInt32(oldStyleValueName) * propPviValue.TypeLength;
				}
				else if (oldStyleValueName.IndexOf('[') == 0)
				{
					string[] array = oldStyleValueName.Substring(1, oldStyleValueName.Length - 2).Split(',');
					if (1 < array.Length)
					{
						int[] array2 = new int[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array2.SetValue(System.Convert.ToInt32(array.GetValue(i).ToString()), i);
						}
						return SetStructureMemberValue(value, array2);
					}
					System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.Length - 2));
					num = System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.Length - 2)) * propPviValue.TypeLength;
				}
				else
				{
					num = System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.Length - 2)) * propPviValue.TypeLength;
				}
				switch (propPviValue.DataType)
				{
				case DataType.Boolean:
					if (!(bool)value)
					{
						Marshal.WriteByte(propPviValue.pData, num, 0);
					}
					else
					{
						Marshal.WriteByte(propPviValue.pData, num, 1);
					}
					break;
				case DataType.SByte:
					PviMarshal.WriteSByte(propPviValue.pData, num, PviMarshal.toSByte(value));
					break;
				case DataType.Byte:
				case DataType.UInt8:
					Marshal.WriteByte(propPviValue.pData, num, PviMarshal.toByte(value));
					break;
				case DataType.Int16:
					Marshal.WriteInt16(propPviValue.pData, num, PviMarshal.toInt16(value));
					break;
				case DataType.UInt16:
				case DataType.WORD:
					PviMarshal.WriteUInt16(propPviValue.pData, num, PviMarshal.toUInt16(value));
					break;
				case DataType.Int32:
					Marshal.WriteInt32(propPviValue.pData, num, PviMarshal.toInt32(value));
					break;
				case DataType.Int64:
					PviMarshal.WriteInt64(propPviValue.pData, num, PviMarshal.toInt64(value));
					break;
				case DataType.UInt32:
				case DataType.DWORD:
					PviMarshal.WriteUInt32(propPviValue.pData, num, PviMarshal.toUInt32(value));
					break;
				case DataType.UInt64:
					PviMarshal.WriteUInt64(propPviValue.pData, num, PviMarshal.toUInt64(value));
					break;
				case DataType.Single:
					Service.cpyFltToBuffer(value);
					Marshal.WriteByte(propPviValue.pData, num, Service.ByteBuffer[0]);
					Marshal.WriteByte(propPviValue.pData, num + 1, Service.ByteBuffer[1]);
					Marshal.WriteByte(propPviValue.pData, num + 2, Service.ByteBuffer[2]);
					Marshal.WriteByte(propPviValue.pData, num + 3, Service.ByteBuffer[3]);
					break;
				case DataType.Double:
					Service.cpyDblToBuffer(value);
					Marshal.WriteByte(propPviValue.pData, num, Service.ByteBuffer[0]);
					Marshal.WriteByte(propPviValue.pData, num + 1, Service.ByteBuffer[1]);
					Marshal.WriteByte(propPviValue.pData, num + 2, Service.ByteBuffer[2]);
					Marshal.WriteByte(propPviValue.pData, num + 3, Service.ByteBuffer[3]);
					Marshal.WriteByte(propPviValue.pData, num + 4, Service.ByteBuffer[4]);
					Marshal.WriteByte(propPviValue.pData, num + 5, Service.ByteBuffer[5]);
					Marshal.WriteByte(propPviValue.pData, num + 6, Service.ByteBuffer[6]);
					Marshal.WriteByte(propPviValue.pData, num + 7, Service.ByteBuffer[7]);
					break;
				case DataType.TimeSpan:
				case DataType.TimeOfDay:
				case DataType.TOD:
					Marshal.WriteInt32(propPviValue.pData, num, Pvi.GetTimeSpanInt32(value));
					break;
				case DataType.DateTime:
				case DataType.Date:
				case DataType.DT:
					PviMarshal.WriteUInt32(propPviValue.pData, num, Pvi.GetDateTimeUInt32(value));
					break;
				case DataType.String:
					for (int i = 0; i < propPviValue.propTypeLength; i++)
					{
						Marshal.WriteByte(propPviValue.pData, num + i, 0);
						if (i < value.TypeLength)
						{
							byte val = Marshal.ReadByte(value.pData, i);
							Marshal.WriteByte(propPviValue.pData, num + i, val);
						}
					}
					break;
				case DataType.WString:
				{
					Value value2;
					if (DataType.WString == value.DataType)
					{
						value2 = value;
					}
					else
					{
						value2 = new Value();
						value2.propDataType = DataType.WString;
						value2.propTypeLength = value.ToString().Length * 2;
						value2.Assign(value.ToString());
					}
					for (int i = 0; i < propPviValue.propTypeLength; i++)
					{
						Marshal.WriteByte(propPviValue.pData, num + i, 0);
						if (i < value2.TypeLength)
						{
							byte val = Marshal.ReadByte(value2.pData, i);
							Marshal.WriteByte(propPviValue.pData, num + i, val);
						}
					}
					break;
				}
				}
				if (WriteValueAutomatic)
				{
					result = WriteValue();
				}
				return result;
			}
			if (WriteValueAutomatic)
			{
				OnValueWritten(new PviEventArgs(base.Name, base.Address, 120025, Service.Language, Action.VariableValueWrite, Service));
			}
			return -1;
		}

		internal void CleanupMemory()
		{
			propReadingFormat = false;
			if (null != propPviValue)
			{
				if (propPviValue.propHasOwnDataPtr && IntPtr.Zero != propPviValue.pData)
				{
					PviMarshal.FreeHGlobal(ref propPviValue.pData);
					propPviValue.pData = IntPtr.Zero;
				}
				propPviValue.propByteField = null;
				propPviValue.propObjValue = null;
				propPviValue = null;
			}
			if (pReadData == IntPtr.Zero)
			{
				PviMarshal.FreeHGlobal(ref pReadData);
				pReadData = IntPtr.Zero;
			}
			if (pWriteData == IntPtr.Zero)
			{
				PviMarshal.FreeHGlobal(ref pWriteData);
				pWriteData = IntPtr.Zero;
			}
			propInternalByteField = null;
			if (null != propInternalValue)
			{
				if (propInternalValue.propHasOwnDataPtr && IntPtr.Zero != propInternalValue.pData)
				{
					PviMarshal.FreeHGlobal(ref propInternalValue.pData);
					propInternalValue.pData = IntPtr.Zero;
				}
				propInternalValue.propByteField = null;
				propInternalValue.propObjValue = null;
				propInternalValue = null;
			}
			if (propIODataPoints != null)
			{
				propIODataPoints = null;
			}
			if (mapNameToMember != null)
			{
				mapNameToMember.CleanUp(disposing: true);
				mapNameToMember = null;
			}
			if (propMembers != null)
			{
				propMembers.Dispose(disposing: true, removeFromCollection: true);
				propMembers = null;
			}
			if (propUserMembers != null)
			{
				propUserMembers.Dispose(disposing: true, removeFromCollection: true);
				propUserMembers = null;
			}
			propChangedStructMembers = null;
			if (propScalingPoints != null)
			{
				propScalingPoints.Clear();
				propScalingPoints = null;
			}
			if (propWriteByteField != null)
			{
				propWriteByteField = null;
			}
			if (propReadByteField != null)
			{
				propReadByteField = null;
			}
			if (propUserCollections != null)
			{
				propUserCollections.Clear();
				propUserCollections = null;
			}
			if (propUserMembers != null)
			{
				propUserMembers.CleanUp(disposing: true);
				propUserMembers = null;
			}
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (propDisposed)
			{
				return;
			}
			propReadingFormat = false;
			if (disposing)
			{
				if (null != propPviValue)
				{
					propPviValue.Dispose(disposing);
					propPviValue = null;
				}
				if (pReadData == IntPtr.Zero)
				{
					PviMarshal.FreeHGlobal(ref pReadData);
					pReadData = IntPtr.Zero;
				}
				if (pWriteData == IntPtr.Zero)
				{
					PviMarshal.FreeHGlobal(ref pWriteData);
					pWriteData = IntPtr.Zero;
				}
				propInternalByteField = null;
				if (null != propInternalValue)
				{
					propInternalValue.Dispose(disposing);
					propInternalValue = null;
				}
				if (propIODataPoints != null)
				{
					propIODataPoints.Dispose(disposing, removeFromCollection);
					propIODataPoints = null;
				}
				if (mapNameToMember != null)
				{
					mapNameToMember.CleanUp(disposing);
					mapNameToMember = null;
				}
				if (propMembers != null)
				{
					propMembers.Dispose(disposing, removeFromCollection);
					propMembers = null;
				}
				if (propUserMembers != null)
				{
					propUserMembers.Dispose(disposing, removeFromCollection);
					propUserMembers = null;
				}
				propChangedStructMembers = null;
				if (propScalingPoints != null)
				{
					propScalingPoints.Clear();
					propScalingPoints = null;
				}
				if (propWriteByteField != null)
				{
					propWriteByteField = null;
				}
				if (propReadByteField != null)
				{
					propReadByteField = null;
				}
				if (propUserCollections != null)
				{
					propUserCollections.Clear();
					propUserCollections = null;
				}
				if (propUserMembers != null)
				{
					propUserMembers.CleanUp(disposing);
					propUserMembers = null;
				}
				if (removeFromCollection)
				{
					if (Parent is Service && ((Service)Parent).Variables != null)
					{
						((Service)Parent).Variables.Remove(base.Name);
					}
					if (Parent is Cpu)
					{
						((Cpu)Parent).Variables.Remove(base.Name);
					}
					if (Parent is Task)
					{
						((Task)Parent).Variables.Remove(base.Name);
					}
					if (Parent is Variable)
					{
						((Variable)Parent).Members.Remove(base.Name);
					}
				}
			}
			base.Dispose(disposing, removeFromCollection);
		}

		public override void Remove()
		{
			base.Remove();
			if (propMembers != null)
			{
				propMembers.CleanUp(disposing: false);
			}
			if (Parent is Cpu)
			{
				((Cpu)Parent).Variables.Remove(base.Name);
			}
			else if (Parent is Task)
			{
				((Task)Parent).Variables.Remove(base.Name);
			}
			if (propUserCollections != null && 0 < propUserCollections.Count)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					value.Remove(this);
				}
			}
		}

		internal void Remove(object coll)
		{
			base.Remove();
			if (propMembers != null)
			{
				propMembers.CleanUp(disposing: false);
			}
			if (Parent is Cpu && ((Cpu)Parent).Variables != coll)
			{
				((Cpu)Parent).Variables.Remove(base.Name);
			}
			else if (Parent is Task && ((Task)Parent).Variables != coll)
			{
				((Task)Parent).Variables.Remove(base.Name);
			}
			if (propUserCollections != null && 0 < propUserCollections.Count)
			{
				foreach (VariableCollection value in propUserCollections.Values)
				{
					if (value != coll)
					{
						value.Remove(this);
					}
				}
			}
		}

		internal string GetPviDataTypeText(DataType dataType)
		{
			switch (dataType)
			{
			case DataType.UInt8:
				return "u8";
			case DataType.Byte:
				return "u8";
			case DataType.SByte:
				return "i8";
			case DataType.Int16:
				return "i16";
			case DataType.UInt16:
				return "u16";
			case DataType.WORD:
				return "WORD";
			case DataType.Int32:
				return "i32";
			case DataType.Int64:
				return "i64";
			case DataType.UInt32:
				return "u32";
			case DataType.DWORD:
				return "DWORD";
			case DataType.UInt64:
				return "u64";
			case DataType.Single:
				return "f32";
			case DataType.Double:
				return "f64";
			case DataType.String:
				return "string";
			default:
				return "";
			}
		}

		public void WriteScaling()
		{
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			if (Scaling.ScalingType == ScalingType.LimitValuesAndFactor)
			{
				ScalingPointCollection scalingPointCollection = new ScalingPointCollection();
				scalingPointCollection.Add(new ScalingPoint(Scaling.MinValue, Scaling.MinValue * Scaling.Factor));
				scalingPointCollection.Add(new ScalingPoint(Scaling.MaxValue, Scaling.MaxValue * Scaling.Factor));
				Scaling.ScalingPoints = scalingPointCollection;
				Scaling.propScalingType = ScalingType.LimitValuesAndFactor;
			}
			string text = "";
			if (propScaling != null && propScaling.ScalingPoints.Count > 0)
			{
				text = "";
				foreach (ScalingPoint scalingPoint in propScaling.ScalingPoints)
				{
					string text2 = scalingPoint.XValue.ToString(CultureInfo.InvariantCulture);
					string text3 = scalingPoint.YValue.ToString(CultureInfo.InvariantCulture);
					if (DataType.Single == propPviValue.propDataType || DataType.Double == propPviValue.propDataType)
					{
						if (-1 == text2.IndexOf('.'))
						{
							text2 += ".0";
						}
						if (-1 == text3.IndexOf('.'))
						{
							text3 += ".0";
						}
					}
					text += $"{text2},{text3};";
				}
			}
			Service.BuildRequestBuffer(text);
			if (base.ConnectionType == ConnectionType.Link)
			{
				if (ConnectionStates.Connected == propConnectionState || ConnectionStates.ConnectedError == propConnectionState)
				{
					Disconnect(2715u);
					Connect(ConnectionType.Link, 2713);
					return;
				}
			}
			else if (propLinkId != 0)
			{
				num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.Function, Service.RequestBuffer, text.Length, 553u);
			}
			if (num != 0)
			{
				OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableScalingChange, Service));
			}
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base baseObj)
		{
			Variable variable = (Variable)baseObj;
			if (variable == null)
			{
				return -1;
			}
			base.FromXmlTextReader(ref reader, flags, variable);
			string text = "";
			text = reader.GetAttribute("DataType");
			if (text != null && text.Length > 0)
			{
				variable.propPviValue.SetDataType(GetDataTypeFromString(text));
			}
			text = "";
			text = reader.GetAttribute("Length");
			if (text != null && text.Length > 0)
			{
				short result = 0;
				if (PviParse.TryParseInt16(text, out result))
				{
					variable.propPviValue.SetArrayLength(result);
				}
			}
			text = "";
			text = reader.GetAttribute("Value");
			if (text != null && text.Length > 0 && (ConfigurationFlags.Values & flags) != 0)
			{
				variable.Value.Assign(text);
			}
			text = "";
			text = reader.GetAttribute("Active");
			if (text != null && text.Length > 0 && (ConfigurationFlags.ActiveState & flags) != 0 && text.ToLower() == "true")
			{
				variable.SetActive(value: true);
			}
			text = "";
			text = reader.GetAttribute("RefreshTime");
			if (text != null && text.Length > 0 && (ConfigurationFlags.RefreshTime & flags) != 0)
			{
				int result2 = 0;
				if (PviParse.TryParseInt32(text, out result2))
				{
					variable.propRefreshTime = result2;
				}
			}
			text = "";
			text = reader.GetAttribute("Access");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "no":
					variable.Access = Access.No;
					break;
				case "read":
					variable.Access = Access.Read;
					break;
				case "write":
					variable.Access = Access.Write;
					break;
				case "readandwrite":
					variable.Access = Access.ReadAndWrite;
					break;
				case "event":
					variable.Access = Access.EVENT;
					break;
				case "direct":
					variable.Access = Access.DIRECT;
					break;
				case "fastecho":
					variable.Access = Access.FASTECHO;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("CastMode");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "decimalconversion":
					variable.propCastMode = CastModes.DecimalConversion;
					break;
				case "default":
					variable.propCastMode = CastModes.DEFAULT;
					break;
				case "floatconversion":
					variable.propCastMode = CastModes.FloatConversion;
					break;
				case "pg2000string":
					variable.propCastMode = CastModes.PG2000String;
					break;
				case "rangecheck":
					variable.propCastMode = CastModes.RangeCheck;
					break;
				case "stringtermination":
					variable.propCastMode = CastModes.StringTermination;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("StructName");
			if (text != null && text.Length > 0)
			{
				variable.propStructName = text;
			}
			text = "";
			text = reader.GetAttribute("StructMemberName");
			if (text != null && text.Length > 0)
			{
				variable.propName = text;
			}
			text = "";
			text = reader.GetAttribute("UserTag");
			if (text != null && text.Length > 0)
			{
				variable.propUserTag = text;
			}
			text = "";
			text = reader.GetAttribute("Scope");
			if (text != null && text.Length > 0 && (ConfigurationFlags.Scope & flags) != 0)
			{
				switch (text)
				{
				case "Global":
					variable.propScope = Scope.Global;
					break;
				case "Local":
					variable.propScope = Scope.Local;
					break;
				case "Dynamic":
					variable.propScope = Scope.Dynamic;
					break;
				default:
					variable.propScope = Scope.UNDEFINED;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("BitOffset");
			if (text != null && text.Length > 0)
			{
				int result3 = 0;
				if (PviParse.TryParseInt32(text, out result3))
				{
					variable.propBitOffset = result3;
				}
			}
			text = "";
			text = reader.GetAttribute("Hysteresis");
			if (text != null && text.Length > 0)
			{
				PviParse.TryParseDouble(text, out variable.propHysteresis);
			}
			text = "";
			text = reader.GetAttribute("InitValue");
			if (text != null && text.Length > 0)
			{
				variable.propInitValue = text;
			}
			text = "";
			text = reader.GetAttribute("ExpandMembers");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "true":
					variable.propExpandMembers = true;
					break;
				case "false":
					variable.propExpandMembers = false;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("Polling");
			if (text != null && string.Compare(text.ToLower(), "false") == 0)
			{
				variable.propPolling = false;
			}
			text = "";
			text = reader.GetAttribute("DataValid");
			if (text != null && string.Compare(text.ToLower(), "true") == 0)
			{
				variable.propDataValid = true;
			}
			text = "";
			text = reader.GetAttribute("WriteValueAutomatic");
			if (text != null && string.Compare(text.ToLower(), "false") == 0)
			{
				variable.propWriteValueAutomatic = false;
			}
			text = "";
			text = reader.GetAttribute("Attribute");
			if (text != null && text.Length > 0 && (ConfigurationFlags.IOAttributes & flags) != 0)
			{
				string attribute = reader.GetAttribute("Force");
				switch (text.ToLower())
				{
				case "input":
					variable.propAttribute = VariableAttribute.Input;
					if (attribute != null && attribute.Length > 0 && attribute.ToLower() == "true")
					{
						variable.propForceValue = true;
					}
					break;
				case "output":
					variable.propAttribute = VariableAttribute.Output;
					if (attribute != null && attribute.Length > 0 && attribute.ToLower() == "true")
					{
						variable.propForceValue = true;
					}
					break;
				case "constant":
					variable.propAttribute = VariableAttribute.Constant;
					break;
				case "variable":
					variable.propAttribute = VariableAttribute.Variable;
					break;
				case "none":
					variable.propAttribute = VariableAttribute.None;
					break;
				}
				variable.propWriteValueAutomatic = false;
			}
			if (ConnectionStates.Connected == variable.propConnectionState || ConnectionStates.ConnectedError == variable.propConnectionState)
			{
				variable.Requests |= Actions.Connect;
			}
			if (reader.GetAttribute("ScalingType") != null || reader.GetAttribute("MaxValue") != null || reader.GetAttribute("MinValue") != null || reader.GetAttribute("Factor") != null)
			{
				if (variable.Scaling == null)
				{
					variable.Scaling = new Scaling();
				}
				variable.Scaling.ScalingPoints.Clear();
				variable.Scaling.FromXmlTextReader(ref reader, flags, variable.Scaling);
			}
			else
			{
				reader.Read();
			}
			return 0;
		}

		public void ReadMemberVariables(ref XmlTextReader reader, ConfigurationFlags flags, Variable var)
		{
			if ((flags & ConfigurationFlags.VariableMembers) != 0)
			{
				do
				{
					string attribute = reader.GetAttribute("Name");
					Variable variable = null;
					if (attribute != null && attribute.Length > 0)
					{
						variable = new Variable(isMember: true, var, attribute, addToVColls: true);
						variable.propAlignment = propAlignment;
						FromXmlTextReader(ref reader, flags, variable);
						if (!var.Members.Contains(variable))
						{
							var.Members.Add(variable);
						}
					}
				}
				while (reader.Read() && reader.NodeType != XmlNodeType.EndElement);
			}
			else
			{
				reader.Skip();
			}
			reader.Read();
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			return SaveVariableConfiguration(ref writer, flags, this);
		}

		private int SaveVariableConfiguration(ref XmlTextWriter writer, ConfigurationFlags flags, Variable var)
		{
			writer.WriteStartElement("Variable");
			if (var.StructMemberName != null && var.StructMemberName.Length > 0)
			{
				writer.WriteAttributeString("Name", var.StructMemberName);
				if (var.PVRoot.propAddress + "." + var.StructMemberName != var.propAddress)
				{
					writer.WriteAttributeString("Address", var.propAddress);
				}
			}
			else if (var.Name != null && var.Name.Length > 0)
			{
				writer.WriteAttributeString("Name", var.Name);
				if (var.propAddress != null && var.propAddress.Length > 0 && var.Name != var.propAddress)
				{
					writer.WriteAttributeString("Address", var.propAddress);
				}
			}
			if (var.propUserData is string && var.propUserData != null && ((string)var.propUserData).Length > 0)
			{
				writer.WriteAttributeString("UserData", var.propUserData.ToString());
			}
			if (var.propLinkName != null && var.propLinkName.Length > 0)
			{
				writer.WriteAttributeString("LinkName", var.propLinkName);
			}
			writer.WriteAttributeString("Connected", propConnectionState.ToString());
			if (var.propPviValue.DataType != 0)
			{
				writer.WriteAttributeString("DataType", GetDataTypeString(var.propPviValue.DataType));
			}
			if (var.propPviValue.IsOfTypeArray)
			{
				writer.WriteAttributeString("ArraySize", var.propPviValue.ArrayLength.ToString());
			}
			if (var.propPviValue.DataType == DataType.String)
			{
				writer.WriteAttributeString("StringLength", var.propPviValue.TypeLength.ToString());
			}
			if (var.Active && (flags & ConfigurationFlags.ActiveState) != 0)
			{
				writer.WriteAttributeString("Active", "true");
			}
			if (var.Scaling != null)
			{
				var.Scaling.ToXMLTextWriter(ref writer, flags);
			}
			if ((flags & ConfigurationFlags.IOAttributes) != 0)
			{
				if ((var.propAttribute & VariableAttribute.Variable) != 0)
				{
					if ((var.propAttribute & VariableAttribute.Input) != 0)
					{
						writer.WriteAttributeString("Attribute", "Input");
						writer.WriteAttributeString("Force", var.ForceValue.ToString());
					}
					else if ((var.propAttribute & VariableAttribute.Output) != 0)
					{
						writer.WriteAttributeString("Attribute", "Output");
						writer.WriteAttributeString("Force", var.ForceValue.ToString());
					}
				}
				else if ((var.propAttribute & VariableAttribute.Constant) != 0)
				{
					writer.WriteAttributeString("Attribute", "Constant");
				}
			}
			if ((var != var.PVRoot && var.PVRoot.ConnectionType != var.ConnectionType) || var == var.PVRoot)
			{
				if ((flags & ConfigurationFlags.Scope) != 0)
				{
					if (var.Scope == Scope.Global)
					{
						writer.WriteAttributeString("Scope", "Global");
					}
					else if (var.Scope == Scope.Local)
					{
						writer.WriteAttributeString("Scope", "Local");
					}
					else if (var.Scope == Scope.Dynamic)
					{
						writer.WriteAttributeString("Scope", "Dynamic");
					}
					else
					{
						writer.WriteAttributeString("Scope", "UNDEFINED");
					}
				}
				if ((ConfigurationFlags.ConnectionState & flags) != 0 && var.ConnectionType != ConnectionType.CreateAndLink)
				{
					writer.WriteAttributeString("ConnectionType", var.ConnectionType.ToString());
				}
				if (var.propErrorCode > 0)
				{
					writer.WriteAttributeString("ErrorCode", var.propErrorCode.ToString());
				}
				if (var.RefreshTime != 100 && (flags & ConfigurationFlags.RefreshTime) != 0)
				{
					writer.WriteAttributeString("RefreshTime", var.RefreshTime.ToString());
				}
				if (var.propVariableAccess != Access.ReadAndWrite)
				{
					writer.WriteAttributeString("Access", var.propVariableAccess.ToString());
				}
				if (var.CastMode != 0)
				{
					writer.WriteAttributeString("CastMode", var.CastMode.ToString());
				}
				if (var.StructName != null && var.StructName.Length > 0)
				{
					writer.WriteAttributeString("StructName", var.StructName);
				}
				if (var.propUserTag != null && var.propUserTag.Length > 0)
				{
					writer.WriteAttributeString("UserTag", var.propUserTag);
				}
				if (-1 != var.BitOffset)
				{
					writer.WriteAttributeString("BitOffset", var.BitOffset.ToString());
				}
				if (0.0 != var.Hysteresis)
				{
					writer.WriteAttributeString("Hysteresis", var.Hysteresis.ToString());
				}
				if (var.InitValue != null && var.InitValue.Length > 0)
				{
					writer.WriteAttributeString("InitValue", var.InitValue);
				}
				if (!var.ExpandMembers && (GetDataTypeString(var.propPviValue.DataType) == DataType.Structure.ToString() || var.propPviValue.propArrayLength > 0))
				{
					writer.WriteAttributeString("ExpandMembers", var.ExpandMembers.ToString());
				}
				if (!var.Polling)
				{
					writer.WriteAttributeString("Polling", var.Polling.ToString());
				}
				if (var.DataValid)
				{
					writer.WriteAttributeString("DataValid", var.DataValid.ToString());
				}
				if (!var.WriteValueAutomatic)
				{
					writer.WriteAttributeString("WriteValueAutomatic", var.WriteValueAutomatic.ToString());
				}
			}
			if ((flags & ConfigurationFlags.VariableMembers) != 0 && var.mapNameToMember != null && var.mapNameToMember.Count > 0)
			{
				writer.WriteStartElement("Members");
				for (int i = 0; i < var.mapNameToMember.Count; i++)
				{
					Variable var2 = (Variable)var.mapNameToMember[i];
					SaveVariableConfiguration(ref writer, flags, var2);
				}
				writer.WriteEndElement();
			}
			if (var.Value != null && var.Value.ToString() != null && var.Value.ToString() != "" && (flags & ConfigurationFlags.Values) != 0 && var.Value.propDataType != DataType.Structure && var.Value.propArrayLength < 2)
			{
				writer.WriteAttributeString("Value", var.Value.ToString());
			}
			writer.WriteEndElement();
			return 0;
		}

		private Variable GetVariable(params int[] indices)
		{
			if (Members != null)
			{
				string text = base.Name;
				for (int i = 0; i < indices.Length; i++)
				{
					text = text + "[" + indices.GetValue(i).ToString() + "]";
				}
				return Members[text];
			}
			return null;
		}

		private Value InternalGetValue()
		{
			if (propExpandMembers)
			{
				if (Convert == null || propPviValue.propArrayLength > 1 || propPviValue.propObjValue == null)
				{
					if (IsConnected || null == propInternalValue)
					{
						return propPviValue;
					}
					return propInternalValue;
				}
				if (IsConnected)
				{
					return Convert.PviValueToValue(propPviValue);
				}
				return Convert.PviValueToValue(propInternalValue);
			}
			if (IsConnected || null == propInternalValue)
			{
				return propPviValue;
			}
			return propInternalValue;
		}

		private bool InternalSetArrayValue(Value newValue)
		{
			int num = 0;
			if (newValue.propArrayLength > 1)
			{
				if (newValue.DataType != propPviValue.propDataType || newValue.propArrayLength != propPviValue.propArrayLength)
				{
					num = 12034;
				}
				ResizePviDataPtr(propPviValue.propArrayLength * propPviValue.propTypeLength);
				ResizePviDataPtr(propPviValue.propDataSize);
				if (newValue.propByteField.Length > propPviValue.propDataSize)
				{
					Marshal.Copy(newValue.propByteField, 0, propPviValue.pData, propPviValue.propDataSize);
				}
				else
				{
					Marshal.Copy(newValue.propByteField, 0, propPviValue.pData, newValue.propByteField.Length);
				}
				if (WriteValueAutomatic)
				{
					num = WriteValue();
					if (num != 0)
					{
						OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableValueWrite, Service));
						OnValueWritten(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableValueWrite, Service));
					}
				}
				newValue = null;
				return true;
			}
			return false;
		}

		private void InternalSetBoolValue(Value newValue)
		{
			ResizePviDataPtr(1);
			if (newValue == value2: false || newValue.ToInt32(null) == 0)
			{
				Marshal.WriteByte(propPviValue.pData, 0);
			}
			else
			{
				Marshal.WriteByte(propPviValue.pData, 1);
			}
		}

		private void InternalSetSByteValue(Value newValue)
		{
			if (CastModes.PG2000String == (CastMode & CastModes.PG2000String) && propPviValue.IsOfTypeArray)
			{
				propPviValue.Assign(newValue.ToString());
				return;
			}
			ResizePviDataPtr(1);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					sbyte b = System.Convert.ToSByte(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
					Marshal.WriteByte(propPviValue.pData, (byte)b);
				}
			}
			else
			{
				Marshal.WriteByte(propPviValue.pData, (byte)newValue.ToSByte(null));
			}
		}

		private void InternalSetInt16Value(Value newValue)
		{
			ResizePviDataPtr(2);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					short val = System.Convert.ToInt16(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
					Marshal.WriteInt16(propPviValue.pData, val);
				}
			}
			else
			{
				Marshal.WriteInt16(propPviValue.pData, newValue.ToInt16(null));
			}
		}

		private void InternalSetInt32Value(Value newValue)
		{
			ResizePviDataPtr(4);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					int val = System.Convert.ToInt32(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
					Marshal.WriteInt32(propPviValue.pData, val);
				}
			}
			else
			{
				Marshal.WriteInt32(propPviValue.pData, newValue.ToInt32(null));
			}
		}

		private void InternalSetInt64Value(Value newValue)
		{
			ResizePviDataPtr(8);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					long val = System.Convert.ToInt64(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
					PviMarshal.WriteInt64(propPviValue.pData, val);
				}
			}
			else
			{
				PviMarshal.WriteInt64(propPviValue.pData, newValue.ToInt64(null));
			}
		}

		private void InternalSetUInt8Value(Value newValue)
		{
			if (CastModes.PG2000String == (CastMode & CastModes.PG2000String) && propPviValue.IsOfTypeArray)
			{
				propPviValue.Assign(newValue.ToString());
				return;
			}
			ResizePviDataPtr(1);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					byte val = System.Convert.ToByte(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
					Marshal.WriteByte(propPviValue.pData, val);
				}
			}
			else
			{
				Marshal.WriteByte(propPviValue.pData, newValue.ToByte(null));
			}
		}

		private void InternalSetUInt16Value(Value newValue)
		{
			ResizePviDataPtr(2);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					ushort num = System.Convert.ToUInt16(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
					Marshal.WriteInt16(propPviValue.pData, (short)num);
				}
			}
			else
			{
				Marshal.WriteInt16(propPviValue.pData, (short)newValue.ToUInt16(null));
			}
		}

		private void InternalSetUInt32Value(Value newValue)
		{
			ResizePviDataPtr(4);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					uint val = System.Convert.ToUInt32(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
					Marshal.WriteInt32(propPviValue.pData, (int)val);
				}
			}
			else
			{
				Marshal.WriteInt32(propPviValue.pData, (int)newValue.ToUInt32(null));
			}
		}

		private void InternalSetUInt64Value(Value newValue)
		{
			ResizePviDataPtr(8);
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					System.Convert.ToUInt64(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
					PviMarshal.WriteInt64(propPviValue.pData, newValue);
				}
			}
			else
			{
				PviMarshal.WriteInt64(propPviValue.pData, (long)newValue.ToUInt64(null));
			}
		}

		private void InternalSetSingleValue(Value newValue)
		{
			ResizePviDataPtr(4);
			float[] array = new float[1];
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (!propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					return;
				}
				array[0] = System.Convert.ToSingle(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
			}
			else
			{
				array[0] = newValue.ToSingle(null);
			}
			Marshal.Copy(array, 0, propPviValue.pData, 1);
		}

		private void InternalSetDoubleValue(Value newValue)
		{
			ResizePviDataPtr(8);
			double[] array = new double[1];
			if (1 == propPviValue.IsEnum && DataType.String == newValue.DataType)
			{
				if (!propPviValue.propEnumerations.Names.Contains(newValue.ToString()))
				{
					return;
				}
				array[0] = System.Convert.ToDouble(propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString());
			}
			else
			{
				array[0] = newValue.ToDouble(null);
			}
			Marshal.Copy(array, 0, propPviValue.pData, 1);
		}

		private void InternalSetTimeValue(Value newValue)
		{
			ResizePviDataPtr(4);
			Marshal.WriteInt32(propPviValue.pData, Pvi.GetTimeSpanInt32(newValue.propObjValue));
		}

		private void InternalSetDateValue(Value newValue)
		{
			ResizePviDataPtr(4);
			Marshal.WriteInt32(propPviValue.pData, (int)Pvi.GetDateTimeUInt32(newValue.propObjValue));
		}

		private void InternalSetStringValue(Value newValue)
		{
			if (newValue.propParent == null)
			{
				newValue.propParent = this;
			}
			ResizePviDataPtr(propPviValue.DataSize);
			ResetWriteDataPtr(this, propPviValue.DataSize, setZero: true);
			if (0 < newValue.ToString().Length)
			{
				if (newValue.ToString().Length < propPviValue.DataSize)
				{
					Marshal.Copy(newValue.pData, propWriteByteField, 0, newValue.ToString().Length);
					propWriteByteField.SetValue((byte)0, newValue.ToString().Length);
				}
				else
				{
					Marshal.Copy(newValue.pData, propWriteByteField, 0, propPviValue.DataSize);
				}
			}
			Marshal.Copy(propWriteByteField, 0, propPviValue.pData, propWriteByteField.Length);
		}

		private bool ConvertNewValue(Value valueToSet)
		{
			Value newValue = valueToSet;
			if (Convert != null && propPviValue.propObjValue != null)
			{
				newValue = Convert.ValueToPviValue(valueToSet);
			}
			if (InternalSetArrayValue(newValue))
			{
				return false;
			}
			switch (propPviValue.propDataType)
			{
			case DataType.Boolean:
				InternalSetBoolValue(newValue);
				break;
			case DataType.SByte:
				InternalSetSByteValue(newValue);
				break;
			case DataType.Int16:
				InternalSetInt16Value(newValue);
				break;
			case DataType.Int32:
				InternalSetInt32Value(newValue);
				break;
			case DataType.Int64:
				InternalSetInt64Value(newValue);
				break;
			case DataType.Byte:
			case DataType.UInt8:
				InternalSetUInt8Value(newValue);
				break;
			case DataType.UInt16:
			case DataType.WORD:
				InternalSetUInt16Value(newValue);
				break;
			case DataType.UInt32:
			case DataType.DWORD:
				InternalSetUInt32Value(newValue);
				break;
			case DataType.UInt64:
				InternalSetUInt64Value(newValue);
				break;
			case DataType.Single:
				InternalSetSingleValue(newValue);
				break;
			case DataType.Double:
				InternalSetDoubleValue(newValue);
				break;
			case DataType.TimeSpan:
			case DataType.TimeOfDay:
			case DataType.TOD:
				InternalSetTimeValue(newValue);
				break;
			case DataType.DateTime:
			case DataType.Date:
			case DataType.DT:
				InternalSetDateValue(newValue);
				break;
			case DataType.String:
				InternalSetStringValue(newValue);
				break;
			case DataType.WString:
				InternalSetWStringValue(newValue);
				break;
			}
			return true;
		}

		private void InternalSetWStringValue(Value newValue)
		{
			if (newValue.propParent == null)
			{
				newValue.propParent = this;
			}
			ResizePviDataPtr(propPviValue.DataSize);
			ResetWriteDataPtr(this, propPviValue.DataSize, setZero: true);
			if (0 < newValue.ToString().Length)
			{
				if (DataType.WString == newValue.DataType)
				{
					if (newValue.ToStringUNI(null, null).Length < propPviValue.DataSize)
					{
						Marshal.Copy(newValue.pData, propWriteByteField, 0, newValue.ToStringUNI(null, null).Length);
						propWriteByteField.SetValue((byte)0, newValue.ToStringUNI(null, null).Length);
					}
					else
					{
						Marshal.Copy(newValue.pData, propWriteByteField, 0, propPviValue.DataSize);
					}
				}
				else
				{
					propPviValue.Assign(newValue.ToString());
					Marshal.Copy(propPviValue.pData, propWriteByteField, 0, propPviValue.DataSize);
					propWriteByteField.SetValue((byte)0, newValue.ToString().Length * 2);
					propWriteByteField.SetValue((byte)0, newValue.ToString().Length * 2 + 1);
				}
			}
			Marshal.Copy(propWriteByteField, 0, propPviValue.pData, propWriteByteField.Length);
		}

		private void InternalSetValue(Value valueToSet)
		{
			int num = 0;
			if (propReadOnly)
			{
				OnError(new PviEventArgs(propName, propAddress, 12034, Service.Language, Action.VariableConnect, Service));
				OnValueWritten(new PviEventArgs(base.Name, base.Address, 12034, Service.Language, Action.VariableValueWrite, Service));
				num = 12034;
			}
			else if (IsConnected)
			{
				if (valueToSet.propObjValue is string && -1 != valueToSet.propObjValue.ToString().IndexOf(';'))
				{
					Value.Assign(valueToSet.propObjValue.ToString());
				}
				else if (!ConvertNewValue(valueToSet))
				{
					return;
				}
				if (WriteValueAutomatic && !isMemberVar)
				{
					num = WriteValue();
					if (0 < num)
					{
						OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableValueWrite, Service));
						OnValueWritten(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.VariableValueWrite, Service));
					}
				}
				else
				{
					Value.isAssigned = true;
				}
			}
			else
			{
				if (Service.WaitForParentConnection)
				{
					base.Requests |= Actions.SetValue;
				}
				if (Convert != null)
				{
					propInternalValue = Convert.ValueToPviValue(valueToSet);
				}
				else
				{
					propInternalValue = valueToSet;
				}
			}
		}

		private Value InitializeValue(Value baseVal, Value newVal)
		{
			return newVal;
		}

		public TypeCode GetMemberDataType(string name)
		{
			if (mapNameToMember != null && mapNameToMember.ContainsKey(name))
			{
				return mapNameToMember[name].Value.SystemDataType;
			}
			if (Value.IsOfTypeArray)
			{
				return Value.SystemDataType;
			}
			return TypeCode.Empty;
		}

		public int GetMemberDataSize(string name)
		{
			if (mapNameToMember != null && mapNameToMember.ContainsKey(name))
			{
				return mapNameToMember[name].Value.DataSize;
			}
			if (Value.IsOfTypeArray)
			{
				return Value.TypeLength;
			}
			return 0;
		}

		public int GetMemberArrayLength(string name)
		{
			if (mapNameToMember.ContainsKey(name))
			{
				return mapNameToMember[name].Value.ArrayLength;
			}
			if (Value.IsOfTypeArray)
			{
				return 1;
			}
			return 0;
		}

		private void SetActive(bool value)
		{
			propActive = value;
			if (propLinkId != 0)
			{
				if (!propActive)
				{
					Deactivate();
				}
				else
				{
					Activate();
				}
				return;
			}
			if (propActive)
			{
				base.Requests |= (Actions.SetActive | Actions.FireActivated);
				return;
			}
			if (Actions.FireActivated == (base.Requests & Actions.FireActivated))
			{
				base.Requests &= ~Actions.FireActivated;
			}
			if (Actions.SetActive == (base.Requests & Actions.SetActive))
			{
				base.Requests &= ~Actions.SetActive;
			}
		}

		private int Activate()
		{
			int num = 0;
			string eventMaskParameters = GetEventMaskParameters(ConnectionType.Link, useParamMarker: false);
			if (propLinkId != 0)
			{
				Service.BuildRequestBuffer(eventMaskParameters);
				base.Requests |= Actions.FireActivated;
				num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.EventMask, Service.RequestBuffer, eventMaskParameters.Length, 503u);
				if (num != 0)
				{
					OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableActivate, Service));
					OnActivated(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableActivate, Service));
				}
			}
			else
			{
				base.Requests |= (Actions.SetActive | Actions.FireActivated);
			}
			return num;
		}

		protected void DeactivateInternal()
		{
			Marshal.WriteByte(Service.RequestBuffer, 0);
			WriteRequest(Service.hPvi, propLinkId, AccessTypes.EventMask, Service.RequestBuffer, 0, 0u);
		}

		private int Deactivate()
		{
			int num = 0;
			Marshal.WriteByte(Service.RequestBuffer, 0);
			num = WriteRequest(Service.hPvi, propLinkId, AccessTypes.EventMask, Service.RequestBuffer, 0, 504u);
			if (num != 0)
			{
				OnError(new PviEventArgs(propName, propAddress, num, Service.Language, Action.VariableDeactivate, Service));
			}
			return num;
		}

		private string getMemberName()
		{
			if (propOwner is Variable)
			{
				if (((Variable)propOwner).Value.IsOfTypeArray)
				{
					string structMemberName = ((Variable)propOwner).StructMemberName;
					if (0 < structMemberName.Length && structMemberName.Length - 1 == structMemberName.LastIndexOf(']'))
					{
						return ((Variable)propOwner).StructMemberName + "." + base.Name;
					}
					return ((Variable)propOwner).StructMemberName + base.Name;
				}
				if (((Variable)propOwner).StructMemberName.Length == 0)
				{
					return base.Name;
				}
				return ((Variable)propOwner).StructMemberName + "." + base.Name;
			}
			return "";
		}

		internal virtual string GetStructMemberName(Variable root)
		{
			if (base.Address.IndexOf(root.Address, 0) == 0)
			{
				if (root.Value.IsOfTypeArray)
				{
					return base.Address.Substring(root.Address.Length);
				}
				return base.Address.Substring(root.Address.Length + 1);
			}
			return base.Address;
		}
	}
}
