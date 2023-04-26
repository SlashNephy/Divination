namespace Divination.FaloopIntegration.Faloop.Model;

public record UserLoginResponse(bool Success, string SessionId, string Token);
