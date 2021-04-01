// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblBuilderTest
    {
        [TestMethod]
        public void GblBuilder_Construction_1()
        {
            var builder = new GblBuilder();
            Assert.AreEqual(0, builder.ToStatements().Length);
        }

        [TestMethod]
        public void GblBuilder_Add_1()
        {
            var builder = new GblBuilder();
            builder.Add("910", "^a0^b1");
            var statements = builder.ToStatements();
            Assert.AreEqual("ADD", statements[0].Command);
            Assert.AreEqual("910", statements[0].Parameter1);
            Assert.AreEqual("^a0^b1", statements[0].Format1);
        }

        [TestMethod]
        public void GblBuilder_Change_1()
        {
            var builder = new GblBuilder();
            builder.Change("910", "^a0^b1", "^a9^b1");
            var statements = builder.ToStatements();
            Assert.AreEqual("CHA", statements[0].Command);
            Assert.AreEqual("910", statements[0].Parameter1);
            Assert.AreEqual("^a0^b1", statements[0].Format1);
            Assert.AreEqual("^a9^b1", statements[0].Format2);
        }

        [TestMethod]
        public void GblBuilder_Delete_1()
        {
            var builder = new GblBuilder();
            builder.Delete("910", "1");
            var statements = builder.ToStatements();
            Assert.AreEqual("DEL", statements[0].Command);
            Assert.AreEqual("910", statements[0].Parameter1);
            Assert.AreEqual("1", statements[0].Parameter2);
        }

        [TestMethod]
        public void GblBuilder_DeleteRecord_1()
        {
            var builder = new GblBuilder();
            builder.DeleteRecord();
            var statements = builder.ToStatements();
            Assert.AreEqual("DELR", statements[0].Command);
        }
    }
}
