using System.Reflection;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Plugin;

namespace Dalamud.Divination.Template
{
    public class SamplePlugin : DivinationPlugin<SamplePlugin, SampleConfig>, IDalamudPlugin, ICommandSupport
    {
        /// <summary>
        /// プラグインの名前を設定します。
        /// </summary>
        public override string Name => "SamplePlugin";

        /// <summary>
        /// プラグインのコマンドのプレフィックスを設定します。
        /// </summary>
        public string CommandPrefix => "/another";

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
    }
}
