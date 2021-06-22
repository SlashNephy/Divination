using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.XivApi
{
    internal sealed class XivApiClient : IXivApiClient
    {
        private readonly string apiKey;
        private readonly string cacheName;

        private readonly HttpClient client = new();
        private readonly Dictionary<string, Lazy<Dictionary<string, JObject>>> cache = new();
        private readonly object cacheLock = new();
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(XivApiClient));

        public XivApiClient(string apiKey, string? cacheName)
        {
            this.apiKey = apiKey;
            this.cacheName = cacheName ?? "Default";

            if (!Directory.Exists(DivinationEnvironment.CacheDirectory))
            {
                Directory.CreateDirectory(DivinationEnvironment.CacheDirectory);
            }
            else
            {
                LoadCache();
            }
        }

        public async Task<XivApiResponse> GetAsync(string content, uint id, bool ignoreCache = false)
        {
            var key = id.ToString();
            var dictionary = GetOrCreateCache(content);

            if (ignoreCache || !dictionary.ContainsKey(key))
            {
                var url = $"https://xivapi.com/{content}/{key}";
                using var response = await client.GetAsync($"{url}?private_key={apiKey}");
                var result = await response.Content.ReadAsStringAsync();
                dictionary[key] = JObject.Parse(result);

                logger.Verbose("{Code}: {Method} {Url}", (int) response.StatusCode, response.RequestMessage.Method.Method, url);
            }

            return new XivApiResponse(dictionary[key]);
        }

        public async Task<XivApiResponse> GetCharacterAsync(string name, string world, bool ignoreCache = false)
        {
            var key = $"{name}@{world}";
            var dictionary = GetOrCreateCache("Character");

            if (ignoreCache || !dictionary.ContainsKey(key))
            {
                var url = $"https://xivapi.com/character/search?name={Uri.EscapeUriString(name)}&server={world}";
                using var response = await client.GetAsync($"{url}&private_key={apiKey}");
                var result = await response.Content.ReadAsStringAsync();
                dynamic json = JObject.Parse(result);
                dictionary[key] = (JObject) ((JArray) json.Results).First();

                logger.Verbose("{Code}: {Method} {Url}", (int) response.StatusCode, response.RequestMessage.Method.Method, url);
            }

            return new XivApiResponse(dictionary[key]);
        }

        public static string GetIconUrl(uint icon)
        {
            string folderId;
            var iconId = icon.ToString();

            if (iconId.Length >= 6)
            {
                iconId = iconId.PadLeft(5, '0');
                folderId = string.Join("", iconId.Take(3)) + "000";
            }
            else
            {
                iconId = "0" + iconId.PadLeft(5, '0');
                folderId = "0" + string.Join("", iconId.Skip(1).Take(2)) + "000";
            }

            return $"https://xivapi.com/i/{folderId}/{iconId}.png";
        }

        #region Cache

        private void LoadCache()
        {
            foreach (var path in Directory.EnumerateFiles(DivinationEnvironment.CacheDirectory, $"XivApi.{cacheName}.*.json"))
            {
                var content = Path.GetFileNameWithoutExtension(path)?.Split('.').LastOrDefault();
                if (content == null)
                {
                    continue;
                }

                cache[content] = new Lazy<Dictionary<string, JObject>>(() =>
                {
                    try
                    {
                        var json = File.ReadAllText(path);
                        var directory = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);
                        return directory ?? new Dictionary<string, JObject>();
                    }
                    catch
                    {
                        return new Dictionary<string, JObject>();
                    }
                });
            }
        }

        private void SaveCache()
        {
            foreach (var pair in cache)
            {
                if (pair.Value.IsValueCreated && pair.Value.Value.Count > 0)
                {
                    var path = Path.Combine(DivinationEnvironment.CacheDirectory, $"XivApi.{cacheName}.{pair.Key}.json");
                    var json = JsonConvert.SerializeObject(pair.Value.Value);

                    File.WriteAllText(path, json);
                }
            }
        }

        public void ClearCache()
        {
            foreach (var pair in cache.Where(pair => pair.Value.IsValueCreated))
            {
                pair.Value.Value.Clear();
            }

            cache.Clear();

            foreach (var path in Directory.EnumerateFiles(DivinationEnvironment.CacheDirectory, "XivApi.*.*.json"))
            {
                File.Delete(path);
            }
        }

        private Dictionary<string, JObject> GetOrCreateCache(string content)
        {
            lock (cacheLock)
            {
                if (!cache.ContainsKey(content))
                {
                    cache[content] = new Lazy<Dictionary<string, JObject>>(() => new Dictionary<string, JObject>());
                }

                return cache[content].Value;
            }
        }

        #endregion

        public void Dispose()
        {
            SaveCache();

            client.Dispose();
        }
    }
}
