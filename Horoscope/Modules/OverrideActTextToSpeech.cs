using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud;

namespace Divination.Horoscope.Modules;

public class OverrideActTextToSpeech : IModule
{
    public string Id => "override_act_text_to_speech";
    public string Name => "Override IINACT TextToSpeech method";
    public string Description => "Override IINACT TextToSpeech method";

    private static object FormActMain => AppDomain.CurrentDomain.GetAssemblies()
        .FirstOrDefault(x => x.GetName().Name == "Advanced Combat Tracker")
        ?.GetType("Advanced_Combat_Tracker.ActGlobals")
        ?.GetField("oFormActMain", BindingFlags.Public | BindingFlags.Static)
        ?.GetValue(null)
        ?? throw new ReflectionException("oFormActMain field not found");

    private static EventInfo TextToSpeechEventInfo => FormActMain.GetType()
        .GetEvent("TextToSpeech")
        ?? throw new ReflectionException("TextToSpeech event not found");

    private CancellationTokenSource? cancellation;

    public void Enable()
    {
        cancellation = new CancellationTokenSource();
        Task.Run(async () =>
        {
            while (!cancellation.IsCancellationRequested)
            {
                try
                {
                    // first remove existing IINACT handler
                    UnregisterEventHandlers();
                    RegisterEventHandler();
                    break;
                }
                catch (ReflectionException e)
                {
                    DalamudLog.Log.Warning(e, "Failed to register TextToSpeech handler. Retrying in 3s...");
                    await Task.WhenAny(Task.Delay(3000, cancellation.Token));
                }
            }
        }, cancellation.Token);
    }

    public void Disable()
    {
        cancellation?.Cancel();
        cancellation?.Dispose();
        UnregisterEventHandlers();
    }

    private void RegisterEventHandler()
    {
        if (Horoscope.Instance.Dalamud.PluginInterface.InstalledPlugins.All(x => x.Name != "IINACT"))
        {
            throw new Exception("IINACT plugin not installed");
        }

        var methodInfo = GetType().GetMethod("OnTextToSpeech", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new ReflectionException("OnTextToSpeech method not found");
        var handler = Delegate.CreateDelegate(TextToSpeechEventInfo.EventHandlerType!, this, methodInfo);
        TextToSpeechEventInfo.AddEventHandler(FormActMain, handler);
        DalamudLog.Log.Debug("Registered TextToSpeech handler");
    }

    private static void UnregisterEventHandlers()
    {
        var eventField = FormActMain.GetType()
            .GetField("TextToSpeech", BindingFlags.NonPublic | BindingFlags.Instance);
        if (eventField == default)
        {
            throw new ReflectionException("TextToSpeech field not found");
        }

        var eventDelegate = eventField.GetValue(FormActMain);
        if (eventDelegate == null)
        {
            return;
        }

        foreach (var item in ((Delegate)eventDelegate).GetInvocationList())
        {
            TextToSpeechEventInfo.RemoveEventHandler(FormActMain, item);
        }
    }

    private void OnTextToSpeech(string text)
    {
        DalamudLog.Log.Debug("TextToSpeech: {Text}", text);

        Horoscope.Instance.Divination.Voiceroid2Proxy.TalkAsync(text);
    }

    private class ReflectionException(string message) : Exception(message);
}
