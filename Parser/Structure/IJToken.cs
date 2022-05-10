namespace JsonParser.Structure
{
    public interface IJToken
    {
        TokenType Type();

        T Convert<T>();
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