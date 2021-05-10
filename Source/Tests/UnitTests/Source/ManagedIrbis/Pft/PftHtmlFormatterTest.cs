// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftHtmlFormatterTest
    {
        [TestMethod]
        public void PftHtmlFormatter_Construction_1()
        {
            var formatter = new PftHtmlFormatter();
            Assert.IsNotNull(formatter.Separator);
            Assert.AreEqual("<%", formatter.Separator.Open);
            Assert.AreEqual("%>", formatter.Separator.Close);
        }

        [TestMethod]
        public void PftHtmlFormatter_Construction_2()
        {
            var context = new PftContext(null);
            var formatter = new PftHtmlFormatter(context);
            Assert.IsNotNull(formatter.Separator);
            Assert.AreEqual("<%", formatter.Separator.Open);
            Assert.AreEqual("%>", formatter.Separator.Close);
            Assert.AreSame(context, formatter.Context);
        }

        [TestMethod]
        public void PftHtmlFormatter_ParseProgram_1()
        {
            var formatter = new PftHtmlFormatter();
            var source = "<body><p> <% 'Hello, world!' %></p></body> ";
            formatter.ParseProgram(source);
            var actual = formatter.Program;

            Assert.IsNotNull(actual);

            var expected = new PftProgram
            {
                Children =
                {
                    new PftVerbatim("<body><p> "),
                    new PftUnconditionalLiteral("Hello, world!"),
                    new PftVerbatim("</p></body> "),
                }
            };

            PftSerializationUtility.VerifyDeserializedProgram(expected, actual!);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftHtmlFormatter_ParseProgram_2()
        {
            var formatter = new PftHtmlFormatter();
            var source = "<body><p> <% 'Hello, world!' ";
            formatter.ParseProgram(source);
        }
    }
}
