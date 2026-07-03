namespace Acervo.Web.Endpoints.Cart
{
    public static class CartEndpoints
    {
        public static string GetAll()         => "cart";
        public static string GetById(long id) => $"cart/{id}";
        public static string Create()         => "cart";
        public static string Update()         => "cart";
        public static string Delete(long id)  => $"cart/{id}";
    }
}
