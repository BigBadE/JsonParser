using JsonParser.Structure;
using Parser.Exceptions;
using Parser.Structure;

namespace Parser.Reader
{
    public interface IFileReader
    {
        ReaderState State();

        IJToken NextToken();

        void Read();

        InvalidJsonException CreateException(string reason, int offset);
    }
}