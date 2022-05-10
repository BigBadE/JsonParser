using JsonParser.Structure;

namespace JsonParser.Reader
{
    public interface IFileReader
    {
        ReaderState State();

        IJToken NextToken();
    }
}