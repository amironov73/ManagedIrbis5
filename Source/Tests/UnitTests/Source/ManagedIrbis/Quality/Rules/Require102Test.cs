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
    public class Require102Test
        : RuleTest
    {
        [TestMethod]
        public void Require102_Construction_1()
        {
            var check = new Require102();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require102_FieldSpec_1()
        {
            var check = new Require102();
            Assert.AreEqual("102", check.FieldSpec);
        }

        [TestMethod]
        public void Require102_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require102();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
