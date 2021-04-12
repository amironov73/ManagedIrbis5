// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftForEachTest
    {
        private void _Execute
            (
                Record record,
                PftForEach node,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private Record _GetRecord()
        {
            var result = new Record();

            var field = new Field(700) {{'a', "Иванов"}, {'b', "И. И."}};
            result.Fields.Add(field);

            field = new Field(701) {{'a', "Петров"}, {'b', "П. П."}};
            result.Fields.Add(field);

            field = new Field(200) {{'a', "Заглавие"}, {'e', "подзаголовочное"}, {'f', "И. И. Иванов, П. П. Петров"}};
            result.Fields.Add(field);

            field = new Field(300, "Первое примечание");
            result.Fields.Add(field);
            field = new Field(300, "Второе примечание");
            result.Fields.Add(field);
            field = new Field(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        private PftForEach _GetNode()
        {
            var name = "x";
            return new PftForEach
            {
                Variable = new PftVariableReference(name),
                Sequence =
                {
                    new PftV("v200^a"),
                    new PftV("v200^e"),
                    new PftUnconditionalLiteral("Hello")
                },
                Body =
                {
                    new PftVariableReference(name),
                    new PftSlash()
                }
            };
        }

        [TestMethod]
        public void PftForEach_Construction_1()
        {
            var node = new PftForEach();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Variable);
            Assert.IsNotNull(node.Sequence);
            Assert.AreEqual(0, node.Sequence.Count);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
        }

        [TestMethod]
        public void PftForEach_Construction_2()
        {
            var token = new PftToken(PftTokenKind.ForEach, 1, 1, "foreach");
            var node = new PftForEach(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Variable);
            Assert.IsNotNull(node.Sequence);
            Assert.AreEqual(0, node.Sequence.Count);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftForEach_Clone_1()
        {
            var first = new PftForEach();
            var second = (PftForEach) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftForEach_Clone_2()
        {
            var first = _GetNode();
            var second = (PftForEach)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftForEach_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetNode();
            _Execute
                (
                    record,
                    node,
                    "Заглавие\nподзаголовочное\nHello\n"
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftForEach_Execute_2()
        {
            var record = _GetRecord();
            var node = new PftForEach();
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftForEach_Execute_3()
        {
            var record = _GetRecord();
            var node = _GetNode();
            node.Body.Add(new PftBreak());
            _Execute
                (
                    record,
                    node,
                    "Заглавие\n"
                );
        }

        [TestMethod]
        public void PftForEach_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("ForEach", info.Name);
        }

        [TestMethod]
        public void PftForEach_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nforeach $x in v200^a, v200^e, \'Hello\'do\n  $x /\nend\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftForEach first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftForEach) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftForEach_Serialization_1()
        {
            var node = new PftForEach();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftForEach_ToString_1()
        {
            var node = new PftForEach();
            Assert.AreEqual("foreach  in  do  end", node.ToString());
        }

        [TestMethod]
        public void PftForEach_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("foreach $x in v200^a v200^e 'Hello' do $x / end", node.ToString());
        }
    }
}
