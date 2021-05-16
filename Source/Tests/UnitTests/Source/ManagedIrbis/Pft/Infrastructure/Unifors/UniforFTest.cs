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
    public class UniforFTest
    {
        private void _F
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
        public void UniforF_GetLastWords_1()
        {
            _F("F3Съешь ещё этих мягких французских булок", " мягких французских булок");
            _F("F0Съешь ещё этих мягких французских булок", "");
            _F("F100Съешь ещё этих мягких французских булок", " ещё этих мягких французских булок");
        }
    }
}
