namespace BR.AN.PviServices
{
	public enum ConnectionStates
	{
		Unininitialized,
		Connecting,
		ConnectedError,
		Connected,
		Disconnecting,
		Disconnected,
		LinkBroken,
		ConnectionChanging,
		ConnectionChanged
	}
}
