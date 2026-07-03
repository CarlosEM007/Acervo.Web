using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Library;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class LibraryService(HttpClient http)
    {
        public async Task<List<LibraryDto>> GetAll() =>
            await http.GetFromJsonAsync<List<LibraryDto>>(LibraryEndpoints.GetAll()) ?? [];

        public async Task<LibraryDto?> GetById(long id)
        {
            var response = await http.GetAsync(LibraryEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<LibraryDto>();
        }

        public async Task<bool> Create(CreateLibraryDto dto)
        {
            var response = await http.PostAsJsonAsync(LibraryEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateLibraryDto dto)
        {
            var response = await http.PutAsJsonAsync(LibraryEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(LibraryEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
