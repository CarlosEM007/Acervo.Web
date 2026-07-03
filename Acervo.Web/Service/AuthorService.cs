using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Author;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class AuthorService(HttpClient http)
    {
        public async Task<List<AuthorDto>> GetAll() =>
            await http.GetFromJsonAsync<List<AuthorDto>>(AuthorEndpoints.GetAll()) ?? [];

        public async Task<AuthorDto?> GetById(long id)
        {
            var response = await http.GetAsync(AuthorEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AuthorDto>();
        }

        public async Task<bool> Create(CreateAuthorDto dto)
        {
            var response = await http.PostAsJsonAsync(AuthorEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateAuthorDto dto)
        {
            var response = await http.PutAsJsonAsync(AuthorEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(AuthorEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
