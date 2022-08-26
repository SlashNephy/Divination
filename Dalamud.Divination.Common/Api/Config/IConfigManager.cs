using Dalamud.Configuration;

namespace Dalamud.Divination.Common.Api.Config
{
    public interface IConfigManager<out T> where T : IPluginConfiguration
    {
        public T Config { get; }

        public bool TryUpdate(string key, string? value, bool useTts);
    }
}
