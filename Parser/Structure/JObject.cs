using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Parser.Exceptions;
using Parser.Settings;
using Parser.Structure;

namespace JsonParser.Structure
{
    public class JObject : IJToken
    {
        private readonly Dictionary<string, IJToken> _children = new Dictionary<string, IJToken>();

        public TokenType Type() => TokenType.Object;

        public IJToken this[string key] => _children[key];

        public bool TryGetValue(string key, out IJToken output) => _children.TryGetValue(key, out output);

        public void Add(string key, IJToken value) => _children.Add(key, value);

        public JObject() {}
        
        public JObject(JsonParserSettings settings, object serializing) {
            foreach (MemberInfo member in serializing.GetType().GetMembers())
            {
                switch (member)
                {
                    case PropertyInfo property:
                        _children.Add(property.Name, Parser.Util.Serializer.Serialize(property.GetValue(serializing), settings));
                        break;
                    case FieldInfo field:
                        _children.Add(field.Name, Parser.Util.Serializer.Serialize(field.GetValue(serializing), settings));
                        break;
                }
            }
        }
        
        public T Convert<T>(JsonParserSettings settings)
        {
            if (typeof(T).IsPrimitive)
            {
                throw new JsonCastException(typeof(T), "Objects cannot be primitives");
            }

            throw new Exception("TODO deserialization");
        }

        public void ToString(StringBuilder builder, JsonParserSettings settings, int depth)
        {
            builder.Append("{").Append(settings.Newline);
            foreach (KeyValuePair<string, IJToken> pair in _children)
            {
                TokenUtils.Indent(builder, depth, settings.Tab);
                builder.Append('"').Append(pair.Key).Append("\":").Append(settings.BetweenMembers);
                pair.Value.ToString(builder, settings, depth + 1);
                builder.Append(",").Append(settings.Newline);
            }

            builder.Remove(builder.Length - 1 - settings.Newline.Length, 1);
            TokenUtils.Indent(builder, depth, settings.Tab);
            builder.Append("}").Append(settings.Newline);
        }
    }
}