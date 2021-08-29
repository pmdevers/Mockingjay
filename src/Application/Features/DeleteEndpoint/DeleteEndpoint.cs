using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public partial class Endpoint
    {
        public bool Deleted { get; internal set; }

        public void Delete()
        {
            ApplyEvent(new EndpointDelted());
        }

        internal void When(EndpointDelted e)
        {
            Deleted = true;
        }
    }
    
    public record EndpointDelted();
}
