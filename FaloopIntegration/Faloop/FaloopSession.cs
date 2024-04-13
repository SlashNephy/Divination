using System;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud;

namespace Divination.FaloopIntegration.Faloop;

public class FaloopSession : IDisposable
{
    private readonly FaloopApiClient client = new();

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

        var login = await client.LoginAsync(username, password, initialSession.Data.SessionId, initialSession.Data.Token);
        if (login is not { Success: true })
        {
            DalamudLog.Log.Debug("LoginAsync: login is not success");
            return false;
        }

        IsLoggedIn = true;
        SessionId = login.Data.SessionId;
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
