using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Unipag.Tests
{
    public class ApiTests
    {
        [Test]
        [ExpectedException(typeof(UnauthorizedException))]
        public void BadApiKey()
        {
            Api.RequestArray("GET", "invoices", "i-am-bad-api-key-that-will-never-work");
        }

        [Test]
        public void UpcomingApiChanges()
        {
            // Change 1: connection.payment_gateway parameter will be converted to string 
            // with gateway name, instead of dictionary with gateway_name and read_only flag.
            var conn = new Connection();
            // Try old version first
            conn.FromString(
                "{ " +
                    "\"object\": \"connection\", " +
                    "\"id\": \"42\", " +
                    "\"payment_gateway\": { " +
                        "\"name\": \"old version\", " +
                        "\"read_only\": false " +
                    "} " +
                "}");
            Assert.AreEqual(conn.PaymentGateway, "old version");
            // Then new version
            conn.FromString(
                "{ " +
                    "\"object\": \"connection\", " +
                    "\"id\": \"42\", " +
                    "\"payment_gateway\": \"new version\" " +
                "}");
            conn.Properties["payment_gateway"] = "new version";
            Assert.AreEqual(conn.PaymentGateway, "new version");

            // Change 2: payment.external_params will be renamed to processing.info
            var payment = new Payment();
            // Try old version first
            payment.FromObject(JObject.Parse(
                "{ " +
                    "\"object\": \"payment\", " +
                    "\"id\": \"42\", " +
                    "\"external_params\": { " +
                        "\"token\": 42, " +
                    "} " +
                "}"));
            Assert.AreEqual(42, payment.ProcessingInfo["token"].Value<int>());
            // Then new version
            payment.FromObject(JObject.Parse(
                "{ " +
                    "\"object\": \"payment\", " +
                    "\"id\": \"42\", " +
                    "\"processing_info\": { " +
                        "\"token\": 9000, " +
                    "} " +
                "}"));
            Assert.AreEqual(9000, payment.ProcessingInfo["token"].Value<int>());
        }
    }
}
