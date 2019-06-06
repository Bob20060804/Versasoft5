#define TRACE
using Ersa.Global.Controls.BildEditor.Grafik;
using Ersa.Global.Controls.Extensions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;

namespace Ersa.Global.Controls.BildEditor.Tools
{
	public class EDC_TextTool : EDC_RechteckBasisTool
	{
		protected EDC_TextGrafik m_edcTextGrafik;

		private const string mC_strCursorUri = "pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/Text.cur";

		public TextBox PRO_fdcTextBox
		{
			get;
			set;
		}

		public string PRO_strAlterText
		{
			get;
			set;
		}

		public EDC_TextTool()
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/Ersa.Global.Controls;component/BildEditor/Cursor/Text.cur"));
			base.PRO_fdcToolCursor = ((resourceStream != null) ? new Cursor(resourceStream.Stream) : Cursors.Arrow);
		}

		public override void SUB_OnMouseDown(EDC_BildEditorCanvas i_edcBildEditorCanvas, MouseButtonEventArgs i_fdcArgs)
		{
			Point position = i_fdcArgs.GetPosition(i_edcBildEditorCanvas);
			EDC_TextGrafik i_edcGrafik = new EDC_TextGrafik(string.Empty, position.X, position.Y, position.X + 1.0, position.Y + 1.0, i_edcBildEditorCanvas.PRO_fdcGrafikFarbe, i_edcBildEditorCanvas.PRO_fdcTextFontSize, i_edcBildEditorCanvas.PRO_strTextFontFamilyName, i_edcBildEditorCanvas.PRO_fdcTextFontStyle, i_edcBildEditorCanvas.PRO_fdcTextFontWeight, i_edcBildEditorCanvas.PRO_fdcTextFontStretch, i_edcBildEditorCanvas.PRO_dblSkalierung);
			EDC_BasisTool.SUB_FuegeNeuesGrafikObjektHinzu(i_edcBildEditorCanvas, i_edcGrafik);
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
					SUB_ErstelleDieTextbox(eDC_TextGrafik, i_edcBildEditorCanvas);
				}
			}
		}

		public void SUB_ErstelleDieTextbox(EDC_TextGrafik i_blnTextGrafik, EDC_BildEditorCanvas i_edcBildEditorCanvas)
		{
			i_blnTextGrafik.PRO_blnIstSelektiert = false;
			if (PRO_fdcTextBox != null && PRO_fdcTextBox.IsVisible)
			{
				i_edcBildEditorCanvas.SUB_BlendeTextboxAus(m_edcTextGrafik);
			}
			PRO_strAlterText = i_blnTextGrafik.PRO_strText;
			m_edcTextGrafik = i_blnTextGrafik;
			PRO_fdcTextBox = new TextBox
			{
				Width = i_blnTextGrafik.PRO_fdcRechteck.Width,
				Height = i_blnTextGrafik.PRO_fdcRechteck.Height,
				FontFamily = new FontFamily(i_blnTextGrafik.PRO_strFontFamilyName),
				FontSize = i_blnTextGrafik.PRO_dblFontSize,
				FontStretch = i_blnTextGrafik.PRO_fdcFontStretch,
				FontStyle = i_blnTextGrafik.PRO_fdcFontStyle,
				FontWeight = i_blnTextGrafik.PRO_fdcFontWeight,
				Text = i_blnTextGrafik.PRO_strText,
				AcceptsReturn = true,
				TextWrapping = TextWrapping.Wrap
			};
			i_edcBildEditorCanvas.Children.Add(PRO_fdcTextBox);
			Canvas.SetLeft(PRO_fdcTextBox, i_blnTextGrafik.PRO_fdcRechteck.Left);
			Canvas.SetTop(PRO_fdcTextBox, i_blnTextGrafik.PRO_fdcRechteck.Top);
			PRO_fdcTextBox.Width = PRO_fdcTextBox.Width;
			PRO_fdcTextBox.Height = PRO_fdcTextBox.Height;
			PRO_fdcTextBox.Focus();
			PRO_fdcTextBox.PreviewKeyDown += SUB_TextBoxPreviewKeyDown;
			PRO_fdcTextBox.ContextMenu = null;
			PRO_fdcTextBox.Loaded += SUB_TextboxWurdeGeladen;
		}

		public void SUB_TextboxWurdeGeladen(object i_objParent, RoutedEventArgs i_fdcArgs)
		{
			SUB_BerechneTextOffset(PRO_fdcTextBox, out double i_dblOffsetX, out double i_dblOffsetY);
			Canvas.SetLeft(PRO_fdcTextBox, Canvas.GetLeft(PRO_fdcTextBox) - i_dblOffsetX);
			Canvas.SetTop(PRO_fdcTextBox, Canvas.GetTop(PRO_fdcTextBox) - i_dblOffsetY);
			PRO_fdcTextBox.Width = PRO_fdcTextBox.Width + i_dblOffsetX + i_dblOffsetX;
			PRO_fdcTextBox.Height = PRO_fdcTextBox.Height + i_dblOffsetY + i_dblOffsetY;
		}

		public void SUB_TextBoxPreviewKeyDown(object i_objParent, KeyEventArgs i_fdcArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = EDC_DependencyObjectExtensions.FUN_objFindeBestimmtenParent<EDC_BildEditorCanvas>((DependencyObject)i_objParent);
			if (eDC_BildEditorCanvas != null)
			{
				if (i_fdcArgs.Key == Key.Escape)
				{
					PRO_fdcTextBox.Text = PRO_strAlterText;
					eDC_BildEditorCanvas.SUB_BlendeTextboxAus(m_edcTextGrafik);
					i_fdcArgs.Handled = true;
				}
				else if (i_fdcArgs.Key == Key.Return && Keyboard.Modifiers == ModifierKeys.None)
				{
					eDC_BildEditorCanvas.SUB_BlendeTextboxAus(m_edcTextGrafik);
					i_fdcArgs.Handled = true;
				}
			}
		}

		protected static void SUB_BerechneTextOffset(TextBox i_fdcTextbox, out double i_dblOffsetX, out double i_dblOffsetY)
		{
			i_dblOffsetX = 5.0;
			i_dblOffsetY = 3.0;
			try
			{
				Point point = ((Visual)((ContentControl)i_fdcTextbox.Template.FindName("PART_ContentHost", i_fdcTextbox)).Content).TransformToAncestor(i_fdcTextbox).Transform(new Point(0.0, 0.0));
				i_dblOffsetX = point.X;
				i_dblOffsetY = point.Y;
			}
			catch (ArgumentException ex)
			{
				Trace.WriteLine("SUB_BerechneTextOffset: " + ex.Message);
			}
			catch (InvalidOperationException ex2)
			{
				Trace.WriteLine("SUB_BerechneTextOffset: " + ex2.Message);
			}
		}
	}
}
