using System.Text;
using Parser.Settings;

namespace Parser.Structure
{
    public class JString : IJToken
    {
        private readonly string _value;

        public TokenType Type() => TokenType.String;

        public string Convert() => _value;

        public JString(string value)
        {
            _value = value;
        }

        public void ToString(StringBuilder builder, JsonParserSettings settings, int depth)
        {
            builder.Append('"').Append(Escape(_value)).Append('"');
        }

        public static string Escape(string value)
        {
            StringBuilder output = new StringBuilder();
            foreach (char character in value)
            {
                switch (character)
                {
                    case '\u0022':
                        return "\\\"";
                    case '\u005C':
                        return "\\\\";
                    case '\u002F':
                        return "\\/";
                    case '\u0008':
                        return "\\b";
                    case '\u000C':
                        return "\\f";
                    case '\u000A':
                        return "\\n";
                    case '\u000D':
                        return "\\r";
                    case '\u0009':
                        return "\\t";
                    default:
                        output.Append(character);
                        break;
                }
            }

            return output.ToString();
        }
    }
}