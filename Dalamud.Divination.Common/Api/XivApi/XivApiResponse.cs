using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.XivApi
{
    public sealed class XivApiResponse : IReadOnlyDictionary<string, JToken>
    {
        private readonly JObject json;

        public XivApiResponse(JObject json)
        {
            this.json = json;
        }

        public dynamic Dynamic => json;

        public JToken this[string key]
        {
            get
            {
                if (json.TryGetValue(key, out var value))
                {
                    return value;
                }

                throw new KeyNotFoundException($"{key} is not found in json.");
            }
        }

        public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator() => json.GetEnumerator()!;

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) json).GetEnumerator();

        public int Count => json.Count;

        public bool ContainsKey(string key) => json.ContainsKey(key);

        public bool TryGetValue(string key, out JToken value) => json.TryGetValue(key, out value!);

        public IEnumerable<string> Keys => ((IDictionary<string, JToken>) json).Keys;

        public IEnumerable<JToken> Values => ((IDictionary<string, JToken>) json).Values;
    }
}
