using System;

namespace BR.AN.PviServices
{
	[CLSCompliant(false)]
	public interface IConvert
	{
		Value PviValueToValue(Value value);

		Value ValueToPviValue(Value value);
	}
}
