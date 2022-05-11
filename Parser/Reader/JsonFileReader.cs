using System;
using System.IO;
using Parser.Exceptions;

namespace Parser.Reader
{
    public class JsonFileReader : JsonReader
    {
        private const int BufferSize = 2048 * 8;

        private readonly char[] _characters = new char[BufferSize];
        private readonly StreamReader _reader;
        
        public JsonFileReader(StreamReader reader)
        {
            _reader = reader;
            reader.ReadBlock(_characters, 0, BufferSize);
        }


        protected override char GetCharacter(int index) => _characters[index];
        
        
        protected override char ReadNext()
        {
            if (Index != _characters.Length) return _characters[Index++];

            //Give up on line index if it's too big
            if (LineIndex == 0)
            {
                LineIndex = BufferSize;
            }
            
            //Resize buffer, leaving the last line
            int leaving = Math.Max(2, BufferSize - LineIndex);
            for (int i = 0; i < leaving; i++)
            {
                _characters[i] = _characters[_characters.Length - leaving - i];
            }

            LineIndex = 0;
            _reader.ReadBlock(_characters, leaving, BufferSize - leaving);

            if (Index == _characters.Length)
            {
                throw new InvalidJsonException("Unexpected EOF", Line, LineIndex - Index);
            }
            return _characters[Index++];
        }
    }
}