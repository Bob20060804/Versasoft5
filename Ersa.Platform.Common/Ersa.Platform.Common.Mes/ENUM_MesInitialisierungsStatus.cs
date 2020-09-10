using System.ComponentModel;

namespace Ersa.Platform.Common.Mes
{
	/// <summary>
	/// MES��ʼ��״̬
	/// </summary>
	public enum ENUM_MesInitialisierungsStatus
	{
		/// <summary>
		/// Successfully
		/// </summary>
		[Description("successful")]
		Erfolgreich,
		/// <summary>
		/// ��ͣ��
		/// Mes Deactivated
		/// </summary>
		[Description("MesDisabled")]
		MesDeaktiviert,
		/// <summary>
		/// δ����
		/// Mes Not Configured
		/// </summary>
		[Description("MesNotConfigured")]
		MesNichtKonfiguriert,
		/// <summary>
		/// ������
		/// </summary>
		[Description("Faulty")]
		Fehlerhaft
	}
}
