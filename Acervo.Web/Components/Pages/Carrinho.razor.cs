using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Carrinho
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        // ── Modelo de view ─────────────────────────────────────────
        private class CartItemVm
        {
            public long BookId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string AuthorName { get; set; } = string.Empty;
            public string CategoryName { get; set; } = string.Empty;
            public string? CoverImageUrl { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal SubTotal => UnitPrice * Quantity;
        }

        // ── Estado ─────────────────────────────────────────────────
        private List<CartItemVm> Items { get; set; } = new()
        {
            new() { BookId = 1, Title = "Fundação", AuthorName = "Isaac Asimov",
                    CategoryName = "Ficção Científica", UnitPrice = 39.90m, Quantity = 1 },
            new() { BookId = 2, Title = "1984", AuthorName = "George Orwell",
                    CategoryName = "Ficção", UnitPrice = 34.90m, Quantity = 2 },
        };

        private string? CouponCode { get; set; }
        private string? CouponMessage { get; set; }
        private bool CouponSuccess { get; set; }
        private decimal Discount { get; set; } = 0;

        // ── Computed ───────────────────────────────────────────────
        private decimal Subtotal => Items.Sum(i => i.SubTotal);
        private decimal Total => Subtotal - Discount;

        // ── Handlers ──────────────────────────────────────────────
        private void IncreaseQty(CartItemVm item) => item.Quantity++;

        private void DecreaseQty(CartItemVm item)
        {
            if (item.Quantity > 1) item.Quantity--;
        }

        private void RemoveItem(CartItemVm item) => Items.Remove(item);

        private void ClearCart() => Items.Clear();

        private void ApplyCoupon()
        {
            if (CouponCode?.ToUpperInvariant() == "ACERVO10")
            {
                Discount = Subtotal * 0.10m;
                CouponMessage = "Cupom aplicado! 10% de desconto.";
                CouponSuccess = true;
            }
            else
            {
                Discount = 0;
                CouponMessage = "Cupom inválido ou expirado.";
                CouponSuccess = false;
            }
        }

        private void Checkout()
        {
            // Substituir pela lógica de checkout real
            Navigation.NavigateTo("/checkout");
        }
    }
}
