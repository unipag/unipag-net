using System.Collections.Generic;
using NUnit.Framework;

namespace Unipag.Tests
{
    class EventTests
    {
        [Test]
        public void EventList()
        {
            var ev = new Event();
            Assert.Null(ev.Account);
            Assert.Null(ev.Created);
            Assert.Null(ev.RelatedObject);
            Assert.Null(ev.RelatedObjectPreviousState);
            Assert.Null(ev.TestMode);
            Assert.Null(ev.Type);

            // Generate 3 events for some invoice - created, changed, deleted
            var inv = Invoice.Create(new Invoice
            {
                Amount = 13,
                Currency = "RUB"
            });
            inv.Amount = 14;
            inv.Save();
            inv.Delete();

            var events = Event.List(new Dictionary<string, object>
            {
                {"invoice", inv.Id},
                {"type", "invoice.deleted"},
            });
            Assert.AreEqual(1, events.Count);

            ev = Event.Get(events[0].Id);
            Assert.AreEqual(events[0].ToString(), ev.ToString());
            Assert.AreEqual(inv.Account, ev.Account);
            Assert.AreEqual(inv.TestMode, ev.TestMode);
            Assert.AreEqual("invoice.deleted", ev.Type);
            Assert.True(ev.RelatedObject is Invoice);
            var evInv = (Invoice) ev.RelatedObject;
            Assert.AreEqual(evInv.Amount, inv.Amount);
            Assert.True(evInv.Deleted);
        }
    }
}
