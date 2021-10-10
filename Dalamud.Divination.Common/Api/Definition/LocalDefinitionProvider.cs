﻿using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.Definition
{
    internal sealed class LocalDefinitionProvider<TContainer> : DefinitionProvider<TContainer> where TContainer : DefinitionContainer, new()
    {
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
        private readonly string fallbackUrl;

        public override bool AllowObsoleteDefinitions => true;

        private void OnDefinitionFileChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            Update();
        }

        internal override JObject Fetch()
        {
            var localPath = Path.Combine(DivinationEnvironment.DivinationDirectory, Filename);
            if (!File.Exists(localPath))
            {
                using var remote = new RemoteDefinitionProvider<TContainer>(fallbackUrl, Filename);
                return remote.Fetch();
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
