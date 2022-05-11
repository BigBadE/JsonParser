using System;
using System.Runtime.Serialization;

namespace Parser.Exceptions
{
    [Serializable]
    public class JsonCastException : Exception
    {
        public JsonCastException(Type target, string error) : base("Invalid cast to " + target + ": " + error)
        {
        }

        protected JsonCastException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
    }
}