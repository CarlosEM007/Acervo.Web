namespace Acervo.Web.Endpoints.Seller
{
    public static class SellerEndpoints
    {
        public static string GetAll()         => "seller";
        public static string GetById(long id) => $"seller/{id}";
        public static string Create()         => "seller";
        public static string Update()         => "seller";
        public static string Delete(long id)  => $"seller/{id}";
    }
}
