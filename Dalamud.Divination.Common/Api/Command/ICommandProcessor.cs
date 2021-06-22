using System;
using System.Collections.Generic;

namespace Dalamud.Divination.Common.Api.Command
{
    public interface ICommandProcessor : IDisposable
    {
        public string Prefix { get; }
        public IReadOnlyList<DivinationCommand> Commands { get; }

        public bool ProcessCommand(string text);
        public void DispatchCommand(DivinationCommand command, string[] arguments);
        public void RegisterCommandsByAttribute(object instance);
    }
}
