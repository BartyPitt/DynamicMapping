using System.Xml.Serialization;

namespace DynamicMapping.Serializers
{
    internal class XMLMapper : IMapper
    {
        /// <summary>
        /// Deseralizes the XML Inputstring into the type given 
        /// </summary>
        /// <param name="InputObject"></param>
        /// <param name="type"></param>
        /// <returns>A class of type "type" containing the data from the string.</returns>
        public object? Deserialize(string InputString, Type type)
        {
            XmlSerializer serializer = new(type);
            using StringReader reader = new(InputString);
            return serializer?.Deserialize(reader);
        }
        /// <summary>
        /// Converts a predefined object into a JSON representation.
        /// </summary>
        /// <param name="InputObject">The input object to be converted</param>
        /// <returns>the object returned in JSON form </returns>
        public string Serialize(object InputObject)
        {
            XmlSerializer serializer = new(InputObject.GetType());
            using StringWriter writer = new();
            serializer.Serialize(writer, InputObject);
            return writer.ToString();
        }
    }
}
