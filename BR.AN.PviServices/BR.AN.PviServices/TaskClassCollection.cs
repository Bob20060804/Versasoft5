using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	public class TaskClassCollection : BaseCollection
	{
		public TaskClass this[int index]
		{
			get
			{
				return (TaskClass)propArrayList[index];
			}
		}

		internal TaskClassCollection(Base parent, string name)
			: base(CollectionType.ArrayList, parent, name)
		{
		}

		internal void Add(TaskClass taskClass)
		{
			propArrayList.Add(taskClass);
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
				return;
			}
			num = PInvokePvicom.PviComReadArgumentRequest(((Cpu)propParent).Service, ((Cpu)propParent).LinkId, AccessTypes.TaskClass, IntPtr.Zero, 0, 617u, base.InternId);
			if (num != 0)
			{
				OnError(new CollectionEventArgs(((Base)propParent).Name, ((Base)propParent).Address, num, Service.Language, Action.TaskClassesUpload, null));
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (PVIReadAccessTypes.TaskClasses == accessType)
			{
				if (errorCode == 0)
				{
					int num = 44;
					int num2 = (int)((long)dataLen / (long)num);
					Clear();
					for (int i = 0; i < num2; i++)
					{
						APIFC_TkInfoRes taskClassInfo = (APIFC_TkInfoRes)Marshal.PtrToStructure(PviMarshal.GetIntPtr(pData, (ulong)(i * num)), typeof(APIFC_TkInfoRes));
						TaskClass taskClass = new TaskClass(taskClassInfo);
						Add(taskClass);
					}
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.TaskClassesUpload, Service));
				}
				else
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.TaskClassesUpload, Service));
					OnError(new CollectionEventArgs(propName, "", errorCode, Service.Language, Action.TaskClassesUpload, null));
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
				base.Dispose(disposing, removeFromCollection: false);
				base.propParent = null;
				base.propUserData = null;
				base.propName = null;
			}
		}

		internal void CleanUp(bool disposing)
		{
			propCounter = 0;
			if (Values != null)
			{
				foreach (TaskClass value in Values)
				{
					value.Dispose(disposing, removeFromCollection: true);
				}
			}
			Clear();
		}
	}
}
