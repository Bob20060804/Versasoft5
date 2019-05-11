using System;
using System.Collections;

namespace BR.AN.PviServices
{
	public class MemberCollection : BaseCollection
	{
		private Hashtable mapNameToIndex;

		private int datavalidCount;

		internal int CountExtended
		{
			get
			{
				int num = 0;
				int num2 = 0;
				if (0 < Count)
				{
					foreach (Variable value in Values)
					{
						if (value.Members != null && value.Members.Count > 0)
						{
							num += value.Members.CountExtended * value.propPviValue.ArrayLength;
						}
						if (value.propPviValue.ArrayLength > 1)
						{
							num += value.propPviValue.ArrayLength;
						}
					}
					foreach (Variable value2 in Values)
					{
						if (value2.propPviValue.DataType != DataType.Structure && value2.propPviValue.ArrayLength < 2)
						{
							num2++;
						}
					}
				}
				return (num + num2) * ((Variable)propParent).propPviValue.ArrayLength;
			}
		}

		public Variable this[int index]
		{
			get
			{
				return (Variable)propArrayList[index];
			}
		}

		internal Variable FirstSimpleTyped
		{
			get
			{
				Variable variable = null;
				if (0 < propArrayList.Count)
				{
					variable = (Variable)propArrayList[0];
					if (1 < variable.propPviValue.ArrayLength && DataType.Structure == variable.propPviValue.DataType)
					{
						variable = ((Variable)propArrayList[0]).Members.FirstSimpleTyped;
					}
				}
				return variable;
			}
		}

		public Variable First
		{
			get
			{
				if (0 < propArrayList.Count)
				{
					return (Variable)propArrayList[0];
				}
				return null;
			}
		}

		public Variable this[string name]
		{
			get
			{
				if (mapNameToIndex.ContainsKey(name))
				{
					return (Variable)propArrayList[(int)mapNameToIndex[name]];
				}
				return null;
			}
		}

		public override Service Service
		{
			get
			{
				if (propParent is Variable)
				{
					return ((Variable)propParent).Service;
				}
				return null;
			}
		}

		public bool DataValid
		{
			get
			{
				if (Count == datavalidCount)
				{
					return true;
				}
				return false;
			}
		}

		internal MemberCollection(Variable parent, string address)
			: base(CollectionType.ArrayList, parent, address)
		{
			mapNameToIndex = new Hashtable();
		}

		internal MemberCollection()
			: base(CollectionType.ArrayList, null, null)
		{
			mapNameToIndex = new Hashtable();
		}

		public override void Connect()
		{
			propCounter = 0;
			datavalidCount = 0;
			propErrorCount = 0;
			if (((Variable)propParent).IsConnected && 0 < Count)
			{
				foreach (Variable value in Values)
				{
					value.Connect();
					value.Connected += MemberConnected;
					value.Error += MemberError;
					value.DataValidated += MemberDataValid;
				}
			}
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				object propParent = base.propParent;
				object propUserData = base.propUserData;
				string propName = base.propName;
				CleanUp(disposing);
				base.propParent = propParent;
				base.propUserData = propUserData;
				base.propName = propName;
				base.Dispose(disposing, removeFromCollection);
				base.propParent = null;
				base.propUserData = null;
				base.propName = null;
			}
		}

		internal void CleanUp(bool disposing)
		{
			ArrayList arrayList = new ArrayList();
			propCounter = 0;
			try
			{
				if (0 < Count)
				{
					foreach (Variable value in Values)
					{
						if (!value.ContainedInParentCollection())
						{
							arrayList.Add(value);
							if (value.LinkId != 0)
							{
								value.Disconnect(0u);
							}
						}
					}
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					object obj = arrayList[i];
					((Variable)obj).Dispose(disposing, removeFromCollection: true);
					obj = null;
				}
				Clear();
			}
			catch (OutOfMemoryException ex)
			{
				string message = ex.Message;
			}
		}

		public void Disconnect()
		{
			propCounter = 0;
			if (0 < Count)
			{
				foreach (Variable value in Values)
				{
					value.Disconnected += MemberDisconnected;
					value.Disconnect();
				}
			}
		}

		private void MemberConnected(object sender, PviEventArgs e)
		{
			if (mapNameToIndex == null)
			{
				mapNameToIndex = new Hashtable();
			}
			propCounter++;
			((Variable)sender).Connected -= MemberConnected;
			int propCounter = propCounter;
			int count = propArrayList.Count;
		}

		private void MemberDisconnected(object sender, PviEventArgs e)
		{
			propCounter++;
			((Variable)sender).Disconnected -= MemberDisconnected;
			int propCounter = propCounter;
			int count = Count;
		}

		private void MemberError(object sender, PviEventArgs e)
		{
			propErrorCount++;
			((Variable)sender).Error -= MemberError;
		}

		private void MemberDataValid(object sender, PviEventArgs e)
		{
			datavalidCount++;
			((Variable)sender).DataValidated -= MemberDataValid;
		}

		internal virtual void MoveToFirst()
		{
			propEnumer = GetEnumerator();
		}

		internal virtual Variable GetNext()
		{
			if (propEnumer.MoveNext())
			{
				return (Variable)propEnumer.Current;
			}
			return null;
		}

		public override void Clear()
		{
			base.Clear();
			mapNameToIndex.Clear();
		}

		public override void Remove(string key)
		{
			if (mapNameToIndex.ContainsKey(key))
			{
				base.Remove(mapNameToIndex[key]);
				mapNameToIndex.Remove(key);
			}
		}

		internal virtual int Add(Variable member)
		{
			if (CollectionType.ArrayList == propCollectionType && !mapNameToIndex.ContainsKey(member.Name))
			{
				mapNameToIndex.Add(member.Name, propArrayList.Count);
				propArrayList.Add(member);
			}
			return 0;
		}

		public override bool ContainsKey(object key)
		{
			return mapNameToIndex.ContainsKey(key);
		}

		internal virtual int CopyTo(MemberCollection collection)
		{
			if (collection != null)
			{
				collection.propArrayList = new ArrayList(propArrayList);
			}
			return 0;
		}
	}
}
