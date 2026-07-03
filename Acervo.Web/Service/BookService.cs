using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Book;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class BookService(HttpClient http)
    {
        public async Task<List<BookDto>> GetAll() =>
            await http.GetFromJsonAsync<List<BookDto>>(BookEndpoints.GetAll()) ?? [];

        public async Task<BookDto?> GetById(long id)
        {
            var response = await http.GetAsync(BookEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<BookDto>();
        }

        public async Task<bool> Create(CreateBookDto dto)
        {
            var response = await http.PostAsJsonAsync(BookEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateBookDto dto)
        {
            var response = await http.PutAsJsonAsync(BookEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(BookEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
