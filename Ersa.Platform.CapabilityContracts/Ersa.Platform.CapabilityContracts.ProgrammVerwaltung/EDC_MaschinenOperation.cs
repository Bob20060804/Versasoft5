using Ersa.Global.Mvvm;
using System.Windows.Input;

namespace Ersa.Platform.CapabilityContracts.ProgrammVerwaltung
{
	public class EDC_MaschinenOperation : BindableBase
	{
		public string PRO_strNameKey
		{
			get;
		}

		public string PRO_strIconUri
		{
			get;
		}

		public ICommand PRO_fdcCommand
		{
			get;
		}

		public EDC_MaschinenOperation(string i_strNameKey, ICommand i_fdcCommand, string i_strIconUri)
		{
			PRO_strNameKey = i_strNameKey;
			PRO_fdcCommand = i_fdcCommand;
			PRO_strIconUri = i_strIconUri;
		}
	}
}
