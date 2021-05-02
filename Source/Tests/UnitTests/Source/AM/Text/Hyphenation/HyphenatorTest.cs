// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Hyphenation;

#nullable enable

namespace UnitTests.AM.Text.Hyphenation
{
    [TestClass]
    public class HyphenatorTest
    {
        protected void _TestHyphenate<T>
            (
                string word,
                string expected
            )
            where T: Hyphenator, new()
        {
            Hyphenator hyphenator = new T();

            var positions = hyphenator.Hyphenate(word);
            var actual = Hyphenator.ShowHyphenated
                (
                    word,
                    positions
                );

            Assert.AreEqual(expected, actual);
        }
    }
}
