using Acervo.Web.DTOs;
using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Acervo.Web.Components.Pages
{
    public partial class Conta
    {
        [Inject] private NavigationManager  Navigation  { get; set; } = default!;
        [Inject] private SessionService     Session     { get; set; } = default!;
        [Inject] private UserService        UserSvc     { get; set; } = default!;
        [Inject] private SaleService        SaleSvc     { get; set; } = default!;
        [Inject] private SaleItemService    SaleItemSvc { get; set; } = default!;
        [Inject] private LibraryService     LibrarySvc  { get; set; } = default!;
        [Inject] private LibraryItemService LibItemSvc  { get; set; } = default!;
        [Inject] private BookService        BookSvc     { get; set; } = default!;
        [Inject] private AuthorService      AuthorSvc   { get; set; } = default!;
        [Inject] private ToastService       Toast       { get; set; } = default!;

        // ── Dados do usuário ───────────────────────────────────────
        private string UserName  { get; set; } = string.Empty;
        private string UserEmail { get; set; } = string.Empty;

        // ── Edição de perfil ───────────────────────────────────────
        private string EditName  { get; set; } = string.Empty;
        private string EditEmail { get; set; } = string.Empty;

        // ── Tabs ───────────────────────────────────────────────────
        private string ActiveTab { get; set; } = "perfil";

        private record TabItem(string Key, string Icon, string Label);

        private List<TabItem> Tabs { get; } = new()
        {
            new("perfil",     "fa-solid fa-user", "Perfil"),
            new("pedidos",    "fa-solid fa-box",  "Pedidos"),
            new("biblioteca", "fa-solid fa-book", "Minha Biblioteca"),
        };

        // ── Pedidos ────────────────────────────────────────────────
        private record OrderVm(long Id, DateTime Date, string StatusLabel, string StatusSlug,
            List<string> Items, decimal Total);

        private List<OrderVm> Orders { get; set; } = [];

        // ── Biblioteca ─────────────────────────────────────────────
        private record LibraryBookVm(string Title, string AuthorName, int Progress);

        private List<LibraryBookVm> LibraryBooks { get; set; } = [];

        protected override void OnInitialized()
        {
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("tab", out var tab))
                ActiveTab = tab!;
        }

        protected override async Task OnInitializedAsync()
        {
            if (Session.UserId is not long userId)
            {
                Toast.ShowError("Faça login para acessar sua conta.");
                return;
            }

            try
            {
                await Task.WhenAll(
                    LoadProfile(userId),
                    LoadOrders(userId),
                    LoadLibrary(userId));
            }
            catch
            {
                Toast.ShowError("Não foi possível carregar os dados da conta.");
            }
        }

        private async Task LoadProfile(long userId)
        {
            var user = await UserSvc.GetById(userId);
            if (user is null) return;

            UserName  = user.Name;
            UserEmail = user.Email;
            EditName  = user.Name;
            EditEmail = user.Email;
        }

        private async Task LoadOrders(long userId)
        {
            var salesTask     = SaleSvc.GetAll();
            var saleItemsTask = SaleItemSvc.GetAll();
            var booksTask     = BookSvc.GetAll();

            await Task.WhenAll(salesTask, saleItemsTask, booksTask);

            var books     = booksTask.Result.ToDictionary(b => b.Id, b => b.Title);
            var itemsBySale = saleItemsTask.Result
                .GroupBy(si => si.SaleId)
                .ToDictionary(g => g.Key, g => g.ToList());

            Orders = salesTask.Result
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.Id)
                .Select(s =>
                {
                    var titles = itemsBySale.GetValueOrDefault(s.Id, [])
                        .Select(si =>
                        {
                            var title = books.GetValueOrDefault(si.BookId, "Livro");
                            return si.Quantity > 1 ? $"{title} (x{si.Quantity})" : title;
                        })
                        .ToList();

                    var (label, slug) = StatusInfo(s.Status);
                    return new OrderVm(s.Id, s.CreatedAt, label, slug, titles, s.TotalAmount);
                })
                .ToList();
        }

        private async Task LoadLibrary(long userId)
        {
            var library = await LibrarySvc.GetByUser(userId);
            if (library is null)
            {
                LibraryBooks = [];
                return;
            }

            var itemsTask = LibItemSvc.GetAll();
            var booksTask = BookSvc.GetAll();
            var authTask  = AuthorSvc.GetAll();

            await Task.WhenAll(itemsTask, booksTask, authTask);

            var books   = booksTask.Result.ToDictionary(b => b.Id);
            var authors = authTask.Result.ToDictionary(a => a.Id, a => a.Name);

            LibraryBooks = itemsTask.Result
                .Where(li => li.LibraryId == library.Id)
                .Select(li =>
                {
                    books.TryGetValue(li.BookId, out var book);
                    return new LibraryBookVm(
                        book?.Title ?? "—",
                        book is null ? "—" : authors.GetValueOrDefault(book.AuthorId, "—"),
                        li.ReadingProgress);
                })
                .ToList();
        }

        private async Task SaveProfile()
        {
            if (Session.UserId is not long userId) return;

            if (string.IsNullOrWhiteSpace(EditName) || string.IsNullOrWhiteSpace(EditEmail))
            {
                Toast.ShowError("Nome e e-mail são obrigatórios.");
                return;
            }

            try
            {
                if (await UserSvc.Update(new UpdateUserDto(userId, EditName, EditEmail)))
                {
                    UserName  = EditName;
                    UserEmail = EditEmail;
                    Toast.ShowSuccess("Perfil atualizado com sucesso!");
                }
                else
                {
                    Toast.ShowError("Não foi possível atualizar o perfil.");
                }
            }
            catch
            {
                Toast.ShowError("Erro ao comunicar com o servidor.");
            }
        }

        private static (string Label, string Slug) StatusInfo(int status) => status switch
        {
            1 => ("Pendente",  "pending"),
            2 => ("Confirmado", "confirmed"),
            3 => ("Concluído", "completed"),
            4 => ("Cancelado", "cancelled"),
            _ => ("—",         "pending"),
        };

        private static string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "U";

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 2
                ? $"{parts[0][0]}{parts[^1][0]}"
                : name[..1].ToUpperInvariant();
        }
    }
}
