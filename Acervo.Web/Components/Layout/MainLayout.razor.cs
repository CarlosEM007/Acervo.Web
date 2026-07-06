using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Layout
{
    public partial class MainLayout
    {
        [Inject] private NavigationManager Navigation  { get; set; } = default!;
        [Inject] private SessionService    Session     { get; set; } = default!;
        [Inject] private CartService       CartSvc     { get; set; } = default!;
        [Inject] private CartItemService   CartItemSvc { get; set; } = default!;

        private bool   SidebarOpen { get; set; } = false;
        private string SearchQuery { get; set; } = string.Empty;
        private int    CartCount   { get; set; } = 0;

        private string AvatarInitial =>
            string.IsNullOrWhiteSpace(Session.Email) ? "U" : Session.Email![..1].ToUpperInvariant();

        protected override async Task OnInitializedAsync()
        {
            if (Session.UserId is not long userId) return;

            try
            {
                var cart = await CartSvc.GetByUser(userId);
                if (cart is not null)
                    CartCount = (await CartItemSvc.GetAll()).Count(ci => ci.CartId == cart.Id);
            }
            catch
            {
                // Falha ao obter o carrinho não deve quebrar o layout.
            }
        }

        private void ToggleSidebar() => SidebarOpen = !SidebarOpen;

        private void HandleSearchKey(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !string.IsNullOrWhiteSpace(SearchQuery))
                Navigation.NavigateTo($"/busca?q={Uri.EscapeDataString(SearchQuery)}");
        }

        private void Logout()
        {
            Session.Clear();
            Navigation.NavigateTo("/");
        }
    }
}
