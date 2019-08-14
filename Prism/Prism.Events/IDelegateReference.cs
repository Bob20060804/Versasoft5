using System;

namespace Prism.Events
{
	public interface IDelegateReference
	{
		Delegate Target
		{
			get;
		}
	}
}
