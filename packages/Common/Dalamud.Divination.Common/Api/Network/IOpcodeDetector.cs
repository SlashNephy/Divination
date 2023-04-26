using System.Collections.Generic;
using Dalamud.Game.Text.SeStringHandling;

namespace Dalamud.Divination.Common.Api.Network;

public interface IOpcodeDetector
{
    /// <summary>
    ///     指定されたステップに必要な操作を記述します。
    /// </summary>
    /// <param name="step">現在のステップ。</param>
    /// <returns>ステップの記述。必要な操作がない場合は <c>null</c> を返します。</returns>
    public SeString? DescribeStep(uint step);

    /// <summary>
    ///     Opcode を検出します。
    /// </summary>
    /// <param name="context">ネットワークデータ。</param>
    /// <param name="step">現在のステップ。</param>
    /// <param name="definitions">発見された Opcode 定義を書き込む Dictionary。</param>
    /// <returns>次のステップに進行する場合は <c>true</c> を返します。</returns>
    public bool Detect(NetworkContext context, uint step, Dictionary<string, ushort> definitions);
}
