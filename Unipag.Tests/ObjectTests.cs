using NUnit.Framework;

namespace Unipag.Tests
{
    class ObjectTests
    {
        [Test]
        public void ObjectEquals()
        {
            var inv1 = new Invoice
            {
                Amount = 1,
                Currency = "RUB",
            };

            var inv2 = new Invoice
            {
                Amount = inv1.Amount,
                Currency = inv1.Currency,
            };

            Assert.AreEqual(inv1.ToString(), inv2.ToString());
            inv2.Customer = "customer";
            Assert.AreNotEqual(inv1.ToString(), inv2.ToString());
            inv1.Customer = inv2.Customer;
            Assert.AreEqual(inv1.ToString(), inv2.ToString());
            inv1.CustomData["custom"] = "data";
            Assert.AreNotEqual(inv1.ToString(), inv2.ToString());
            inv2.CustomData["custom"] = "data";
            Assert.AreEqual(inv1.ToString(), inv2.ToString());
        }
    }
}
