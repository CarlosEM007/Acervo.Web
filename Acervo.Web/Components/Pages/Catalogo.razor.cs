using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Catalogo
    {
        [Inject] private NavigationManager Navigation  { get; set; } = default!;
        [Inject] private BookService        BookSvc    { get; set; } = default!;
        [Inject] private AuthorService      AuthorSvc  { get; set; } = default!;
        [Inject] private CategoryService    CatSvc     { get; set; } = default!;
        [Inject] private StockItemService   StockSvc   { get; set; } = default!;

        // ── Estado de UI ───────────────────────────────────────────
        private bool IsLoading { get; set; } = true;
        private bool FiltersOpen { get; set; } = false;
        private string ViewMode { get; set; } = "grid";

        // ── Filtros ────────────────────────────────────────────────
        private string SearchQuery { get; set; } = string.Empty;
        private List<long> SelectedCategories { get; set; } = [];
        private string? PriceMin { get; set; }
        private string? PriceMax { get; set; }
        private string SelectedSort { get; set; } = "lancamento";

        // ── Paginação ──────────────────────────────────────────────
        private int CurrentPage { get; set; } = 1;
        private const int PageSize = 16;
        private int TotalPages => (int)Math.Ceiling((double)TotalBooks / PageSize);
        private int TotalBooks { get; set; } = 0;

        private List<(string value, string label)> SortOptions { get; } =
        [
            ("lancamento",  "Lançamento"),
            ("titulo",      "Título (A–Z)"),
            ("titulo-desc", "Título (Z–A)"),
            ("preco-asc",   "Menor preço"),
            ("preco-desc",  "Maior preço"),
        ];

        private record BookVm(long Id, string Title, string AuthorName, string CategoryName,
            string Description, decimal Price, string? CoverImageUrl);

        private record CategoryFilterVm(long Id, string Description, int Count);

        private List<CategoryFilterVm> AvailableCategories { get; set; } = [];
        private List<BookVm>           AllBooks            { get; set; } = [];
        private List<BookVm>           FilteredBooks       { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var booksTask  = BookSvc.GetAll();
                var authTask   = AuthorSvc.GetAll();
                var catTask    = CatSvc.GetAll();
                var stockTask  = StockSvc.GetAll();

                await Task.WhenAll(booksTask, authTask, catTask, stockTask);

                var books      = booksTask.Result;
                var authors    = authTask.Result.ToDictionary(a => a.Id, a => a.Name);
                var categories = catTask.Result;
                var prices     = stockTask.Result
                                    .GroupBy(s => s.BookId)
                                    .ToDictionary(g => g.Key, g => g.Min(s => s.Price));

                var catById = categories.ToDictionary(c => c.Id, c => c.Description);

                AllBooks = books.Select(b => new BookVm(
                    b.Id,
                    b.Title,
                    authors.GetValueOrDefault(b.AuthorId, "—"),
                    catById.GetValueOrDefault(b.CategoryId, "—"),
                    b.Description,
                    prices.GetValueOrDefault(b.Id, 0m),
                    string.IsNullOrEmpty(b.CoverImageUrl) ? null : b.CoverImageUrl))
                    .ToList();

                // Categorias disponíveis: apenas as que têm livros
                AvailableCategories = categories
                    .Select(c => new CategoryFilterVm(
                        c.Id,
                        c.Description,
                        books.Count(b => b.CategoryId == c.Id)))
                    .Where(c => c.Count > 0)
                    .OrderByDescending(c => c.Count)
                    .ToList();
            }
            catch { /* API offline */ }
            finally
            {
                IsLoading = false;
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            var q = SearchQuery.Trim().ToLowerInvariant();
            var result = AllBooks.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(q))
                result = result.Where(b =>
                    b.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    b.AuthorName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    b.CategoryName.Contains(q, StringComparison.OrdinalIgnoreCase));

            if (SelectedCategories.Count > 0)
                result = result.Where(b =>
                    AvailableCategories
                        .Where(c => SelectedCategories.Contains(c.Id))
                        .Any(c => c.Description == b.CategoryName));

            if (decimal.TryParse(PriceMin, out var min))
                result = result.Where(b => b.Price >= min);

            if (decimal.TryParse(PriceMax, out var max))
                result = result.Where(b => b.Price <= max);

            result = SelectedSort switch
            {
                "titulo"      => result.OrderBy(b => b.Title),
                "titulo-desc" => result.OrderByDescending(b => b.Title),
                "preco-asc"   => result.OrderBy(b => b.Price),
                "preco-desc"  => result.OrderByDescending(b => b.Price),
                _             => result.OrderByDescending(b => b.Id)
            };

            var list = result.ToList();
            TotalBooks  = list.Count;
            CurrentPage = 1;
            FilteredBooks = list.Take(PageSize).ToList();
        }

        private void GoToPage(int page)
        {
            if (page < 1 || page > TotalPages) return;
            CurrentPage   = page;
            var q         = SearchQuery.Trim().ToLowerInvariant();
            FilteredBooks = AllBooks
                .Where(b => string.IsNullOrWhiteSpace(q) ||
                    b.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    b.AuthorName.Contains(q, StringComparison.OrdinalIgnoreCase))
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        private void ToggleCategory(long id, bool selected)
        {
            if (selected) SelectedCategories.Add(id);
            else SelectedCategories.Remove(id);
        }

        private void ClearFilters()
        {
            SearchQuery = string.Empty;
            SelectedCategories.Clear();
            PriceMin = null;
            PriceMax = null;
            SelectedSort = "lancamento";
            ApplyFilters();
        }
    }
}
