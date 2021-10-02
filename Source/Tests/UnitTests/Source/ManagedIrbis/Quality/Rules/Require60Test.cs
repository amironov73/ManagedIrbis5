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
    public class Require60Test
        : RuleTest
    {
        [TestMethod]
        public void Require60_Construction_1()
        {
            var check = new Require60();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require60_FieldSpec_1()
        {
            var check = new Require60();
            Assert.AreEqual("60", check.FieldSpec);
        }

        [TestMethod]
        public void Require60_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require60();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void Require60_CheckRecord_2()
        {
            var context = GetContext();
            context.Record!.Add(60, "4");
            var check = new Require60();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
