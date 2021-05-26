// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftGlobalTest
    {
        [TestMethod]
        public void PftGlobal_Construction_1()
        {
            var node = new PftGlobal();
            Assert.AreEqual(0, node.Number);
            Assert.IsNotNull(node.Fields);
            Assert.AreEqual(0, node.Fields.Count);
        }

        [TestMethod]
        public void PftGlobal_Construction_2()
        {
            var number = 123;
            var node = new PftGlobal(number);
            Assert.AreEqual(number, node.Number);
            Assert.IsNotNull(node.Fields);
            Assert.AreEqual(0, node.Fields.Count);
        }

        [TestMethod]
        public void PftGlobal_Construction_3()
        {
            var number = 123;
            var text = "123\n234";
            var node = new PftGlobal(number, text);
            Assert.AreEqual(number, node.Number);
            Assert.IsNotNull(node.Fields);
            Assert.AreEqual(2, node.Fields.Count);
        }

        [TestMethod]
        public void PftGlobal_Parse_1()
        {
            var node = new PftGlobal();
            Assert.AreSame(node, node.Parse(string.Empty));
            Assert.AreEqual(0, node.Fields.Count);
        }

        [TestMethod]
        public void PftGlobal_Parse_2()
        {
            var node = new PftGlobal();
            Assert.AreSame(node, node.Parse("\n"));
            Assert.AreEqual(2, node.Fields.Count);
            Assert.IsNull(node.Fields[0].Value);
            Assert.IsNull(node.Fields[1].Value);
        }

        [TestMethod]
        public void PftGlobal_Parse_3()
        {
            var text = "text";
            var node = new PftGlobal();
            Assert.AreSame(node, node.Parse(text));
            Assert.AreEqual(1, node.Fields.Count);
            Assert.AreEqual(text, node.Fields[0].Value);
        }

        [TestMethod]
        public void PftGlobal_Parse_4()
        {
            string text1 = "text1", text2 = "text2";
            var node = new PftGlobal();
            Assert.AreSame(node, node.Parse(text1 + '\n' + text2));
            Assert.AreEqual(2, node.Fields.Count);
            Assert.AreEqual(text1, node.Fields[0].Value);
            Assert.AreEqual(text2, node.Fields[1].Value);
        }

        [TestMethod]
        public void PftGlobal_Parse_5()
        {
            var node = new PftGlobal();
            Assert.AreSame(node, node.Parse("^atext"));
            Assert.AreEqual(1, node.Fields.Count);
            Assert.AreEqual("text", node.Fields[0].GetFirstSubFieldValue('a'));
        }

        [TestMethod]
        public void PftGlobal_Parse_6()
        {
            var node = new PftGlobal();
            Assert.AreSame(node, node.Parse("^atext1^btext2"));
            Assert.AreEqual(1, node.Fields.Count);
            Assert.AreEqual("text1", node.Fields[0].GetFirstSubFieldValue('a'));
            Assert.AreEqual("text2", node.Fields[0].GetFirstSubFieldValue('b'));
        }

        [TestMethod]
        public void PftGlobal_Parse_7()
        {
            var node = new PftGlobal();
            Assert.AreSame(node, node.Parse("^atext1^btext2\n^ctext3^dtext4"));
            Assert.AreEqual(2, node.Fields.Count);
            Assert.AreEqual("text1", node.Fields[0].GetFirstSubFieldValue('a'));
            Assert.AreEqual("text2", node.Fields[0].GetFirstSubFieldValue('b'));
            Assert.AreEqual("text3", node.Fields[1].GetFirstSubFieldValue('c'));
            Assert.AreEqual("text4", node.Fields[1].GetFirstSubFieldValue('d'));
        }

        [TestMethod]
        public void PftGlobal_Parse_8()
        {
            var node = new PftGlobal();
            Assert.AreSame(node, node.Parse("^atext1^btext2"));
            Assert.AreEqual("text1", node.Fields[0].GetFirstSubFieldValue('a'));
            Assert.AreEqual("text2", node.Fields[0].GetFirstSubFieldValue('b'));
            Assert.AreEqual(1, node.Fields.Count);
            Assert.AreSame(node, node.Parse("^ctext3^dtext4"));
            Assert.AreEqual("text3", node.Fields[1].GetFirstSubFieldValue('c'));
            Assert.AreEqual("text4", node.Fields[1].GetFirstSubFieldValue('d'));
        }

        private void _TestSerialization
            (
                PftGlobal first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<PftGlobal>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Number, second!.Number);
            Assert.AreEqual(first.Fields.Count, second.Fields.Count);
            for (var i = 0; i < first.Fields.Count; i++)
            {
                Assert.AreEqual(first.Fields[i].ToText(), second.Fields[i].ToText());
            }
        }

        [Ignore]
        [TestMethod]
        public void PftGlobal_Serialization_1()
        {
            var node = new PftGlobal();
            _TestSerialization(node);

            node.Number = 123;
            node.Parse("111\n^atext1^btext2");
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftGlobal_ToString_1()
        {
            var node = new PftGlobal();
            Assert.AreEqual("", node.ToString());
        }
    }
}
