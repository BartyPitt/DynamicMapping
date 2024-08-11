using System.Text.Json;

namespace DynamicMapping.Serializers
{
    internal class JsonMapper : IMapper
    {
        public JsonMapper() { }
        /// <summary>
        /// Deseralizes the JSON Inputstring into the type given 
        /// </summary>
        /// <param name="InputObject"></param>
        /// <param name="type"></param>
        /// <returns>A class of type "type" containing the data from the string.</returns>
        public object? Deserialize(string InputObject , Type type)
        {
            return JsonSerializer.Deserialize(InputObject , type);
        }
        /// <summary>
        /// Converts a predefined object into a JSON representation.
        /// </summary>
        /// <param name="InputObject">The input object to be converted</param>
        /// <returns>the object returned in JSON form </returns>
        public string Serialize(object InputObject)
        {
            return JsonSerializer.Serialize(InputObject);
        }
    }
}
