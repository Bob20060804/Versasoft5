using System;

namespace BR.AN.PviServices
{
	public abstract class TargetObjectBase : PviCBEvents, IDisposable
	{
		private bool _Disposed;

		private TargetObjectType _Type;

		private Cpu _ParentOject;

		public TargetObjectType Type => _Type;

		internal Cpu CpuParent => _ParentOject;

		public event DisposeEventHandler Disposing;

		internal TargetObjectBase(TargetObjectType objType, Cpu parentObj)
		{
			InitTargetObjectBase(objType, parentObj);
		}

		private void InitTargetObjectBase(TargetObjectType objType, Cpu parentObj)
		{
			_Type = objType;
			_ParentOject = parentObj;
			if (_ParentOject != null && _ParentOject.Service != null)
			{
				_ParentOject.Service.AddID(this, ref _internId);
				_Disposed = false;
			}
		}

		~TargetObjectBase()
		{
			if (_ParentOject != null && _ParentOject.Service != null)
			{
				_ParentOject.Service.AddID(this, ref _internId);
				Dispose(disposing: false);
			}
		}

		private void DetachFromPviCallBacks()
		{
			if (_ParentOject != null && _ParentOject.Service != null)
			{
				_ParentOject.Service.RemoveID(_internId);
			}
		}

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		public void Dispose()
		{
			if (!_Disposed)
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing)
		{
			if (!_Disposed)
			{
				DetachFromPviCallBacks();
				OnDisposing(disposing);
				if (disposing)
				{
					_Type = TargetObjectType.Undefined;
					_ParentOject = null;
					_Disposed = true;
				}
			}
		}

		public override string ToString()
		{
			return "InfoType=\"" + _Type + "\"";
		}
	}
}
