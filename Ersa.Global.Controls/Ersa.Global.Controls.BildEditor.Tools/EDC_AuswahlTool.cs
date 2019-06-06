using Ersa.Global.Controls.BildEditor.Commands;
using Ersa.Global.Controls.BildEditor.Grafik;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public class EDC_AuswahlTool : EDC_AbstractTool
	{
		private ENUM_AuswahlModus m_enuAuswahlModus;

		private EDC_GrafikBasisObjekt m_edcAktuellesObjekt;

		private int m_i32AktuellesObjektHandle;

		private Point m_fdcLetzePosition = new Point(0.0, 0.0);

		private EDC_BildEditorCommandChangeState m_edcBildEditorCommandChangeState;

		private bool m_blnHatAenderung;

		public override void SUB_OnMouseDown(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			m_edcBildEditorCommandChangeState = null;
			m_blnHatAenderung = false;
			Point position = i_fdcArgs.GetPosition(i_edcBildEditorCanvas);
			m_enuAuswahlModus = ENUM_AuswahlModus.KeinModus;
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = null;
			for (int num = i_edcBildEditorCanvas.PRO_lstGrafikliste.Count - 1; num >= 0; num--)
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt2 = i_edcBildEditorCanvas[num];
				int num2 = eDC_GrafikBasisObjekt2.FUN_i32MacheTrefferTest(position);
				if (num2 > 0)
				{
					m_enuAuswahlModus = ENUM_AuswahlModus.GroesseAendern;
					m_edcAktuellesObjekt = eDC_GrafikBasisObjekt2;
					m_i32AktuellesObjektHandle = num2;
					i_edcBildEditorCanvas.SUB_UnselektiereAlleGrafikObjekte();
					eDC_GrafikBasisObjekt2.PRO_blnIstSelektiert = true;
					m_edcBildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
					break;
				}
			}
			if (m_enuAuswahlModus == ENUM_AuswahlModus.KeinModus)
			{
				for (int num3 = i_edcBildEditorCanvas.PRO_lstGrafikliste.Count - 1; num3 >= 0; num3--)
				{
					EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt2 = i_edcBildEditorCanvas[num3];
					if (eDC_GrafikBasisObjekt2.FUN_i32MacheTrefferTest(position) == 0)
					{
						eDC_GrafikBasisObjekt = eDC_GrafikBasisObjekt2;
						break;
					}
				}
				if (eDC_GrafikBasisObjekt != null)
				{
					m_enuAuswahlModus = ENUM_AuswahlModus.Verschieben;
					if (Keyboard.Modifiers != ModifierKeys.Control && !eDC_GrafikBasisObjekt.PRO_blnIstSelektiert)
					{
						i_edcBildEditorCanvas.SUB_UnselektiereAlleGrafikObjekte();
					}
					eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = true;
					i_edcBildEditorCanvas.Cursor = Cursors.SizeAll;
					m_edcBildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(i_edcBildEditorCanvas);
				}
			}
			if (m_enuAuswahlModus == ENUM_AuswahlModus.KeinModus)
			{
				if (Keyboard.Modifiers != ModifierKeys.Control)
				{
					i_edcBildEditorCanvas.SUB_UnselektiereAlleGrafikObjekte();
				}
				EDC_AuswahlGrafik visual = new EDC_AuswahlGrafik(position.X, position.Y, position.X + 1.0, position.Y + 1.0, i_edcBildEditorCanvas.PRO_dblSkalierung)
				{
					Clip = new RectangleGeometry(new Rect(0.0, 0.0, i_edcBildEditorCanvas.ActualWidth, i_edcBildEditorCanvas.ActualHeight))
				};
				i_edcBildEditorCanvas.PRO_lstGrafikliste.Add(visual);
				m_enuAuswahlModus = ENUM_AuswahlModus.GruppenAuswahl;
			}
			m_fdcLetzePosition = position;
			i_edcBildEditorCanvas.CaptureMouse();
		}

		public override void SUB_OnMouseMove(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseEventArgs i_fdcArgs)
		{
			if (i_fdcArgs.MiddleButton == MouseButtonState.Pressed || i_fdcArgs.RightButton == MouseButtonState.Pressed)
			{
				SUB_SetzeCursor(i_edcBildEditorCanvas);
				return;
			}
			Point position = i_fdcArgs.GetPosition(i_edcBildEditorCanvas);
			if (i_fdcArgs.LeftButton == MouseButtonState.Released)
			{
				Cursor cursor = null;
				for (int i = 0; i < i_edcBildEditorCanvas.PRO_i32GrafikAnzahl; i++)
				{
					int num = i_edcBildEditorCanvas[i].FUN_i32MacheTrefferTest(position);
					if (num > 0)
					{
						cursor = i_edcBildEditorCanvas[i].FUN_fdcHoleCursor(num);
						break;
					}
				}
				if (cursor == null)
				{
					SUB_SetzeCursor(i_edcBildEditorCanvas);
				}
				else
				{
					i_edcBildEditorCanvas.Cursor = cursor;
				}
			}
			else if (i_edcBildEditorCanvas.IsMouseCaptured)
			{
				m_blnHatAenderung = true;
				double i_dblDeltax = position.X - m_fdcLetzePosition.X;
				double i_dblDeltay = position.Y - m_fdcLetzePosition.Y;
				m_fdcLetzePosition = position;
				if (m_enuAuswahlModus == ENUM_AuswahlModus.GroesseAendern && m_edcAktuellesObjekt != null)
				{
					m_edcAktuellesObjekt.SUB_BewegeHandleZu(position, m_i32AktuellesObjektHandle);
				}
				if (m_enuAuswahlModus == ENUM_AuswahlModus.Verschieben)
				{
					foreach (EDC_GrafikBasisObjekt item in i_edcBildEditorCanvas.PRO_enuGrafikSelktionsliste)
					{
						item.SUB_BewegeObjekt(i_dblDeltax, i_dblDeltay);
					}
				}
				if (m_enuAuswahlModus == ENUM_AuswahlModus.GruppenAuswahl)
				{
					i_edcBildEditorCanvas[i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1].SUB_BewegeHandleZu(position, 5);
				}
			}
		}

		public override void SUB_OnMouseUp(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			if (!i_edcBildEditorCanvas.IsMouseCaptured)
			{
				SUB_SetzeCursor(i_edcBildEditorCanvas);
				m_enuAuswahlModus = ENUM_AuswahlModus.KeinModus;
				return;
			}
			if (m_edcAktuellesObjekt != null)
			{
				m_edcAktuellesObjekt.SUB_Normalisiere();
				(m_edcAktuellesObjekt as EDC_TextGrafik)?.SUB_AktualisiereDasRechteck();
				m_edcAktuellesObjekt = null;
			}
			if (m_enuAuswahlModus == ENUM_AuswahlModus.GruppenAuswahl)
			{
				EDC_AuswahlGrafik eDC_AuswahlGrafik = (EDC_AuswahlGrafik)i_edcBildEditorCanvas[i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1];
				eDC_AuswahlGrafik.SUB_Normalisiere();
				Rect pRO_fdcRechteck = eDC_AuswahlGrafik.PRO_fdcRechteck;
				i_edcBildEditorCanvas.PRO_lstGrafikliste.Remove(eDC_AuswahlGrafik);
				VisualCollection.Enumerator enumerator = i_edcBildEditorCanvas.PRO_lstGrafikliste.GetEnumerator();
				while (enumerator.MoveNext())
				{
					EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = (EDC_GrafikBasisObjekt)enumerator.Current;
					if (eDC_GrafikBasisObjekt.FUN_blnSchneidenSich(pRO_fdcRechteck))
					{
						eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = true;
					}
				}
			}
			i_edcBildEditorCanvas.ReleaseMouseCapture();
			SUB_SetzeCursor(i_edcBildEditorCanvas);
			m_enuAuswahlModus = ENUM_AuswahlModus.KeinModus;
			SUB_FuegeAenderungInHistoryHinzu(i_edcBildEditorCanvas);
		}

		public override void SUB_SetzeCursor(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			i_edcBildEditorCanvas.Cursor = EDC_BildEditorExtensions.PRO_fdcDefaultCursor;
		}

		public bool PRO_blnIstPanningMoeglich()
		{
			if (!m_enuAuswahlModus.Equals(ENUM_AuswahlModus.KeinModus))
			{
				return m_enuAuswahlModus.Equals(ENUM_AuswahlModus.GruppenAuswahl);
			}
			return true;
		}

		public void SUB_FuegeAenderungInHistoryHinzu(EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			if (m_edcBildEditorCommandChangeState != null && m_blnHatAenderung)
			{
				m_edcBildEditorCommandChangeState.SUB_NeuerZustand(i_edcBildEditorCanvas);
				i_edcBildEditorCanvas.SUB_FuegeCommandZuHistoryHinzu(m_edcBildEditorCommandChangeState);
				m_edcBildEditorCommandChangeState = null;
			}
		}
	}
}
