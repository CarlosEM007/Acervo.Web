using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Autores
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private AuthorService      AuthorSvc { get; set; } = default!;
        [Inject] private BookService        BookSvc   { get; set; } = default!;

        private record AuthorVm(long Id, string Name, string? Biography, int BookCount);

        private bool  IsLoading    { get; set; } = true;
        private string SearchQuery { get; set; } = string.Empty;
        private char?  SelectedLetter { get; set; }

        private static readonly char[] Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private List<AuthorVm> AllAuthors      { get; set; } = [];
        private List<AuthorVm> FilteredAuthors { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authTask  = AuthorSvc.GetAll();
                var bookTask  = BookSvc.GetAll();
                await Task.WhenAll(authTask, bookTask);

                var bookCount = bookTask.Result
                    .GroupBy(b => b.AuthorId)
                    .ToDictionary(g => g.Key, g => g.Count());

                AllAuthors = authTask.Result
                    .Select(a => new AuthorVm(
                        a.Id,
                        a.Name,
                        a.Biography,
                        bookCount.GetValueOrDefault(a.Id, 0)))
                    .OrderBy(a => a.Name)
                    .ToList();
            }
            catch { /* API offline */ }
            finally
            {
                IsLoading = false;
                FilterAuthors();
            }
        }

        private void FilterAuthors()
        {
            var q = SearchQuery.Trim().ToLowerInvariant();
            FilteredAuthors = AllAuthors
                .Where(a =>
                    (string.IsNullOrEmpty(q) || a.Name.Contains(q, StringComparison.OrdinalIgnoreCase)) &&
                    (SelectedLetter == null  || char.ToUpperInvariant(a.Name[0]) == SelectedLetter))
                .ToList();
        }

        private void SetLetter(char? letter)
        {
            SelectedLetter = letter;
            FilterAuthors();
        }

        private static string GetInitials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 2
                ? $"{parts[0][0]}{parts[^1][0]}"
                : name[..1].ToUpperInvariant();
        }

        private static string TruncateBio(string? bio) =>
            string.IsNullOrWhiteSpace(bio) ? "Autor do acervo."
            : bio.Length > 80 ? bio[..80] + "…"
            : bio;
    }
}
