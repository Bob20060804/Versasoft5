using BR.AN.PviServices.ObjectList;

namespace BR.AN.PviServices
{
	public class ModuleObject : TransferObject
	{
		private int _MemType;

		private int _Revision;

		private int _ModuleType;

		private int _Version;

		public int MemType => _MemType;

		public int Revision => _Revision;

		public int ModuleType => _ModuleType;

		public int Version => _Version;

		internal ModuleObject(Cpu parentObj, ModInfo modInfo)
			: base(TargetObjectType.Module, parentObj, modInfo)
		{
			InitMember(modInfo);
		}

		internal ModuleObject(TargetObjectType objType, Cpu parentObj, ModInfo modInfo)
			: base(TargetObjectType.Module | objType, parentObj, modInfo)
		{
			InitMember(modInfo);
		}

		private void InitMember(ModInfo modInfo)
		{
			_MemType = modInfo.MemType;
			_Revision = modInfo.Revision;
			_ModuleType = modInfo.Type;
			_Version = modInfo.Version;
		}

		public override string ToString()
		{
			return base.ToString() + " Type=\"" + ModuleType + "\" MemType=\"" + MemType + "\" Revision=\"" + Revision + "\" Version=\"" + Version + "\"";
		}
	}
}
