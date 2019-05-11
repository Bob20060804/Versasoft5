using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_GeometrieHelfer
	{
		public static bool FUN_blnLiegtLinieInRechteck(Point i_sttLinienPunkt1, Point i_sttLinienPunkt2, Rect i_sttRechteck)
		{
			if (!FUN_blnSchneidenSichLinien(i_sttLinienPunkt1, i_sttLinienPunkt2, new Point(i_sttRechteck.X, i_sttRechteck.Y), new Point(i_sttRechteck.X + i_sttRechteck.Width, i_sttRechteck.Y)) && !FUN_blnSchneidenSichLinien(i_sttLinienPunkt1, i_sttLinienPunkt2, new Point(i_sttRechteck.X + i_sttRechteck.Width, i_sttRechteck.Y), new Point(i_sttRechteck.X + i_sttRechteck.Width, i_sttRechteck.Y + i_sttRechteck.Height)) && !FUN_blnSchneidenSichLinien(i_sttLinienPunkt1, i_sttLinienPunkt2, new Point(i_sttRechteck.X + i_sttRechteck.Width, i_sttRechteck.Y + i_sttRechteck.Height), new Point(i_sttRechteck.X, i_sttRechteck.Y + i_sttRechteck.Height)) && !FUN_blnSchneidenSichLinien(i_sttLinienPunkt1, i_sttLinienPunkt2, new Point(i_sttRechteck.X, i_sttRechteck.Y + i_sttRechteck.Height), new Point(i_sttRechteck.X, i_sttRechteck.Y)))
			{
				if (i_sttRechteck.Contains(i_sttLinienPunkt1))
				{
					return i_sttRechteck.Contains(i_sttLinienPunkt2);
				}
				return false;
			}
			return true;
		}

		public static bool FUN_blnSchneidenSichLinien(Point i_sttLinie1Punkt1, Point i_sttLinie1Punkt2, Point i_sttLinie2Punkt1, Point i_sttLinie2Punkt2)
		{
			double num = (i_sttLinie1Punkt1.Y - i_sttLinie2Punkt1.Y) * (i_sttLinie2Punkt2.X - i_sttLinie2Punkt1.X) - (i_sttLinie1Punkt1.X - i_sttLinie2Punkt1.X) * (i_sttLinie2Punkt2.Y - i_sttLinie2Punkt1.Y);
			double num2 = (i_sttLinie1Punkt2.X - i_sttLinie1Punkt1.X) * (i_sttLinie2Punkt2.Y - i_sttLinie2Punkt1.Y) - (i_sttLinie1Punkt2.Y - i_sttLinie1Punkt1.Y) * (i_sttLinie2Punkt2.X - i_sttLinie2Punkt1.X);
			if (Math.Abs(num2) < 1E-10)
			{
				return false;
			}
			double num3 = num / num2;
			num = (i_sttLinie1Punkt1.Y - i_sttLinie2Punkt1.Y) * (i_sttLinie1Punkt2.X - i_sttLinie1Punkt1.X) - (i_sttLinie1Punkt1.X - i_sttLinie2Punkt1.X) * (i_sttLinie1Punkt2.Y - i_sttLinie1Punkt1.Y);
			double num4 = num / num2;
			if (num3 < 0.0 || num3 > 1.0 || num4 < 0.0 || num4 > 1.0)
			{
				return false;
			}
			return true;
		}

		public static bool FUN_blnLinieSchneidetRechteck(IList<Point> i_lstPunkte, double i_dblBreite, Rect i_sttRect)
		{
			Polygon i_fdcPolygon = FUN_fdcKonvertiereZuPolygon(i_sttRect);
			return FUN_blnLinieSchneidetPolygon(i_lstPunkte, i_dblBreite, i_fdcPolygon);
		}

		public static bool FUN_blnLinieSchneidetPolygon(IList<Point> i_lstPunkte, double i_dblBreite, Polygon i_fdcPolygon)
		{
			if (i_lstPunkte.Count >= 2)
			{
				for (int i = 0; i < i_lstPunkte.Count - 1; i++)
				{
					Point i_fdcStart = i_lstPunkte[i];
					Point i_fdcEnd = i_lstPunkte[i + 1];
					Polygon i_fdcPolygonB = FUN_fdcKonvertiereZuPolygon(i_fdcStart, i_fdcEnd, i_dblBreite);
					if (FUN_blnPolygoneUeberschneidenSich(i_fdcPolygon, i_fdcPolygonB))
					{
						return true;
					}
				}
			}
			else if (i_lstPunkte.Count == 1)
			{
				Polygon i_fdcPolygonB2 = FUN_fdcKonvertiereZuPolygon(i_lstPunkte[0], i_dblBreite);
				if (FUN_blnPolygoneUeberschneidenSich(i_fdcPolygon, i_fdcPolygonB2))
				{
					return true;
				}
			}
			return false;
		}

		public static bool FUN_blnPolygoneUeberschneidenSich(Polygon i_fdcPolygonA, Polygon i_fdcPolygonB)
		{
			List<Vector> list = FUN_lstHoleKanten(i_fdcPolygonA);
			List<Vector> list2 = FUN_lstHoleKanten(i_fdcPolygonB);
			int count = list.Count;
			int count2 = list2.Count;
			for (int i = 0; i < count + count2; i++)
			{
				Vector vector = (i >= count) ? list2[i - count] : list[i];
				Vector i_fdcAxis = new Vector(0.0 - vector.Y, vector.X);
				i_fdcAxis.Normalize();
				double r_dblMin = 0.0;
				double r_dblMin2 = 0.0;
				double r_dblMax = 0.0;
				double r_dblMax2 = 0.0;
				SUB_ProjizierePolygon(i_fdcAxis, i_fdcPolygonA, out r_dblMin, out r_dblMax);
				SUB_ProjizierePolygon(i_fdcAxis, i_fdcPolygonB, out r_dblMin2, out r_dblMax2);
				if (FUN_dblIntervallDistanz(r_dblMin, r_dblMax, r_dblMin2, r_dblMax2) > 0.0)
				{
					return false;
				}
			}
			return true;
		}

		public static Polygon FUN_fdcKonvertiereZuPolygon(Point i_fdcStart, Point i_fdcEnd, double i_dblThickness)
		{
			double num = i_fdcEnd.Y - i_fdcStart.Y;
			double num2 = i_fdcEnd.X - i_fdcStart.X;
			Vector vector = i_fdcEnd - i_fdcStart;
			vector.Normalize();
			vector *= i_dblThickness / 2.0;
			Point point = i_fdcStart - vector;
			Point point2 = i_fdcEnd + vector;
			Math.Atan(num / num2);
			Vector vector2 = new Vector(vector.Y, 0.0 - vector.X);
			vector2.Normalize();
			vector2 = Vector.Multiply(vector2, i_dblThickness / 2.0);
			return new Polygon
			{
				Points = new PointCollection(new List<Point>
				{
					Point.Add(point, vector2),
					Point.Add(point2, vector2),
					Point.Subtract(point2, vector2),
					Point.Subtract(point, vector2)
				})
			};
		}

		public static Polygon FUN_fdcKonvertiereZuPolygon(Rect i_sttRect)
		{
			return new Polygon
			{
				Points = new PointCollection(new List<Point>
				{
					new Point(i_sttRect.Left, i_sttRect.Top),
					new Point(i_sttRect.Left + i_sttRect.Width, i_sttRect.Top),
					new Point(i_sttRect.Left + i_sttRect.Width, i_sttRect.Top + i_sttRect.Height),
					new Point(i_sttRect.Left, i_sttRect.Top + i_sttRect.Height)
				})
			};
		}

		public static Polygon FUN_fdcKonvertiereZuPolygon(Rect i_sttRect, float i_sngDrehwinkel)
		{
			Vector sttRectOffset = new Vector(i_sttRect.Location.X, i_sttRect.Location.Y);
			Polygon polygon = FUN_fdcKonvertiereZuPolygon(Rect.Offset(i_sttRect, -sttRectOffset));
			return new Polygon
			{
				Points = new PointCollection(from i_sttPoint in polygon.Points
				select FUN_sttPunktDrehenUndVerschieben(i_sttPoint, sttRectOffset, i_sngDrehwinkel))
			};
		}

		public static Point FUN_sttPunktDrehenUndVerschieben(Point i_sttPoint, Vector i_sttVerschiebung, float i_sngRotationInRadian)
		{
			double num = Math.Cos(i_sngRotationInRadian);
			double num2 = Math.Sin(i_sngRotationInRadian);
			return new Point(i_sttPoint.X * num - i_sttPoint.Y * num2, i_sttPoint.X * num2 + i_sttPoint.Y * num) + i_sttVerschiebung;
		}

		public static float FUN_sngGradZuRadian(float i_sngWinkelInGrad)
		{
			return (float)((double)i_sngWinkelInGrad * 0.017453292519943295);
		}

		public static Rect FUN_sttPasseRechteckKoordinatenAn(Rect i_sttBereich, Vector i_sttNullpunktVerschiebung)
		{
			Point i_sttPunkt = new Point(i_sttBereich.BottomLeft.X, i_sttBereich.BottomLeft.Y);
			Point point = new Point(i_sttBereich.Width, i_sttBereich.Height);
			Point location = FUN_sttPunktKoordinatenUmrechnen(i_sttPunkt, i_sttNullpunktVerschiebung);
			Size size = new Size(point.X, point.Y);
			return new Rect(location, size);
		}

		private static Polygon FUN_fdcKonvertiereZuPolygon(Point i_fdcPoint, double i_dblDurchmesser)
		{
			Polygon polygon = new Polygon();
			double num = i_dblDurchmesser / 2.0;
			polygon.Points = new PointCollection(new List<Point>
			{
				new Point(i_fdcPoint.X - num, i_fdcPoint.Y - num),
				new Point(i_fdcPoint.X + num, i_fdcPoint.Y - num),
				new Point(i_fdcPoint.X + num, i_fdcPoint.Y + num),
				new Point(i_fdcPoint.X - num, i_fdcPoint.Y + num)
			});
			return polygon;
		}

		private static double FUN_dblIntervallDistanz(double i_dblMinA, double i_dblMaxA, double i_dblMinB, double i_dblMaxB)
		{
			if (i_dblMinA < i_dblMinB)
			{
				return i_dblMinB - i_dblMaxA;
			}
			return i_dblMinA - i_dblMaxB;
		}

		private static double FUN_dblSkalarprodukt(Vector i_fdcVector, Point i_fdcPoint)
		{
			return i_fdcVector.X * i_fdcPoint.X + i_fdcVector.Y * i_fdcPoint.Y;
		}

		private static void SUB_ProjizierePolygon(Vector i_fdcAxis, Polygon i_fdcPolygon, out double r_dblMin, out double r_dblMax)
		{
			r_dblMax = (r_dblMin = FUN_dblSkalarprodukt(i_fdcAxis, i_fdcPolygon.Points[0]));
			for (int i = 0; i < i_fdcPolygon.Points.Count; i++)
			{
				double num = FUN_dblSkalarprodukt(i_fdcAxis, i_fdcPolygon.Points[i]);
				if (num < r_dblMin)
				{
					r_dblMin = num;
				}
				else if (num > r_dblMax)
				{
					r_dblMax = num;
				}
			}
		}

		private static List<Vector> FUN_lstHoleKanten(Polygon i_fdcPolygon)
		{
			List<Vector> list = new List<Vector>();
			PointCollection points = i_fdcPolygon.Points;
			for (int i = 0; i < points.Count; i++)
			{
				Point point = points[i];
				Point point2 = (i + 1 < points.Count) ? points[i + 1] : points[0];
				list.Add(point2 - point);
			}
			return list;
		}

		private static Point FUN_sttPunktKoordinatenUmrechnen(Point i_sttPunkt, Vector i_sttNullpunktVerschiebung)
		{
			return new Point(i_sttPunkt.X + i_sttNullpunktVerschiebung.X, i_sttNullpunktVerschiebung.Y - i_sttPunkt.Y);
		}
	}
}
