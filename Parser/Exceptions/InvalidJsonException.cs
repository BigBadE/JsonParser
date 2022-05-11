using System;
using System.Runtime.Serialization;

namespace Parser.Exceptions
{
    [Serializable]
    public class InvalidJsonException : Exception
    {
        public InvalidJsonException(string error, int line, int column) : base(line + ":" + column + " - " + error)
        {
        }

        protected InvalidJsonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}