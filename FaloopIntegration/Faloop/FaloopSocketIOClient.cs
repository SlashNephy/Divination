using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud;
using Divination.FaloopIntegration.Faloop.Model;
using SocketIO.Core;
using SocketIOClient;
using SocketIOClient.Transport;

namespace Divination.FaloopIntegration.Faloop;

// ReSharper disable once InconsistentNaming
public class FaloopSocketIOClient : IDisposable
{
    private readonly SemaphoreSlim clientLock = new(1, 1);
    private SocketIOClient.SocketIO? client;

    public delegate void ConnectedHandler();
    public event ConnectedHandler? OnConnected;

    public delegate void DisconnectedHandler(string cause);
    public event DisconnectedHandler? OnDisconnected;

    public delegate void ErrorHandler(string error);
    public event ErrorHandler? OnError;

    public delegate void MobReportHandler(MobReportData data);
    public event MobReportHandler? OnMobReport;

    public delegate void MessageHandler(SocketIOResponse response);
    public event MessageHandler? OnMessage;

    public delegate void AnyHandler(string name, SocketIOResponse response);
    public event AnyHandler? OnAny;

    public delegate void PingHandler();
    public event PingHandler? OnPing;

    public delegate void PongHandler(TimeSpan span);
    public event PongHandler? OnPong;

    private SocketIOClient.SocketIO CreateClient()
    {
        // The URI path is used as the Socket.IO namespace.
        var newClient = new SocketIOClient.SocketIO("https://faloop.app/legacy",
            new SocketIOOptions
            {
                EIO = EngineIO.V4,
                // reconnection is handled by FaloopIntegration, which logs in again and creates a new client
                Reconnection = false,
                Transport = TransportProtocol.Polling,
                // SocketIOClient appends a trailing "/" to the path.
                Path = "/messaging/socket.io",
                ExtraHeaders = new Dictionary<string, string>
                {
                    {
                        "Accept",
                        "*/*"
                    },
                    {
                        "Referer",
                        "https://faloop.app/"
                    },
                    {
                        "Origin",
                        "https://faloop.app"
                    },
                    {
                        "User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0"
                    },
                },
        });

        // ignore events from clients that have already been replaced
        newClient.OnConnected += (sender, args) => { if (ReferenceEquals(sender, client)) HandleOnConnected(sender, args); };
        newClient.OnDisconnected += (sender, cause) => { if (ReferenceEquals(sender, client)) HandleOnDisconnected(sender, cause); };
        newClient.OnError += (sender, error) => { if (ReferenceEquals(sender, client)) HandleOnError(sender, error); };
        newClient.On("message", response => { if (ReferenceEquals(newClient, client)) HandleOnMessage(response); });
        newClient.OnAny((name, response) => { if (ReferenceEquals(newClient, client)) HandleOnAny(name, response); });
        newClient.OnPing += (sender, args) => { if (ReferenceEquals(sender, client)) HandleOnPing(sender, args); };
        newClient.OnPong += (sender, span) => { if (ReferenceEquals(sender, client)) HandleOnPong(sender, span); };
        return newClient;
    }

    /// <summary>
    /// Connects to Faloop with a new Socket.IO client, replacing the current one.
    /// </summary>
    /// <exception cref="TimeoutException">The server did not accept the connection within <paramref name="timeout"/>.</exception>
    public async Task Connect(FaloopSession session, TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (!session.IsLoggedIn)
        {
            throw new ApplicationException("session is not authenticated.");
        }

        await clientLock.WaitAsync(cancellationToken);
        try
        {
            await CloseClientAsync();

            var newClient = CreateClient();
            newClient.Options.Auth = new Dictionary<string, string?>
            {
                {"sessionid", session.SessionId},
            };
            client = newClient;

            // ConnectAsync only times out at the transport level, and keeps waiting if the server never accepts the namespace connection
            var connectTask = newClient.ConnectAsync();
            var completed = await Task.WhenAny(connectTask, Task.Delay(timeout, cancellationToken));
            if (completed != connectTask)
            {
                // observe the exception thrown once the abandoned client is disposed
                _ = connectTask.ContinueWith(t => _ = t.Exception, TaskContinuationOptions.OnlyOnFaulted);
                await CloseClientAsync();
                cancellationToken.ThrowIfCancellationRequested();
                throw new TimeoutException($"Faloop did not accept the connection within {timeout}.");
            }

            await connectTask;
        }
        finally
        {
            clientLock.Release();
        }
    }

    private async Task CloseClientAsync()
    {
        var oldClient = client;
        client = null;
        if (oldClient == null)
        {
            return;
        }

        try
        {
            await oldClient.DisconnectAsync();
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Debug(exception, nameof(CloseClientAsync));
        }

        oldClient.Dispose();
    }

    private void HandleOnConnected(object? sender, EventArgs __)
    {
        try
        {
            OnConnected?.Invoke();
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(HandleOnConnected));
        }
        finally
        {
            (sender as SocketIOClient.SocketIO)?.EmitAsync("ack");
        }
    }

    private void HandleOnDisconnected(object? _, string cause)
    {
        try
        {
            OnDisconnected?.Invoke(cause);
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(HandleOnDisconnected));
        }
    }

    private void HandleOnError(object? _, string error)
    {
        try
        {
            OnError?.Invoke(error);
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(HandleOnError));
        }
    }

    private void HandleOnMessage(SocketIOResponse response)
    {
        var payload = response.GetValue<FaloopEventPayload>();
        if (payload is not { Type: FaloopEventTypes.MobType, SubType: FaloopEventTypes.ReportSubType })
        {
            return;
        }

        var data = payload.Data.Deserialize<MobReportData>();
        if (data == default)
        {
            return;
        }

        try
        {
            OnMobReport?.Invoke(data);
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(HandleOnMessage));
        }

        try
        {
            OnMessage?.Invoke(response);
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(HandleOnMessage));
        }
    }

    private void HandleOnAny(string name, SocketIOResponse response)
    {
        try
        {
            OnAny?.Invoke(name, response);
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(HandleOnAny));
        }
    }

    private void HandleOnPing(object? _, EventArgs __)
    {
        try
        {
            OnPing?.Invoke();
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(HandleOnPing));
        }
    }

    private void HandleOnPong(object? _, TimeSpan span)
    {
        try
        {
            OnPong?.Invoke(span);
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(HandleOnPong));
        }
    }

    public void Dispose()
    {
        var oldClient = client;
        client = null;
        oldClient?.DisconnectAsync();
        oldClient?.Dispose();
        clientLock.Dispose();
    }
}
