using FluentValidation;
using System;
using System.Linq;

namespace Mockingjay.Features
{
    public class GetEndpointByPathCommandValidation : AbstractValidator<GetEndpointByPathCommand>
    {
        public GetEndpointByPathCommandValidation()
        {
            var allowedMethods = new[] { "GET", "POST", "PUT", "DELETE", "PATCH" };

            RuleFor(x => x.Path).Must(x => x.StartsWith("/", StringComparison.InvariantCulture));
            RuleFor(x => x.Method).Must(x => allowedMethods.Contains(x));
        }
    }
}
