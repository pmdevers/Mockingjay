using System.Net;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public partial class Endpoint
    {
        internal void Update(string path, string method, HttpStatusCode statusCode, string contentType, string content)
        {
            ApplyEvent(new EndpointUpdated(Id, path, method, statusCode, contentType, content));
        }

        internal void When(EndpointUpdated e)
        {
            Path = e.Path;
            Method = e.Method;
            StatusCode = e.StatusCode;
            ContentType = e.ContentType;
            Content = e.Content;
        }
    }

    public class EndpointUpdated
    {
        public EndpointUpdated()
        {

        }
        public EndpointUpdated(EndpointId id, string path, string method, HttpStatusCode statusCode, string contentType, string content)
        {
            Id = id;
            Path = path;
            Method = method;
            StatusCode = statusCode;
            ContentType = contentType;
            Content = content;
        }

        public EndpointId Id { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
