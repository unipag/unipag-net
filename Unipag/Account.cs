using System.Collections.Generic;

namespace Unipag
{
    public class Account : UObject
    {
        public Account() { }

        public Account(Account obj)
            : base(obj)
        {
        }

        public override string InstanceUrl
        {
            get { return "account"; }
        }

        #region Read-only properties

        public List<string> Currencies
        {
            get { return _list<string>("currencies"); }
        }

        public string Name
        {
            get { return _string("name"); }
        }

        public string PublicKey
        {
            get { return _string("public_key"); }
        }

        public string SecretKey
        {
            get { return _string("secret_key"); }
        }

        public bool? TestMode
        {
            get { return _nullableValue<bool>("test_mode"); }
        }

        #endregion

        #region Methods

        public static Account Get(string apiKey)
        {
            return BaseGet<Account>(null, apiKey);
        }

        public static Account Get()
        {
            return BaseGet<Account>(null, null);
        }

        public Account Reload()
        {
            BaseReload();
            return this;
        }

        #endregion
    }
}
