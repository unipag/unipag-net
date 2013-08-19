using NUnit.Framework;

namespace Unipag.Tests
{
    class EventTests
    {
        [Test]
        public void EventGet()
        {
            var ev = new Event();
            Assert.Null(ev.Account);
            Assert.Null(ev.Created);
            Assert.Null(ev.RelatedObject);
            Assert.Null(ev.RelatedObjectPreviousState);
            Assert.Null(ev.TestMode);
            Assert.Null(ev.Type);

            ev.Id = "ev_vMjxqzBkwKS87dy3D";
            ev.Reload();

            Assert.AreEqual("acc_111tov4zxNTQObb3", ev.Account);
            Assert.False((bool)ev.TestMode);
            Assert.AreEqual("payment.created", ev.Type);
            Assert.True(ev.RelatedObject is Payment);
            Assert.Null(ev.RelatedObjectPreviousState);

            var payment = (Payment)ev.RelatedObject;
            payment.Reload();
        }
    }
}
