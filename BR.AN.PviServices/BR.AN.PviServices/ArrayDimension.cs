namespace BR.AN.PviServices
{
	public class ArrayDimension
	{
		protected int propStartIdx;

		protected int propEndIdx;

		public int StartIndex => propStartIdx;

		public int EndIndex => propEndIdx;

		public int NumOfElements => propEndIdx - propStartIdx + 1;

		internal ArrayDimension(int endIdx, int startIdx)
		{
			propStartIdx = startIdx;
			propEndIdx = endIdx;
		}

		internal ArrayDimension(ArrayDimension cloneItem)
		{
			propStartIdx = cloneItem.propStartIdx;
			propEndIdx = cloneItem.propEndIdx;
		}

		internal ArrayDimension(int endIdx)
		{
			propStartIdx = 0;
			propEndIdx = endIdx;
		}

		public override string ToString()
		{
			return "a," + propStartIdx.ToString() + "," + propEndIdx.ToString();
		}
	}
}
