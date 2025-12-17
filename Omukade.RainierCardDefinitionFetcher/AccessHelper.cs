/*************************************************************************
* Rainier Card Definition Fetcher
* (c) 2022 Hastwell/Electrosheep Networks 
* 
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published
* by the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Affero General Public License for more details.
* 
* You should have received a copy of the GNU Affero General Public License
* along with this program.  If not, see <http://www.gnu.org/licenses/>.
**************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TPCI.PTCS;
using static Omukade.Tools.RainierCardDefinitionFetcher.Manipulators;

namespace Omukade.Tools.RainierCardDefinitionFetcher
{
    internal static class AccessHelper
    {
        public static PTCSUtils.TokenData GetTokenForManuallyLogin()
        {
            TPCI.PTCS.ClientData clientData = newWithoutConstructor<TPCI.PTCS.ClientData>();
            clientData.clientID = "tpci-tcg-app";
            clientData.redirectURI = "https://tpcitcgapp/callback";
            clientData.scope = new string[] { "offline", "screen_name", "openid", "friends" };

            const string AUTH_STAGE_1_PREFIX = "https://access.pokemon.com/oauth2/auth";
            const string AUDIENCE_VALUE = "https://op-core.pokemon.com+https://api.friends.pokemon.com";
            const string SELECTED_LANGUAGE = "en";
            string loginUrl = TPCI.PTCS.PTCSUtils.GetAuthRequest(AUTH_STAGE_1_PREFIX, AUDIENCE_VALUE, clientData, SELECTED_LANGUAGE, "1.33.0", out string challenge, out string verifier);

            HttpClient httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
            httpClient.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/141.0.0.0 Safari/537.36 Edg/141.0.0.0");

            Console.WriteLine(loginUrl);

            string tokenResult = Console.ReadLine() ?? "";
            tokenResult = tokenResult.Substring(tokenResult.IndexOf('?'));

            // Stage 3 - POST /oauth2/token
            System.Collections.Specialized.NameValueCollection stage3queryparams = System.Web.HttpUtility.ParseQueryString(tokenResult.TrimStart('?'));
            FormUrlEncodedContent postbody = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"client_id", clientData.clientID },
                {"code", stage3queryparams["code"] },
                {"code_verifier", verifier },
                {"grant_type", "authorization_code" },
                {"redirect_uri", clientData.redirectURI},
                {"state", challenge }
            });

            HttpResponseMessage loginResult = httpClient.PostAsync("https://access.pokemon.com/oauth2/token", postbody).Result;
            string loginBody = loginResult.Content.ReadAsStringAsync().Result;
            Console.WriteLine(loginBody);

            return JsonConvert.DeserializeObject<PTCSUtils.TokenData>(loginBody);
        }
    }
}
