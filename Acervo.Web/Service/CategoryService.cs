using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Category;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class CategoryService(HttpClient http)
    {
        public async Task<List<CategoryDto>> GetAll() =>
            await http.GetFromJsonAsync<List<CategoryDto>>(CategoryEndpoints.GetAll()) ?? [];
    }
}
