namespace Divination.FaloopIntegration.Faloop.Model;

public record UserData(string SessionId, string Token);
public record UserRefreshResponse(bool Success, UserData Data);
