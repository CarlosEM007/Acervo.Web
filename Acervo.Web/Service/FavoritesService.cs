using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Favorites;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class FavoritesService(HttpClient http)
    {
        public async Task<List<FavoritesDto>> GetAll() =>
            await http.GetFromJsonAsync<List<FavoritesDto>>(FavoritesEndpoints.GetAll()) ?? [];

        public async Task<FavoritesDto?> GetById(long id)
        {
            var response = await http.GetAsync(FavoritesEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<FavoritesDto>();
        }

        public async Task<bool> Create(CreateFavoritesDto dto)
        {
            var response = await http.PostAsJsonAsync(FavoritesEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateFavoritesDto dto)
        {
            var response = await http.PutAsJsonAsync(FavoritesEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(FavoritesEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
