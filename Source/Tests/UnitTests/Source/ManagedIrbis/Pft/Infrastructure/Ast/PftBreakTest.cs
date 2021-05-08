// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftBreakTest
    {
        private void _Execute
            (
                Record record,
                PftNode node,
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
        public void PftBreak_Construction_1()
        {
            var node = new PftBreak();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftBreak_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Break, 1, 1, "break");
            var node = new PftBreak(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftVariableReference_Compile_1()
        {
            PftNode node = new PftBreak();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftBreak_Execute_1()
        {
            try
            {
                var record = _GetRecord();
                var node = new PftBreak();
                _Execute(record, node, "");
            }
            catch (Exception exception)
            {
                Assert.AreEqual("PftBreakException", exception.GetType().Name);
            }
        }

        [TestMethod]
        public void PftBreak_Execute_2()
        {
            var record = _GetRecord();
            var node = new PftGroup
            {
                Children =
                {
                    new PftV(300),
                    new PftBreak(),
                    new PftUnconditionalLiteral(" == ")
                }
            };
            _Execute(record, node, "Первое примечание");
        }

        [TestMethod]
        public void PftBreak_Execute_3()
        {
            var record = _GetRecord();
            var node = new PftGroup
            {
                Children =
                {
                    new PftV(300),
                    new PftBreak(),
                    new PftUnconditionalLiteral(" == ")
                }
            };
            var saveBreak = PftConfig.BreakImmediate;
            try
            {
                PftConfig.BreakImmediate = true;
                _Execute(record, node, "Первое примечание");
            }
            finally
            {
                PftConfig.BreakImmediate = saveBreak;
            }
        }

        [TestMethod]
        public void PftBreak_Execute_4()
        {
            var record = _GetRecord();
            var node = new PftGroup
            {
                Children =
                {
                    new PftConditionalStatement
                    {
                        Condition = new PftTrue(),
                        ThenBranch =
                        {
                            new PftV(300),
                            new PftBreak(),
                            new PftUnconditionalLiteral(" == ")
                        }
                    }
                }
            };
            var saveBreak = PftConfig.BreakImmediate;
            try
            {
                PftConfig.BreakImmediate = false;
                _Execute(record, node, "Первое примечание == ");
            }
            finally
            {
                PftConfig.BreakImmediate = saveBreak;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(PftBreakException))]
        public void PftBreak_Execute_5()
        {
            var record = _GetRecord();
            var node = new PftConditionalStatement
            {
                Condition = new PftTrue(),
                ThenBranch =
                {
                    new PftV(300),
                    new PftBreak(),
                    new PftUnconditionalLiteral(" == ")
                }
            };
            var saveBreak = PftConfig.BreakImmediate;
            try
            {
                PftConfig.BreakImmediate = true;
                _Execute(record, node, "Первое примечание");
            }
            finally
            {
                PftConfig.BreakImmediate = saveBreak;
            }
        }

        [TestMethod]
        public void PftBreak_PrettyPrint_1()
        {
            var node = new PftBreak();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("break ", printer.ToString());
        }

        [TestMethod]
        public void PftBreak_ToString_1()
        {
            var node = new PftBreak();
            Assert.AreEqual("break", node.ToString());
        }
    }
}
