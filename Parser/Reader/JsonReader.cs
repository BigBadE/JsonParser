using System;
using System.Globalization;
using System.Text;
using Parser.Exceptions;
using Parser.Structure;

namespace Parser.Reader
{
    public abstract class JsonReader : IFileReader
    {
        protected int Index;
        protected int LineIndex;
        protected int Line;
        
        private ReaderState _state;

        private IJToken _token;

        public ReaderState State() => _state;

        public IJToken NextToken() => _token;

        /// <summary>
        /// Reads until the next token is hit
        /// </summary>
        public void Read()
        {
            while (true)
            {
                switch (_state)
                {
                    case ReaderState.End:
                    case ReaderState.Start:
                        switch (NextNonWhitespace())
                        {
                            case '{':
                                _state = ReaderState.Object;
                                return;
                            case '[':
                                _state = ReaderState.Array;
                                return;
                            default:
                                throw new InvalidCastException(
                                    "Illegal start character " + GetCharacter(Index - 1) + " at " + (Index - 1));
                        }
                    case ReaderState.Object:
                        _state = ReaderState.Key;
                        break;
                    case ReaderState.Key:
                        ParseKey();
                        break;
                    case ReaderState.Array:
                    case ReaderState.Value:
                        if (ParseValue())
                        {
                            continue;
                        }

                        return;
                    case ReaderState.Number:
                        Index--;
                        ParseNumber();
                        break;
                    case ReaderState.String:
                        ParseString(true);
                        break;
                    case ReaderState.StringUnescaped:
                        ParseString(false);
                        break;
                    case ReaderState.EndOfFile:
                        return;
                }
            }
        }

        protected abstract char GetCharacter(int index);
        
        protected abstract char ReadNext();

        private void ParseKey()
        {
            bool escaped = false;
            if (ReadNext() == '"')
            {
                escaped = true;
            }
            else
            {
                Index--;
            }
            ParseString(escaped);
        }
        
        private void ParseString(bool escapedString)
        {
            StringBuilder builder = new StringBuilder();
            char current = ReadNext();
            bool escaped = false;
            while (escaped || (escapedString ? current != '"' : 
                       CharUnicodeInfo.GetUnicodeCategory(current) == UnicodeCategory.SpaceSeparator))
            {
                if (escaped)
                {
                    builder.Append(GetEscapedCharacter(current));
                    escaped = false;
                }
                else if (current == '\\')
                {
                    escaped = true;
                }
                else
                {
                    builder.Append(current);
                }

                current = ReadNext();
            }

            _token = new JString(builder.ToString());
        }

        private string GetEscapedCharacter(char current)
        {
            switch (current)
            {
                case '"':
                    return "\u0022";
                case '\\':
                    return "\u005C";
                case '/':
                    return "\u002F";
                case 'b':
                    return "\u0008";
                case 'f':
                    return "\u000C";
                case 'n':
                    return "\u000A";
                case 'r':
                    return "\u000D";
                case 't':
                    return "\u0009";
                case 'u':
                    int unicode = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        unicode *= 16;
                        char found = ReadNext();
                        if (found > '9')
                        {
                            found -= '\u0009';
                        }

                        if (found < '0' || found > '@')
                        {
                            throw new InvalidJsonException(
                                "Invalid hex digit " + GetCharacter(Index - 1), Line, Index - LineIndex - 1);
                        }

                        unicode += found - '0';
                    }

                    return char.ConvertFromUtf32(unicode);
                default:
                    throw new InvalidJsonException(
                        "Unknown escape character " + current, Line, Index - LineIndex - 1);
            }
        }

        private void ParseNumber()
        {
            bool negative = NextNonWhitespace() == '-';
            Index -= 1;
            float value = 0;
            int decimalPart = -1;
            int exponent = -1;
            bool onExponent = false;
            while (true)
            {
                char found = NextNonWhitespace();
                switch (found)
                {
                    case '"':
                    case '}':
                        Index--;
                        if (negative)
                        {
                            value *= -1;
                        }

                        _token = new JNumber<float>(value * (float) Math.Pow(10, exponent));
                        return;
                    case '.':
                        decimalPart = 1;
                        break;
                    case 'e':
                        onExponent = true;
                        break;
                    default:
                        if (found < '0' || found > '9')
                        {
                            throw new InvalidJsonException(
                                "Invalid number part: " + found, Line, Index - LineIndex - 1);
                        }
                        
                        if (decimalPart > 0)
                        {
                            value += (found - '0') / (10f * decimalPart++);
                        }
                        else if (onExponent)
                        {
                            exponent *= 10;
                            exponent += found - '0';
                        }
                        else
                        {
                            value *= 10;
                            value += found - '0';
                        }

                        break;
                }
            }
        }

        private void CheckValue(string value, bool? literal)
        {
            char[] testing = value.ToLower().ToCharArray();
            foreach (char character in testing)
            {
                if (ReadNext() != character)
                {
                    throw new InvalidJsonException("Invalid value", Line, Index - LineIndex - 1);
                }
            }

            _token = new JLiteral(literal);
        }

        private bool ParseValue()
        {
            switch (NextNonWhitespace())
            {
                case 'f':
                case 'F':
                    Index -= 1;
                    CheckValue("false", false);
                    break;
                case 'n':
                case 'N':
                    Index -= 1;
                    CheckValue("null", null);
                    break;
                case 't':
                case 'T':
                    Index -= 1;
                    CheckValue("true", true);
                    break;
                case '"':
                    _state = ReaderState.String;
                    break;
                case '{':
                    _state = ReaderState.Object;
                    return false;
                case '[':
                    _state = ReaderState.Array;
                    break;
                case '}':
                case ']':
                    _state = ReaderState.End;
                    return false;
                default:
                    char found = GetCharacter(Index - 1);
                    if ((found < '0' || found > '9'))
                    {
                        _state = ReaderState.Number;
                    } else if (found != '.' && found != '-')
                    {
                        found = ReadNext();
                        if (found < '0' || found > '9')
                        {
                            _state = ReaderState.StringUnescaped;
                        }
                        else
                        {
                            _state = ReaderState.Number;
                        }
                        Index--;
                    }
                    else
                    {
                        _state = ReaderState.StringUnescaped;
                    }
                    Index--;
                    return true;
            }

            _state = NextNonWhitespace() == ',' ? ReaderState.Key : ReaderState.Start;
            return false;
        }

        private char NextNonWhitespace()
        {
            char character = ReadNext();
            while (true)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(character))
                {
                    case UnicodeCategory.LineSeparator:
                        Line++;
                        LineIndex = Index - 1;
                        break;
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ParagraphSeparator:
                        break;
                    default:
                        return character;
                }

                character = ReadNext();
            }
        }
    }
}