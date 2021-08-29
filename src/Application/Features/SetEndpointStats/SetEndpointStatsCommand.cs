using Mockingjay.Common.Handling;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;


namespace Mockingjay.Features
{
    public class SetEndpointStatsCommand : ICommand
    {
        public EndpointId Id { get; set; }
    }
}
