using System.Data;
using System.Threading.Tasks;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_XmlZuDataSetReader
	{
		public static Task<DataSet> FUN_fdcLeseXmlDateiInDatasetAsync(string i_strXmlDatei)
		{
			return Task.Factory.StartNew(delegate
			{
				DataSet dataSet = new DataSet();
				dataSet.ReadXml(i_strXmlDatei, XmlReadMode.ReadSchema);
				if (dataSet.Tables.Count == 0)
				{
					dataSet.ReadXml(i_strXmlDatei, XmlReadMode.InferSchema);
				}
				return dataSet;
			});
		}

		public static DataSet FUN_fdcLeseXmlDateiInDataset(string i_strXmlDatei)
		{
			DataSet dataSet = new DataSet();
			dataSet.ReadXml(i_strXmlDatei, XmlReadMode.ReadSchema);
			if (dataSet.Tables.Count == 0)
			{
				dataSet.ReadXml(i_strXmlDatei, XmlReadMode.InferSchema);
			}
			return dataSet;
		}
	}
}
