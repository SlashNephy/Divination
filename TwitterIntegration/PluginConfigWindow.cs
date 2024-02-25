using System.Diagnostics;
using System.Linq;
using CoreTweet;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;

namespace Divination.TwitterIntegration;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{TwitterIntegration.Instance.Name} 設定", ref IsOpen, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGuiEx.TextConfig("Consumer Key", ref Config.ConsumerKey, 64);
            ImGuiEx.TextConfig("Consumer Secret", ref Config.ConsumerSecret, 64);
            ImGuiEx.TextConfig("Access Token", ref Config.AccessToken, 64);
            ImGuiEx.TextConfig("Access Token Secret", ref Config.AccessTokenSecret, 64);

            ImGui.Separator();

            ImGuiEx.CheckboxConfig("Show list TL", ref Config.ShowListTimeline);
            ImGuiEx.TextConfig("List ID", ref Config.ListId, 32);
            ImGui.InputInt("Update Interval (ms)", ref Config.UpdateIntervalInMs);

            ImGui.Separator();

            CreateAuthenticateButton();
            ImGui.SameLine();
            CreateFindListButton();

            ImGui.Separator();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;

                TwitterIntegration.Instance.Dalamud.PluginInterface.SavePluginConfig(Config);
                DalamudLog.Log.Information("Config saved");
            }

            ImGui.End();
        }

        CreatePinWindow();
    }

    private static OAuth.OAuthSession? _session;
    private static bool _isPinWindowDrawing;
    private static string _pin = string.Empty;

    private static void CreateAuthenticateButton()
    {
        if (ImGui.Button("Authenticate"))
        {
            if (string.IsNullOrEmpty(TwitterIntegration.Instance.Config.ConsumerKey) || string.IsNullOrEmpty(TwitterIntegration.Instance.Config.ConsumerSecret))
            {
                TwitterIntegration.Instance.Divination.Chat.PrintError("Consumer Key または Consumer Secret が設定されていません。");
                return;
            }

            _session = OAuth.Authorize(TwitterIntegration.Instance.Config.ConsumerKey, TwitterIntegration.Instance.Config.ConsumerSecret);
            Process.Start(_session!.AuthorizeUri.AbsoluteUri);

            _isPinWindowDrawing = true;
        }
    }

    private static void CreatePinWindow()
    {
        if (_isPinWindowDrawing && _pin.Length == 7)
        {
            var tokens = _session?.GetTokens(_pin);
            if (tokens != null)
            {
                TwitterIntegration.Instance.Config.AccessToken = tokens.AccessToken;
                TwitterIntegration.Instance.Config.AccessTokenSecret = tokens.AccessTokenSecret;

                TwitterIntegration.Instance.Divination.Chat.Print("Twitter API への認証に成功しました。");
            }

            _session = null;
            _isPinWindowDrawing = false;
            _pin = string.Empty;
        }

        if (!_isPinWindowDrawing)
        {
            return;
        }

        if (ImGui.Begin("PinWindow", ref _isPinWindowDrawing, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration))
        {
            ImGui.Text("Enter PIN code:");

            ImGui.InputText("", ref _pin, 7);
            ImGui.End();
        }
    }

    private static void CreateFindListButton()
    {
        if (ImGui.Button("Find List"))
        {
            if (TwitterIntegration.Twitter == null)
            {
                TwitterIntegration.Instance.Divination.Chat.PrintError("Twitter API の資格情報が設定されていません。");
                return;
            }

            TwitterIntegration.Twitter.Lists.OwnershipsAsync(count: 50).ContinueWith(completed =>
            {
                if (completed.IsCompleted)
                {
                    TwitterIntegration.Instance.Divination.Chat.Print(
                        $"使用可能なリスト一覧です。\n{string.Join("\n", completed.Result.Select(list => $"{list.Name} (ID: {list.Id})"))}");
                }
                else if (completed.Exception != null)
                {
                    TwitterIntegration.Instance.Divination.Chat.PrintError("リスト一覧の取得に失敗しました。");
                    DalamudLog.Log.Error(completed.Exception, "Error occurred while OwnershipsAsync");
                }
            });
        }
    }
}
