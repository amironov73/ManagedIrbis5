// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftATest
        : Common.CommonUnitTest
    {
        private void _Execute
        (
            Record record,
            PftA node,
            bool expected
        )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            context.Globals.Add(100, "global100");
            node.Execute(context);
            var actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        private PftA _GetVNode()
        {
            return new PftA(200, 'a');
        }

        private PftA _GetGNode(int index)
        {
            return new PftA
            {
                Field = new PftG(index)
            };
        }

        private Record _GetRecord()
        {
            var result = new Record();

            var field = new Field { Tag = 700 };
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

            field = new Field (300, "Первое примечание");
            result.Fields.Add(field);
            field = new Field (300, "Второе примечание");
            result.Fields.Add(field);
            field = new Field (300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void PftA_Construction_1()
        {
            var node = new PftA();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Field);
        }

        [TestMethod]
        public void PftA_Construction_2()
        {
            var token = new PftToken(PftTokenKind.P, 1, 1, "p");
            var node = new PftA(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Field);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftA_Construction_3()
        {
            var text = "v200^a";
            var node = new PftA(text);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual(text, node.Field!.ToString());
        }

        [TestMethod]
        public void PftA_Construction_4()
        {
            var node = new PftA(200);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual("v200", node.Field!.ToString());
        }

        [TestMethod]
        public void PftA_Construction_5()
        {
            var node = new PftA(200, 'a');
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual("v200^a", node.Field!.ToString());
        }

        [TestMethod]
        public void PftA_Clone_1()
        {
            var first = new PftA();
            var second = (PftA)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftA_Clone_2()
        {
            var first = _GetVNode();
            var second = (PftA)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftA_Compile_1()
        {
            var node = _GetVNode();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftA_Compile_2()
        {
            var node = _GetGNode(100);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftA_Compile_3()
        {
            var node = new PftA();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftA_Execute_1()
        {
            var record = _GetRecord();
            var node = new PftA();
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_2()
        {
            var record = _GetRecord();
            var node = _GetVNode();
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_3()
        {
            var record = _GetRecord();
            var node = _GetGNode(100);
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_3a()
        {
            var record = _GetRecord();
            var node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field!.SubField = 'a';
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftA_Execute_3b()
        {
            var record = _GetRecord();
            var node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field!.SubField = '*';
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_3c()
        {
            var record = _GetRecord();
            var node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field!.Offset = 1;
            node.Field.Length = 2;
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_4()
        {
            var record = _GetRecord();
            var node = _GetGNode(101);
            _Execute(record, node, true);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftA_Execute_5()
        {
            var record = _GetRecord();
            var node = new PftA
            {
                Field = new PftField
                {
                    Command = 'd',
                    Tag = "100"
                }
            };
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_GetNodeInfo_1()
        {
            var node = _GetVNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("A", info.Name);
        }

        [TestMethod]
        public void PftA_GetNodeInfo_2()
        {
            var node = _GetGNode(100);
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("A", info.Name);
        }

        private void _TestSerialization
            (
                PftA first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftA)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRef_Serialization_1()
        {
            var node = new PftA();
            _TestSerialization(node);

            node = _GetVNode();
            _TestSerialization(node);

            node = _GetGNode(100);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftA_ToString_1()
        {
            var node = new PftA();
            Assert.AreEqual("a()", node.ToString());

            node = _GetVNode();
            Assert.AreEqual("a(v200^a)", node.ToString());

            node = _GetGNode(100);
            Assert.AreEqual("a(g100)", node.ToString());
        }
    }
}
