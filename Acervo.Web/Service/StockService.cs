using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Stock;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class StockService(HttpClient http)
    {
        public async Task<List<StockDto>> GetAll() =>
            await http.GetFromJsonAsync<List<StockDto>>(StockEndpoints.GetAll()) ?? [];

        public async Task<StockDto?> GetById(long id)
        {
            var response = await http.GetAsync(StockEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<StockDto>();
        }

        public async Task<bool> Create(CreateStockDto dto)
        {
            var response = await http.PostAsJsonAsync(StockEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateStockDto dto)
        {
            var response = await http.PutAsJsonAsync(StockEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(StockEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
