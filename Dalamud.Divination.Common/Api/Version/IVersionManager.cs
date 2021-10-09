namespace Dalamud.Divination.Common.Api.Version
{
    public interface IVersionManager
    {
        public IGitVersion Plugin { get; }
        public IGitVersion Divination { get; }
    }
}
