using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using Divination.Common;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StaticMemberInGenericType
#nullable enable
namespace Divination.ACT
{
    public abstract partial class DivinationActPlugin<TW, TU, TS> where TW : IPluginWorker
        where TU : UserControl
        where TS : PluginSettings
    {
        public static readonly IDivinationLogger Logger = DivinationLoggerFactory.Create();
        public static readonly HttpClient HttpClient = new HttpClient();
        public static readonly XivApiClient XivApi = new XivApiClient(HttpClient);

        public abstract TW CreateWorker();

        public virtual TU CreateTabControl()
        {
            return (TU) new UserControl();
        }

        public virtual TS CreateSettings()
        {
            return (TS) new PluginSettings();
        }

        public void InitPlugin(TabPage screenSpace, Label statusText)
        {
            try
            {
                PluginData = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.pluginObj == this)
                             ?? throw new ApplicationException("Could not get plugin instance.");
                AssemblyDirectory = Path.GetDirectoryName(PluginData.pluginFile.FullName)!;

                FFXIV_ACT_Plugin =
                    ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.pluginFile.Name == "FFXIV_ACT_Plugin.dll")
                        ?.pluginObj ?? throw new ApplicationException(
                        "FFXIV_ACT_Plugin is not loaded. Get plugin at https://github.com/ravahn/FFXIV_ACT_Plugin/releases.");

                ScreenSpace = screenSpace;
                StatusText = statusText;

                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    Logger.Error((Exception) eventArgs.ExceptionObject);
                };

                ScreenSpace.Text = DivinationEnvironment.AssemblyName.Replace(".ACT.", ".");
                TabControl = CreateTabControl();
                ScreenSpace.Controls.Add(TabControl);
                TabControl.Show();

                Settings = CreateSettings();
                Settings.Load();

                Worker = CreateWorker();

                InitWatcher();

                AppendStatusText("Plugin started!");
            }
            catch (Exception e)
            {
                Logger.Error(e);
                DeInitPlugin();

                throw;
            }
        }

        public static void Stop()
        {
            Logger.Info("Plugin stopping...");

            var thread = new Thread(() =>
            {
                PluginData.cbEnabled.Invoke(new Action(() => { PluginData.cbEnabled.Checked = false; }));
            });
            thread.Start();
        }

        public static void Reload()
        {
            Logger.Info("Plugin reloading...");

            var thread = new Thread(() =>
            {
                PluginData.cbEnabled.Invoke(new Action(() =>
                {
                    PluginData.cbEnabled.Checked = false;

                    Thread.Sleep(1500);

                    PluginData.cbEnabled.Checked = true;
                }));
            });
            thread.Start();
        }

        public void DeInitPlugin()
        {
            DeInitWatcher();
            Worker.Dispose();
            XivApi.Dispose();
            HttpClient.Dispose();
            Settings.Dispose();
            AppendStatusText("Plugin stopped!");
            Logger.Dispose();
            TabControl.Dispose();
        }

#pragma warning disable CS8618
        public static TW Worker { get; private set; }
        public static TU TabControl { get; private set; }
        public static TS Settings { get; private set; }
#pragma warning restore CS8618
    }
}
