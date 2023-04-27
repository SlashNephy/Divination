using System.Collections.Generic;
using System.Linq;

namespace Divination.DiscordIntegration.Ipc;

public static class IpcPayloadManager
{
    public static void UpdateVariables(string? source, string? group, Dictionary<string, object?>? variables)
    {
        if (variables == null)
        {
            return;
        }

        lock (Formatter.IpcVariables)
        {
            foreach (var (key, variable) in variables)
            {
                Formatter.IpcVariables[$"{{{key}}}"] = new IpcValueRecord(variable?.ToString() ?? string.Empty, source, group);
            }
        }
    }

    public static void ClearVariables(string? source, string? group)
    {
        lock (Formatter.IpcVariables)
        {
            foreach (var (key, _) in Formatter.IpcVariables.Where(pair => pair.Value.Source == source && pair.Value.Group == group).ToList())
            {
                Formatter.IpcVariables.Remove(key);
            }
        }
    }

    public static void UpdateTemplates(string? source, string? group, Dictionary<string, string?>? templates)
    {
        if (templates == null)
        {
            return;
        }

        lock (Formatter.IpcTemplates)
        {
            foreach (var (key, template) in templates)
            {
                Formatter.IpcTemplates[key] = new IpcValueRecord(template ?? string.Empty, source, group);
            }
        }
    }

    public static void ClearTemplates(string? source, string? group)
    {
        lock (Formatter.IpcTemplates)
        {
            foreach (var (key, _) in Formatter.IpcTemplates.Where(pair => pair.Value.Source == source && pair.Value.Group == group).ToList())
            {
                Formatter.IpcTemplates.Remove(key);
            }
        }
    }
}
