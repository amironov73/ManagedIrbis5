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
public sealed class BiblioFilterTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void BiblioFilter_Construction_1()
    {
        var filter = new BiblioFilter();
        Assert.IsNull(filter.FormatExpression);
        Assert.IsNull(filter.SelectExpression);
        Assert.IsNull(filter.SortExpression);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void BiblioFilter_Verify_1()
    {
        var filter = new BiblioFilter();
        Assert.IsTrue (filter.Verify (false));
    }

}
