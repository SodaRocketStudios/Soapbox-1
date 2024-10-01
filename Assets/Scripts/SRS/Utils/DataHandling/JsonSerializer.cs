using Unity.Plastic.Newtonsoft.Json;

namespace SRS.Utils.DataHandling
{
    public class JsonSerializer : ISerializer
    {
        public object Deserialize(string serializedObject)
        {
            return JsonConvert.DeserializeObject(serializedObject);
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
    }
}