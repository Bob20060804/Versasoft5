using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren.EditorElemente
{
	public class EDC_GeometrieElement : EDC_EditorElement
	{
		private double m_dblLinienBreite;

		private double m_dblGesamtGroesse;

		private GeometryCollection m_lstGeometrien;

		public double PRO_dblLinienBreite
		{
			get
			{
				if (!base.PRO_blnUebergehtSkalierung)
				{
					if (base.PRO_dblSkalierung == 0.0)
					{
						return 0.0;
					}
					return m_dblLinienBreite / base.PRO_dblSkalierung;
				}
				return m_dblLinienBreite;
			}
			set
			{
				SetProperty(ref m_dblLinienBreite, value, "PRO_dblLinienBreite");
			}
		}

		public double PRO_dblGesamtGroesse
		{
			get
			{
				return m_dblGesamtGroesse;
			}
			set
			{
				SetProperty(ref m_dblGesamtGroesse, value, "PRO_dblGesamtGroesse");
			}
		}

		public GeometryCollection PRO_lstGeometrien
		{
			get
			{
				return m_lstGeometrien;
			}
			set
			{
				SetProperty(ref m_lstGeometrien, value, "PRO_lstGeometrien");
			}
		}

		public EDC_GeometrieElement(IEnumerable<Geometry> i_enuGeometrien)
		{
			m_lstGeometrien = new GeometryCollection(i_enuGeometrien);
			m_dblLinienBreite = 1.0;
			m_dblGesamtGroesse = 25.0;
		}

		public static EDC_GeometrieElement FUN_edcErzeugeFadenkreuz()
		{
			return new EDC_GeometrieElement(new Geometry[3]
			{
				new EllipseGeometry(new Point(0.5, 0.5), 0.3, 0.3),
				new LineGeometry(new Point(-1003.0, 0.5), new Point(1004.0, 0.5)),
				new LineGeometry(new Point(0.5, -1003.0), new Point(0.5, 1004.0))
			})
			{
				PRO_blnAuswaehlbar = false
			};
		}

		public static EDC_GeometrieElement FUN_edcErzeugeFadenkreuzKlein()
		{
			return new EDC_GeometrieElement(new Geometry[3]
			{
				new EllipseGeometry(new Point(0.5, 0.5), 0.3, 0.3),
				new LineGeometry(new Point(-1.5, 0.5), new Point(2.5, 0.5)),
				new LineGeometry(new Point(0.5, -1.5), new Point(0.5, 2.0))
			})
			{
				PRO_blnAuswaehlbar = false
			};
		}

		public static EDC_GeometrieElement FUN_edcErzeugeKoodinatenUrsprung()
		{
			return new EDC_GeometrieElement(new Geometry[7]
			{
				new EllipseGeometry(new Point(0.5, 0.5), 0.2, 0.2),
				new LineGeometry(new Point(0.4, 0.5), new Point(1.0, 0.5)),
				new LineGeometry(new Point(1.0, 0.5), new Point(0.9, 0.4)),
				new LineGeometry(new Point(1.0, 0.5), new Point(0.9, 0.6)),
				new LineGeometry(new Point(0.5, 0.4), new Point(0.5, 1.0)),
				new LineGeometry(new Point(0.5, 1.0), new Point(0.4, 0.9)),
				new LineGeometry(new Point(0.5, 1.0), new Point(0.6, 0.9))
			})
			{
				PRO_blnAuswaehlbar = false
			};
		}

		protected override void SUB_OnSkalierungGeaendert()
		{
			base.SUB_OnSkalierungGeaendert();
			RaisePropertyChanged("PRO_dblLinienBreite");
		}
	}
}
