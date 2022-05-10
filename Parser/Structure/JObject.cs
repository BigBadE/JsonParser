using System;
using System.Collections.Generic;
using JsonParser.Exceptions;
using JsonParser.Settings;

namespace JsonParser.Structure
{
    public class JObject : IJToken
    {
        private readonly Dictionary<string, IJToken> _children = new Dictionary<string, IJToken>();
        private readonly JsonParserSettings _settings;

        public JObject(JsonParserSettings settings)
        {
            _settings = settings;
        }
        
        public TokenType Type() => TokenType.Object;

        public IJToken this[string key] => _children[key];

        public bool TryGet(string key, out IJToken output) => _children.TryGetValue(key, out output);
        
        public T Convert<T>()
        {
            if (typeof(T).IsPrimitive)
            {
                throw new JsonCastException(typeof(T), "Objects cannot be primitives");
            }

            throw new Exception("TODO");
        }
    }
}