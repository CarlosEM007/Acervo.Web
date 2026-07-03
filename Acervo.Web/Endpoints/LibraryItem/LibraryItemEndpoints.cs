namespace Acervo.Web.Endpoints.LibraryItem
{
    public static class LibraryItemEndpoints
    {
        public static string GetAll()         => "libraryitem";
        public static string GetById(long id) => $"libraryitem/{id}";
        public static string Create()         => "libraryitem";
        public static string Update()         => "libraryitem";
        public static string Delete(long id)  => $"libraryitem/{id}";
    }
}
