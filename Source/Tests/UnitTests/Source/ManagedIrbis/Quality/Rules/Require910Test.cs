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
    public class Require910Test
        : RuleTest
    {
        [TestMethod]
        public void Require910_Construction_1()
        {
            var check = new Require910();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require910_FieldSpec_1()
        {
            var check = new Require910();
            Assert.AreEqual("910", check.FieldSpec);
        }

        [TestMethod]
        public void Require910_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require910();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void Require910_CheckRecord_2()
        {
            var context = GetContext();
            context.Record!.Add(920, "PAZK");
            context.Record!.Add(910, "^A0^B1^HE00401004DD08FF2^FП26^C20170327^DФП^E1.00");
            var check = new Require910();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
