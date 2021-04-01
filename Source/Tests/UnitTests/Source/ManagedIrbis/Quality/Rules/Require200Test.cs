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
    public class Require200Test
        : RuleTest
    {
        [TestMethod]
        public void Require200_Construction_1()
        {
            var check = new Require200();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require200_FieldSpec_1()
        {
            var check = new Require200();
            Assert.AreEqual("200", check.FieldSpec);
        }

        [TestMethod]
        public void Require200_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require200();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
