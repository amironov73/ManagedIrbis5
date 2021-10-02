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
    public sealed class Check675Test
        : RuleTest
    {
        [TestMethod]
        public void Check675_Construction_1()
        {
            var check = new Check675();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check675_FieldSpec_1()
        {
            var check = new Check675();
            Assert.AreEqual("675", check.FieldSpec);
        }

        [TestMethod]
        public void Check675_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Check675();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void Check675_CheckRecord_2()
        {
            var context = GetContext();
            context.Record!.Add(675, "614(571.53)");
            var check = new Check675();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

    }
}
