using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoreTweet;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.TwitterIntegration
{
    public class TwitterIntegrationPlugin : DivinationPlugin<TwitterIntegrationPlugin, PluginConfig>,
        IDalamudPlugin, ICommandSupport, IConfigWindowSupport<PluginConfig>
    {
        public TwitterIntegrationPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
            Task.Run(WatchTimeline);
        }

        private Tokens? twitter;
        public static Tokens? Twitter
        {
            get
            {
                Instance.twitter ??= CreateTokens();
                return Instance.twitter;
            }
        }

        private static Tokens? CreateTokens()
        {
            if (string.IsNullOrEmpty(Instance.Config.ConsumerKey)
                || string.IsNullOrEmpty(Instance.Config.ConsumerSecret)
                || string.IsNullOrEmpty(Instance.Config.AccessToken)
                || string.IsNullOrEmpty(Instance.Config.AccessTokenSecret))
            {
                return null;
            }

            return Tokens.Create(
                Instance.Config.ConsumerKey,
                Instance.Config.ConsumerSecret,
                Instance.Config.AccessToken,
                Instance.Config.AccessTokenSecret);
        }

        public string MainCommandPrefix => "/twitter";

        public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

        [Command("/tw", "<text...>")]
        [CommandHelp("与えられた <text...> をツイートします。")]
        private void OnTweetCommand(CommandContext context)
        {
            if (twitter == null)
            {
                Divination.Chat.PrintError("Twitter API の資格情報が設定されていません。");
                return;
            }

            twitter.Statuses.UpdateAsync(context["text"]).ContinueWith(completed =>
            {
                if (completed.IsCompleted)
                {
                    Divination.Chat.Print($"「{context["text"]}」をツイートしました。");
                }
                else if (completed.Exception != null)
                {
                    Divination.Chat.PrintError("ツイートに失敗しました。");
                    PluginLog.Error(completed.Exception, "Failed to tweet");
                }
            });
        }

        private async void WatchTimeline()
        {
            while (!IsDisposed)
            {
                if (Config.ShowListTimeline && Twitter != null && long.TryParse(Config.ListId, out var listId))
                {
                    try
                    {
                        var result = await Twitter.Lists.StatusesAsync(listId, count:5, since_id:Config.SinceId);

                        foreach (var status in result.Reverse())
                        {
                            Dalamud.ChatGui.PrintChat(new XivChatEntry
                            {
                                Type = XivChatType.Echo,
                                Message = new SeString(
                                    new UIForegroundPayload(57),
                                    new TextPayload("[Twitter]"),
                                    UIForegroundPayload.UIForegroundOff,
                                    new TextPayload($"<@{status.User.ScreenName}> {HttpUtility.HtmlDecode(status.Text)}"))
                            });

                            PluginLog.Verbose("Tweet = {Text} ({Id}) from {Name}", status.Text, status.Id, status.User.Name);
                        }

                        if (result.Count > 0)
                        {
                            Config.SinceId = result[0].Id + 1;
                        }
                    }
                    catch (Exception exception)
                    {
                        Divination.Chat.PrintError("タイムラインの取得に失敗しました。");
                        PluginLog.Error(exception, "Failed to fetch timeline");
                    }
                }

                await Task.Delay(Config.UpdateIntervalInMs);
            }
        }
    }
}
