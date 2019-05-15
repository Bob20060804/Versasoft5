using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.UI.Programm.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Ersa.Platform.UI.Converters
{
	[ValueConversion(typeof(EDC_ProgrammInfo), typeof(string))]
	public class EDC_ProgrammNachStatusIconConverter : IValueConverter
	{
		public string PRO_strArbeitsversionIcon
		{
			get;
			set;
		}

		public string PRO_strVersioniertIcon
		{
			get;
			set;
		}

		public string PRO_strNichtFreigegebenIcon
		{
			get;
			set;
		}

		public string PRO_strFreigegebenIcon
		{
			get;
			set;
		}

		public string PRO_strFreigegebenStufe1Icon
		{
			get;
			set;
		}

		public string PRO_strFreigegebenStufe2Icon
		{
			get;
			set;
		}

		public string PRO_strDefaultIcon
		{
			get;
			set;
		}

		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			EDC_ProgrammViewModel eDC_ProgrammViewModel = i_objValue as EDC_ProgrammViewModel;
			if (eDC_ProgrammViewModel != null)
			{
				return FUN_strStatusIconErmitteln(eDC_ProgrammViewModel);
			}
			EDC_VersionViewModel eDC_VersionViewModel = i_objValue as EDC_VersionViewModel;
			if (eDC_VersionViewModel != null)
			{
				return FUN_strStatusIconErmitteln(eDC_VersionViewModel);
			}
			return Binding.DoNothing;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			return Binding.DoNothing;
		}

		private string FUN_strStatusIconErmitteln(EDC_ProgrammViewModel i_edcPrgViewModel)
		{
			if (i_edcPrgViewModel.PRO_enmFreigabeArt == ENUM_LoetprogrammFreigabeArt.Einstufig)
			{
				if (!i_edcPrgViewModel.PROa_enmStatus.Any((ENUM_LoetprogrammStatus i_enmStatus) => i_enmStatus == ENUM_LoetprogrammStatus.Freigegeben))
				{
					return PRO_strNichtFreigegebenIcon;
				}
				return PRO_strFreigegebenIcon;
			}
			if (i_edcPrgViewModel.PROa_enmStatus.Any((ENUM_LoetprogrammStatus i_enmStatus) => i_enmStatus == ENUM_LoetprogrammStatus.Freigegeben))
			{
				return PRO_strFreigegebenStufe2Icon;
			}
			if (i_edcPrgViewModel.PROa_enmStatus.Any((ENUM_LoetprogrammStatus i_enmStatus) => i_enmStatus == ENUM_LoetprogrammStatus.Arbeitsversion) && i_edcPrgViewModel.PROa_enmFreigabeStatus.Any((ENUM_LoetprogrammFreigabeStatus i_enmFreigabeStatus) => i_enmFreigabeStatus == ENUM_LoetprogrammFreigabeStatus.InFreigabe))
			{
				return PRO_strFreigegebenStufe1Icon;
			}
			return PRO_strNichtFreigegebenIcon;
		}

		private string FUN_strStatusIconErmitteln(EDC_VersionViewModel i_edcVersionViewModel)
		{
			if (i_edcVersionViewModel.PRO_enmFreigabeArt == ENUM_LoetprogrammFreigabeArt.Einstufig)
			{
				switch (i_edcVersionViewModel.PRO_enmStatus)
				{
				case ENUM_LoetprogrammStatus.Versioniert:
					return PRO_strVersioniertIcon;
				case ENUM_LoetprogrammStatus.Freigegeben:
					return PRO_strFreigegebenIcon;
				case ENUM_LoetprogrammStatus.Arbeitsversion:
					return PRO_strArbeitsversionIcon;
				default:
					return PRO_strDefaultIcon;
				}
			}
			switch (i_edcVersionViewModel.PRO_enmStatus)
			{
			case ENUM_LoetprogrammStatus.Versioniert:
				return PRO_strVersioniertIcon;
			case ENUM_LoetprogrammStatus.Freigegeben:
				return PRO_strFreigegebenStufe2Icon;
			case ENUM_LoetprogrammStatus.Arbeitsversion:
				if (i_edcVersionViewModel.PRO_enmFreigabeStatus == ENUM_LoetprogrammFreigabeStatus.InFreigabe)
				{
					return PRO_strFreigegebenStufe1Icon;
				}
				return PRO_strArbeitsversionIcon;
			default:
				return PRO_strDefaultIcon;
			}
		}
	}
}
