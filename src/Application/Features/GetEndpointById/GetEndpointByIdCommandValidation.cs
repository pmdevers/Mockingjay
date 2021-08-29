using FluentValidation;
using System;
using System.Linq;

namespace Mockingjay.Features
{
    internal class GetEndpointByIdCommandValidation : AbstractValidator<GetEndpointByIdCommand>
    {
        public GetEndpointByIdCommandValidation()
        {
        }
    }
}
