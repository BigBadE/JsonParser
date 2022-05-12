using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JsonParser.Structure;
using Parser.Serializer;
using Parser.Settings;
using Parser.Structure;

namespace Parser.Serializing
{
    public class ValueSerializer : ISerializer
    {
        public bool Serializes(Type type) => type == null || type == typeof(bool);
        
        public IJToken Serialize(JsonParserSettings settings, object serializing) => new JLiteral((bool?) serializing);
    }

    public class StringSerializer : ISerializer
    {
        public bool Serializes(Type type) => type == typeof(string) || type == typeof(String);

        public IJToken Serialize(JsonParserSettings settings, object serializing) => new JString((string) serializing);
    }

    public class NumberSerializer : ISerializer
    {
        public bool Serializes(Type type) => Util.Serializer.IsNumber(type);

        public IJToken Serialize(JsonParserSettings settings, object serializing) => Util.Serializer.AsNumber(serializing);
    }

    public class ArraySerializer : ISerializer
    {
        public bool Serializes(Type type) => type.IsArray;

        public IJToken Serialize(JsonParserSettings settings, object serializing)
        {
            List<IJToken> tokens = new List<IJToken>();
            foreach (object value in (IEnumerable) serializing)
            {
                tokens.Add(Util.Serializer.Serialize(value, settings));
            }

            return new JArray(tokens.ToArray());
        }
    }

    public class ObjectSerializer : ISerializer
    {
        public bool Serializes(Type type) => true;

        public IJToken Serialize(JsonParserSettings settings, object serializing)
        {
            JObject jObject = new JObject();
            foreach (MemberInfo member in serializing.GetType().GetMembers())
            {
                switch (member)
                {
                    case PropertyInfo property:
                        jObject.Add(property.Name, Util.Serializer.Serialize(property.GetValue(serializing), settings));
                        break;
                    case FieldInfo field:
                        jObject.Add(field.Name, Util.Serializer.Serialize(field.GetValue(serializing), settings));
                        break;
                }
            }

            return jObject;
        }
    }
}