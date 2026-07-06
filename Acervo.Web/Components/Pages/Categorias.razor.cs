using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Categorias
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private CategoryService   CatSvc     { get; set; } = default!;
        [Inject] private BookService       BookSvc    { get; set; } = default!;
        [Inject] private ToastService      Toast      { get; set; } = default!;

        private record CategoryVm(string Name, string Icon, string Slug, string Description, int Count);

        private bool IsLoading { get; set; } = true;
        private List<CategoryVm> Categories { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var catTask  = CatSvc.GetAll();
                var bookTask = BookSvc.GetAll();
                await Task.WhenAll(catTask, bookTask);

                var bookCount = bookTask.Result
                    .GroupBy(b => b.CategoryId)
                    .ToDictionary(g => g.Key, g => g.Count());

                Categories = catTask.Result
                    .Select(c => new CategoryVm(
                        c.Description,
                        CategoryPresentation.IconFor(c.Description),
                        CategoryPresentation.Slugify(c.Description),
                        CategoryPresentation.DescriptionFor(c.Description),
                        bookCount.GetValueOrDefault(c.Id, 0)))
                    .OrderByDescending(c => c.Count)
                    .ToList();
            }
            catch
            {
                Toast.ShowError("Não foi possível carregar as categorias.");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
