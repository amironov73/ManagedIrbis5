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
    public class UniforQTest
    {
        private void _Q
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
        public void UniforQ_ToLower_1()
        {
            _Q("Q", "");
            _Q("QУнылая, пора!", "унылая, пора!");
            _Q("Q1234567890", "1234567890");
        }
    }
}
