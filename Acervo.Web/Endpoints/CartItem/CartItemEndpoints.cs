namespace Acervo.Web.Endpoints.CartItem
{
    public static class CartItemEndpoints
    {
        public static string GetAll()         => "cartitem";
        public static string GetById(long id) => $"cartitem/{id}";
        public static string Create()         => "cartitem";
        public static string Update()         => "cartitem";
        public static string Delete(long id)  => $"cartitem/{id}";
    }
}
