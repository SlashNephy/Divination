using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.Template
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class TemplatePlugin : DivinationPlugin<TemplatePlugin, PluginConfig, PluginDefinition>,
        IDalamudPlugin,
        ICommandSupport,
        IConfigWindowSupport<PluginConfig>,
        IDefinitionSupport
    {
        public TemplatePlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
            PluginLog.Information("Plugin loaded!");
        }

        public override PluginConfig Config { get; }
        string ICommandSupport.MainCommandPrefix => "/template";
        string IDefinitionSupport.DefinitionUrl => "https://horoscope-dev.github.io/Dalamud.Divination.Ephemera/dist/Template.json";
        ConfigWindow<PluginConfig> IConfigWindowSupport<PluginConfig>.CreateConfigWindow() => new PluginConfigWindow();

        [Command("/example", "foo", "<arg>", "<optionalarg?>", "<vararg...>")]
        [CommandHelp("This is sample command.")]
        private static void OnExampleCommand(CommandContext context)
        {
            PluginLog.Information("/example called.");
        }

        protected override void ReleaseManaged()
        {
            Dalamud.PluginInterface.SavePluginConfig(Config);
        }

        protected override void ReleaseUnmanaged()
        {
        }
    }
}
