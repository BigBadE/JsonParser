using System;
using System.Collections.Generic;
using JsonParser.Structure;
using Parser.Exceptions;
using Parser.Reader;
using Parser.Structure;

namespace Parser
{
    public class JsonParser
    {
        public IJToken Parse(IFileReader reader)
        {
            reader.Read();
            if (reader.State() == ReaderState.Object)
            {
                return ReadObject(reader);
            }

            if (reader.State() == ReaderState.Array)
            {
                return ReadArray(reader);
            }

            throw new InvalidJsonException("Reader had invalid top element state " + reader.State(), -1, -1);
        }

        private JObject ReadObject(IFileReader reader)
        {
            JObject jObject = new JObject();

            string key = null;
            while (true)
            {
                reader.Read();
                switch (reader.State())
                {
                    case ReaderState.Key:
                        reader.Read();
                        key = ((JString) reader.NextToken()).Convert();
                        break;
                    case ReaderState.Object:
                        ReadObject(reader);
                        jObject.Add(key, reader.NextToken());
                        break;
                    case ReaderState.Array:
                        ReadArray(reader);
                        jObject.Add(key, reader.NextToken());
                        break;
                    case ReaderState.Value:
                    case ReaderState.String:
                    case ReaderState.Number:
                        reader.Read();
                        jObject.Add(key, reader.NextToken());
                        break;
                    case ReaderState.End:
                    case ReaderState.EndOfFile:
                        return jObject;
                    default:
                        throw new ArgumentException("Reader stopped at invalid state " + reader.State());
                }
            }
        }

        private JArray ReadArray(IFileReader reader)
        {
            List<IJToken> array = new List<IJToken>();

            while (reader.State() != ReaderState.End)
            {
                reader.Read();
                switch (reader.State())
                {
                    case ReaderState.Object:
                        ReadObject(reader);
                        array.Add(reader.NextToken());
                        break;
                    case ReaderState.Array:
                        ReadArray(reader);
                        array.Add(reader.NextToken());
                        break;
                    case ReaderState.Value:
                    case ReaderState.String:
                    case ReaderState.Number:
                        array.Add(reader.NextToken());
                        break;
                    case ReaderState.End:
                    case ReaderState.EndOfFile:
                        return new JArray(array.ToArray());
                    default:
                        throw new ArgumentException("Reader stopped at invalid state " + reader.State());
                }
            }

            return new JArray(array.ToArray());
        }
    }
}