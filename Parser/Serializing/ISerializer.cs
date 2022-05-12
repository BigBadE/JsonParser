using System;
using Parser.Settings;
using Parser.Structure;

namespace Parser.Serializer
{
    public interface ISerializer
    {
        bool Serializes(Type type);

        IJToken Serialize(JsonParserSettings settings, object serializing);
    }
}