using System.Web;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace Unipag
{
    public class Utils
    {
        public static string Urlify(string key, object value, string prefix)
        {
            var pfx = string.IsNullOrEmpty(prefix) ? "" : string.Format("{0}__", prefix);

            if (value is JObject)
            {
                var dict = new Dictionary<string, object>();
                foreach (var item in (value as JObject).Children())
                {
                    if (item is JProperty)
                    {
                        var prop = item as JProperty;
                        dict.Add((string)prop.Name, prop.Value);
                    }
                }
                return Urlify(dict, string.Format("{0}{1}", pfx, key));
            }

            if (value is IDictionary)
            {
                var dict = new Dictionary<string, object>();
                foreach (DictionaryEntry item in value as IDictionary)
                {
                    dict.Add((string)item.Key, item.Value);
                }
                return Urlify(dict, string.Format("{0}{1}", pfx, key));
            }

            if (value is IList)
            {
                var list = new List<object>();
                foreach (var item in value as IList)
                {
                    list.Add(item);
                }
                return Urlify(list, string.Format("{0}{1}", pfx, key));
            }

            return string.Format("{0}{1}={2}",
                pfx,
                HttpUtility.UrlEncode(key),
                HttpUtility.UrlEncode(value.ToString()));
        }

        public static string Urlify(Dictionary<string, object> parameters, string prefix)
        {
            if (parameters == null)
                return "";
            var pairs = new List<string>();
            foreach (var p in parameters)
            {
                pairs.Add(Urlify(p.Key, p.Value, prefix));
            }
            return string.Join("&", pairs.ToArray());
        }

        public static string Urlify(List<object> parameters, string prefix)
        {
            var pairs = new List<string>();
            for (var i = 0; i < parameters.Count; i++)
            {
                pairs.Add(Urlify(i.ToString(), parameters[i], prefix));
            }
            return string.Join("&", pairs.ToArray());
        }

        public static string Urlify(Dictionary<string, object> parameters)
        {
            return Urlify(parameters, null);
        }

        public static string Urlify(JObject parameters, string prefix)
        {
            var pairs = new List<string>();
            foreach (var item in parameters.Children())
            {
                if (item is JProperty)
                {
                    var prop = item as JProperty;
                    pairs.Add(Urlify(prop.Name, prop.Value, prefix));
                }
            }
            return string.Join("&", pairs.ToArray());
        }

        public static string Urlify(JObject parameters)
        {
            return Urlify(parameters, null);
        }
    }
}
