using System;

namespace Ersa.Global.Common
{
	public static class EDC_Disposable
	{
		private class EDC_Subscription : EDC_DisposableObject
		{
			private readonly Action m_delUnsubscribeAction;

			public EDC_Subscription(Action i_delUnsubscribeAction)
			{
				m_delUnsubscribeAction = i_delUnsubscribeAction;
			}

			protected override void SUB_InternalDispose()
			{
				if (m_delUnsubscribeAction != null)
				{
					m_delUnsubscribeAction();
				}
			}
		}

		public static IDisposable FUN_fdcCreate(Action i_delAction)
		{
			return new EDC_Subscription(i_delAction);
		}

		public static IDisposable FUN_fdcEmpty()
		{
			return new EDC_Subscription(delegate
			{
			});
		}
	}
}
