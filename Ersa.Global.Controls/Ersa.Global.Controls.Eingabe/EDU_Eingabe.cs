using Ersa.Global.Controls.Converters;
using Ersa.Global.Controls.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Ersa.Global.Controls.Eingabe
{
	public abstract class EDU_Eingabe : Control
	{
		public static readonly DependencyProperty PRO_strBeschriftungProperty = DependencyProperty.Register("PRO_strBeschriftung", typeof(string), typeof(EDU_Eingabe), new PropertyMetadata(string.Empty));

		public static readonly DependencyProperty PRO_strEinheitProperty = DependencyProperty.Register("PRO_strEinheit", typeof(string), typeof(EDU_Eingabe), new PropertyMetadata(string.Empty));

		public static readonly DependencyProperty PRO_cmdWertGeandertProperty = DependencyProperty.Register("PRO_cmdWertGeandert", typeof(ICommand), typeof(EDU_Eingabe));

		public static readonly DependencyProperty PRO_objCommandParameterProperty = DependencyProperty.Register("PRO_objCommandParameter", typeof(object), typeof(EDU_Eingabe));

		public static readonly DependencyProperty PRO_strTextWennKeinFehlerProperty = DependencyProperty.Register("PRO_strTextWennKeinFehler", typeof(string), typeof(EDU_Eingabe), new PropertyMetadata(string.Empty));

		public static readonly DependencyProperty PRO_strAbbrechenTextProperty = DependencyProperty.Register("PRO_strAbbrechenText", typeof(string), typeof(EDU_Eingabe), new PropertyMetadata(string.Empty));

		public static readonly DependencyProperty PRO_strUebernehmenTextProperty = DependencyProperty.Register("PRO_strUebernehmenText", typeof(string), typeof(EDU_Eingabe), new PropertyMetadata(string.Empty));

		public static readonly DependencyProperty PRO_fdcLokalisierungsConverterProperty = DependencyProperty.Register("PRO_fdcLokalisierungsConverter", typeof(IValueConverter), typeof(EDU_Eingabe), new PropertyMetadata(new EDC_DefaultConverter()));

		public string PRO_strBeschriftung
		{
			get
			{
				return (string)GetValue(PRO_strBeschriftungProperty);
			}
			set
			{
				SetValue(PRO_strBeschriftungProperty, value);
			}
		}

		public string PRO_strEinheit
		{
			get
			{
				return (string)GetValue(PRO_strEinheitProperty);
			}
			set
			{
				SetValue(PRO_strEinheitProperty, value);
			}
		}

		public ICommand PRO_cmdWertGeandert
		{
			get
			{
				return (ICommand)GetValue(PRO_cmdWertGeandertProperty);
			}
			set
			{
				SetValue(PRO_cmdWertGeandertProperty, value);
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

		public string PRO_strTextWennKeinFehler
		{
			get
			{
				return (string)GetValue(PRO_strTextWennKeinFehlerProperty);
			}
			set
			{
				SetValue(PRO_strTextWennKeinFehlerProperty, value);
			}
		}

		public string PRO_strAbbrechenText
		{
			get
			{
				return (string)GetValue(PRO_strAbbrechenTextProperty);
			}
			set
			{
				SetValue(PRO_strAbbrechenTextProperty, value);
			}
		}

		public string PRO_strUebernehmenText
		{
			get
			{
				return (string)GetValue(PRO_strUebernehmenTextProperty);
			}
			set
			{
				SetValue(PRO_strUebernehmenTextProperty, value);
			}
		}

		public IValueConverter PRO_fdcLokalisierungsConverter
		{
			get
			{
				return (IValueConverter)GetValue(PRO_fdcLokalisierungsConverterProperty);
			}
			set
			{
				SetValue(PRO_fdcLokalisierungsConverterProperty, value);
			}
		}

		public bool PRO_blnTastaturAnzeigenBeimLaden
		{
			get;
			set;
		}

		public bool PRO_blnMausEventsBehandeln
		{
			get;
			set;
		}

		protected EDU_Eingabe()
		{
			PRO_blnMausEventsBehandeln = true;
			base.PreviewMouseDown += SUB_MausGedrueckt;
			base.PreviewMouseUp += SUB_MausLosgelassen;
			base.Loaded += SUB_EingabeGeladen;
		}

		protected abstract void SUB_TastaturAnzeigen(Action i_delErfolgsAktion);

		private void SUB_MausGedrueckt(object i_objSender, MouseButtonEventArgs i_fdcMouseButtonEventArgs)
		{
			if (i_fdcMouseButtonEventArgs.ButtonState == MouseButtonState.Pressed && i_fdcMouseButtonEventArgs.ChangedButton == MouseButton.Left && PRO_blnMausEventsBehandeln)
			{
				i_fdcMouseButtonEventArgs.Handled = true;
			}
		}

		private void SUB_MausLosgelassen(object i_objSender, MouseButtonEventArgs i_fdcMouseButtonEventArgs)
		{
			if (i_fdcMouseButtonEventArgs.ButtonState == MouseButtonState.Released && i_fdcMouseButtonEventArgs.ChangedButton == MouseButton.Left)
			{
				if (PRO_blnMausEventsBehandeln)
				{
					i_fdcMouseButtonEventArgs.Handled = true;
				}
				SUB_TastaturAnzeigen();
			}
		}

		private void SUB_EingabeGeladen(object i_objSender, RoutedEventArgs i_fdcEventArg)
		{
			if (PRO_blnTastaturAnzeigenBeimLaden)
			{
				SUB_TastaturAnzeigen();
			}
		}

		private void SUB_TastaturAnzeigen()
		{
			Action i_delErfolgsAktion = null;
			if (PRO_cmdWertGeandert != null)
			{
				i_delErfolgsAktion = delegate
				{
					PRO_cmdWertGeandert.SUB_Execute(PRO_objCommandParameter, this);
				};
			}
			SUB_TastaturAnzeigen(i_delErfolgsAktion);
		}
	}
}
