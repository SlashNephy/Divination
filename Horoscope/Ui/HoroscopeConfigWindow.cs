using System.Runtime.InteropServices;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Bindings.ImGui;

namespace Divination.Horoscope.Ui;

public class HoroscopeConfigWindow(ModuleManager manager) : ConfigWindow<HoroscopeConfig>()
{
    private readonly ModuleManager manager = manager;

    public override void Draw()
    {
        if (ImGui.Begin("Horoscope", ref IsOpen))
        {
            ImGui.Text("available modules: ");

            foreach (var (id, module) in manager.Modules)
            {
                ref var enabled = ref CollectionsMarshal.GetValueRefOrAddDefault(Config.ModuleStates, id, out _);
                if (ImGui.Checkbox($"###{id}", ref enabled))
                {
                    if (enabled)
                    {
                        manager.Enable(id);
                    }
                    else
                    {
                        manager.Disable(id);
                    }
                }

                ImGui.SameLine();

                if (ImGui.CollapsingHeader(module.Name))
                {
                    ImGui.Indent();
                    ImGui.Text(module.Description);
                }
            }

            ImGui.Separator();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
            }

            ImGui.End();
        }
    }
}
