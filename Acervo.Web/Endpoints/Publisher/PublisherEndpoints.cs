namespace Acervo.Web.Endpoints.Publisher
{
    public static class PublisherEndpoints
    {
        public static string GetAll()         => "publisher";
        public static string GetById(long id) => $"publisher/{id}";
        public static string Create()         => "publisher";
        public static string Update()         => "publisher";
        public static string Delete(long id)  => $"publisher/{id}";
    }
}
