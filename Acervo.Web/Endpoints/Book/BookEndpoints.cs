namespace Acervo.Web.Endpoints.Book
{
    public static class BookEndpoints
    {
        public static string GetAll()      => "book";
        public static string GetById(long id) => $"book/{id}";
    }
}
