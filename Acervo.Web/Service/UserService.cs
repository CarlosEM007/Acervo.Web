using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Auth;
using Acervo.Web.Endpoints.User;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class UserService(HttpClient httpClient, SessionService session)
    {
        // ── Autenticação ───────────────────────────────────────────
        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                var login = new LoginDto { Email = email, PasswordHash = password };

                var response = await httpClient.PostAsJsonAsync(AuthEndpoints.Login(), login);
                if (!response.IsSuccessStatusCode) return false;

                var result = await response.Content.ReadFromJsonAsync<TokenDto>();
                if (result?.Token == null) return false;

                session.SetToken(result.Token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] LoginUser error: {ex.Message}");
                return false;
            }
        }

        // ── CRUD ───────────────────────────────────────────────────
        public async Task<List<UserDto>> GetAll() =>
            await httpClient.GetFromJsonAsync<List<UserDto>>(UserEndpoints.GetAll()) ?? [];

        public async Task<UserDto?> GetById(long id)
        {
            var response = await httpClient.GetAsync(UserEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<bool> Create(CreateUserDto dto)
        {
            var response = await httpClient.PostAsJsonAsync(UserEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateUserDto dto)
        {
            var response = await httpClient.PutAsJsonAsync(UserEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await httpClient.DeleteAsync(UserEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
