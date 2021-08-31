using FluentValidation;

namespace Mockingjay.Features
{
    public class ImportCommandValidator : AbstractValidator<ImportCommand>
    {
        public ImportCommandValidator()
        {
            RuleFor(x => x.Bytes).Must(x => x.Length > 0);
        }
    }
}
