using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Author;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class AuthorService(HttpClient http)
    {
        public async Task<List<AuthorDto>> GetAll() =>
            await http.GetFromJsonAsync<List<AuthorDto>>(AuthorEndpoints.GetAll()) ?? [];

        public async Task<AuthorDto?> GetById(long id) =>
            await http.GetFromJsonAsync<AuthorDto>(AuthorEndpoints.GetById(id));
    }
}
