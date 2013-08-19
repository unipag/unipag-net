using NUnit.Framework;

namespace Unipag.Tests
{
    class AccountTests
    {
        [Test]
        public void AccountGet()
        {
            var account = Account.Get();
            Assert.AreEqual("acc_111tov4zxNTQObb3", account.Id);
            Assert.AreEqual("unipag_unittest", account.Name);
            Assert.AreEqual(Config.ApiKey, account.SecretKey);
            Assert.False((bool)account.TestMode);

            var account2 = Account.Get("xmc2h3UXvzYJrwS4D4ZFFEnH");
            Assert.AreEqual("unipag_unittest2", account2.Name);

            var account3 = new Account
            {
                Id = account2.Id,
            };
            account3.Reload();
        }
    }
}
