using System;
using System.Collections;

namespace BR.AN.PviServices
{
	internal class PviObjectBrowser : Base
	{
		private bool propUseIDName;

		private Hashtable propLineNames;

		private Hashtable propDeviceNames;

		private Hashtable propCpuNames;

		private Hashtable propTaskNames;

		private Action pviActionType;

		private PviObjectBrowser propBrowserParent;

		private string pathName;

		public Action PviActionType
		{
			get
			{
				return pviActionType;
			}
			set
			{
				pviActionType = value;
			}
		}

		public PviObjectBrowser BrowserParent
		{
			get
			{
				return propBrowserParent;
			}
			set
			{
				propBrowserParent = value;
			}
		}

		public override string FullName
		{
			get
			{
				if (base.Name != null && 0 < base.Name.Length)
				{
					return propBrowserParent.FullName + "." + base.Name;
				}
				if (propBrowserParent != null)
				{
					return propBrowserParent.FullName;
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
					return propBrowserParent.PviPathName + "/\"" + propName + "\" OT=Task";
				}
				return propBrowserParent.PviPathName;
			}
		}

		public string PathName
		{
			get
			{
				if (pathName == null)
				{
					if (!propUseIDName && propBrowserParent != null)
					{
						return propBrowserParent.PathName + "/" + propName;
					}
					return propName;
				}
				return pathName;
			}
			set
			{
				pathName = value;
			}
		}

		public PviObjectBrowser(string strName, bool useIDName, Base baseObj, PviObjectBrowser parent)
			: base(baseObj)
		{
			propUseIDName = useIDName;
			propBrowserParent = parent;
			propName = strName;
			if (useIDName && 0 < strName.IndexOf(" OT="))
			{
				propName = strName.Substring(0, strName.IndexOf(" OT="));
			}
			Initialize(parent);
		}

		public PviObjectBrowser(string strName, bool useIDName, PviObjectBrowser parent)
			: base(parent.Service)
		{
			propUseIDName = useIDName;
			propBrowserParent = parent;
			propName = strName;
			if (useIDName && 0 < strName.IndexOf(" OT="))
			{
				propName = strName.Substring(0, strName.IndexOf(" OT="));
			}
			Initialize(parent);
		}

		internal void Deinit()
		{
			if (Action.ServiceReadLinesList == pviActionType)
			{
				Service.Cpus.CleanUp(disposing: true);
				Service.LoggerCollections.Clear();
				Service.LoggerEntries.CleanUp(disposing: true);
				Service.LogicalObjects.Clear();
				Service.DisconnectEx(Service.hPvi);
			}
			propLineNames.Clear();
			propDeviceNames.Clear();
			propCpuNames.Clear();
			propTaskNames.Clear();
		}

		internal int Link()
		{
			int num = 2;
			string text = "";
			string text2 = "";
			string text3 = "";
			if (Action.ServiceReadLinesList == pviActionType)
			{
				text = "LM=" + Service.MessageLimitation.ToString();
				if (1 != Service.PVIAutoStart)
				{
					text3 = " AS=" + Service.PVIAutoStart.ToString();
				}
				if (0 < Service.ProcessTimeout)
				{
					text2 = " PT=" + Service.ProcessTimeout.ToString();
				}
				string pInitParam = (Service.Server == null || Service.Server.Length <= 0) ? $"{text}{text2}{text3}" : $"{text} IP={Service.Server} PN={Service.Port}{text2}{text3}";
				num = Service.Initialize(ref Service.hPvi, Service.Timeout, Service.RetryTime, pInitParam, new IntPtr(0));
				Service.propPendingObjectBrowserEvents++;
				if (num == 0)
				{
					num = XLinkRequest(Service.hPvi, "Pvi", 99u, "", 98u);
				}
			}
			return num;
		}

		public int CreateReadObjectRequest(int lnkId)
		{
			int num = 0;
			Service.propPendingObjectBrowserEvents++;
			if (propService.EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComReadRequest(propService.hPvi, (uint)lnkId, AccessTypes.List, Service.cbRead, 4294967294u, base.InternId);
			}
			return PInvokePvicom.PviComMsgReadRequest(propService.hPvi, (uint)lnkId, AccessTypes.List, Service.WindowHandle, (uint)pviActionType, _internId);
		}

