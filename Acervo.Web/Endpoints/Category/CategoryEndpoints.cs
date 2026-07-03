namespace Acervo.Web.Endpoints.Category
{
    public static class CategoryEndpoints
    {
        public static string GetAll()         => "category";
        public static string GetById(long id) => $"category/{id}";
        public static string Create()         => "category";
        public static string Update()         => "category";
        public static string Delete(long id)  => $"category/{id}";
    }
}
