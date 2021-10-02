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
    public class Require900Test
        : RuleTest
    {
        [TestMethod]
        public void Require900_Construction_1()
        {
            var check = new Require900();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require900_FieldSpec_1()
        {
            var check = new Require900();
            Assert.AreEqual("900", check.FieldSpec);
        }

        [TestMethod]
        public void Require900_CheckRecord_1()
        {
            var context = GetContext();
            var check = new Require900();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void Require900_CheckRecord_2()
        {
            var context = GetContext();
            context.Record!.Add(903, "51.1(2Рос-4Ирк)/О-64-304341458");
            var check = new Require900();
            var report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
