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
    public class Require700Test
        : RuleTest
    {
        [TestMethod]
        public void Require700_Construction_1()
        {
            var check = new Require700();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require700_FieldSpec_1()
        {
            var check = new Require700();
            Assert.AreEqual("70[01]", check.FieldSpec);
        }

        [TestMethod]
        public void Require700_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require700();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
