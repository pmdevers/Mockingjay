using Mockingjay.Common.Handling;

namespace Mockingjay.Features
{
    public class ImportCommand : ICommand
    {
        public string Filename { get; set; }
    }
}
