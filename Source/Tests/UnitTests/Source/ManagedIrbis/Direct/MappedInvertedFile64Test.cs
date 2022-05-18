// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct;

[TestClass]
public sealed class MappedInvertedFile64Test
    : Common.CommonUnitTest
{
    private string _GetInvertedFilePath()
    {
        return Path.Combine
            (
                TestDataPath,
                "Irbis64/Datai/IBIS/ibis.ifp"
            );
    }

    private MappedInvertedFile64 _GetInvertedFile()
    {
        return new MappedInvertedFile64 (_GetInvertedFilePath());
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void MappedInvertedFile64_Construction_1()
    {
        var fileName = _GetInvertedFilePath();
        var inverted = _GetInvertedFile();
        Assert.AreEqual (fileName, inverted.FileName);
        Assert.IsNotNull (inverted.IfpControlRecord);
        inverted.Dispose();
    }

    [TestMethod]
    [Description ("Чтение обычной ноды")]
    public void MappedInvertedFile64_ReadNode_1()
    {
        using var inverted = _GetInvertedFile();
        var node = inverted.ReadNode (1);
        Assert.IsNotNull (node);
    }

    [TestMethod]
    [Description ("Чтение листовой ноды")]
    public void MappedInvertedFile64_ReadLeaf_1()
    {
        using var inverted = _GetInvertedFile();
        var node = inverted.ReadLeaf (1);
        Assert.IsNotNull (node);
    }

    [TestMethod]
    [Description ("Чтение следующей ноды")]
    public void MappedInvertedFile64_ReadNext_1()
    {
        using var inverted = _GetInvertedFile();
        var node = inverted.ReadNode (1);
        Assert.IsNotNull (node);
        var next = inverted.ReadNext (node);
        Assert.IsNotNull (next);
    }

    [TestMethod]
    [Description ("Чтение следующей нода")]
    public void MappedInvertedFile64_ReadNext_2()
    {
        using var inverted = _GetInvertedFile();
        var node = inverted.ReadNode (1);
        Assert.IsNotNull (node);
        var next = node;
        do
        {
            next = inverted.ReadNext (next);
        }
        while (!ReferenceEquals (next, null));

        Assert.IsNull (next);
    }

    [TestMethod]
    [Description ("Чтение предыдущей ноды")]
    public void MappedInvertedFile64_ReadPrevious_1()
    {
        using var inverted = _GetInvertedFile();
        var node = inverted.ReadNode (1);
        Assert.IsNotNull (node);

        var next = inverted.ReadNext (node);
        Assert.IsNotNull (next);

        var previous = inverted.ReadPrevious (next);
        Assert.IsNotNull (previous);
        Assert.AreEqual (node.Leader.Number, previous.Leader.Number);
    }

    [TestMethod]
    [Description ("Чтение предыдущей ноды: нет предыдущей")]
    public void MappedInvertedFile64_ReadPrevious_2()
    {
        using var inverted = _GetInvertedFile();
        var node = inverted.ReadNode (1);
        Assert.IsNotNull (node);

        var previous = inverted.ReadPrevious (node);
        Assert.IsNull (previous);
    }

    [TestMethod]
    [Description ("Чтение терминов, начиная с указанного")]
    public void MappedInvertedFile64_ReadTerms_1()
    {
        using var inverted = _GetInvertedFile();
        var parameters = new TermParameters
        {
            StartTerm = "K=",
            NumberOfTerms = 10
        };
        var terms = inverted.ReadTerms (parameters);
        Assert.AreEqual (10, terms.Length);
    }

    [TestMethod]
    [Description ("Поиск точных совпадений с термином: есть совпадения")]
    public void MappedInvertedFile64_SearchExact_1()
    {
        using var inverted = _GetInvertedFile();
        var links = inverted.SearchExact ("K=CASE");
        Assert.AreEqual (2, links.Length);
    }

    [TestMethod]
    [Description ("Поиск точных совпадений с термином: нет совпадений")]
    public void MappedInvertedFile64_SearchExact_2()
    {
        using var inverted = _GetInvertedFile();
        var links = inverted.SearchExact ("K=CAS0");
        Assert.AreEqual (0, links.Length);
    }

    [TestMethod]
    [Description ("Поиск по началу термина")]
    public void MappedInvertedFile64_SearchStart_1()
    {
        using var inverted = _GetInvertedFile();
        var links = inverted.SearchStart ("K=C");
        Assert.AreEqual (35, links.Length);
    }

    [TestMethod]
    [Description ("Простой поиск: есть точное совпадение")]
    public void MappedInvertedFile64_SearchSimple_1()
    {
        using var inverted = _GetInvertedFile();
        var found = inverted.SearchSimple ("K=CASE");
        Assert.AreEqual (2, found.Length);
    }

    [TestMethod]
    [Description ("Простой поиск: нет точного совпадения")]
    public void MappedInvertedFile64_SearchSimple_2()
    {
        using var inverted = _GetInvertedFile();
        var found = inverted.SearchSimple ("K=CAS0");
        Assert.AreEqual (0, found.Length);
    }

    [TestMethod]
    [Description ("Простой поиск: с усечением")]
    public void MappedInvertedFile64_SearchSimple_3()
    {
        using var inverted = _GetInvertedFile();
        var found = inverted.SearchSimple ("K=C$");
        Assert.AreEqual (19, found.Length);
    }
}
