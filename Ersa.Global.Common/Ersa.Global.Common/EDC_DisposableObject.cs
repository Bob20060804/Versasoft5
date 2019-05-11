using System;

namespace Ersa.Global.Common
{
	public abstract class EDC_DisposableObject : IDisposable
	{
		private readonly object m_objLock = new object();

		private bool m_blnIstDisposed;

		public bool PRO_blnIstDisposed
		{
			get
			{
				lock (m_objLock)
				{
					return m_blnIstDisposed;
				}
			}
			private set
			{
				lock (m_objLock)
				{
					m_blnIstDisposed = value;
				}
			}
		}

		protected EDC_DisposableObject()
		{
			m_blnIstDisposed = false;
		}

		~EDC_DisposableObject()
		{
			SUB_Dispose(i_blnIsDisposing: false);
		}

		public void Dispose()
		{
			SUB_Dispose(i_blnIsDisposing: true);
			GC.SuppressFinalize(this);
		}

		protected abstract void SUB_InternalDispose();

		private void SUB_Dispose(bool i_blnIsDisposing)
		{
			if (!PRO_blnIstDisposed)
			{
				if (i_blnIsDisposing)
				{
					SUB_InternalDispose();
				}
				PRO_blnIstDisposed = true;
			}
		}
	}
}
