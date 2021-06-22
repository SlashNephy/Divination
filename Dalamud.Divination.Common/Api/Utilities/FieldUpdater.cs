using System.Collections.Generic;
using System.Reflection;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Utilities
{
    public class FieldUpdater : IFieldUpdater
    {
        public object Object { get; }
        public FieldInfo Field { get; }

        private readonly IChatClient chatClient;

        public FieldUpdater(object obj, FieldInfo fieldInfo, IChatClient chatClient)
        {
            Object = obj;
            Field = fieldInfo;
            this.chatClient = chatClient;
        }

        public bool UpdateBoolField(string? value, bool previousValue)
        {
            if (value == null)
            {
                Field.SetValue(Object, !previousValue);
                PrintConfigValueSuccessLog(!previousValue);
                return true;
            }

            if (bool.TryParse(value, out var tmp))
            {
                Field.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(tmp);
                return true;
            }

            PrintConfigValueTypeError(value);
            return false;
        }

        public bool UpdateFloatField(string? value)
        {
            if (float.TryParse(value, out var tmp))
            {
                Field.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(tmp);
                return true;
            }

            PrintConfigValueTypeError(value);
            return false;
        }

        public bool UpdateIntField(string? value)
        {
            if (int.TryParse(value, out var tmp))
            {
                Field.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(tmp);
                return true;
            }

            PrintConfigValueTypeError(value);
            return false;
        }

        public bool UpdateUInt16Field(string? value)
        {
            if (ushort.TryParse(value, out var tmp))
            {
                Field.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(tmp);
                return true;
            }

            PrintConfigValueTypeError(value);
            return false;
        }

        public bool UpdateByteField(string? value)
        {
            if (byte.TryParse(value, out var tmp))
            {
                Field.SetValue(Object, tmp);
                PrintConfigValueSuccessLog(tmp);
                return true;
            }

            PrintConfigValueTypeError(value);
            return false;
        }

        public bool UpdateStringField(string? value)
        {
            Field.SetValue(Object, value ?? string.Empty);
            PrintConfigValueSuccessLog(value ?? string.Empty);
            return true;
        }

        private void PrintConfigValueSuccessLog(object? value)
        {
            chatClient.Print(new List<Game.Text.SeStringHandling.Payload>
            {
                new TextPayload("フィールド "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload(Field.Name),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(" の値を "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload($"{value ?? "null"}"),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(" に変更しました。")
            });
        }

        private void PrintConfigValueTypeError(object? value)
        {
            chatClient.PrintError(new List<Game.Text.SeStringHandling.Payload>
            {
                new TextPayload("指定された値 "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload($"{value ?? "null"}"),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(" はフィールド "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload(Field.Name),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(" の型 ("),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload(Field.FieldType.Name),
                EmphasisItalicPayload.ItalicsOff,
                new TextPayload(") に変換できませんでした。")
            });
        }
    }
}
