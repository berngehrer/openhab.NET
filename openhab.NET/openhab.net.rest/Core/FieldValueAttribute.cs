using System;

namespace openhab.net.rest.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal class FieldValueAttribute : Attribute
    {
        public FieldValueAttribute(object value) {
            Value = value;
        }
        public object Value { get; }
        public override string ToString() => Value?.ToString();
    }
}
