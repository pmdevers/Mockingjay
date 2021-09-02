using Microsoft.Extensions.Options;
using Mockingjay.Common.Storage;
using Mockingjay.Features;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly JsonSerializerOptions _options;

        public SettingsRepository(IOptions<JsonSerializerOptions> settings)
        {
            _options = settings?.Value ?? new JsonSerializerOptions();
        }

        public Settings Get()
        {
            var filePath = Path.Combine(EndpointDatafile.Directory, "settings.json");
            if (File.Exists(filePath))
            {
                var content = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Settings>(content, _options);
            }
            else
            {
                return new Settings();
            }
        }

        public void Save(Settings settings)
        {
            var content = JsonSerializer.Serialize(settings, _options);
            var filePath = Path.Combine(EndpointDatafile.Directory, "settings.json");
            File.WriteAllText(filePath, content);
        }
    }
}
