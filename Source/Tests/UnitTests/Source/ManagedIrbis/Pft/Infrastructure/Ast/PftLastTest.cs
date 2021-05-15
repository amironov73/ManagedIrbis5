// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftLastTest
    {
        private void _Execute
            (
                Record record,
                PftLast node,
                double expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            var actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        private Record _GetRecord()
        {
            var result = new Record();

            var field = new Field(700)
            {
                {'a', "Иванов"},
                {'b', "И. И."}
            };
            result.Fields.Add(field);

            field = new Field(701)
            {
                {'a', "Петров"},
                {'b', "П. П."}
            };
            result.Fields.Add(field);

            field = new Field(200)
            {
                {'a', "Заглавие"},
                {'e', "подзаголовочное"},
                {'f', "И. И. Иванов, П. П. Петров"}
            };
            result.Fields.Add(field);

            field = new Field(300, "Первое примечание");
            result.Fields.Add(field);
            field = new Field(300, "Второе примечание");
            result.Fields.Add(field);
            field = new Field(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        private PftLast _GetNode()
        {
            return new PftLast
            {
                InnerCondition = new PftComparison
                {
                    LeftOperand = new PftV("v300"),
                    Operation = ":",
                    RightOperand = new PftUnconditionalLiteral("примечание")
                }
            };
        }

        [TestMethod]
        public void PftLast_Construction_1()
        {
            var node = new PftLast();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftLast_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Last, 1, 1, "last");
            var node = new PftLast(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftLast_Clone_1()
        {
            var first = new PftLast();
            var second = (PftLast) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLast_Clone_2()
        {
            var first = _GetNode();
            var second = (PftLast)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLast_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetNode();
            _Execute(record, node, 3);
        }

        [TestMethod]
        public void PftLast_Execute_2()
        {
            var record = _GetRecord();
            var node = _GetNode();
            var comparison = (PftComparison?) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison!.RightOperand = new PftUnconditionalLiteral("noSuchWord");
            _Execute(record, node, 0);
        }

        [TestMethod]
        public void PftLast_Execute_2a()
        {
            var record = _GetRecord();
            var node = _GetNode();
            var comparison = (PftComparison?) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison!.RightOperand = new PftV("v444");
            _Execute(record, node, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftLast_Execute_3()
        {
            var node = _GetNode();
            var context = new PftContext(null)
            {
                CurrentGroup = new PftGroup()
            };
            node.Execute(context);
        }

        [TestMethod]
        public void PftLast_Execute_4()
        {
            var record = _GetRecord();
            var node = new PftLast
            {
                InnerCondition = new PftComparison
                {
                    LeftOperand = new PftUnconditionalLiteral("1"),
                    Operation = "=",
                    RightOperand = new PftUnconditionalLiteral("1")
                }
            };
            _Execute(record, node, 0);
        }


        [TestMethod]
        public void PftLast_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Last", info.Name);
        }

        [TestMethod]
        public void PftLast_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("last(v300:'примечание')", printer.ToString());
        }

        private void _TestSerialization
            (
                PftLast first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftLast) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLast_Serialization_1()
        {
            var node = new PftLast();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftLast_ToString_1()
        {
            var node = new PftLast();
            Assert.AreEqual("last()", node.ToString());
        }

        [TestMethod]
        public void PftLast_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("last(v300:'примечание')", node.ToString());
        }
    }
}

