namespace BR.AN.PviServices
{
	internal class PviLINE : Base
	{
		public override string FullName
		{
			get
			{
				if (base.Name != null && 0 < base.Name.Length)
				{
					return Parent.FullName + "." + base.Name;
				}
				if (Parent != null)
				{
					return Parent.FullName;
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
					return Parent.PviPathName + "/" + base.Name;
				}
				return Parent.PviPathName;
			}
		}

		public PviLINE(Cpu cpu, string name)
			: base(cpu.Service, name)
		{
		}

		public void SetName(string newName)
		{
			propName = newName;
		}

		public void Initialize(int type)
		{
			switch (type)
			{
			case 2:
				propName = "LNANSL";
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					propObjectParam = "CD=\"/LN=LNANSL\"";
				}
				else
				{
					propObjectParam = "CD=\"Pvi\"/\"/LN=LNANSL\"";
				}
				break;
			case 1:
				propName = "LNMODBUS";
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					propObjectParam = "CD=\"/LN=LNMODBUS\"";
				}
				else
				{
					propObjectParam = "CD=\"Pvi\"/\"/LN=LNMODBUS\"";
				}
				break;
			default:
				propName = "LNINA2";
				if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
				{
					propObjectParam = "CD=\"LNINA2\"";
				}
				else
				{
					propObjectParam = "CD=\"Pvi\"/\"LNINA2\"";
				}
				break;
			}
		}

		public int PviDisconnect()
		{
			int num = 0;
			propConnectionState = ConnectionStates.Disconnecting;
			if (base.LinkId != 0)
			{
				num = UnlinkRequest(2801u);
				if (num != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.CpuDisconnect, Service));
				}
				propLinkId = 0u;
			}
			else
			{
				OnDisconnected(new PviEventArgs(propName, propAddress, 4808, Service.Language, Action.CpuDisconnect, Service));
			}
			return num;
		}

		public int PviConnect()
		{
			int num = 0;
			string pObjName = base.AddressEx;
			if (Service != null && LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
			{
				pObjName = PviPathName;
			}
			return XCreateRequest(Service.hPvi, pObjName, ObjectType.POBJ_LINE, base.ObjectParam, 2802u, "", 2800u);
		}

		internal int TurnOffEvents()
		{
			int num = 0;
			string text = "";
			Service.BuildRequestBuffer(text);
			return Write(Service.hPvi, base.LinkId, AccessTypes.EventMask, Service.RequestBuffer, text.Length);
		}

		internal int TurnOnEvents()
		{
			int num = 0;
			string text = "eds";
			Service.BuildRequestBuffer(text);
			return WriteRequest(Service.hPvi, base.LinkId, AccessTypes.EventMask, Service.RequestBuffer, text.Length, _internId);
		}
	}
}
