using BR.AN.PviServices.ObjectList;
using System;

namespace BR.AN.PviServices
{
	public class TransferObject : TargetObjectBase
	{
		private readonly string _Name;

		private readonly int _ProjectDependent;

		private readonly uint _Size;

		private readonly string _TimeModified;

		public string Name => _Name;

		public int ProjectDependent => _ProjectDependent;

		public uint Size => _Size;

		public string TimeModified => _TimeModified;

		public event PviProgressHandler UpLoading;

		public event PviEventHandler UpLoaded;

		internal TransferObject(TargetObjectType objType, Cpu parentObj, string name)
			: base(objType, parentObj)
		{
			_Name = name;
			_ProjectDependent = -1;
			_Size = 0u;
			_TimeModified = "";
			if (parentObj != null)
			{
				foreach (TargetObjectBase item in parentObj.ObjectsOnTarget)
				{
					TransferObject transferObject = item as TransferObject;
					if (transferObject != null && objType == transferObject.Type && name == transferObject.Name)
					{
						throw new ArgumentException("There is already an object in \"ObjectsOnTarget\" which has the same name!", name);
					}
				}
				parentObj.ObjectsOnTarget.Add(this);
			}
		}

		internal TransferObject(TargetObjectType objType, Cpu parentObj, ObjectInfo objInfo)
			: base(objType, parentObj)
		{
			_Name = ((objInfo == null) ? "" : objInfo.Name);
			_ProjectDependent = (objInfo?.ProjectDependent ?? 0);
			_Size = (objInfo?.Size ?? 0);
			_TimeModified = ((objInfo == null) ? "" : objInfo.TimeModified);
		}

		internal int UpLoad(TransferRequest uploadOptions)
		{
			if (DeviceType.ANSLTcp != base.CpuParent.Connection.DeviceType)
			{
				return 12058;
			}
			if (base.CpuParent._TransferRequest != null)
			{
				return 11035;
			}
			int num = 0;
			IntPtr hMemory = IntPtr.Zero;
			int dataLen = 0;
			string text = "";
			text = "<?xml version=\"1.0\" encoding=\"utf-8\"?> ";
			object obj = text;
			text = obj + "<" + uploadOptions.RequestType + " " + uploadOptions.ToString() + "/> ";
			if (0 < text.Length)
			{
				dataLen = text.Length;
				hMemory = PviMarshal.StringToHGlobal(text);
			}
			num = PInvokePvicom.PviComReadArgumentRequest(base.CpuParent.Service, base.CpuParent.LinkId, AccessTypes.ANSL_TransferObject, hMemory, dataLen, 430u, _internId);
			if (IntPtr.Zero != hMemory)
			{
				PviMarshal.FreeHGlobal(ref hMemory);
			}
			if (num == 0)
			{
				base.CpuParent._TransferRequest = uploadOptions.Copy(_internId);
			}
			return num;
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (accessType == PVIReadAccessTypes.ANSL_TransferObject)
			{
				OnUpLoaded(errorCode);
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		internal void OnUpLoading(int percentComplete, int errorCode)
		{
			if (this.UpLoading != null)
			{
				this.UpLoading(this, new PviProgessEventArgs(Name, Name, errorCode, base.CpuParent.Service.Language, Action.ObjectUploading, percentComplete));
			}
		}

		private void OnUpLoaded(int statusCode)
		{
			base.CpuParent._TransferRequest = null;
			if (this.UpLoaded != null)
			{
				this.UpLoaded(this, new PviEventArgs(Name, Name, statusCode, base.CpuParent.Service.Language, Action.ObjectUploaded));
			}
		}

		public override string ToString()
		{
			return base.ToString() + " Name=\"" + Name + "\" ProjectDependent=\"" + ProjectDependent + "\" Size=\"" + Size + "\" TimeModified=\"" + TimeModified + "\"";
		}
	}
}
