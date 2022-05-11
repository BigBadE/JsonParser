using Parser.Exceptions;

namespace Parser.Reader
{
    public class JsonTextReader : JsonReader
    {
        private readonly string _text;

        public JsonTextReader(string text)
        {
            _text = text;
        }

        protected override char GetCharacter(int index) => _text[index];

        protected override char ReadNext()
        {
            
            if (Index == _text.Length)
            {
                throw new InvalidJsonException("Unexpected EOF", Line, LineIndex - Index);
            }
            return _text[Index++];
        }
    }
}