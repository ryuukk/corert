// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Collections.Generic;

namespace System.Reflection
{
    public readonly struct CustomAttributeTypedArgument
    {
        public CustomAttributeTypedArgument(object value)
        {
            // value cannot be null.
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Value = CanonicalizeValue(value);
            ArgumentType = value.GetType();
        }

        public CustomAttributeTypedArgument(Type argumentType, object value)
        {
            // value can be null.
            if (argumentType == null)
                throw new ArgumentNullException(nameof(argumentType));

            Value = (value == null) ? null : CanonicalizeValue(value);
            ArgumentType = argumentType;
        }

        public Type ArgumentType { get; }
        public object Value { get; }

        public override bool Equals(object obj) => obj == (object)this;
        public override int GetHashCode() => base.GetHashCode();
        public static bool operator ==(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right) => left.Equals(right);
        public static bool operator !=(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right) => !(left.Equals(right));

        public override string ToString() => ToString(typed: false);

        internal string ToString(bool typed)
        {
            if (ArgumentType == null)
                return base.ToString(); // Someone called ToString() on "default(CustomAttributeTypedArgument)"

            try
            {
                if (ArgumentType.IsEnum)
                    return string.Format(CultureInfo.CurrentCulture, typed ? "{0}" : "({1}){0}", Value, ArgumentType.FullNameOrDefault);

                else if (Value == null)
                    return string.Format(CultureInfo.CurrentCulture, typed ? "null" : "({0})null", ArgumentType.NameOrDefault);

                else if (ArgumentType == typeof(string))
                    return string.Format(CultureInfo.CurrentCulture, "\"{0}\"", Value);

                else if (ArgumentType == typeof(char))
                    return string.Format(CultureInfo.CurrentCulture, "'{0}'", Value);

                else if (ArgumentType == typeof(Type))
                    return string.Format(CultureInfo.CurrentCulture, "typeof({0})", ((Type)Value).FullNameOrDefault);

                else if (ArgumentType.IsArray)
                {
                    string result = null;
                    IList<CustomAttributeTypedArgument> array = Value as IList<CustomAttributeTypedArgument>;

                    Type elementType = ArgumentType.GetElementType();
                    result = string.Format(CultureInfo.CurrentCulture, @"new {0}[{1}] {{ ", elementType.IsEnum ? elementType.FullNameOrDefault : elementType.NameOrDefault, array.Count);

                    for (int i = 0; i < array.Count; i++)
                        result += string.Format(CultureInfo.CurrentCulture, i == 0 ? "{0}" : ", {0}", array[i].ToString(elementType != typeof(object)));

                    return result += " }";
                }

                return string.Format(CultureInfo.CurrentCulture, typed ? "{0}" : "({1}){0}", Value, ArgumentType.NameOrDefault);
            }
            catch (MissingMetadataException)
            {
                return base.ToString(); // Failsafe. Code inside "try" should still strive to avoid trigging a MissingMetadataException as caught exceptions are annoying when debugging.
            }
        }

        private static object CanonicalizeValue(object value)
        {
            if (value.GetType().IsEnum)
                return ((Enum)value).GetValue();
            return value;
        }
    }
}
