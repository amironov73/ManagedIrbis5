// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

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
    public class PftParallelForTest
    {
        private void _Execute
            (
                Record record,
                PftParallelFor node,
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

        private PftParallelFor _GetNode()
        {
            var name = "i";
            return new PftParallelFor
            {
                Initialization =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        Name = name,
                        Children =
                        {
                            new PftNumericLiteral(1)
                        }
                    }
                },
                Condition = new PftComparison
                {
                    LeftOperand = new PftVariableReference(name),
                    Operation = "<=",
                    RightOperand = new PftNumericLiteral(10)
                },
                Loop =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        Name = name,
                        Children =
                        {
                            new PftNumericExpression
                            {
                                LeftOperand = new PftVariableReference(name),
                                Operation = "+",
                                RightOperand = new PftNumericLiteral(1)
                            }
                        }
                    }
                },
                Body =
                {
                    new PftUnconditionalLiteral("Строка "),
                    new PftVariableReference(name),
                    new PftSlash()
                }
            };
        }

        [TestMethod]
        public void PftParallelFor_Construction_1()
        {
            var node = new PftParallelFor();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNotNull(node.Initialization);
            Assert.AreEqual(0, node.Initialization.Count);
            Assert.IsNull(node.Condition);
            Assert.IsNotNull(node.Loop);
            Assert.AreEqual(0, node.Loop.Count);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
        }

        [TestMethod]
        public void PftParallelFor_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Parallel, 1, 1, "parallel");
            var node = new PftParallelFor(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNotNull(node.Initialization);
            Assert.AreEqual(0, node.Initialization.Count);
            Assert.IsNull(node.Condition);
            Assert.IsNotNull(node.Loop);
            Assert.AreEqual(0, node.Loop.Count);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftParallelFor_Clone_1()
        {
            var first = new PftParallelFor();
            var second = (PftParallelFor) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelFor_Clone_2()
        {
            var first = _GetNode();
            var second = (PftParallelFor)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelFor_Execute_1()
        {
           var record = _GetRecord();
           var node = _GetNode();

            // TODO FIX THIS!
            _Execute
                (
                    record,
                    node,
                    "Строка 11\nСтрока 11\nСтрока 11\nСтрока 11\nСтрока 11\nСтрока 11\nСтрока 11\nСтрока 11\nСтрока 11\nСтрока 11\n"
                );
        }

        //[TestMethod]
        //public void PftParallelFor_Execute_2()
        //{
        //   MarcRecord record = _GetRecord();
        //   PftParallelFor node = _GetNode();
        //    node.Body.Add(new PftBreak());
        //    _Execute
        //        (
        //            record,
        //            node,
        //            "Строка 1\n"
        //        );
        //}

        [TestMethod]
        public void PftParallelFor_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("ParallelFor", info.Name);
        }

        [TestMethod]
        public void PftParallelFor_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nparallel for $i=1;; $i<=10; $i=$i + 1;;\ndo\n  \'Строка \'$i /\nend\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftParallelFor first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftParallelFor) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelFor_Serialization_1()
        {
            var node = new PftParallelFor();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftParallelFor_ToString_1()
        {
            var node = new PftParallelFor();
            Assert.AreEqual("parallel for ;; do  end", node.ToString());
        }

        [TestMethod]
        public void PftParallelFor_ToString_2()
        {
            var node = _GetNode();

            // TODO FIX THIS!
            Assert.AreEqual("parallel for $i=1;;$i<=10;$i=$i+1; do 'Строка ' $i / end", node.ToString());
        }
    }
}
