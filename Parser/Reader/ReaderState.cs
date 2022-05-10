namespace JsonParser.Reader
{
    public enum ReaderState
    {
        StartObject,
        StartMember,
        MemberValue,
        StartArray,
        EndOfFile
    }
}