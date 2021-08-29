using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Linq;

namespace Mockingjay.Features
{
    internal class AddEndpointCommandValidator : AbstractValidator<AddEndpointCommand>
    {
        public AddEndpointCommandValidator(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            var routes = actionDescriptorCollectionProvider.ActionDescriptors.Items
                .Select(x => x.AttributeRouteInfo.Template)
                .Distinct()
                .ToList();

            var allowedMethods = new[] { "GET", "POST", "PUT", "DELETE", "PATCH" };

            RuleFor(x => x.Path).Must(x => x.StartsWith("/", StringComparison.InvariantCulture)).WithMessage("Must start with a '/'");
            RuleFor(x => x.Path).Must(x => !routes.Contains(x.TrimStart('/'))).WithMessage($"Contains reserverved routes '{string.Join(", ", routes)}'");
            RuleFor(x => x.Method).Must(x => allowedMethods.Contains(x));
        }
    }
}
