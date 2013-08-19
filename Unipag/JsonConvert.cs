using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Unipag
{
    public static class JsonConvert<T>
    {
        public static T ConvertJsonToObject(string json, string parentToken = null)
        {
            var jsonToParse = string.IsNullOrEmpty(parentToken) ? json : JObject.Parse(json).SelectToken(parentToken).ToString();

            return JsonConvert.DeserializeObject<T>(jsonToParse);
        }
    }
}
