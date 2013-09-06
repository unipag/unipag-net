using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Unipag
{
    public class Invoice : UObject
    {
        public Invoice()
        {
            Properties["amount"] = Properties["amount_paid"] = 0;
            Properties["deleted"] = false;
            Properties["custom_data"] = new JObject();
        }

        public Invoice(Invoice obj)
            : base(obj)
        {
        }

        #region Read/Write properties

        public decimal Amount
        {
            get { return _value<decimal>("amount"); }
            set { Properties["amount"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        public string Currency
        {
            get { return _string("currency"); }
            set { Properties["currency"] = value; }
        }


        public string Customer
        {
            get { return _string("customer"); }
            set { Properties["customer"] = value; }
        }

        public string Description
        {
            get { return _string("description"); }
            set { Properties["description"] = value; }
        }

        public bool Deleted
        {
            get { return _value<bool>("deleted"); }
            set { Properties["deleted"] = value; }
        }

        public DateTime? Expires
        {
            get { return _nullableValue<DateTime>("expires"); }
            set
            {
                if (value == null)
                    Properties["expires"] = null;
                else
                    Properties["expires"] = ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
        }

        public string Reference
        {
            get { return _string("reference"); }
            set { Properties["reference"] = value; }
        }

        public JToken CustomData
        {
            get { return Properties["custom_data"]; }
            set { Properties["custom_data"] = value;  }
        }

        #endregion

        #region Read-only properties

        public string Account
        {
            get { return _string("account"); }
        }

        public decimal AmountPaid
        {
            get { return _value<decimal>("amount_paid"); }
        }

        public DateTime? Created
        {
            get { return _nullableValue<DateTime>("created"); }
        }

        public DateTime? Modified
        {
            get { return _nullableValue<DateTime>("modified"); }
        }

        public bool? TestMode
        {
            get { return _nullableValue<bool>("test_mode"); }
        }

        #endregion

        #region Methods

        public static Invoice Get(string id, string apiKey)
        {
            return BaseGet<Invoice>(id, apiKey);
        }

        public static Invoice Get(string id)
        {
            return BaseGet<Invoice>(id, null);
        }

        public static List<Invoice> List(Dictionary<string, object> filters, string apiKey)
        {
            return BaseList<Invoice>(filters, apiKey);
        }

        public static List<Invoice> List(Dictionary<string, object> filters)
        {
            return BaseList<Invoice>(filters, null);
        }

        public static List<Invoice> List(string apiKey)
        {
            return BaseList<Invoice>(null, apiKey);
        }

        public static List<Invoice> List()
        {
            return BaseList<Invoice>(null, null);
        }

        public static Invoice Create(Invoice invoice)
        {
            return BaseCreate(invoice, null);
        }

        public static Invoice Create(Invoice invoice, string apiKey)
        {
            return BaseCreate(invoice, apiKey);
        }

        public Invoice Save()
        {
            BaseSave();
            return this;
        }

        public Invoice Reload()
        {
            BaseReload();
            return this;
        }

        public Invoice Delete()
        {
            BaseDelete();
            return this;
        }

        public static Invoice Delete(string id, string apiKey)
        {
            var inv = new Invoice { Id = id, ApiKey = apiKey };
            inv.Delete();
            return inv;
        }

        public static Invoice Delete(string id)
        {
            return Delete(id, null);
        }

        public Invoice Undelete()
        {
            Properties["deleted"] = false;
            BaseSave();
            return this;
        }

        #endregion

    }
}
