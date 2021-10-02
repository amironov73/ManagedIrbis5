// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Quality;
using ManagedIrbis.Quality.Rules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Quality.Rules
{
    [TestClass]
    public class Require101Test
        : RuleTest
    {
        [TestMethod]
        public void Require101_Construction_1()
        {
            var check = new Require101();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require101_FieldSpec_1()
        {
            var check = new Require101();
            Assert.AreEqual("101", check.FieldSpec);
        }

        [TestMethod]
        public void Require101_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require101();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void Require101_CheckRecord_2()
        {
            var context = GetContext();
            context.Record!.Add(101, "rus");
            var check = new Require101();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
