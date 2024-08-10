using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DynamicMapping.Serializers
{
    internal class XMLMapper : IMapper
    {
        public T? Deserialize<T>(string InputString)
        {
            XmlSerializer serializer = new(typeof(T));
            using StringReader reader = new(InputString);
            return (T?)serializer?.Deserialize(reader);
        }
        public string Serialize(object InputObject)
        { 
           XmlSerializer serializer = new(InputObject.GetType());
            using StringWriter writer = new();
            serializer.Serialize(writer, InputObject);
            return writer.ToString();
        }
    }
}
