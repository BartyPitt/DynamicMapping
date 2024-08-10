using System.Text.Json;

namespace DynamicMapping.Serializers
{
    internal class JsonMapper : IMapper
    {
        public JsonMapper() { }
        public T? Deserialize<T>(string InputObject)
        {
            return JsonSerializer.Deserialize<T>(InputObject);
        }
        public string Serialize(object InputObject)
        {
            return JsonSerializer.Serialize(InputObject);
        }
    }
}
