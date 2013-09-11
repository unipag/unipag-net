using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Unipag.Tests
{
    class UtilsTests
    {
        [Test]
        public void UrlifyFromDictionary()
        {
            var dict = new Dictionary<string, object>
            {
                {"key1", "value1"},
                {"key2", "42"},
                {"list", new List<string> {"with space", "®"}},
                {"dict", new Dictionary<string, object>
                {
                    {"1", 1},
                    {"a", 1},
                    {"dikt", new Dictionary<string, decimal>
                    {
                        {"zero", 0.0m},
                        {"random", 42.9000m},
                    }},
                }},
            };
            Assert.AreEqual(
                "key1=value1&key2=42&list__0=with+space&list__1=%c2%ae&dict__1=1&dict__a=1&dict__dikt__zero=0.0&dict__dikt__random=42.9000",
                Utils.Urlify(dict));
        }

        [Test]
        public void UrlifyFromJObject()
        {
            var dict = JObject.Parse(
                "{" +
                    "\"key1\": \"value1\", " +
                    "\"key2\": 42, " +
                    "\"list\": [\"with space\", \"®\"], " +
                    "\"dict\": {" +
                        "1: 1, " +
                        "\"a\": 1, " +
                        "\"dikt\": {" +
                            "\"zero\": 0.0, " +
                            "\"random\": 42.9000" +
                        "}" +
                    "}" +
                "}");
            Assert.AreEqual(
                "key1=value1&key2=42&list__0=with+space&list__1=%c2%ae&dict__1=1&dict__a=1&dict__dikt__zero=0&dict__dikt__random=42.9",
                Utils.Urlify(dict));
        }
    }
}
