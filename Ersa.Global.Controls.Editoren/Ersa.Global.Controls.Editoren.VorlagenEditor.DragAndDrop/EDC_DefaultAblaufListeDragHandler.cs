using Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente;
using GongSolutions.Wpf.DragDrop;

namespace Ersa.Global.Controls.Editoren.VorlagenEditor.DragAndDrop
{
	public class EDC_DefaultAblaufListeDragHandler : DefaultDragHandler
	{
		public override bool CanStartDrag(IDragInfo i_fdcDragInfo)
		{
			if (!base.CanStartDrag(i_fdcDragInfo))
			{
				return false;
			}
			return (i_fdcDragInfo.SourceItem as EDC_VorlageElement)?.PRO_blnKannVerschobenWerden ?? false;
		}
	}
}
