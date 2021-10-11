using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.Template
{
    public class TemplatePlugin : DivinationPlugin<TemplatePlugin, PluginConfig, PluginDefinition>,
        IDalamudPlugin, ICommandSupport, IConfigWindowSupport<PluginConfig>, IDefinitionSupport
    {
        public TemplatePlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            PluginLog.Information("Plugin loaded!");
        }

        public string MainCommandPrefix => "/template";
        public string DefinitionUrl => "https://ephemera.horoscope.dev/dist/Template.json";
        public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

        [Command("/hoge", "foo", "<arg>", "<optionalarg?>", "<vararg...>")]
        [CommandHelp("This is sample command.")]
        private void OnHogeCommand(CommandContext context)
        {
            PluginLog.Information("/hoge called.");
        }
    }
}
