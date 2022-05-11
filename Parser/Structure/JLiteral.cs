using System.Text;
using Parser.Settings;

namespace Parser.Structure
{
    public class JLiteral : IJToken
    {
        private readonly bool? _literal;

        public TokenType Type() => TokenType.Literal;

        public bool? Convert() => _literal;

        public JLiteral(bool? literal)
        {
            _literal = literal;
        }
        
        public void ToString(StringBuilder builder, JsonParserSettings settings, int depth)
        {
            builder.Append(_literal?.ToString() ?? "null");
        }
    }
}