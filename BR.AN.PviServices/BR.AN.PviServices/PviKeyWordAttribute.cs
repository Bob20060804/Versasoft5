using System;

namespace BR.AN.PviServices
{
	public class PviKeyWordAttribute : Attribute
	{
		private string propPviKeyWord;

		public string PviKeyWord => propPviKeyWord;

		public PviKeyWordAttribute(string scText)
		{
			propPviKeyWord = scText;
		}
	}
}
