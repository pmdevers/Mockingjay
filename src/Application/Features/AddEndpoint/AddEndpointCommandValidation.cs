using FluentValidation;
using System;
using System.Linq;

namespace Mockingjay.Features.AddEndpoint
{
    public class AddEndpointCommandValidator : AbstractValidator<AddEndpointCommand>
    {
        public AddEndpointCommandValidator()
        {
            var allowedMethods = new[] { "GET", "POST", "PUT", "DELETE", "PATCH" };

            RuleFor(x => x.Path).Must(x => x.StartsWith("/", StringComparison.InvariantCulture));
            RuleFor(x => x.Method).Must(x => allowedMethods.Contains(x));
        }
    }
}
