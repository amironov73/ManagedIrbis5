﻿// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class IndexSpecificationTest
    {
        [TestMethod]
        public void IndexSpecification_Construction_1()
        {
            var specification = new IndexSpecification();
            Assert.AreEqual(IndexKind.None, specification.Kind);
            Assert.AreEqual(0, specification.Literal);
            Assert.IsNull(specification.Expression);
            Assert.IsNull(specification.Program);
        }

        [TestMethod]
        public void IndexSpecification_Compare_1()
        {
            var left = new IndexSpecification();
            var right = new IndexSpecification();
            Assert.IsTrue(IndexSpecification.Compare(left, right));

            right = new IndexSpecification {Kind = IndexKind.Literal};
            Assert.IsFalse(IndexSpecification.Compare(left, right));

            left = new IndexSpecification {Kind = IndexKind.Literal, Literal = 10};
            Assert.IsFalse(IndexSpecification.Compare(left, right));
        }

        [TestMethod]
        public void IdexSpecification_ComputeValue_1()
        {
            var context = new PftContext(null) {Index = 4};
            context.Variables.SetVariable("x", 2.0);
            int[] array = {101, 102, 103, 104, 105};
            var specification = new IndexSpecification();
            Assert.AreEqual(0, specification.ComputeValue(context, array));

            specification = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = 3
            };
            Assert.AreEqual(2, specification.ComputeValue(context, array));

            specification = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = -2
            };
            Assert.AreEqual(3, specification.ComputeValue(context, array));

            specification = new IndexSpecification
            {
                Kind = IndexKind.LastRepeat
            };
            Assert.AreEqual(4, specification.ComputeValue(context, array));

            specification = new IndexSpecification
            {
                Kind = IndexKind.NewRepeat
            };
            Assert.AreEqual(5, specification.ComputeValue(context, array));

            specification = new IndexSpecification
            {
                Kind = IndexKind.CurrentRepeat
            };
            Assert.AreEqual(4, specification.ComputeValue(context, array));

            specification = new IndexSpecification
            {
                Kind = IndexKind.AllRepeats
            };
            Assert.AreEqual(0, specification.ComputeValue(context, array));

            specification = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "4"
            };
            Assert.AreEqual(3, specification.ComputeValue(context, array));

            specification = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "$x"
            };
            Assert.AreEqual(1, specification.ComputeValue(context, array));
        }

        [TestMethod]
        public void IdexSpecification_ComputeValue_2()
        {
            var context = new PftContext(null) {Index = 4};
            context.Variables.SetVariable("x", 2.0);
            int[] array = {101, 102, 103, 104, 105};
            var specification = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "$x"
            };
            Assert.IsNull(specification.Program);
            // Force to compute the expression
            Assert.AreEqual(1, specification.ComputeValue(context, array));
            Assert.IsNotNull(specification.Program);
            var firstProgram = specification.Program;

            // Recompute the expression
            Assert.AreEqual(1, specification.ComputeValue(context, array));
            // Ensure the program hasn't changed
            Assert.AreSame(firstProgram, specification.Program);
        }

        private void _TestSerialization
        (
            IndexSpecification first
        )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            first.Serialize(writer);
            var memory = stream.ToArray();

            var second = new IndexSpecification();
            stream = new MemoryStream(memory);
            var reader = new BinaryReader(stream);
            second.Deserialize(reader);

            Assert.IsTrue(IndexSpecification.Compare(first, second));
        }

        [TestMethod]
        public void IndexSpecification_Serialization_1()
        {
            var specification = new IndexSpecification();
            _TestSerialization(specification);

            specification = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = 3
            };
            _TestSerialization(specification);

            specification = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = -2
            };
            _TestSerialization(specification);

            specification = new IndexSpecification
            {
                Kind = IndexKind.LastRepeat
            };
            _TestSerialization(specification);

            specification = new IndexSpecification
            {
                Kind = IndexKind.NewRepeat
            };
            _TestSerialization(specification);

            specification = new IndexSpecification
            {
                Kind = IndexKind.CurrentRepeat
            };
            _TestSerialization(specification);

            specification = new IndexSpecification
            {
                Kind = IndexKind.AllRepeats
            };
            _TestSerialization(specification);

            specification = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "4"
            };
            _TestSerialization(specification);

            specification = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "$x"
            };
            _TestSerialization(specification);
        }

        [TestMethod]
        public void IndexSpecification_GetNodeInfo_1()
        {
            var specification = new IndexSpecification();
            var info = specification.GetNodeInfo();
            Assert.AreEqual("Index", info.Name);
            Assert.AreEqual("Kind", info.Children[0].Name);
            Assert.AreEqual("None", info.Children[0].Value);

            specification = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = 10
            };
            info = specification.GetNodeInfo();
            Assert.AreEqual("Index", info.Name);
            Assert.AreEqual("Kind", info.Children[0].Name);
            Assert.AreEqual("Literal", info.Children[0].Value);
            Assert.AreEqual("Literal", info.Children[1].Name);
            Assert.AreEqual("10", info.Children[1].Value);

            specification = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "$x + $y"
            };
            info = specification.GetNodeInfo();
            Assert.AreEqual("Index", info.Name);
            Assert.AreEqual("Kind", info.Children[0].Name);
            Assert.AreEqual("Expression", info.Children[0].Value);
            Assert.AreEqual("Expression", info.Children[1].Name);
            Assert.AreEqual("$x + $y", info.Children[1].Value);
        }

        [TestMethod]
        public void IndexSpecification_ToText_1()
        {
            var specification = new IndexSpecification();
            Assert.AreEqual("", specification.ToText());

            specification = new IndexSpecification
            {
                Kind = IndexKind.AllRepeats
            };
            Assert.AreEqual("[-]", specification.ToText());

            specification = new IndexSpecification
            {
                Kind = IndexKind.CurrentRepeat
            };
            Assert.AreEqual("[.]", specification.ToText());

            specification = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "$x + $y"
            };
            Assert.AreEqual("[$x + $y]", specification.ToText());

            specification = new IndexSpecification
            {
                Kind = IndexKind.LastRepeat
            };
            Assert.AreEqual("[*]", specification.ToText());

            specification = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = 10
            };
            Assert.AreEqual("[10]", specification.ToText());

            specification = new IndexSpecification
            {
                Kind = IndexKind.NewRepeat
            };
            Assert.AreEqual("[+]", specification.ToText());
        }

        [TestMethod]
        public void IndexSpecification_Clone_1()
        {
            var first = new IndexSpecification();
            var second = (IndexSpecification) first.Clone();
            Assert.IsTrue(IndexSpecification.Compare(first, second));

            first = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = 10
            };
            second = (IndexSpecification) first.Clone();
            Assert.IsTrue(IndexSpecification.Compare(first, second));

            first = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "3"
            };
            second = (IndexSpecification) first.Clone();
            Assert.IsTrue(IndexSpecification.Compare(first, second));
        }

        [TestMethod]
        public void IndexSpecification_Clone_2()
        {
            var context = new PftContext(null);
            int[] array = {101, 102, 103, 104, 105};
            var first = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "3"
            };
            // Force to compute the expression
            Assert.AreEqual(2, first.ComputeValue(context, array));
            Assert.IsNotNull(first.Program);

            var second = (IndexSpecification) first.Clone();
            Assert.IsTrue(IndexSpecification.Compare(first, second));
            Assert.IsNull(second.Program);
        }

        [TestMethod]
        public void IndexSpecification_ToString_1()
        {
            var specification = new IndexSpecification();
            Assert.AreEqual("None", specification.ToString());

            specification = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = 10
            };
            Assert.AreEqual("Literal: 10", specification.ToString());

            specification = new IndexSpecification
            {
                Kind = IndexKind.Expression,
                Expression = "$x + $y"
            };
            Assert.AreEqual("Expression: $x + $y", specification.ToString());
        }

        [TestMethod]
        public void IndexSpecification_GetAll_1()
        {
            var specification = IndexSpecification.GetAll();
            Assert.AreEqual(IndexKind.AllRepeats, specification.Kind);
        }

        [TestMethod]
        public void IndexSpecification_GetLiteral_1()
        {
            const int index = 123;
            var specification = IndexSpecification.GetLiteral(index);
            Assert.AreEqual(IndexKind.Literal, specification.Kind);
            Assert.AreEqual(index, specification.Literal);
        }

        [TestMethod]
        public void IndexSpecification_op_Implicit_1()
        {
            const int index = 123;
            IndexSpecification specification = index;
            Assert.AreEqual(IndexKind.Literal, specification.Kind);
            Assert.AreEqual(index, specification.Literal);
        }
    }
}
