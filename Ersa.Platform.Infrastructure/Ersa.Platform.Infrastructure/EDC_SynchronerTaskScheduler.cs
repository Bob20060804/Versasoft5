using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Infrastructure
{
	public class EDC_SynchronerTaskScheduler : TaskScheduler
	{
		public override int MaximumConcurrencyLevel => 1;

		protected override void QueueTask(Task i_fdcTask)
		{
			TryExecuteTask(i_fdcTask);
		}

		protected override bool TryExecuteTaskInline(Task i_fdcTask, bool i_blnTaskWasPreviouslyQueued)
		{
			return TryExecuteTask(i_fdcTask);
		}

		protected override IEnumerable<Task> GetScheduledTasks()
		{
			return Enumerable.Empty<Task>();
		}
	}
}
