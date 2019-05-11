using BR.AN.PviServices.ObjectList;

namespace BR.AN.PviServices
{
	public class DirectoryObject : TargetObjectBase
	{
		private readonly string _Class;

		private readonly string _Group;

		private readonly string _Directory;

		public string Class => _Class;

		public string Group => _Group;

		public string Directory => _Directory;

		internal DirectoryObject(Cpu parentObj, DirInfo dirInfo)
			: base(TargetObjectType.Directory, parentObj)
		{
			_Class = dirInfo.Class;
			_Group = dirInfo.Group;
			_Directory = dirInfo.Dir;
		}

		public override string ToString()
		{
			return base.ToString() + " Class=\"" + Class + "\" Dir=\"" + Directory + "\" Group=\"" + Group + "\"";
		}
	}
}
