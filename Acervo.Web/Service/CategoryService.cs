using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Category;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class CategoryService(HttpClient http)
    {
        public async Task<List<CategoryDto>> GetAll() =>
            await http.GetFromJsonAsync<List<CategoryDto>>(CategoryEndpoints.GetAll()) ?? [];

        public async Task<CategoryDto?> GetById(long id)
        {
            var response = await http.GetAsync(CategoryEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<CategoryDto>();
        }

        public async Task<bool> Create(CreateCategoryDto dto)
        {
            var response = await http.PostAsJsonAsync(CategoryEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateCategoryDto dto)
        {
            var response = await http.PutAsJsonAsync(CategoryEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(CategoryEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
