using Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente;
using Ersa.Global.Controls.Extensions;
using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Collections;
using System.Linq;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.VorlagenEditor.DragAndDrop
{
	public class EDC_DefaultAblaufListeDropHandler : DefaultDropHandler
	{
		public override void DragOver(IDropInfo i_fdcDropInfo)
		{
			base.DragOver(i_fdcDropInfo);
			if (i_fdcDropInfo.Effects == DragDropEffects.None)
			{
				return;
			}
			EDC_VorlageElement eDC_VorlageElement = i_fdcDropInfo.Data as EDC_VorlageElement;
			if (eDC_VorlageElement != null)
			{
				RelativeInsertPosition insertPosition = i_fdcDropInfo.InsertPosition;
				EDC_VorlageElement i_edcZielElement = i_fdcDropInfo.TargetItem as EDC_VorlageElement;
				insertPosition &= ~RelativeInsertPosition.TargetItemCenter;
				EDC_AblaufSchritte i_edcAblaufSchritte = (i_fdcDropInfo.TargetCollection as EDC_AblaufSchritte) ?? new EDC_AblaufSchritte();
				if (insertPosition == RelativeInsertPosition.TargetItemCenter || !FUN_blnIstElementFuerAblaufErlaubt(eDC_VorlageElement, i_edcAblaufSchritte))
				{
					i_fdcDropInfo.Effects = DragDropEffects.None;
				}
				if (!FUN_blnDarfElementEingefuegtWerden(i_edcZielElement, i_edcAblaufSchritte, insertPosition))
				{
					i_fdcDropInfo.Effects = DragDropEffects.None;
				}
			}
		}

		public override void Drop(IDropInfo i_fdcDropInfo)
		{
			IList list = i_fdcDropInfo.DragInfo.SourceCollection.TryGetList();
			object data = i_fdcDropInfo.Data;
			int num = list.IndexOf(data);
			base.Drop(i_fdcDropInfo);
			IList list2 = i_fdcDropInfo.TargetCollection.TryGetList();
			int num2 = i_fdcDropInfo.InsertIndex;
			bool num3 = list == list2;
			if (num3 && i_fdcDropInfo.Effects.HasFlag(DragDropEffects.Move) && num2 > num)
			{
				num2--;
			}
			object obj = list2[num2];
			if (!num3 || !i_fdcDropInfo.Effects.HasFlag(DragDropEffects.Move) || num != num2)
			{
				EDC_AblaufSchrittAenderung i_objCommandParameter = i_fdcDropInfo.Effects.HasFlag(DragDropEffects.Move) ? EDC_AblaufSchrittAenderung.FUN_edcVerschobenErzeugen(obj as EDC_VorlageElement, list as EDC_AblaufSchritte, list2 as EDC_AblaufSchritte, num, num2) : EDC_AblaufSchrittAenderung.FUN_edcHinzugefuegtErzeugen(obj as EDC_VorlageElement, list2 as EDC_AblaufSchritte, num2);
				EDC_RoutedCommands.ms_cmdAblaufschrittElementGeaendert.SUB_Execute(i_objCommandParameter, i_fdcDropInfo.VisualTarget);
			}
		}

		protected virtual bool FUN_blnIstElementFuerAblaufErlaubt(EDC_VorlageElement i_edcElement, EDC_AblaufSchritte i_edcAblaufSchritte)
		{
			if (i_edcAblaufSchritte != null)
			{
				return i_edcElement != null;
			}
			return false;
		}

		private bool FUN_blnDarfElementEingefuegtWerden(EDC_VorlageElement i_edcZielElement, EDC_AblaufSchritte i_edcAblaufSchritte, RelativeInsertPosition i_enmPosition)
		{
			if (!i_edcAblaufSchritte.Any())
			{
				return false;
			}
			if (i_edcZielElement != null)
			{
				if (i_enmPosition == RelativeInsertPosition.BeforeTargetItem && !i_edcZielElement.PRO_blnKannDavorEingefuegtWerden)
				{
					return false;
				}
				if (i_enmPosition == RelativeInsertPosition.AfterTargetItem && !i_edcZielElement.PRO_blnKannDanachEingefuegtWerden)
				{
					return false;
				}
			}
			return true;
		}
	}
}
