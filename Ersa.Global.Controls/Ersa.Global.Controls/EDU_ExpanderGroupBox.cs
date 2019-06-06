using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_ExpanderGroupBox : GroupBox
	{
		public static readonly DependencyProperty PRO_objExpanderInhaltProperty;

		public static readonly DependencyProperty PRO_blnBleibtErweitertProperty;

		public static readonly DependencyProperty PRO_blnIstErweitertProperty;

		public static readonly DependencyProperty PRO_dblBreiteNormaleAnsichtProperty;

		public static readonly DependencyProperty PRO_dblBreiteErweiterteAnsichtProperty;

		public static readonly DependencyProperty PRO_blnHatErweitertenInhaltProperty;

		private const int mC_i32Aussenabstand = 32;

		private FrameworkElement m_fdcExpanderInhalt;

		public object PRO_objExpanderInhalt
		{
			get
			{
				return GetValue(PRO_objExpanderInhaltProperty);
			}
			set
			{
				SetValue(PRO_objExpanderInhaltProperty, value);
			}
		}

		public bool PRO_blnBleibtErweitert
		{
			get
			{
				return (bool)GetValue(PRO_blnBleibtErweitertProperty);
			}
			set
			{
				SetValue(PRO_blnBleibtErweitertProperty, value);
			}
		}

		public bool PRO_blnIstErweitert
		{
			get
			{
				return (bool)GetValue(PRO_blnIstErweitertProperty);
			}
			set
			{
				SetValue(PRO_blnIstErweitertProperty, value);
			}
		}

		public double PRO_dblBreiteNormaleAnsicht
		{
			get
			{
				return (double)GetValue(PRO_dblBreiteNormaleAnsichtProperty);
			}
			set
			{
				SetValue(PRO_dblBreiteNormaleAnsichtProperty, value);
			}
		}

		public double PRO_dblBreiteErweiterteAnsicht
		{
			get
			{
				return (double)GetValue(PRO_dblBreiteErweiterteAnsichtProperty);
			}
			set
			{
				SetValue(PRO_dblBreiteErweiterteAnsichtProperty, value);
			}
		}

		public bool PRO_blnHatErweitertenInhalt
		{
			get
			{
				return (bool)GetValue(PRO_blnHatErweitertenInhaltProperty);
			}
			set
			{
				SetValue(PRO_blnHatErweitertenInhaltProperty, value);
			}
		}

		static EDU_ExpanderGroupBox()
		{
			PRO_objExpanderInhaltProperty = DependencyProperty.Register("PRO_objExpanderInhalt", typeof(object), typeof(EDU_ExpanderGroupBox), new PropertyMetadata(SUB_ExpanderInhaltGeandert));
			PRO_blnBleibtErweitertProperty = DependencyProperty.Register("PRO_blnBleibtErweitert", typeof(bool), typeof(EDU_ExpanderGroupBox));
			PRO_blnIstErweitertProperty = DependencyProperty.Register("PRO_blnIstErweitert", typeof(bool), typeof(EDU_ExpanderGroupBox));
			PRO_dblBreiteNormaleAnsichtProperty = DependencyProperty.Register("PRO_dblBreiteNormaleAnsicht", typeof(double), typeof(EDU_ExpanderGroupBox));
			PRO_dblBreiteErweiterteAnsichtProperty = DependencyProperty.Register("PRO_dblBreiteErweiterteAnsicht", typeof(double), typeof(EDU_ExpanderGroupBox));
			PRO_blnHatErweitertenInhaltProperty = DependencyProperty.Register("PRO_blnHatErweitertenInhalt", typeof(bool), typeof(EDU_ExpanderGroupBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_ExpanderGroupBox), new FrameworkPropertyMetadata(typeof(EDU_ExpanderGroupBox)));
		}

		public EDU_ExpanderGroupBox()
		{
			base.DataContextChanged += delegate
			{
				SUB_IstErweitertEinschraenken();
			};
			base.IsVisibleChanged += delegate
			{
				SUB_IstErweitertEinschraenken();
			};
			CommandBinding commandBinding = new CommandBinding(EDC_RoutedCommands.ms_cmdExpanderGroupBoxEingeklappt, delegate
			{
				SUB_InhaltInSichtbarenBereichBringen();
			});
			CommandBinding commandBinding2 = new CommandBinding(EDC_RoutedCommands.ms_cmdExpanderGroupBoxAusgeklappt, delegate
			{
				SUB_ErweitertenInhaltInSichtbarenBereichBringen();
			});
			base.CommandBindings.Add(commandBinding);
			base.CommandBindings.Add(commandBinding2);
		}

		private static void SUB_ExpanderInhaltGeandert(DependencyObject i_fdcDependencyObject, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			(i_fdcDependencyObject as EDU_ExpanderGroupBox)?.SUB_ExpanderInhaltAenderungBehandlen();
		}

		private void SUB_ExpanderInhaltAenderungBehandlen()
		{
			FrameworkElement frameworkElement = PRO_objExpanderInhalt as FrameworkElement;
			if (frameworkElement != null)
			{
				m_fdcExpanderInhalt = frameworkElement;
				m_fdcExpanderInhalt.IsVisibleChanged += SUB_ExpanderInhaltSichtbarkeitGeandert;
			}
			else if (m_fdcExpanderInhalt != null)
			{
				m_fdcExpanderInhalt.IsVisibleChanged -= SUB_ExpanderInhaltSichtbarkeitGeandert;
				m_fdcExpanderInhalt = null;
			}
		}

		private void SUB_ExpanderInhaltSichtbarkeitGeandert(object i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			PRO_blnHatErweitertenInhalt = m_fdcExpanderInhalt.IsVisible;
			if (!PRO_blnHatErweitertenInhalt)
			{
				PRO_blnIstErweitert = false;
			}
		}

		private void SUB_IstErweitertEinschraenken()
		{
			if (!PRO_blnBleibtErweitert)
			{
				PRO_blnIstErweitert = false;
			}
		}

		private void SUB_InhaltInSichtbarenBereichBringen()
		{
			double width = base.RenderSize.Width;
			double height = base.RenderSize.Height;
			Rect targetRectangle = new Rect(new Point(-32.0, 0.0), new Size(width + 64.0, height));
			BringIntoView(targetRectangle);
		}

		private void SUB_ErweitertenInhaltInSichtbarenBereichBringen()
		{
			double width = base.RenderSize.Width;
			double height = base.RenderSize.Height;
			Rect targetRectangle = new Rect(new Point(PRO_dblBreiteNormaleAnsicht, 0.0), new Size(width - PRO_dblBreiteNormaleAnsicht + 64.0, height));
			BringIntoView(targetRectangle);
		}
	}
}
