using FluentValidation;

namespace Mockingjay.Features.GetEndpoints
{
    public class GetEndpointsCommandValidator : AbstractValidator<GetEndpointsCommand>
    {
        public GetEndpointsCommandValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0);

            RuleFor(x => x.ItemsPerPage)
                .GreaterThanOrEqualTo(5)
                .LessThanOrEqualTo(100);
        }
    }
}
