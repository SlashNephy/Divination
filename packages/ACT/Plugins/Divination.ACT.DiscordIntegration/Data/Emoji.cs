namespace Divination.ACT.DiscordIntegration.Data
{
    internal class Emoji
    {
        public Emoji(string name, string id)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
