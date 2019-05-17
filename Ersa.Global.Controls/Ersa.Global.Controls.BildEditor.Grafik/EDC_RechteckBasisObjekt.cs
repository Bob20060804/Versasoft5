using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls.BildEditor.Grafik
{
	public abstract class EDC_RechteckBasisObjekt : EDC_GrafikBasisObjekt
	{
		private const int mC_blnRechteckHandleAnzahl = 8;

		public Rect PRO_fdcRechteck
		{
			get
			{
				double x;
				double width;
				if (base.PRO_fdcStartPunkt.X <= base.PRO_fdcEndPunkt.X)
				{
					x = base.PRO_fdcStartPunkt.X;
					width = base.PRO_fdcEndPunkt.X - base.PRO_fdcStartPunkt.X;
				}
				else
				{
					x = base.PRO_fdcEndPunkt.X;
					width = base.PRO_fdcStartPunkt.X - base.PRO_fdcEndPunkt.X;
				}
				double y;
				double height;
				if (base.PRO_fdcEndPunkt.Y <= base.PRO_fdcStartPunkt.Y)
				{
					y = base.PRO_fdcEndPunkt.Y;
					height = base.PRO_fdcStartPunkt.Y - base.PRO_fdcEndPunkt.Y;
				}
				else
				{
					y = base.PRO_fdcStartPunkt.Y;
					height = base.PRO_fdcEndPunkt.Y - base.PRO_fdcStartPunkt.Y;
				}
				return new Rect(x, y, width, height);
			}
		}

		public override int PRO_i32HandleAnzahl => 8;

		public override Point FUN_fdcGetHandle(int i_i32HandleNummer)
		{
			double num = (base.PRO_fdcEndPunkt.X + base.PRO_fdcStartPunkt.X) / 2.0;
			double num2 = (base.PRO_fdcStartPunkt.Y + base.PRO_fdcEndPunkt.Y) / 2.0;
			double x = base.PRO_fdcStartPunkt.X;
			double y = base.PRO_fdcEndPunkt.Y;
			switch (i_i32HandleNummer)
			{
			case 1:
				x = base.PRO_fdcStartPunkt.X;
				y = base.PRO_fdcEndPunkt.Y;
				break;
			case 2:
				x = num;
				y = base.PRO_fdcEndPunkt.Y;
				break;
			case 3:
				x = base.PRO_fdcEndPunkt.X;
				y = base.PRO_fdcEndPunkt.Y;
				break;
			case 4:
				x = base.PRO_fdcEndPunkt.X;
				y = num2;
				break;
			case 5:
				x = base.PRO_fdcEndPunkt.X;
				y = base.PRO_fdcStartPunkt.Y;
				break;
			case 6:
				x = num;
				y = base.PRO_fdcStartPunkt.Y;
				break;
			case 7:
				x = base.PRO_fdcStartPunkt.X;
				y = base.PRO_fdcStartPunkt.Y;
				break;
			case 8:
				x = base.PRO_fdcStartPunkt.X;
				y = num2;
				break;
			}
			return new Point(x, y);
		}

		public override int FUN_i32MacheTrefferTest(Point i_fdcPunkt)
		{
			if (base.PRO_blnIstSelektiert)
			{
				for (int i = 1; i <= PRO_i32HandleAnzahl; i++)
				{
					if (FUN_fdcHoleHandleRechteck(i).Contains(i_fdcPunkt))
					{
						return i;
					}
				}
			}
			if (FUN_blnEnthaeltPunkt(i_fdcPunkt))
			{
				return 0;
			}
			return -1;
		}

		public override Cursor FUN_fdcHoleCursor(int i_i32HandleNummer)
		{
			switch (i_i32HandleNummer)
			{
			case 1:
				return Cursors.SizeNESW;
			case 2:
				return Cursors.SizeNS;
			case 3:
				return Cursors.SizeNWSE;
			case 4:
				return Cursors.SizeWE;
			case 5:
				return Cursors.SizeNESW;
			case 6:
				return Cursors.SizeNS;
			case 7:
				return Cursors.SizeNWSE;
			case 8:
				return Cursors.SizeWE;
			default:
				return Cursors.Arrow;
			}
		}

		public override void SUB_BewegeHandleZu(Point i_fdcPunkt, int i_i32HandleNummer)
		{
			switch (i_i32HandleNummer)
			{
			case 1:
				base.PRO_fdcStartPunkt = new Point(i_fdcPunkt.X, base.PRO_fdcStartPunkt.Y);
				base.PRO_fdcEndPunkt = new Point(base.PRO_fdcEndPunkt.X, i_fdcPunkt.Y);
				break;
			case 2:
				base.PRO_fdcEndPunkt = new Point(base.PRO_fdcEndPunkt.X, i_fdcPunkt.Y);
				break;
			case 3:
				base.PRO_fdcEndPunkt = new Point(i_fdcPunkt.X, i_fdcPunkt.Y);
				break;
			case 4:
				base.PRO_fdcEndPunkt = new Point(i_fdcPunkt.X, base.PRO_fdcEndPunkt.Y);
				break;
			case 5:
				base.PRO_fdcEndPunkt = new Point(i_fdcPunkt.X, base.PRO_fdcEndPunkt.Y);
				base.PRO_fdcStartPunkt = new Point(base.PRO_fdcStartPunkt.X, i_fdcPunkt.Y);
				break;
			case 6:
				base.PRO_fdcStartPunkt = new Point(base.PRO_fdcStartPunkt.X, i_fdcPunkt.Y);
				break;
			case 7:
				base.PRO_fdcStartPunkt = new Point(i_fdcPunkt.X, i_fdcPunkt.Y);
				break;
			case 8:
				base.PRO_fdcStartPunkt = new Point(i_fdcPunkt.X, base.PRO_fdcStartPunkt.Y);
				break;
			}
			SUB_Refresh();
		}

		public override bool FUN_blnSchneidenSich(Rect i_fdcRechteck)
		{
			return PRO_fdcRechteck.IntersectsWith(i_fdcRechteck);
		}

		public override void SUB_BewegeObjekt(double i_dblDeltax, double i_dblDeltay)
		{
			base.PRO_fdcStartPunkt = new Point(base.PRO_fdcStartPunkt.X + i_dblDeltax, base.PRO_fdcStartPunkt.Y + i_dblDeltay);
			base.PRO_fdcEndPunkt = new Point(base.PRO_fdcEndPunkt.X + i_dblDeltax, base.PRO_fdcEndPunkt.Y + i_dblDeltay);
			SUB_Refresh();
		}

		public override void SUB_Normalisiere()
		{
			if (base.PRO_fdcStartPunkt.X > base.PRO_fdcEndPunkt.X)
			{
				double x = base.PRO_fdcStartPunkt.X;
				double x2 = base.PRO_fdcEndPunkt.X;
				base.PRO_fdcStartPunkt = new Point(x2, base.PRO_fdcStartPunkt.Y);
				base.PRO_fdcEndPunkt = new Point(x, base.PRO_fdcEndPunkt.Y);
			}
			if (base.PRO_fdcStartPunkt.Y > base.PRO_fdcEndPunkt.Y)
			{
				double y = base.PRO_fdcStartPunkt.Y;
				double y2 = base.PRO_fdcEndPunkt.Y;
				base.PRO_fdcStartPunkt = new Point(base.PRO_fdcStartPunkt.X, y2);
				base.PRO_fdcEndPunkt = new Point(base.PRO_fdcEndPunkt.X, y);
			}
		}
	}
}
