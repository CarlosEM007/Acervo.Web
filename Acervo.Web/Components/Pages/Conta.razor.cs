using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Acervo.Web.Components.Pages
{
    public partial class Conta
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        // ── Dados do usuário ───────────────────────────────────────
        private string UserName { get; set; } = "João Silva";
        private string UserEmail { get; set; } = "joao.silva@email.com";

        // ── Edição de perfil ───────────────────────────────────────
        private string EditName { get; set; } = "João Silva";
        private string EditEmail { get; set; } = "joao.silva@email.com";
        private string? ProfileMsg { get; set; }

        // ── Senha ──────────────────────────────────────────────────
        private string? CurrentPassword { get; set; }
        private string? NewPassword { get; set; }
        private string? ConfirmPassword { get; set; }
        private string? PasswordMsg { get; set; }
        private bool PasswordSuccess { get; set; }

        // ── Tabs ───────────────────────────────────────────────────
        private string ActiveTab { get; set; } = "perfil";

        protected override void OnInitialized()
        {
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("tab", out var tab))
                ActiveTab = tab!;
        }

        private record TabItem(string Key, string Icon, string Label);

        private List<TabItem> Tabs { get; } = new()
        {
            new("perfil",     "fa-solid fa-user",     "Perfil"),
            new("pedidos",    "fa-solid fa-box",      "Pedidos"),
            new("biblioteca", "fa-solid fa-book",     "Minha Biblioteca"),
        };

        // ── Pedidos mock ───────────────────────────────────────────
        private record OrderVm(long Id, DateTime Date, string StatusLabel, string StatusSlug,
            List<string> Items, decimal Total);

        private List<OrderVm> Orders { get; } = new()
        {
            new(1001, new DateTime(2025, 5, 12), "Concluído", "completed",
                new() { "Fundação", "1984" }, 74.80m),
            new(1002, new DateTime(2025, 6, 1), "Pendente", "pending",
                new() { "Duna" }, 49.90m),
        };

        // ── Biblioteca mock ────────────────────────────────────────
        private record LibraryBookVm(string Title, string AuthorName, int Progress);

        private List<LibraryBookVm> LibraryBooks { get; } = new()
        {
            new("Fundação", "Isaac Asimov", 68),
            new("1984", "George Orwell", 100),
        };

        // ── Handlers ──────────────────────────────────────────────
        private async Task SaveProfile()
        {
            await Task.Delay(300);
            UserName = EditName;
            UserEmail = EditEmail;
            ProfileMsg = "Perfil atualizado com sucesso!";
            StateHasChanged();
            await Task.Delay(3000);
            ProfileMsg = null;
        }

        private async Task ChangePassword()
        {
            if (NewPassword != ConfirmPassword)
            {
                PasswordMsg = "As senhas não coincidem.";
                PasswordSuccess = false;
                return;
            }

            await Task.Delay(300);
            PasswordMsg = "Senha alterada com sucesso!";
            PasswordSuccess = true;
            CurrentPassword = NewPassword = ConfirmPassword = null;
            StateHasChanged();
            await Task.Delay(3000);
            PasswordMsg = null;
        }

        private static string GetInitials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 2
                ? $"{parts[0][0]}{parts[^1][0]}"
                : name[..1].ToUpperInvariant();
        }
    }
}
