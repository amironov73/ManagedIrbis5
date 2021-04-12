// ReSharper disable CheckNamespace
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
    public class PftFieldAssignmentTest
    {
        private void _Execute
            (
                Record record,
                PftFieldAssignment node,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            var actual = record.FM(int.Parse(node.Field!.Tag!));
            Assert.AreEqual(expected, actual.ToString());
        }

        private static Field Parse
            (
                int tag,
                string value
            )
        {
            var result = new Field {Tag = tag};
            result.DecodeBody(value);

            return result;
        }

        private Record _GetRecord()
        {
            var record = new Record();
            record.Fields.Add(Parse(910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
            record.Fields.Add(Parse(910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
            record.Fields.Add(Parse(910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
            record.Fields.Add(Parse(910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
            record.Fields.Add(Parse(910, "^A0^B559^C19990924^H107256G^=2^U2004/7"));
            record.Fields.Add(Parse(910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60"));
            record.Fields.Add(Parse(910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));

            return record;
        }

        private PftFieldAssignment _GetNode()
        {
            return new PftFieldAssignment
            {
                Field = new PftV("v300"),
                Expression =
                {
                    new PftUnconditionalLiteral("Hello,"),
                    new PftComma(),
                    new PftUnconditionalLiteral(" world!")
                }
            };
        }

        [TestMethod]
        public void PftFieldAssignment_Construction_1()
        {
            var node = new PftFieldAssignment();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Field);
            Assert.IsNotNull(node.Expression);
            Assert.AreEqual(0, node.Expression.Count);
        }

        [TestMethod]
        public void PftFieldAssignment_Construction_2()
        {
            var field = "200";
            var token = new PftToken(PftTokenKind.V, 1, 1, "v" + field);
            var node = new PftFieldAssignment(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.AreEqual(field, node.Field!.Tag);
            Assert.IsNotNull(node.Expression);
            Assert.AreEqual(0, node.Expression.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFieldAssignment_Construction_3()
        {
            var field = "200";
            var node = new PftFieldAssignment("v" + field);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.AreEqual(field, node.Field!.Tag);
            Assert.IsNotNull(node.Expression);
            Assert.AreEqual(0, node.Expression.Count);
        }

        [TestMethod]
        public void PftFieldAssignment_Clone_1()
        {
            var first = new PftFieldAssignment();
            var second = (PftFieldAssignment) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFieldAssignment_Clone_2()
        {
            var first = _GetNode();
            var second = (PftFieldAssignment)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftFieldAssignment_Execute_1()
        {
            var record = _GetRecord();
            var node = new PftFieldAssignment();
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftFieldAssignment_Execute_2()
        {
            var record = _GetRecord();
            var node = _GetNode();
            _Execute(record, node, "Hello, world!");
        }

        [TestMethod]
        public void PftFieldAssignment_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("FieldAssignment", info.Name);
        }

        [TestMethod]
        public void PftFieldAssignment_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("v300='Hello,', ' world!';", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftFieldAssignment first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftFieldAssignment) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFieldAssignment_Serialization_1()
        {
            var node = new PftFieldAssignment();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftFieldAssignment_ToString_1()
        {
            var node = new PftFieldAssignment();
            Assert.AreEqual("=;", node.ToString());
        }

        [TestMethod]
        public void PftFieldAssignment_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("v300='Hello,' , ' world!';", node.ToString());
        }
    }
}
