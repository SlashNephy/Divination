using System;
using Dalamud.Interface.Internal;

namespace Dalamud.Divination.Common.Api.Ui;

public interface ITextureManager : IDisposable
{
    public IDalamudTextureWrap? GetIconTexture(uint iconId);
}
