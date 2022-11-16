namespace Dalamud.Divination.Common.Api.Network
{
    public interface IOpcodeDetectorManager
    {
        public void Enable();
        public void Disable();
        public void Register(IOpcodeDetector detector);
    }
}
