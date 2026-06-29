namespace Acervo.Web.Endpoints.Publisher
{
    public static class PublisherEndpoints
    {
        public static string GetAll()         => "publisher";
        public static string GetById(long id) => $"publisher/{id}";
    }
}
