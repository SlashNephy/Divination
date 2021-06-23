using System.Collections.Generic;
using System.Reflection;

namespace Dalamud.Divination.Common.Api.Utilities
{
    public interface IFieldUpdater
    {
        public object Object { get; }

        public bool TryUpdate(string key, string? value, IEnumerable<FieldInfo> fields);
        public bool UpdateBoolField(FieldInfo fieldInfo, string? value, bool previousValue);
        public bool UpdateFloatField(FieldInfo fieldInfo, string? value);
        public bool UpdateIntField(FieldInfo fieldInfo, string? value);
        public bool UpdateUInt16Field(FieldInfo fieldInfo, string? value);
        public bool UpdateByteField(FieldInfo fieldInfo, string? value);
        public bool UpdateStringField(FieldInfo fieldInfo, string? value);
    }
}
