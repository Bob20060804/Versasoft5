namespace BR.AN.PviServices
{
	public class LogicalCollection : BaseCollection
	{
		public object this[string name]
		{
			get
			{
				if (propCollectionType == CollectionType.HashTable)
				{
					return base[name];
				}
				return null;
			}
		}

		public override Service Service
		{
			get
			{
				if (propParent is Service)
				{
					return (Service)propParent;
				}
				return null;
			}
		}

		public LogicalCollection(object parent, string name)
			: base(parent, name)
		{
		}

		public override void Connect()
		{
			propCounter = 0;
			propErrorCount = 0;
			if (propCollectionType == CollectionType.HashTable && 0 < Count)
			{
				foreach (object value in Values)
				{
					if (value is Variable)
					{
						((Variable)value).Connected += LogicalObjectConnected;
						((Variable)value).Connect();
					}
					else if (value is Task)
					{
						((Task)value).Connected += LogicalObjectConnected;
						((Task)value).Connect();
					}
					else if (value is Cpu)
					{
						((Cpu)value).Connected += LogicalObjectConnected;
						((Cpu)value).Connect();
					}
				}
			}
		}

		public void Disconnect()
		{
			if (propCollectionType == CollectionType.HashTable && 0 < Count)
			{
				foreach (object value in Values)
				{
					if (value is Variable)
					{
						((Variable)value).Disconnected += LogicalObjectDisconnected;
						((Variable)value).Disconnect();
					}
					else if (value is Task)
					{
						((Task)value).Disconnected += LogicalObjectDisconnected;
						((Task)value).Disconnect();
					}
					else if (value is Cpu)
					{
						((Cpu)value).Disconnected += LogicalObjectDisconnected;
						((Cpu)value).Disconnect();
					}
				}
			}
		}

		private void LogicalObjectConnected(object sender, PviEventArgs e)
		{
			propCounter++;
			if (sender is Variable)
			{
				((Variable)sender).Connected -= LogicalObjectConnected;
			}
			else if (sender is Task)
			{
				((Task)sender).Connected -= LogicalObjectConnected;
			}
			int propCounter = propCounter;
			int count = Count;
		}

		private void LogicalObjectDisconnected(object sender, PviEventArgs e)
		{
			propCounter++;
			if (sender is Variable)
			{
				((Variable)sender).Disconnected -= LogicalObjectDisconnected;
			}
			else if (sender is Task)
			{
				((Task)sender).Disconnected -= LogicalObjectDisconnected;
			}
			int propCounter = propCounter;
			int count = Count;
		}
	}
}
