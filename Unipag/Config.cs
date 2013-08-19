using System.Configuration;

namespace Unipag
{
    public static class Config
    {
        private static string _apiKey;

        public static string ApiKey
        {
            get { return string.IsNullOrEmpty(_apiKey) ? ConfigurationManager.AppSettings["UnipagApiKey"] : _apiKey; }
            set { _apiKey = value; }
        }

        public const string DefaultApiUrl = "https://api.unipag.com/v1/";

        private static string _apiUrl;

        public static string ApiUrl
        {
            get
            {
                var configUrl = ConfigurationManager.AppSettings["UnipagApiUrl"];
                var defaultUrl = string.IsNullOrEmpty(configUrl) ? DefaultApiUrl : configUrl;
                return string.IsNullOrEmpty(_apiUrl) ? defaultUrl : _apiUrl;
            }
            set { _apiUrl = value; }
        }

    }
}
