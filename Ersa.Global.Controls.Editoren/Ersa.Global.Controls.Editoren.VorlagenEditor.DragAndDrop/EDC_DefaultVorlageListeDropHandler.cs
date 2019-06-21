using Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente;
using Ersa.Global.Controls.Extensions;
using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Collections;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.VorlagenEditor.DragAndDrop
{
	public class EDC_DefaultVorlageListeDropHandler : DefaultDropHandler
	{
		public override void DragOver(IDropInfo i_fdcDropInfo)
		{
			base.DragOver(i_fdcDropInfo);
			if (i_fdcDropInfo.Effects != 0 && i_fdcDropInfo.TargetCollection == i_fdcDropInfo.DragInfo.SourceCollection)
			{
				i_fdcDropInfo.Effects = DragDropEffects.None;
			}
		}

		public override void Drop(IDropInfo i_fdcDropInfo)
		{
			object data = i_fdcDropInfo.Data;
			IList list = i_fdcDropInfo.DragInfo.SourceCollection.TryGetList();
			IList list2 = i_fdcDropInfo.TargetCollection.TryGetList();
			int i_i32AlterIndex = list.IndexOf(data);
			base.Drop(i_fdcDropInfo);
			list2.Remove(data);
			EDC_AblaufSchrittAenderung i_objCommandParameter = EDC_AblaufSchrittAenderung.FUN_edcEntferntErzeugen(data as EDC_VorlageElement, list as EDC_AblaufSchritte, i_i32AlterIndex);
			EDC_RoutedCommands.ms_cmdAblaufschrittElementGeaendert.SUB_Execute(i_objCommandParameter, i_fdcDropInfo.VisualTarget);
		}
	}
}
