using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Utility;

namespace Dalamud.Divination.Common.Api.Command
{
    public sealed class DivinationCommand
    {
        public MethodInfo Method { get; }
        internal readonly ICommandProvider Instance;
        public CommandAttribute Attribute { get; }
        public string? Help { get; }
        public bool HideInHelp { get; }
        public bool HideInStartUp { get; }

        public bool CanReceiveContext { get; }
        public string[] Syntaxes { get; }
        public Regex Regex { get; }

        private static readonly Regex ArgRegex = new(@"<(\w+)>", RegexOptions.Compiled);
        private static readonly Regex OptionalArgRegex = new(@"<(\w+)\?>", RegexOptions.Compiled);
        private static readonly Regex VarargRegex = new(@"<(\w+)\.\.\.>", RegexOptions.Compiled);

        public DivinationCommand(MethodInfo method, ICommandProvider instance, CommandAttribute attribute, string defaultPrefix, string pluginName)
        {
            Attribute = attribute;
            Method = method;
            Instance = instance;

            var parameters = method.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    CanReceiveContext = false;
                    break;
                case 1 when parameters.First().ParameterType == typeof(CommandContext):
                    CanReceiveContext = true;
                    break;
                default:
                    throw new ArgumentException($"引数が不正です。CommandContext 以外の型が引数となっているため, コマンドハンドラとして登録できません。\nMethod = {instance.GetType().FullName}#{method.Name}");
            }

            if (!attribute.Commands.First().StartsWith("/"))
            {
                Syntaxes = attribute.Commands.Prepend(defaultPrefix).Select(x => x.ToLower()).ToArray();
            }
            else
            {
                Syntaxes = attribute.Commands.Select(x => x.ToLower()).ToArray();
            }

            Syntaxes = Syntaxes.Where(x => !x.IsNullOrWhitespace()).Select(x => x.Trim()).ToArray();

            var syntaxes = Syntaxes.Select(x =>
            {
                var optionalArg = OptionalArgRegex.Match(x);
                if (optionalArg.Success)
                {
                    return @$"(?<{optionalArg.Groups[1].Value}>\S+)?";
                }

                var vararg = VarargRegex.Match(x);
                if (vararg.Success)
                {
                    return @$"(?<{vararg.Groups[1].Value}>.+)";
                }

                var arg = ArgRegex.Match(x);
                return arg.Success ? @$"(?<{arg.Groups[1].Value}>\S+)" : x;
            });
            Regex = new Regex($"^{string.Join(" ", syntaxes)}$", RegexOptions.IgnoreCase);

            Help = method.GetCustomAttribute<CommandHelpAttribute>()?.Help.Replace("{Name}", pluginName);

            var hidden = method.GetCustomAttribute<HiddenCommandAttribute>();
            HideInHelp = hidden?.HideInHelp ?? false;
            HideInStartUp = hidden?.HideInStartUp ?? false;
        }

        /// <summary>
        /// このコマンドの使用例。
        /// </summary>
        public string Usage => string.Join(" ", Syntaxes);
    }
}
