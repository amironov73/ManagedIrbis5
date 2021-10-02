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
    public class Require210Test
        : RuleTest
    {
        [TestMethod]
        public void Require210_Construction_1()
        {
            var check = new Require210();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require210_FieldSpec_1()
        {
            var check = new Require210();
            Assert.AreEqual("210", check.FieldSpec);
        }

        [TestMethod]
        public void Require210_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require210();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void Require210_CheckRecord_2()
        {
            var context = GetContext();
            context.Record!.Add(210, "^cАспринт^aИркутск^d2004");
            var check = new Require210();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
