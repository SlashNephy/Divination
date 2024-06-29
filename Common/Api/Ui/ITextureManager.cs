using System;
using Dalamud.Interface.Textures.TextureWraps;

namespace Dalamud.Divination.Common.Api.Ui;

public interface ITextureManager : IDisposable
{
    public IDalamudTextureWrap? GetIconTexture(uint iconId);
}
