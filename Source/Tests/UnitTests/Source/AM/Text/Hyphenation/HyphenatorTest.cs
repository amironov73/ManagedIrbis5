// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Hyphenation;

#endregion

#nullable enable

namespace UnitTests.AM.Text.Hyphenation;

[TestClass]
public class HyphenatorTest
{
    protected void _TestHyphenate<T>
        (
            string word,
            string expected
        )
        where T : Hyphenator, new()
    {
        Hyphenator hyphenator = new T();

        var positions = hyphenator.Hyphenate (word);
        var actual = Hyphenator.ShowHyphenated
            (
                word,
                positions
            );

        Assert.AreEqual (expected, actual);
    }
}
