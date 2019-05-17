using Ersa.Global.Controls.BildEditor.Grafik;
using Ersa.Global.Controls.Extensions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public class EDC_ComboxTool : EDC_TextTool
	{
		public List<string> PRO_lstTexte
		{
			get;
			set;
		}

		public ComboBox PRO_fdcComboBox
		{
			get;
			set;
		}

		public EDC_ComboxTool()
		{
		}

		public EDC_ComboxTool(List<string> i_lstTexte)
		{
			PRO_lstTexte = i_lstTexte;
			base.PRO_strAlterText = string.Empty;
		}

		public override void SUB_OnMouseUp(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			i_edcBildEditorCanvas.ReleaseMouseCapture();
			if (i_edcBildEditorCanvas.PRO_i32GrafikAnzahl > 0)
			{
				i_edcBildEditorCanvas[i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1].SUB_Normalisiere();
				EDC_TextGrafik eDC_TextGrafik = i_edcBildEditorCanvas[i_edcBildEditorCanvas.PRO_i32GrafikAnzahl - 1] as EDC_TextGrafik;
				if (eDC_TextGrafik != null)
				{
					SUB_ErstelleDieCombobox(eDC_TextGrafik, i_edcBildEditorCanvas);
				}
			}
		}

		public void SUB_ComboPreviewKeyDown(object i_objParent, KeyEventArgs i_fdcArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = EDC_DependencyObjectExtensions.FUN_objFindeBestimmtenParent<EDC_BildEditorCanvas>((DependencyObject)i_objParent);
			if (eDC_BildEditorCanvas != null && (i_fdcArgs.Key == Key.Escape || i_fdcArgs.Key == Key.Return))
			{
				m_edcTextGrafik.PRO_strText = string.Empty;
				eDC_BildEditorCanvas.SUB_BlendeComboboxAus(m_edcTextGrafik);
				i_fdcArgs.Handled = true;
			}
		}

		public void SUB_ComboBoxLostFocus(object i_objParent, RoutedEventArgs i_fdcArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = EDC_DependencyObjectExtensions.FUN_objFindeBestimmtenParent<EDC_BildEditorCanvas>((DependencyObject)i_objParent);
			if (eDC_BildEditorCanvas != null && PRO_fdcComboBox != null && PRO_fdcComboBox.SelectedValue != null)
			{
				m_edcTextGrafik.PRO_strText = PRO_fdcComboBox.SelectedValue.ToString();
				eDC_BildEditorCanvas.SUB_BlendeComboboxAus(m_edcTextGrafik);
			}
		}

		public void SUB_TextAuswahlGeaendert(object i_objParent, SelectionChangedEventArgs i_fdcArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = EDC_DependencyObjectExtensions.FUN_objFindeBestimmtenParent<EDC_BildEditorCanvas>((DependencyObject)i_objParent);
			if (eDC_BildEditorCanvas != null && PRO_fdcComboBox != null && PRO_fdcComboBox.SelectedValue != null)
			{
				m_edcTextGrafik.PRO_strText = PRO_fdcComboBox.SelectedValue.ToString();
				eDC_BildEditorCanvas.SUB_BlendeComboboxAus(m_edcTextGrafik);
			}
		}

		private void SUB_ErstelleDieCombobox(EDC_TextGrafik i_blnTextGrafik, EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			i_blnTextGrafik.PRO_blnIstSelektiert = false;
			if (PRO_fdcComboBox != null && PRO_fdcComboBox.IsVisible)
			{
				i_edcBildEditorCanvas.SUB_BlendeComboboxAus(m_edcTextGrafik);
			}
			m_edcTextGrafik = i_blnTextGrafik;
			PRO_fdcComboBox = new ComboBox
			{
				Width = i_blnTextGrafik.PRO_fdcRechteck.Width,
				Height = i_blnTextGrafik.PRO_fdcRechteck.Height,
				FontFamily = new FontFamily(i_blnTextGrafik.PRO_strFontFamilyName),
				FontSize = i_blnTextGrafik.PRO_dblFontSize,
				FontStretch = i_blnTextGrafik.PRO_fdcFontStretch,
				FontStyle = i_blnTextGrafik.PRO_fdcFontStyle,
				FontWeight = i_blnTextGrafik.PRO_fdcFontWeight,
				Text = i_blnTextGrafik.PRO_strText,
				ItemsSource = PRO_lstTexte
			};
			i_edcBildEditorCanvas.Children.Add(PRO_fdcComboBox);
			Canvas.SetLeft(PRO_fdcComboBox, i_blnTextGrafik.PRO_fdcRechteck.Left);
			Canvas.SetTop(PRO_fdcComboBox, i_blnTextGrafik.PRO_fdcRechteck.Top);
			PRO_fdcComboBox.Width = PRO_fdcComboBox.Width;
			PRO_fdcComboBox.Height = PRO_fdcComboBox.Height;
			PRO_fdcComboBox.Focus();
			PRO_fdcComboBox.LostFocus += SUB_ComboBoxLostFocus;
			PRO_fdcComboBox.PreviewKeyDown += SUB_ComboPreviewKeyDown;
			PRO_fdcComboBox.SelectionChanged += SUB_TextAuswahlGeaendert;
		}
	}
}
