namespace Acervo.Web.Endpoints.Sale
{
    public static class SaleEndpoints
    {
        public static string GetAll()         => "sale";
        public static string GetById(long id) => $"sale/{id}";
        public static string Create()         => "sale";
        public static string Update()         => "sale";
        public static string Delete(long id)  => $"sale/{id}";
    }
}
