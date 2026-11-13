using Divination.FaloopIntegration.Config;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace Divination.FaloopIntegration;

public static class SoundEffect
{
    /// <summary>
    /// Gets the ID of the UI sound effect to play on spawn reports, or null if none is configured.
    /// </summary>
    public static uint? GetSpawnSoundEffectId(PluginConfig.PerRankConfig config)
    {
        return config.SpawnSoundEffect switch
        {
            // <se.1> to <se.16> are UI sound effects 37 to 52
            SpawnSoundEffect.Chat when config.SpawnChatSoundEffect is >= 1 and <= 16 => (uint)config.SpawnChatSoundEffect + 36,
            SpawnSoundEffect.Custom when config.SpawnCustomSoundEffectId > 0 => (uint)config.SpawnCustomSoundEffectId,
            _ => null,
        };
    }

    /// <summary>
    /// Plays a UI sound effect. Must be called on the framework thread.
    /// </summary>
    public static unsafe void Play(uint id)
    {
        UIGlobals.PlaySoundEffect(id);
    }
}
