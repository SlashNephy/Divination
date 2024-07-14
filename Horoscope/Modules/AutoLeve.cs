using ClickLib.Clicks;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;

namespace Divination.Horoscope.Modules;

public class AutoLeve : IModule
{
    public string Id => "auto_leve";
    public string Name => "Auto Leve";
    public string Description => "Automate your leve works.";

    public void Enable()
    {
        Horoscope.Instance.Dalamud.AddonLifecycle.RegisterListener(AddonEvent.PostSetup, ["GuildLeve"], OnGuildLeveAddonSetup);
    }

    private static unsafe void OnGuildLeveAddonSetup(AddonEvent type, AddonArgs args)
    {
        DalamudLog.Log.Debug("OnGuildLeveAddonSetup: {Addon}", args.Addon);

        var addon = new ClickGuildLeve(args.Addon);
        addon.Fieldcraft();
        addon.Culinarian();
    }

    public void Disable()
    {
        Horoscope.Instance.Dalamud.AddonLifecycle.UnregisterListener(AddonEvent.PostSetup, ["GuildLeve"], OnGuildLeveAddonSetup);
    }
}
