using System;

namespace Prism.Regions
{
	public class NavigationResult
	{
		public bool? Result
		{
			get;
			private set;
		}

		public Exception Error
		{
			get;
			private set;
		}

		public NavigationContext Context
		{
			get;
			private set;
		}

		public NavigationResult(NavigationContext context, bool? result)
		{
			Context = context;
			Result = result;
		}

		public NavigationResult(NavigationContext context, Exception error)
		{
			Context = context;
			Error = error;
			Result = false;
		}
	}
}
