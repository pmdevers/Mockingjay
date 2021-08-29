using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Mockingjay.Common.DomainModel;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public partial class Endpoint : AggregateRoot<Endpoint, EndpointId>
    {
        public Endpoint() : base(EndpointId.Next()) { }
        protected Endpoint(EndpointId aggregateId) : base(aggregateId)
        {
        }

        public string Path { get; internal set; }
        public string Method { get; internal set; }
        public HttpStatusCode StatusCode { get; internal set; }
        public string ContentType { get; internal set; }
        public string Content { get; internal set; }


        public static Endpoint Create(string path, string method, HttpStatusCode statusCode, string contentType, string content)
        {
            var endpoint = new Endpoint();
            return endpoint.ApplyEvent(new EndpointCreated(endpoint.Id, path, method, statusCode, contentType, content));
        }

        internal void When(EndpointCreated e)
        {
            Path = e.Path;
            Method = e.Method;
            StatusCode = e.StatusCode;
            ContentType = e.ContentType;
            Content = e.Content;
        }
    }

    public class EndpointCreated
    {
        public EndpointCreated()
        {

        }
        public EndpointCreated(EndpointId id, string path, string method, HttpStatusCode statusCode, string contentType, string content)
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
