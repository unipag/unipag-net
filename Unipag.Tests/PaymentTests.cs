using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Unipag.Tests
{
    public class PaymentTests
    {
        [Test]
        public void PaymentCreate()
        {
            var liqpayConnId = "111000064";
            var payment = new Payment
            {
                Amount = 13,
                Currency = "RUB",
                Connection = liqpayConnId,
            };
            payment.Params["description"] = "yo, liqpay";
            Assert.AreEqual(0, payment.AmountPaid);
            Assert.AreEqual(0, payment.AmountRefunded);
            Assert.False(payment.Paid);
            Assert.False(payment.Refunded);
            Assert.False(payment.Cancelled);
            Assert.True(string.IsNullOrEmpty(payment.Id));
            payment.Save();
            Assert.False(string.IsNullOrEmpty(payment.Id));

            var payment2 = Payment.Get(payment.Id);
            Assert.AreEqual(13, payment2.Amount);
            Assert.AreEqual("RUB", payment2.Currency);
            Assert.AreEqual(liqpayConnId, payment2.Connection);
            Assert.AreEqual("yo, liqpay", payment2.Params["description"].ToString());
            Assert.That(payment2.Created, Is.EqualTo(DateTime.UtcNow).Within(5).Minutes);
            Assert.That(payment2.Modified, Is.EqualTo(DateTime.UtcNow).Within(5).Minutes);
            Assert.True(payment2.PaymentUrl.Contains("unipag"));
            Assert.AreEqual(payment.ToString(), payment2.ToString());
        }

        [Test]
        public void PaymentCancel()
        {
            var payment = Payment.Create(new Payment
            {
                Amount = 1,
                Connection = "111000064",
                Currency = "RUB",
            });
            Assert.False(payment.Cancelled);

            Payment.Cancel(payment.Id);
            payment.Reload();
            Assert.True(payment.Cancelled);
        }

        [Test]
        public void PaymentsList()
        {
            var payments = Payment.List(new Dictionary<string, object>
            {
                {"count", 3},
            });
            Assert.AreEqual(3, payments.Count);
        }

        [Test]
        [ExpectedException(typeof(NotFoundException))]
        public void PaymentNotFound()
        {
            Payment.Get("4242424242424242");
        }

    }
}
