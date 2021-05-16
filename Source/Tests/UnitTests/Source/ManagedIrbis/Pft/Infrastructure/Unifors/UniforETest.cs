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
    public class UniforETest
    {
        private void _E
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
        public void UniforE_GetFirstWords_1()
        {
            _E("E3Съешь ещё этих мягких французских булок", "Съешь ещё этих");
            _E("E0Съешь ещё этих мягких французских булок", "Съешь ещё этих мягких французских булок");
            _E("E100Съешь ещё этих мягких французских булок", "00Съешь");
        }
    }
}
