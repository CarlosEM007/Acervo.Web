namespace Acervo.Web.Endpoints.Category
{
    public static class CategoryEndpoints
    {
        public static string GetAll()         => "category";
        public static string GetById(long id) => $"category/{id}";
    }
}
