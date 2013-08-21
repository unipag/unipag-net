using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Unipag
{
    public class Payment : UObject
    {
        public Payment()
        {
            Properties["amount"] = Properties["amount_paid"] = Properties["amount_refunded"] = 0;
            Properties["cancelled"] = Properties["paid"] = Properties["refunded"] = false;
            Properties["params"] = new JObject();
            Properties["extra_info"] = new JObject();
        }

        public Payment(Payment obj)
            : base(obj)
        {
        }

        #region Read/Write properties

        public decimal Amount
        {
            get { return _value<decimal>("amount"); }
            set { Properties["amount"] = value; }
        }

        public string Currency
        {
            get { return _string("currency"); }
            set { Properties["currency"] = value; }
        }

        public string Connection
        {
            get { return _string("connection"); }
            set { Properties["connection"] = value; }
        }

        public string Invoice
        {
            get { return _string("invoice"); }
            set { Properties["invoice"] = value; }
        }

        public JObject Params
        {
            get { return (JObject)Properties["params"]; }
        }

        public string ReturnUrl
        {
            get { return _string("return_url"); }
            set { Properties["return_url"] = value; }
        }

        #endregion

        #region Read-only properties

        public string Account
        {
            get { return _string("account"); }
        }


        public List<string> AllowedActions
        {
            get { return _list<string>("allowed_actions"); }
        }

        public decimal AmountPaid
        {
            get { return _value<decimal>("amount_paid"); }
        }

        public decimal AmountRefunded
        {
            get { return _value<decimal>("amount_refunded"); }
        }


        public bool Cancelled
        {
            get { return _value<bool>("cancelled"); }
        }

        public DateTime? Created
        {
            get { return _nullableValue<DateTime>("created"); }
        }

        public DateTime? Modified
        {
            get { return _nullableValue<DateTime>("modified"); }
        }

        public bool Paid
        {
            get { return _value<bool>("paid"); }
        }

        public bool Refunded
        {
            get { return _value<bool>("refunded"); }
        }

        public string PaymentUrl
        {
            get { return _string("payment_url"); }
        }

        public JObject ProcessingInfo
        {
            get
            {
                // Planned API change: external_params will be renamed to processing_info

                // new
                if (Properties["external_params"] == null)
                    return (JObject)Properties["processing_info"];

                // old
                return (JObject)Properties["external_params"];
            }
        }

        public bool? TestMode
        {
            get { return _nullableValue<bool>("test_mode"); }
        }

        #endregion

        #region Methods

        public static Payment Get(string id, string apiKey)
        {
            return BaseGet<Payment>(id, apiKey);
        }

        public static Payment Get(string id)
        {
            return BaseGet<Payment>(id, null);
        }

        public static List<Payment> List(Dictionary<string, object> filters, string apiKey)
        {
            return BaseList<Payment>(filters, apiKey);
        }

        public static List<Payment> List(Dictionary<string, object> filters)
        {
            return BaseList<Payment>(filters, null);
        }

        public static List<Payment> List(string apiKey)
        {
            return BaseList<Payment>(null, apiKey);
        }

        public static List<Payment> List()
        {
            return BaseList<Payment>(null, null);
        }

        public static Payment Create(Payment payment, string apiKey)
        {
            return BaseCreate(payment, apiKey);
        }

        public static Payment Create(Payment payment)
        {
            return BaseCreate(payment, null);
        }

        public Payment Save()
        {
            BaseSave();
            return this;
        }

        public Payment Cancel()
        {
            BaseDelete();
            return this;
        }

        public static Payment Cancel(string id, string apiKey)
        {
            var payment = new Payment { Id = id, ApiKey = apiKey };
            payment.Cancel();
            return payment;
        }

        public static Payment Cancel(string id)
        {
            return Cancel(id, null);
        }

        public Payment Reload()
        {
            BaseReload();
            return this;
        }

        #endregion
    }
}
