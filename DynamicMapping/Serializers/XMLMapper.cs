using System.Xml.Serialization;

namespace DynamicMapping.Serializers
{
    internal class XMLMapper : IMapper
    {
        public object? Deserialize(string InputString, Type type)
        {
            XmlSerializer serializer = new(type);
            using StringReader reader = new(InputString);
            return serializer?.Deserialize(reader);
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
