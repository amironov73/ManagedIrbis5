﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftNTest
    {
        private void _Execute
            (
                Record? record,
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

        private PftN _GetNode
            (
                int tag,
                char code
            )
        {
            return new PftN(tag, code)
            {
                LeftHand =
                {
                    new PftConditionalLiteral("absent", false)
                }
            };
        }

        [TestMethod]
        public void PftN_Construction_1()
        {
            var node = new PftN();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('\0', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('\0', node.Command);
        }

        [TestMethod]
        public void PftN_Construction_2()
        {
            var text = "n200^a";
            var specification = new FieldSpecification(text);
            var token = new PftToken(PftTokenKind.V, 1, 1, text)
            {
                UserData = specification
            };
            var node = new PftN(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("200", node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('a', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('n', node.Command);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftN_Construction_3()
        {
            var text = "n200^a";
            var node = new PftN(text);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("200", node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('a', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('n', node.Command);
            Assert.AreEqual(text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftN_Construction_3a()
        {
            var text = "n200^";
            Assert.IsNotNull(new PftN(text));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftN_Construction_3b()
        {
            var text = "q200";
            Assert.IsNotNull(new PftN(text));
        }

        [TestMethod]
        public void PftN_Construction_4()
        {
            var node = new PftN(200, 'a');
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("200", node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('a', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('n', node.Command);
            Assert.AreEqual("n200^a", node.Text);
        }

        [TestMethod]
        public void PftN_Construction_5()
        {
            var node = new PftN(200);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("200", node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('\0', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('n', node.Command);
            Assert.AreEqual("n200", node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftN_CompareNode_1()
        {
            var left = new PftN(200, 'a');
            var right = new PftN(200, 'b');
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftN_Compile_1()
        {
            var node = new PftN(200, 'a');
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftN_Clone_1()
        {
            var left = new PftN(200, 'a');
            var right = (PftN)left.Clone();
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftN_Clone_2()
        {
            var left = _GetNode(200, 'a');
            var right = (PftN)left.Clone();
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftN_Execute_1()
        {
            var record = _GetRecord();
            var node = _GetNode(200, 'a');
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_2()
        {
            var record = _GetRecord();
            var node = _GetNode(201, 'a');
            _Execute(record, node, "absent");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftN_Execute_11()
        {
            var record = _GetRecord();
            var node = new PftV(200, 'a')
            {
                LeftHand = { new PftN(300) }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_3()
        {
            var record = _GetRecord();
            var node = new PftN(200, 'a');
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_4()
        {
            var record = _GetRecord();
            var node = new PftGroup()
            {
                Children =
                {
                    _GetNode(200, 'a'),
                    new PftSlash()
                }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_5()
        {
            var record = _GetRecord();
            var node = new PftGroup()
            {
                Children =
                {
                    _GetNode(300, '\0'),
                    new PftSlash()
                }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_6()
        {
            var record = _GetRecord();
            var node = new PftGroup()
            {
                Children =
                {
                    new PftN(300)
                    {
                        LeftHand =
                        {
                            new PftRepeatableLiteral("absent", true)
                        }
                    },
                    new PftSlash()
                }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_ToString_1()
        {
            var node = new PftN(200);
            Assert.AreEqual("n200", node.ToString());
        }
    }
}
