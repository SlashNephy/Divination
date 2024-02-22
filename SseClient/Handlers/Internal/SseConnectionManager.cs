using System;
using System.Threading;
using Dalamud.Logging;
using Divination.SseClient.Payloads;
using EvtSource;
using Newtonsoft.Json;

namespace Divination.SseClient.Handlers;

public class SseConnectionManager : IDisposable
{
    public event OnSsePayload? SsePayload;
    public delegate void OnSsePayload(string eventId, SsePayload payload);

    public bool IsDisconnected;
    public bool IsUnderMaintenance;
    public bool IsConfigChanged;
    private bool isDisposed;

    private EventSourceReader? eventSourceReader;

    public void Connect()
    {
        if (isDisposed)
        {
            return;
        }

        if (string.IsNullOrEmpty(SseClient.Instance.Config.EndpointUrl))
        {
            SseClient.Instance.Divination.Chat.PrintError("SseServer の URL が設定されていません。接続先を設定で入力してください。");
            return;
        }

        eventSourceReader?.Dispose();
        eventSourceReader = new EventSourceReader(new Uri($"{SseClient.Instance.Config.EndpointUrl}/stream?token={SseClient.Instance.Config.Token}"));
        eventSourceReader.MessageReceived += OnSseMessageReceived;
        eventSourceReader.Disconnected += OnSseDisconnected;

        eventSourceReader.Start();
    }

    private void OnSseMessageReceived(object sender, EventSourceMessageEventArgs e)
    {
        try
        {
            var payload = JsonConvert.DeserializeObject<SsePayload>(e.Message);
            if (payload == null || SseUtils.IsSelfPayload(payload))
            {
                return;
            }

            SsePayload?.Invoke(e.Event, payload);
        }
        catch (Exception ex)
        {
            PluginLog.Error(ex, "Error occurred while handling SSE payload");
        }
        finally
        {
            PluginLog.Verbose($"Event = {e.Event}, Message = {e.Message}, Id = {e.Id}");
        }
    }

    private void OnSseDisconnected(object sender, DisconnectEventArgs e)
    {
        if (isDisposed || IsConfigChanged)
        {
            return;
        }

        if (!IsDisconnected && !IsUnderMaintenance)
        {
            SseClient.Instance.Divination.Chat.PrintError("SseServer から切断されました。再接続を試みます。");
        }

        PluginLog.Error(e.Exception, "Error occurred while sse connecting");

        IsDisconnected = true;
        Thread.Sleep(Math.Max(e.ReconnectDelay, 3000));

        Connect();
    }

    public void Dispose()
    {
        isDisposed = true;
        eventSourceReader?.Dispose();
    }
}