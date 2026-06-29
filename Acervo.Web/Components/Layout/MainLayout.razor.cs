using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Layout
{
    public partial class MainLayout
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private bool SidebarOpen { get; set; } = false;
        private string SearchQuery { get; set; } = string.Empty;
        private int CartCount { get; set; } = 0;

        private void ToggleSidebar() => SidebarOpen = !SidebarOpen;

        private void HandleSearchKey(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !string.IsNullOrWhiteSpace(SearchQuery))
                Navigation.NavigateTo($"/busca?q={Uri.EscapeDataString(SearchQuery)}");
        }

        private void Logout()
        {
            Navigation.NavigateTo("/");
        }
    }
}