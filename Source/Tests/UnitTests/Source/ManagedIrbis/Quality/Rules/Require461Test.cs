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
    public class Require461Test
        : RuleTest
    {
        [TestMethod]
        public void Require416_Construction_1()
        {
            var check = new Require461();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require461_FieldSpec_1()
        {
            var check = new Require461();
            Assert.AreEqual("461", check.FieldSpec);
        }

        [TestMethod]
        public void Require461_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require461();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
