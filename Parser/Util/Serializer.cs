using System;
using System.Linq;
using Parser.Exceptions;
using Parser.Settings;
using Parser.Structure;

namespace Parser.Util
{
    public static class Serializer
    {
        public static IJToken Serialize(object value, JsonParserSettings settings)
        {
            return settings.Serializers.FirstOrDefault(serializer => serializer.Serializes(value?.GetType()))
                ?.Serialize(settings, value) ?? throw new JsonCastException(value.GetType(), "No serializer for type");
        }

        public static bool IsNumber(Type value)
        {
            return value == typeof(sbyte)
                   || value == typeof(byte)
                   || value == typeof(short)
                   || value == typeof(ushort)
                   || value == typeof(int)
                   || value == typeof(uint)
                   || value == typeof(long)
                   || value == typeof(ulong)
                   || value == typeof(float)
                   || value == typeof(double)
                   || value == typeof(decimal);
        }
        
        public static IJToken AsNumber(object value)
        {
            switch (value)
            {
                case sbyte found:
                    return new JNumber<sbyte>(found);
                case byte found:
                    return new JNumber<byte>(found);
                case short found:
                    return new JNumber<short>(found);
                case ushort found:
                    return new JNumber<ushort>(found);
                case int found:
                    return new JNumber<int>(found);
                case uint found:
                    return new JNumber<uint>(found);
                case long found:
                    return new JNumber<long>(found);
                case ulong found:
                    return new JNumber<ulong>(found);
                case float found:
                    return new JNumber<float>(found);
                case double found:
                    return new JNumber<double>(found);
                case decimal found:
                    return new JNumber<decimal>(found);
                default:
                    throw new JsonCastException(value.GetType(), "Isn't a number");
            }
        }
    }
}