using Dalamud.Bindings.ImGui;

namespace Dalamud.Divination.Common.Api.Ui;

public static class ImGuiEx
{
    public static void TextConfig(string label, ref string input, int maxLength, params string[] descriptions)
    {
        ImGui.Text(label);
        ImGui.Indent();
        ImGui.InputText($"##{label.Replace(" ", string.Empty)}", ref input, maxLength);
        foreach (var description in descriptions)
        {
            ImGui.Text(description);
        }

        ImGui.Unindent();
    }

    public static bool CheckboxConfig(string label, ref bool value, params string[] descriptions)
    {
        var ret = ImGui.Checkbox(label, ref value);
        ImGui.Indent();
        foreach (var description in descriptions)
        {
            ImGui.Text(description);
        }

        ImGui.Unindent();
        return ret;
    }

    public static void HelpMarker(params string[] descriptions)
    {
        ImGui.TextDisabled("(?)");

        if (ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
            foreach (var description in descriptions)
            {
                ImGui.TextUnformatted(description);
            }

            ImGui.PopTextWrapPos();
            ImGui.EndTooltip();
        }
    }
}
