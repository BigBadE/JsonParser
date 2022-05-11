using JsonParser.Structure;
using Parser.Structure;

namespace Parser.Reader
{
    public interface IFileReader
    {
        ReaderState State();

        IJToken NextToken();

        void Read();
    }
}