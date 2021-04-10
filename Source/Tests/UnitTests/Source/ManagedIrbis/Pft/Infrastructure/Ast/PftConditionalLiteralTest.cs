// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftConditionalLiteralTest
    {
        private void _Execute
            (
                Record record,
                PftField node,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            context.Globals.Add(100, "First");
            context.Globals.Append(100, "Second");
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private void _ExecuteUpper
            (
                Record record,
                PftField node,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record,
                UpperMode = true
            };
            context.Globals.Add(100, "First");
            context.Globals.Append(100, "Second");
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private PftField _GetVNode
            (
                int tag,
                char code,
                bool suffix
            )
        {
            var result = new PftV(tag, code);

            if (suffix)
            {
                result.RightHand.Add
                    (
                        new PftConditionalLiteral(" <<", true)
                    );
            }
            else
            {
                result.LeftHand.Add
                    (
                        new PftConditionalLiteral(">> ", false)
                    );
            }

            return result;
        }

        private PftField _GetVNode2
            (
                int tag,
                char code,
                bool suffix
            )
        {
            var result = new PftV(tag, code);

            if (suffix)
            {
                result.RightHand.Add
                    (
                        new PftConditionalLiteral(" суффикс", true)
                    );
            }
            else
            {
                result.LeftHand.Add
                    (
                        new PftConditionalLiteral("префикс ", false)
                    );
            }

            return result;
        }

        private PftField _GetGNode
            (
                int tag,
                char code,
                bool suffix
            )
        {
            var result = new PftG(tag, code);

            if (suffix)
            {
                result.RightHand.Add
                    (
                        new PftConditionalLiteral(" <<", true)
                    );
            }
            else
            {
                result.LeftHand.Add
                    (
                        new PftConditionalLiteral(">> ", false)
                    );
            }

            return result;
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
        public void PftConditionalLiteral_Construction_1()
        {
            var node = new PftConditionalLiteral();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.IsSuffix);
            Assert.IsNull(node.Text);
        }

        [TestMethod]
        public void PftConditionalLiteral_Construction_2()
        {
            var token = new PftToken(PftTokenKind.ConditionalLiteral, 1, 1, "\"\"");
            var node = new PftConditionalLiteral(token, false);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.IsSuffix);
            Assert.IsNotNull(node.Text);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionalLiteral_Construction_2a()
        {
            var token = new PftToken(PftTokenKind.ConditionalLiteral, 1, 1, null);
            var literal = new PftConditionalLiteral(token, false);
            Assert.IsNotNull(literal);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftConditionalLiteral_CompareNode_1()
        {
            var left = new PftConditionalLiteral("Hello", false);
            var right = new PftConditionalLiteral("Hello", true);
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftConditionalLiteral_Compile_1()
        {
            PftNode node = _GetVNode(200, 'a', true);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftConditionalLiteral_Compile_2()
        {
            PftNode node = _GetGNode(100, '\0', true);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionalLiteral_Compile_3()
        {
            PftNode node = new PftConditionalLiteral("text", false);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetVNode(200, 'a', false);
            _Execute(record, node, ">> Заглавие");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_2()
        {
            var record = _GetRecord();
            var node = _GetVNode(200, 'a', true);
            _Execute(record, node, "Заглавие <<");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_3()
        {
            var record = _GetRecord();
            var node = _GetVNode(201, 'a', false);
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_4()
        {
            var record = _GetRecord();
            var node = _GetVNode(201, 'a', true);
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_5()
        {
            var record = _GetRecord();
            var node = _GetGNode(100, '\0', false);
            _Execute(record, node, ">> FirstSecond");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_6()
        {
            var record = _GetRecord();
            var node = _GetGNode(100, '\0', true);
            _Execute(record, node, "FirstSecond <<");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_7()
        {
            var record = _GetRecord();
            var node = _GetGNode(101, '\0', false);
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_8()
        {
            var record = _GetRecord();
            var node = _GetGNode(101, '\0', true);
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_9()
        {
            var record = _GetRecord();
            var node = _GetVNode(300, '\0', false);
            _Execute(record, node, ">> Первое примечаниеВторое примечаниеТретье примечание");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_10()
        {
            var record = _GetRecord();
            var node = _GetVNode(300, '\0', true);
            _Execute(record, node, "Первое примечаниеВторое примечаниеТретье примечание <<");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_11()
        {
            var record = _GetRecord();
            var node = _GetVNode2(200, 'a', false);
            _ExecuteUpper(record, node, "ПРЕФИКС ЗАГЛАВИЕ");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_12()
        {
            var record = _GetRecord();
            var node = _GetVNode2(200, 'a', true);
            _ExecuteUpper(record, node, "ЗАГЛАВИЕ СУФФИКС");
        }

        [TestMethod]
        public void PftConditionalLiteral_Optimize_1()
        {
            var node = new PftConditionalLiteral();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftConditionalLiteral_Optimize_2()
        {
            var node = new PftConditionalLiteral(string.Empty, true);
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftConditionalLiteral_Optimize_3()
        {
            var node = new PftConditionalLiteral("Hello", true);
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionalLiteral_PrettyPrint_1()
        {
            var node = _GetVNode(200, 'a', true);
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("v200^a\" <<\"", printer.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_PrettyPrint_2()
        {
            var node = _GetVNode(200, 'a', false);
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\">> \"v200^a", printer.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_PrettyPrint_3()
        {
            var node = _GetGNode(100, '\0', true);
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("g100\" <<\"", printer.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_PrettyPrint_4()
        {
            var node = _GetGNode(100, '\0', false);
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\">> \"g100", printer.ToString());
        }

        private void _TestSerialization
            (
                PftNode first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionalLiteral_Serialization_1()
        {
            PftNode node = new PftRepeatableLiteral();
            _TestSerialization(node);

            node = new PftRepeatableLiteral("Hello", true, true);
            _TestSerialization(node);

            node = _GetVNode(200, 'a', true);
            _TestSerialization(node);

            node = _GetVNode2(200, 'a', true);
            _TestSerialization(node);

            node = _GetGNode(100, '\0', true);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionalLiteral_ToString_1()
        {
            var node = new PftConditionalLiteral();
            Assert.AreEqual("\"\"", node.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_ToString_2()
        {
            var node = _GetVNode(200, 'a', true);
            Assert.AreEqual("v200^a\" <<\"", node.ToString());

            node = _GetVNode(200, 'a', false);
            Assert.AreEqual("\">> \"v200^a", node.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_ToString_3()
        {
            var node = _GetGNode(100, '\0', true);
            Assert.AreEqual("g100\" <<\"", node.ToString());

            node = _GetGNode(100, '\0', false);
            Assert.AreEqual("\">> \"g100", node.ToString());
        }
    }
}
