using Acervo.Web.DTOs;
using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Home
    {
        [Inject] private NavigationManager Navigation  { get; set; } = default!;
        [Inject] private BookService        BookSvc    { get; set; } = default!;
        [Inject] private AuthorService      AuthorSvc  { get; set; } = default!;
        [Inject] private CategoryService    CatSvc     { get; set; } = default!;
        [Inject] private StockItemService   StockSvc   { get; set; } = default!;
        [Inject] private ToastService       Toast      { get; set; } = default!;

        private record BookVm(long Id, string Title, string AuthorName, string CategoryName,
            decimal Price, decimal? OriginalPrice, string? CoverImageUrl);

        private record CategoryVm(string Name, string Icon, string Slug, int Count);

        private bool IsLoading { get; set; } = true;

        private List<CategoryVm> FeaturedCategories { get; set; } = [];
        private List<BookVm>     NewReleases        { get; set; } = [];
        private List<BookVm>     BestSellers        { get; set; } = [];

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
                var catById    = categories.ToDictionary(c => c.Id, c => c.Description);
                var prices     = stockTask.Result
                                    .GroupBy(s => s.BookId)
                                    .ToDictionary(g => g.Key, g => g.Min(s => s.Price));

                BookVm ToVm(BookDto b) => new(
                    b.Id,
                    b.Title,
                    authors.GetValueOrDefault(b.AuthorId, "—"),
                    catById.GetValueOrDefault(b.CategoryId, "—"),
                    prices.GetValueOrDefault(b.Id, 0m),
                    null,
                    string.IsNullOrEmpty(b.CoverImageUrl) ? null : b.CoverImageUrl);

                // Lançamentos = 4 mais recentes
                NewReleases = books
                    .OrderByDescending(b => b.Release)
                    .Take(4)
                    .Select(ToVm)
                    .ToList();

                // Mais vendidos = 4 seguintes (ordem aleatória estável)
                BestSellers = books
                    .OrderBy(b => b.Id)
                    .Skip(4)
                    .Take(4)
                    .Select(ToVm)
                    .ToList();

                // Categorias em destaque = as 8 com mais títulos (dados reais da API)
                var bookCount = books
                    .GroupBy(b => b.CategoryId)
                    .ToDictionary(g => g.Key, g => g.Count());

                FeaturedCategories = categories
                    .Select(c => new CategoryVm(
                        c.Description,
                        CategoryPresentation.IconFor(c.Description),
                        CategoryPresentation.Slugify(c.Description),
                        bookCount.GetValueOrDefault(c.Id, 0)))
                    .OrderByDescending(c => c.Count)
                    .Take(8)
                    .ToList();
            }
            catch
            {
                Toast.ShowError("Não foi possível carregar a página inicial.");
            }
            finally { IsLoading = false; }
        }
    }
}
