using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Seller;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class SellerService(HttpClient http)
    {
        public async Task<List<SellerDto>> GetAll() =>
            await http.GetFromJsonAsync<List<SellerDto>>(SellerEndpoints.GetAll()) ?? [];

        public async Task<SellerDto?> GetById(long id)
        {
            var response = await http.GetAsync(SellerEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<SellerDto>();
        }

        public async Task<bool> Create(CreateSellerDto dto)
        {
            var response = await http.PostAsJsonAsync(SellerEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateSellerDto dto)
        {
            var response = await http.PutAsJsonAsync(SellerEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(SellerEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
