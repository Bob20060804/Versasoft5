namespace Prism.Logging
{
	public class EmptyLogger : ILoggerFacade
	{
		public void Log(string message, Category category, Priority priority)
		{
		}
	}
}
