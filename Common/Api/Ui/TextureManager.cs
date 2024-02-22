using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.XivApi;
using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Plugin.Services;

namespace Dalamud.Divination.Common.Api.Ui;

internal sealed class TextureManager : ITextureManager
{
    private readonly Dictionary<uint, IDalamudTextureWrap?> cache = new();
    private readonly object cacheLock = new();

    private readonly HttpClient client = new();
    private readonly ITextureProvider textureProvider;
    private readonly UiBuilder uiBuilder;

    public TextureManager(ITextureProvider textureProvider, UiBuilder uiBuilder)
    {
        this.textureProvider = textureProvider;
        this.uiBuilder = uiBuilder;
    }

    public IDalamudTextureWrap? GetIconTexture(uint iconId)
    {
        lock (cacheLock)
        {
            if (cache.TryGetValue(iconId, out var texture))
            {
                return texture;
            }

            cache[iconId] = null;
            LoadIconTexture(iconId);

            return null;
        }
    }

    public void Dispose()
    {
        foreach (var texture in cache.Values)
        {
            texture?.Dispose();
        }

        cache.Clear();

        client.Dispose();
    }

    private void LoadIconTexture(uint iconId)
    {
        Task.Run(async () =>
        {
            try
            {
                cache[iconId] = LoadIconTextureFromLumina(iconId) ?? await LoadIconTextureFromXivApi(iconId);
            }
            catch (Exception exception)
            {
                cache.Remove(iconId);
                DalamudLog.Log.Error(exception, "Error occurred while LoadIconTexture");
            }
        });
    }

    private IDalamudTextureWrap? LoadIconTextureFromLumina(uint iconId)
    {
        try
        {
            var iconTex = textureProvider.GetIcon(iconId);
            if (iconTex != null)
            {
                if (iconTex.ImGuiHandle != IntPtr.Zero)
                {
                    return iconTex;
                }

                iconTex.Dispose();
            }
        }
        catch (NotImplementedException)
        {
        }
        catch (MissingFieldException)
        {
        }

        return null;
    }

    private async Task<IDalamudTextureWrap?> LoadIconTextureFromXivApi(uint iconId)
    {
        var path = Path.Combine(DivinationEnvironment.CacheDirectory, $"Icon.{iconId}.png");
        if (!File.Exists(path))
        {
            var iconUrl = XivApiClient.GetIconUrl(iconId);
            await using var stream = await client.GetStreamAsync(iconUrl);
            await using var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);

            await stream.CopyToAsync(fileStream);
        }

        var tex = await uiBuilder.LoadImageAsync(path);
        if (tex.ImGuiHandle != IntPtr.Zero)
        {
            return tex;
        }

        tex.Dispose();

        return null;
    }
}
