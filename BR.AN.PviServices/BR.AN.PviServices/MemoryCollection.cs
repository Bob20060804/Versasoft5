using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
	public class MemoryCollection : BaseCollection
	{
		public Memory this[int index]
		{
			get
			{
				return (Memory)propArrayList[index];
			}
		}

		public Memory this[MemoryType type]
		{
			get
			{
				if (propParent is Cpu && 0 < ((Cpu)propParent).Memories.Count)
				{
					foreach (Memory value in ((Cpu)propParent).Memories.Values)
					{
						if (value.Type == type)
						{
							return value;
						}
					}
				}
				return null;
			}
		}

		public MemoryCollection(Cpu parent, string name)
			: base(CollectionType.ArrayList, parent, name)
		{
		}

		public void Add(Memory memory)
		{
			propArrayList.Add(memory);
		}

		protected void OnError(CollectionEventArgs e)
		{
			if (propParent is Cpu)
			{
				((Cpu)propParent).OnError(e);
			}
		}

		public void Upload()
		{
			int num = 0;
			if (propParent is Cpu && !((Cpu)propParent).IsConnected && Service.WaitForParentConnection)
			{
				base.Requests |= Actions.Upload;
			}
			num = PInvokePvicom.PviComReadArgumentRequest(((Cpu)propParent).Service, ((Cpu)propParent).LinkId, AccessTypes.CpuMemoryInfo, IntPtr.Zero, 0, 613u, base.InternId);
			if (num != 0)
			{
				OnError(new CollectionEventArgs(((Cpu)propParent).Name, ((Cpu)propParent).Address, num, Service.Language, Action.MemoriesUpload, null));
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (PVIReadAccessTypes.CpuMemoryInfo == accessType)
			{
				if (errorCode == 0)
				{
					uint num = 22u;
					uint num2 = dataLen / num;
					Clear();
					for (uint num3 = 0u; num3 < num2; num3++)
					{
						APIFC_CPmemInfoRes memory = (APIFC_CPmemInfoRes)Marshal.PtrToStructure(PviMarshal.GetIntPtr(pData, num3 * num), typeof(APIFC_CPmemInfoRes));
						Memory memory2 = new Memory((Cpu)propParent, memory);
						Add(memory2);
					}
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.MemoriesUpload, Service));
				}
				else
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.MemoriesUpload, Service));
					OnError(new CollectionEventArgs(propName, "", errorCode, Service.Language, Action.MemoriesUpload, null));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
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
			ArrayList arrayList = new ArrayList();
			propCounter = 0;
			if (Values != null)
			{
				foreach (Memory value in Values)
				{
					arrayList.Add(value);
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				object obj = arrayList[i];
				((Memory)obj).Dispose(disposing);
				obj = null;
			}
			Clear();
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = 0;
			int num = 0;
			if (Values.Count > 0)
			{
				writer.WriteStartElement(GetType().Name);
				foreach (object value in Values)
				{
					num = ((Memory)value).ToXMLTextWriter(ref writer, flags);
					if (num != 0)
					{
						result = num;
					}
				}
				writer.WriteEndElement();
			}
			return result;
		}
	}
}
