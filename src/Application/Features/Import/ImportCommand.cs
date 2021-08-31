using Mockingjay.Common.Handling;

namespace Mockingjay.Features
{
    public class ImportCommand : ICommand
    {
        public byte[] Bytes { get; set; }
    }
}
