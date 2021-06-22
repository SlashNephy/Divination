using System;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Dalamud.Divination.Common.Api.Chat
{
    public interface IChatClient : IDisposable
    {
        /*
         * ゲームクライアントに <code>XivChatEntry</code> で記述された, チャットメッセージを書き込みます。
         * 通常は, <code>Print</code> または <code>PrintError</code> 関数を使用してください。
         * <br></br>
         * ログイン前はキューに格納され, ログイン前に書き込まれたメッセージが見えなくなることを防止できます。
         */
        public void EnqueueChat(XivChatEntry entry);

        /*
         * ゲームクライアントに通常メッセージとして, チャットメッセージを書き込みます。
         */
        public void Print(SeString seString, string? sender = null, XivChatType? type = null);

        /*
         * ゲームクライアントにエラーメッセージとして, チャットメッセージを書き込みます。
         */
        public void PrintError(SeString seString, string? sender = null, XivChatType? type = null);
    }
}
