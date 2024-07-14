using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dalamud.Divination.Common.Api.Dalamud;
using Divination.Horoscope.Modules;

namespace Divination.Horoscope;

public class ModuleManager
{
    private readonly Dictionary<string, IModule> modules = new();
    private readonly Dictionary<string, bool> states = new();

    public ModuleManager()
    {
        LoadModules();
    }

    private void LoadModules()
    {
        var modules = typeof(IModule).Assembly.GetExportedTypes()
            .Where(type => type.IsAssignableTo(typeof(IModule)) && !type.IsInterface)
            .Select(type => (IModule)Activator.CreateInstance(type)!);
        foreach (var module in modules)
        {
            this.modules[module.Id] = module;
            states[module.Id] = false;
        }
    }

    public Dictionary<string, IModule> Modules => modules;

    public void EnableAll()
    {
        foreach (var id in modules.Keys)
        {
            if (Horoscope.Instance.Config.ModuleStates.TryGetValue(id, out var shouldEnable) && shouldEnable)
            {
                Enable(id);
            }
        }
    }

    public void DisableAll()
    {
        foreach (var id in modules.Keys)
        {
            Disable(id);
        }
    }

    public void Enable(string id)
    {
        if (states.TryGetValue(id, out var alreadyEnabled) && alreadyEnabled)
        {
            return;
        }

        if (!modules.TryGetValue(id, out var module))
        {
            return;
        }

        try
        {
            module.Enable();
            states[id] = true;
            DalamudLog.Log.Info("{Module} enabled", module.Name);
        }
        catch (Exception e)
        {
            DalamudLog.Log.Error("{Module} failed to enable: {Error}", module.Name, e);
        }
    }

    public void Disable(string id)
    {
        if (states.TryGetValue(id, out var enabled) && !enabled)
        {
            return;
        }

        if (!modules.TryGetValue(id, out var module))
        {
            return;
        }

        try
        {
            module.Disable();
            states[id] = false;
            DalamudLog.Log.Info("{Module} disabled", module.Name);
        }
        catch (Exception e)
        {
            DalamudLog.Log.Error("{Module} failed to disable: {Error}", module.Name, e);
        }
    }
}
