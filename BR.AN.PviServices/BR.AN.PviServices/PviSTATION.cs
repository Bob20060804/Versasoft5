namespace BR.AN.PviServices
{
	internal class PviSTATION : Base
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

		public PviSTATION(PviDEVICE device, string name)
			: base(device, name)
		{
		}

		public void SetName(string newName)
		{
			propName = newName;
		}

		public void Initialize(string parentName, string objParameters)
		{
			if (LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
			{
				propObjectParam = "CD=" + objParameters;
			}
			else
			{
				propObjectParam = "CD=\"" + parentName + "\"/" + objParameters;
			}
		}

		public int PviConnect()
		{
			int num = 0;
			string pObjName = base.AddressEx;
			if (Service != null && LogicalObjectsUsage.ObjectNameWithType == Service.LogicalObjectsUsage)
			{
				pObjName = PviPathName;
			}
			return XCreateRequest(Service.hPvi, pObjName, ObjectType.POBJ_STATION, base.ObjectParam, 2808u, "", 2806u);
		}

		public int PviDisconnect()
		{
			int num = 0;
			propConnectionState = ConnectionStates.Disconnecting;
			if (base.LinkId != 0)
			{
				num = UnlinkRequest(2807u);
				if (num != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.CpuDisconnect, Service));
				}
				propLinkId = 0u;
			}
			else
			{
				propConnectionState = ConnectionStates.Disconnected;
				num = 4808;
			}
			return num;
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
