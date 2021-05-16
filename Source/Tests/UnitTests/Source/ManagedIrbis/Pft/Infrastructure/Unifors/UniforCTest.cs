// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforCTest
    {
        private void _C
            (
                string input,
                string expected
            )
        {
            var context = new PftContext(null);
            var unifor = new Unifor();
            var expression = input;
            unifor.Execute(context, null, expression);
            var actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforC_CheckIsbn_1()
        {
            // Пустой ISBN считается правильным
            _C("C", "1");

            _C("C5-02-003157-7", "0");
            _C("C5-02-003206-9", "0");
            _C("C5-02-003206-1", "1");

            // TODO: починить EAN-7
            // _C("C0033-765X", "0");
            // _C("C0033-7651", "1");
        }
    }
}
