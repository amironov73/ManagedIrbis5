// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftNestedTest
    {
        private void _Execute
            (
                Record mainRecord,
                Record alternativeRecord,
                PftNode node,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = mainRecord,
                AlternativeRecord = alternativeRecord
            };
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private Record _GetMainRecord()
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

        private Record _GetAlternativeRecord()
        {
            var record = new Record();
            record.Fields.Add(new Field(900, "^A0^B32"));

            return record;
        }

        private PftNested _GetNode()
        {
            return new PftNested
            {
                Children =
                {
                    new PftUnconditionalLiteral(" Alternative: "),
                    new PftV("v900^b")
                }
            };
        }

        [TestMethod]
        public void PftNested_Construction_1()
        {
            var node = new PftNested();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftNested_Construction_2()
        {
            var token = new PftToken(PftTokenKind.LeftCurly, 1, 1, "{");
            var node = new PftNested(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftNested_Execute_1()
        {
            var mainRecord = _GetMainRecord();
            var alternativeRecord = _GetAlternativeRecord();
            var program = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Main: "),
                    new PftV("v200^a"),
                    _GetNode(),
                    new PftUnconditionalLiteral(" Main again: "),
                    new PftV("v200^e")
                }
            };
            var expected = "Main: Заглавие Alternative: 32 Main again: подзаголовочное";
            _Execute(mainRecord, alternativeRecord, program, expected);
        }

        [TestMethod]
        public void PftNested_PrettyPrint_1()
        {
            var printer = new PftPrettyPrinter();
            var node = _GetNode();
            node.PrettyPrint(printer);
            Assert.AreEqual("{' Alternative: ' v900^b} ", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftNested_ToString_1()
        {
            var node = new PftNested();
            Assert.AreEqual("{}", node.ToString());
        }

        [TestMethod]
        public void PftNested_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("{' Alternative: ' v900^b}", node.ToString());
        }
    }
}
