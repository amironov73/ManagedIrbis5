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
    public sealed class Check621Test
        : RuleTest
    {
        [TestMethod]
        public void Check621_Construction_1()
        {
            var check = new Check621();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check621_FieldSpec_1()
        {
            var check = new Check621();
            Assert.AreEqual("621", check.FieldSpec);
        }

        [TestMethod]
        public void Check621_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Check621();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void Check621_CheckRecord_2()
        {
            var context = GetContext();
            context.Record!.Add(621, "51.1(2Рос-4Ирк)я431");
            var check = new Check621();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

    }
}
