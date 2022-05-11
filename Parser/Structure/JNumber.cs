using System.Text;
using Parser.Settings;

namespace Parser.Structure
{
    public class JNumber<T> : IJToken
    {
        private readonly T _number;

        public TokenType Type() => TokenType.Number;

        public T Convert() => _number;

        public JNumber(T number)
        {
            _number = number;
        }
        
        public void ToString(StringBuilder builder, JsonParserSettings settings, int depth)
        {
            builder.Append(_number);
        }
    }
}