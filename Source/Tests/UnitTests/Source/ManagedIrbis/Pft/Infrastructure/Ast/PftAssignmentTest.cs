// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM.Text;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftAssignmentTest
    {
        private void _Execute
            (
                PftAssignment node,
                string expected
            )
        {
            var context = new PftContext(null);
            context.Variables.SetVariable("y1", 3.14);
            context.Variables.SetVariable("y2", "hello");
            node.Execute(context);
            var variable = context.Variables.GetExistingVariable(node.VaruableName!);
            Assert.IsNotNull(variable);
            var actual = variable!.StringValue.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private void _Execute
            (
                PftAssignment node,
                double expected
            )
        {
            var context = new PftContext(null);
            context.Variables.SetVariable("y1", 3.14);
            context.Variables.SetVariable("y2", "hello");
            node.Execute(context);
            var variable = context.Variables.GetExistingVariable(node.VaruableName!);
            Assert.IsNotNull(variable);
            var actual = variable!.NumericValue;
            Assert.AreEqual(expected, actual);
        }

        private PftAssignment _GetNumericAssignment()
        {
            return new ()
            {
                IsNumeric = true,
                VaruableName = "x",
                Children =
                {
                    new PftNumericExpression
                    {
                        LeftOperand = new PftNumericLiteral(1),
                        Operation = "+",
                        RightOperand = new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(2),
                            Operation = "*",
                            RightOperand = new PftNumericLiteral(3)
                        }
                    }
                }
            };
        }

        private PftAssignment _GetDirectAssignment_1()
        {
            return new ()
            {
                IsNumeric = true,
                VaruableName = "x",
                Children =
                {
                    new PftVariableReference("y1")
                }
            };
        }

        private PftAssignment _GetDirectAssignment_2()
        {
            return new ()
            {
                IsNumeric = false,
                VaruableName = "x",
                Children =
                {
                    new PftVariableReference("y2")
                }
            };
        }

        private PftAssignment _GetStringAssignment()
        {
            return new ()
            {
                IsNumeric = false,
                VaruableName = "x",
                Children =
                {
                    new PftUnconditionalLiteral("Hello,"),
                    new PftComma(),
                    new PftUnconditionalLiteral(" world!")
                }
            };
        }

        [TestMethod]
        public void PftAssignment_Construction_1()
        {
            var node = new PftAssignment();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.VaruableName);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftAssignment_Construction_2()
        {
            var name = "x";
            var token = new PftToken(PftTokenKind.Equals, 1, 1, name);
            var node = new PftAssignment(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.AreSame(name, node.VaruableName);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftAssignment_Construction_3()
        {
            var name = "x";
            var node = new PftAssignment(name);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.AreSame(name, node.VaruableName);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftAssignment_Construction_4()
        {
            var name = "x";
            var body = new PftNode[]
            {
                new PftUnconditionalLiteral("Hello,"),
                new PftComma(),
                new PftUnconditionalLiteral(" world!")
            };
            var node = new PftAssignment(false, name, body);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.AreSame(name, node.VaruableName);
            Assert.AreEqual(body.Length, node.Children.Count);
        }

        [TestMethod]
        public void PftAssignment_Clone_1()
        {
            var first = new PftAssignment();
            var second = (PftAssignment) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAssignment_Clone_2()
        {
            var first = _GetNumericAssignment();
            var second = (PftAssignment)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAssignment_Clone_3()
        {
            var first = _GetStringAssignment();
            var second = (PftAssignment)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftAssignment_Clone_4()
        {
            var first = _GetStringAssignment();
            var second = (PftAssignment)first.Clone();
            second.IsNumeric = true;
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftWhile_Compile_1()
        {
            var program = new PftProgram();
            var node = _GetNumericAssignment();
            program.Children.Add(node);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftWhile_Compile_2()
        {
            var program = new PftProgram();
            var node = _GetStringAssignment();
            program.Children.Add(node);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftWhile_Compile_3()
        {
            var program = new PftProgram();
            var node = new PftAssignment();
            program.Children.Add(node);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftWhile_Compile_4()
        {
            var program = new PftProgram();
            var node = _GetStringAssignment();
            node.Children.Clear();
            program.Children.Add(node);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftAssignment_Execute_1()
        {
            var node = new PftAssignment();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftAssignment_Execute_2()
        {
            var node = _GetNumericAssignment();
            _Execute(node, 7);
        }

        [TestMethod]
        public void PftAssignment_Execute_3()
        {
            var node = _GetStringAssignment();
            _Execute(node, "Hello, world!");
        }

        [TestMethod]
        public void PftAssignment_Execute_4()
        {
            var node = _GetDirectAssignment_1();
            _Execute(node, 3.14);
        }

        [TestMethod]
        public void PftAssignment_Execute_5()
        {
            var node = _GetDirectAssignment_2();
            _Execute(node, "hello");
        }

        [TestMethod]
        public void PftAssignment_GetNodeInfo_1()
        {
            var node = _GetNumericAssignment();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Assignment", info.Name);
        }

        [TestMethod]
        public void PftAssignment_GetNodeInfo_2()
        {
            var node = _GetStringAssignment();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Assignment", info.Name);
        }

        [TestMethod]
        public void PftAssignment_GetNodeInfo_3()
        {
            var node = _GetStringAssignment();
            node.Index = IndexSpecification.GetLiteral(5);
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Assignment", info.Name);
        }

        [TestMethod]
        public void PftAssignment_PrettyPrint_1()
        {
            var node = _GetNumericAssignment();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("$x=1 + 2 * 3;", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftAssignment_PrettyPrint_2()
        {
            var node = _GetStringAssignment();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("$x='Hello,', ' world!';", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftAssignment first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftAssignment) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAssignment_Serialization_1()
        {
            var node = new PftAssignment();
            _TestSerialization(node);

            node = _GetNumericAssignment();
            _TestSerialization(node);

            node = _GetStringAssignment();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftAssignment_ToString_1()
        {
            var node = new PftAssignment();
            Assert.AreEqual("$=;", node.ToString());
        }

        [TestMethod]
        public void PftAssignment_ToString_2()
        {
            var node = _GetNumericAssignment();
            Assert.AreEqual("$x=1+2*3;", node.ToString());
        }

        [TestMethod]
        public void PftAssignment_ToString_3()
        {
            var node = _GetStringAssignment();
            Assert.AreEqual("$x='Hello,' , ' world!';", node.ToString());
        }
    }
}
