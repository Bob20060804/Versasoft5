using Ersa.Global.Mvvm;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public class EDC_PunktContentDefinition : BindableBase
	{
		private double m_dblX;

		private double m_dblY;

		private object m_objContent;

		public double PRO_dblX
		{
			get
			{
				return m_dblX;
			}
			internal set
			{
				SetProperty(ref m_dblX, value, "PRO_dblX");
			}
		}

		public double PRO_dblY
		{
			get
			{
				return m_dblY;
			}
			internal set
			{
				SetProperty(ref m_dblY, value, "PRO_dblY");
			}
		}

		public object PRO_objContent
		{
			get
			{
				return m_objContent;
			}
			internal set
			{
				SetProperty(ref m_objContent, value, "PRO_objContent");
			}
		}
	}
}
