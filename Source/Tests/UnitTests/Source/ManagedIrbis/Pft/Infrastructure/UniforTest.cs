// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class UniforTest
    {
        private void _Execute
            (
                Record record,
                string expression,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            var node = new PftNode();
            var unifor = new Unifor();
            unifor.Execute(context, node, expression);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private Record _GetRecord()
        {
            var result = new Record();
            result.Fields.Add(new Field(1, "11|22|33|44"));
            result.Fields.Add(new Field(2, "^A11^B00^A22^B99^A33"));
            result.Fields.Add(new Field(3, "^1100^AQQQ^BWWW^1200^AEEE^BRRR"));
            result.Fields.Add(new Field(4));

            return result;
        }

        [TestMethod]
        public void Unifor_Construction_1()
        {
            var unifor = new Unifor();
            Assert.AreEqual("unifor", unifor.Name);

            Assert.IsNotNull(Unifor.Registry);
            Assert.IsTrue(Unifor.Registry.Count > 0);
            Assert.IsFalse(Unifor.ThrowOnUnknown);
            Assert.IsFalse(Unifor.ThrowOnEmpty);
        }

        [TestMethod]
        public void Unifor_FindAction_1()
        {
            var expression = "+9V";
            var action = Unifor.FindAction(ref expression);
            Assert.IsNotNull(action);
        }

        [TestMethod]
        public void Unifor_Execute_1()
        {
            var record = _GetRecord();
            var expression = "+9V";
            var expected = "64";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void Unifor_Execute_2()
        {
            var save = Unifor.ThrowOnUnknown;
            try
            {
                Unifor.ThrowOnUnknown = true;
                var record = _GetRecord();
                var expression = "*";
                var expected = "";
                _Execute(record, expression, expected);
            }
            finally
            {
                Unifor.ThrowOnUnknown = save;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void Unifor_Execute_3()
        {
            var save = Unifor.ThrowOnEmpty;
            try
            {
                Unifor.ThrowOnEmpty = true;
                var record = _GetRecord();
                var expression = "";
                var expected = "";
                _Execute(record, expression, expected);
            }
            finally
            {
                Unifor.ThrowOnEmpty = save;
            }
        }

        [TestMethod]
        public void Unifor_Execute_3a()
        {
            var save = Unifor.ThrowOnEmpty;
            try
            {
                Unifor.ThrowOnEmpty = false;
                var record = _GetRecord();
                var expression = "";
                var expected = "";
                _Execute(record, expression, expected);
            }
            finally
            {
                Unifor.ThrowOnEmpty = save;
            }
        }
    }
}
