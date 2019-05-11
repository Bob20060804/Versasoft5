using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class IODataPointCollection : BaseCollection
	{
		private bool cpuPVRequest;

		public IODataPoint this[string name]
		{
			get
			{
				if (propCollectionType == CollectionType.HashTable)
				{
					return (IODataPoint)base[name];
				}
				return null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Service Service
		{
			get
			{
				if (propParent is Cpu)
				{
					return ((Cpu)propParent).Service;
				}
				if (propParent is Task)
				{
					return ((Task)propParent).Service;
				}
				if (propParent is Variable)
				{
					return ((Variable)propParent).Service;
				}
				if (propParent is Service)
				{
					return (Service)propParent;
				}
				return null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public IODataPointCollection(Base parent, string name)
			: base(parent, name)
		{
			cpuPVRequest = false;
			propParent = parent;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public IODataPointCollection(object parent, string name)
			: base(parent, name)
		{
			cpuPVRequest = false;
			propParent = parent;
		}

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
			int num = 0;
			ArrayList arrayList = new ArrayList();
			propCounter = 0;
			if (Values != null)
			{
				foreach (IODataPoint value in Values)
				{
					arrayList.Add(value);
				}
				for (num = 0; num < arrayList.Count; num++)
				{
					IODataPoint iODataPoint = (IODataPoint)arrayList[num];
					iODataPoint.Disconnect(0);
					iODataPoint.Dispose(disposing, removeFromCollection: true);
				}
			}
			Clear();
		}

		internal void Disconnect(bool noResponse)
		{
			if (noResponse)
			{
				foreach (IODataPoint value in Values)
				{
					value.Disconnect(noResponse: true);
				}
				propConnectionState = ConnectionStates.Disconnected;
			}
		}

		public void Upload()
		{
			Upload(Scope.UNDEFINED);
		}

		internal void Upload(Scope variableScope)
		{
			int num = 0;
			cpuPVRequest = false;
			if (propParent is Cpu)
			{
				cpuPVRequest = true;
				num = ReadIOLinkNodes("", ((Cpu)propParent).Service, ((Cpu)propParent).LinkId, base.InternId);
			}
			else
			{
				if (!(propParent is Variable))
				{
					return;
				}
				Variable variable = (Variable)propParent;
				Cpu cpu = null;
				if (variable.propParent is Cpu)
				{
					cpu = (Cpu)variable.propParent;
				}
				else if (variable.propParent is Task)
				{
					Task task = (Task)variable.propParent;
					if (task.propParent is Cpu)
					{
						cpu = (Cpu)task.propParent;
					}
				}
				if (cpu != null)
				{
					string text = "";
					text = GetUploadFilter(variable, (variableScope == Scope.UNDEFINED) ? variable.Scope : variableScope);
					IntPtr hMemory = PviMarshal.StringToHGlobal(text);
					num = ReadIOLinkNodes(text, Service, cpu.LinkId, variable.InternId);
					PviMarshal.FreeHGlobal(ref hMemory);
					if (num != 0)
					{
						OnError(new PviEventArgs(variable.Name, variable.Address, num, Service.Language, Action.LinkNodeList, Service));
					}
				}
				variable = null;
			}
		}

		private int ReadIOLinkNodes(string filter, Service sObject, uint lnkID, uint internID)
		{
			int num = 0;
			IntPtr hMemory = PviMarshal.StringToHGlobal(filter);
			num = PInvokePvicom.PviComReadArgumentRequest(sObject, lnkID, AccessTypes.LinkNodeList, hMemory, filter.Length, 810u, internID);
			PviMarshal.FreeHGlobal(ref hMemory);
			return num;
		}

		private string GetUploadFilter(Variable varRequ, Scope ioVarScope)
		{
			string text = "";
			if (varRequ.propParent is Cpu)
			{
				cpuPVRequest = true;
				text = "/QU=Pv /QP=" + varRequ.propAddress;
			}
			else if (varRequ.propParent is Task)
			{
				if (Scope.Global == ioVarScope)
				{
					cpuPVRequest = true;
					text = "/QU=Pv /QP=" + varRequ.propAddress;
				}
				else
				{
					text = $"/QU=Pv /QP={varRequ.propParent.Name}:{varRequ.propAddress}";
				}
			}
			return text.Replace(",]", "]");
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (PVIReadAccessTypes.LinkNodeList == accessType)
			{
				if (errorCode == 0 && dataLen != 0)
				{
					string text = "";
					text = PviMarshal.PtrToStringAnsi(pData, dataLen);
					string[] array = null;
					bool flag = text != null && 1 < text.Length;
					if (flag)
					{
						IODataPoint iODataPoint = null;
						Cpu cpu = null;
						if (propParent is Cpu)
						{
							cpu = (Cpu)propParent;
						}
						else if (propParent is Variable)
						{
							Variable variable = (Variable)propParent;
							cpu = ((!(variable.propParent is Cpu)) ? ((Cpu)((Task)variable.propParent).propParent) : ((Cpu)variable.propParent));
						}
						array = text.Split("\t".ToCharArray());
						for (int i = 0; i < array.Length; i++)
						{
							string text2 = array.GetValue(i).ToString();
							int num = text2.IndexOf("\0");
							if (-1 != num)
							{
								text2 = text2.Substring(0, num);
							}
							if ((iODataPoint = cpu.IODataPoints[text2]) == null)
							{
								iODataPoint = new IODataPoint(cpu, text2);
							}
							if (propParent is Variable && !((Variable)propParent).IODataPoints.ContainsKey(iODataPoint.Name))
							{
								((Variable)propParent).IODataPoints.Add(iODataPoint);
							}
						}
					}
					FireUploadedEvent(flag, errorCode);
				}
				else if (4599 != errorCode)
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.LinkNodesUpload, Service));
					OnError(new CollectionEventArgs(propName, "", errorCode, Service.Language, Action.LinkNodesUpload, null));
				}
				else
				{
					OnError(new CollectionEventArgs(propName, "", errorCode, Service.Language, Action.LinkNodesUpload, null));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		private void FireUploadedEvent(bool ioData, int errorCode)
		{
			if (cpuPVRequest)
			{
				OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.LinkNodesUpload, Service));
			}
			else
			{
				Upload(Scope.Global);
			}
		}

		protected virtual void OnError(PviEventArgs e)
		{
			Fire_Error(this, e);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual void Add(IODataPoint ioDataPoint)
		{
			if (!base.ContainsKey(ioDataPoint.Name))
			{
				base.Add(ioDataPoint.Name, ioDataPoint);
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
					writer.WriteStartElement("IODataPoint");
					num = ((IODataPoint)value).ToXMLTextWriter(ref writer, flags);
					if (num != 0)
					{
						result = num;
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			return result;
		}
	}
}
