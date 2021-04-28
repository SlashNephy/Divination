using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.Definition
{
    public class LocalDefinitionProvider<TContainer> : DefinitionProvider<TContainer> where TContainer : DefinitionContainer, new()
    {
        private readonly FileSystemWatcher watcher;

        public LocalDefinitionProvider(string filename = DefaultFilename)
        {
            Filename = filename;
            watcher = new FileSystemWatcher(DivinationEnvironment.DivinationDirectory, filename);

            watcher.Changed += OnDefinitionFileChanged;
            watcher.EnableRaisingEvents = true;
        }

        public override string Filename { get; }

        public override bool AllowObsoleteDefinitions => true;

        private void OnDefinitionFileChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            Update();
        }

        protected override JObject? Fetch()
        {
            var localPath = Path.Combine(DivinationEnvironment.DivinationDirectory, Filename);
            if (!File.Exists(localPath))
            {
                return null;
            }

            var content = File.ReadAllText(localPath);
            return JObject.Parse(content);
        }

        public override void Dispose()
        {
            base.Dispose();
            watcher.Dispose();
        }
    }
}
