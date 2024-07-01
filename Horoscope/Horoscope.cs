using System;
using System.Linq;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Plugin;
using Divination.Horoscope.Modules;
using ECommons;

namespace Divination.Horoscope;

public class Horoscope : DivinationPlugin<Horoscope>, IDalamudPlugin
{
    private readonly IModule[] modules;

    public Horoscope(IDalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        ECommonsMain.Init(pluginInterface, this);

        modules = typeof(IModule).Assembly.GetExportedTypes()
            .Where(type => type.IsAssignableTo(typeof(IModule)) && !type.IsInterface)
            .Select(type => (IModule)Activator.CreateInstance(type)!)
            .ToArray();

        foreach (var module in modules)
        {
            try
            {
                module.Enable();
                DalamudLog.Log.Info("{Module} enabled", module.Name);
            }
            catch (Exception e)
            {
                DalamudLog.Log.Error("{Module} failed to enable: {Error}", module.Name, e);
            }
        }
    }

    protected override void ReleaseManaged()
    {
        foreach (var module in modules)
        {
            try
            {
                module.Disable();
                DalamudLog.Log.Info("{Module} disabled", module.Name);
            }
            catch (Exception e)
            {
                DalamudLog.Log.Error("{Module} failed to disable: {Error}", module.Name, e);
            }
        }

        ECommonsMain.Dispose();
    }
}
