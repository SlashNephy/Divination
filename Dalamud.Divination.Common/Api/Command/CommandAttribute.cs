using System;
using System.Linq;

namespace Dalamud.Divination.Common.Api.Command
{
    /// <summary>
    /// この関数がコマンドによって呼び出し可能であることを通知します。このクラスは継承できません。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CommandAttribute : Attribute
    {
        /// <summary>
        /// この関数が指定された構文で呼び出し可能であることを通知します。
        /// </summary>
        /// <param name="syntax">コマンドを呼び出す構文。(e.g. /hello)</param>
        /// <param name="arguments">コマンドの引数の名前の配列。</param>
        public CommandAttribute(string syntax, params string[] arguments)
        {
            Syntax = syntax;
            Arguments = arguments;
            Strict = arguments.Length > 0;
        }

        /// <summary>
        /// コマンドの構文。
        /// </summary>
        public string Syntax { get; set; }

        /// <summary>
        /// コマンドの引数の名前。
        /// </summary>
        public string[] Arguments { get; set; }

        /// <summary>
        /// このコマンドを処理する際に引数の数をチェックするかどうか。
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        /// このコマンドを説明するヘルプテキスト。
        /// </summary>
        public string? Help { get; set; }

        /// <summary>
        /// このコマンドをヘルプコマンドに表示するかどうか。
        /// </summary>
        public bool ShowInHelp { get; set; } = true;

        /// <summary>
        /// 関数のパラメータの種類の配列。
        /// </summary>
        internal ParameterKind[] Types { get; set; } = new ParameterKind[0];

        /// <summary>
        /// パラメータの種類。
        /// </summary>
        internal enum ParameterKind
        {
            Context,
            Argument
        }

        /// <summary>
        /// このコマンドが受け取る引数の最小の数。
        /// </summary>
        internal int MinimalArgumentLength => Arguments.Count(x => !x.EndsWith("?"));

        /// <summary>
        /// このコマンドの使用例。
        /// </summary>
        public string Usage
        {
            get
            {
                return $"{Syntax} {string.Join(" ", Arguments.Select(x => $"<{x}>"))}".Trim();
            }
        }
    }
}
