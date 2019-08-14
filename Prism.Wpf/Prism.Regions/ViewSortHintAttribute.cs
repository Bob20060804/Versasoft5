using System;

namespace Prism.Regions
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ViewSortHintAttribute : Attribute
	{
		public string Hint
		{
			get;
			private set;
		}

		public ViewSortHintAttribute(string hint)
		{
			Hint = hint;
		}
	}
}
