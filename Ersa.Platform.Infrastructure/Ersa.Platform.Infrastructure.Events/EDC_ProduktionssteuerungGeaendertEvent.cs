using Prism.Events;

namespace Ersa.Platform.Infrastructure.Events
{
	/// <summary>
	/// 生产控制变更事件
	/// </summary>
	public class EDC_ProduktionssteuerungGeaendertEvent : PubSubEvent<EDC_ProduktionssteuerungGeaendertPayload>
	{
	}
}
