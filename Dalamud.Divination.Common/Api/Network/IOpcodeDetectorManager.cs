namespace Dalamud.Divination.Common.Api.Network
{
    public interface IOpcodeDetectorManager
    {
        public void Register(IOpcodeDetector detector);
    }
}
