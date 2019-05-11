using System.Text;

namespace BR.AN.PviServices
{
	internal class StringMarshal : ASCIIEncoding
	{
		public override byte[] GetBytes(string s)
		{
			s += "\0";
			return base.GetBytes(s);
		}
	}
}
