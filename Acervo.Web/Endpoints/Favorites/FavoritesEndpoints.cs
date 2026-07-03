namespace Acervo.Web.Endpoints.Favorites
{
    public static class FavoritesEndpoints
    {
        public static string GetAll()         => "favorites";
        public static string GetById(long id) => $"favorites/{id}";
        public static string Create()         => "favorites";
        public static string Update()         => "favorites";
        public static string Delete(long id)  => $"favorites/{id}";
    }
}
