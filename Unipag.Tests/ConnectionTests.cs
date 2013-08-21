using NUnit.Framework;

namespace Unipag.Tests
{
    class ConnectionTests
    {
        [Test]
        public void ConnectionList()
        {
            var connections = Connection.List();
            foreach (var conn in connections)
            {
                if (conn.PaymentGateway == "liqpay.com")
                {
                    Assert.AreEqual("acc_111tov4zxNTQObb3", conn.Account);
                    Assert.True(conn.CurrenciesSupported.Contains("UAH"));
                    Assert.True(conn.Enabled);
                    Assert.AreEqual(false, conn.TestMode);
                    Assert.Null(conn.SharedFrom);

                    var conn2 = Connection.Get(conn.Id);
                    Assert.AreEqual(conn.ToString(), conn2.ToString());
                    return;
                }
            }
            Assert.Fail("Liqpay connection not found");
        }
    }
}
