using System;
using JsonParser.Settings;

namespace Parser.Settings
{
    public class JsonParserSettings
    {
        public readonly IJsonTypeBinder TypeBinder = new DefaultJsonTypeBinder();

        public readonly String Tab = "    ";

        public readonly String Newline = "\r\n";

        public readonly String BetweenMembers = " ";
    }
}