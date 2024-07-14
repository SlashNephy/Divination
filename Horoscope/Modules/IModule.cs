namespace Divination.Horoscope.Modules;

public interface IModule
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }

    public void Enable();
    public void Disable();
}
