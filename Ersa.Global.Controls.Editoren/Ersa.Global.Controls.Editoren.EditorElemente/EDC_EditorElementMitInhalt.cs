using System.Windows;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public abstract class EDC_EditorElementMitInhalt : EDC_EditorElement
	{
		private object m_objContent;

		private DataTemplate m_fdcContentTemplate;

		public object PRO_objContent
		{
			get
			{
				return m_objContent;
			}
			set
			{
				SetProperty(ref m_objContent, value, "PRO_objContent");
			}
		}

		public DataTemplate PRO_fdcContentTemplate
		{
			get
			{
				return m_fdcContentTemplate;
			}
			set
			{
				SetProperty(ref m_fdcContentTemplate, value, "PRO_fdcContentTemplate");
			}
		}
	}
}
