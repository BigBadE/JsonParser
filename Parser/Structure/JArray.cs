using System.Text;
using Parser.Settings;

namespace Parser.Structure
{
    public class JArray : IJToken
    {
        private readonly IJToken[] _array;

        public TokenType Type() => TokenType.Array;

        public IJToken[] Convert() => _array;

        public JArray(IJToken[] array)
        {
            _array = array;
        }
        
        public void ToString(StringBuilder builder, JsonParserSettings settings, int depth)
        {
            builder.Append("[").Append(settings.Newline);
            foreach (IJToken value in _array)
            {
                TokenUtils.Indent(builder, depth, settings.Tab);
                value.ToString(builder, settings, depth + 1);
                builder.Append(",").Append(settings.Newline);
            }
            builder.Remove(builder.Length - 1 - settings.Newline.Length, 1);
            TokenUtils.Indent(builder, depth, settings.Tab);
            builder.Append("]").Append(settings.Newline);
        }
    }
}