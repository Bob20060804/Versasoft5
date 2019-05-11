using System.Collections;

namespace BR.AN.PviServices
{
	internal class Comparer : IComparer
	{
		public int Compare(object x, object y)
		{
			if ((ulong)x > (ulong)y)
			{
				return 1;
			}
			if ((ulong)x < (ulong)y)
			{
				return -1;
			}
			return 0;
		}
	}
}
