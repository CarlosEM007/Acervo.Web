namespace Acervo.Web.Endpoints.Book
{
    public static class BookEndpoints
    {
        public static string GetAll()         => "book";
        public static string GetById(long id) => $"book/{id}";
        public static string Create()         => "book";
        public static string Update()         => "book";
        public static string Delete(long id)  => $"book/{id}";
    }
}
