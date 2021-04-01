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
    public class Require215Test
        : RuleTest
    {
        [TestMethod]
        public void Require215_Construction_1()
        {
            var check = new Require215();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require215_FieldSpec_1()
        {
            var check = new Require215();
            Assert.AreEqual("215", check.FieldSpec);
        }

        [TestMethod]
        public void Require215_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require215();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
