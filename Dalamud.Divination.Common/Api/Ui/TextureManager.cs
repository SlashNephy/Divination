using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Data;
using Dalamud.Divination.Common.Api.Logger;
using Dalamud.Divination.Common.Api.XivApi;
using Dalamud.Interface;
using Dalamud.Utility;
using ImGuiScene;

namespace Dalamud.Divination.Common.Api.Ui
{
    internal sealed class TextureManager : ITextureManager
    {
        private readonly DataManager dataManager;
        private readonly UiBuilder uiBuilder;
        private readonly IXivApiClient? xivApi;

        private readonly HttpClient client = new();
        private readonly Dictionary<uint, TextureWrap?> cache = new();
        private readonly object cacheLock = new();
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(TextureManager));

        public TextureManager(DataManager dataManager, UiBuilder uiBuilder, string? xivApiKey = null)
        {
            this.dataManager = dataManager;
            this.uiBuilder = uiBuilder;
            xivApi = xivApiKey != null ? new XivApiClient(xivApiKey, null) : null;
        }

        public TextureWrap? GetIconTexture(uint iconId)
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
                    logger.Error(exception, "Error occurred while LoadIconTexture");
                }
            });
        }

        private TextureWrap? LoadIconTextureFromLumina(uint iconId)
        {
            try
            {
                var iconTex = dataManager.GetIcon(iconId);
                if (iconTex != null)
                {
                    var tex = uiBuilder.LoadImageRaw(iconTex.GetRgbaImageData(), iconTex.Header.Width, iconTex.Header.Height, 4);
                    if (tex != null && tex.ImGuiHandle != IntPtr.Zero)
                    {
                        return tex;
                    }

                    tex?.Dispose();
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

        private async Task<TextureWrap?> LoadIconTextureFromXivApi(uint iconId)
        {
            var path = Path.Combine(DivinationEnvironment.CacheDirectory, $"Icon.{iconId}.png");
            if (!File.Exists(path))
            {
                var iconUrl = XivApiClient.GetIconUrl(iconId);
                using var stream = await client.GetStreamAsync(iconUrl);
                using var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);

                await stream.CopyToAsync(fileStream);
            }

            var tex = uiBuilder.LoadImage(path);
            if (tex != null && tex.ImGuiHandle != IntPtr.Zero)
            {
                return tex;
            }

            tex?.Dispose();

            return null;
        }

        public void Dispose()
        {
            foreach (var texture in cache.Values)
            {
                texture?.Dispose();
            }
            cache.Clear();

            xivApi?.Dispose();
            client.Dispose();
            logger.Dispose();
        }
    }
}
