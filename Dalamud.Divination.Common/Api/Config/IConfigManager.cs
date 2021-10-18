using System;
using Dalamud.Configuration;

namespace Dalamud.Divination.Common.Api.Config
{
    public interface IConfigManager<out T> : IDisposable where T : IPluginConfiguration
    {
        public T Config { get; }

        public bool TryUpdate(string key, string? value);

        /// <summary>
        /// 現在のプラグインの設定インスタンスをファイルに保存します。
        /// プラグインの終了時に自動的に呼ばれます。
        /// </summary>
        public void Save();
    }
}
