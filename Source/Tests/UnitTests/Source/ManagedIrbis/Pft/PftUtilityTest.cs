// ReSharper disable CheckNamespace
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Text;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftUtilityTest
    {
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
            field = new Field( 300, "Второе примечание");
            result.Fields.Add(field);
            field = new Field(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void PftUtility_AssignField_1()
        {
            var record = _GetRecord();
            var context = new PftContext(null) { Record = record };

            PftUtility.AssignField(context, 300, 1, null);
            var actual = record.FMA(300);
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("Второе примечание", actual[0].ToString());
            Assert.AreEqual("Третье примечание", actual[1].ToString());
        }

        [TestMethod]
        public void PftUtility_GetFieldCount_1()
        {
            var record = _GetRecord();
            var context = new PftContext(null)
            {
                Record = record
            };

            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 701));
            Assert.AreEqual(0, PftUtility.GetFieldCount(context, 710));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700, 710));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700, 701));
            Assert.AreEqual(3, PftUtility.GetFieldCount(context, 300));
            Assert.AreEqual(3, PftUtility.GetFieldCount(context, 300, 700));
        }

        [TestMethod]
        public void PftUtility_NodesToText_1()
        {
            PftNode[] nodes =
            {
                new PftUnconditionalLiteral("unconditional"),
                new PftConditionalLiteral("conditional", false),
                new PftRepeatableLiteral("repeatable")
            };
            var expected = "'unconditional' \"conditional\" |repeatable|";
            var builder = new StringBuilder();
            PftUtility.NodesToText(builder, nodes);
            var actual = builder.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_NodesToText_()
        {
            PftNode[] nodes =
            {
                new PftGroup
                {
                    Children =
                    {
                        new PftUnconditionalLiteral("unconditional"),
                        new PftConditionalLiteral("conditional", false)
                    }
                },
                new PftRepeatableLiteral("repeatable")
            };
            var expected = "('unconditional' \"conditional\") |repeatable|";
            var builder = new StringBuilder();
            PftUtility.NodesToText(builder, nodes);
            var actual = builder.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_NodesToText_3()
        {
            var nodes = new PftNode[0];
            var expected = "";
            var builder = new StringBuilder();
            PftUtility.NodesToText(builder, nodes);
            var actual = builder.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_1()
        {
            var actual = PftUtility.PrepareText(null);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_2()
        {
            var expected = string.Empty;
            var actual = PftUtility.PrepareText(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_3()
        {
            var expected = "There is single-line text";
            var actual = PftUtility.PrepareText(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_4()
        {
            var text = "There is multi-line\ntext";
            var expected = "There is multi-linetext";
            var actual = PftUtility.PrepareText(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_5()
        {
            var text = "There is\rmulti-line\ntext";
            var expected = "There ismulti-linetext";
            var actual = PftUtility.PrepareText(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_6()
        {
            var text = "\r\n";
            var expected = string.Empty;
            var actual = PftUtility.PrepareText(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_RequiresConnection_1()
        {
            PftNode[] nodes = { new PftNode() };
            Assert.IsTrue(PftUtility.RequiresConnection(nodes));
        }

        [TestMethod]
        public void PftUtility_RequiresConnection_2()
        {
            var mock = new Mock<PftNode>();
            var node = mock.Object;
            mock.SetupGet(n => n.RequiresConnection).Returns(false);
            mock.SetupGet(n => n.Children).Returns(new PftNodeCollection(node));
            PftNode[] nodes = { node };
            Assert.IsFalse(PftUtility.RequiresConnection(nodes));
        }

        [TestMethod]
        public void PftUtility_RequiresConnection_3()
        {
            var node = new PftNode();
            Assert.IsTrue(PftUtility.RequiresConnection(node));
        }

        [TestMethod]
        public void PftUtility_RequiresConnection_4()
        {
            var mock = new Mock<PftNode>();
            var node = mock.Object;
            mock.SetupGet(n => n.RequiresConnection).Returns(false);
            mock.SetupGet(n => n.Children).Returns(new PftNodeCollection(node));
            Assert.IsFalse(PftUtility.RequiresConnection(node));
        }

        [TestMethod]
        public void PftUtility_SetArrayItem_1()
        {
            var context = new PftContext(null);
            int[] array = { 1, 2, 3, 4 };
            var index = new IndexSpecification()
            {
                Kind = IndexKind.Literal,
                Literal = 2
            };
            var value = 123;
            var result = PftUtility.SetArrayItem(context, array, index, value);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(value, result[1]);
        }

        [TestMethod]
        public void PftUtility_SetArrayItem_2()
        {
            var context = new PftContext(null);
            int[] array = { 1, 2, 3, 4 };
            var index = new IndexSpecification()
            {
                Kind = IndexKind.Expression,
                Expression = "1+1"
            };
            var value = 123;
            var result = PftUtility.SetArrayItem(context, array, index, value);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(value, result[1]);
        }

        [TestMethod]
        public void PftUtility_SetArrayItem_3()
        {
            var context = new PftContext(null);
            int[] array = { 1, 2, 3, 4 };
            var index = new IndexSpecification()
            {
                Kind = IndexKind.None
            };
            var value = 123;
            var result = PftUtility.SetArrayItem(context, array, index, value);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(value, result[0]);
        }

        [TestMethod]
        public void PftUtility_SetArrayItem_4()
        {
            var context = new PftContext(null);
            int[] array = { 1, 2, 3, 4 };
            var index = new IndexSpecification()
            {
                Kind = IndexKind.AllRepeats
            };
            var value = 123;
            var result = PftUtility.SetArrayItem(context, array, index, value);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(value, result[0]);
            Assert.AreEqual(value, result[1]);
            Assert.AreEqual(value, result[2]);
            Assert.AreEqual(value, result[3]);
        }

        [TestMethod]
        public void PftUtility_SetArrayItem_5()
        {
            var context = new PftContext(null);
            int[] array = { 1, 2, 3, 4 };
            var index = new IndexSpecification()
            {
                Kind = IndexKind.NewRepeat
            };
            var value = 123;
            var result = PftUtility.SetArrayItem(context, array, index, value);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual(value, result[4]);
        }
    }
}
