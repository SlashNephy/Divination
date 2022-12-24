using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dalamud.Divination.Common.Api.Definition;

public interface IDefinitionProvider<out TContainer> : IDisposable where TContainer : DefinitionContainer
{
    /// <summary>
    ///     定義ファイルの名前。
    /// </summary>
    public string Filename { get; }

    /// <summary>
    ///     定義ファイルの JSON モデルクラスのインスタンス。
    /// </summary>
    public TContainer Container { get; }

    /// <summary>
    ///     この IDefinitionProvider が無効な定義を受け入れるかどうか。
    /// </summary>
    public bool AllowObsoleteDefinitions { get; }

    /// <summary>
    ///     この IDefinitionProvider の保持している定義を更新します。
    /// </summary>
    public Task Update(CancellationToken token);
}
