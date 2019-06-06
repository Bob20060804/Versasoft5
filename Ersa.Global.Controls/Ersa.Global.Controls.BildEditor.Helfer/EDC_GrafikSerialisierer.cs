using Ersa.Global.Controls.BildEditor.Eigenschaften;
using Ersa.Global.Controls.BildEditor.Grafik;
using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Ersa.Global.Controls.BildEditor.Helfer
{
	[XmlRoot("SUB_GrafikObjekteArray")]
	public class EDC_GrafikSerialisierer
	{
		private EDC_GrafikEigenschaften[] ma_edcGrafikObjekteArray;

		[XmlArrayItem(typeof(EDC_EllipsenEigenschaften))]
		[XmlArrayItem(typeof(EDC_LinienEigenschaften))]
		[XmlArrayItem(typeof(EDC_MehrfachLinienEigensachften))]
		[XmlArrayItem(typeof(EDC_RechteckEigenschaften))]
		[XmlArrayItem(typeof(EDC_TextEigenschaften))]
		[XmlArrayItem(typeof(EDC_PunktEigenschaften))]
		public EDC_GrafikEigenschaften[] SUB_GrafikObjekteArray
		{
			get
			{
				return ma_edcGrafikObjekteArray;
			}
			set
			{
				ma_edcGrafikObjekteArray = value;
			}
		}

		public EDC_GrafikSerialisierer()
		{
		}

		public EDC_GrafikSerialisierer(VisualCollection i_fdcCollection)
		{
			if (i_fdcCollection == null)
			{
				throw new ArgumentNullException("i_fdcCollection");
			}
			ma_edcGrafikObjekteArray = new EDC_GrafikEigenschaften[i_fdcCollection.Count];
			int num = 0;
			VisualCollection.Enumerator enumerator = i_fdcCollection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = (EDC_GrafikBasisObjekt)enumerator.Current;
				ma_edcGrafikObjekteArray[num++] = eDC_GrafikBasisObjekt.FUN_edcSerialisiereObjekt();
			}
		}
	}
}
