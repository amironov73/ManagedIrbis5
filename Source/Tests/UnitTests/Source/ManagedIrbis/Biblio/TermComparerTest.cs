// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio;

[TestClass]
public sealed class TermComparerTest
{
    [TestMethod]
    [Description ("Сравнение термов как чисел")]
    public void TermComparer_Numeric_1()
    {
        var term1 = new BiblioTerm { Title = "Begin11" };
        var term2 = new BiblioTerm { Title = "Begin2" };
        var comparer = new TermComparer.Numeric();
        Assert.IsTrue (comparer.Compare (term1, term2) > 0);
    }

    [TestMethod]
    [Description ("Сравнение термов как обычных строк")]
    public void TermComparer_Trivial_1()
    {
        var term1 = new BiblioTerm { Title = "Begin" };
        var term2 = new BiblioTerm { Title = "End" };
        var comparer = new TermComparer.Trivial();
        Assert.IsTrue (comparer.Compare (term1, term2) < 0);
    }
}
