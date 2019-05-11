using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class ArrayDimensionArray
	{
		private ArrayList propItems;

		public int Count => propItems.Count;

		public ArrayDimension this[int index]
		{
			get
			{
				return (ArrayDimension)propItems[index];
			}
		}

		internal ArrayDimensionArray()
		{
			propItems = new ArrayList();
		}

		internal void Clear()
		{
			propItems.Clear();
		}

		internal ArrayDimensionArray Clone()
		{
			ArrayDimensionArray arrayDimensionArray = new ArrayDimensionArray();
			for (int i = 0; i < propItems.Count; i++)
			{
				arrayDimensionArray.Add(new ArrayDimension((ArrayDimension)propItems[i]));
			}
			return arrayDimensionArray;
		}

		internal int Add(params string[] dims)
		{
			if (2 < dims.Length)
			{
				return propItems.Add(new ArrayDimension(Convert.ToInt32(dims.GetValue(2).ToString()), Convert.ToInt32(dims.GetValue(1).ToString())));
			}
			if (1 < dims.Length)
			{
				return propItems.Add(new ArrayDimension(Convert.ToInt32(dims.GetValue(1).ToString())));
			}
			return -1;
		}

		internal int Add(ArrayDimension arrayDimItem)
		{
			return propItems.Add(arrayDimItem);
		}

		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < propItems.Count; i++)
			{
				text = ((i != 0) ? (text + ";" + propItems[i].ToString()) : propItems[i].ToString());
			}
			return text;
		}
	}
}
