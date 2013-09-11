using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Globalization;

namespace Unipag.Tests
{
    class InvoiceTests
    {
        [Test]
        public void InvoiceCreate()
        {
            var inv = new Invoice();
            Assert.Null(inv.Account);
            Assert.True(inv.Amount == 0);
            Assert.True(inv.AmountPaid == 0);
            Assert.Null(inv.Currency);
            Assert.Null(inv.Customer);
            Assert.Null(inv.Description);
            Assert.Null(inv.Created);
            Assert.Null(inv.Modified);
            Assert.Null(inv.Expires);
            Assert.Null(inv.TestMode);
            Assert.Null(inv.Reference);

            inv.Amount = 42.9m;
            inv.Currency = "RUB";
            inv.Customer = "Посвящаю эту песню Ринату";
            inv.Description = "Test invoice ®";
            inv.Save();

            Assert.AreEqual("acc_111tov4zxNTQObb3", inv.Account);
            Assert.AreEqual(42.9m, inv.Amount);
            Assert.AreEqual(0, inv.AmountPaid);
            Assert.AreEqual("RUB", inv.Currency);
            Assert.AreEqual("Посвящаю эту песню Ринату", inv.Customer);
            Assert.AreEqual("Test invoice ®", inv.Description);
            Assert.That(inv.Created, Is.EqualTo(DateTime.UtcNow).Within(5).Minutes);
            Assert.That(inv.Modified, Is.EqualTo(DateTime.UtcNow).Within(5).Minutes);
            Assert.AreEqual("", inv.Reference);
            Assert.Null(inv.Expires);
            Assert.AreEqual(false, inv.TestMode);

            var inv2 = Invoice.Get(inv.Id);
            Assert.AreEqual(inv.ToString(), inv2.ToString());
        }

        [Test]
        public void InvoiceExpires()
        {
            var d = DateTime.UtcNow;
            // Truncate milliseconds
            d = d.AddTicks(-(d.Ticks % TimeSpan.TicksPerSecond));

            // Set expiration date and verify that it is set            
            var inv = new Invoice
            {
                Amount = 9000,
                Currency = "RUB",
                Expires = d
            };
            inv.Save();
            var inv2 = Invoice.Get(inv.Id);
            Assert.AreEqual(inv.Expires, d);
            Assert.AreEqual(inv.Expires, inv2.Expires);

            // Now clear and verify that it is cleared
            inv.Expires = null;
            inv.Save();
            inv2.Reload();
            Assert.Null(inv2.Expires);
        }

        [Test]
        public void InvoiceDelete()
        {
            var inv = new Invoice
            {
                Amount = 1,
                Currency = "rub",
            };
            Assert.False(inv.Deleted);
            inv.Save();
            Assert.AreEqual(inv.Currency, "RUB");
            Assert.False(inv.Deleted);
            inv.Delete();
            Assert.True(inv.Deleted);
            inv.Undelete();
            Assert.False(inv.Deleted);
            inv.Deleted = true;
            inv.Save();
            Assert.True(inv.Deleted);
        }

        [Test]
        [ExpectedException(typeof(NotFoundException))]
        public void InvoiceNotFound()
        {
            Invoice.Get("424242424242");
        }

        [Test]
        public void InvoiceCustomData()
        {
            var inv = new Invoice
            {
                Amount = 3,
                Currency = "RUB",
            };
            inv.CustomData["sixteen"] = "being tested";
            inv.CustomData["bool"] = false;
            inv.CustomData["int"] = 42;
            inv.CustomData["decimal"] = 42.5m;
            inv.Save();

            var inv2 = Invoice.Get(inv.Id);
            Assert.AreEqual(inv.ToString(), inv2.ToString());
            Assert.AreEqual(42.5m, inv2.CustomData.Value<decimal>("decimal"));

            inv.CustomData = JObject.Parse("{\"int\": 9000}");
            inv.Save();
            Assert.AreEqual(9000, inv.CustomData.Value<int>("int"));
        }

        [Test]
        [ExpectedException(typeof(NotFoundException))]
        public void InvoiceDifferentApiKeys()
        {
            var inv = new Invoice
            {
                Amount = 4.42m,
                Currency = "RUB",
            };
            Assert.True(string.IsNullOrEmpty(inv.ApiKey));
            inv.Save();
            Assert.AreEqual(Config.ApiKey, inv.ApiKey);

            var originalKey = Config.ApiKey;
            var anotherKey = "xmc2h3UXvzYJrwS4D4ZFFEnH";
            var inv2 = new Invoice
            {
                Amount = 49,
                Currency = "RUB",
            };
            Assert.Null(inv2.ApiKey);
            try
            {
                Config.ApiKey = anotherKey;
                inv2.Save();
                Assert.AreEqual(anotherKey, inv2.ApiKey);
                inv.Reload();
                Assert.AreEqual(originalKey, inv.ApiKey);
            }
            finally
            {
                Config.ApiKey = null;  // When null, effective value will be taken from App.config
            }

            inv2.Reload();
            Assert.AreEqual(anotherKey, inv2.ApiKey);

            // This should throw NotFoundException, since we're using key from another account
            inv2.ApiKey = originalKey;
            inv2.Reload();
        }

        [Test]
        public void InvoiceList()
        {
            var invoices = Invoice.List();
            Assert.AreEqual(invoices[0].ApiKey, Config.ApiKey);
            var inv1 = invoices[0];
            var inv2 = Invoice.Get(inv1.Id);
            Assert.AreEqual(inv1.Amount, inv2.Amount);
            Assert.AreEqual(inv1.Created, inv2.Created);

            invoices = Invoice.List(new Dictionary<string, object>
            { 
                { "count", "3" } 
            });
            Assert.AreEqual(3, invoices.Count);
        }
    }
}
