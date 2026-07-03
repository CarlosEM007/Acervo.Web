namespace Acervo.Web.Endpoints.Author
{
    public static class AuthorEndpoints
    {
        public static string GetAll()         => "author";
        public static string GetById(long id) => $"author/{id}";
        public static string Create()         => "author";
        public static string Update()         => "author";
        public static string Delete(long id)  => $"author/{id}";
    }
}
