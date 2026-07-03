using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.FavoritesItem;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class FavoritesItemService(HttpClient http)
    {
        public async Task<List<FavoritesItemDto>> GetAll() =>
            await http.GetFromJsonAsync<List<FavoritesItemDto>>(FavoritesItemEndpoints.GetAll()) ?? [];

        public async Task<FavoritesItemDto?> GetById(long id)
        {
            var response = await http.GetAsync(FavoritesItemEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<FavoritesItemDto>();
        }

        public async Task<bool> Create(CreateFavoritesItemDto dto)
        {
            var response = await http.PostAsJsonAsync(FavoritesItemEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateFavoritesItemDto dto)
        {
            var response = await http.PutAsJsonAsync(FavoritesItemEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(FavoritesItemEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
