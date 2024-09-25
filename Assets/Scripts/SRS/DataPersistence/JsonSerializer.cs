using Unity.Plastic.Newtonsoft.Json;

namespace SRS.DataPersistence
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        public string Serialize<T>(T objectData)
        {
            return JsonConvert.SerializeObject(objectData, Formatting.Indented);
        }
    }
}