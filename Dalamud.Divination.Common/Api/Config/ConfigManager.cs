using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Utilities;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api.Config
{
    internal partial class ConfigManager<TConfiguration> : ICommandProvider, IConfigManager<TConfiguration> where TConfiguration : class, IPluginConfiguration, new()
    {
        private readonly DalamudPluginInterface @interface;
        private readonly IChatClient chatClient;
        private readonly ICommandProcessor commandProcessor;

        private TConfiguration? config;
        private readonly object configLock = new();
        public TConfiguration Config => config ?? throw new InvalidOperationException("Config はまだ初期化されていません。");

        public ConfigManager(DalamudPluginInterface @interface, IChatClient chatClient, ICommandProcessor commandProcessor)
        {
            this.@interface = @interface;
            this.chatClient = chatClient;
            this.commandProcessor = commandProcessor;
        }

        public void LoadConfig()
        {
            lock (configLock)
            {
                config = @interface.GetPluginConfig() as TConfiguration ?? new TConfiguration();
            }
        }

        private static IEnumerable<FieldInfo> EnumerateConfigFields(bool includeUpdateIgnore = false)
        {
            var result = typeof(TConfiguration)
                .GetFields()
                .Where(x => x.FieldType == typeof(float) || x.FieldType == typeof(bool) || x.FieldType == typeof(int) || x.FieldType == typeof(string));

            return !includeUpdateIgnore ? result : result.Where(x => x.GetCustomAttribute<ConfigUpdateProhibitedAttribute>() == null);
        }

        public bool TryUpdate(string key, string? value)
        {
            var fieldInfo = EnumerateConfigFields(true).FirstOrDefault(x => x.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            if (fieldInfo == null)
            {
                chatClient.PrintError(new List<Payload>
                {
                    new TextPayload("指定されたフィールド名 "),
                    EmphasisItalicPayload.ItalicsOn,
                    new TextPayload(key),
                    EmphasisItalicPayload.ItalicsOff,
                    new TextPayload(" は存在しません。")
                });

                return false;
            }

            if (fieldInfo.GetCustomAttribute<ConfigUpdateProhibitedAttribute>() != null)
            {
                chatClient.PrintError(new List<Payload>
                {
                    new TextPayload("指定されたフィールド名 "),
                    EmphasisItalicPayload.ItalicsOn,
                    new TextPayload(key),
                    EmphasisItalicPayload.ItalicsOff,
                    new TextPayload(" の変更は許可されていません。")
                });

                return false;
            }

            var fieldValue = fieldInfo.GetValue(Config);
            var updater = new FieldUpdater(Config, fieldInfo, chatClient);
            switch (fieldValue)
            {
                case bool b:
                    return updater.UpdateBoolField(value, b);
                case byte:
                    return updater.UpdateByteField(value);
                case int:
                    return updater.UpdateIntField(value);
                case float:
                    return updater.UpdateFloatField(value);
                case ushort:
                    return updater.UpdateUInt16Field(value);
                case string:
                    return updater.UpdateStringField(value);
                default:
                    chatClient.PrintError(new List<Payload>
                    {
                        new TextPayload("指定されたフィールド名 "),
                        EmphasisItalicPayload.ItalicsOn,
                        new TextPayload(key),
                        EmphasisItalicPayload.ItalicsOff,
                        new TextPayload(" の変更はサポートされていません。")
                    });

                    return false;
            }
        }

        public void SaveConfig()
        {
            lock (configLock)
            {
                @interface.SavePluginConfig(config);
            }
        }

        public void Dispose()
        {
            SaveConfig();
        }
    }
}
