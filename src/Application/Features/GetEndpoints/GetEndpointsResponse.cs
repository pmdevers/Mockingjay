using Mockingjay.Entities;
using System.Collections;
using System.Collections.Generic;

namespace Mockingjay.Features
{
    public class GetEndpointsResponse : IEnumerable<EndpointInformation>
    {
        private readonly IEnumerable<EndpointInformation> _items;

        public GetEndpointsResponse(
            IEnumerable<EndpointInformation> items)
        {
            _items = items;
        }

        public IEnumerator<EndpointInformation> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