		public int CreateLinkObjectRequest()
		{
			int num = 0;
			Action respMsgNo = Action.ServiceLinkLine;
			switch (pviActionType)
			{
			case Action.ServiceReadDevicesList:
				respMsgNo = Action.ServiceLinkLine;
				break;
			case Action.ServiceReadStationsList:
				respMsgNo = Action.ServiceLinkDevice;
				break;
			case Action.ServiceReadCpuList:
				respMsgNo = Action.ServiceLinkStation;
				break;
			case Action.CpuReadTasksList:
				respMsgNo = Action.ServiceLinkCpu;
				break;
			case Action.CpuReadVariableList:
				respMsgNo = Action.CpuLinkVariable;
				break;
			case Action.TaskReadVariablesList:
				respMsgNo = Action.TaskLinkVariable;
				break;
			}
			Service.propPendingObjectBrowserEvents++;
			if (propService.EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComLinkRequest(propService.hPvi, PathName, Service.cbEvent, 0u, 0u, "", Service.cbLink, 4294967294u, base.InternId);
			}
			StringMarshal stringMarshal = new StringMarshal();
			return PInvokePvicom.PviComMsgLinkRequest(propService.hPvi, stringMarshal.GetBytes(PathName), Service.WindowHandle, 0u, 0u, null, Service.WindowHandle, (uint)respMsgNo, _internId);
		}

		private void Initialize(PviObjectBrowser parent)
		{
			if (parent == null)
			{
				pviActionType = Action.ServiceReadLinesList;
			}
			else if (parent.pviActionType == Action.ServiceReadLinesList)
			{
				pviActionType = Action.ServiceReadDevicesList;
			}
			else if (parent.pviActionType == Action.ServiceReadDevicesList)
			{
				pviActionType = Action.ServiceReadStationsList;
			}
			else if (parent.pviActionType == Action.ServiceReadStationsList)
			{
				pviActionType = Action.ServiceReadCpuList;
			}
			else if (parent.pviActionType == Action.ServiceReadCpuList)
			{
				pviActionType = Action.CpuReadTasksList;
			}
			else if (parent.pviActionType == Action.CpuReadTasksList)
			{
				pviActionType = Action.TaskReadVariablesList;
			}
			else if (parent.pviActionType == Action.ServiceReadStationsList)
			{
				pviActionType = Action.ServiceReadCpuList;
			}
			propLineNames = new Hashtable();
			propDeviceNames = new Hashtable();
			propCpuNames = new Hashtable();
			propTaskNames = new Hashtable();
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
			base.OnPviCreated(errorCode, linkID);
		}

