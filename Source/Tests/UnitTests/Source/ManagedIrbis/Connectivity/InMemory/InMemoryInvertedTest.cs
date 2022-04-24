// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory;

[TestClass]
public sealed class InMemoryInvertedTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void InMemoryInverted_Construction_1()
    {
        var inverted = new InMemoryInverted();
        Assert.AreEqual (0, inverted.Count);
    }

    [TestMethod]
    [Description ("Дамп словаря")]
    public void InMemoryInverted_Dump_1()
    {
        var inverted = new InMemoryInverted();
        var output = new StringWriter();
        inverted.Dump (output);
        var dump = output.ToString();
        Assert.IsNotNull (dump);
    }

    [TestMethod]
    [Description ("Загрузка из потока")]
    public void InMemoryInverted_Read_1()
    {
        var inverted = new InMemoryInverted();
        using var reader = new BinaryReader (Stream.Null);
        inverted.Read (reader);
    }

    [TestMethod]
    [Description ("Сохранение в поток")]
    public void InMemoryInverted_Write_1()
    {
        var inverted = new InMemoryInverted();
        using var memory = new MemoryStream();
        using var writer = new BinaryWriter(memory);
        inverted.Save (writer);
    }
}