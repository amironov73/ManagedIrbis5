// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor4Test
        : CommonUniforTest
    {
        private void Execute
            (
                int mfn,
                string input,
                string expected
            )
        {
            using var provider = GetProvider();
            var context = new PftContext(null)
            {
                Record = provider.ReadRecord(mfn)
            };
            context.SetProvider(provider);
            var unifor = new Unifor();
            var expression = input;
            unifor.Execute(context, null, expression);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [Ignore]
        [TestMethod]
        public void Unifor4_FormatPreviousVersion_1()
        {
            Execute(1, "4", "1");
            Execute(1, "4,v200^a", "Куда пойти учиться?");
            Execute(1, "41,v200^a", "Куда пойти учиться?");
            Execute(1, "4*,v200^a", "Куда пойти учиться?");

            Execute(2, "4", "25");
            Execute(2, "4,v461^c", "Управление банком");

            // Обработка ошибок
            Execute(1, "4Q", "");
            Execute(1, "4,", "");
        }
    }
}
