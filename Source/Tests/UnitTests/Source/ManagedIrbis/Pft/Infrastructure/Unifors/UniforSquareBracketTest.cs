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
    public class UniforSquareBracketTest
        : CommonUniforTest
    {
        private void _TestCleanup
            (
                string input,
                string expected
            )
        {
            using var provider = GetProvider();
            var context = new PftContext(null);
            context.SetProvider(provider);
            context.Write(null, input);
            var unifor = new Unifor();
            unifor.Execute(context, null, "[");
            var actual = context.GetProcessedOutput();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforSquareBracket_CleanContextMarkup_1()
        {
            _TestCleanup
                (
                    "Вот [[b]]жирный[[/b]] текст, а вот [[i]]курсивный[[/i]] текст",
                    "Вот жирный текст, а вот курсивный текст"
                );

            _TestCleanup
                (
                    "Вот [[b]]жирный [[i]]курсивный[[/i]][[/b]] текст",
                    "Вот жирный курсивный текст"
                );

            _TestCleanup("no markup", "no markup");

            // Обработка ошибок
            _TestCleanup("", "");
            _TestCleanup("[[no markup", "[[no markup");
        }
    }
}
