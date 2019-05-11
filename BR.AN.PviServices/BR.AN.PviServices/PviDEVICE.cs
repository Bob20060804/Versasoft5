namespace BR.AN.PviServices
{
	internal class PviDEVICE : Base
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

		public PviDEVICE(PviLINE line, string name)
			: base(line, name)
		{
		}

		public void SetName(string newName)
		{
			propName = newName;
		}

		public void Inititialize(string parentName, string objParameters)
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
			return XCreateRequest(Service.hPvi, pObjName, ObjectType.POBJ_DEVICE, base.ObjectParam, 2805u, "", 2803u);
		}

		internal int PviDisconnect()
		{
			int num = 0;
			propConnectionState = ConnectionStates.Disconnecting;
			if (base.LinkId != 0)
			{
				num = UnlinkRequest(2804u);
				if (num != 0)
				{
					OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.CpuDisconnect, Service));
				}
				propLinkId = 0u;
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
