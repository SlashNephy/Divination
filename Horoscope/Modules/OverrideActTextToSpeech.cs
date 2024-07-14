using System;
using System.Linq;
using System.Reflection;
using Dalamud.Divination.Common.Api.Dalamud;

namespace Divination.Horoscope.Modules;

public class OverrideActTextToSpeech : IModule
{
    public string Id => "override_act_text_to_speech";
    public string Name => "Override IINACT TextToSpeech method";
    public string Description => "Override IINACT TextToSpeech method";

    private object formActMain => AppDomain.CurrentDomain.GetAssemblies()
        .FirstOrDefault(x => x.GetName().Name == "Advanced Combat Tracker")
        ?.GetType("Advanced_Combat_Tracker.ActGlobals")
        ?.GetField("oFormActMain", BindingFlags.Public | BindingFlags.Static)
        ?.GetValue(null)
        ?? throw new Exception("oFormActMain field not found");

    private EventInfo textToSpeechEventInfo => formActMain.GetType()
        .GetEvent("TextToSpeech")
        ?? throw new Exception("TextToSpeech event not found");

    public void Enable()
    {
        if (!Horoscope.Instance.Dalamud.PluginInterface.InstalledPlugins.Any(x => x.Name == "IINACT"))
        {
            throw new Exception("IINACT plugin not insalled");
        }

        // first remove existing IINACT handler
        UnregisterEventHandlers();
        RegisterEventHandler();
    }

    public void Disable()
    {
        UnregisterEventHandlers();
    }

    private void RegisterEventHandler()
    {
        var methodInfo = GetType().GetMethod("OnTextToSpeech", BindingFlags.NonPublic | BindingFlags.Instance)!;
        if (methodInfo == default)
        {
            throw new Exception("OnTextToSpeech method not found");
        }

        var handler = Delegate.CreateDelegate(textToSpeechEventInfo.EventHandlerType!, this, methodInfo);
        textToSpeechEventInfo.AddEventHandler(formActMain, handler);
        DalamudLog.Log.Debug("Registered TextToSpeech handler");
    }

    private void UnregisterEventHandlers()
    {
        var eventField = formActMain.GetType()
            .GetField("TextToSpeech", BindingFlags.NonPublic | BindingFlags.Instance);
        if (eventField == default)
        {
            throw new Exception("TextToSpeech field not found");
        }

        var eventDelegate = eventField.GetValue(formActMain);
        if (eventDelegate == default)
        {
            return;
        }

        foreach (var item in ((Delegate)eventDelegate).GetInvocationList())
        {
            textToSpeechEventInfo.RemoveEventHandler(formActMain, item);
        }
    }

    private void OnTextToSpeech(string text)
    {
        DalamudLog.Log.Debug("TextToSpeech: {Text}", text);

        Horoscope.Instance.Divination.Voiceroid2Proxy.TalkAsync(text);
    }
}
