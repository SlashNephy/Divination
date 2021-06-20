using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Logger;
using Dalamud.Game.Internal.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Command
{
    public class CommandProcessor : IDisposable
    {
        private static readonly Regex CommandRegex = new(@"^そのコマンドはありません。： (?<command>.+)$", RegexOptions.Compiled);

        private readonly string prefix;
        private readonly object plugin;
        private readonly ChatGui chatGui;
        private readonly IChatClient chatClient;

        private readonly List<DivinationCommand> commands = new();
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(CommandProcessor));

        public CommandProcessor(string prefix, object plugin, ChatGui chatGui, IChatClient chatClient)
        {
            this.prefix = (prefix.StartsWith("/") ? prefix : $"/{prefix}").Trim();
            this.plugin = plugin;
            this.chatGui = chatGui;
            this.chatClient = chatClient;

            chatGui.OnChatMessage += OnChatMessage;
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (type != XivChatType.ErrorMessage || senderId != 0)
            {
                return;
            }

            var match = CommandRegex.Match(message.TextValue).Groups["command"];
            if (match.Success && ProcessCommand(match.Value.Trim()))
            {
                isHandled = true;
            }
        }

        private bool ProcessCommand(string text)
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

        private void DispatchCommand(DivinationCommand command, string[] arguments)
        {
            try
            {
                if (command.Attribute.Strict && (arguments.Length < command.Attribute.MinimalArgumentLength || arguments.Length > command.Attribute.Arguments.Length))
                {
                    throw new ArgumentException("コマンドの引数が正しくありません。");
                }

                var context = new CommandContext(command.Attribute, arguments);

                command.Method.Invoke(command.Method.IsStatic ? null : plugin, new object[] {context});
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

                logger.Error(e, "Error occurred while DispatchCommand");
            }
            finally
            {
                logger.Verbose("=> {Syntax} {Arguments}", command.Attribute.Syntax, string.Join(" ", arguments));
            }
        }

        private void RegisterCommandsByAttribute()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            // public/internal/protected/private/static メソッドを検索し 宣言順にソートする
            foreach (var method in plugin.GetType().BaseType!
                .GetMethods(flags)
                .OrderBy(x => x.MetadataToken)
                .Concat(
                    plugin.GetType()
                        .GetMethods(flags | BindingFlags.DeclaredOnly)
                        .OrderBy(x => x.MetadataToken)
                )
            )
            {
                foreach (var attribute in method.GetCustomAttributes<CommandAttribute>())
                {
                    try
                    {
                        CheckCommandHandler(method, attribute);
                        RegisterCommand(method, attribute);
                    }
                    catch (Exception exception)
                    {
                        logger.Error(exception, "Error occurred while RegisterCommandsByAttribute");
                    }
                }
            }
        }

        private void CheckCommandHandler(MethodInfo method, CommandAttribute attribute)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != 1 || parameters.First().ParameterType != typeof(CommandContext))
            {
                throw new ArgumentException("パラメータが不正です。CommandContext 以外の型が引数となっているため, コマンドハンドラとして登録できません。");
            }

            if (!attribute.Syntax.StartsWith("/"))
            {
                attribute.Syntax = $"{prefix} {attribute.Syntax}";
            }

            attribute.Syntax = attribute.Syntax.Trim().ToLower();
        }

        private void RegisterCommand(MethodInfo method, CommandAttribute attribute)
        {
            commands.Add(new DivinationCommand(attribute, method));
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
                    new UIForegroundPayload(null, 28)
                });
                payloads.AddRange(HighlightAngleBrackets(attribute.Usage));
                payloads.AddRange(new List<Payload> {
                    UIForegroundPayload.UIForegroundOff,
                    new TextPayload(" が追加されました。")
                });

                if (!string.IsNullOrEmpty(attribute.Help))
                {
                    payloads.Add(new TextPayload($"\n  {(char) SeIconChar.ArrowRight} "));
                    payloads.AddRange(HighlightAngleBrackets(attribute.Help));
                }
            });
        }

        private static IEnumerable<Payload> HighlightAngleBrackets(string? text)
        {
            if (text == null)
            {
                yield break;
            }

            var bracketRegex = new Regex(@"(<.+?>)", RegexOptions.Compiled);
            var matches = bracketRegex.Matches(text);
            if (matches.Count > 0)
            {
                var original = text;
                var lastIndex = 0;

                foreach (Match match in matches)
                {
                    yield return new TextPayload(text.Substring(0, match.Index - lastIndex));
                    yield return new UIForegroundPayload(null, 500);
                    yield return new TextPayload(match.Value);
                    yield return UIForegroundPayload.UIForegroundOff;

                    lastIndex = match.Index + match.Length;
                    text = original.Substring(lastIndex);
                }
            }

            yield return new TextPayload(text);
        }

        public void Dispose()
        {
            chatGui.OnChatMessage -= OnChatMessage;

            logger.Dispose();
        }
    }
}
