using System;
using Dalamud.Configuration;

namespace Dalamud.Divination.Common.Api.Config
{
    public interface IConfigManager<out T> : IDisposable where T : IPluginConfiguration
    {
        public T Config { get; }

        /// <summary>
        /// ファイルからプラグインの設定を読み込みます。ファイルが存在しなかった場合、新たなインスタンスを作成します。
        /// </summary>
        public void LoadConfig();

        public bool TryUpdate(string key, string? value);

        /// <summary>
        /// 現在のプラグインの設定インスタンスをファイルに保存します。
        /// プラグインの終了時に自動的に呼ばれます。
        /// </summary>
        public void SaveConfig();
    }
}
