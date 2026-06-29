using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Book;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class BookService(HttpClient http)
    {
        public async Task<List<BookDto>> GetAll() =>
            await http.GetFromJsonAsync<List<BookDto>>(BookEndpoints.GetAll()) ?? [];

        public async Task<BookDto?> GetById(long id) =>
            await http.GetFromJsonAsync<BookDto>(BookEndpoints.GetById(id));
    }
}
