// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Searching;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Search;

[TestClass]
public sealed class AuthorUtilityTest
{
    [TestMethod]
    [Description ("Пустая строка")]
    public void AuthorUtility_WithComma_1()
    {
        Assert.IsNull (AuthorUtility.WithComma (null));
        Assert.AreEqual (string.Empty, AuthorUtility.WithComma (string.Empty));
    }

    [TestMethod]
    [Description ("Только фамилия")]
    public void AuthorUtility_WithComma_2()
    {
        Assert.AreEqual ("Иванов", AuthorUtility.WithComma ("Иванов"));
    }

    [TestMethod]
    [Description ("Фамилия, имя")]
    public void AuthorUtility_WithComma_3()
    {
        Assert.AreEqual ("Иванов, Иван", AuthorUtility.WithComma ("Иванов Иван"));
        Assert.AreEqual ("Иванов, Иван", AuthorUtility.WithComma ("Иванов, Иван"));
    }

    [TestMethod]
    [Description ("Фамилия, имя, отчество")]
    public void AuthorUtility_WithComma_4()
    {
        Assert.AreEqual ("Иванов, Иван Иванович", AuthorUtility.WithComma ("Иванов Иван Иванович"));
        Assert.AreEqual ("Иванов, Иван Иванович", AuthorUtility.WithComma ("Иванов, Иван Иванович"));
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void AuthorUtility_WithoutComma_1()
    {
        Assert.IsNull (AuthorUtility.WithoutComma (null));
        Assert.AreEqual (string.Empty, AuthorUtility.WithoutComma (string.Empty));
    }

    [TestMethod]
    [Description ("Только фамилия")]
    public void AuthorUtility_WithoutComma_2()
    {
        Assert.AreEqual ("Иванов", AuthorUtility.WithoutComma ("Иванов"));
    }

    [TestMethod]
    [Description ("Фамилия, имя")]
    public void AuthorUtility_WithoutComma_3()
    {
        Assert.AreEqual ("Иванов Иван", AuthorUtility.WithoutComma ("Иванов Иван"));
        Assert.AreEqual ("Иванов Иван", AuthorUtility.WithoutComma ("Иванов, Иван"));
    }

    [TestMethod]
    [Description ("Фамилия, имя, отчество")]
    public void AuthorUtility_WithoutComma_4()
    {
        Assert.AreEqual ("Иванов Иван Иванович", AuthorUtility.WithoutComma ("Иванов Иван Иванович"));
        Assert.AreEqual ("Иванов Иван Иванович", AuthorUtility.WithoutComma ("Иванов, Иван Иванович"));
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void AuthorUtility_WithAndWithoutComma_1()
    {
        Assert.IsNull (AuthorUtility.WithAndWithoutComma (null));
        Assert.IsNull (AuthorUtility.WithAndWithoutComma (string.Empty));
    }

    [TestMethod]
    [Description ("Только фамилия")]
    public void AuthorUtility_WithAndWithoutComma_2()
    {
        CollectionAssert.AreEqual
            (
                new[] { "Иванов" },
                AuthorUtility.WithAndWithoutComma ("Иванов")
            );
    }

    [TestMethod]
    [Description ("Фамилия, имя")]
    public void AuthorUtility_WithAndWithoutComma_3()
    {
        CollectionAssert.AreEqual
            (
                new[] { "Иванов, Иван", "Иванов Иван" },
                AuthorUtility.WithAndWithoutComma ("Иванов Иван")
            );
        CollectionAssert.AreEqual
            (
                new[] { "Иванов, Иван", "Иванов Иван" },
                AuthorUtility.WithAndWithoutComma ("Иванов, Иван")
            );
    }

    [TestMethod]
    [Description ("Фамилия, имя, отчество")]
    public void AuthorUtility_WithAndWithoutComma_4()
    {
        CollectionAssert.AreEqual
            (
                new[] { "Иванов, Иван Иванович", "Иванов Иван Иванович" },
                AuthorUtility.WithAndWithoutComma ("Иванов Иван Иванович")
            );
        CollectionAssert.AreEqual
            (
                new[] { "Иванов, Иван Иванович", "Иванов Иван Иванович" },
                AuthorUtility.WithAndWithoutComma ("Иванов, Иван Иванович")
            );
    }
}
