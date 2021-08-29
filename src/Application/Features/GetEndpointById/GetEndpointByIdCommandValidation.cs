using FluentValidation;
using System;
using System.Linq;

namespace Mockingjay.Features
{
    public class GetEndpointByIdCommandValidation : AbstractValidator<GetEndpointByIdCommand>
    {
        public GetEndpointByIdCommandValidation()
        {
            
        }
    }
}
