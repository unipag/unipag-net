using Newtonsoft.Json.Linq;

namespace Unipag
{
    public class UObjectFactory
    {
        public static UObject FromJObject(JObject obj)
        {
            if (obj == null)
                return null;
            if (obj["object"] == null || obj["id"] == null)
                return null;
            var objType = obj["object"].Value<string>();
            switch (objType)
            {
                case "account":
                    var account = new Account();
                    account.FromObject(obj);
                    return account;
                case "connection":
                    var connection = new Connection();
                    connection.FromObject(obj);
                    return connection;
                case "event":
                    var evt = new Event();
                    evt.FromObject(obj);
                    return evt;
                case "invoice":
                    var invoice = new Invoice();
                    invoice.FromObject(obj);
                    return invoice;
                case "payment":
                    var payment = new Payment();
                    payment.FromObject(obj);
                    return payment;
            }
            return null;
        }
    }
}
