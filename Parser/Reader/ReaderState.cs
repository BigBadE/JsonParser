namespace Parser.Reader
{
    public enum ReaderState
    {
        Start,
        Object,
        Array,
        String,
        StringUnescaped,
        Number,
        Value,
        End,
        Key,
        Next,
        EndOfFile
    }
}