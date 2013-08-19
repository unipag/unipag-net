using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Unipag
{
    public class Connection : UObject
    {
        public Connection() { }

        public Connection(Connection obj)
            : base(obj)
        {
            Properties["configured"] = Properties["enabled"] = false;
        }

        #region Read-only properties

        public string Account
        {
            get { return _string("account"); }
        }

        public bool Configured
        {
            get { return _value<bool>("configured"); }
        }

        public List<string> CurrenciesSupported
        {
            get { return _list<string>("currencies_supported"); }
        }

        public bool Enabled
        {
            get { return _value<bool>("enabled"); }
        }

        public string PaymentGateway
        {
            get
            {
                // Planned API change: upcoming version will return just a string with gateway name.
                // Previuos version returned a dictionary with gateway name and "read_only" flag.
                var pg = Properties["payment_gateway"];

                // old
                if (pg is JObject)
                    return pg["name"] == null ? (string)null : pg["name"].Value<string>();

                // new
                return _string("payment_gateway");
            }
        }

        public string Protocol
        {
            get { return _string("protocol"); }
        }

        public bool? TestMode
        {
            get { return _nullableValue<bool>("test_mode"); }
        }

        public string SharedFrom
        {
            get { return _string("shared_from"); }
        }

        #endregion

        #region Methods

        public static List<Connection> List(Dictionary<string, object> filters, string apiKey)
        {
            return BaseList<Connection>(filters, apiKey);
        }

        public static List<Connection> List(Dictionary<string, object> filters)
        {
            return List(filters, null);
        }

        public static Connection Get(string id, string apiKey)
        {
            return BaseGet<Connection>(id, apiKey);
        }

        public static Connection Get(string id)
        {
            return BaseGet<Connection>(id, null);
        }

        public Connection Reload()
        {
            BaseReload();
            return this;
        }

        #endregion
    }
}
