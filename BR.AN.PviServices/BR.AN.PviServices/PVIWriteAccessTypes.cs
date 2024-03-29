namespace BR.AN.PviServices
{
	internal enum PVIWriteAccessTypes
	{
		EventMask = 5,
		Connection = 10,
		Data = 11,
		State = 12,
		BasicAttributes = 13,
		ExtendedAttributes = 14,
		Refresh = 0xF,
		Hysteresis = 0x10,
		ConversionFunction = 18,
		Download = 21,
		DateNTime = 22,
		Delete_PLC_Memory = 23,
		StreamDownLoad = 27,
		CpuModuleDelete = 29,
		Cancel = 0x80,
		UserTag = 129,
		Snapshot = 240,
		WritePhysicalMemory = 260,
		TTService = 268,
		DeleteDiagModule = 274,
		StartModule = 276,
		StopModule = 277,
		ResumeModule = 278,
		BurnModule = 279,
		DeleteModule = 280,
		ClearMemory = 281,
		ForceOn = 282,
		ForceOff = 283,
		SavePath = 290,
		GlobalForceOFF = 297,
		ANSL_RedundancyControl = 440,
		ANSL_TracePointsRegister = 450,
		ANSL_TracePointsUnregister = 451,
		ANSL_COMMAND_Data = 460
	}
}
