using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Dalamud.Payload;
using Dalamud.Divination.Common.Api.Logger;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

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
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(CommandProcessor));

        public string Prefix { get; }

        public CommandProcessor(string pluginName, string prefix, ChatGui chatGui, IChatClient chatClient)
        {
            this.pluginName = pluginName;
            Prefix = (prefix.StartsWith("/") ? prefix : $"/{prefix}").Trim();
            this.chatGui = chatGui;
            this.chatClient = chatClient;

            RegisterCommandsByAttribute(new DefaultCommands(this));

            chatGui.ChatMessage += OnChatMessage;
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
            if (match.Success && ProcessCommand(match.Value.Trim()))
            {
                isHandled = true;
            }
        }

        public bool ProcessCommand(string text)
        {
            foreach (var command in commands)
            {
                // コマンドに一致 or コマンド + 空白で始まるとき (文字種区別なし)
                if (text.Equals(command.Attribute.Syntax, StringComparison.CurrentCultureIgnoreCase)
                    || text.StartsWith($"{command.Attribute.Syntax} ", StringComparison.CurrentCultureIgnoreCase))
                {
                    var arguments = text.Remove(0, command.Attribute.Syntax.Length)
                        .Trim()
                        .Split(' ')
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToArray();

                    DispatchCommand(command, arguments);
                    return true;
                }
            }

            return false;
        }

        public void DispatchCommand(DivinationCommand command, string[] arguments)
        {
            try
            {
                if (command.Attribute.Strict && (arguments.Length < command.Attribute.MinimalArgumentLength || arguments.Length > command.Attribute.Arguments.Length))
                {
                    throw new ArgumentException("コマンドの引数が正しくありません。");
                }

                var context = new CommandContext(command.Attribute, arguments);

                command.Method.Invoke(
                    command.Method.IsStatic ? null : command.Instance,
                    command.Attribute.ReceiveContext ? new object[] {context} : new object[] {});
            }
            catch (Exception exception)
            {
                var e = exception.InnerException ?? exception;
                chatClient.PrintError(new List<Payload>
                {
                    new TextPayload($"{command.Attribute.Syntax} {string.Join(" ", arguments)}"),
                    new TextPayload(e.Message),
                    new TextPayload($"Usage: {command.Attribute.Usage}")
                });

                logger.Error(e, "Error occurred while DispatchCommand for {Command}", command.Method.Name);
            }
            finally
            {
                logger.Verbose("=> {Syntax} {Arguments}", command.Attribute.Syntax, string.Join(" ", arguments));
            }
        }

        public void RegisterCommandsByAttribute(ICommandProvider instance)
        {
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
                        CheckCommandHandler(instance, method, attribute);
                        RegisterCommand(method, attribute, instance);
                    }
                    catch (ArgumentException exception)
                    {
                        logger.Error(exception, "Error occurred while RegisterCommandsByAttribute");
                    }
                }
            }
        }

        private void CheckCommandHandler(ICommandProvider instance, MethodInfo method, CommandAttribute attribute)
        {
            var parameters = method.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    attribute.ReceiveContext = false;
                    break;
                case 1 when parameters.First().ParameterType == typeof(CommandContext):
                    attribute.ReceiveContext = true;
                    break;
                default:
                    throw new ArgumentException($"引数が不正です。CommandContext 以外の型が引数となっているため, コマンドハンドラとして登録できません。\nMethod = {instance.GetType().FullName}#{method.Name}, Attribute = {attribute}");
            }

            if (!attribute.Syntax.StartsWith("/"))
            {
                attribute.Syntax = $"{Prefix} {attribute.Syntax}";
            }

            attribute.Syntax = attribute.Syntax.Trim().ToLower();
        }

        private void RegisterCommand(MethodInfo method, CommandAttribute attribute, object instance)
        {
            commands.Add(new DivinationCommand(attribute, method, instance));
            logger.Information("コマンド: {Usage} が登録されました。", attribute.Usage);

            if (!attribute.ShowInHelp)
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
                payloads.AddRange(PayloadUtilities.HighlightAngleBrackets(attribute.Usage));
                payloads.AddRange(new List<Payload> {
                    UIForegroundPayload.UIForegroundOff,
                    new TextPayload(" が追加されました。")
                });

                if (!string.IsNullOrEmpty(attribute.Help))
                {
                    payloads.Add(new TextPayload($"\n  {SeIconChar.ArrowRight.AsString()} "));
                    payloads.AddRange(PayloadUtilities.HighlightAngleBrackets(attribute.Help));
                }
            });
        }

        public void Dispose()
        {
            chatGui.ChatMessage -= OnChatMessage;
            commands.Clear();

            logger.Dispose();
        }
    }
}
