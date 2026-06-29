using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.StockItem;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class StockItemService(HttpClient http)
    {
        public async Task<List<StockItemDto>> GetAll() =>
            await http.GetFromJsonAsync<List<StockItemDto>>(StockItemEndpoints.GetAll()) ?? [];
    }
}
