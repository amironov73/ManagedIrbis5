// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

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
    public class PftForTest
    {
        private void _Execute
            (
                Record record,
                PftFor node,
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

        private PftFor _GetNode()
        {
            var name = "i";
            return new PftFor
            {
                Initialization =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        VaruableName = name,
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
                        VaruableName = name,
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
        public void PftFor_Construction_1()
        {
            var node = new PftFor();
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
        public void PftFor_Construction_2()
        {
            var token = new PftToken(PftTokenKind.For, 1, 1, "for");
            var node = new PftFor(token);
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
        public void PftFor_Clone_1()
        {
            var first = new PftFor();
            var second = (PftFor)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFor_Clone_2()
        {
            var first = _GetNode();
            var second = (PftFor)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFor_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetNode();
            _Execute
                (
                    record,
                    node,
                    "Строка 1\nСтрока 2\nСтрока 3\nСтрока 4\nСтрока 5\nСтрока 6\nСтрока 7\nСтрока 8\nСтрока 9\nСтрока 10\n"
                );
        }

        [TestMethod]
        public void PftFor_Execute_2()
        {
            var record = _GetRecord();
            var node = _GetNode();
            node.Body.Add(new PftBreak());
            _Execute
                (
                    record,
                    node,
                    "Строка 1\n"
                );
        }

        [TestMethod]
        public void PftFor_Execute_3()
        {
            // Вложенные циклы

            string outer = "i", inner = "j";
            var record = _GetRecord();
            var node = new PftFor
            {
                Initialization =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        VaruableName = outer,
                        Children =
                        {
                            new PftNumericLiteral(1)
                        }
                    }
                },
                Condition = new PftComparison
                {
                    LeftOperand = new PftVariableReference(outer),
                    Operation = "<=",
                    RightOperand = new PftNumericLiteral(3)
                },
                Loop =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        VaruableName = outer,
                        Children =
                        {
                            new PftNumericExpression
                            {
                                LeftOperand = new PftVariableReference(outer),
                                Operation = "+",
                                RightOperand = new PftNumericLiteral(1)
                            }
                        }
                    }
                },
                Body =
                {
                    new PftFor
                    {
                        Initialization =
                        {
                            new PftAssignment
                            {
                                IsNumeric = true,
                                VaruableName = inner,
                                Children =
                                {
                                    new PftNumericLiteral(1)
                                }
                            }
                        },
                        Condition = new PftComparison
                        {
                            LeftOperand = new PftVariableReference(inner),
                            Operation = "<=",
                            RightOperand = new PftNumericLiteral(3)
                        },
                        Loop =
                        {
                            new PftAssignment
                            {
                                IsNumeric = true,
                                VaruableName = inner,
                                Children =
                                {
                                    new PftNumericExpression
                                    {
                                        LeftOperand = new PftVariableReference(inner),
                                        Operation = "+",
                                        RightOperand = new PftNumericLiteral(1)
                                    }
                                }
                            }
                        },
                        Body =
                        {
                            new PftVariableReference(outer),
                            new PftUnconditionalLiteral("=>"),
                            new PftVariableReference(inner),
                            new PftSlash()
                        }
                    },
                    new PftUnconditionalLiteral("=================="),
                    new PftSlash()
                }
            };
            _Execute
                (
                    record,
                    node,
                    "1=>1\n1=>2\n1=>3\n==================\n" +
                    "2=>1\n2=>2\n2=>3\n==================\n" +
                    "3=>1\n3=>2\n3=>3\n==================\n"
                );
        }

        [TestMethod]
        public void PftFor_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("For", info.Name);
        }

        [TestMethod]
        public void PftFor_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nfor $i=1;; $i<=10; $i=$i + 1;;\ndo\n  \'Строка \'$i /\nend\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftFor first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftFor)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFor_Serialization_1()
        {
            var node = new PftFor();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftFor_ToString_1()
        {
            var node = new PftFor();
            Assert.AreEqual("for ;; do  end", node.ToString());
        }

        [TestMethod]
        public void PftFor_ToString_2()
        {
            var node = _GetNode();

            // TODO FIX THIS!
            Assert.AreEqual("for $i=1;;$i<=10;$i=$i+1; do 'Строка ' $i / end", node.ToString());
        }
    }
}
