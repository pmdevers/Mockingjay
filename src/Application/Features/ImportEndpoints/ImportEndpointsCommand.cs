using Mockingjay.Common.Handling;

namespace Mockingjay.Features
{
    public class ImportEndpointsCommand : ICommand
    {
        public string Filename { get; set; }
    }
}
