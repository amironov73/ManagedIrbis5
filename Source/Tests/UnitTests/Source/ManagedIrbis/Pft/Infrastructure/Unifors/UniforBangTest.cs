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
    public class UniforBangTest
        : CommonUniforTest
    {
        private void _Execute
            (
                string input,
                string expected
            )
        {
            var context = new PftContext(null);
            context.Output.Write(input);
            var unifor = new Unifor();
            unifor.Execute(context, null, "!");
            var actual = context.GetProcessedOutput();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforBang_CleanDoubleText_1()
        {
            _Execute("", "");
            _Execute("1 Hello. 1", "1 Hello. 1");
            _Execute("2 Hello.. 1", "2 Hello. 1");
            _Execute("3 Hello... 3", "3 Hello... 3");
            _Execute("4 Hello.... 2", "4 Hello.. 2");
            _Execute("5 Hello..... 4", "5 Hello.... 4");
            _Execute("6 Hello...... 3", "6 Hello... 3");
            _Execute("7 Hello....... 5", "7 Hello..... 5");
            _Execute("8 Hello........ 4", "8 Hello.... 4");
            _Execute("9 Hello......... 6", "9 Hello...... 6");

            _Execute("Hello, world. - Hello again!", "Hello, world. - Hello again!");
            _Execute("Hello, world. - . - Hello again!", "Hello, world. - Hello again!");
            _Execute("Hello, world.. - .. - Hello again!", "Hello, world. - . - Hello again!");
            _Execute("Hello, world.. - . - Hello again!", "Hello, world. - Hello again!");
            _Execute("Hello, world.-.-Hello again!", "Hello, world.-.-Hello again!");
            _Execute("Hello, world. - . - . - Hello again!", "Hello, world. - Hello again!");
            _Execute("Hello, world. - . - . - . - Hello again!", "Hello, world. - Hello again!");
        }
    }
}
