using System;

namespace JsonParser.Settings
{
    public interface IJsonTypeBinder
    {
        Type BindType(string type);
    }
    
    public class DefaultJsonTypeBinder : IJsonTypeBinder {
        public Type BindType(string type)
        {
            return Type.GetType(type);
        }
    }
}