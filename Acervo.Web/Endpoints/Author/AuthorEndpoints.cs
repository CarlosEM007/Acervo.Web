namespace Acervo.Web.Endpoints.Author
{
    public static class AuthorEndpoints
    {
        public static string GetAll()         => "author";
        public static string GetById(long id) => $"author/{id}";
    }
}
