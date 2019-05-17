using Ersa.Global.Common;
using Ersa.Global.Controls;
using Ersa.Global.Controls.Adorner;
using Ersa.Global.Controls.Eingabe;
using Ersa.Global.Controls.Extensions;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.UI.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media.Imaging;

namespace Ersa.Platform.UI.Behaviors
{
	public class EDC_PhysischeAdresseBehavior : Behavior<FrameworkElement>
	{
		private const int mC_i32RasterBreite = 8;

		private const int mC_i32VisibleDelay = 300;

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.Loaded += SUB_AssociatedObjectOnLoaded;
			base.AssociatedObject.IsVisibleChanged += SUB_AssociatedObjectOnIsVisibleChanged;
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.Loaded -= SUB_AssociatedObjectOnLoaded;
			base.AssociatedObject.IsVisibleChanged -= SUB_AssociatedObjectOnIsVisibleChanged;
			base.OnDetaching();
		}

		private static bool FUN_blnIstZeigeSpsVariablenAktiv()
		{
			return ConfigurationManager.AppSettings["ZeigeSpsVariablen"] == "true";
		}

		private IEnumerable<DependencyObject> FUN_enuKindElementeErmitteln(DependencyObject i_fdcElement)
		{
			foreach (DependencyObject fdcKindElement in i_fdcElement.FUN_lstKindElementeErmitteln())
			{
				yield return fdcKindElement;
				foreach (DependencyObject item in FUN_enuKindElementeErmitteln(fdcKindElement))
				{
					yield return item;
				}
			}
		}

		private void SUB_AssociatedObjectOnLoaded(object i_objSender, EventArgs i_fdcEventArgs)
		{
			SUB_AssociatedObjectInitialisieren();
		}

		private void SUB_AssociatedObjectOnIsVisibleChanged(object i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			if (base.AssociatedObject.IsVisible)
			{
				Task.Delay(300).Wait(300);
				SUB_AssociatedObjectInitialisieren();
			}
		}

		private void SUB_AssociatedObjectInitialisieren()
		{
			if (FUN_blnIstZeigeSpsVariablenAktiv())
			{
				foreach (EDU_NumerischeEingabe item in FUN_enuKindElementeErmitteln(base.AssociatedObject).OfType<EDU_NumerischeEingabe>().ToList())
				{
					SUB_BehandleNumerischeEingabe(item);
				}
				foreach (CheckBox item2 in FUN_enuKindElementeErmitteln(base.AssociatedObject).OfType<CheckBox>().ToList())
				{
					SUB_BehandleCheckboxOderRadioButton(item2);
				}
				foreach (RadioButton item3 in FUN_enuKindElementeErmitteln(base.AssociatedObject).OfType<RadioButton>().ToList())
				{
					SUB_BehandleCheckboxOderRadioButton(item3);
				}
				foreach (EDU_ToggleButton item4 in FUN_enuKindElementeErmitteln(base.AssociatedObject).OfType<EDU_ToggleButton>().ToList())
				{
					SUB_BehandleToggleButton(item4);
				}
				foreach (EDU_Taster item5 in FUN_enuKindElementeErmitteln(base.AssociatedObject).OfType<EDU_Taster>().ToList())
				{
					SUB_BehandleTaster(item5);
				}
			}
		}

