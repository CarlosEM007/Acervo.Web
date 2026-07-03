using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Sale;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class SaleService(HttpClient http)
    {
        public async Task<List<SaleDto>> GetAll() =>
            await http.GetFromJsonAsync<List<SaleDto>>(SaleEndpoints.GetAll()) ?? [];

        public async Task<SaleDto?> GetById(long id)
        {
            var response = await http.GetAsync(SaleEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<SaleDto>();
        }

        public async Task<bool> Create(CreateSaleDto dto)
        {
            var response = await http.PostAsJsonAsync(SaleEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateSaleDto dto)
        {
            var response = await http.PutAsJsonAsync(SaleEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(SaleEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
