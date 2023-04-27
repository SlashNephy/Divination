using System.Diagnostics.CodeAnalysis;

namespace Divination.FaloopIntegration.Config;

public enum Jurisdiction
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")] None,
    World,
    DataCenter,
    Region,
    All,
}
