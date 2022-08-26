using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.Definition
{
    internal sealed class LocalDefinitionProvider<TContainer> : DefinitionProvider<TContainer>
        where TContainer : DefinitionContainer, new()
    {
        private readonly string fallbackUrl;
        private readonly FileSystemWatcher watcher;

        public LocalDefinitionProvider(string filename, string fallbackUrl)
        {
            Filename = filename;
            this.fallbackUrl = fallbackUrl;

            if (!Directory.Exists(DivinationEnvironment.DivinationDirectory))
            {
                Directory.CreateDirectory(DivinationEnvironment.DivinationDirectory);
            }
            watcher = new FileSystemWatcher(DivinationEnvironment.DivinationDirectory, filename);

            watcher.Changed += OnDefinitionFileChanged;
            watcher.EnableRaisingEvents = true;
        }

        public override string Filename { get; }

        public override bool AllowObsoleteDefinitions => true;

        private void OnDefinitionFileChanged(object sender, FileSystemEventArgs e)
        {
            Task.Run(async () =>
            {
                await Update(Cancellable.Token);
            });
        }

        internal override async Task<JObject?> Fetch()
        {
            var localPath = Path.Combine(DivinationEnvironment.DivinationDirectory, Filename);
            if (!File.Exists(localPath))
            {
                using var remote = new RemoteDefinitionProvider<TContainer>(fallbackUrl, Filename);
                return await remote.Fetch();
            }

            var exceptions = new List<Exception>();
            for (var retry = 0; retry < 10; retry++)
            {
                try
                {
                    var content = await File.ReadAllTextAsync(localPath);
                    return JObject.Parse(content);
                }
                // ファイルロックされてる場合などで失敗するので10回までリトライさせる
                catch (IOException e)
                {
                    exceptions.Add(e);
                    await Task.Delay(500, Cancellable.Token);
                }
            }

            throw new AggregateException(exceptions);
        }

        public override void Dispose()
        {
            base.Dispose();
            watcher.Dispose();
        }
    }
}
