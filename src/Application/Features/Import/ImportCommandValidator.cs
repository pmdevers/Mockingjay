using FluentValidation;
using System.IO;

namespace Mockingjay.Features
{
    public class ImportCommandValidator : AbstractValidator<ImportCommand>
    {
        public ImportCommandValidator()
        {
            RuleFor(x => x.Filename).Must(filename => File.Exists(filename));
        }
    }
}
