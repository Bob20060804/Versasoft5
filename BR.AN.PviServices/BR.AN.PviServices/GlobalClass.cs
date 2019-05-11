namespace BR.AN.PviServices
{
	internal static class GlobalClass
	{
		internal static uint m_ItemCounter;

		internal static void DumpAddObject(string name)
		{
			m_ItemCounter++;
		}
	}
}
