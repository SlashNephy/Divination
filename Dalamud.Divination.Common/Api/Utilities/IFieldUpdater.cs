using System.Reflection;

namespace Dalamud.Divination.Common.Api.Utilities
{
    public interface IFieldUpdater
    {
        public object Object { get; }
        public FieldInfo Field { get; }

        public bool UpdateBoolField(string? value, bool previousValue);
        public bool UpdateFloatField(string? value);
        public bool UpdateIntField(string? value);
        public bool UpdateUInt16Field(string? value);
        public bool UpdateByteField(string? value);
        public bool UpdateStringField(string? value);
    }
}
