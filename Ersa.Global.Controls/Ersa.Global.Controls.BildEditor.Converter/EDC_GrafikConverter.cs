using Ersa.Global.Controls.BildEditor.Eigenschaften;
using Ersa.Global.Controls.BildEditor.Exceptions;
using Ersa.Global.Controls.BildEditor.Grafik;
using Ersa.Global.Controls.BildEditor.Helfer;
using System;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Ersa.Global.Controls.BildEditor.Converter
{
	public static class EDC_GrafikConverter
	{
		public static VisualCollection FUN_lstErstelleGrafikListeVonXml(Visual i_fdcParent, string i_strXml)
		{
			VisualCollection visualCollection = new VisualCollection(i_fdcParent);
			i_strXml = i_strXml.Trim();
			if (string.IsNullOrEmpty(i_strXml))
			{
				return visualCollection;
			}
			EDC_GrafikSerialisierer eDC_GrafikSerialisierer = (EDC_GrafikSerialisierer)FUN_objErstelleObjektVonXml(typeof(EDC_GrafikSerialisierer), i_strXml);
			if (eDC_GrafikSerialisierer == null || eDC_GrafikSerialisierer.SUB_GrafikObjekteArray == null)
			{
				throw new EDC_BildEditorCanvasException("EDC_GrafikConverter.FUN_lstErstelleGrafikListeVonXml: Die XML Datei hat nicht die gewünschten Informationen");
			}
			EDC_GrafikEigenschaften[] sUB_GrafikObjekteArray = eDC_GrafikSerialisierer.SUB_GrafikObjekteArray;
			foreach (EDC_GrafikEigenschaften eDC_GrafikEigenschaften in sUB_GrafikObjekteArray)
			{
				visualCollection.Add(eDC_GrafikEigenschaften.FUN_edcErstelleGrafikObjekt());
			}
			return visualCollection;
		}

		public static string FUN_strErstelleXmlVonGrafiken(VisualCollection i_lstGrafiken)
		{
			if (i_lstGrafiken.Count < 1)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(string.Empty);
			using (StringWriter textWriter = new StringWriter(stringBuilder))
			{
				EDC_GrafikSerialisierer o = new EDC_GrafikSerialisierer(i_lstGrafiken);
				new XmlSerializer(typeof(EDC_GrafikSerialisierer)).Serialize(textWriter, o);
			}
			return stringBuilder.ToString();
		}

		public static void SUB_ZeichneInDenContext(VisualCollection i_lstGrafiken, DrawingContext i_fdcContext, bool i_blnSeletiert = false)
		{
			if (i_lstGrafiken.Count < 1)
			{
				return;
			}
			bool pRO_blnIstSelektiert = false;
			VisualCollection.Enumerator enumerator = i_lstGrafiken.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = (EDC_GrafikBasisObjekt)enumerator.Current;
				if (!i_blnSeletiert)
				{
					pRO_blnIstSelektiert = eDC_GrafikBasisObjekt.PRO_blnIstSelektiert;
					eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = false;
				}
				eDC_GrafikBasisObjekt.Draw(i_fdcContext);
				if (!i_blnSeletiert)
				{
					eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = pRO_blnIstSelektiert;
				}
			}
		}

		private static object FUN_objErstelleObjektVonXml(Type i_fdcTyp, string i_strXml)
		{
			try
			{
				return new XmlSerializer(i_fdcTyp).Deserialize(new StringReader(i_strXml));
			}
			catch (Exception ex)
			{
				throw new EDC_BildEditorCanvasException("EDC_GrafikConverter.FUN_objErstelleObjektVonXml: Fehler bei Deserialisierung " + ex.Message);
			}
		}
	}
}
