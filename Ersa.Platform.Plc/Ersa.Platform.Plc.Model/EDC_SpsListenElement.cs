namespace Ersa.Platform.Plc.Model
{
    /// <summary>
    /// ±´¼ÓÀ³plcÔªËØ
    /// </summary>
    public class EDC_SpsListenElement
    {
        /// <summary>
        /// GroupName
        /// </summary>
		public string PRO_strGruppenName { get; set; }

        /// <summary>
        /// Variable
        /// </summary>
		public string PRO_strVariable { get; set; }

        /// <summary>
        /// Value
        /// </summary>
		public object PRO_objWert { get; set; }
    }
}
