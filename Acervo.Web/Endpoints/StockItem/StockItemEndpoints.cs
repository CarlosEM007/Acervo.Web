namespace Acervo.Web.Endpoints.StockItem
{
    public static class StockItemEndpoints
    {
        public static string GetAll()         => "stockitem";
        public static string GetById(long id) => $"stockitem/{id}";
        public static string Create()         => "stockitem";
        public static string Update()         => "stockitem";
        public static string Delete(long id)  => $"stockitem/{id}";
    }
}
