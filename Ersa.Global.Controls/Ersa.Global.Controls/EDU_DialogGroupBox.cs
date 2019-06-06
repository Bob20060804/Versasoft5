using Ersa.Global.Controls.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls
{
	public class EDU_DialogGroupBox : GroupBox
	{
		public static readonly DependencyProperty PRO_objFooterProperty = DependencyProperty.Register("PRO_objFooter", typeof(object), typeof(EDU_DialogGroupBox));

		private const string mC_strNameKlickbaresControl = "Header";

		private const double mC_dblMindestAbstandHorizontal = 100.0;

		private const double mC_dblMindestAbstandVertikal = 50.0;

		private readonly TranslateTransform m_fdcTranslation = new TranslateTransform(0.0, 0.0);

		private Border m_fdcHeaderBorder;

		private Panel m_fdcParentPanel;

		private bool m_blnVerschiebenAktiv;

		private Point m_fdcLetztePosition;

		public object PRO_objFooter
		{
			get
			{
				return GetValue(PRO_objFooterProperty);
			}
			set
			{
				SetValue(PRO_objFooterProperty, value);
			}
		}

		public EDU_DialogGroupBox()
		{
			base.Loaded += delegate
			{
				Window window = Window.GetWindow(this);
				if (window != null)
				{
					window.Closing += delegate
					{
						SUB_Aufraeumen();
					};
					SUB_VerschiebenInitialisieren();
				}
			};
		}

		private void SUB_VerschiebenInitialisieren()
		{
			m_fdcParentPanel = this.FUN_objElternElementErmitteln<Panel>();
			if (m_fdcParentPanel != null)
			{
				m_fdcHeaderBorder = this.FUN_objBenanntesKindElementSuchen<Border>("Header").FirstOrDefault();
				if (m_fdcHeaderBorder != null)
				{
					base.RenderTransform = m_fdcTranslation;
					m_fdcHeaderBorder.MouseLeftButtonDown += SUB_HeaderMouseLeftButtonDown;
					m_fdcParentPanel.MouseLeave += SUB_ParentPanelMouseLeave;
					m_fdcParentPanel.MouseUp += SUB_ParentPanelMouseUp;
					m_fdcParentPanel.MouseMove += SUB_ParentPanelMouseMove;
				}
			}
		}

		private void SUB_Aufraeumen()
		{
			if (m_fdcHeaderBorder != null)
			{
				m_fdcHeaderBorder.MouseLeftButtonDown -= SUB_HeaderMouseLeftButtonDown;
			}
			if (m_fdcParentPanel != null)
			{
				m_fdcParentPanel.MouseLeave -= SUB_ParentPanelMouseLeave;
				m_fdcParentPanel.MouseUp -= SUB_ParentPanelMouseUp;
				m_fdcParentPanel.MouseMove -= SUB_ParentPanelMouseMove;
			}
		}

		private void SUB_HeaderMouseLeftButtonDown(object i_objSender, MouseButtonEventArgs i_fdcArgs)
		{
			m_fdcLetztePosition = i_fdcArgs.GetPosition(m_fdcParentPanel);
			m_blnVerschiebenAktiv = true;
		}

		private void SUB_ParentPanelMouseLeave(object i_objSender, MouseEventArgs i_fdcArgs)
		{
			m_blnVerschiebenAktiv = false;
		}

		private void SUB_ParentPanelMouseUp(object i_objSender, MouseButtonEventArgs i_fdcArgs)
		{
			m_blnVerschiebenAktiv = false;
		}

		private void SUB_ParentPanelMouseMove(object i_objSender, MouseEventArgs i_fdcArgs)
		{
			if (m_blnVerschiebenAktiv)
			{
				Vector i_fdcOffsetVektor = i_fdcArgs.GetPosition(m_fdcParentPanel) - m_fdcLetztePosition;
				Vector vector = FUN_fdcBegrenzeOffsetWennNoetig(m_fdcParentPanel, m_fdcHeaderBorder, i_fdcOffsetVektor);
				if (vector.Length > 0.0)
				{
					m_fdcTranslation.X += vector.X;
					m_fdcTranslation.Y += vector.Y;
					m_fdcLetztePosition += vector;
				}
			}
		}

		private Vector FUN_fdcBegrenzeOffsetWennNoetig(FrameworkElement i_fdcParentElement, FrameworkElement i_fdcChildElement, Vector i_fdcOffsetVektor)
		{
			Point point = i_fdcChildElement.TransformToAncestor(i_fdcParentElement).Transform(default(Point));
			Rect rect = new Rect(point.X + i_fdcOffsetVektor.X, point.Y + i_fdcOffsetVektor.Y, i_fdcChildElement.ActualWidth, i_fdcChildElement.ActualHeight);
			Vector result = default(Vector);
			result.X = ((rect.Right > 100.0 && rect.Left < i_fdcParentElement.RenderSize.Width - 100.0) ? i_fdcOffsetVektor.X : 0.0);
			result.Y = ((rect.Top > 50.0 && rect.Bottom < i_fdcParentElement.RenderSize.Height - 50.0) ? i_fdcOffsetVektor.Y : 0.0);
			return result;
		}
	}
}
