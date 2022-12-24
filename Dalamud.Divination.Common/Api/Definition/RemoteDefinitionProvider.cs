using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.Definition;

internal sealed class RemoteDefinitionProvider<TContainer> : DefinitionProvider<TContainer>
    where TContainer : DefinitionContainer, new()
{
    private readonly HttpClient client = new();
    private readonly Timer timer = new(60 * 60 * 1000);
    private readonly string url;

    public RemoteDefinitionProvider(string url, string filename)
    {
        this.url = url;
        Filename = filename;

        timer.Elapsed += OnTimerElapsed;
        timer.Start();
    }

    public override string Filename { get; }

    private void OnTimerElapsed(object? _, ElapsedEventArgs __)
    {
        Task.Run(async () =>
        {
            await Update(Cancellable.Token);
        });
    }

    internal override async Task<JObject?> Fetch()
    {
        await using var stream = await client.GetStreamAsync(url);

        using var reader = new StreamReader(stream, Encoding.UTF8);
        var content = await reader.ReadToEndAsync();

        return JObject.Parse(content);
    }

    public override void Dispose()
    {
        base.Dispose();
        timer.Dispose();
        client.Dispose();
    }
}
