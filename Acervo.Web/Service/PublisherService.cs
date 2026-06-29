using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Publisher;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class PublisherService(HttpClient http)
    {
        public async Task<List<PublisherDto>> GetAll() =>
            await http.GetFromJsonAsync<List<PublisherDto>>(PublisherEndpoints.GetAll()) ?? [];
    }
}
