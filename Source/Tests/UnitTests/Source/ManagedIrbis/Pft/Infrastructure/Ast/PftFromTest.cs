// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftFromTest
    {
        private void _Execute
            (
                Record record,
                PftFrom node,
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

        private static Field _Parse(int tag, string body)
        {
            var result = new Field(tag);
            result.DecodeBody(body);

            return result;
        }

        private Record _GetRecord()
        {
            var record = new Record();
            record.Fields.Add(_Parse(910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
            record.Fields.Add(_Parse(910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
            record.Fields.Add(_Parse(910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
            record.Fields.Add(_Parse(910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
            record.Fields.Add(_Parse(910, "^A0^B559^C19990924^H107256G^=2^U2004/7"));
            record.Fields.Add(_Parse(910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60"));
            record.Fields.Add(_Parse(910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));

            return record;
        }

        private PftFrom _GetNode()
        {
            var name = "x";
            return new PftFrom
            {
                Variable = new PftVariableReference(name),
                Source =
                {
                    new PftGroup
                        {
                            Children =
                            {
                                new PftV("v910^b"),
                                new PftSlash()
                            }
                        }
                },
                Where = new PftComparison
                {
                    LeftOperand = new PftVariableReference(name),
                    Operation = ":",
                    RightOperand = new PftUnconditionalLiteral("55")
                },
                Select =
                {
                    new PftUnconditionalLiteral("=> "),
                    new PftVariableReference(name)
                },
                Order =
                {
                    new PftVariableReference(name)
                }
            };
        }

        [TestMethod]
        public void PftFrom_Construction_1()
        {
            var node = new PftFrom();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Variable);
            Assert.IsNotNull(node.Source);
            Assert.AreEqual(0, node.Source.Count);
            Assert.IsNull(node.Where);
            Assert.IsNotNull(node.Select);
            Assert.AreEqual(0, node.Select.Count);
            Assert.IsNotNull(node.Order);
            Assert.AreEqual(0, node.Order.Count);
        }

        [TestMethod]
        public void PftFrom_Construction_2()
        {
            var token = new PftToken(PftTokenKind.From, 1, 1, "from");
            var node = new PftFrom(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Variable);
            Assert.IsNotNull(node.Source);
            Assert.AreEqual(0, node.Source.Count);
            Assert.IsNull(node.Where);
            Assert.IsNotNull(node.Select);
            Assert.AreEqual(0, node.Select.Count);
            Assert.IsNotNull(node.Order);
            Assert.AreEqual(0, node.Order.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFrom_Clone_1()
        {
            var first = new PftFrom();
            var second = (PftFrom) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFrom_Clone_2()
        {
            var first = _GetNode();
            var second = (PftFrom)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFrom_Compile_1()
        {
            var node = _GetNode();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        [Description("Пустая нода не должна компилироваться")]
        public void PftFrom_Compile_2()
        {
            var node = new PftFrom();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftFrom_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetNode();
            _Execute(record, node, "=> 556\n=> 557\n=> 558\n=> 559");
        }

        [TestMethod]
        public void PftFrom_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("From", info.Name);
        }

        [TestMethod]
        public void PftFrom_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nfrom $x in (v910^b / )\nwhere $x:\'55\'\nselect \'=> \', $x\norder $x\n\nend\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftFrom_PrettyPrint_2()
        {
            var node = _GetNode();
            node.Source.Add(new PftUnconditionalLiteral("Hello"));
            node.Order.Add(new PftUnconditionalLiteral("Garbage"));
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nfrom $x in (v910^b / ), 'Hello'\nwhere $x:\'55\'\nselect \'=> \', $x\norder $x, 'Garbage'\n\nend\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftFrom first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftFrom) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFrom_Serialization_1()
        {
            var node = new PftFrom();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftFrom_ToString_1()
        {
            var node = new PftFrom();
            Assert.AreEqual("from  in  select  end", node.ToString());
        }

        [TestMethod]
        public void PftFrom_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("from $x in (v910^b /) where $x:'55' select '=> ' $x order $x end", node.ToString());
        }
    }
}
