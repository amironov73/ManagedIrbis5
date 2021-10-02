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
    public sealed class Check906Test
        : RuleTest
    {
        [TestMethod]
        public void Check906_Construction_1()
        {
            var check = new Check906();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check906_FieldSpec_1()
        {
            var check = new Check906();
            Assert.AreEqual("906", check.FieldSpec);
        }

        [TestMethod]
        public void Check906_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Check906();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void Check906_CheckRecord_2()
        {
            var context = GetContext();
            context.Record!.Add(621, "51.1(2Рос-4Ирк)я431");
            context.Record!.Add(908, "О-64");
            context.Record!.Add(903, "51.1(2Рос-4Ирк)/О-64-304341458");
            context.Record!.Add(906, "51.1(2Рос-4Ирк)");
            var check = new Check906();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

    }
}
