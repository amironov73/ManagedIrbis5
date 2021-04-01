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
    public class Check610Test
        : RuleTest
    {
        [TestMethod]
        public void Check610_Construction_1()
        {
            var check = new Check610();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check610_FieldSpec_1()
        {
            var check = new Check610();
            Assert.AreEqual("610", check.FieldSpec);
        }

        [TestMethod]
        public void Check610_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Check610();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
