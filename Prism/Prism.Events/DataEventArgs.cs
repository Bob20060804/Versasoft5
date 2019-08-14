using System;

namespace Prism.Events
{
	public class DataEventArgs<TData> : EventArgs
	{
		private readonly TData _value;

		public TData Value => _value;

		public DataEventArgs(TData value)
		{
			_value = value;
		}
	}
}
