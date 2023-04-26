using ImGuiScene;

namespace Dalamud.Divination.Common.Api.Ui;

public static class TextureManagerEx
{
    public static TextureWrap? GetIconTexture(this ITextureManager manager, int iconId)
    {
        return manager.GetIconTexture((uint)iconId);
    }
}
