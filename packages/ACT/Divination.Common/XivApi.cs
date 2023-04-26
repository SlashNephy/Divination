using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Divination.Common
{
    public class XivApiClient : IDisposable
    {
        private const string ApiKey = "<reducted>";

        private readonly Dictionary<string, Lazy<Dictionary<string, JObject>>> cache =
            new Dictionary<string, Lazy<Dictionary<string, JObject>>>();

        private readonly HttpClient client;

        private readonly IDivinationLogger logger = DivinationLoggerFactory.Create("XivApiClient");

        private readonly Timer timer;

        public XivApiClient(HttpClient client)
        {
            if (!Directory.Exists(DivinationEnvironment.CacheDirectory))
            {
                Directory.CreateDirectory(DivinationEnvironment.CacheDirectory);
            }

            this.client = client;
            LoadCache();

            // Saves every 5 minutes.
            timer = new Timer(5 * 60 * 1000);
            timer.Elapsed += (_, __) => SaveCache();
            timer.Start();
        }

        public void Dispose()
        {
            timer.Dispose();

            SaveCache();
        }

        public async Task<XivApiResponse> GetAsync(string content, uint id, bool ignoreCache = false)
        {
            var key = id.ToString();
            var dictionary = GetOrCreateCache(content);

            if (ignoreCache || !dictionary.ContainsKey(key))
            {
                var url = $"https://xivapi.com/{content}/{key}";
                using var response = await client.GetAsync($"{url}?private_key={ApiKey}");
                var result = await response.Content.ReadAsStringAsync();
                dictionary[key] = JObject.Parse(result);

                logger.Trace($"{response.RequestMessage.Method.Method} {(int) response.StatusCode} {url}");
            }

            return new XivApiResponse(dictionary[key]);
        }

        public XivApiResponse Get(string content, uint id, bool ignoreCache = false)
        {
            return GetAsync(content, id, ignoreCache).GetAwaiter().GetResult();
        }

        public async Task<XivApiResponse> GetCharacterAsync(string name, string world, bool ignoreCache = false)
        {
            var key = $"{name}@{world}";
            var dictionary = GetOrCreateCache("Character");

            if (ignoreCache || !dictionary.ContainsKey(key))
            {
                var url = $"https://xivapi.com/character/search?name={name.Replace(" ", "%20")}&server={world}";
                using var response = await client.GetAsync($"{url}&private_key={ApiKey}");
                var result = await response.Content.ReadAsStringAsync();
                dynamic json = JObject.Parse(result);
                dictionary[key] = (JObject) ((JArray) json.Results).First();

                logger.Trace($"{response.RequestMessage.Method.Method} {(int) response.StatusCode} {url}");
            }

            return new XivApiResponse(dictionary[key]);
        }

        public XivApiResponse GetCharacter(string name, string world, bool ignoreCache = false)
        {
            return GetCharacterAsync(name, world, ignoreCache).GetAwaiter().GetResult();
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
            foreach (var path in Directory.EnumerateFiles(DivinationEnvironment.CacheDirectory,
                $"XivApi.{DivinationEnvironment.AssemblyName}.*.json"))
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
                if (pair.Value.IsValueCreated)
                {
                    var path = Path.Combine(DivinationEnvironment.CacheDirectory,
                        $"XivApi.{DivinationEnvironment.AssemblyName}.{pair.Key}.json");
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

            foreach (var path in Directory.EnumerateFiles(DivinationEnvironment.CacheDirectory, "XivApi.*.json"))
            {
                File.Delete(path);
            }
        }

        private Dictionary<string, JObject> GetOrCreateCache(string content)
        {
            if (!cache.ContainsKey(content))
            {
                cache[content] = new Lazy<Dictionary<string, JObject>>(() => new Dictionary<string, JObject>());
            }

            return cache[content].Value;
        }

        #endregion
    }

    public class XivApiResponse
    {
        private readonly JObject json;
        public static Language Language = Language.Japanese;

        public XivApiResponse(JObject json)
        {
            this.json = json;
        }

        public JToken this[string key]
        {
            get
            {
                if (json.TryGetValue(key, out var value))
                {
                    return value;
                }

                throw new KeyNotFoundException($"Json.{key} is not found.");
            }
        }

        public dynamic Dynamic => json;

        public string? GetLocalizedString(params string[] keys)
        {
            try
            {
                var t = json;
                foreach (var key in keys.Take(keys.Length - 1))
                {
                    t = (JObject?) json[key];

                    if (t == null)
                    {
                        return null;
                    }
                }

                var lastKey = keys.Last();
                var localizableKey = Language == Language.Japanese ? $"{lastKey}_ja" : $"{lastKey}_en";

                return (string?) (t[localizableKey] ?? t[lastKey]);
            }
            catch
            {
                return null;
            }
        }
    }
}
