// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

#nullable enable

namespace UnitTests.AM.Text.Tokenizer
{
    [TestClass]
    public class StringTokenizerTest
    {
        [TestMethod]
        public void StringTokenizer_Construction()
        {
            const string text = "Hello, world!";
            var tokenizer = new StringTokenizer(text);
            Assert.IsNotNull(tokenizer.Settings);
        }

        [TestMethod]
        public void StringTokenizer_GetAllTokens()
        {
            const string text = "Hello, world!";
            var tokenizer = new StringTokenizer(text);
            var tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(5, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual(",", tokens[1].Value);
            Assert.AreEqual("world", tokens[2].Value);
            Assert.AreEqual("!", tokens[3].Value);
            Assert.AreEqual(TokenKind.EOF, tokens[4].Kind);
        }

        [TestMethod]
        [ExpectedException(typeof(TokenizerException))]
        public void StringTokenizer_ReadNumber_Exception()
        {
            const string text = "Hello, 123EWorld!";
            var tokenizer = new StringTokenizer(text);
            tokenizer.GetAllTokens();
        }

        [TestMethod]
        public void StringTokenizer_NextToken1()
        {
            const string text = "Hello\r\nWorld!";
            var tokenizer = new StringTokenizer(text);
            var tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);
        }

        [TestMethod]
        public void StringTokenizer_NextToken2()
        {
            const string text = "Hello World!";
            var tokenizer = new StringTokenizer(text);
            tokenizer.Settings.IgnoreWhitespace = false;
            var tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(5, tokens.Length);
        }

        [TestMethod]
        public void StringTokenizer_ReadWhitespace()
        {
            const string text = "Hello  World!";
            var tokenizer = new StringTokenizer(text);
            tokenizer.Settings.IgnoreWhitespace = false;
            var tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(5, tokens.Length);
        }

        [TestMethod]
        public void StringTokenizer_ReadString()
        {
            const string text = "Hello\"\\x123\"World!";
            var tokenizer = new StringTokenizer(text);
            var tokens = tokenizer.GetAllTokens();

            // TODO: fix this!

            Assert.AreEqual(3, tokens.Length);
        }

        [TestMethod]
        public void StringTokenizer_ReadChar()
        {
            var tokenizer = new StringTokenizer("");
            tokenizer.ReadChar();
            Assert.AreEqual('\0', tokenizer.ReadChar());
        }

        [TestMethod]
        public void StringTokenizer_GetEnumerator()
        {
            const string text = "Hello, World!";
            IEnumerable tokenizer = new StringTokenizer(text);
            var count = 0;
            foreach (var o in tokenizer)
            {
                count++;
            }
            Assert.AreEqual(5, count);
        }

        [TestMethod]
        public void StringTokenizer_IgnoreEOF()
        {
            const string text = "Hello, world!";
            var tokenizer = new StringTokenizer(text);
            tokenizer.Settings.IgnoreEOF = true;
            var tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual(",", tokens[1].Value);
            Assert.AreEqual("world", tokens[2].Value);
            Assert.AreEqual("!", tokens[3].Value);
        }
    }
}
