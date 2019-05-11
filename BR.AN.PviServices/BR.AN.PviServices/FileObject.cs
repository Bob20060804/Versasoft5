using BR.AN.PviServices.ObjectList;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	public class FileObject : TransferObject
	{
		private readonly string _Class;

		private readonly string _Group;

		private readonly string _Directory;

		private readonly string _HashValue;

		public string Class => _Class;

		public string Group => _Group;

		public string Directory => _Directory;

		public string HashValue => _HashValue;

		internal FileObject(Cpu parentObj, BR.AN.PviServices.ObjectList.FileInfo fileInfo)
			: base(TargetObjectType.File, parentObj, fileInfo)
		{
			_Class = fileInfo.Class;
			_Group = fileInfo.Group;
			_Directory = fileInfo.Dir;
			_HashValue = fileInfo.HashValue;
		}

		public FileObject(Cpu parentObj, string name, string classOfObject, string group, string directory, string hashValue)
			: base(TargetObjectType.File, parentObj, name)
		{
			_Class = classOfObject;
			_Group = group;
			_Directory = directory;
			_HashValue = hashValue;
		}

		public int UpLoad(FileTransferRequest uploadOptions)
		{
			uploadOptions._LoadFromTarget = true;
			return UpLoad((TransferRequest)uploadOptions);
		}

		public int UpLoad(string fileName)
		{
			FileTransferRequest fileTransferRequest = new FileTransferRequest(fileName, base.Name, Class, Group, Directory);
			fileTransferRequest._LoadFromTarget = true;
			return UpLoad((TransferRequest)fileTransferRequest);
		}

		private int SaveFileData(string fileName, string strTemp, IntPtr pData, uint dataLen)
		{
			try
			{
				int num = 1 + (strTemp?.Length ?? 0);
				FileStream fileStream = new FileStream(fileName, FileMode.Create);
				BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				byte[] array = new byte[dataLen - num];
				Marshal.Copy(pData + num, array, 0, (int)(dataLen - num));
				binaryWriter.Write(array, 0, (int)(dataLen - num));
				binaryWriter.Close();
				fileStream.Close();
			}
			catch
			{
				return -1;
			}
			return 0;
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (accessType == PVIReadAccessTypes.ANSL_TransferObject)
			{
				string strTemp = PviMarshal.PtrUpToNullToString(pData, (int)dataLen);
				if (!string.IsNullOrEmpty(((FileTransferRequest)base.CpuParent._TransferRequest).TargetFileName) && errorCode == 0)
				{
					SaveFileData(((FileTransferRequest)base.CpuParent._TransferRequest).TargetFileName, strTemp, pData, dataLen);
				}
			}
			base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
		}

		public override string ToString()
		{
			return base.ToString() + " Class=\"" + Class + "\" Group=\"" + Group + "\" Dir=\"" + Directory + "\" HashValue=\"" + HashValue + "\"";
		}
	}
}
