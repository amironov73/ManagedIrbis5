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
    public class UniforGTest
    {
        private void _G
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
        public void UniforG_GetPart_1()
        {
            _G("G", "");
            _G("G0", "");

            _G("G0sThis is the text", "Thi");
            _G("G1sThis is the text", "s is the text");
            _G("G2sThis is the text", " is the text");
            _G("G3sThis is the text", " the text");
            _G("G4sThis is the text", "This is");
            _G("G5sThis is the text", "This i");

            _G("G0#Hello1goodbye2", "Hello");
            _G("G1#Hello1goodbye2", "1goodbye2");
            _G("G2#Hello1goodbye2", "goodbye2");
            _G("G3#Hello1goodbye2", "Hello1goodbye2");
            _G("G4#Hello1goodbye2", "");
            _G("G5#Hello1goodbye2", "");

            _G("G0$Hello1goodbye2", "");
            _G("G1$Hello1goodbye2", "Hello1goodbye2");
            _G("G2$Hello1goodbye2", "ello1goodbye2");
            _G("G3$Hello1goodbye2", "Hello1goodbye2");
            _G("G4$Hello1goodbye2", "");
            _G("G5$Hello1goodbye2", "");

            _G("G0qThis is the text", "This is the text");
            _G("G1qThis is the text", "This is the text");
            _G("G2qThis is the text", "This is the text");
            _G("G3qThis is the text", "This is the text");
            _G("G4qThis is the text", "This is the text");
            _G("G5qThis is the text", "This is the text");
        }

        [TestMethod]
        public void UniforG_GetPart_2()
        {
            _G("G9sThis is the text", "");
        }
    }
}
