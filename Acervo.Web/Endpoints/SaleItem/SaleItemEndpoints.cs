namespace Acervo.Web.Endpoints.SaleItem
{
    public static class SaleItemEndpoints
    {
        public static string GetAll()         => "saleitem";
        public static string GetById(long id) => $"saleitem/{id}";
        public static string Create()         => "saleitem";
        public static string Update()         => "saleitem";
        public static string Delete(long id)  => $"saleitem/{id}";
    }
}
