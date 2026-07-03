namespace Acervo.Web.Endpoints.User
{
    public static class UserEndpoints
    {
        public static string GetAll()         => "user";
        public static string GetById(long id) => $"user/{id}";
        public static string Create()         => "user";
        public static string Update()         => "user";
        public static string Delete(long id)  => $"user/{id}";
    }
}
