using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Net.Models.Common
{
	public class TokenResponse
	{
		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

		public int ExpiresIn { get; set; }

		public int RefreshExpiresIn { get; set; }

		public string TokenType { get; set; }

		public string SessionState { get; set; }

		public string Scope { get; set; }
	}
}
