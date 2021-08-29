﻿using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public partial interface IEndpointRepository
    {
        Task DeleteAsync(EndpointId endpointId);
    }
}
