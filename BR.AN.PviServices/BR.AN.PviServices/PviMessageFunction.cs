namespace BR.AN.PviServices
{
	internal delegate bool PviMessageFunction(int wParam, int lParam, ref ResponseInfo info, uint dataLen);
}
