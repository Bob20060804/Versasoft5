using Ersa.Global.Controls.BildEditor.Grafik;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Ersa.Global.Controls.BildEditor.Control
{
	public class EDC_BildEditorControlViewModel : INotifyPropertyChanged
	{
		private const double mC_dblMaximaleVergroesserung = 12.0;

		private const double mC_dblZoomSchrittweite = 0.02;

		private const double mC_intAdaptivesScrollenZeitraum = 100.0;

		private const double mC_intMinimaleZoomRechteckGroesse = 100.0;

		private DateTime m_fdcLetztesScrollen = DateTime.MinValue;

		private EDC_BildEditorCanvas m_edcEditorControl;

		private WriteableBitmap m_fdcBild;

		private string m_strPosition = string.Empty;

		private double m_dblControlHoehe;

		private double m_dblControlBreite;

		public EDC_BildEditorCanvas PRO_edcEditorCanvas
		{
			get
			{
				return m_edcEditorControl;
			}
			set
			{
				m_edcEditorControl = value;
				SUB_OnPropertyChanged("PRO_edcEditorCanvas");
				m_edcEditorControl.PRO_delRechteckZoomAction = SUB_RechteckZoomAction;
			}
		}

		public WriteableBitmap PRO_fdcBild
		{
			get
			{
				return m_fdcBild;
			}
			set
			{
				m_fdcBild = value;
				SUB_OnPropertyChanged("PRO_fdcBild");
				PRO_dblControlBreite = m_fdcBild.PixelWidth;
				PRO_dblControlHoehe = m_fdcBild.PixelHeight;
			}
		}

		public double PRO_dblSkalierung
		{
			get
			{
				if (PRO_edcEditorCanvas != null)
				{
					return PRO_edcEditorCanvas.PRO_dblSkalierung;
				}
				return 1.0;
			}
			set
			{
				if (Math.Abs(PRO_edcEditorCanvas.PRO_dblSkalierung - value) > 0.001)
				{
					PRO_edcEditorCanvas.PRO_dblSkalierung = value;
					SUB_OnPropertyChanged("PRO_dblSkalierung");
				}
			}
		}

		public string PRO_strPosition
		{
			get
			{
				return m_strPosition;
			}
			set
			{
				m_strPosition = value;
				SUB_OnPropertyChanged("PRO_strPosition");
			}
		}

		public double PRO_dblControlBreite
		{
			get
			{
				return m_dblControlBreite;
			}
			set
			{
				if (Math.Abs(value - m_dblControlBreite) > 1.0)
				{
					m_dblControlBreite = value;
					SUB_OnPropertyChanged("PRO_dblControlBreite");
					SUB_SkaliereVollbild();
				}
			}
		}

		public double PRO_dblControlHoehe
		{
			get
			{
				return m_dblControlHoehe;
			}
			set
			{
				if (Math.Abs(value - m_dblControlHoehe) > 1.0)
				{
					m_dblControlHoehe = value;
					SUB_OnPropertyChanged("PRO_dblControlHoehe");
					SUB_SkaliereVollbild();
				}
			}
		}

		public double PRO_dblMaximaleVergroeserung
		{
			get;
			set;
		}

		public double PRO_dblZoomSchrittweite
		{
			get;
			set;
		}

		private double PRO_dblMinimaleSkalierung
		{
			get
			{
				if (PRO_fdcBild == null)
				{
					return 1.0;
				}
				Size size = new Size(PRO_edcEditorCanvas.PRO_fdcScrollView.ActualWidth, PRO_edcEditorCanvas.PRO_fdcScrollView.ActualHeight);
				if (size == Size.Empty)
				{
					return 1.0;
				}
				double num = size.Width / PRO_fdcBild.Width;
				double num2 = size.Height / PRO_fdcBild.Height;
				if (!(num > num2))
				{
					return num;
				}
				return num2;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public EDC_BildEditorControlViewModel()
		{
			PRO_dblMaximaleVergroeserung = 12.0;
			PRO_dblZoomSchrittweite = 0.02;
		}

		public void SUB_MouseWheel(object i_objSender, MouseWheelEventArgs i_fdcArgs)
		{
			i_fdcArgs.Handled = true;
			TimeSpan i_fdcScrollAbstand = DateTime.Now - m_fdcLetztesScrollen;
			m_fdcLetztesScrollen = DateTime.Now;
			double num = FUN_intBerechneAdaptiveScrollweite(i_fdcScrollAbstand);
			int num2 = 1;
			if (i_fdcArgs.Delta < 0)
			{
				num2 = -1;
			}
			double num3 = PRO_dblZoomSchrittweite * (double)num2 * num;
			double num4 = PRO_dblSkalierung + num3;
			if (num3 < 0.0 && num4 < PRO_dblMinimaleSkalierung)
			{
				num4 = PRO_dblMinimaleSkalierung;
			}
			double num5 = (double)PRO_fdcBild.PixelWidth / PRO_fdcBild.Width;
			if (!(num3 > 0.0) || !(num4 > PRO_dblMaximaleVergroeserung * num5))
			{
				Point position = i_fdcArgs.MouseDevice.GetPosition(PRO_edcEditorCanvas);
				Point point = new Point(position.X * num4 - position.X * PRO_dblSkalierung, position.Y * num4 - position.Y * PRO_dblSkalierung);
				Point i_fdcPunkt = new Point(PRO_edcEditorCanvas.PRO_fdcScrollView.HorizontalOffset + point.X, PRO_edcEditorCanvas.PRO_fdcScrollView.VerticalOffset + point.Y);
				PRO_dblSkalierung = num4;
				PRO_edcEditorCanvas.SUB_VerschiebeViewPort(i_fdcPunkt);
			}
		}

		public void SUB_TransformToPixels(double i_dblUnitX, double i_dblUnitY, out int i_i32PixelX, out int i_i32PixelY)
		{
			i_i32PixelX = (int)(PRO_fdcBild.DpiX / 96.0 * i_dblUnitX);
			i_i32PixelY = (int)(PRO_fdcBild.DpiY / 96.0 * i_dblUnitY);
		}

		public Point FUN_fdcTransformiereNachPixel(Point i_fdcPosition)
		{
			SUB_TransformToPixels(i_fdcPosition.X, i_fdcPosition.Y, out int i_i32PixelX, out int i_i32PixelY);
			return new Point(i_i32PixelX, i_i32PixelY);
		}

		public void SUB_MouseMoveHandler(object i_objSender, MouseEventArgs i_fdcArgs)
		{
			if (object.Equals(i_objSender, PRO_edcEditorCanvas))
			{
				Point position = i_fdcArgs.GetPosition(PRO_edcEditorCanvas);
				if ((int)position.X < 0)
				{
					position.X = 0.0;
				}
				if ((int)position.Y < 0)
				{
					position.Y = 0.0;
				}
				PRO_strPosition = $"X={(int)position.X:####}  Y={(int)position.Y:####}";
			}
		}

		public void SUB_SkaliereVollbild()
		{
			PRO_dblSkalierung = PRO_dblMinimaleSkalierung;
			PRO_edcEditorCanvas.SUB_VerschiebeViewPort(new Rect(new Point(0.0, 0.0), new Size(50.0, 50.0)));
		}

		protected void SUB_OnPropertyChanged(string i_strInfo)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(i_strInfo));
			}
		}

		private void SUB_RechteckZoomAction(EDC_GrafikBasisObjekt i_edcGrafik)
		{
			if (i_edcGrafik != null)
			{
				Rect rect = new Rect(i_edcGrafik.PRO_fdcStartPunkt, i_edcGrafik.PRO_fdcEndPunkt);
				if (!(rect.Width * rect.Height < 100.0))
				{
					double num = PRO_dblControlHoehe / PRO_dblControlBreite;
					Rect i_fdcView = (rect.Height / rect.Width > num) ? new Rect(rect.TopLeft, new Size(rect.Width * num, rect.Height)) : new Rect(rect.TopLeft, new Size(rect.Width, rect.Height * num));
					PRO_dblSkalierung = (double)PRO_fdcBild.PixelWidth / i_fdcView.Width;
					PRO_dblSkalierung *= 0.65;
					i_fdcView.X *= PRO_dblSkalierung;
					i_fdcView.Y *= PRO_dblSkalierung;
					i_fdcView.Width *= PRO_dblSkalierung;
					i_fdcView.Height *= PRO_dblSkalierung;
					PRO_edcEditorCanvas.SUB_VerschiebeViewPort(i_fdcView);
				}
			}
		}

		private double FUN_intBerechneAdaptiveScrollweite(TimeSpan i_fdcScrollAbstand)
		{
			if (i_fdcScrollAbstand.TotalMilliseconds > 100.0 || i_fdcScrollAbstand.TotalMilliseconds < 0.0)
			{
				return 1.0;
			}
			return Math.Pow(100.0 - i_fdcScrollAbstand.TotalMilliseconds, 0.33333333333333331);
		}
	}
}
