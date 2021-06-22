using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Dalamud.Divination.Common.Api.Dalamud.String
{
    /// <summary>
    /// Dalamud が提供する SeString を補助する拡張メソッドが定義された静的クラスです。
    /// </summary>
    public static class SeStringEx
    {
        /// <summary>
        /// インスタンスの値を強制的に UTF-8 文字列に変換します。
        /// </summary>
        /// <param name="seString">SeString インスタンス。</param>
        /// <returns>UTF-8 文字列。</returns>
        public static string ToUtf8String(this SeString seString)
        {
            return string.Join(string.Empty, seString.Payloads.Select(x => x.ToUtf8String()));
        }

        /// <summary>
        /// インスタンスの値を強制的に UTF-8 文字列に変換します。
        /// </summary>
        /// <param name="payload">Payload インスタンス。</param>
        /// <returns>UTF-8 文字列。</returns>
        public static string ToUtf8String(this Game.Text.SeStringHandling.Payload payload)
        {
            return Encoding.UTF8.GetString(payload.Encode());
        }

        /// <summary>
        /// インスタンスの値を強制的に UTF-8 文字列に変換します。
        /// </summary>
        /// <param name="payloads">Payload インスタンスのイテレータ。</param>
        /// <returns>UTF-8 文字列。</returns>
        public static string ToUtf8String(this IEnumerable<Game.Text.SeStringHandling.Payload> payloads)
        {
            return string.Join(string.Empty, payloads.Select(x => x.ToUtf8String()));
        }

        /// <summary>
        /// インスタンスの値を強制的に UTF-8 文字列に変換します。
        /// </summary>
        /// <param name="payloads">Payload インスタンスの配列。</param>
        /// <returns>UTF-8 文字列。</returns>
        public static string ToUtf8String(this Game.Text.SeStringHandling.Payload[] payloads)
        {
            return string.Join(string.Empty, payloads.Select(x => x.ToUtf8String()));
        }
    }
}
