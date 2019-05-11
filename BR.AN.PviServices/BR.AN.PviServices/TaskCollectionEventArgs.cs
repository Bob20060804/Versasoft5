namespace BR.AN.PviServices
{
	public class TaskCollectionEventArgs : CollectionEventArgs
	{
		private TaskCollection propTasks;

		public TaskCollection Tasks => propTasks;

		public TaskCollectionEventArgs(string name, string address, int error, string language, Action action, TaskCollection tasks)
			: base(name, address, error, language, action, tasks)
		{
			propTasks = tasks;
		}
	}
}
