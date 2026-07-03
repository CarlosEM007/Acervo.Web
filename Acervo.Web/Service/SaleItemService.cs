using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.SaleItem;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class SaleItemService(HttpClient http)
    {
        public async Task<List<SaleItemDto>> GetAll() =>
            await http.GetFromJsonAsync<List<SaleItemDto>>(SaleItemEndpoints.GetAll()) ?? [];

        public async Task<SaleItemDto?> GetById(long id)
        {
            var response = await http.GetAsync(SaleItemEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<SaleItemDto>();
        }

        public async Task<bool> Create(CreateSaleItemDto dto)
        {
            var response = await http.PostAsJsonAsync(SaleItemEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateSaleItemDto dto)
        {
            var response = await http.PutAsJsonAsync(SaleItemEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(SaleItemEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
