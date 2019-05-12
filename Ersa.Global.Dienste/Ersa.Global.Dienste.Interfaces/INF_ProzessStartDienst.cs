namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_ProzessStartDienst
	{
		void SUB_ProzessStarten(string i_strDateiname);

		void SUB_ProzessStarten(string i_strDateiname, string i_strArgumente);
	}
}
