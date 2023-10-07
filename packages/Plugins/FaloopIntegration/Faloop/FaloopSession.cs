﻿using System;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Logging;

namespace Divination.FaloopIntegration.Faloop;

public class FaloopSession : IDisposable
{
    private readonly FaloopApiClient client = new();
    public readonly FaloopEmbedData EmbedData;

    public FaloopSession()
    {
        EmbedData = new FaloopEmbedData(client);
    }

    public bool IsLoggedIn { get; private set; }

    public string? SessionId { get; private set; }

    public async Task<bool> LoginAsync(string username, string password)
    {
        Logout();

        var initialSession = await client.RefreshAsync();
        if (initialSession is not { Success: true })
        {
            DalamudLog.Log.Debug("LoginAsync: initialSession is not success");
            return false;
        }

        var login = await client.LoginAsync(username, password, initialSession.SessionId, initialSession.Token);
        if (login is not { Success: true })
        {
            DalamudLog.Log.Debug("LoginAsync: login is not success");
            return false;
        }

        try
        {
            await EmbedData.Initialize();
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, "LoginAsync: EmbedData.Initialize failed");
            return false;
        }

        IsLoggedIn = true;
        SessionId = login.SessionId;
        return true;
    }

    private void Logout()
    {
        IsLoggedIn = false;
        SessionId = default;
    }

    public void Dispose()
    {
        client.Dispose();
    }
}
