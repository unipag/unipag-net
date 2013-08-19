using System;
using Newtonsoft.Json.Linq;

namespace Unipag
{
    public class Event : UObject
    {
        public Event() { }

        public Event(Event obj)
            : base(obj)
        {
        }

        #region Read-only properties

        public string Account
        {
            get { return _string("account"); }
        }

        public DateTime? Created
        {
            get { return _nullableValue<DateTime>("created"); }
        }

        public UObject RelatedObject
        {
            get { return UObjectFactory.FromJObject((JObject)Properties["related_object"]); }
        }

        public UObject RelatedObjectPreviousState
        {
            get { return UObjectFactory.FromJObject((JObject)Properties["related_object_previous_state"]); }
        }

        public bool? TestMode
        {
            get { return _nullableValue<bool>("test_mode"); }
        }

        public string Type
        {
            get { return _string("type"); }
        }

        #endregion

        #region Methods

        public static Event Get(string id, string apiKey)
        {
            return BaseGet<Event>(id, apiKey);
        }

        public static Event Get(string id)
        {
            return BaseGet<Event>(id, null);
        }

        public Event Reload()
        {
            BaseReload();
            return this;
        }

        #endregion
    }
}
