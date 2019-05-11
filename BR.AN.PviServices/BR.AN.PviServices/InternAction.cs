namespace BR.AN.PviServices
{
	internal enum InternAction
	{
		LinkForHysteresis = 2712,
		LinkForScaling = 2713,
		UnlinkForHysteresis = 2714,
		UnlinkForScaling = 2715,
		UnlinkForSetDataType = 2716,
		UnlinkForGetDataType = 2717,
		LinkForSetDataType = 2718,
		LineConnect = 2800,
		LineDisconnect = 2801,
		LineEvent = 2802,
		DeviceConnect = 2803,
		DeviceDisconnect = 2804,
		DeviceEvent = 2805,
		StationConnect = 2806,
		StationDisconnect = 2807,
		StationEvent = 2808,
		VariableReadInternal = 2809,
		VariableReadFormatInternal = 2810,
		VariableExtendedTypInfo = 2811,
		VariableReadStatus = 2812,
		VariableUnlinkStructConnect = 2813,
		LinkForUpload = 2820,
		UnlinkForUpload = 2821,
		ChangeConnection = 3000,
		ChangeStationConnection = 3001,
		ChangeDeviceConnection = 3002,
		ChangeLineConnection = 3003,
		LoggerReadInfo = 4460
	}
}
