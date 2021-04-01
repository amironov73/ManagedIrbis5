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
    public class Require903Test
        : RuleTest
    {
        [TestMethod]
        public void Require903_Construction_1()
        {
            Require903 check = new Require903();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require903_FieldSpec_1()
        {
            Require903 check = new Require903();
            Assert.AreEqual("903", check.FieldSpec);
        }

        [TestMethod]
        public void Require903_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require903 check = new Require903();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
