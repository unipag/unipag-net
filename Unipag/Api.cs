using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Unipag
{
    public class Api
    {
        public static string Request(string method, string url, string parameters, string apiKey)
        {
            // Make request URL and request object
            var reqUrl = Config.ApiUrl + url;
            if (method.ToLower() == "get")
                reqUrl += string.Format("?{0}", parameters);
            var req = (HttpWebRequest)WebRequest.Create(reqUrl);
            req.Method = method;
            req.ContentType = "application/x-www-form-urlencoded";

            // Add auth
            var reqKey = string.IsNullOrEmpty(apiKey) ? Config.ApiKey : apiKey;
            var authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:", reqKey)));
            req.Headers.Add("Authorization", string.Format("Basic {0}", authBase64));

            // Add system information
            req.UserAgent = string.Format("Unipag Client for .Net v{0}", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);
            var sysInfo = new JObject();
            sysInfo["publisher"] = "Unipag";
            sysInfo["platform"] = Environment.OSVersion.ToString();
            sysInfo["language"] = string.Format("CLR {0}", Environment.Version.ToString());
            req.Headers.Add("X-Unipag-User-Agent-Info", sysInfo.ToString(Formatting.None));

            // Add parameters for requests other than GET
            if (method.ToLower() != "get")
            {
                byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                req.ContentLength = bytes.Length;
                using (Stream st = req.GetRequestStream())
                {
                    st.Write(bytes, 0, bytes.Length);
                }
            }

            // Send request and handle errors
            try
            {
                using (var response = req.GetResponse())
                {
                    return ReadStream(response.GetResponseStream());
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    var statusCode = ((HttpWebResponse)webException.Response).StatusCode;
                    var unipagError = JsonConvert<Error>.ConvertJsonToObject(ReadStream(webException.Response.GetResponseStream()), "error");

                    throw new UnipagExceptionFactory().Exception(unipagError, statusCode);
                }

                throw;
            }
        }

        private static string ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static string Request(string method, string url, Dictionary<string, object> parameters, string apiKey)
        {
            return Request(method, url, Utils.Urlify(parameters), apiKey);
        }

        public static string Request(string method, string url, JObject parameters, string apiKey)
        {
            return Request(method, url, Utils.Urlify(parameters), apiKey);
        }

        public static JObject RequestObject(string method, string url, Dictionary<string, object> parameters, string apiKey)
        {
            string response = Request(method, url, parameters, apiKey);
            var obj = JObject.Parse(response);
            return obj;
        }

        public static JObject RequestObject(string method, string url, Dictionary<string, object> parameters)
        {
            return RequestObject(method, url, parameters, Config.ApiKey);
        }

        public static JObject RequestObject(string method, string url, JObject parameters, string apiKey)
        {
            string response = Request(method, url, parameters, apiKey);
            var obj = JObject.Parse(response);
            return obj;
        }

        public static JObject RequestObject(string method, string url, JObject parameters)
        {
            return RequestObject(method, url, parameters, Config.ApiKey);
        }

        public static JObject RequestObject(string method, string url, string apiKey)
        {
            return RequestObject(method, url, new Dictionary<string, object>(), apiKey);
        }

        public static JObject RequestObject(string method, string url)
        {
            return RequestObject(method, url, new Dictionary<string, object>(), Config.ApiKey);
        }

        public static JArray RequestArray(string method, string url, Dictionary<string, object> parameters, string apiKey)
        {
            string response = Request(method, url, parameters, apiKey);
            var obj = JArray.Parse(response);
            return obj;
        }

        public static JArray RequestArray(string method, string url, Dictionary<string, object> parameters)
        {
            return RequestArray(method, url, parameters, Config.ApiKey);
        }

        public static JArray RequestArray(string method, string url)
        {
            return RequestArray(method, url, new Dictionary<string, object>(), Config.ApiKey);
        }

        public static JArray RequestArray(string method, string url, string apiKey)
        {
            return RequestArray(method, url, new Dictionary<string, object>(), apiKey);
        }
    }
}
