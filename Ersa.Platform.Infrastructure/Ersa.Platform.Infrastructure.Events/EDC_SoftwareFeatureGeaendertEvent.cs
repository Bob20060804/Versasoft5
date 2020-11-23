using Prism.Events;
using System.Collections.Generic;

namespace Ersa.Platform.Infrastructure.Events
{
	/// <summary>
	/// 软件功能更改事件
	/// </summary>
	public class EDC_SoftwareFeatureGeaendertEvent : PubSubEvent<List<EDC_SoftwareFeatureGeaendertPayload>>
	{
	}
}
