using System;
using System.Windows.Input;

namespace Ersa.Platform.UI
{
	public class EDC_WaitCursor : IDisposable
	{
		private readonly Cursor m_fdcAlterCursor;

		public EDC_WaitCursor()
		{
			m_fdcAlterCursor = Mouse.OverrideCursor;
			Mouse.OverrideCursor = Cursors.Wait;
		}

		public void Dispose()
		{
			Mouse.OverrideCursor = m_fdcAlterCursor;
		}
	}
}
