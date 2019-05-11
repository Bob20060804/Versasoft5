using System.Collections;

namespace BR.AN.PviServices
{
	public class StructMemberCollection : HashtableArray
	{
		protected int propCount;

		public Variable this[string name]
		{
			get
			{
				if (-1 != propCount)
				{
					return null;
				}
				return (Variable)base[name];
			}
		}

		public new object this[int index]
		{
			get
			{
				if (-1 != propCount)
				{
					if (propCount > index)
					{
						return "[" + index.ToString() + "]";
					}
					return -1;
				}
				return (Variable)base[index];
			}
		}

		public bool IsVirtual
		{
			get
			{
				if (-1 != propCount)
				{
					return true;
				}
				return false;
			}
		}

		public override int Count
		{
			get
			{
				if (-1 != propCount)
				{
					return propCount;
				}
				return base.Count;
			}
		}

		public StructMemberCollection()
		{
			propCount = -1;
		}

		public StructMemberCollection(int count)
		{
			propCount = count;
		}

		internal void CleanUp(bool disposing)
		{
			int num = 0;
			for (num = 0; num < Count; num++)
			{
				object obj = this[num];
				if (obj is Variable)
				{
					if (((Variable)obj).LinkId != 0)
					{
						((Variable)obj).Disconnect(0u);
					}
					if (((Variable)obj).StructureMembers != null)
					{
						((Variable)obj).StructureMembers.CleanUp(disposing);
					}
					((Variable)obj).Dispose(disposing, removeFromCollection: true);
				}
			}
			Clear();
		}

		public override object Clone()
		{
			StructMemberCollection structMemberCollection = new StructMemberCollection();
			structMemberCollection.propHashTable = (Hashtable)propHashTable.Clone();
			structMemberCollection.propArrayList = (ArrayList)propArrayList.Clone();
			structMemberCollection.propCount = propCount;
			return structMemberCollection;
		}
	}
}
