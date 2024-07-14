using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;
using Divination.Horoscope.Ui;
using ECommons;

namespace Divination.Horoscope;

public class Horoscope : DivinationPlugin<Horoscope, HoroscopeConfig>, IDalamudPlugin, ICommandSupport, IConfigWindowSupport<HoroscopeConfig>
{
    private readonly ModuleManager manager = new();

    public Horoscope(IDalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as HoroscopeConfig ?? new HoroscopeConfig();
        ECommonsMain.Init(pluginInterface, this);

        manager.EnableAll();
    }

    public string MainCommandPrefix => "/horoscope";

    public ConfigWindow<HoroscopeConfig> CreateConfigWindow()
    {
        return new HoroscopeConfigWindow(manager);
    }

    protected override void ReleaseManaged()
    {
        manager.DisableAll();
        ECommonsMain.Dispose();
    }
}
