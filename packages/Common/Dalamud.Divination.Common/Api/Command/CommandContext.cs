using System.Text.RegularExpressions;

namespace Dalamud.Divination.Common.Api.Command;

/// <summary>
///     呼び出されたコマンドのコンテキストを持つクラスです。このクラスは継承できません。
/// </summary>
public sealed class CommandContext
{
    /// <summary>
    ///     コマンドのコンテキストを初期化します。
    /// </summary>
    /// <param name="command">コマンド。</param>
    /// <param name="match">正規表現のマッチ結果。</param>
    internal CommandContext(DivinationCommand command, Match match)
    {
        Command = command;
        Match = match;
    }

    /// <summary>
    ///     呼び出されたコマンド。
    /// </summary>
    public DivinationCommand Command { get; }

    /// <summary>
    ///     コマンドに与えられた引数の配列。
    /// </summary>
    public Match Match { get; }

    public string? this[string name] => Match.Groups.TryGetValue(name, out var result) ? result.Value : null;

    public string GetArgument(string name)
    {
        return Match.Groups[name].Value;
    }
}
