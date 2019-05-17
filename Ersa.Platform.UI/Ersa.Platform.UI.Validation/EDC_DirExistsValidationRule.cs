using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Infrastructure.Prism;
using System.Globalization;
using System.Windows.Controls;

namespace Ersa.Platform.UI.Validation
{
	public class EDC_DirExistsValidationRule : ValidationRule
	{
		private INF_IODienst m_edcIoDienst;

		private INF_IODienst PRO_edcIoDienst => m_edcIoDienst ?? (m_edcIoDienst = EDC_ServiceLocator.PRO_edcInstanz.FUN_objObjektSicherAusContainerHolen<INF_IODienst>());

		public override ValidationResult Validate(object i_objValue, CultureInfo i_fdcCultureInfo)
		{
			string text = i_objValue as string;
			if (string.IsNullOrEmpty(text))
			{
				return new ValidationResult(isValid: false, "10_776");
			}
			if (PRO_edcIoDienst != null && !PRO_edcIoDienst.FUN_blnVerzeichnisExistiert(text))
			{
				return new ValidationResult(isValid: false, "10_776");
			}
			return ValidationResult.ValidResult;
		}
	}
}
