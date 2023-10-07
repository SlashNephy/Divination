using Dalamud.Interface.Internal;

namespace Dalamud.Divination.Common.Api.Ui;

public static class TextureManagerEx
{
    public static IDalamudTextureWrap? GetIconTexture(this ITextureManager manager, int iconId)
    {
        return manager.GetIconTexture((uint)iconId);
    }
}
