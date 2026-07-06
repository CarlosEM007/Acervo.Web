using Acervo.Web.DTOs;

namespace Acervo.Web.Service
{
    /// <summary>
    /// Orquestra as operações de carrinho do usuário logado sobre a API
    /// (obter-ou-criar carrinho, adicionar livro mesclando quantidade, etc.).
    /// Centraliza a lógica compartilhada entre as telas de Catálogo/Detalhes/Favoritos.
    /// </summary>
    public class CartManager(CartService carts, CartItemService cartItems, SessionService session)
    {
        public async Task<CartDto?> GetOrCreateForCurrentUser()
        {
            if (session.UserId is not long userId) return null;

            var cart = await carts.GetByUser(userId);
            if (cart is not null) return cart;

            if (!await carts.Create(new CreateCartDto(userId))) return null;
            return await carts.GetByUser(userId);
        }

        /// <summary>Adiciona um livro ao carrinho; se já existir, incrementa a quantidade.</summary>
        public async Task<bool> AddBook(long bookId, decimal unitPrice, int quantity = 1)
        {
            var cart = await GetOrCreateForCurrentUser();
            if (cart is null) return false;

            var existing = (await cartItems.GetAll())
                .FirstOrDefault(ci => ci.CartId == cart.Id && ci.BookId == bookId);

            if (existing is not null)
                return await cartItems.Update(new UpdateCartItemDto(
                    existing.Id, cart.Id, bookId, existing.UnitPrice, existing.Quantity + quantity));

            return await cartItems.Create(new CreateCartItemDto(cart.Id, bookId, unitPrice, quantity));
        }
    }
}
