namespace BR.AN.PviServices
{
	public abstract class TransferRequest
	{
		internal readonly uint ObjectId;

		private readonly TransferRequestType _RequestType;

		private readonly string _Name;

		public TransferRequestType RequestType => _RequestType;

		public string Name => _Name;

		protected TransferRequest(TransferRequestType requestType, string nameOfObject)
		{
			_RequestType = requestType;
			_Name = nameOfObject;
			ObjectId = uint.MaxValue;
		}

		protected TransferRequest(TransferRequest copyObject, uint internId)
		{
			_RequestType = copyObject.RequestType;
			_Name = copyObject.Name;
			ObjectId = internId;
		}

		public override string ToString()
		{
			return "Name=\"" + Name + "\"";
		}

		internal abstract TransferRequest Copy(uint internId);
	}
}
