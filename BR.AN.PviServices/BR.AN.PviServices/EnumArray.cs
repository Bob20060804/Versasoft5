using System.Collections;

namespace BR.AN.PviServices
{
	public class EnumArray
	{
		private ArrayList propItems;

		private ArrayList propNames;

		private ArrayList propValues;

		private string propName;

		public string Name => propName;

		public ArrayList Names => propNames;

		public ArrayList Values => propValues;

		public EnumBase this[int index]
		{
			get
			{
				return (EnumBase)propItems[index];
			}
		}

		public int Count => propItems.Count;

		internal EnumArray(string name)
		{
			propItems = new ArrayList();
			propNames = new ArrayList();
			propValues = new ArrayList();
			propName = name;
		}

		internal EnumArray Clone()
		{
			EnumArray enumArray = new EnumArray(propName);
			enumArray.propItems = (ArrayList)propItems.Clone();
			enumArray.propNames = (ArrayList)propNames.Clone();
			enumArray.propValues = (ArrayList)propNames.Clone();
			return enumArray;
		}

		internal void Clear()
		{
			propItems.Clear();
			propNames.Clear();
			propValues.Clear();
		}

		internal int AddEnum(EnumBase enumVal)
		{
			int num = 0;
			for (num = 0; num < propItems.Count; num++)
			{
				if (((EnumBase)propItems[num]).Name.CompareTo(enumVal.Name) == 0)
				{
					return -1;
				}
			}
			if (enumVal.Value == null)
			{
				enumVal.SetEnumValue(propValues[propValues.Count - 1]);
			}
			propNames.Add(enumVal.Name);
			propValues.Add(enumVal.Value);
			return propItems.Add(enumVal);
		}

		public object EnumValue(string name)
		{
			int num = 0;
			for (num = 0; num < propNames.Count; num++)
			{
				if (name.CompareTo(propNames[num].ToString()) == 0)
				{
					return propValues[num];
				}
			}
			return null;
		}

		public string EnumName(object value)
		{
			int num = 0;
			for (num = 0; num < propValues.Count; num++)
			{
				if (value.ToString().CompareTo(propValues[num].ToString()) == 0)
				{
					return (string)propNames[num];
				}
			}
			return null;
		}

		public virtual string ToPviString()
		{
			string text = "";
			for (int i = 0; i < propValues.Count; i++)
			{
				if (i == 0)
				{
					string text2 = text;
					text = text2 + "e," + propValues[i].ToString() + "," + propNames[i].ToString();
				}
				else
				{
					string text3 = text;
					text = text3 + ";e," + propValues[i].ToString() + "," + propNames[i].ToString();
				}
			}
			return text;
		}

		public override string ToString()
		{
			return ToPviString();
		}
	}
}
