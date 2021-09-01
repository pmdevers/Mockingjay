using NUnit.Framework;
using System.Threading.Tasks;

namespace Mockingjay.Tests
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetStateAsync();
        }
    }
}
