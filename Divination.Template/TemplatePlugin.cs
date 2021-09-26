using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;

namespace Divination.Template
{
    public class TemplatePlugin : DivinationPlugin<TemplatePlugin, PluginConfig, PluginDefinition>,
        IDalamudPlugin, ICommandSupport, IConfigWindowSupport<PluginConfig>, IDefinitionSupport
    {
        public TemplatePlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            Logger.Information("Plugin loaded!");
        }

        public string MainCommandPrefix => "/template";
        public string DefinitionUrl => "https://raw.githubusercontent.com/SlashNephy/Dalamud.Divination.Ephemera/master/dist/Template.json";
        public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

        [Command("/hoge", "foo", "bar")]
        [CommandHelp("This is sample command.")]
        private void OnHogeCommand(CommandContext context)
        {
            Logger.Information("/hoge foo bar called");
        }
    }
}
