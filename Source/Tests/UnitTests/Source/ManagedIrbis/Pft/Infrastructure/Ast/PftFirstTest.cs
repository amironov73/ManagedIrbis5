// ReSharper disable CheckNamespace
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
    public class PftFirstTest
    {
        private void _Execute
            (
                Record record,
                PftFirst node,
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

        private PftFirst _GetNode()
        {
            return new PftFirst
            {
                InnerCondition = new PftComparison
                {
                    LeftOperand = new PftV("v300"),
                    Operation = ":",
                    RightOperand = new PftUnconditionalLiteral("Третье")
                }
            };
        }

        [TestMethod]
        public void PftFirst_Construction_1()
        {
            var node = new PftFirst();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftFirst_Construction_2()
        {
            var token = new PftToken(PftTokenKind.First, 1, 1, "first");
            var node = new PftFirst(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFirst_Clone_1()
        {
            var first = new PftFirst();
            var second = (PftFirst) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFirst_Clone_2()
        {
            var first = _GetNode();
            var second = (PftFirst)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFirst_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetNode();
            _Execute(record, node, 3);
        }

        [TestMethod]
        public void PftFirst_Execute_2()
        {
            var record = _GetRecord();
            var node = _GetNode();
            var comparison = (PftComparison?) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison!.RightOperand = new PftUnconditionalLiteral("noSuchWord");
            _Execute(record, node, 0);
        }

        [TestMethod]
        public void PftFirst_Execute_2a()
        {
            var record = _GetRecord();
            var node = _GetNode();
            var comparison = (PftComparison?) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison!.LeftOperand = new PftV("v444");
            _Execute(record, node, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftFirst_Execute_3()
        {
            var record = _GetRecord();
            var node = _GetNode();
            var context = new PftContext(null)
            {
                CurrentGroup = new PftGroup(),
                Record = record
            };
            node.Execute(context);
        }

        [TestMethod]
        public void PftFirst_Execute_4()
        {
            var record = _GetRecord();
            var node = new PftFirst
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
        public void PftFirst_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("First", info.Name);
        }

        [TestMethod]
        public void PftFirst_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("first(v300:'Третье')", printer.ToString());
        }

        private void _TestSerialization
            (
                PftFirst first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftFirst) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFirst_Serialization_1()
        {
            var node = new PftFirst();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftFirst_ToString_1()
        {
            var node = new PftFirst();
            Assert.AreEqual("first()", node.ToString());
        }

        [TestMethod]
        public void PftFirst_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("first(v300:'Третье')", node.ToString());
        }
    }
}

