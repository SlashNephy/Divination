using System.IO;
using System.Net.Http;
using System.Text;
using System.Timers;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.Definition
{
    internal sealed class RemoteDefinitionProvider<TContainer> : DefinitionProvider<TContainer> where TContainer : DefinitionContainer, new()
    {
        private readonly string url;
        private readonly Timer timer = new(60 * 60 * 1000);
        private readonly HttpClient client = new();

        public RemoteDefinitionProvider(string url, string filename)
        {
            this.url = url;
            Filename = filename;

            timer.Elapsed += OnTimerElapsed;
            timer.Start();
        }

        private void OnTimerElapsed(object? _, ElapsedEventArgs __)
        {
            Update(Cancellable.Token);
        }

        public override string Filename { get; }

        internal override JObject Fetch()
        {
            using var stream = client.GetStreamAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();

            using var reader = new StreamReader(stream, Encoding.UTF8);
            var content = reader.ReadToEnd();

            return JObject.Parse(content);
        }

        public override void Dispose()
        {
            base.Dispose();
            timer.Dispose();
            client.Dispose();
        }
    }
}
