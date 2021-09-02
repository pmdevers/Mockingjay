using Mockingjay.Features;

namespace Mockingjay.Common
{
    public class SettingsService
    {
        public SettingsService(Settings settings)
        {
            Settings = settings;
        }
        public Settings Settings { get; private set; }

        internal void Reset(Settings settings)
        {
           Settings = settings;
        }
    }
}