		internal override void OnPviLinked(int errorCode, uint linkID, int option)
		{
			Action action = pviActionType;
			int num = errorCode;
			if (num == 0)
			{
				switch (pviActionType)
				{
				case Action.ServiceReadLinesList:
					num = CreateReadObjectRequest((int)linkID);
					action = Action.ServiceReadDevicesList;
					break;
				case Action.ServiceReadDevicesList:
					num = CreateReadObjectRequest((int)linkID);
					action = Action.ServiceReadStationsList;
					break;
				case Action.ServiceReadStationsList:
					num = CreateReadObjectRequest((int)linkID);
					action = Action.ServiceReadCpuList;
					break;
				case Action.ServiceReadCpuList:
					num = CreateReadObjectRequest((int)linkID);
					action = Action.CpuReadTasksList;
					break;
				case Action.CpuReadTasksList:
					num = CreateReadObjectRequest((int)linkID);
					action = Action.TaskReadVariablesList;
					break;
				case Action.TaskReadVariablesList:
					num = CreateReadObjectRequest((int)linkID);
					action = Action.TaskLinkVariable;
					break;
				case Action.CpuReadVariableList:
					num = CreateReadObjectRequest((int)linkID);
					action = Action.CpuLinkVariable;
					break;
				}
			}
			propService.OnPVIObjectsAttached(new PviEventArgs(propName, propService.propAddress, num, propService.Language, action));
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (eventType != EventTypes.Error && eventType != EventTypes.Data)
			{
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			int num = 0;
			bool bCpusIn = false;
			if (accessType == PVIReadAccessTypes.ChildObjects)
			{
				if (errorCode != 0 || 0 >= dataLen)
				{
					return;
				}
				switch (pviActionType)
				{
				case Action.ServiceReadLinesList:
					num = ReadAttachedLines(pData, dataLen);
					break;
				case Action.ServiceReadDevicesList:
					num = ReadAttachedDevices(pData, dataLen);
					break;
				case Action.ServiceReadStationsList:
					num = ReadAttachedStations(pData, dataLen, ref bCpusIn);
					if (num == 0 && bCpusIn)
					{
						pviActionType = Action.ServiceReadCpuList;
						num = ReadAttachedCpus(pData, dataLen);
					}
					break;
				case Action.ServiceReadCpuList:
					num = ReadAttachedCpus(pData, dataLen);
					break;
				case Action.CpuReadTasksList:
					num = ReadAttachedTasksOrGPVars(pData, dataLen);
					break;
				case Action.CpuReadVariableList:
					num = ReadAttachedVariables(pData, dataLen);
					break;
				case Action.TaskReadVariablesList:
					num = ReadAttachedVariables(pData, dataLen);
					break;
				}
				propService.OnPVIObjectsAttached(new PviEventArgs(propName, propService.propAddress, num, propService.Language, pviActionType));
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		private int ReadAttachedLines(IntPtr pData, uint dataLen)
		{
			int num = 0;
			bool flag = false;
			string text = "";
			string text2 = "";
			text = PviMarshal.ToAnsiString(pData, dataLen);
			string[] array = text.Split('\t');
			for (int i = 0; i < array.Length; i++)
			{
				flag = false;
				if (-1 != array[i].ToLower().IndexOf("ot=line"))
				{
					text2 = array[i].Substring(0, array[i].ToLower().IndexOf("ot=line") - 1);
					flag = propLineNames.ContainsKey(text2);
					if (!flag)
					{
						propLineNames.Add(text2, text2);
					}
					PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(text2, flag, this);
					num = pviObjectBrowser.CreateLinkObjectRequest();
					if (num != 0)
					{
						propService.OnPVIObjectsAttached(new PviEventArgs(propName, propService.propAddress, num, propService.Language, Action.ServiceLinkLine));
					}
				}
				else if (-1 != array[i].ToLower().IndexOf("ot=pvar"))
				{
					text2 = array[i].Substring(0, array[i].ToLower().IndexOf("ot=pvar") - 1);
					if (propLineNames.ContainsKey(text2))
					{
						text2 = "@Pvi/" + text2;
					}
					Variable variable = new Variable(propService, text2);
					variable.ConnectionType = ConnectionType.Link;
					PviObjectBrowser pviObjectBrowser2 = new PviObjectBrowser(array[i], propUseIDName, this);
					variable.LinkName = pviObjectBrowser2.PathName;
				}
			}
			return num;
		}

		private int ReadAttachedDevices(IntPtr pData, uint dataLen)
		{
			int num = 0;
			string text = "";
			text = PviMarshal.ToAnsiString(pData, dataLen);
			if (text == "")
			{
				return -1;
			}
			string[] array = text.Split('\t');
			for (int i = 0; i < array.Length; i++)
			{
				PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(array[i], propUseIDName, this);
				num = pviObjectBrowser.CreateLinkObjectRequest();
				if (num != 0)
				{
					propService.OnPVIObjectsAttached(new PviEventArgs(propName, propService.propAddress, num, propService.Language, Action.ServiceLinkDevice));
				}
			}
			return num;
		}

		private int ReadAttachedStations(IntPtr pData, uint dataLen, ref bool bCpusIn)
		{
			int num = 0;
			string text = "";
			text = PviMarshal.ToAnsiString(pData, dataLen);
			if (text == "")
			{
				return -1;
			}
			string[] array = text.Split('\t');
			for (int i = 0; i < array.Length; i++)
			{
				if (-1 != array[i].ToLower().IndexOf("ot=station"))
				{
					PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(array[i], propUseIDName, this);
					num = pviObjectBrowser.CreateLinkObjectRequest();
					if (num != 0)
					{
						propService.OnPVIObjectsAttached(new PviEventArgs(propName, propService.propAddress, num, propService.Language, Action.ServiceLinkStation));
					}
				}
				else if (-1 != array[i].ToLower().IndexOf("ot=cpu"))
				{
					bCpusIn = true;
				}
			}
			return num;
		}

		private string GetBrowserObjectName(PviObjectBrowser browseParent, LogicalObjectsUsage nameOption, string pviText, string otString)
		{
			int num = 0;
			string result = pviText.Substring(0, pviText.ToLower().IndexOf(otString) - 1);
			if (nameOption == LogicalObjectsUsage.ObjectName)
			{
				if (browseParent == null)
				{
					num = pviText.IndexOf('.');
					if (-1 == num)
					{
						result = pviText.Substring(0, pviText.ToLower().IndexOf(otString) - 1);
					}
					else
					{
						result = pviText.Substring(num + 1, pviText.ToLower().IndexOf(otString) - 2 - num);
						Service.propAddress = pviText.Substring(0, num);
					}
				}
				else
				{
					result = browseParent.Name.Substring(0, browseParent.Name.ToLower().IndexOf("ot=") - 1);
					num = pviText.IndexOf(result);
					result = ((-1 != num) ? pviText.Substring(num + result.Length + 1, pviText.ToLower().IndexOf(otString) - 1 - num - result.Length) : pviText.Substring(0, pviText.ToLower().IndexOf(otString) - 1));
				}
			}
			return result;
		}

		private int ReadAttachedCpus(IntPtr pData, uint dataLen)
		{
			int num = 0;
			string text = "";
			text = PviMarshal.ToAnsiString(pData, dataLen);
			if ("" != text)
			{
				string[] array = text.Split('\t');
				for (int i = 0; i < array.Length; i++)
				{
					if (-1 == array[i].ToLower().IndexOf("ot=cpu"))
					{
						continue;
					}
					if (!Service.Cpus.ContainsKey(array[i]))
					{
						Cpu cpu = new Cpu(Service, GetBrowserObjectName(null, Service.LogicalObjectsUsage, array[i], "ot=cpu"));
						cpu.ConnectionType = ConnectionType.Link;
						PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(array[i], propUseIDName, cpu, this);
						cpu.LinkName = pviObjectBrowser.PathName;
						num = pviObjectBrowser.CreateLinkObjectRequest();
						if (num != 0)
						{
							propService.OnPVIObjectsAttached(new PviEventArgs(propName, propService.propAddress, num, propService.Language, Action.ServiceLinkCpu));
						}
					}
					else
					{
						propService.OnPVIObjectsAttached(new PviEventArgs(propName, propService.propAddress, 0, propService.Language, Action.ServiceLinkCpu));
					}
				}
			}
			return num;
		}

		private int ReadAttachedTasksOrGPVars(IntPtr pData, uint dataLen)
		{
			int num = 0;
			string text = "";
			text = PviMarshal.ToAnsiString(pData, dataLen);
			if ("" != text)
			{
				string[] array = text.Split('\t');
				for (int i = 0; i < array.Length; i++)
				{
					if (-1 != array[i].ToLower().IndexOf("ot=task"))
					{
						Task task = new Task((Cpu)Parent, GetBrowserObjectName(this, Service.LogicalObjectsUsage, array[i], "ot=task"));
						task.ConnectionType = ConnectionType.Link;
						PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(array[i], propUseIDName, task, this);
						task.LinkName = pviObjectBrowser.PathName;
						num = pviObjectBrowser.CreateLinkObjectRequest();
						if (num != 0)
						{
							propService.OnPVIObjectsAttached(new PviEventArgs(propName, propService.propAddress, num, propService.Language, Action.CpuLinkTask));
						}
					}
					else if (-1 != array[i].ToLower().IndexOf("ot=module"))
					{
						Module module = new Module((Cpu)Parent, GetBrowserObjectName(this, Service.LogicalObjectsUsage, array[i], "ot=module"));
						module.ConnectionType = ConnectionType.Link;
						PviObjectBrowser pviObjectBrowser2 = new PviObjectBrowser(array[i], propUseIDName, module, this);
						module.LinkName = pviObjectBrowser2.PathName;
					}
					else if (-1 != array[i].ToLower().IndexOf("ot=pvar"))
					{
						Variable variable = new Variable((Cpu)Parent, GetBrowserObjectName(this, Service.LogicalObjectsUsage, array[i], "ot=pvar"));
						variable.ConnectionType = ConnectionType.Link;
						PviObjectBrowser pviObjectBrowser3 = new PviObjectBrowser(array[i], propUseIDName, variable, this);
						variable.LinkName = pviObjectBrowser3.PathName;
					}
				}
			}
			return num;
		}

		private int ReadAttachedVariables(IntPtr pData, uint dataLen)
		{
			int result = 0;
			string text = "";
			text = PviMarshal.ToAnsiString(pData, dataLen);
			if ("" != text)
			{
				string[] array = text.Split('\t');
				for (int i = 0; i < array.Length; i++)
				{
					if (-1 != array[i].ToLower().IndexOf("ot=pvar"))
					{
						Variable variable = new Variable((Task)Parent, array[i].Substring(0, array[i].ToLower().IndexOf("ot=pvar") - 1));
						variable.ConnectionType = ConnectionType.Link;
						PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(array[i], propUseIDName, variable, this);
						variable.LinkName = pviObjectBrowser.PathName;
					}
				}
			}
			return result;
		}
	}
}
