// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

#nullable enable

namespace UnitTests.AM.Text.Tokenizer
{
    [TestClass]
    public class TokenStreamTest
    {
        private TokenStream _GetStream()
        {
            Token[] result =
            {
                new (TokenKind.Word, "Hello", 1, 1),
                new (TokenKind.Word, "Irbis", 2, 1),
                new (TokenKind.Word, "World", 3, 1),
            };

            return new TokenStream(result);
        }

        [TestMethod]
        public void TestTokenStream_Constructor()
        {
            var stream = new TokenStream
                (
                    new [] {"Hello", "Irbis", "World"}
                );
            stream.MoveNext();
            stream.MoveNext();
            stream.MoveNext();
            Assert.AreEqual(3, stream.Position);
        }

        [TestMethod]
        public void TestTokenStream_MoveNext()
        {
            var stream = _GetStream();
            Assert.AreEqual(0, stream.Position);
            var token = stream.Current;
            Assert.IsNotNull(token);
            Assert.AreEqual("Hello", token!.Value);
            Assert.IsTrue(stream.HasNext);
            Assert.IsTrue(stream.MoveNext());
            Assert.AreEqual(1, stream.Position);
            token = stream.Current;
            Assert.IsNotNull(token);
            Assert.AreEqual("Irbis", token!.Value);
            Assert.IsTrue(stream.HasNext);
            Assert.IsTrue(stream.MoveNext());
            Assert.AreEqual(2, stream.Position);
            token = stream.Current;
            Assert.IsNotNull(token);
            Assert.AreEqual("World", token!.Value);
            Assert.IsFalse(stream.HasNext, "stream.HasNext");
            Assert.IsFalse(stream.MoveNext(), "stream.MoveNext()");
            Assert.AreEqual(3, stream.Position);
            token = stream.Current;
            Assert.IsNull(token);
            Assert.IsFalse(stream.HasNext, "stream.HasNext");
            Assert.IsFalse(stream.MoveNext(), "stream.MoveNext()");
            Assert.AreEqual(3, stream.Position);
            token = stream.Current;
            Assert.IsNull(token);
        }

        [TestMethod]
        public void TestTokenStream_Peek()
        {
            var stream = _GetStream();
            string? text = stream.Peek();
            Assert.AreEqual("Irbis", text);
            text = stream.Peek();
            Assert.AreEqual("Irbis", text);
            stream.MoveNext();
            text = stream.Peek();
            Assert.AreEqual("World", text);
            stream.MoveNext();
            text = stream.Peek();
            Assert.AreEqual(null, text);
        }
    }
}
