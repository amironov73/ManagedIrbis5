// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.ConsoleIO.Tables;
using AM.Text;

#endregion

#nullable enable

namespace UnitTests.AM.ConsoleIO.Tables;

[TestClass]
public sealed class TableTest
{
    private sealed class Person
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    [TestMethod]
    public void Table_ToString_1()
    {
        var table = new Table();
        table.AddColumn ("Name")
            .AddColumn ("Age");

        table.AddRow ("Sharik", 1)
            .AddRow ("Matroskin", 2)
            .AddRow ("Fyodor", 12);

        const string expected =
            "| Name      | Age |\n|-----------|-----|\n| Sharik    | 1   |\n| Matroskin | 2   |\n| Fyodor    | 12  |\n";
        var actual = table.ToString().DosToUnix();

        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void Table_From_1()
    {
        var persons = new Person[]
        {
            new () { Name = "Sharik", Age = 1 },
            new () { Name = "Matroskin", Age = 2 },
            new () { Name = "Fyodor", Age = 12 },
        };

        var table = Table.From (persons);

        const string expected =
            "| Name      | Age |\n|-----------|-----|\n| Sharik    | 1   |\n| Matroskin | 2   |\n| Fyodor    | 12  |\n";
        var actual = table.ToString().DosToUnix();

        Assert.AreEqual (expected, actual);
    }
}
