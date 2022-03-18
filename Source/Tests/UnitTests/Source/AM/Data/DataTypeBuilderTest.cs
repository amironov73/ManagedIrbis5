// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.Reflection;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Data;

#nullable enable

namespace UnitTests.AM.Data;

[TestClass]
public sealed class DataTypeBuilderTest
{
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
    public void DataTypeBuilder_GenerateAssembly_1()
    {
        var database = _Kladovka();
        var builder = new DataTypeBuilder();
        var assembly = builder.GenerateAssembly (database);

        var types = assembly.GetTypes();
        Assert.AreEqual (2, types.Length);

        var readers = assembly.GetType ("AM.Data.Generated.Readers", true)
            .ThrowIfNull();
        var books = assembly.GetType ("AM.Data.Generated.Books", true)
            .ThrowIfNull();

        var reader = Activator.CreateInstance (readers).ThrowIfNull();
        Assert.AreEqual ("AM.Data.Generated.Readers", reader.ToString());

        var book = Activator.CreateInstance (books).ThrowIfNull();
        Assert.AreEqual ("AM.Data.Generated.Books", book.ToString());

        var readerProperties = readers.GetProperties (BindingFlags.Instance | BindingFlags.Public);
        Assert.AreEqual (3, readerProperties.Length);

        var ticket = readers.GetProperty ("Ticket", BindingFlags.Instance | BindingFlags.Public)
            .ThrowIfNull();
        ticket.SetValue (reader, "123");
        Assert.AreEqual ("123", ticket.GetValue (reader));

        var bookProperties = books.GetProperties (BindingFlags.Instance | BindingFlags.Public);
        Assert.AreEqual (9, bookProperties.Length);

        var id = books.GetProperty ("Id", BindingFlags.Instance | BindingFlags.Public)
            .ThrowIfNull();
        id.SetValue (book, 123);
        Assert.AreEqual (123, id.GetValue (book));
    }
}
