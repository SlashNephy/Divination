using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Divination.Common.Toast;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;

#nullable enable
namespace Divination.ACT.CustomTrigger
{
    public class Settings : PluginSettings
    {
        public Settings()
        {
            this.Bind("LS1", Plugin.TabControl.ls1CheckBox);
            this.Bind("CWLS1", Plugin.TabControl.cwls1CheckBox);
            this.Bind("Copy", Plugin.TabControl.copyCheckBox);
            this.Bind("TTSFCChat", Plugin.TabControl.ttsFCChatCheckBox);
            this.Bind("FCChat", Plugin.TabControl.fcChatToDiscordCheckBox);
            this.Bind("Gate", Plugin.TabControl.gateCheckBox);
        }

        public IEnumerable<Trigger> Triggers { get; } = new List<Trigger>
        {
            // echo #<text> => TTS <text>
            new Trigger(
                new Regex(@"^00:0038:\#(.+)$"), e =>
                {
                    var text = e.MatchedGroup.Trim();
                    var seRegex = new Regex(@"<se\.\d+>");
                    text = seRegex.Replace(text, "");

                    Plugin.Speak(text);
                }
            ),

            // echo $<text> => Toast <text>
            new Trigger(
                new Regex(@"^00:0038:\$(.+)$"), e =>
                {
                    string title, subTitle;

                    var match = new Regex(@"^(.+?):(.+)$").Match(e.MatchedGroup.Trim());
                    if (match.Success)
                    {
                        title = match.Groups[1].Value;
                        subTitle = match.Groups[2].Value;
                    }
                    else
                    {
                        title = "Divination.ACT.CustomTrigger";
                        subTitle = e.MatchedGroup;
                    }

                    var toast = new ToastContent
                    {
                        Visual = new ToastVisual
                        {
                            BindingGeneric = new ToastBindingGeneric
                            {
                                Children =
                                {
                                    new AdaptiveText
                                    {
                                        Text = title
                                    },
                                    new AdaptiveText
                                    {
                                        Text = subTitle
                                    }
                                }
                            }
                        }
                    };
                    toast.Show();
                }
            ),

            // LS1
            new Trigger(
                new Regex(@"^00:0010:(.+?):(.+)$"), async e =>
                {
                    if (!LS1Enabled)
                    {
                        return;
                    }

                    var (username, text) = e.MatchedGroups2;
                    await WebhookHandler.SendToMobHunt(username, text, "LS1");

                    if (CopyLSMessagesEnabled && IsFFXIVActive())
                    {
                        CopyToClipboard(text);
                    }
                }
            ),

            // CWLS1
            new Trigger(
                new Regex(@"^00:0025:(.+?):(.+)$"), async e =>
                {
                    if (!CWLS1Enabled)
                    {
                        return;
                    }

                    var (username, text) = e.MatchedGroups2;
                    await WebhookHandler.SendToMobHunt(username, text, "CWLS1");

                    if (CopyLSMessagesEnabled && IsFFXIVActive())
                    {
                        CopyToClipboard(text);
                    }
                }
            ),

            // echo %<username>:<text>
            new Trigger(
                new Regex(@"^00:0038:%(.+?):(.+)$"), async e =>
                {
                    var (username, text) = e.MatchedGroups2;
                    await WebhookHandler.SendToMobHunt(username, text, "Debug");
                }
            ),

            // FC Chat
            new Trigger(
                new Regex(@"^00:0018:(.+?):(.+)$"), async e =>
                {
                    var (username, text) = e.MatchedGroups2;

                    // TTS on AFK
                    if (FCChatOnAFKEnabled && !IsFFXIVActive())
                    {
                        Plugin.Speak($"{username}: {text}");
                    }

                    // To Discord
                    if (FCChatToDiscordEnabled)
                    {
                        await WebhookHandler.SendToGeneralVc(username, text);
                    }
                }
            ),

            // GS
            new Trigger(
                new Regex(@"^00:0044:お客様案内窓口:「.+」にて、ゴールドソーサー主催の、 「(.+)」が始まりました。 皆様のご参加を、お待ちしております！$"), async e =>
                {
                    if (!GateEnabled)
                    {
                        return;
                    }

                    var gate = e.MatchedGroup;
                    await WebhookHandler.SendToWorldSatellite("[G.A.T.E.] お客様案内窓口", $"まもなくゴールドソーサーで「{gate}」が始まります。");
                }
            )
        };

        private static void CopyToClipboard(string text)
        {
            var thread = new Thread(() => Clipboard.SetText(text));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        // ReSharper disable once InconsistentNaming
        private static bool IsFFXIVActive()
        {
            var foreground = GetForegroundProcess();
            return foreground?.ProcessName == "ffxiv_dx11";
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private static Process? GetForegroundProcess()
        {
            var window = GetForegroundWindow();
            if (window == IntPtr.Zero)
            {
                return null;
            }

            GetWindowThreadProcessId(window, out var pid);

            return Process.GetProcessById((int) pid);
        }

        // ReSharper disable InconsistentNaming
        private static bool LS1Enabled => Plugin.TabControl.ls1CheckBox.Checked;
        private static bool CWLS1Enabled => Plugin.TabControl.cwls1CheckBox.Checked;
        private static bool CopyLSMessagesEnabled => Plugin.TabControl.copyCheckBox.Checked;
        private static bool FCChatOnAFKEnabled => Plugin.TabControl.ttsFCChatCheckBox.Checked;
        private static bool FCChatToDiscordEnabled => Plugin.TabControl.fcChatToDiscordCheckBox.Checked;

        private static bool GateEnabled => Plugin.TabControl.gateCheckBox.Checked;
        // ReSharper restore InconsistentNaming
    }
}
