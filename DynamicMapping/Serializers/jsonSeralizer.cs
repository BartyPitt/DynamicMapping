using System.Text.Json;

namespace DynamicMapping.Serializers
{
    internal class jsonSeralizer : I_Seralizer
    {
        public jsonSeralizer() { }
        public T? Deseralize<T>(string InputObject)
        {
            return JsonSerializer.Deserialize<T>(InputObject);
        }
        public string Serialize(object InputObject)
        {
            return JsonSerializer.Serialize(InputObject);
        }
    }
}
