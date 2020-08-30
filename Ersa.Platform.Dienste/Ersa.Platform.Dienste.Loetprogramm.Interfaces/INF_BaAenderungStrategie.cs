using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Loetprogramm.Interfaces
{
	public interface INF_BaAenderungStrategie
	{
		ENUM_LpTransferStrategie PRO_enmLpTransferStrategie
		{
			get;
		}

		void SUB_SetzeLsgSollwerte();

		Task FUN_fdcGeheInAutomatikBetrieb();

		void SUB_VerlasseDenAutomatikBetrieb();

		Task FUN_fdcCodebetriebNeuStarten();
	}
}
