using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.StockItem;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class StockItemService(HttpClient http)
    {
        public async Task<List<StockItemDto>> GetAll() =>
            await http.GetFromJsonAsync<List<StockItemDto>>(StockItemEndpoints.GetAll()) ?? [];

        public async Task<StockItemDto?> GetById(long id)
        {
            var response = await http.GetAsync(StockItemEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<StockItemDto>();
        }

        public async Task<bool> Create(CreateStockItemDto dto)
        {
            var response = await http.PostAsJsonAsync(StockItemEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateStockItemDto dto)
        {
            var response = await http.PutAsJsonAsync(StockItemEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(StockItemEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
