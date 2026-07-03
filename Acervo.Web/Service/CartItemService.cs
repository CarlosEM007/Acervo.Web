using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.CartItem;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class CartItemService(HttpClient http)
    {
        public async Task<List<CartItemDto>> GetAll() =>
            await http.GetFromJsonAsync<List<CartItemDto>>(CartItemEndpoints.GetAll()) ?? [];

        public async Task<CartItemDto?> GetById(long id)
        {
            var response = await http.GetAsync(CartItemEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<CartItemDto>();
        }

        public async Task<bool> Create(CreateCartItemDto dto)
        {
            var response = await http.PostAsJsonAsync(CartItemEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateCartItemDto dto)
        {
            var response = await http.PutAsJsonAsync(CartItemEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(CartItemEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
