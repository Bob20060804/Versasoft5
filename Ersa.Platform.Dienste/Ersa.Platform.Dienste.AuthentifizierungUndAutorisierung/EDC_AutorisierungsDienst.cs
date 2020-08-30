using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Microsoft.IdentityModel.Claims;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;

namespace Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung
{
	[Export(typeof(INF_AutorisierungsDienst))]
	public class EDC_AutorisierungsDienst : INF_AutorisierungsDienst
	{
		private bool m_blnBenutzerUeberFehlendeRechteInformieren = true;

		[ImportingConstructor]
		public EDC_AutorisierungsDienst()
		{
		}

		public bool FUN_blnIstBenutzerAutorisiert(string i_strRecht)
		{
			ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
			if (claimsPrincipal != null)
			{
				return ((IClaimsIdentity)claimsPrincipal.Identity).Claims.Any(delegate(Claim c)
				{
					if (c.ClaimType == "Recht")
					{
						return i_strRecht == c.Value;
					}
					return false;
				});
			}
			return false;
		}

		public bool FUN_blnBenutzerUeberFehlendeRechteInformieren()
		{
			bool blnBenutzerUeberFehlendeRechteInformieren = m_blnBenutzerUeberFehlendeRechteInformieren;
			m_blnBenutzerUeberFehlendeRechteInformieren = false;
			return blnBenutzerUeberFehlendeRechteInformieren;
		}

		public void SUB_RechteInformationZuruecksetzen()
		{
			m_blnBenutzerUeberFehlendeRechteInformieren = true;
		}
	}
}
