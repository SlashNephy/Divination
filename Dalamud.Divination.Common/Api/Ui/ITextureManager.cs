using System;
using ImGuiScene;

namespace Dalamud.Divination.Common.Api.Ui
{
    public interface ITextureManager : IDisposable
    {
        public TextureWrap? GetIconTexture(uint iconId);
    }
}
