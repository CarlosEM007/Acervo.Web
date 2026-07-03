using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.LibraryItem;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class LibraryItemService(HttpClient http)
    {
        public async Task<List<LibraryItemDto>> GetAll() =>
            await http.GetFromJsonAsync<List<LibraryItemDto>>(LibraryItemEndpoints.GetAll()) ?? [];

        public async Task<LibraryItemDto?> GetById(long id)
        {
            var response = await http.GetAsync(LibraryItemEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<LibraryItemDto>();
        }

        public async Task<bool> Create(CreateLibraryItemDto dto)
        {
            var response = await http.PostAsJsonAsync(LibraryItemEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateLibraryItemDto dto)
        {
            var response = await http.PutAsJsonAsync(LibraryItemEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(LibraryItemEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
