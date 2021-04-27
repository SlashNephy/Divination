using System.Reflection;
using Dalamud.Divination.Common.Boilerplate;

namespace Dalamud.Divination.Template
{
    public class SamplePlugin : DivinationPlugin<SampleConfig>
    {
        /// <summary>
        /// プラグインの名前を設定します。
        /// </summary>
        public override string Name => "SamplePlugin";

        /// <summary>
        /// プラグインが格納されているアセンブリを設定します。
        /// </summary>
        public override Assembly Assembly => Assembly.GetExecutingAssembly();

        /// <summary>
        /// プラグインがロードされる際に実行される処理を記述します。
        /// </summary>
        public override void Load()
        {
            Logger.Information("Hello!");
        }

        /// <summary>
        /// プラグインがアンロードされる際に実行される処理を記述します。
        /// </summary>
        public override void Unload()
        {
            Logger.Information("Bye!");
        }

        /// <summary>
        /// プラグインの設定の読み込む処理と, (設定が保存されていなければ) 新たにインスタンスを作成する処理を記述します。
        /// </summary>
        public override SampleConfig LoadConfig()
        {
            return Interface.GetPluginConfig() as SampleConfig ?? new SampleConfig();
        }
    }
}
