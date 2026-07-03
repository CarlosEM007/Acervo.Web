namespace Acervo.Web.Endpoints.FavoritesItem
{
    public static class FavoritesItemEndpoints
    {
        public static string GetAll()         => "favoritesitem";
        public static string GetById(long id) => $"favoritesitem/{id}";
        public static string Create()         => "favoritesitem";
        public static string Update()         => "favoritesitem";
        public static string Delete(long id)  => $"favoritesitem/{id}";
    }
}