		private void SUB_BehandleToggleButton(EDU_ToggleButton i_edcToggleButton)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(i_edcToggleButton);
			BindingExpression bindingExpression = i_edcToggleButton.GetBindingExpression(UIElement.IsEnabledProperty);
			if (!(bindingExpression?.ResolvedSource is EDC_PrimitivParameter))
			{
				return;
			}
			TextBlock txtEnabledAdresse = FUN_txtAdresseTextErstellen(bindingExpression);
			BindingExpression bindingExpression2 = i_edcToggleButton.GetBindingExpression(EDU_ToggleButton.PRO_blnIstAktivProperty);
			TextBlock txtIstWertAdresseControl = FUN_txtAdresseTextErstellen(bindingExpression2);
			BindingExpression bindingExpression3 = i_edcToggleButton.GetBindingExpression(EDC_ToggleButtonHelfer.ms_intZustandProperty);
			TextBlock txtZustandAdresseControl = FUN_txtAdresseTextErstellen(bindingExpression3);
			BindingExpression bindingExpression4 = i_edcToggleButton.GetBindingExpression(ButtonBase.CommandParameterProperty);
			TextBlock[] a_txtCommandParameterAdresseControl = FUNa_txtAdresseTextFuerToggleErstellen(bindingExpression4);
			if (txtIstWertAdresseControl != null || txtEnabledAdresse != null || txtZustandAdresseControl != null || a_txtCommandParameterAdresseControl.Any())
			{
				FrameworkElement frameworkElement = FUN_fdcAdornerInhaltErstellen(null, txtIstWertAdresseControl, txtEnabledAdresse, a_txtCommandParameterAdresseControl, null, txtZustandAdresseControl);
				if (frameworkElement != null)
				{
					frameworkElement.HorizontalAlignment = HorizontalAlignment.Right;
					frameworkElement.Opacity = 0.7;
					frameworkElement.MouseUp += delegate
					{
						using (SUB_WarteCursorAnzeigen())
						{
							if (txtIstWertAdresseControl != null || txtEnabledAdresse != null || a_txtCommandParameterAdresseControl != null)
							{
								SUB_TextInZwischenablageSpeichern(i_edcToggleButton.DataContext, null, txtIstWertAdresseControl, txtEnabledAdresse, a_txtCommandParameterAdresseControl, null, txtZustandAdresseControl);
							}
						}
					};
					EDU_FrameworkElementAdorner adorner = new EDU_FrameworkElementAdorner(frameworkElement, i_edcToggleButton, ENUM_AdornerPlacement.enmInside, ENUM_AdornerPlacement.enmInside, 0.0, 0.0);
					adornerLayer?.Add(adorner);
					SUB_AdornerDataContextAktualisieren(frameworkElement, i_edcToggleButton.DataContext);
				}
			}
		}

		private void SUB_BehandleTaster(EDU_Taster i_edcTaster)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(i_edcTaster);
			BindingExpression bindingExpression = i_edcTaster.GetBindingExpression(UIElement.IsEnabledProperty);
			if (!(bindingExpression?.ResolvedSource is EDC_PrimitivParameter))
			{
				return;
			}
			TextBlock txtEnabledAdresse = FUN_txtAdresseTextErstellen(bindingExpression);
			BindingExpression bindingExpression2 = i_edcTaster.GetBindingExpression(EDU_ToggleButton.PRO_blnIstAktivProperty);
			TextBlock txtIstWertAdresseControl = FUN_txtAdresseTextErstellen(bindingExpression2);
			BindingExpression bindingExpression3 = i_edcTaster.GetBindingExpression(ButtonBase.CommandParameterProperty);
			TextBlock txtCommandParameterAdresseControl = FUN_txtAdresseTextFuerTasterErstellen(bindingExpression3);
			if (txtIstWertAdresseControl != null || txtEnabledAdresse != null || txtCommandParameterAdresseControl != null)
			{
				FrameworkElement frameworkElement = FUN_fdcAdornerInhaltErstellen(null, txtIstWertAdresseControl, txtEnabledAdresse, null, txtCommandParameterAdresseControl, null);
				if (frameworkElement != null)
				{
					frameworkElement.HorizontalAlignment = HorizontalAlignment.Right;
					frameworkElement.Opacity = 0.7;
					frameworkElement.MouseUp += delegate
					{
						using (SUB_WarteCursorAnzeigen())
						{
							if (txtIstWertAdresseControl != null || txtEnabledAdresse != null || txtCommandParameterAdresseControl != null)
							{
								SUB_TextInZwischenablageSpeichern(i_edcTaster.DataContext, null, txtIstWertAdresseControl, txtEnabledAdresse, null, txtCommandParameterAdresseControl, null);
							}
						}
					};
					EDU_FrameworkElementAdorner adorner = new EDU_FrameworkElementAdorner(frameworkElement, i_edcTaster, ENUM_AdornerPlacement.enmInside, ENUM_AdornerPlacement.enmInside, 0.0, 0.0);
					adornerLayer?.Add(adorner);
					SUB_AdornerDataContextAktualisieren(frameworkElement, i_edcTaster.DataContext);
				}
			}
		}

		private void SUB_BehandleCheckboxOderRadioButton(ToggleButton i_edcToggleButton)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(i_edcToggleButton);
			BindingExpression bindingExpression = i_edcToggleButton.GetBindingExpression(ToggleButton.IsCheckedProperty);
			if (!(bindingExpression?.ResolvedSource is EDC_PrimitivParameter))
			{
				return;
			}
			TextBlock txtIstWertAdresseControl = FUN_txtAdresseTextErstellen(bindingExpression);
			BindingExpression bindingExpression2 = i_edcToggleButton.GetBindingExpression(UIElement.IsEnabledProperty);
			TextBlock txtEnabledAdresseControl = FUN_txtAdresseTextErstellen(bindingExpression2);
			if (txtIstWertAdresseControl != null || txtEnabledAdresseControl != null)
			{
				FrameworkElement frameworkElement = FUN_fdcAdornerInhaltErstellen(null, txtIstWertAdresseControl, txtEnabledAdresseControl, null, null, null);
				if (frameworkElement != null)
				{
					frameworkElement.HorizontalAlignment = HorizontalAlignment.Right;
					frameworkElement.Opacity = 0.7;
					frameworkElement.MouseUp += delegate
					{
						using (SUB_WarteCursorAnzeigen())
						{
							if (txtIstWertAdresseControl != null || txtEnabledAdresseControl != null)
							{
								SUB_TextInZwischenablageSpeichern(i_edcToggleButton.DataContext, null, txtIstWertAdresseControl, txtEnabledAdresseControl, null, null, null);
							}
						}
					};
					EDU_FrameworkElementAdorner adorner = new EDU_FrameworkElementAdorner(frameworkElement, i_edcToggleButton, ENUM_AdornerPlacement.enmInside, ENUM_AdornerPlacement.enmInside, 0.0, 0.0);
					adornerLayer?.Add(adorner);
					SUB_AdornerDataContextAktualisieren(frameworkElement, i_edcToggleButton.DataContext);
				}
			}
		}

		private void SUB_BehandleNumerischeEingabe(EDU_NumerischeEingabe i_edcNumerischeEingabe)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(i_edcNumerischeEingabe);
			BindingExpression bindingExpression = i_edcNumerischeEingabe.GetBindingExpression(EDU_NumerischeEingabe.PRO_dblWertProperty);
			if (!(bindingExpression?.ResolvedSource is EDC_PrimitivParameter))
			{
				return;
			}
			TextBlock txtWertAdresseControl = FUN_txtAdresseTextErstellen(bindingExpression);
			BindingExpression bindingExpression2 = i_edcNumerischeEingabe.GetBindingExpression(EDU_NumerischeEingabe.PRO_dblIstWertProperty);
			TextBlock txtIstWertAdresseControl = FUN_txtAdresseTextErstellen(bindingExpression2);
			BindingExpression bindingExpression3 = i_edcNumerischeEingabe.GetBindingExpression(UIElement.IsEnabledProperty);
			TextBlock txtEnabledAdresseControl = FUN_txtAdresseTextErstellen(bindingExpression3);
			if (txtWertAdresseControl != null || txtIstWertAdresseControl != null || txtEnabledAdresseControl != null)
			{
				FrameworkElement frameworkElement = FUN_fdcAdornerInhaltErstellen(txtWertAdresseControl, txtIstWertAdresseControl, txtEnabledAdresseControl, null, null, null);
				if (frameworkElement != null)
				{
					frameworkElement.HorizontalAlignment = HorizontalAlignment.Right;
					frameworkElement.Opacity = 0.7;
					frameworkElement.MouseUp += delegate
					{
						using (SUB_WarteCursorAnzeigen())
						{
							if (txtWertAdresseControl != null || txtIstWertAdresseControl != null || txtEnabledAdresseControl != null)
							{
								SUB_TextInZwischenablageSpeichern(i_edcNumerischeEingabe.DataContext, txtWertAdresseControl, txtIstWertAdresseControl, txtEnabledAdresseControl, null, null, null);
							}
						}
					};
					EDU_FrameworkElementAdorner adorner = new EDU_FrameworkElementAdorner(frameworkElement, i_edcNumerischeEingabe, ENUM_AdornerPlacement.enmInside, ENUM_AdornerPlacement.enmInside, 0.0, 0.0);
					adornerLayer?.Add(adorner);
					SUB_AdornerDataContextAktualisieren(frameworkElement, i_edcNumerischeEingabe.DataContext);
				}
			}
		}

		private void SUB_TextInZwischenablageSpeichern(object i_objDataContext, TextBlock i_txtWert, TextBlock i_txtIstWert, TextBlock i_txtEnabled, TextBlock[] ia_txtToggle, TextBlock i_txtCommand, TextBlock i_txtZustand)
		{
			string text = FUN_strErstelleString(i_objDataContext, i_txtWert, i_txtIstWert, i_txtEnabled, ia_txtToggle, i_txtCommand, i_txtZustand);
			if (!string.IsNullOrEmpty(text))
			{
				Clipboard.SetText(text);
			}
		}

		private string FUN_strErstelleString(object i_objDataContext, TextBlock i_txtWert, TextBlock i_txtIstWert, TextBlock i_txtEnabled, TextBlock[] ia_txtToggle, TextBlock i_txtCommand, TextBlock i_txtZustand)
		{
			SUB_RefreshTextBlockBinding(i_objDataContext, i_txtWert);
			SUB_RefreshTextBlockBinding(i_objDataContext, i_txtIstWert);
			SUB_RefreshTextBlockBinding(i_objDataContext, i_txtEnabled);
			SUB_RefreshTextBlockBinding(i_objDataContext, i_txtCommand);
			SUB_RefreshTextBlockBinding(i_objDataContext, i_txtZustand);
			foreach (TextBlock item in ia_txtToggle ?? Enumerable.Empty<TextBlock>())
			{
				SUB_RefreshTextBlockBinding(i_objDataContext, item);
			}
			string text = null;
			if (i_txtWert != null)
			{
				text = $"Wert: {i_txtWert.Text}";
			}
			if (i_txtIstWert != null)
			{
				text = $"{text} \n IstWert: {i_txtIstWert.Text}";
			}
			if (i_txtEnabled != null)
			{
				text = $"{text} \n Enabled: {i_txtEnabled.Text}";
			}
			if (ia_txtToggle != null && ia_txtToggle.Length == 3)
			{
				if (ia_txtToggle[0] != null)
				{
					text = $"{text} \n Ein: {ia_txtToggle[0].Text}";
				}
				if (ia_txtToggle[1] != null)
				{
					text = $"{text} \n Toggle: {ia_txtToggle[1].Text}";
				}
				if (ia_txtToggle[2] != null)
				{
					text = $"{text} \n Quitt: {ia_txtToggle[2].Text}";
				}
			}
			if (i_txtCommand != null)
			{
				text = $"{text} \n Command: {i_txtCommand.Text}";
			}
			if (i_txtZustand != null)
			{
				text = $"{text} \n Zustand: {i_txtZustand.Text}";
			}
			return text;
		}

		private void SUB_RefreshTextBlockBinding(object i_objDataContext, TextBlock i_txtTextBlock)
		{
			if (i_txtTextBlock != null)
			{
				i_txtTextBlock.DataContext = i_objDataContext;
			}
		}

		private TextBlock FUN_txtAdresseTextErstellen(BindingExpression i_fdcWertBinding)
		{
			if (i_fdcWertBinding == null)
			{
				return null;
			}
			string path = i_fdcWertBinding.ParentBinding.Path.Path;
			int num = path.LastIndexOf(".", StringComparison.Ordinal);
			Binding binding = (num >= 0) ? new Binding($"{path.Substring(0, num)}") : new Binding();
			binding.Converter = new EDC_ParameterNachPhysischerAdresseConverter();
			TextBlock textBlock = new TextBlock();
			textBlock.TextWrapping = TextWrapping.Wrap;
			textBlock.SetBinding(TextBlock.TextProperty, binding);
			return textBlock;
		}

		private TextBlock FUN_txtAdresseTextFuerTasterErstellen(BindingExpression i_fdcWertBinding)
		{
			if (i_fdcWertBinding == null)
			{
				return null;
			}
			Binding binding = new Binding(i_fdcWertBinding.ParentBinding.Path.Path)
			{
				Converter = new EDC_ParameterNachPhysischerAdresseConverter()
			};
			TextBlock textBlock = new TextBlock();
			textBlock.TextWrapping = TextWrapping.Wrap;
			textBlock.SetBinding(TextBlock.TextProperty, binding);
			return textBlock;
		}

		private TextBlock[] FUNa_txtAdresseTextFuerToggleErstellen(BindingExpression i_fdcWertBinding)
		{
			if (i_fdcWertBinding == null)
			{
				return new TextBlock[0];
			}
			string path = i_fdcWertBinding.ParentBinding.Path.Path;
			string path2 = $"{path}.PRO_edcEin";
			string path3 = $"{path}.PRO_edcToggle";
			string path4 = $"{path}.PRO_edcToggleQuitt";
			EDC_ParameterNachPhysischerAdresseConverter converter = new EDC_ParameterNachPhysischerAdresseConverter();
			Binding binding = new Binding(path2)
			{
				Converter = converter
			};
			Binding binding2 = new Binding(path3)
			{
				Converter = converter
			};
			Binding binding3 = new Binding(path4)
			{
				Converter = converter
			};
			TextBlock textBlock = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};
			textBlock.SetBinding(TextBlock.TextProperty, binding);
			TextBlock textBlock2 = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};
			textBlock2.SetBinding(TextBlock.TextProperty, binding2);
			TextBlock textBlock3 = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};
			textBlock3.SetBinding(TextBlock.TextProperty, binding3);
			return new TextBlock[3]
			{
				textBlock,
				textBlock2,
				textBlock3
			};
		}

		private void SUB_AdornerDataContextAktualisieren(FrameworkElement i_fdcUiElement, object i_objDataContext)
		{
			if (i_fdcUiElement != null)
			{
				i_fdcUiElement.DataContext = i_objDataContext;
			}
		}

		private FrameworkElement FUN_fdcAdornerInhaltErstellen(TextBlock i_txtWert, TextBlock i_txtIstWert, TextBlock i_txtEnabled, TextBlock[] ia_txtToggle, TextBlock i_txtCommand, TextBlock i_txtZustand)
		{
			Grid grid = new Grid
			{
				RowDefinitions = 
				{
					new RowDefinition
					{
						Height = GridLength.Auto
					},
					new RowDefinition
					{
						Height = GridLength.Auto
					},
					new RowDefinition
					{
						Height = GridLength.Auto
					},
					new RowDefinition
					{
						Height = GridLength.Auto
					},
					new RowDefinition
					{
						Height = GridLength.Auto
					},
					new RowDefinition
					{
						Height = GridLength.Auto
					},
					new RowDefinition
					{
						Height = GridLength.Auto
					},
					new RowDefinition
					{
						Height = GridLength.Auto
					}
				},
				ColumnDefinitions = 
				{
					new ColumnDefinition
					{
						Width = GridLength.Auto
					},
					new ColumnDefinition
					{
						Width = new GridLength(8.0)
					},
					new ColumnDefinition()
				}
			};
			if (i_txtWert != null)
			{
				TextBlock element = new TextBlock
				{
					Text = "Wert:"
				};
				Grid.SetColumn(element, 0);
				Grid.SetRow(element, 0);
				grid.Children.Add(element);
				Grid.SetColumn(i_txtWert, 2);
				Grid.SetRow(i_txtWert, 0);
				grid.Children.Add(i_txtWert);
			}
			if (i_txtIstWert != null)
			{
				TextBlock element2 = new TextBlock
				{
					Text = "IstWert:"
				};
				Grid.SetColumn(element2, 0);
				Grid.SetRow(element2, 1);
				grid.Children.Add(element2);
				Grid.SetColumn(i_txtIstWert, 2);
				Grid.SetRow(i_txtIstWert, 1);
				grid.Children.Add(i_txtIstWert);
			}
			if (i_txtEnabled != null)
			{
				TextBlock element3 = new TextBlock
				{
					Text = "Enabled:"
				};
				Grid.SetColumn(element3, 0);
				Grid.SetRow(element3, 2);
				grid.Children.Add(element3);
				Grid.SetColumn(i_txtEnabled, 2);
				Grid.SetRow(i_txtEnabled, 2);
				grid.Children.Add(i_txtEnabled);
			}
			if (ia_txtToggle != null && ia_txtToggle.Length == 3)
			{
				if (ia_txtToggle[0] != null)
				{
					TextBlock element4 = new TextBlock
					{
						Text = "Ein:"
					};
					Grid.SetColumn(element4, 0);
					Grid.SetRow(element4, 3);
					grid.Children.Add(element4);
					Grid.SetColumn(ia_txtToggle[0], 2);
					Grid.SetRow(ia_txtToggle[0], 3);
					grid.Children.Add(ia_txtToggle[0]);
				}
				if (ia_txtToggle[1] != null)
				{
					TextBlock element5 = new TextBlock
					{
						Text = "Toggle:"
					};
					Grid.SetColumn(element5, 0);
					Grid.SetRow(element5, 4);
					grid.Children.Add(element5);
					Grid.SetColumn(ia_txtToggle[1], 2);
					Grid.SetRow(ia_txtToggle[1], 4);
					grid.Children.Add(ia_txtToggle[1]);
				}
				if (ia_txtToggle[2] != null)
				{
					TextBlock element6 = new TextBlock
					{
						Text = "Quitt:"
					};
					Grid.SetColumn(element6, 0);
					Grid.SetRow(element6, 5);
					grid.Children.Add(element6);
					Grid.SetColumn(ia_txtToggle[2], 2);
					Grid.SetRow(ia_txtToggle[2], 5);
					grid.Children.Add(ia_txtToggle[2]);
				}
			}
			if (i_txtCommand != null)
			{
				TextBlock element7 = new TextBlock
				{
					Text = "Command"
				};
				Grid.SetColumn(element7, 0);
				Grid.SetRow(element7, 6);
				grid.Children.Add(element7);
				Grid.SetColumn(i_txtCommand, 2);
				Grid.SetRow(i_txtCommand, 6);
				grid.Children.Add(i_txtCommand);
			}
			if (i_txtZustand != null)
			{
				TextBlock element8 = new TextBlock
				{
					Text = "Zustand"
				};
				Grid.SetColumn(element8, 0);
				Grid.SetRow(element8, 7);
				grid.Children.Add(element8);
				Grid.SetColumn(i_txtZustand, 2);
				Grid.SetRow(i_txtZustand, 7);
				grid.Children.Add(i_txtZustand);
			}
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.UriSource = new Uri("pack://application:,,,/Ersa.Global.Controls;component/Bilder/Icons/Icon_Info_24x24.png");
			bitmapImage.EndInit();
			return new Image
			{
				Width = 24.0,
				Height = 24.0,
				ToolTip = grid,
				Source = bitmapImage
			};
		}

		private IDisposable SUB_WarteCursorAnzeigen()
		{
			Mouse.OverrideCursor = Cursors.Wait;
			return EDC_Disposable.FUN_fdcCreate(delegate
			{
				Task.Run(async delegate
				{
					await Task.Delay(300);
					await EDC_Dispatch.FUN_fdcAwaitableAktion(delegate
					{
						Mouse.OverrideCursor = null;
					});
				});
			});
		}
	}
}
