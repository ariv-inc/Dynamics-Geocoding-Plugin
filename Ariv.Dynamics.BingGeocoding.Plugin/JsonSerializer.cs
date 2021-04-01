using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Ariv.Dynamics.BingGeocoding.Plugin
{
    public static class JsonSerializer
    {
        public static T Deserialize<T>(string json)
            where T : class, new()
        {
            T instance = default(T);

            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                instance = serializer.ReadObject(memoryStream) as T;
            }

            return instance;
        }
    }
}
