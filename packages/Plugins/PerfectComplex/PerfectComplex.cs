using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.PerfectComplex;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class PerfectComplex : DivinationPlugin<PerfectComplex, PluginConfig, PluginDefinition>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>,
    IDefinitionSupport
{
    public PerfectComplex(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
    }

    #region ICommandSupport

    public string MainCommandPrefix => "/template";

    [Command("/example", "foo", "<arg>", "<optionalarg?>", "<vararg...>")]
    [CommandHelp("This is sample command.")]
    private static void OnExampleCommand(CommandContext context)
    {
        PluginLog.Information("/example called.");
    }

    #endregion

    #region IConfigWindowSupport

    public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

    #endregion

    #region IDefinitionSupport

    public string DefinitionUrl => "https://github.com/horoscope-dev/Divination.Definitions/raw/master/dist/PerfectComplex.json";

    #endregion
}
