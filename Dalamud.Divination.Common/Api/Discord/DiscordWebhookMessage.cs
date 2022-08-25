using System;
using System.Collections.Generic;

namespace Dalamud.Divination.Common.Api.Discord
{
    public record DiscordWebhookMessage
    {
        public string? Content { get; init; }
        public string? Username { get; init; }
        public string? AvatarUrl { get; init; }
        public List<DiscordEmbed>? Embeds { get; init; }

        public record DiscordEmbed
        {
            public enum EmbedType
            {
                Rich, Image, Video,
                Gifv, Article, Link,
            }

            public string? Title { get; init; }
            public EmbedType? Type { get; init; }
            public string? Description { get; init; }
            public string? Url { get; init; }
            public DateTime Timestamp { get; init; }
            public int? Color { get; init; }
            public EmbedFooter? Footer { get; init; }
            public EmbedImage? Image { get; init; }
            public EmbedThumbnail? Thumbnail { get; init; }
            public EmbedVideo? Video { get; init; }
            public EmbedProvider? Provider { get; init; }
            public EmbedAuthor? Author { get; init; }
            public List<EmbedField>? Fields { get; init; }

            public record EmbedFooter
            {
                public EmbedFooter(string text)
                {
                    Text = text;
                }

                public string Text { get; }
                public string? IconUrl { get; init; }
                public string? ProxyIconUrl { get; init; }
            }

            public record EmbedImage
            {
                public string? Url { get; init; }
                public string? ProxyUrl { get; init; }
                public int? Height { get; init; }
                public int? Width { get; init; }
            }

            public record EmbedThumbnail
            {
                public string? Url { get; init; }
                public string? ProxyUrl { get; init; }
                public int? Height { get; init; }
                public int? Width { get; init; }
            }

            public record EmbedVideo
            {
                public string? Url { get; init; }
                public string? ProxyUrl { get; init; }
                public int? Height { get; init; }
                public int? Width { get; init; }
            }

            public record EmbedProvider
            {
                public string? Name { get; init; }
                public string? Url { get; init; }
            }

            public record EmbedAuthor
            {
                public string? Name { get; init; }
                public string? Url { get; init; }
                public string? IconUrl { get; init; }
                public string? ProxyIconUrl { get; init; }
            }

            public record EmbedField
            {
                public EmbedField(string name, string value)
                {
                    Name = name;
                    Value = value;
                }

                public string Name { get; }
                public string Value { get; }
                public bool? Inline { get; init; }
            }
        }
    }
}
