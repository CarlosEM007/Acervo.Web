using Acervo.Web.DTOs;
using Acervo.Web.Endpoints.Cart;
using System.Net.Http.Json;

namespace Acervo.Web.Service
{
    public class CartService(HttpClient http)
    {
        public async Task<List<CartDto>> GetAll() =>
            await http.GetFromJsonAsync<List<CartDto>>(CartEndpoints.GetAll()) ?? [];

        public async Task<CartDto?> GetById(long id)
        {
            var response = await http.GetAsync(CartEndpoints.GetById(id));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<CartDto>();
        }

        public async Task<bool> Create(CreateCartDto dto)
        {
            var response = await http.PostAsJsonAsync(CartEndpoints.Create(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateCartDto dto)
        {
            var response = await http.PutAsJsonAsync(CartEndpoints.Update(), dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(long id)
        {
            var response = await http.DeleteAsync(CartEndpoints.Delete(id));
            return response.IsSuccessStatusCode;
        }
    }
}
