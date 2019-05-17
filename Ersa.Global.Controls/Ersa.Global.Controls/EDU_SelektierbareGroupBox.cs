using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ersa.Global.Controls
{
	public class EDU_SelektierbareGroupBox : GroupBox
	{
		public static readonly DependencyProperty PRO_blnIstSelektiertProperty;

		public static readonly DependencyProperty PRO_cmdSelektionGeaendertProperty;

		public static readonly DependencyProperty PRO_objCommandParameterProperty;

		public static readonly DependencyProperty PRO_strIconUriProperty;

		public static readonly DependencyProperty PRO_blnIstSelektionSichtbarProperty;

		public static readonly DependencyProperty PRO_blnIstSelektionAenderbarProperty;

		public bool PRO_blnIstSelektiert
		{
			get
			{
				return (bool)GetValue(PRO_blnIstSelektiertProperty);
			}
			set
			{
				SetValue(PRO_blnIstSelektiertProperty, value);
			}
		}

		public ICommand PRO_cmdSelektionGeaendert
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdSelektionGeaendertProperty);
			}
			set
			{
				SetValue(PRO_cmdSelektionGeaendertProperty, value);
			}
		}

		public object PRO_objCommandParameter
		{
			get
			{
				return GetValue(PRO_objCommandParameterProperty);
			}
			set
			{
				SetValue(PRO_objCommandParameterProperty, value);
			}
		}

		public string PRO_strIconUri
		{
			get
			{
				return (string)GetValue(PRO_strIconUriProperty);
			}
			set
			{
				SetValue(PRO_strIconUriProperty, value);
			}
		}

		public bool PRO_blnIstSelektionSichtbar
		{
			get
			{
				return (bool)GetValue(PRO_blnIstSelektionSichtbarProperty);
			}
			set
			{
				SetValue(PRO_blnIstSelektionSichtbarProperty, value);
			}
		}

		public bool PRO_blnIstSelektionAenderbar
		{
			get
			{
				return (bool)GetValue(PRO_blnIstSelektionAenderbarProperty);
			}
			set
			{
				SetValue(PRO_blnIstSelektionAenderbarProperty, value);
			}
		}

		static EDU_SelektierbareGroupBox()
		{
			PRO_blnIstSelektiertProperty = DependencyProperty.Register("PRO_blnIstSelektiert", typeof(bool), typeof(EDU_SelektierbareGroupBox));
			PRO_cmdSelektionGeaendertProperty = DependencyProperty.Register("PRO_cmdSelektionGeaendert", typeof(ICommand), typeof(EDU_SelektierbareGroupBox));
			PRO_objCommandParameterProperty = DependencyProperty.Register("PRO_objCommandParameter", typeof(object), typeof(EDU_SelektierbareGroupBox));
			PRO_strIconUriProperty = DependencyProperty.Register("PRO_strIconUri", typeof(string), typeof(EDU_SelektierbareGroupBox));
			PRO_blnIstSelektionSichtbarProperty = DependencyProperty.Register("PRO_blnIstSelektionSichtbar", typeof(bool), typeof(EDU_SelektierbareGroupBox), new PropertyMetadata(true));
			PRO_blnIstSelektionAenderbarProperty = DependencyProperty.Register("PRO_blnIstSelektionAenderbar", typeof(bool), typeof(EDU_SelektierbareGroupBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_SelektierbareGroupBox), new FrameworkPropertyMetadata(typeof(EDU_SelektierbareGroupBox)));
		}
	}
}
