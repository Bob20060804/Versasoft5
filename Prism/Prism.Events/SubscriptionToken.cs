using System;

namespace Prism.Events
{
	public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable
	{
		private readonly Guid _token;

		private Action<SubscriptionToken> _unsubscribeAction;

		public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction)
		{
			_unsubscribeAction = unsubscribeAction;
			_token = Guid.NewGuid();
		}

		public bool Equals(SubscriptionToken other)
		{
			if (other == null)
			{
				return false;
			}
			return object.Equals(_token, other._token);
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			return Equals(obj as SubscriptionToken);
		}

		public override int GetHashCode()
		{
			return _token.GetHashCode();
		}

		public virtual void Dispose()
		{
			if (_unsubscribeAction != null)
			{
				_unsubscribeAction(this);
				_unsubscribeAction = null;
			}
			GC.SuppressFinalize(this);
		}
	}
}
