using System.Text.Json;

namespace DynamicMapping.Serializers
{
    internal class JsonMapper : IMapper
    {
        public JsonMapper() { }
        public object? Deserialize(string InputObject , Type type)
        {
            return JsonSerializer.Deserialize(InputObject , type);
        }
        public string Serialize(object InputObject)
        {
            return JsonSerializer.Serialize(InputObject);
        }
    }
}
