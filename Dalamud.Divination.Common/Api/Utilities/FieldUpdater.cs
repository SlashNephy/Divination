using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Voiceroid2Proxy;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Utilities
{
    internal class FieldUpdater : IFieldUpdater, IDisposable
    {
        public object Object { get; }

        private readonly IChatClient chatClient;
        private readonly Lazy<IVoiceroid2ProxyClient> v2PClient = new(() => new Voiceroid2ProxyClient());
        private readonly bool useTts;

        public FieldUpdater(object obj, IChatClient chatClient, bool useTts)
        {
            Object = obj;
            this.chatClient = chatClient;
            this.useTts = useTts;
        }

        private void Respond(List<Payload> payloads)
        {
            if (useTts)
            {
                var text = new SeString(payloads).TextValue;
                v2PClient.Value.TalkAsync(text);
            }
            else
            {
                chatClient.Print(payloads);
            }
        }

        private void RespondError(List<Payload> payloads)
        {
            if (useTts)
            {
                Respond(payloads);
            }
            else
            {
                chatClient.PrintError(payloads);
            }
        }

        public bool TryUpdate(string key, string? value, IEnumerable<FieldInfo> fields)
        {
            var fieldInfo = fields.FirstOrDefault(x => x.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            if (fieldInfo == null)
            {
                RespondError(new List<Payload>
                {
                    new TextPayload("指定されたフィールド名 "),
                    EmphasisItalicPayload.ItalicsOn,
                    new TextPayload(key),
                    EmphasisItalicPayload.ItalicsOff,
                    new TextPayload(" は存在しません。")
                });

                return false;
            }

            if (fieldInfo.GetCustomAttribute<UpdateProhibitedAttribute>() != null)
            {
                RespondError(new List<Payload>
                {
                    new TextPayload("指定されたフィールド名 "),
                    EmphasisItalicPayload.ItalicsOn,
                    new TextPayload(key),
                    EmphasisItalicPayload.ItalicsOff,
                    new TextPayload(" の変更は許可されていません。")
                });

                return false;
            }

            var fieldValue = fieldInfo.GetValue(Object);
            switch (fieldValue)
            {
                case bool b:
                    return UpdateBoolField(fieldInfo, value, b);
                case byte:
                    return UpdateByteField(fieldInfo, value);
                case int:
                    return UpdateIntField(fieldInfo, value);
                case float:
                    return UpdateFloatField(fieldInfo, value);
                case ushort:
                    return UpdateUInt16Field(fieldInfo, value);
                case string:
                    return UpdateStringField(fieldInfo, value);
                default:
                    RespondError(new List<Payload>
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

        public bool UpdateBoolField(FieldInfo fieldInfo, string? value, bool previousValue)
        {
            if (value == null)
            {
                fieldInfo.SetValue(Object, !previousValue);
                PrintConfigValueSuccessLog(fieldInfo, !previousValue);
                return true;
            }

            if (bool.TryParse(value, out var tmp))
            {
                fieldInfo.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(fieldInfo, tmp);
                return true;
            }

            PrintConfigValueTypeError(fieldInfo, value);
            return false;
        }

        public bool UpdateFloatField(FieldInfo fieldInfo, string? value)
        {
            if (float.TryParse(value, out var tmp))
            {
                fieldInfo.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(fieldInfo, tmp);
                return true;
            }

            PrintConfigValueTypeError(fieldInfo, value);
            return false;
        }

        public bool UpdateIntField(FieldInfo fieldInfo, string? value)
        {
            if (int.TryParse(value, out var tmp))
            {
                fieldInfo.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(fieldInfo, tmp);
                return true;
            }

            PrintConfigValueTypeError(fieldInfo, value);
            return false;
        }

        public bool UpdateUInt16Field(FieldInfo fieldInfo, string? value)
        {
            if (ushort.TryParse(value, out var tmp))
            {
                fieldInfo.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(fieldInfo, tmp);
                return true;
            }

            PrintConfigValueTypeError(fieldInfo, value);
            return false;
        }

        public bool UpdateByteField(FieldInfo fieldInfo, string? value)
        {
            if (byte.TryParse(value, out var tmp))
            {
                fieldInfo.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(fieldInfo, tmp);
                return true;
            }

            PrintConfigValueTypeError(fieldInfo, value);
            return false;
        }

        public bool UpdateStringField(FieldInfo fieldInfo, string? value)
        {
            fieldInfo.SetValue(Object, value ?? string.Empty);
            PrintConfigValueSuccessLog(fieldInfo, value ?? string.Empty);
            return true;
        }

        private void PrintConfigValueSuccessLog(FieldInfo fieldInfo, object? value)
        {
            Respond(new List<Payload>
            {
                new TextPayload("フィールド "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload(fieldInfo.Name),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(" の値を "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload($"{value ?? "null"}"),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(" に変更しました。")
            });
        }

        private void PrintConfigValueTypeError(FieldInfo fieldInfo, object? value)
        {
            RespondError(new List<Payload>
            {
                new TextPayload("指定された値 "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload($"{value ?? "null"}"),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(" はフィールド "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload(fieldInfo.Name),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(" の型 ("),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload(fieldInfo.FieldType.Name),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(") に変換できませんでした。")
            });
        }

        public void Dispose()
        {
            if (v2PClient.IsValueCreated)
            {
                v2PClient.Value.Dispose();
            }
        }
    }
}
