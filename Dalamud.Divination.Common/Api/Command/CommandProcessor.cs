using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Dalamud.Payload;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;

namespace Dalamud.Divination.Common.Api.Command
{
    internal sealed partial class CommandProcessor : ICommandProcessor
    {
        private readonly string pluginName;
        private readonly ChatGui chatGui;
        private readonly IChatClient chatClient;

        private readonly Regex commandRegex = new(@"^そのコマンドはありません。： (?<command>.+)$", RegexOptions.Compiled);
        private readonly List<DivinationCommand> commands = new();
        private readonly object commandsLock = new();

        public string? Prefix { get; }

        public CommandProcessor(string pluginName, string? prefix, ChatGui chatGui, IChatClient chatClient)
        {
            this.pluginName = pluginName;
            Prefix = prefix == null ? null : (prefix.StartsWith("/") ? prefix : $"/{prefix}").Trim();
            this.chatGui = chatGui;
            this.chatClient = chatClient;

            RegisterCommandsByAttribute(new DefaultCommands(this));

            chatGui.CheckMessageHandled += OnChatMessage;
        }

        public IReadOnlyList<DivinationCommand> Commands
        {
            get
            {
                lock (commandsLock)
                {
                    return commands.AsReadOnly();
                }
            }
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (type != XivChatType.ErrorMessage || senderId != 0)
            {
                return;
            }

            var match = commandRegex.Match(message.TextValue).Groups["command"];
            if (match.Success)
            {
                isHandled = ProcessCommand(match.Value.Trim());
                PluginLog.Debug($"Command: {match.Value}");
            }
        }

        public bool ProcessCommand(string text)
        {
            foreach (var command in commands)
            {
                var match = command.Regex.Match(text);
                if (match.Success)
                {
                    DispatchCommand(command, match);
                    return true;
                }
            }

            return false;
        }

        public void DispatchCommand(DivinationCommand command, Match match)
        {
            try
            {
                var context = new CommandContext(command, match);

                command.Method.Invoke(
                    command.Method.IsStatic ? null : command.Instance,
                    command.CanReceiveContext ? new object[] {context} : Array.Empty<object>());
            }
            catch (Exception exception)
            {
                var e = exception.InnerException ?? exception;
                chatClient.PrintError(new List<Payload>
                {
                    new TextPayload(match.Value),
                    new TextPayload("\n"),
                    new TextPayload(e.Message),
                    new TextPayload("\n"),
                    new TextPayload($"Usage: {command.Usage}")
                });

                PluginLog.Error(e, "Error occurred while DispatchCommand for {Command}", command.Method.Name);
            }
            finally
            {
                PluginLog.Verbose("=> {Syntax}", match.Value);
            }
        }

        public void RegisterCommandsByAttribute(ICommandProvider? instance)
        {
            if (instance == null)
            {
                return;
            }

            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            // public/internal/protected/private/static メソッドを検索し 宣言順にソートする
            foreach (var method in instance.GetType().BaseType!
                .GetMethods(flags)
                .OrderBy(x => x.MetadataToken)
                .Concat(
                    instance.GetType()
                        .GetMethods(flags | BindingFlags.DeclaredOnly)
                        .OrderBy(x => x.MetadataToken)
                )
            )
            {
                foreach (var attribute in method.GetCustomAttributes<CommandAttribute>())
                {
                    try
                    {
                        var command = new DivinationCommand(method, instance, attribute, Prefix ?? $"/{pluginName}".Replace("Divination.", string.Empty).ToLower(), pluginName);
                        RegisterCommand(command);
                    }
                    catch (ArgumentException exception)
                    {
                        PluginLog.Error(exception, "Error occurred while RegisterCommandsByAttribute");
                    }
                }
            }
        }

        private void RegisterCommand(DivinationCommand command)
        {
            commands.Add(command);
            PluginLog.Information("コマンド: {Usage} が登録されました。", command.Usage);

            if (command.HideInStartUp)
            {
                return;
            }

            chatClient.Print(payloads =>
            {
                payloads.AddRange(new List<Payload>
                {
                    new TextPayload("コマンド: "),
                    new UIForegroundPayload(28)
                });
                payloads.AddRange(PayloadUtilities.HighlightAngleBrackets(command.Usage));
                payloads.AddRange(new List<Payload> {
                    UIForegroundPayload.UIForegroundOff,
                    new TextPayload(" が追加されました。")
                });

                if (!string.IsNullOrEmpty(command.Help))
                {
                    payloads.Add(new TextPayload($"\n  {SeIconChar.ArrowRight.AsString()} "));
                    payloads.AddRange(PayloadUtilities.HighlightAngleBrackets(command.Help));
                }
            });
        }

        public void Dispose()
        {
            chatGui.CheckMessageHandled -= OnChatMessage;
            commands.Clear();
        }
    }
}
