using Ersa.Global.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public class EDC_MehrfachLinienElement : EDC_LinienElement
	{
		private readonly EDC_SmartObservableCollection<EDC_MehrfachLinienDefinition> m_lstLinienElemente;

		private Func<EDC_LinienElement> m_delLinieErstellen;

		public EDC_SmartObservableCollection<EDC_MehrfachLinienDefinition> PRO_lstLinienElemente => m_lstLinienElemente;

		public EDC_LinienElement PRO_edcMasterLinie => m_lstLinienElemente[0].PRO_edcLinienElement;

		public EDC_MehrfachLinienElement()
		{
			m_lstLinienElemente = new EDC_SmartObservableCollection<EDC_MehrfachLinienDefinition>
			{
				new EDC_MehrfachLinienDefinition(this, new Vector(0.0, 0.0))
			};
		}

		public void SUB_SetzeLinieErstellenFunktion(Func<EDC_LinienElement> i_delLinieErstellen)
		{
			m_delLinieErstellen = i_delLinieErstellen;
		}

		public void SUB_DuplikatHinzufuegen(Vector i_sttOffset)
		{
			EDC_LinienElement eDC_LinienElement = m_delLinieErstellen?.Invoke() ?? new EDC_LinienElement();
			PropertyChangedEventManager.AddHandler(eDC_LinienElement, SUB_UnterLinieAusgewaehlt, "PRO_blnAusgewaehlt");
			eDC_LinienElement.SUB_SetzeNormalisiertePunkte(base.PRO_enuNormalisiertePunkte);
			m_lstLinienElemente.Add(new EDC_MehrfachLinienDefinition(eDC_LinienElement, i_sttOffset));
		}

		public void SUB_DuplikateZuruecksetzen()
		{
			m_lstLinienElemente.Clear();
			m_lstLinienElemente.Add(new EDC_MehrfachLinienDefinition(this, new Vector(0.0, 0.0)));
		}

		public override void SUB_SetzePunkte(IEnumerable<Point> i_enuPunkte)
		{
			IList<Point> list = (i_enuPunkte as IList<Point>) ?? i_enuPunkte.ToList();
			base.SUB_SetzePunkte((IEnumerable<Point>)list);
			double x = list.Any() ? list.Min((Point i_edcVector) => i_edcVector.X) : 0.0;
			double y = list.Any() ? list.Min((Point i_edcVector) => i_edcVector.Y) : 0.0;
			Vector vector = new Vector(x, y);
			base.PRO_sttPosition = new Point(x, y);
			List<Point> list2 = (List<Point>)(base.PRO_enuNormalisiertePunkte = FUN_enuPunkteBerechnen(list, -vector).ToList());
			foreach (EDC_MehrfachLinienDefinition item in FUN_enuLinienMitOffsets(1))
			{
				item.PRO_edcLinienElement.SUB_SetzePunkte(list);
			}
		}

		public override bool FUN_blnIstInBereich(Rect i_sttBereich)
		{
			if (!base.FUN_blnIstInBereich(i_sttBereich))
			{
				return FUN_enuLinienMitOffsets(1).Any((EDC_MehrfachLinienDefinition i_edcDefinition) => FUN_blnChildLinieIstInBereich(i_edcDefinition, i_sttBereich));
			}
			return true;
		}

		private bool FUN_blnChildLinieIstInBereich(EDC_MehrfachLinienDefinition i_edcLinienDefinition, Rect i_sttBereich)
		{
			Rect i_sttBereich2 = new Rect(i_sttBereich.Location - i_edcLinienDefinition.PRO_sttVerschiebung, i_sttBereich.Size);
			return i_edcLinienDefinition.PRO_edcLinienElement.FUN_blnIstInBereich(i_sttBereich2);
		}

		private IEnumerable<EDC_MehrfachLinienDefinition> FUN_enuLinienMitOffsets(int i_i32Skip = 0)
		{
			return m_lstLinienElemente.Skip(i_i32Skip);
		}

		private IEnumerable<Point> FUN_enuPunkteBerechnen(IEnumerable<Point> i_enuPunkte, Vector i_sttOffset)
		{
			return from i_edcPoint in i_enuPunkte
			select i_edcPoint + i_sttOffset;
		}

		private void SUB_UnterLinieAusgewaehlt(object i_edcSender, PropertyChangedEventArgs i_edcE)
		{
			EDC_LinienElement eDC_LinienElement = i_edcSender as EDC_LinienElement;
			if (eDC_LinienElement != null && eDC_LinienElement.PRO_blnAusgewaehlt)
			{
				eDC_LinienElement.PRO_blnAusgewaehlt = false;
				base.PRO_blnAusgewaehlt = true;
			}
		}
	}
}
