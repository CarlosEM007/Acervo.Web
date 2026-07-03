namespace Acervo.Web.Endpoints.Stock
{
    public static class StockEndpoints
    {
        public static string GetAll()         => "stock";
        public static string GetById(long id) => $"stock/{id}";
        public static string Create()         => "stock";
        public static string Update()         => "stock";
        public static string Delete(long id)  => $"stock/{id}";
    }
}
