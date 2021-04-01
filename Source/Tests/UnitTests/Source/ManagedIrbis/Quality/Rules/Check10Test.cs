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
    public class Check10Test
        : RuleTest
    {
        [TestMethod]
        public void Check10_Construction_1()
        {
            var check = new Check10();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check10_FieldSpec_1()
        {
            var check = new Check10();
            Assert.AreEqual("10", check.FieldSpec);
        }

        [TestMethod]
        public void Check10_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Check10();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
