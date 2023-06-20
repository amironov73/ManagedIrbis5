// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Data;
using AM.Text;

#endregion

#nullable enable

namespace UnitTests.AM.Data;

[TestClass]
public sealed class SharpSourcerTest
{
    private void _Database
        (
            DatabaseDescriptor database,
            string expected
        )
    {
        var scripter = new SharpSourcer();
        var output = new StringWriter();
        scripter.GenerateDatabase (output, database);
        var actual = output.ToString().DosToUnix();
        Assert.AreEqual (expected, actual);
    }

    private TableDescriptor _ReaderTable()
    {
        return new TableDescriptor()
        {
            Name = "readers",
            Fields = new FieldDescriptor[]
            {
                new()
                {
                    Name = "ticket",
                    Type = DataType.Text,
                    Length = 50,
                    PrimaryKey = true
                },

                new()
                {
                    Name = "name",
                    Type = DataType.Text,
                    Length = 50,
                    Indexed = true
                },

                new()
                {
                    Name = "category",
                    Type = DataType.Text,
                    Length = 10,
                    Indexed = true
                }
            }
        };
    }

    private DatabaseDescriptor _Kladovka()
    {
        return new DatabaseDescriptor
        {
            Name = "kladovka",
            Tables = new []
            {
                _BookTable(),
                _ReaderTable()
            }
        };
    }

    private TableDescriptor _BookTable()
    {
        return new TableDescriptor
        {
            Name = "books",
            Fields = new FieldDescriptor[]
            {
                new()
                {
                    Name = "id",
                    Type = DataType.Integer,
                    Identity = true,
                    PrimaryKey = true
                },

                new()
                {
                    Name = "catalog",
                    Type = DataType.Text,
                    Length = 50,
                    Indexed = true
                },

                new()
                {
                    Name = "number",
                    Type = DataType.Text,
                    Length = 50,
                    Unique = true
                },

                new()
                {
                    Name = "card",
                    Type = DataType.Text,
                    Length = 50
                },

                new()
                {
                    Name = "moment",
                    Type = DataType.Date,
                    Indexed = true
                },

                new()
                {
                    Name = "deadline",
                    Type = DataType.Date,
                    Length = 4
                },

                new()
                {
                    Name = "prolong",
                    Type = DataType.Integer
                },

                new()
                {
                    Name = "pilot",
                    Type = DataType.Boolean
                },

                new()
                {
                    Name = "price",
                    Type = DataType.Money
                }
            }
        };
    }

    [TestMethod]
    public void SharpSourcer_GenerateDatabase_1()
    {
        var db = _Kladovka();
        var expected = "namespace AM.Data.Generated\n{\n    using System;\n\n    public sealed class Books\n    {\n        public int Id { get; set; }\n        public string Catalog { get; set; }\n        public string Number { get; set; }\n        public string Card { get; set; }\n        public DateTime Moment { get; set; }\n        public DateTime Deadline { get; set; }\n        public int Prolong { get; set; }\n        public bool Pilot { get; set; }\n        public decimal Price { get; set; }\n    }\n\n    public sealed class Readers\n    {\n        public string Ticket { get; set; }\n        public string Name { get; set; }\n        public string Category { get; set; }\n    }\n}\n";
        _Database (db, expected);
    }

}
