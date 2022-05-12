using System;
using JsonParser.Settings;
using Parser.Serializer;
using Parser.Serializing;

namespace Parser.Settings
{
    public class JsonParserSettings
    {
        public readonly ISerializer[] Serializers =
        {
            new ValueSerializer(), new NumberSerializer(), new StringSerializer(), new ArraySerializer(),
            new ObjectSerializer()
        };

        public readonly IJsonTypeBinder TypeBinder = new DefaultJsonTypeBinder();

        public readonly String Tab = "    ";

        public readonly String Newline = "\r\n";

        public readonly String BetweenMembers = " ";
    }
}