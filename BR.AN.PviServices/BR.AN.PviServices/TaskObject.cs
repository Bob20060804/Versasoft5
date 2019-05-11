using BR.AN.PviServices.ObjectList;

namespace BR.AN.PviServices
{
	public class TaskObject : ModuleObject
	{
		private readonly string _InstallOrder;

		private readonly int _TaskClass;

		private readonly int _TaskState;

		public string InstallOrder => _InstallOrder;

		public int TaskClass => _TaskClass;

		public int TaskState => _TaskState;

		internal TaskObject(Cpu parentObj, ModInfo modInfo)
			: base(TargetObjectType.Task, parentObj, modInfo)
		{
			_InstallOrder = modInfo.InstallOrder;
			_TaskClass = modInfo.TaskClass;
			_TaskState = modInfo.TaskState;
		}

		public override string ToString()
		{
			return base.ToString() + " InstallOrder=\"" + InstallOrder + "\" TaskClass=\"" + TaskClass + "\" TaskState=\"" + TaskState + "\"";
		}
	}
}
