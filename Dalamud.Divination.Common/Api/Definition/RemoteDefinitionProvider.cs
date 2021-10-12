using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Timers;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.Definition
{
    internal sealed class RemoteDefinitionProvider<TContainer> : DefinitionProvider<TContainer> where TContainer : DefinitionContainer, new()
    {
        private readonly string url;
        private readonly Timer timer = new(60 * 60 * 1000);

        public RemoteDefinitionProvider(string url, string filename)
        {
            this.url = url;
            Filename = filename;

            timer.Elapsed += OnTimerElapsed;
            timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Update(Cancellable.Token);
        }

        public override string Filename { get; }

        internal override JObject Fetch()
        {
            var request = WebRequest.CreateHttp(url);
            request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.Method = "GET";

            using var response = request.GetResponse();
            using var stream = response.GetResponseStream();
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            using var reader = new StreamReader(stream!, Encoding.UTF8);

            var content = reader.ReadToEnd();
            return JObject.Parse(content);
        }

        public override void Dispose()
        {
            base.Dispose();
            timer.Dispose();
        }
    }
}
