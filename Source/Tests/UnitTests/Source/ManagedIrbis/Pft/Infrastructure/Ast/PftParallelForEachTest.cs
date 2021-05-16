// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

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
    public class PftParallelForEachTest
    {
        private void _Execute
            (
                Record record,
                PftParallelForEach node,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.IsNotNull(actual);

            // Assert.AreEqual(expected, actual);
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

        private PftParallelForEach _GetNode()
        {
            var name = "x";
            return new PftParallelForEach
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
        public void PftParallelForEach_Construction_1()
        {
            var node = new PftParallelForEach();
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
        public void PftParallelForEach_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Parallel, 1, 1, "parallel");
            var node = new PftParallelForEach(token);
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
        public void PftParallelForEach_Clone_1()
        {
            var first = new PftParallelForEach();
            var second = (PftParallelForEach) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelForEach_Clone_2()
        {
            var first = _GetNode();
            var second = (PftParallelForEach)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelForEach_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetNode();

            // TODO FIX THIS!
            _Execute
                (
                    record,
                    node,
                    "Заглавие\nподзаголовочное\nHello\n"
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftParallelForEach_Execute_2()
        {
            var record = _GetRecord();
            var node = new PftParallelForEach();
            _Execute(record, node, "");
        }

        //[TestMethod]
        //public void PftParallelForEach_Execute_3()
        //{
        //    MarcRecord record = _GetRecord();
        //    PftParallelForEach node = _GetNode();
        //    node.Body.Add(new PftBreak());
        //    _Execute
        //        (
        //            record,
        //            node,
        //            "Заглавие\n"
        //        );
        //}

        [TestMethod]
        public void PftParallelForEach_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("ParallelForEach", info.Name);
        }

        [TestMethod]
        public void PftParallelForEach_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nparallel foreach $x in v200^a, v200^e, \'Hello\'do\n  $x /\nend\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftParallelForEach first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftParallelForEach) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelForEach_Serialization_1()
        {
            var node = new PftParallelForEach();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftParallelForEach_ToString_1()
        {
            var node = new PftParallelForEach();
            Assert.AreEqual("parallel foreach  in  do  end", node.ToString());
        }

        [TestMethod]
        public void PftParallelForEach_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("parallel foreach $x in v200^a v200^e 'Hello' do $x / end", node.ToString());
        }
    }
}
