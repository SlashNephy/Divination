namespace Divination.FaloopIntegration.Config;

public enum Jurisdiction : byte
{
    None = 0,
    World = 10,
    DataCenter = 100,
    Region = 200,
    Travelable = 210,
    All = byte.MaxValue,
}
