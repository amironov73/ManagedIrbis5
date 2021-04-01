// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Quality;
using ManagedIrbis.Quality.Rules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Quality.Rules
{
    [TestClass]
    public class Check908Test
        : RuleTest
    {
        [TestMethod]
        public void Check908_Construction_1()
        {
            var check = new Check908();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check908_FieldSpec_1()
        {
            var check = new Check908();
            Assert.AreEqual("908", check.FieldSpec);
        }

        [TestMethod]
        public void Check908_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Check908();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
