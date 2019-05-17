using Ersa.Global.Controls.BildEditor.Commands;
using Ersa.Global.Controls.BildEditor.Grafik;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor
{
	public static class EDC_BildEditorExtensions
	{
		public static Cursor PRO_fdcDefaultCursor => Cursors.Arrow;

		public static void SUB_SelektiereAlleGrafikObjekte(this EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			for (int i = 0; i < i_edcBildEditorCanvas.PRO_i32GrafikAnzahl; i++)
			{
				i_edcBildEditorCanvas[i].PRO_blnIstSelektiert = true;
			}
		}

		public static void SUB_UnselektiereAlleGrafikObjekte(this EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			for (int i = 0; i < i_edcBildEditorCanvas.PRO_i32GrafikAnzahl; i++)
			{
				i_edcBildEditorCanvas[i].PRO_blnIstSelektiert = false;
			}
		}

		public static void SUB_LoescheAuswahl(this EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			EDC_BildEditorCommandDelete i_blnBildEditorCommand = new EDC_BildEditorCommandDelete(i_edcBildEditorCanvas);
			bool flag = false;
			for (int num = i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1; num >= 0; num--)
			{
				if (i_edcBildEditorCanvas[num].PRO_blnIstSelektiert)
				{
					i_edcBildEditorCanvas.PRO_lstGrafikliste.RemoveAt(num);
					flag = true;
				}
			}
			if (flag)
			{
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(i_blnBildEditorCommand);
			}
		}

		public static void SUB_LoescheDieGrafik(this EDC_BildEditorCanvas i_edcBildEditorCanvas, EDC_GrafikBasisObjekt i_edcGrafik)
		{
			EDC_BildEditorCommandDelete i_blnBildEditorCommand = new EDC_BildEditorCommandDelete(i_edcBildEditorCanvas);
			bool flag = false;
			for (int num = i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1; num >= 0; num--)
			{
				if (i_edcBildEditorCanvas[num].PRO_i32ObjektId == i_edcGrafik.PRO_i32ObjektId)
				{
					i_edcBildEditorCanvas.PRO_lstGrafikliste.RemoveAt(num);
					flag = true;
				}
			}
			if (flag)
			{
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(i_blnBildEditorCommand);
			}
		}

		public static void SUB_LoescheAlleGrafikObjekte(this EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			if (i_edcBildEditorCanvas.PRO_lstGrafikliste.Count > 0)
			{
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(new EDC_BildEditorCommandDeleteAll(i_edcBildEditorCanvas));
				i_edcBildEditorCanvas.PRO_lstGrafikliste.Clear();
			}
		}

		public static void SUB_BringeAuswahlInVordergrund(this EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			List<EDC_GrafikBasisObjekt> list = new List<EDC_GrafikBasisObjekt>();
			EDC_BildEditorCommandChangeOrder eDC_BildEditorCommandChangeOrder = new EDC_BildEditorCommandChangeOrder(i_edcBildEditorCanvas);
			for (int num = i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1; num >= 0; num--)
			{
				if (i_edcBildEditorCanvas[num].PRO_blnIstSelektiert)
				{
					list.Insert(0, i_edcBildEditorCanvas[num]);
					i_edcBildEditorCanvas.PRO_lstGrafikliste.RemoveAt(num);
				}
			}
			foreach (EDC_GrafikBasisObjekt item in list)
			{
				i_edcBildEditorCanvas.PRO_lstGrafikliste.Add(item);
			}
			if (list.Count > 0)
			{
				eDC_BildEditorCommandChangeOrder.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeOrder);
			}
		}

		public static void SUB_BringeAuswahlInHintergrund(this EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			List<EDC_GrafikBasisObjekt> list = new List<EDC_GrafikBasisObjekt>();
			EDC_BildEditorCommandChangeOrder eDC_BildEditorCommandChangeOrder = new EDC_BildEditorCommandChangeOrder(i_edcBildEditorCanvas);
			for (int num = i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1; num >= 0; num--)
			{
				if (i_edcBildEditorCanvas[num].PRO_blnIstSelektiert)
				{
					list.Add(i_edcBildEditorCanvas[num]);
					i_edcBildEditorCanvas.PRO_lstGrafikliste.RemoveAt(num);
				}
			}
			foreach (EDC_GrafikBasisObjekt item in list)
			{
				i_edcBildEditorCanvas.PRO_lstGrafikliste.Insert(0, item);
			}
			if (list.Count > 0)
			{
				eDC_BildEditorCommandChangeOrder.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeOrder);
			}
		}

		public static bool FUN_blnSetzeNeueStrichStaerke(this EDC_BildEditorCanvas i_edcBildEditorCanvas, double i_dblNeuerWert, bool i_blnZurHistoryHinzufuegen)
		{
			EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
			bool flag = false;
			foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
			{
				if ((item is EDC_RechteckGrafik || item is EDC_EllipsenGrafik || item is EDC_LinienGrafik || item is EDC_PunktGrafik || item is EDC_MehrfachLinienGrafik) && Math.Abs(item.PRO_dblStrichStaerke - i_dblNeuerWert) > 0.1)
				{
					item.PRO_dblStrichStaerke = i_dblNeuerWert;
					flag = true;
				}
			}
			if (flag && i_blnZurHistoryHinzufuegen)
			{
				eDC_BildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeState);
			}
			return flag;
		}

		public static bool FUN_blnSetzeNeueFarbe(this EDC_BildEditorCanvas i_edcBildEditorCanvas, Color i_dblNeuerWert, bool i_blnZurHistoryHinzufuegen)
		{
			EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
			bool flag = false;
			foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
			{
				if (item.PRO_fdcGrafikFarbe != i_dblNeuerWert)
				{
					item.PRO_fdcGrafikFarbe = i_dblNeuerWert;
					flag = true;
				}
			}
			if (flag && i_blnZurHistoryHinzufuegen)
			{
				eDC_BildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeState);
			}
			return flag;
		}

		public static bool FUN_blnSetzeNeueSkalierung(this EDC_BildEditorCanvas i_edcBildEditorCanvas, double i_dblNeuerWert)
		{
			foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
			{
				item.PRO_dblSkalierung = i_dblNeuerWert;
			}
			return true;
		}

		public static bool FUN_blnSetzeNeueFontFamily(this EDC_BildEditorCanvas i_edcBildEditorCanvas, string i_dblNeuerWert, bool i_blnZurHistoryHinzufuegen)
		{
			EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
			bool flag = false;
			foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
			{
				EDC_TextGrafik eDC_TextGrafik = item as EDC_TextGrafik;
				if (eDC_TextGrafik != null && eDC_TextGrafik.PRO_strFontFamilyName != i_dblNeuerWert)
				{
					eDC_TextGrafik.PRO_strFontFamilyName = i_dblNeuerWert;
					flag = true;
				}
			}
			if (flag && i_blnZurHistoryHinzufuegen)
			{
				eDC_BildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeState);
			}
			return flag;
		}

		public static bool FUN_blnSetzeNeuenFontStyle(this EDC_BildEditorCanvas i_edcBildEditorCanvas, FontStyle i_dblNeuerWert, bool i_blnZurHistoryHinzufuegen)
		{
			EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
			bool flag = false;
			VisualCollection.Enumerator enumerator = i_edcBildEditorCanvas.PRO_lstGrafikliste.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = (EDC_GrafikBasisObjekt)enumerator.Current;
				if (eDC_GrafikBasisObjekt.PRO_blnIstSelektiert)
				{
					EDC_TextGrafik eDC_TextGrafik = eDC_GrafikBasisObjekt as EDC_TextGrafik;
					if (eDC_TextGrafik != null && eDC_TextGrafik.PRO_fdcFontStyle != i_dblNeuerWert)
					{
						eDC_TextGrafik.PRO_fdcFontStyle = i_dblNeuerWert;
						flag = true;
					}
				}
			}
			if (flag && i_blnZurHistoryHinzufuegen)
			{
				eDC_BildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeState);
			}
			return flag;
		}

		public static bool FUN_blnSetzeNeuenFontWeight(this EDC_BildEditorCanvas i_edcBildEditorCanvas, FontWeight i_dblNeuerWert, bool i_blnZurHistoryHinzufuegen)
		{
			EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
			bool flag = false;
			foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
			{
				EDC_TextGrafik eDC_TextGrafik = item as EDC_TextGrafik;
				if (eDC_TextGrafik != null && eDC_TextGrafik.PRO_fdcFontWeight != i_dblNeuerWert)
				{
					eDC_TextGrafik.PRO_fdcFontWeight = i_dblNeuerWert;
					flag = true;
				}
			}
			if (flag && i_blnZurHistoryHinzufuegen)
			{
				eDC_BildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeState);
			}
			return flag;
		}

		public static bool FUN_blnSetzeNeuenFontStretch(this EDC_BildEditorCanvas i_edcBildEditorCanvas, FontStretch i_dblNeuerWert, bool i_blnZurHistoryHinzufuegen)
		{
			EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
			bool flag = false;
			foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
			{
				EDC_TextGrafik eDC_TextGrafik = item as EDC_TextGrafik;
				if (eDC_TextGrafik != null && eDC_TextGrafik.PRO_fdcFontStretch != i_dblNeuerWert)
				{
					eDC_TextGrafik.PRO_fdcFontStretch = i_dblNeuerWert;
					flag = true;
				}
			}
			if (flag && i_blnZurHistoryHinzufuegen)
			{
				eDC_BildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeState);
			}
			return flag;
		}

		public static bool FUN_blnSetzeNeuenFontSize(this EDC_BildEditorCanvas i_edcBildEditorCanvas, double i_dblNeuerWert, bool i_blnZurHistoryHinzufuegen)
		{
			EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
			bool flag = false;
			foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
			{
				EDC_TextGrafik eDC_TextGrafik = item as EDC_TextGrafik;
				if (eDC_TextGrafik != null && Math.Abs(eDC_TextGrafik.PRO_dblFontSize - i_dblNeuerWert) > 0.1)
				{
					eDC_TextGrafik.PRO_dblFontSize = i_dblNeuerWert;
					flag = true;
				}
			}
			if (flag && i_blnZurHistoryHinzufuegen)
			{
				eDC_BildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeState);
			}
			return flag;
		}

		public static bool FUN_blnKannEigenschaftGesetztWerden(this EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			VisualCollection.Enumerator enumerator = i_edcBildEditorCanvas.PRO_lstGrafikliste.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = (EDC_GrafikBasisObjekt)enumerator.Current;
				if (!eDC_GrafikBasisObjekt.PRO_blnIstSelektiert)
				{
					continue;
				}
				if (eDC_GrafikBasisObjekt.PRO_fdcGrafikFarbe != i_edcBildEditorCanvas.PRO_fdcGrafikFarbe)
				{
					return true;
				}
				EDC_TextGrafik eDC_TextGrafik = eDC_GrafikBasisObjekt as EDC_TextGrafik;
				if (eDC_TextGrafik == null)
				{
					if (Math.Abs(eDC_GrafikBasisObjekt.PRO_dblStrichStaerke - i_edcBildEditorCanvas.PRO_dblStrichStaerke) > 0.1)
					{
						return true;
					}
					continue;
				}
				if (eDC_TextGrafik.PRO_strFontFamilyName != i_edcBildEditorCanvas.PRO_strTextFontFamilyName)
				{
					return true;
				}
				if (Math.Abs(eDC_TextGrafik.PRO_dblFontSize - i_edcBildEditorCanvas.PRO_fdcTextFontSize) > 0.1)
				{
					return true;
				}
				if (eDC_TextGrafik.PRO_fdcFontStretch != i_edcBildEditorCanvas.PRO_fdcTextFontStretch)
				{
					return true;
				}
				if (eDC_TextGrafik.PRO_fdcFontStyle != i_edcBildEditorCanvas.PRO_fdcTextFontStyle)
				{
					return true;
				}
				if (eDC_TextGrafik.PRO_fdcFontWeight != i_edcBildEditorCanvas.PRO_fdcTextFontWeight)
				{
					return true;
				}
			}
			return false;
		}

		public static void SUB_SetzeEigenschaften(this EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
			bool flag = i_edcBildEditorCanvas.FUN_blnSetzeNeueStrichStaerke(i_edcBildEditorCanvas.PRO_dblStrichStaerke, i_blnZurHistoryHinzufuegen: false);
			if (i_edcBildEditorCanvas.FUN_blnSetzeNeueFarbe(i_edcBildEditorCanvas.PRO_fdcGrafikFarbe, i_blnZurHistoryHinzufuegen: false))
			{
				flag = true;
			}
			if (i_edcBildEditorCanvas.FUN_blnSetzeNeueFontFamily(i_edcBildEditorCanvas.PRO_strTextFontFamilyName, i_blnZurHistoryHinzufuegen: false))
			{
				flag = true;
			}
			if (i_edcBildEditorCanvas.FUN_blnSetzeNeuenFontSize(i_edcBildEditorCanvas.PRO_fdcTextFontSize, i_blnZurHistoryHinzufuegen: false))
			{
				flag = true;
			}
			if (i_edcBildEditorCanvas.FUN_blnSetzeNeuenFontStretch(i_edcBildEditorCanvas.PRO_fdcTextFontStretch, i_blnZurHistoryHinzufuegen: false))
			{
				flag = true;
			}
			if (i_edcBildEditorCanvas.FUN_blnSetzeNeuenFontStyle(i_edcBildEditorCanvas.PRO_fdcTextFontStyle, i_blnZurHistoryHinzufuegen: false))
			{
				flag = true;
			}
			if (i_edcBildEditorCanvas.FUN_blnSetzeNeuenFontWeight(i_edcBildEditorCanvas.PRO_fdcTextFontWeight, i_blnZurHistoryHinzufuegen: false))
			{
				flag = true;
			}
			if (flag)
			{
				eDC_BildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(eDC_BildEditorCommandChangeState);
			}
		}
	}
}
