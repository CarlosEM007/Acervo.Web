namespace Acervo.Web.Endpoints.Library
{
    public static class LibraryEndpoints
    {
        public static string GetAll()         => "library";
        public static string GetById(long id) => $"library/{id}";
        public static string Create()         => "library";
        public static string Update()         => "library";
        public static string Delete(long id)  => $"library/{id}";
    }
}
