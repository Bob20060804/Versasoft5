using System;

namespace BR.AN.PviServices
{
	[Flags]
	internal enum Actions : uint
	{
		NONE = 0x0,
		Connect = 0x1,
		GetList = 0x2,
		GetValue = 0x4,
		SetValue = 0x8,
		GetForce = 0x10,
		SetForce = 0x20,
		Start = 0x40,
		Stop = 0x80,
		RunCycle = 0x100,
		GetType = 0x200,
		SetType = 0x400,
		GetLength = 0x800,
		SetRefresh = 0x1000,
		CreateLink = 0x2000,
		Upload = 0x4000,
		Download = 0x8000,
		SetActive = 0x10000,
		SetHysteresis = 0x20000,
		Disconnect = 0x40000,
		SetInitValue = 0x80000,
		GetLBType = 0x100000,
		ModuleInfo = 0x200000,
		ListSNMPVariables = 0x400000,
		Connected = 0x800000,
		Uploading = 0x1000000,
		GetCpuInfo = 0x2000000,
		ReadPVFormat = 0x4000000,
		FireActivated = 0x8000000,
		ReadIndex = 0x10000000,
		FireDataValidated = 0x20000000,
		FireValueChanged = 0x40000000,
		Link = 0x80000000
	}
}
