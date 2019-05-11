using System.Collections;

namespace BR.AN.PviServices
{
	public class CollectionErrorEventArgs : ErrorEventArgs
	{
		internal ArrayList propNewItems;

		internal ArrayList propChangedItems;

		public ArrayList NewItems => propNewItems;

		public ArrayList ChangedItems => propChangedItems;

		internal CollectionErrorEventArgs(string name, int errorCode, string language, Action actEvent)
			: base(name, name, errorCode, language, actEvent)
		{
			propNewItems = new ArrayList();
			propChangedItems = new ArrayList();
		}
	}
}
