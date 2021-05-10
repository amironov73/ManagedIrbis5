// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Reports;

#nullable enable

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class TextCellTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TextCell_Construction_1()
        {
            var cell = new TextCell();
            Assert.IsNotNull(cell.Attributes);
            Assert.IsNull(cell.Band);
            Assert.IsNull(cell.Report);
            Assert.IsNull(cell.UserData);
            Assert.IsNull(cell.Text);
        }

        [TestMethod]
        public void TextCell_Construction_2()
        {
            var text = "Text";
            var cell = new TextCell(text);
            Assert.IsNotNull(cell.Attributes);
            Assert.IsNull(cell.Band);
            Assert.IsNull(cell.Report);
            Assert.IsNull(cell.UserData);
            Assert.AreSame(text, cell.Text);
        }


        [TestMethod]
        public void TextCell_Compute_1()
        {
            var text = "Text";
            var cell = new TextCell(text);
            using var provider = GetProvider();
            var context = new ReportContext(provider);
            var output = cell.Compute(context);
            Assert.AreEqual(text, output);
        }

        [TestMethod]
        public void TextCell_Render_1()
        {
            var text = "Text";
            var cell = new TextCell(text);
            using var provider = GetProvider();
            var context = new ReportContext(provider);
            cell.Render(context);
            var output = context.Output.Text;
            Assert.AreEqual("Text\t", output);
        }

        [TestMethod]
        public void TextCell_Verify_1()
        {
            var cell = new TextCell();
            Assert.IsTrue(cell.Verify(false));
        }

        [TestMethod]
        public void TextCell_Dispose_1()
        {
            var cell = new TextCell();
            cell.Dispose();
        }

        [TestMethod]
        public void TextCell_Clone_1()
        {
            var first = new TextCell("text")
            {
                UserData = "user data"
            };
            var second = (TextCell) first.Clone();
            Assert.AreEqual(first.Attributes.Count, second.Attributes.Count);
            Assert.AreSame(first.Band, second.Band);
            Assert.AreSame(first.Report, second.Report);
            Assert.AreSame(first.UserData, second.UserData);
            Assert.AreEqual(first.Text, second.Text);
        }
    }
}
