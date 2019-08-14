using Prism.Events;
using System.ComponentModel.Composition;

namespace Prism.Mef.Events
{
	[Export(typeof(IEventAggregator))]
	public class MefEventAggregator : EventAggregator
	{
	}
}
