namespace BR.AN.PviServices
{
	public enum EventDataTypes
	{
		Undefined = 0,
		ASCIIStrings = 1,
		ANSIStrings = 0x10,
		UInt8 = 0x40,
		Int8 = 65,
		UInt16 = 66,
		Int16 = 67,
		UInt32 = 68,
		Int32 = 69,
		UInt64 = 70,
		Int64 = 71,
		UInt8BE = 72,
		Int8BE = 73,
		UInt16BE = 74,
		Int16BE = 75,
		UInt32BE = 76,
		Int32BE = 77,
		UInt64BE = 78,
		Int64BE = 79,
		AsciiChar = 80,
		AnsiChar = 81,
		UTF16Char = 82,
		UTF16CharBE = 83,
		UTF32Char = 84,
		UTF32CharBE = 85,
		BooleanFalse = 96,
		BooleanTrue = 97,
		MemAddress = 100,
		MemAddressBE = 101,
		Float32 = 103,
		Float32BE = 104,
		Double64 = 105,
		Double64BE = 105,
		MBCSStrings = 136,
		UTF16Strings = 138,
		UTF16StringsBE = 139,
		UTF32Strings = 140,
		UTF32StringsBE = 141,
		ComplexType = 228,
		BytesLigttleEndian = 240,
		BytesBigEndian = 242,
		ArLoggerAPI = 65534,
		EmptyEventData = 0xFFFF
	}
}
