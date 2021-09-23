using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dalamud.Divination.Common.Api.Command
{
    public interface ICommandProcessor : IDisposable
    {
        public string Prefix { get; }
        public IReadOnlyList<DivinationCommand> Commands { get; }

        public bool ProcessCommand(string text);
        public void DispatchCommand(DivinationCommand command, Match match);
        public void RegisterCommandsByAttribute(ICommandProvider instance);
    }
}
