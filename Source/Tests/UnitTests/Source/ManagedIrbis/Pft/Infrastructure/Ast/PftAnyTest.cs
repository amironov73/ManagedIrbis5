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
    public class PftAnyTest
    {
        private void _Execute
            (
                Record record,
                PftAny node,
                bool expected
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

            var field = new Field { Tag = 700};
            field.Add('a', "Иванов");
            field.Add('b', "И. И.");
            result.Fields.Add(field);

            field = new Field { Tag = 701 };
            field.Add('a', "Петров");
            field.Add('b', "П. П.");
            result.Fields.Add(field);

            field = new Field { Tag = 200 };
            field.Add('a', "Заглавие");
            field.Add('e', "подзаголовочное");
            field.Add('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new Field { Tag = 300, Value = "Первое примечание" };
            result.Fields.Add(field);
            field = new Field { Tag = 300, Value = "Второе примечание"};
            result.Fields.Add(field);
            field = new Field { Tag = 300, Value = "Третье примечание" };
            result.Fields.Add(field);

            return result;
        }

        private PftAny _GetNode()
        {
            return new PftAny
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
        public void PftAny_Construction_1()
        {
            var node = new PftAny();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftAny_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Any, 1, 1, "any");
            var node = new PftAny(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftAny_Clone_1()
        {
            var first = new PftAny();
            var second = (PftAny) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAny_Clone_2()
        {
            var first = _GetNode();
            var second = (PftAny)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAny_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetNode();
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftAny_Execute_2()
        {
            var record = _GetRecord();
            var node = _GetNode();
            var comparison = (PftComparison?) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison!.RightOperand = new PftUnconditionalLiteral("noSuchWord");
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftAny_Execute_2a()
        {
            var record = _GetRecord();
            var node = _GetNode();
            var comparison = (PftComparison?) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison!.LeftOperand = new PftV("v444");
            _Execute(record, node, false);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftAny_Execute_3()
        {
            var node = _GetNode();
            var context = new PftContext(null)
            {
                CurrentGroup = new PftGroup()
            };
            node.Execute(context);
        }

        [TestMethod]
        public void PftAny_Execute_4()
        {
            var record = _GetRecord();
            var node = new PftAny
            {
                InnerCondition = new PftComparison
                {
                    LeftOperand = new PftUnconditionalLiteral("1"),
                    Operation = "=",
                    RightOperand = new PftUnconditionalLiteral("1")
                }
            };
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftAny_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Any", info.Name);
        }

        [TestMethod]
        public void PftAny_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("any(v300:'примечание')", printer.ToString());
        }

        private void _TestSerialization
            (
                PftAny first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftAny) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAny_Serialization_1()
        {
            var node = new PftAny();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftAny_ToString_1()
        {
            var node = new PftAny();
            Assert.AreEqual("any()", node.ToString());
        }

        [TestMethod]
        public void PftAny_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("any(v300:'примечание')", node.ToString());
        }
    }
}
