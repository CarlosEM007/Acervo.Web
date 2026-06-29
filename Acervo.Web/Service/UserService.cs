using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Auth;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class UserService(HttpClient httpClient, SessionService session)
    {
        public async Task<bool> LoginUser(string email, string password)
        {
            var login = new LoginDto { Email = email, PasswordHash = password };

            var response = await httpClient.PostAsJsonAsync(AuthEndpoints.Login(), login);
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<TokenDto>();
            if (result?.Token == null) return false;

            session.SetToken(result.Token);
            return true;
        }
    }
}
