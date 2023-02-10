using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PortalBlazor.Client.Utils;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace PortalBlazor.Client.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider, IAuthorizeService
    {
        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    //auth user and his claims
        //    //await Task.Delay(4000);
        //    var user = new ClaimsIdentity(new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.Role,"admin"),
        //        new Claim(ClaimTypes.Name, "Danilo")
        //    }, "admin");

        //    return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
        //}

        private readonly IJSRuntime js;
        private readonly HttpClient http;

        public static readonly string tokenKey = "tokenKey";
        private AuthenticationState noAuthenticate => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public AuthStateProvider(IJSRuntime jsruntime, HttpClient httpCliente)
        {
            js = jsruntime;
            http = httpCliente;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.GetItemFromLocalStorage(tokenKey);

            if (string.IsNullOrEmpty(token))
                return noAuthenticate;

            return CreateAuthenticationState(token);
        }

        public AuthenticationState CreateAuthenticationState(string token)
        {
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(string token)
        {
            await js.SetItemInLocalStorage(tokenKey, token);
            var authState = CreateAuthenticationState(token);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await js.RemoveItemInLocalStorage(tokenKey);
            http.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(noAuthenticate));
        }
    }
}