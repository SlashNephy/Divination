using System.IO;
using System.Windows.Forms;
using Divination.Common;

namespace Divination.ACT
{
    public abstract partial class DivinationActPlugin<TW, TU, TS>
    {
        private volatile bool isUpdated;
        // ReSharper disable once StaticMemberInGenericType
#pragma warning disable 8618
        private static FileSystemWatcher Watcher { get; set; }
#pragma warning restore 8618

        private void InitWatcher()
        {
            Watcher = new FileSystemWatcher(AssemblyDirectory, $"{DivinationEnvironment.AssemblyName}.dll");
            Watcher.Changed += OnPluginUpdated;
            Watcher.EnableRaisingEvents = true;
        }

        private void OnPluginUpdated(object sender, FileSystemEventArgs args)
        {
            if (isUpdated || new FileInfo(args.FullPath).Length < 512000)
            {
                return;
            }

            isUpdated = true;

            var result = MessageBox.Show("プラグインファイルが更新されました。リロードしますか？", DivinationEnvironment.AssemblyName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.No)
            {
                isUpdated = false;
                return;
            }

            Reload();
        }

        private static void DeInitWatcher()
        {
            Watcher.Dispose();
        }
    }
}
