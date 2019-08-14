namespace Prism.Logging
{
	public interface ILoggerFacade
	{
		void Log(string message, Category category, Priority priority);
	}
}
