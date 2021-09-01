using System;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Dalamud.Divination.Common.Api.Chat
{
    public interface IChatClient : IDisposable
    {
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
