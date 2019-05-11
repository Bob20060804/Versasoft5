namespace BR.AN.PviServices
{
	public abstract class SynchronizableBaseCollection : BaseCollection
	{
		internal bool synchronize;

		internal ModuleListOptions _UploadOption;

		internal bool hasBeenUploadedOnce;

		public bool Synchronize
		{
			get
			{
				return synchronize;
			}
			set
			{
				synchronize = value;
			}
		}

		internal bool isSyncable
		{
			get
			{
				if (!hasBeenUploadedOnce || Count == 0)
				{
					return false;
				}
				return true;
			}
		}

		internal SynchronizableBaseCollection()
		{
			synchronize = false;
			hasBeenUploadedOnce = false;
			_UploadOption = ModuleListOptions.INA2000CompatibleMode;
		}

		internal SynchronizableBaseCollection(object parent, string name)
			: base(parent, name)
		{
			synchronize = false;
			hasBeenUploadedOnce = false;
			_UploadOption = ModuleListOptions.INA2000CompatibleMode;
		}

		internal SynchronizableBaseCollection(CollectionType colType, object parentObj, string name)
			: base(colType, parentObj, name)
		{
			synchronize = false;
			hasBeenUploadedOnce = false;
			_UploadOption = ModuleListOptions.INA2000CompatibleMode;
		}

		internal bool ValidateAdd(object parentObj, int modListed)
		{
			bool flag = parentObj is Cpu && BootMode.Diagnostics == ((Cpu)parentObj).BootMode;
			if (parentObj is Cpu && ((Cpu)parentObj).Connection.DeviceType != DeviceType.ANSLTcp)
			{
				return true;
			}
			if ((_UploadOption == ModuleListOptions.INA2000CompatibleMode && ((flag && 2 == (modListed & 2)) || (!flag && 1 == (modListed & 1)))) || (ModuleListOptions.INA2000List == _UploadOption && 1 == (modListed & 1)) || (ModuleListOptions.INA2000DiagnosisList == _UploadOption && 2 == (modListed & 2)) || ModuleListOptions.All == _UploadOption)
			{
				return true;
			}
			return false;
		}
	}
}
