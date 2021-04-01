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
    public class Require920Test
        : RuleTest
    {
        [TestMethod]
        public void Require920_Construction_1()
        {
            var check = new Require920();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require920_FieldSpec_1()
        {
            var check = new Require920();
            Assert.AreEqual("920", check.FieldSpec);
        }

        [TestMethod]
        public void Require920_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require920();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
