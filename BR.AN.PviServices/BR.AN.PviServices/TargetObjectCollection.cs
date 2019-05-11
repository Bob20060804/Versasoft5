using BR.AN.PviServices.ObjectList;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BR.AN.PviServices
{
	public class TargetObjectCollection : PviCBEvents, IDisposable, ICollection<TargetObjectBase>, IEnumerable<TargetObjectBase>, IEnumerable
	{
		private bool _disposed;

		private readonly List<TargetObjectBase> _items;

		private readonly Cpu _cpuParent;

		public int Count => _items.Count;

		public bool IsReadOnly => true;

		public TargetObjectBase this[int indexer]
		{
			get
			{
				return _items[indexer];
			}
		}

		public event DisposeEventHandler Disposing;

		public event PviEventHandler ListRead;

		internal TargetObjectCollection(Cpu cpuParent)
		{
			_cpuParent = cpuParent;
			_items = new List<TargetObjectBase>();
			_disposed = true;
			if (_cpuParent != null)
			{
				_cpuParent.Service.AddID(this, ref _internId);
			}
		}

		public void Dispose()
		{
			if (_cpuParent != null)
			{
				_cpuParent._TransferRequest = null;
				_cpuParent.Service.RemoveID(_internId);
			}
			if (!_disposed)
			{
				Dispose(disposing: true, removeFromCollection: false);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!_disposed)
			{
				OnDisposing(disposing);
				if (disposing)
				{
					_disposed = true;
				}
			}
		}

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		public void Add(TargetObjectBase itemToAdd)
		{
			_items.Add(itemToAdd);
		}

		public void Clear()
		{
			_items.Clear();
		}

		public bool Contains(TargetObjectBase searchItem)
		{
			return _items.Contains(searchItem);
		}

		public void CopyTo(TargetObjectBase[] array, int arrayIndex)
		{
			_items.CopyTo(array, arrayIndex);
		}

		public bool Remove(TargetObjectBase itemToRemove)
		{
			return _items.Remove(itemToRemove);
		}

		public IEnumerator<TargetObjectBase> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		private void OnListRead(int statusCode)
		{
			if (this.ListRead != null)
			{
				this.ListRead(this, new PviEventArgs(_cpuParent.Name, _cpuParent.Address, statusCode, _cpuParent.Service.Language, Action.CpuReadObjectList));
			}
		}

		public int ReadList(string filterOptions)
		{
			int num = 0;
			IntPtr hMemory = IntPtr.Zero;
			int dataLen = 0;
			if (DeviceType.ANSLTcp == _cpuParent.Connection.DeviceType)
			{
				if (filterOptions != null && 0 < filterOptions.Length)
				{
					dataLen = filterOptions.Length;
					hMemory = PviMarshal.StringToHGlobal(filterOptions);
				}
				num = PInvokePvicom.PviComReadArgumentRequest(_cpuParent.Service, _cpuParent.LinkId, AccessTypes.ANSL_ReadObjectList, hMemory, dataLen, 430u, _internId);
				if (IntPtr.Zero != hMemory)
				{
					PviMarshal.FreeHGlobal(ref hMemory);
				}
			}
			else
			{
				num = 12058;
			}
			return num;
		}

		internal void OnObjectTransfering(int percentComplete, int errorCode)
		{
			if (_cpuParent._TransferRequest != null)
			{
				TransferObject transferObject = (TransferObject)_cpuParent.Service.GetObjectForId(_cpuParent._TransferRequest.ObjectId);
				if (transferObject != null && ((FileTransferRequest)_cpuParent._TransferRequest)._LoadFromTarget)
				{
					transferObject.OnUpLoading(percentComplete, errorCode);
				}
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (accessType == PVIReadAccessTypes.ANSL_ReadObjectList)
			{
				LoadFromResponseData(PviMarshal.PtrToStringAnsi(pData, dataLen));
				OnListRead(errorCode);
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		private int LoadFromResponseData(string utf8Data)
		{
			ObjectListInfo objectListInfo = utf8Data.ToObject<ObjectListInfo>();
			if (objectListInfo == null)
			{
				return -1;
			}
			_items.Clear();
			if (objectListInfo.ModInfo != null)
			{
				ModInfo[] modInfo = objectListInfo.ModInfo;
				foreach (ModInfo modInfo2 in modInfo)
				{
					if (-1 == modInfo2.TaskClass)
					{
						_items.Add(new ModuleObject(_cpuParent, modInfo2));
					}
					else
					{
						_items.Add(new TaskObject(_cpuParent, modInfo2));
					}
				}
			}
			if (objectListInfo.FileInfo != null)
			{
				FileInfo[] fileInfo = objectListInfo.FileInfo;
				foreach (FileInfo fileInfo2 in fileInfo)
				{
					_items.Add(new FileObject(_cpuParent, fileInfo2));
				}
			}
			if (objectListInfo.DirInfo != null)
			{
				DirInfo[] dirInfo = objectListInfo.DirInfo;
				foreach (DirInfo dirInfo2 in dirInfo)
				{
					_items.Add(new DirectoryObject(_cpuParent, dirInfo2));
				}
			}
			return 0;
		}

		public string ToXmlString()
		{
			string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ObjectList>";
			for (int i = 0; i < Count; i++)
			{
				str = str + "<Item " + this[i].ToString() + "/>";
			}
			return str + "</ObjectList>";
		}
	}
}
