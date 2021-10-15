using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.Template
{
    public sealed class TemplatePlugin : DivinationPlugin<TemplatePlugin, PluginConfig, PluginDefinition>,
        IDalamudPlugin,
        ICommandSupport,
        IConfigWindowSupport<PluginConfig>,
        IDefinitionSupport
    {
        public TemplatePlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            PluginLog.Information("Plugin loaded!");
        }

        string ICommandSupport.MainCommandPrefix => "/template";
        string IDefinitionSupport.DefinitionUrl => "https://ephemera.horoscope.dev/dist/Template.json";
        ConfigWindow<PluginConfig> IConfigWindowSupport<PluginConfig>.CreateConfigWindow() => new PluginConfigWindow();

        [Command("/example", "foo", "<arg>", "<optionalarg?>", "<vararg...>")]
        [CommandHelp("This is sample command.")]
        private static void OnExampleCommand(CommandContext context)
        {
            PluginLog.Information("/example called.");
        }
    }
}
