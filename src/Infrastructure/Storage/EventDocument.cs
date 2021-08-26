using System;
using System.Globalization;
using System.Security.Claims;


namespace Infrastructure.Storage
{
    internal class EventDocument<TId>
    {
        public string Id => AggregateId.ToString() + ':' + Version.ToString(CultureInfo.InvariantCulture);
        public TId AggregateId { get; set; }
        public int Version { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; }
        public object PayLoad { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
