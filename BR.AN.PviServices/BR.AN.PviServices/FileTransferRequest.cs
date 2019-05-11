namespace BR.AN.PviServices
{
	public class FileTransferRequest : TransferRequest
	{
		internal bool _LoadFromTarget;

		private string _TargetFileName;

		private string _Class;

		private string _Group;

		private string _Directory;

		public string TargetFileName => _TargetFileName;

		public string Class => _Class;

		public string Group => _Group;

		public string Directory => _Directory;

		public FileTransferRequest(string targetFileName, string nameOfObject)
			: base(TransferRequestType.File, nameOfObject)
		{
			Init(targetFileName, null, null, null);
		}

		internal FileTransferRequest(FileTransferRequest copyObject, uint internId)
			: base(copyObject, internId)
		{
			Init(copyObject.TargetFileName, copyObject.Class, copyObject.Group, copyObject.Directory);
		}

		public FileTransferRequest(string targetFileName, string nameOfObject, string arClass)
			: base(TransferRequestType.File, nameOfObject)
		{
			Init(targetFileName, arClass, null, null);
		}

		public FileTransferRequest(string targetFileName, string nameOfObject, string arClass, string arGroup)
			: base(TransferRequestType.File, nameOfObject)
		{
			Init(targetFileName, arClass, arGroup, null);
		}

		public FileTransferRequest(string targetFileName, string nameOfObject, string arClass, string arGroup, string arDirectory)
			: base(TransferRequestType.File, nameOfObject)
		{
			Init(targetFileName, arClass, arGroup, arDirectory);
		}

		private void Init(string targetFileName, string arClass, string arGroup, string arDirectory)
		{
			_TargetFileName = targetFileName;
			_Class = arClass;
			_Group = arGroup;
			_Directory = arDirectory;
			_LoadFromTarget = true;
		}

		public override string ToString()
		{
			string text = base.ToString();
			if (!string.IsNullOrEmpty(Class))
			{
				text = text + " Class=\"" + Class + "\"";
			}
			if (!string.IsNullOrEmpty(Group))
			{
				text = text + " Group=\"" + Group + "\"";
			}
			if (!string.IsNullOrEmpty(Directory))
			{
				text = text + " Dir=\"" + Directory + "\"";
			}
			return text;
		}

		internal override TransferRequest Copy(uint internId)
		{
			return new FileTransferRequest(this, internId);
		}
	}
}
