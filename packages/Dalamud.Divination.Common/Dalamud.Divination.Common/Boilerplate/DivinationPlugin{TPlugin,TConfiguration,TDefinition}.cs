﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Definition;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate;

/// <summary>
///     Divination プラグインのボイラープレートを提供します。
///     このクラスを継承することで Dalamud 互換のプラグインを作成できます。
/// </summary>
/// <typeparam name="TPlugin">プラグインのクラス。</typeparam>
/// <typeparam name="TConfiguration">Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラス。</typeparam>
/// <typeparam name="TDefinition">プラグインの外部定義クラス。</typeparam>
[SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
public abstract class
    DivinationPlugin<TPlugin, TConfiguration, TDefinition> : IDivinationPluginApi<TConfiguration, TDefinition>
    where TPlugin : DivinationPlugin<TPlugin, TConfiguration, TDefinition>
    where TConfiguration : class, IPluginConfiguration, new()
    where TDefinition : DefinitionContainer, new()
{
#pragma warning disable CS8618
    protected DivinationPlugin(DalamudPluginInterface pluginInterface)
#pragma warning restore CS8618
    {
        Instance = this as TPlugin ?? throw new TypeAccessException("クラス インスタンスが型パラメータ: TPlugin と一致しません。");
        IsDisposed = false;
        Dalamud = new DalamudApi(pluginInterface);
        Divination = new DivinationApi<TConfiguration, TDefinition>(Dalamud, Assembly, this);

        PluginLog.Information("プラグイン: {Name} の初期化に成功しました。バージョン = {Version}",
            Name,
            Divination.Version.Plugin.InformationalVersion);
    }

    /// <summary>
    ///     プラグインのインスタンスの静的プロパティ。
    /// </summary>
#pragma warning disable 8618
    public static TPlugin Instance { get; private set; }
#pragma warning restore 8618

    public string Name => $"Divination.{Instance.GetType().Name.Replace("Plugin", string.Empty)}";
    public bool IsDisposed { get; private set; }
    public TConfiguration Config { get; init; }
    public TDefinition? Definition => Divination.Definition?.Container;
    public IDalamudApi Dalamud { get; }
    public IDivinationApi<TConfiguration, TDefinition> Divination { get; }
    public Assembly Assembly => Instance.GetType().Assembly;

    #region IDisposable

    /// <summary>
    ///     Divination プラグイン内で確保されているすべてのリソースを解放します。
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     .NET 管理リソースを解放します。
    /// </summary>
    protected virtual void ReleaseManaged()
    {
    }

    /// <summary>
    ///     .NET 管理外のリソースの解放を試みます。
    /// </summary>
    protected virtual void ReleaseUnmanaged()
    {
    }

    protected virtual void Dispose(bool disposing)
    {
        IsDisposed = true;

        if (disposing)
        {
            ReleaseManaged();
            Divination.Dispose();
        }

        ReleaseUnmanaged();
    }

    ~DivinationPlugin()
    {
        Dispose(false);
    }

    #endregion
}
