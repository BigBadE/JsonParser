using System;
using System.Text;
using Parser.Settings;

namespace Parser.Structure
{
    public interface IJToken
    {
        TokenType Type();

        void ToString(StringBuilder builder, JsonParserSettings settings, int depth);
    }

    public static class TokenUtils
    {
        public static void Indent(StringBuilder builder, int depth, String indentation)
        {
            for (int i = 0; i < depth; i++)
            {
                builder.Append(indentation);
            }
        }
    }

    public enum TokenType
    {
        Object,
        Array,
        String,
        Number,
        Literal
    }
}