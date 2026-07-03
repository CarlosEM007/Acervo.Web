using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Publisher;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class PublisherService(HttpClient http)
    {
        public async Task<List<PublisherDto>> GetAll() =>
            await http.GetFromJsonAsync<List<PublisherDto>>(PublisherEndpoints.GetAll()) ?? [];

        public async Task<PublisherDto?> GetById(long id)
        {
            var response = await http.GetAsync(PublisherEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<PublisherDto>();
        }

        public async Task<bool> Create(CreatePublisherDto dto)
        {
            var response = await http.PostAsJsonAsync(PublisherEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdatePublisherDto dto)
        {
            var response = await http.PutAsJsonAsync(PublisherEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(PublisherEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
