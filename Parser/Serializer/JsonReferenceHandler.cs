using System.Collections.Generic;

namespace JsonParser.Serializer
{
    public class JsonReferenceHandler
    {
        private readonly Dictionary<long, object> _references = new Dictionary<long, object>();
        private long _nextId;
        
        public long AddReference(object referencing)
        {
            _references.Add(_nextId, referencing);
            return _nextId++;
        }

        public object GetReference(long id)
        {
            return _references[id];
        }
    }
}